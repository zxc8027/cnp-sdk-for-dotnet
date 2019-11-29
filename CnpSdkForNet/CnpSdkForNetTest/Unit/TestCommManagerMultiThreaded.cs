/*
 * Zachary Cook
 *
 * Tests CommManager with as multi-threaded.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;

namespace Cnp.Sdk.Test.Unit
{
    [TestFixture]
    public class TestCommManagerMultiThreaded
    {
        List<Thread> testPool = new List<Thread>();

        public const int THREAD_COUNT = 100;
        public const int CYCLE_COUNT = 1000;
        public ConfigManager config;
        
        /*
         * Sets up the test.
         */
        [OneTimeSetUp]
        public void Setup() {
            EnvironmentVariableTestFlags.RequirePerformanceTestsEnabled();
            
            CommManager.Reset();
            config = new ConfigManager(new Dictionary<string, string>
                {
                    {"multiSite", "true"},
                    {"printMultiSiteDebug", "false"},
                    { "url", Properties.Settings.Default.url }
            });
        }
        
        /*
         * Class for a thread for the performance test.
         */
        private class PerformanceTest
        {
            private long threadId;
            private long requestCount = 0;
            private int cycleCount;
            private ConfigManager config;

            /*
             * Creates a performance test object.
             */
            public PerformanceTest(long idNumber,int numCycles,ConfigManager config)
            {
                this.threadId = idNumber;
                this.config = config;
                this.cycleCount = numCycles;
            }

            /*
             * Runs the performance test.
             */
            public void RunPerformanceTest()
            {
                var rand = new Random();
                var startTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                var totalTransactionTime = 0;

                // Perform the cycles.
                for (var n = 0; n < this.cycleCount; n++)
                {
                    // Create a request target.
                    this.requestCount++;
                    var target = CommManager.GetInstance(this.config).FindUrl();
                    
                    // Wait for between 0.1 and 0.6 seconds to simulate a request.
                    try
                    {
                        int sleepTime = 100 + rand.Next(500);
                        totalTransactionTime += sleepTime;
                        Thread.Sleep(sleepTime);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    
                    // Report the result as success.
                    CommManager.GetInstance(this.config).ReportResult(target,CommManager.REQUEST_RESULT_RESPONSE_RECEIVED, 200);
                }
                
                // Output the results.
                var duration = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond - startTime;
                Console.WriteLine("Thread " + threadId + " completed. Total Requests:" + requestCount + "  Elapsed Time:" + (duration / 1000) + " secs    Average Txn Time:" + (totalTransactionTime / requestCount) + " ms");
            }
        }
        
        /*
         * Tests the CommManager as multi-threaded.
         */
        [Test]
        public void TestMultiThreaded()
        {
            try {
                // Create the threads.
                for (var x = 0; x < THREAD_COUNT; x++)
                {
                    var performanceTest = new PerformanceTest(1000 + x,CYCLE_COUNT,config);
                    var threadDelegate = new ThreadStart(performanceTest.RunPerformanceTest);
                    var thread = new Thread(threadDelegate);
                    this.testPool.Add(thread);
                }
            } catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            
            // Perform the test.
            this.PerformTest();
        }
        
        /*
         * Performs the test.
         */
        public void PerformTest()
        {
            // Start the threads.
            foreach (var thread in this.testPool)
            {
                thread.Start();
            }

            // WAit for the threads to finish.
            var allDone = false;
            while (!allDone)
            {
                var doneCount = 0;
                foreach (var thread in this.testPool)
                {
                    if (thread.IsAlive == false)
                    {
                        doneCount++;
                    }
                }
                if (doneCount == this.testPool.Count())
                {
                    allDone = true;
                }
                else
                {
                    try
                    {
                        Thread.Sleep(1000);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
            }
            
            // Output that the tests have completed.
            Console.WriteLine("All test threads have completed");
        }
    }
}