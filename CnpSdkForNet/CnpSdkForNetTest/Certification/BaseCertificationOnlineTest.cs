/*
 * Zachary Cook
 * 
 * Base class for running a transaction.
 */

using System.Collections.Generic;
using NUnit.Framework;
 
namespace Cnp.Sdk.Test.Certification
{
    [TestFixture]
    public class BaseCertificationOnlineTest
    {
        private CnpOnline cnpOnline;
         
        [OneTimeSetUp]
        public void Setup() {
            // Skip the test if online certification tests aren't enabled.
            EnvironmentVariableTestFlags.RequirePreliveOnlineTestsEnabled();
            
            // Create the configuration.
            var config = new Dictionary<string, string>();
            config.Add("url","https://payments.vantivprelive.com/vap/communicator/online");
            config.Add("timeout","20000");
            config.Add("printxml","true");
            config.Add("logFile", null);
            config.Add("neuterAccountNums", null);
            config.Add("proxyHost","");
            config.Add("proxyPort","");
            config.Add("version",CnpVersion.CurrentCNPXMLVersion);
            
            // Create the CNP Online.
            this.cnpOnline = new CnpOnline(new ConfigManager(config));
        }
        
        /*
         * Performs a CNP Online transaction.
         */
        public T SendTransaction<T>(cnpTransactionInterface transaction)
        {
            return this.cnpOnline.SendTransaction<T>(transaction);
        }
    }
}