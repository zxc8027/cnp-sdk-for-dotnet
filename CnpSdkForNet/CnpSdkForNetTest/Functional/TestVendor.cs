/*
 * Zachary Cook
 *
 * Tests the VendorDebit and VendorCredit requests
 * using Sandbox.
 */

using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;

namespace Cnp.Sdk.Test.Functional
{
    [TestFixture]
    public class TestVendor
    {
        private CnpOnline cnp;
        private Dictionary<string, string> config;

        [OneTimeSetUp]
        public void SetUpCnp()
        {
            CommManager.reset();
            config = new Dictionary<string, string>
            {
                {"url", Properties.Settings.Default.url},
                {"reportGroup", "Default Report Group"},
                {"username", "DOTNET"},
                {"version", CnpVersion.CurrentCNPXMLVersion},
                {"timeout", "5000"},
                {"merchantId", "101"},
                {"password", "TESTCASE"},
                {"printxml", "true"},
                {"proxyHost", Properties.Settings.Default.proxyHost},
                {"proxyPort", Properties.Settings.Default.proxyPort},
                {"logFile", Properties.Settings.Default.logFile},
                {"neuterAccountNums", "true"}
            };

            cnp = new CnpOnline(config);
        }
        [Test]
        public void ReserveDebit()
        {
            var vendorDebit = new vendorDebit
            {
                // attributes.
                id = "1",
                reportGroup = "Default Report Group",
                // required child elements.
                accountInfo = new echeckType()
                {
                    accType = echeckAccountTypeEnum.Savings,
                    accNum = "1234",
                    routingNum = "12345678"
                },
                amount = 1500,
                fundingSubmerchantId = "value for fundingSubmerchantId",
                fundsTransferId = "value for fundsTransferId",
                vendorName = "WorldPay"
            };

            var response = cnp.VendorDebit(vendorDebit);
            Assert.AreEqual("000", response.response);
            
            
            Assembly asm = typeof(transactionRequest).Assembly;
            Type type = asm.GetType("Cnp.Sdk.vendorDebit");
            Console.WriteLine("TYPE: " + type);
        }
    }
}