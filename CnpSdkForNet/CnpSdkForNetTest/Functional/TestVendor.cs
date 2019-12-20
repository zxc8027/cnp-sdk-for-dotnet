/*
 * Zachary Cook
 *
 * Tests the VendorDebit and VendorCredit requests
 * using Sandbox.
 */

using Cnp.Sdk.VersionedXML;
using NUnit.Framework;

namespace Cnp.Sdk.Test.Functional
{
    [TestFixture]
    public class TestVendor
    {
        [Test]
        public void VendorCredit()
        {
            var transaction = new vendorCredit()
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
                vendorName = "WorldPay",
            };

            var test = new BaseCnpOnlineTest();
            test.SetExceptionExpected(typeof(InvalidVersionException),null,"9.2");
            test.SetExpectedPopulated("id",transaction.id,null,null);
            test.SetExpectedUnpopulated("customerId",null,null);
            test.SetExpectedPopulated("cnpTxnId","12.0",null);
            test.SetExpectedPopulated("fundsTransferId",transaction.fundsTransferId,"9.2",null);
            test.SetExpectedPopulated("response","000","9.2",null);
            test.SetExpectedPopulated("responseTime","9.2",null);
            test.SetExpectedPopulated("message","Approved","9.2",null);
            test.SetExpectedPopulated("litleTxnId","9.2","12.0");
            test.RunCnpTestThreaded<vendorCreditResponse>(transaction);
        }
        
        [Test]
        public void VendorCreditWithFundingCustomerId()
        {
            var transaction = new vendorCredit()
            {
                id = "1",
                accountInfo = new echeckType()
                {
                    accType = echeckAccountTypeEnum.Savings,
                    accNum = "1234",
                    routingNum = "12345678"
                },
                amount = 1500,
                fundingCustomerId = "value for fundingCustomerId",
                fundsTransferId = "value for fundsTransferId",
                vendorName = "WorldPay"
            };

            var test = new BaseCnpOnlineTest();
            test.SetExceptionExpected(typeof(InvalidVersionException),null,"9.2");
            test.SetExceptionExpected(typeof(CnpOnlineException),"9.2","12.9");
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
        public void VendorDebit()
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
        public void VendorDebitWithFundingCustomerId()
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
                fundingCustomerId = "value for fundingCustomerId",
                fundsTransferId = "value for fundsTransferId",
                vendorName = "WorldPay"
            };

            var test = new BaseCnpOnlineTest();
            test.SetExceptionExpected(typeof(InvalidVersionException),null,"9.2");
            test.SetExceptionExpected(typeof(CnpOnlineException),"9.2","12.9");
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
    }
}