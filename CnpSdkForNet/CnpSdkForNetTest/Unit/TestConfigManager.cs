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
            Assert.AreEqual(config.getConfig()["url"],Properties.Settings.Default.url);
            Assert.AreEqual(config.getConfig()["timeout"],Properties.Settings.Default.timeout);
            Assert.AreEqual(config.getConfig()["responseDirectory"],Properties.Settings.Default.responseDirectory);
            Assert.AreEqual(config.GetVersion(),XMLVersion.FromString(CnpVersion.CurrentCNPXMLVersion));
        }
        
        /*
         * Tests the overrides constructor.
         */
        [Test]
        public void TestOverridesonstructor()
        {
            // Create the config.
            var configDictionary = new Dictionary<string,string>();
            configDictionary["url"] = "testUrl";
            configDictionary["version"] = "12.4";
            var config = new ConfigManager(configDictionary);
            
            // Assert some values are correct.
            Assert.AreEqual(config.getConfig()["url"],"testUrl");
            Assert.AreEqual(config.getConfig()["timeout"],Properties.Settings.Default.timeout);
            Assert.AreEqual(config.getConfig()["responseDirectory"],Properties.Settings.Default.responseDirectory);
            Assert.AreEqual(config.GetVersion(),new XMLVersion(12,4));
        }
    }
}