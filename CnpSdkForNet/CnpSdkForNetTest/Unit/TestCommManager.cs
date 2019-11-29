/*
 * Zachary Cook
 *
 * Tests the CommManager class.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Cnp.Sdk.Test.Unit
{
    class TestCommManager
    {
        public const string site1Url = "https://multisite1.com";
        public const string site2Url = "https://multisite2.com";
        public const string legacyUrl = "https://legacy.com";

        /*
         * Tests GetInstance with a legacy URL.
         */
        [Test]
        public void TestGetInstanceLegacy()
        {
            // Set up a comm manager.
            var config = new Dictionary<string, string>
            {
                {"multiSite", "false"},
                {"printMultiSiteDebug", "true"},
                {"url",legacyUrl }
            };
            CommManager.Reset();
	        var commManger = CommManager.GetInstance(new ConfigManager(config));
            
            // Assert the comm manager was created correctly.
            Assert.IsNotNull(commManger);
            Assert.IsFalse(commManger.GetMultiSite());
            Assert.AreEqual(legacyUrl, commManger.GetLegacyUrl());
            
            // Create a second comm manger.
            var config2 = new Dictionary<string, string>
            {
                {"multiSite", "false"},
                {"printMultiSiteDebug", "true"},
                {"url", "https://nowhere.com" }
            };
            
            // Assert the singleton instance is the same.
            var commManger2 = CommManager.GetInstance(new ConfigManager(config2));
            Assert.AreEqual(legacyUrl, commManger2.GetLegacyUrl()); 
        }
        
        /*
         * Tests GetInstance with a multi-site setup.
         */
        [Test]
        public void TestGetInstanceMultiSite()
        {
            // Create a config and comm manager.
            var config = new Dictionary<string, string>
            {
                {"multiSite", "true"},
                {"printMultiSiteDebug", "true"},
                {"url",legacyUrl },
                {"multiSiteUrl1", site1Url},
                {"multiSiteUrl2", site2Url },
                {"multiSiteErrorThreshold", "4"},
                {"maxHoursWithoutSwitch", "48"}
            };
            CommManager.Reset();
            var commManger = CommManager.GetInstance(new ConfigManager(config));
            
            // Assert the comm manager was created correctly.
            Assert.IsNotNull(commManger);
            Assert.IsTrue(commManger.GetMultiSite());
            Assert.AreEqual(commManger.GetMultiSiteThreshold(),4);
            Assert.AreEqual(commManger.GetMultiSiteUrls().Count(),2);
        }

        /*
         * Tests GetInstance with a multi-site setup with no URLs.
         */
        [Test]
        public void TestGetInstanceMultiSiteNoUrls()
        {
            // Create a config and comm manager.
            var config = new Dictionary<string, string>
            {
                {"multiSite", "true"},
                {"printMultiSiteDebug", "true"},
                {"url",legacyUrl },
                {"multiSiteUrl1", ""},
                {"multiSiteUrl2", ""},
            };
            CommManager.Reset();
            var commManger = CommManager.GetInstance(new ConfigManager(config));
            
            // Assert the comm manager was created correctly.
            Assert.IsNotNull(commManger);
            Assert.IsFalse(commManger.GetMultiSite());
        }

        /*
         * Tests GetInstance with a multi-site setup with out of range properties.
         */
        [Test]
        public void TestGetInstanceMultiSiteOutOfRangeProperties()
        {
            // Create a config and comm manager.
            var config = new Dictionary<string, string>
            {
                {"multiSite", "true"},
                {"printMultiSiteDebug", "true"},
                {"url",legacyUrl },
                {"multiSiteUrl1", site1Url},
                {"multiSiteUrl2", site2Url },
                {"multiSiteErrorThreshold", "102"},
                {"maxHoursWithoutSwitch", "500"}

            };
            CommManager.Reset();
            var commManager = CommManager.GetInstance(new ConfigManager(config));
            
            // Assert the comm manager was created correctly.
            Assert.IsNotNull(commManager);
            Assert.IsTrue(commManager.GetMultiSite());
            Assert.AreEqual(5, commManager.GetMultiSiteThreshold());
            Assert.AreEqual(48, commManager.GetMaxHoursWithoutSwitch());
        }
        
        /*
         * Tests FindUrl with a legacy URL.
         */
        [Test]
        public void TestFindUrlLegacy()
        {
            // Create a config and comm manager.
            var config = new Dictionary<string, string>
            {
                {"multiSite", "false"},
                {"printMultiSiteDebug", "true"},
                {"url",legacyUrl }
            };
            CommManager.Reset();
            var commManager = CommManager.GetInstance(new ConfigManager(config));
            
            // Assert the request target is correct.
            var requestTarget = commManager.FindUrl();
            Assert.AreEqual(legacyUrl,requestTarget.GetUrl());
        }
        
        /*
         * Tests FindUrl with multi-site URL.
         */
        [Test]
        public void TestFindUrlMultiSite1()
        { 
            // Create a config and comm manager.
            var config = new Dictionary<string, string>
            {
                {"multiSite", "true"},
                {"printMultiSiteDebug", "true"},
                {"url",legacyUrl },
                {"multiSiteUrl1", site1Url},
                {"multiSiteUrl2", site2Url },
                {"multiSiteErrorThreshold", "4"},
                {"maxHoursWithoutSwitch", "48"}
            };
            CommManager.Reset();
            var commManager = CommManager.GetInstance(new ConfigManager(config));
            
            // Assert the request target is correct.
            var requestTarget = commManager.FindUrl();
            Assert.AreEqual(commManager.GetMultiSiteUrls()[commManager.GetCurrentMultiSiteUrlIndex()],requestTarget.GetUrl());
            Assert.True(requestTarget.GetUrl().Equals(site1Url) || requestTarget.GetUrl().Equals(site2Url));
        }

        /*
         * Tests FindUrl with multi-site URL with switching.
         */
        [Test]
        public void TestFindUrlMultiSiteSwitching()
        {
            // Create a config and comm manager.
           var config = new Dictionary<string, string>
            {
                {"multiSite", "true"},
                {"printMultiSiteDebug", "false"},
                {"url",legacyUrl },
                {"multiSiteUrl1", site1Url},
                {"multiSiteUrl2", site2Url },
                {"multiSiteErrorThreshold", "3"},
                {"maxHoursWithoutSwitch", "48"}
            };
            CommManager.Reset();
            var commManager = CommManager.GetInstance(new ConfigManager(config));

            // Create request targets and report them as timing out.
            var requestTarget1 = commManager.FindUrl();
            Assert.AreEqual(commManager.GetMultiSiteUrls()[commManager.GetCurrentMultiSiteUrlIndex()], requestTarget1.GetUrl());
            commManager.ReportResult(requestTarget1,CommManager.REQUEST_RESULT_RESPONSE_TIMEOUT, 0);
            var requestTarget2 = commManager.FindUrl();
            Assert.AreEqual(requestTarget1.GetUrl(), requestTarget2.GetUrl());
            commManager.ReportResult(requestTarget2, CommManager.REQUEST_RESULT_RESPONSE_TIMEOUT, 0);
            var requestTarget3 = commManager.FindUrl();
            Assert.AreEqual(requestTarget1.GetUrl(), requestTarget3.GetUrl());
            commManager.ReportResult(requestTarget3, CommManager.REQUEST_RESULT_RESPONSE_TIMEOUT, 0);
            Assert.AreEqual(commManager.GetErrorCount(), 3);

            // Assert the URL changed.
            var requestTarget4 = commManager.FindUrl();
            Assert.IsFalse(requestTarget4.GetUrl().Equals(requestTarget1.GetUrl()));
            
            // Create request targets and report them as timing out.
            var requestTarget5 = commManager.FindUrl();
            Assert.AreEqual(commManager.GetMultiSiteUrls()[commManager.GetCurrentMultiSiteUrlIndex()], requestTarget5.GetUrl());
            commManager.ReportResult(requestTarget5,CommManager.REQUEST_RESULT_RESPONSE_TIMEOUT, 0);
            var requestTarget6 = commManager.FindUrl();
            Assert.AreEqual(requestTarget5.GetUrl(), requestTarget6.GetUrl());
            commManager.ReportResult(requestTarget6, CommManager.REQUEST_RESULT_RESPONSE_TIMEOUT, 0);
            var requestTarget7 = commManager.FindUrl();
            Assert.AreEqual(requestTarget5.GetUrl(), requestTarget7.GetUrl());
            commManager.ReportResult(requestTarget7, CommManager.REQUEST_RESULT_RESPONSE_TIMEOUT, 0);
            Assert.AreEqual(commManager.GetErrorCount(), 3);

            // Assert the URL changed.
            var requestTarget8 = commManager.FindUrl();
            Assert.IsFalse(requestTarget8.GetUrl().Equals(requestTarget4.GetUrl()));
            Assert.IsTrue(requestTarget8.GetUrl().Equals(requestTarget1.GetUrl()));
        }
        
        /*
         * Tests FindUrl with multi-site URL with successes resetting the count.
         */
        [Test]
        public void TestFindUrlMultiSiteSwitchingErrorCountResetting()
        {
            // Create a config and comm manager.
            var config = new Dictionary<string, string>
            {
                {"multiSite", "true"},
                {"printMultiSiteDebug", "false"},
                {"url", legacyUrl},
                {"multiSiteUrl1", site1Url},
                {"multiSiteUrl2", site2Url},
                {"multiSiteErrorThreshold", "3"},
                {"maxHoursWithoutSwitch", "48"}
            };
            CommManager.Reset();
            var commManager = CommManager.GetInstance(new ConfigManager(config));
            
            // Send an error then success message and assert the error count is 0.
            var requestTarget1 = commManager.FindUrl();
            Assert.AreEqual(commManager.GetMultiSiteUrls()[commManager.GetCurrentMultiSiteUrlIndex()], requestTarget1.GetUrl());
            commManager.ReportResult(requestTarget1, CommManager.REQUEST_RESULT_RESPONSE_TIMEOUT, 0);
            var requestTarget2 = commManager.FindUrl();
            Assert.AreEqual(requestTarget1.GetUrl(), requestTarget2.GetUrl());
            commManager.ReportResult(requestTarget2, CommManager.REQUEST_RESULT_RESPONSE_RECEIVED, 200);
            Assert.AreEqual(0, commManager.GetErrorCount());

            // Assert another success doesn't increment the error count.
            var requestTarget3 = commManager.FindUrl();
            Assert.AreEqual(requestTarget1.GetUrl(), requestTarget3.GetUrl());
            commManager.ReportResult(requestTarget3, CommManager.REQUEST_RESULT_RESPONSE_RECEIVED, 301);
            Assert.AreEqual(0, commManager.GetErrorCount());
        }
        
        /*
         * Tests FindUrl with multi-site URL with the max-hours being exceeded.
         */
        [Test]
        public void TestFindUrlMultiSiteMaxHoursExceeded()
        {
            // Create a config and comm manager.
            var config = new Dictionary<string, string>
            {
                {"multiSite", "true"},
                {"printMultiSiteDebug", "false"},
                {"url", legacyUrl},
                {"multiSiteUrl1", site1Url},
                {"multiSiteUrl2", site2Url},
                {"multiSiteErrorThreshold", "3"},
                {"maxHoursWithoutSwitch", "48"}
            };
            CommManager.Reset();
            var commManager = CommManager.GetInstance(new ConfigManager(config));

            // Get a base target.
            var requestTarget1 = commManager.FindUrl();
            Assert.AreEqual(commManager.GetMultiSiteUrls()[commManager.GetCurrentMultiSiteUrlIndex()], requestTarget1.GetUrl());
            
            // Set the last switch time beyond the limit.
            var newLastTime = new DateTime(commManager.GetLastSiteSwitchTime() * 10000);
            newLastTime = newLastTime.Add(new TimeSpan(-49,0,0));
            commManager.SetLastSiteSwitchTime(newLastTime.Ticks/10000);

            // Assert the URL changed.
            var requestTarget2 = commManager.FindUrl();
            Assert.IsFalse(requestTarget2.GetUrl().Equals(requestTarget1.GetUrl()));
        }

        /*
         * Tests ReportResult without multi-site.
         */
        [Test]
        public void TestReportResultNotMultiSite()
        {
            // Create a config and comm manager.
            var config = new Dictionary<string, string>
            {
                {"multiSite", "false"},
                {"printMultiSiteDebug", "true"},
                {"url",legacyUrl }
            };
            CommManager.Reset();
            var commManager = CommManager.GetInstance(new ConfigManager(config));
            
            // Report a result.
            commManager.ReportResult(new RequestTarget("",1),  1,  0);
        }
    }
}