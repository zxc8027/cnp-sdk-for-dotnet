/*
 * Zachary Cook
 *
 * Tests the CnpBatchRequest class.
 * Due to time constraints, the tests are limited.
 */

using NUnit.Framework;

namespace Cnp.Sdk.Test.Unit
{
    [TestFixture]
    public class TestCnpBatchRequest
    {
        /*
         * Tests the AddTransaction method.
         */
        [Test]
        public void TestAddTransaction()
        {
            // Create 2 requests and add them to a batch request.
            var request = new CnpBatchRequest();
            request.AddTransaction(new vendorCredit() { amount = 50, });
            request.AddTransaction(new vendorCredit() { amount = 60, });
            request.AddTransaction(new vendorCredit() { });
            request.AddTransaction(new echeckRedeposit());
            
            // Assert the request was made correctly.
            var config = new ConfigManager();
            var batchRequest = request.GetBatchRequest();
            Assert.AreEqual(batchRequest.transaction.Count,4);
            Assert.AreEqual(batchRequest.numVendorCredit,3);
            Assert.AreEqual(batchRequest.vendorCreditAmount,110);
            Assert.AreEqual(batchRequest.numEcheckRedeposit,1);
            
            // Assert the sub-requests are correct.
            var subRequest1 = (vendorCredit) batchRequest.transaction[0];
            var subRequest2 = (vendorCredit) batchRequest.transaction[1];
            var subRequest3 = (vendorCredit) batchRequest.transaction[2];
            var subRequest4 = (echeckRedeposit) batchRequest.transaction[3];
            Assert.AreEqual(subRequest1.reportGroup,config.GetValue("reportGroup"));
            Assert.AreEqual(subRequest2.reportGroup,config.GetValue("reportGroup"));
            Assert.AreEqual(subRequest3.reportGroup,config.GetValue("reportGroup"));
            Assert.AreEqual(subRequest4.reportGroup,config.GetValue("reportGroup"));
        }
    }
}