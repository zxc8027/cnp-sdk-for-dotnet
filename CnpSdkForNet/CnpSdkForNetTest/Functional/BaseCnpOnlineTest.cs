/*
 * Zachary Cook
 *
 * Base for running CNP online tests.
 */

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cnp.Sdk.VersionedXML;
using NUnit.Framework;

namespace Cnp.Sdk.Test.Functional
{
    /*
     * Class for a thread pool for testing transactions transaction.
     */
    public class CnpOnlineRequestThreadPool<T>
    {
        private List<Task> activeTasks = new List<Task>();
        private BaseCnpOnlineTest test;
        private cnpTransactionInterface request;
        private List<XMLVersion> versions;
        private int maxThreads;
        
        /*
         * Creates a thread pool object.
         */
        public CnpOnlineRequestThreadPool(BaseCnpOnlineTest test,cnpTransactionInterface request,List<XMLVersion> versions,int maxThreads)
        {
            this.test = test;
            this.request = request;
            this.versions = versions;
            this.maxThreads = maxThreads;
        }
        
        /*
         * Performs a test for a specific version.
         */
        public async Task PerformTest(XMLVersion version)
        {
            Console.WriteLine("Running transaction test for " + version);
            await Task.Run(() => test.RunCnpTest<T>(this.request,version));
        }
        
        /*
         * Starts the threads. If the limit is reached,
         * the thread yields until one opens.
         */
        public void StartThreads()
        {
            foreach (var version in this.versions)
            {
                // If the thread count is hit, wait for a spot to open.
                if (this.activeTasks.Count >= this.maxThreads)
                {
                    Task.WaitAny(this.activeTasks.ToArray());
                    
                    // Remove the completed tasks.
                    foreach (var endedTask in this.activeTasks.ToArray())
                    {
                        if (endedTask.IsCompleted)
                        {
                            this.activeTasks.Remove(endedTask);
                        }
                    }
                }
                
                // Create and add the thread.
                var task = this.PerformTest(version);
                this.activeTasks.Add(task);
            }
        }
        
        /*
         * Waits for the active threads to finish.
         */
        public void WaitForAllThreads()
        {
            Task.WaitAll(this.activeTasks.ToArray());
        }
        
        /*
         * Starts the pool and waits for it to finish.
         */
        public void RunTestPool()
        {
            this.StartThreads();
            this.WaitForAllThreads();
        }
    }

    public class BaseCnpOnlineTest
    {
        // Limit to the amount of threads that can be run.
        // Be aware each thread makes 2 requests.
        // More threads may slow down the process (host throttling requests?).
        public const int MAX_REQUEST_THREADS = 10;
        
        // Limits for the version to test. Does [MajorVersion.0 to MajorVersion.Minorversion]
        public static List<XMLVersion> MAX_MINOR_VERSIONS = new List<XMLVersion>()
        {
            new XMLVersion(8,31),
            new XMLVersion(9,14),
            new XMLVersion(10,8),
            new XMLVersion(11,4),
            XMLVersion.FromString(CnpVersion.CurrentCNPXMLVersion),
        };
        
        // Versions to not test.
        public static List<XMLVersion> EXCLUDED_VERSIONS = new List<XMLVersion>()
        {
            new XMLVersion(12, 6), // 12.6 release doesn't exist
        };

        public XMLVersion StartingVersion = new XMLVersion(8,0);
        public XMLVersion EndingVersion = XMLVersion.FromString(CnpVersion.CurrentCNPXMLVersion);
        private Dictionary<XMLVersionInformation,Type> expectedExceptions = new Dictionary<XMLVersionInformation,Type>();
        private Dictionary<XMLAttribute,bool> expectedPopulated = new Dictionary<XMLAttribute,bool>();
        private Dictionary<XMLAttribute,object> expectedPopulatedObjects = new Dictionary<XMLAttribute,object>();
        
        /*
         * Returns a list of all of the versions to run.
         */
        public List<XMLVersion> GetVersionsToRun()
        {
            // Create the list.
            var versions = new List<XMLVersion>();
            
            // Add the versions.
            foreach (var maxMinorVersion in MAX_MINOR_VERSIONS)
            {
                for (var i = 0; i <= maxMinorVersion.SubVersion; i++)
                {
                    var version = new XMLVersion(maxMinorVersion.MainVersion,i);
                    if (version >= this.StartingVersion && version <= this.EndingVersion && !EXCLUDED_VERSIONS.Contains(version))
                    {
                        versions.Add(version);
                    }
                }
            }
            
            // Return the list.
            return versions;
        }
        
        /*
         * Validates a response object.
         */
        private void ValidateResponse<T>(object response,XMLVersion version)
        {
            // Go through the properties.
            var responseType = typeof(T);
            foreach (var member in responseType.GetMembers())
            {
                var property = responseType.GetProperty(member.Name);
                if (property != null)
                {
                    var name = member.Name;
                    var value = property.GetValue(response,null);
                    
                    // Validate the property.
                    foreach (var expectedPopulatedRange in this.expectedPopulated.Keys)
                    {
                        if (expectedPopulatedRange.IsVersionValid(version) && expectedPopulatedRange.Name == name)
                        {
                            var valueExpectedPopulated = this.expectedPopulated[expectedPopulatedRange];
                            var valueDefined = (value != null);
                            if (valueExpectedPopulated && valueDefined)
                            {
                                if (this.expectedPopulatedObjects.ContainsKey(expectedPopulatedRange))
                                {
                                    var expectedValue = this.expectedPopulatedObjects[expectedPopulatedRange];
                                    if (!expectedValue.Equals(value))
                                    {
                                        Assert.Fail(name + " is expected to be populated with:\n" + expectedValue + "\n\nbut is populated with:\n" + value);
                                    }
                                }
                            }
                            else if (valueExpectedPopulated)
                            {
                                Assert.Fail(name + " is expected to be populated in " + responseType.Name + " but isn't.");
                            }
                            else if (valueDefined)
                            {
                                Assert.Fail(name + " is expected to be unpopulated in " + responseType.Name + " but is with:\n" + value);
                            }
                        }
                    }
                }
            }
        }
        
        /*
         * Validates an exception is correct.
         */
        public void ValidateException(Exception exception, XMLVersion version)
        {
            // Find the expected exception and throw an error if the type is incorrect.
            var exceptionExpected = false;
            foreach (var range in this.expectedExceptions.Keys)
            {
                if (range.IsVersionValid(version))
                {
                    var actualExceptionType = exception.GetType();
                    var expectedExceptionType = this.expectedExceptions[range];
                    if (VersionedXMLDeserializer.IsOfType(actualExceptionType,expectedExceptionType))
                    {
                        exceptionExpected = true;
                    }
                    else
                    {
                        Assert.Fail("Exception does not match.\n\tExpected exception: " + expectedExceptionType.Name + "\n\tActual exception: " + actualExceptionType.Name + "\n\n" + exception);
                    }
                }
            }
                
            // Fail the test for an unexpected exception.
            if (!exceptionExpected)
            {
                Assert.Fail("Unexpected exception:\n" + exception);
            }
        }
        
        
        /*
         * Sets a range where an exception is expected.
         * The start and invalid versions can be null.
         */
        public void SetExceptionExpected(Type exceptionClass,string versionStart,string firstInvalidVersion)
        {
            expectedExceptions.Add(new XMLVersionInformation() { FirstVersion = versionStart,RemovedVersion = firstInvalidVersion},exceptionClass);
        }
        
        /*
         * Sets a range where a property is expected to be unpopulated.
         * The start and invalid versions can be null.
         */
        public void SetExpectedUnpopulated(string name,string versionStart,string firstInvalidVersion)
        {
            expectedPopulated.Add(new XMLAttribute() { Name = name,FirstVersion = versionStart,RemovedVersion = firstInvalidVersion},false);
        }
        
        /*
         * Sets a range where a property is expected to be populated.
         * The start and invalid versions can be null.
         */
        public void SetExpectedPopulated(string name,string versionStart,string firstInvalidVersion)
        {
            expectedPopulated.Add(new XMLAttribute() { Name = name,FirstVersion = versionStart,RemovedVersion = firstInvalidVersion},true);
        }
        
        /*
         * Sets a range where a property is expected to be populated with a specific object.
         * The start and invalid versions can be null.
         */
        public void SetExpectedPopulated(string name,object populatedObject,string versionStart,string firstInvalidVersion)
        {
            var range = new XMLAttribute() {Name = name, FirstVersion = versionStart, RemovedVersion = firstInvalidVersion};
            expectedPopulated.Add(range,true);
            expectedPopulatedObjects.Add(range,populatedObject);
        }
        
        /*
         * Runs a test transaction for a specific version.
         */
        public void RunCnpTest<T>(cnpTransactionInterface transaction,XMLVersion version)
        {
            // Create the configuration.
            var config = new ConfigManager(new Dictionary<string,string>
            {
                {"reportGroup", "Default Report Group"},
                {"username", "DOTNET"},
                {"timeout", "15000"},
                {"merchantId", "101"},
                {"password", "TESTCASE"},
                {"printxml", "true"},
                {"neuterAccountNums", "true"},
                {"version", version.ToString()}
            });

            // Create a CNP Online object.
            var cnpOnline = new CnpOnline(config);
            
            // Perform the sync transaction, and validate the response.
            try
            {
                var transactionResponse = cnpOnline.SendTransaction<T>(transaction);
                this.ValidateResponse<T>(transactionResponse, version);
            }
            catch (Exception exception)
            {
                this.ValidateException(exception,version);
            }
            
            
            // Perform the async transaction, and validate the response.
            try
            {
                var transactionResponse = cnpOnline.SendTransactionAsync<T>(transaction,new CancellationToken()).Result;
                this.ValidateResponse<T>(transactionResponse,version);
            }
            catch (AggregateException exception)
            {
                this.ValidateException(exception.InnerException,version);
            }
        }
        
        /*
         * Runs a test transaction for all versions.
         */
        public void RunCnpTest<T>(cnpTransactionInterface transaction)
        {
            foreach (var version in this.GetVersionsToRun())
            {
                Console.WriteLine("Running transaction test for " + version);
                this.RunCnpTest<T>(transaction,version);
            }
        }
        
        /*
         * Runs a test transaction for all versions with threading.
         */
        public void RunCnpTestThreaded<T>(cnpTransactionInterface transaction)
        {
            var pool = new CnpOnlineRequestThreadPool<T>(this,transaction,this.GetVersionsToRun(),MAX_REQUEST_THREADS);
            pool.RunTestPool();
        }
    }
}