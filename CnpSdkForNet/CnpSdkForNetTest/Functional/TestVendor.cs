/*
 * Zachary Cook
 *
 * Tests the VendorDebit and VendorCredit requests
 * using Sandbox.
 */

using System.Collections.Generic;
using System.Threading;
using Cnp.Sdk.VersionedXML;
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
            config = new Dictionary<string, string>
            {
                {"reportGroup", "Default Report Group"},
                {"username", "DOTNET"},
                {"timeout", "5000"},
                {"merchantId", "101"},
                {"password", "TESTCASE"},
                {"printxml", "true"},
                {"neuterAccountNums", "true"},
                {"version", CnpVersion.CurrentCNPXMLVersion}
            };

            cnp = new CnpOnline(config);
        }
        [Test]
        public void ReserveDebit()
        {
            var transaction = new vendorDebit
            {
                id = "1",
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

            var test = new BaseCnpOnlineTest();
            test.GetVersionsToRun();
            test.SetExceptionExpected(typeof(InvalidVersionException),null,"9.2");
            test.SetExpectedPopulated("id",transaction.id,null,null);
            test.SetExpectedUnpopulated("customerId",null,null);
            test.SetExpectedPopulated("cnpTxnId","12.0",null);
            test.SetExpectedPopulated("fundsTransferId",transaction.fundsTransferId,"9.2",null);
            test.SetExpectedPopulated("response","000","9.2",null);
            test.SetExpectedPopulated("responseTime","9.2",null);
            test.SetExpectedPopulated("message","Approved","9.2",null);
            test.SetExpectedPopulated("litleTxnId","9.2","12.0");
            test.RunCnpTestThreaded<vendorDebitResponse>(transaction);
        }
        
        [Test]
        public void ReserveDebitWithFundingCustomerId()
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
                fundsTransferId = "value for fundsTransferId",
                fundingCustomerId = "value for fundingCustomerId",
                vendorName = "WorldPay"
            };

            var response = cnp.VendorDebit(vendorDebit);
            Assert.AreEqual("000", response.response);
        }

        [Test]
        public void TestReserveDebitAsync()
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

            CancellationToken cancellationToken = new CancellationToken(false);
            var response = cnp.VendorDebitAsync(vendorDebit, cancellationToken);
            Assert.AreEqual("000", response.Result.response);
        }
    }
}