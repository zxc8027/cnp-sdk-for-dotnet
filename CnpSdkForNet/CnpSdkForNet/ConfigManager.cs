/*
 * Zachary Cook
 *
 * Creates configurations and stores information.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Cnp.Sdk.VersionedXML;

namespace Cnp.Sdk
{
    public class ConfigManager
    {
        private Dictionary<string,string> config;

        /*
         * Creates a default configuration.
         */
        public ConfigManager()
        {
            // Create the base configuration.
            config = new Dictionary<string, string>();
            config["url"] = Properties.Settings.Default.url;
            config["reportGroup"] = Properties.Settings.Default.reportGroup;
            config["username"] = Properties.Settings.Default.username;
            config["printxml"] = Properties.Settings.Default.printxml;
            config["timeout"] = Properties.Settings.Default.timeout;
            config["proxyHost"] = Properties.Settings.Default.proxyHost;
            config["merchantId"] = Properties.Settings.Default.merchantId;
            config["password"] = Properties.Settings.Default.password;
            config["proxyPort"] = Properties.Settings.Default.proxyPort;
            config["logFile"] = Properties.Settings.Default.logFile;
            config["neuterAccountNums"] = Properties.Settings.Default.neuterAccountNums;
            config["multiSite"] = Properties.Settings.Default.multiSite;
            config["printMultiSiteDebug"] = Properties.Settings.Default.printMultiSiteDebug;
            config["multiSiteUrl1"] = Properties.Settings.Default.multiSiteUrl1;
            config["multiSiteUrl2"] = Properties.Settings.Default.multiSiteUrl2;
            config["multiSiteErrorThreshold"] = Properties.Settings.Default.multiSiteErrorThreshold;
            config["maxHoursWithoutSwitch"] = Properties.Settings.Default.maxHoursWithoutSwitch;
            config["sftpUrl"] = Properties.Settings.Default.sftpUrl;
            config["sftpUsername"] = Properties.Settings.Default.sftpUsername;
            config["sftpPassword"] = Properties.Settings.Default.sftpPassword;
            config["onlineBatchUrl"] = Properties.Settings.Default.onlineBatchUrl;
            config["onlineBatchPort"] = Properties.Settings.Default.onlineBatchPort;
            config["requestDirectory"] = Properties.Settings.Default.requestDirectory;
            config["responseDirectory"] = Properties.Settings.Default.responseDirectory;
            config["useEncryption"] = Properties.Settings.Default.useEncryption;
            config["vantivPublicKeyId"] = Properties.Settings.Default.vantivPublicKeyId;
            config["pgpPassphrase"] = Properties.Settings.Default.pgpPassphrase;
            config["neuterUserCredentials"] = Properties.Settings.Default.neuterUserCredentials;
            config["version"] = Properties.Settings.Default.version;
        }

        /*
         * Creates a configuration with a set of overrides.
         */
        public ConfigManager(Dictionary<string,string> config) : this()
        {
            // Set the overrides.
            foreach (var key in config.Keys)
            {
                if (this.config.ContainsKey(key))
                {
                    this.config[key] = config[key];
                }
                else
                {
                    Console.WriteLine("Config value " + key + " is unknown; ignoring.");
                }
            }
        }
        
        /*
         * Returns the configuration as a dictionary.
         */
        [Obsolete("Deprecated in favor of ConfigManager.GetValue")]
        public Dictionary<string,string> getConfig()
        {
            return this.config;
        }
        
        /*
         * Returns the value in the config for a given key.
         */
        public string GetValue(string key)
        {
            return this.config[key];
        }
        
        /*
         * Returns the XML version to use.
         */
        public XMLVersion GetVersion()
        {
            // Return the default if the version is not set.
            if (config["version"] == "")
            {
                Console.WriteLine("Version isn't defined in the configuration. It is recommended" +
                                  "to set it so the SDK version can be upgraded without needing" +
                                  "to change your XML version. Assuming XML version" + CnpVersion.CurrentCNPXMLVersion);

                return XMLVersion.FromString(CnpVersion.CurrentCNPXMLVersion);
            }
            
            // Return the version from the string.
            return XMLVersion.FromString(config["version"]);
        }
        
        /*
         * Returns if two configs are equal.
         */
        public override bool Equals(object obj)
        {
            // Return false if the type is incorrect.
            if (!(obj is ConfigManager)) return false;
            
            // Compare the strings.
            var dictionary1 = this.config;
            var dictionary2 = ((ConfigManager) obj).config;
            return dictionary1.Count == dictionary2.Count && !dictionary1.Except(dictionary2).Any();
        }
        
        /*
         * Returns if two config managers are equal.
         */
        public static bool operator ==(ConfigManager config1,ConfigManager config2)
        {
            return (config1 is null && config2 is null) || (!(config1 is null) && config1.Equals(config2));
        }

        /*
         * Returns if two config managers are not equal.
         */
        public static bool operator !=(ConfigManager config1, ConfigManager config2)
        {
            return !(config1 == config1);
        }
    }
}