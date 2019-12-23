/*
 * Zachary Cook
 *
 * Tests the AuthReversal requests using Sandbox.
 */

using NUnit.Framework;

namespace Cnp.Sdk.Test.Functional
{
    [TestFixture]
    public class TestAuthReversal
    {
        [Test]
        public void TestSimpleAuthReversal()
        {
            var transaction = new authReversal()
            {
                id = "1",
                litleTxnId = 12345678000L,
                cnpTxnId = 12345678000L,
                amount = 106,
                payPalNotes = "Notes"
            };

            var test = new BaseCnpOnlineTest();
            test.SetExpectedPopulated("id",transaction.id,null,null);
            test.SetExpectedUnpopulated("customerId",null,null);
            test.SetExpectedPopulated("cnpTxnId","12.0",null);
            test.SetExpectedPopulated("response","000",null,"10.0");
            test.SetExpectedPopulated("response","001","10.0","10.8");
            test.SetExpectedPopulated("response","000","11.0",null);
            test.SetExpectedPopulated("responseTime",null,null);
            test.SetExpectedPopulated("message","Approved",null,"10.0");
            test.SetExpectedPopulated("message","Transaction Received","10.0","10.8");
            test.SetExpectedPopulated("message","Approved","11.0",null);
            test.SetExpectedPopulated("litleTxnId",null,"12.0");
            test.RunCnpTestThreaded<authReversalResponse>(transaction);
        }
        
        [Test]
        public void TestAuthReversalHandleSpecialCharacters()
        {
            var transaction = new authReversal()
            {
                id = "1",
                litleTxnId = 12345678000L,
                cnpTxnId = 12345678000L,
                amount = 106,
                payPalNotes = "<'&\">"
            };

            var test = new BaseCnpOnlineTest();
            test.SetExpectedPopulated("id",transaction.id,null,null);
            test.SetExpectedUnpopulated("customerId",null,null);
            test.SetExpectedPopulated("cnpTxnId","12.0",null);
            test.SetExpectedPopulated("response","000",null,"10.0");
            test.SetExpectedPopulated("response","001","10.0","10.8");
            test.SetExpectedPopulated("response","000","11.0",null);
            test.SetExpectedPopulated("responseTime",null,null);
            test.SetExpectedPopulated("message","Approved",null,"10.0");
            test.SetExpectedPopulated("message","Transaction Received","10.0","10.8");
            test.SetExpectedPopulated("message","Approved","11.0",null);
            test.SetExpectedPopulated("litleTxnId",null,"12.0");
            test.RunCnpTestThreaded<authReversalResponse>(transaction);
        }
    }
}