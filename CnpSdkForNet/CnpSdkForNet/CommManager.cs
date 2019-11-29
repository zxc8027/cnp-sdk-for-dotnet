/*
 * Zachary Cook
 *
 * Singleton class for handling multi-site communications.
 */

using System;
using System.Collections.Generic;

namespace Cnp.Sdk
{
    public class RequestTarget
    {
        private String targetUrl;
        private int urlIndex;
        private long requestTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        /*
         * Creates a request target object.
         */
        public RequestTarget(string url, int index)
        {
            this.targetUrl = url;
            this.urlIndex = index;
        }

        /*
         * Returns the URL to use.
         */
        [Obsolete("Deprecated in favor of RequestTarget.GetUrl()")]
        public string getUrl()
        {
            return this.GetUrl();
        }

        /*
         * Returns the time of the request.
         */
        [Obsolete("Deprecated in favor of RequestTarget.GetRequestTime()")]
        public long getRequestTime()
        {
            return this.GetRequestTime();
        }

        /*
         * Returns the URL index.
         */
        [Obsolete("Deprecated in favor of RequestTarget.GetUrlIndex()")]
        public int getUrlIndex()
        {
            return this.GetUrlIndex();
        }

        /*
         * Returns the URL to use.
         */
        public string GetUrl()
        {
            return this.targetUrl;
        }

        /*
         * Returns the time of the request.
         */
        public long GetRequestTime()
        {
            return requestTime;
        }

        /*
         * Returns the URL index.
         */
        public int GetUrlIndex()
        {
            return urlIndex;
        }
    }

    public class CommManager
    {
        public const int REQUEST_RESULT_RESPONSE_RECEIVED = 1;
        public const int REQUEST_RESULT_CONNECTION_FAILED = 2;
        public const int REQUEST_RESULT_RESPONSE_TIMEOUT = 3;
        
        private static readonly object SynLock = new object();
        private static CommManager staticManager = null;
        private static Random randomNumberGenerator = new Random();

        protected ConfigManager config;
        protected Boolean doMultiSite = false;
        protected String legacyUrl;
        protected List<String> multiSiteUrls = new List<String>();
        protected int errorCount = 0;
        protected int currentMultiSiteUrlIndex = 0;
        protected int multiSiteThreshold = 5;
        protected long lastSiteSwitchTime = 0;
        protected int maxHoursWithoutSwitch = 48;
        protected Boolean printDebug = false;

        /*
         * Returns if multi-site is enabled.
         */
        [Obsolete("Deprecated in favor of CommManager.GetMultiSite()")]
        public bool getMultiSite()
        {
            return this.GetMultiSite();
        }
        
        /*
         * Returns the legacy url (non-multi-site).
         */
        [Obsolete("Deprecated in favor of CommManager.GetLegacyUrl()")]
        public string getLegacyUrl()
        {
            return this.GetLegacyUrl();
        }
        
        /*
         * Returns the multi-site urls.
         */
        [Obsolete("Deprecated in favor of CommManager.GetMultiSiteUrls()")]
        public List<string> getMultiSiteUrls()
        {
            return this.GetMultiSiteUrls();
        }
        
        /*
         * Returns the error threshold to change sites.
         */
        [Obsolete("Deprecated in favor of CommManager.GetMultiSiteThreshold()")]
        public int getMultiSiteThreshold()
        {
            return this.GetMultiSiteThreshold();
        }
        
        /*
         * Returns the maximum amount of hours to run
         * without changing the site.
         */
        [Obsolete("Deprecated in favor of CommManager.GetMaxHoursWithoutSwitch()")]
        public int getMaxHoursWithoutSwitch()
        {
            return this.GetMaxHoursWithoutSwitch();
        }
        
        /*
         * Returns the current index in the list of
         * multi-site URLs to use.
         */
        [Obsolete("Deprecated in favor of CommManager.GetCurrentMultiSiteUrlIndex()")]
        public int getCurrentMultiSiteUrlIndex()
        {
            return this.GetCurrentMultiSiteUrlIndex();
        }
        
        /*
         * Returns the current error count.
         */
        [Obsolete("Deprecated in favor of CommManager.GetErrorCount()")]
        public int getErrorCount()
        {
            return this.GetErrorCount();
        }
        
        /*
         * Returns the last time the site was switched.
         */
        [Obsolete("Deprecated in favor of CommManager.GetLastSiteSwitchTime()")]
        public long getLastSiteSwitchTime()
        {
            return this.GetLastSiteSwitchTime();
        }
        
        /*
         * Sets the last time the site was switched.
         */
        [Obsolete("Deprecated in favor of CommManager.SetLastSiteSwitchTime()")]
        public void setLastSiteSwitchTime(long milliseconds)
        {
            this.SetLastSiteSwitchTime(milliseconds);
        }
        
        
        
        
        
        /*
         * Returns if multi-site is enabled.
         */
        public bool GetMultiSite()
        {
            return this.doMultiSite;
        }
        
        /*
         * Returns the legacy url (non-multi-site).
         */
        public string GetLegacyUrl()
        {
            return this.legacyUrl;
        }
        
        /*
         * Returns the multi-site urls.
         */
        public List<string> GetMultiSiteUrls()
        {
            return this.multiSiteUrls;
        }
        
        /*
         * Returns the error threshold to change sites.
         */
        public int GetMultiSiteThreshold()
        {
            return this.multiSiteThreshold;
        }
        
        /*
         * Returns the maximum amount of hours to run
         * without changing the site.
         */
        public int GetMaxHoursWithoutSwitch()
        {
            return this.maxHoursWithoutSwitch;
        }
        
        /*
         * Returns the current index in the list of
         * multi-site URLs to use.
         */
        public int GetCurrentMultiSiteUrlIndex()
        {
            return this.currentMultiSiteUrlIndex;
        }
        
        /*
         * Returns the current error count.
         */
        public int GetErrorCount()
        {
            return this.errorCount;
        }
        
        /*
         * Returns the last time the site was switched.
         */
        public long GetLastSiteSwitchTime()
        {
            return this.lastSiteSwitchTime;
        }
        
        /*
         * Sets the last time the site was switched.
         */
        public void SetLastSiteSwitchTime(long milliseconds)
        {
            this.lastSiteSwitchTime = milliseconds;
        }

        /*
         * Returns the singleton instance of CommManager
         * to use.
         */
        [Obsolete("Deprecated in favor of CommManager.GetInstance()")]
        public static CommManager instance()
        {
            return CommManager.GetInstance();
        }
        
        /*
         * Returns the singleton instance of CommManager
         * to use.
         */
        public static CommManager GetInstance()
        {
            // Create the singleton instance if it is not defined.
            if (staticManager == null)
            {
                staticManager = new CommManager();
            }
            
            // Returns the static instance.
            return staticManager;
        }

        /*
         * Returns the singleton instance of CommManager
         * to use for a custom configuration.
         */
        [Obsolete("Deprecated in favor of CommManager.GetInstance(ConfigManager config)")]
        public static CommManager instance(Dictionary<string,string> config)
        {
            return CommManager.GetInstance(new ConfigManager(config));
        }

        /*
         * Returns the singleton instance of CommManager
         * to use for a custom configuration.
         */
        public static CommManager GetInstance(ConfigManager config)
        {
            // Create the singleton instance if it is not defined.
            if (staticManager == null)
            {
                staticManager = new CommManager(config);
            }
            else if (config != staticManager.config)
            {
                // Display a message if the configuration is different.
                Console.WriteLine("Different configurations for CommManager are detected." +
                                  "Make sure to use CommManager.Reset() if the intent " +
                                  "is to change the CommManager config.");
            }
            
            // Returns the static instance.
            return staticManager;
        }

        /*
         * Resets the static CommManager.
         */
        [Obsolete("Deprecated in favor of CommManager.Reset()")]
        public static void reset()
        {
            CommManager.Reset();
        }

        /*
         * Resets the static CommManager.
         */
        public static void Reset()
        {
            staticManager = null;
        }

        /*
         * Creates a CommManager with the default config.
         */
        private CommManager()
        {
            this.config = new ConfigManager();
            SetupMultiSite();
        }

        /*
         * Creates a CommManager with a custom config.
         */
        private CommManager(ConfigManager config)
        {
            this.config = config;
            SetupMultiSite();
        }

        /*
         * Sets up the url to use.
         */
        private void SetupMultiSite()
        {
            // Set the legacy url and multi-site status.
            this.legacyUrl = this.config.GetValue("url");
            this.doMultiSite = Convert.ToBoolean(this.config.GetValue("multiSite"));

            // If multi-site is enabled, set up the url to use.
            if (doMultiSite)
            {
                // Get the URLs (currently 2).
                for (var x = 1; x < 3; x++)
                {
                    var multiSiteUrl = this.config.GetValue("multiSiteUrl" + x);
                    if (!string.IsNullOrEmpty(multiSiteUrl))
                    {
                        this.multiSiteUrls.Add(multiSiteUrl);
                    }
                }
                
                if (this.multiSiteUrls.Count == 0)
                {
                    // Disable multi-site if no URLs exist.
                    this.doMultiSite = false;
                }
                else
                {
                    // Shuffle the URLs and reset the index and error count.
                    Shuffle(this.multiSiteUrls);
                    this.currentMultiSiteUrlIndex = 0;
                    this.errorCount = 0;
                    
                    // Set the error threshold.
                    if (!string.IsNullOrEmpty(this.config.GetValue("multiSiteErrorThreshold")))
                    {
                        String threshold = this.config.GetValue("multiSiteErrorThreshold");
                        var t = int.Parse(threshold);
                        if (t > 0 && t < 100)
                        {
                            this.multiSiteThreshold = t;
                        }
                        
                    }
                    
                    // Set the maximum hours without switching.
                    if (!string.IsNullOrEmpty(this.config.GetValue("maxHoursWithoutSwitch")))
                    {
                        var maxHours = this.config.GetValue("maxHoursWithoutSwitch");
                        var t = int.Parse(maxHours);
                        if (t >= 0 && t < 300)
                        {
                            this.maxHoursWithoutSwitch = t;
                        }
                    }
                    
                    // Set the last site switch time.
                    this.lastSiteSwitchTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                }
            }
        }

        /*
         * Returns the url to use.
         */
        [Obsolete("Deprecated in favor of CommManager.FindUrl()")]
        public RequestTarget findUrl()
        {
            return this.FindUrl();
        }
        
        /*
         * Returns the url to use.
         */
        public RequestTarget FindUrl()
        {
            var url = this.legacyUrl;
            if (this.doMultiSite)
            {
                lock (SynLock)
                {
                    // Determine if the URL should be switched.
                    var switchSite = false;
                    var switchReason = "";
                    var currentUrl = this.multiSiteUrls[this.currentMultiSiteUrlIndex];
                    if (this.errorCount < this.multiSiteThreshold)
                    {
                        if (this.maxHoursWithoutSwitch > 0)
                        {
                            var diffSinceSwitch = ((DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - this.lastSiteSwitchTime) / 3600000;
                            if (diffSinceSwitch > this.maxHoursWithoutSwitch)
                            {
                                switchReason = " more than " + maxHoursWithoutSwitch + " hours since last switch";
                                switchSite = true;
                            }
                        }
                    }
                    else
                    {
                        switchReason = " consecutive error count has reached threshold of " + multiSiteThreshold;
                        switchSite = true;
                    }

                    // Switch the URL if needed.
                    if (switchSite)
                    {
                        // Cycle the URL.
                        this.currentMultiSiteUrlIndex++;
                        if (this.currentMultiSiteUrlIndex >= this.multiSiteUrls.Count)
                        {
                            this.currentMultiSiteUrlIndex = 0;
                        }
                        
                        url = this.multiSiteUrls[this.currentMultiSiteUrlIndex];
                        this.errorCount = 0;
                        
                        // Output the new URL.
                        if (this.printDebug)
                        {
                            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  Switched to " + url + " because " + switchReason);
                        }
                    }
                    else
                    {
                        // Set the url as the current.
                        url = currentUrl;
                    }
                }
            }
            
            // Output the url to use.
            if (this.printDebug)
            {
                Console.WriteLine("Selected URL: " + url);
            }
            
            // Return the request target.
            return new RequestTarget(url,this.currentMultiSiteUrlIndex);
        }
        
        /*
         * Reports the result of a request.
         */
        [Obsolete("Deprecated in favor of CommManager.ReportResult(RequestTarget target,int result,int statusCode)")]
        public void reportResult(RequestTarget target,int result,int statusCode)
        {
            this.ReportResult(target, result, statusCode);
        }

        public void ReportResult(RequestTarget target,int result,int statusCode)
        {
            lock (SynLock)
            {
                // Return if the last switch was after the request or multi-site is disabled.
                if (target.GetRequestTime() < lastSiteSwitchTime || !doMultiSite)
                {
                    return;
                }
                
                // Update the error count.
                switch (result)
                {
                    case REQUEST_RESULT_RESPONSE_RECEIVED:
                        if (statusCode == 200)
                        {
                            errorCount = 0;
                        }
                        else if (statusCode >= 400)
                        {
                            errorCount++;
                        }
                        break;
                    case REQUEST_RESULT_CONNECTION_FAILED:
                        errorCount++;
                        break;
                    case REQUEST_RESULT_RESPONSE_TIMEOUT:
                        errorCount++;
                        break;
                }
            }
        }

        /*
         * Shuffles a list.
         */
        private static void Shuffle<T> (IList<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = randomNumberGenerator.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}