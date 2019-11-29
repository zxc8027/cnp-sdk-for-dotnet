/*
 * Zachary Cook
 *
 * Tests the ConfigManager class.
 */

using System.Collections.Generic;
using Cnp.Sdk.VersionedXML;
using NUnit.Framework;

namespace Cnp.Sdk.Test.Unit
{
    [TestFixture]
    public class TestConfigManager
    {
        /*
         * Tests the default constructor.
         */
        [Test]
        public void TestDefaultConstructor()
        {
            // Create the config.
            var config = new ConfigManager();
            
            // Assert some values are correct.
            Assert.AreEqual(config.GetValue("url"),Properties.Settings.Default.url);
            Assert.AreEqual(config.GetValue("timeout"),Properties.Settings.Default.timeout);
            Assert.AreEqual(config.GetValue("responseDirectory"),Properties.Settings.Default.responseDirectory);
            Assert.AreEqual(config.GetVersion(),XMLVersion.FromString(CnpVersion.CurrentCNPXMLVersion));
        }
        
        /*
         * Tests the overrides constructor.
         */
        [Test]
        public void TestOverridesConstructor()
        {
            // Create the config.
            var configDictionary = new Dictionary<string,string>();
            configDictionary["url"] = "testUrl";
            configDictionary["version"] = "12.4";
            var config = new ConfigManager(configDictionary);
            
            // Assert some values are correct.
            Assert.AreEqual(config.GetValue("url"),"testUrl");
            Assert.AreEqual(config.GetValue("timeout"),Properties.Settings.Default.timeout);
            Assert.AreEqual(config.GetValue("responseDirectory"),Properties.Settings.Default.responseDirectory);
            Assert.AreEqual(config.GetVersion(),new XMLVersion(12,4));
        }
        
        /*
         * Tests equality.
         */
        [Test]
        public void TestEquality()
        {
            // Create several configs.
            var configDictionary1 = new Dictionary<string,string>();
            configDictionary1["url"] = "testUrl1";
            var configDictionary2 = new Dictionary<string,string>();
            configDictionary2["url"] = "testUrl1";
            var configDictionary3 = new Dictionary<string,string>();
            configDictionary3["url"] = "testUrl2";
            var config1 = new ConfigManager(configDictionary1);
            var config2 = new ConfigManager(configDictionary2);
            var config3 = new ConfigManager(configDictionary3);
            
            // Assert the equality is correct.
            Assert.AreEqual(config1,config2);
            Assert.AreNotEqual(config1,config3);
            Assert.AreNotEqual(config2,config3);
        }
    }
}