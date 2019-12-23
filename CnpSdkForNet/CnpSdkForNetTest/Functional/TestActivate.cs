/*
 * Zachary Cook
 *
 * Tests the Activate and VirtualGiftCard requests
 * using Sandbox.
 */

using Cnp.Sdk.VersionedXML;
using NUnit.Framework;

namespace Cnp.Sdk.Test.Functional
{
    [TestFixture]
    public class TestActivate
    {
        [Test]
        public void TestActivateSimple()
        {
            var transaction = new activate()
            {
                id = "1",
                orderId = "12344",
                amount = 1500,
                orderSource = orderSourceType.ecommerce,
                card = new giftCardCardType
                {
                    type = methodOfPaymentTypeEnum.GC,
                    number = "414100000000000000",
                    cardValidationNum = "123",
                    expDate = "1215"
                }
            };

            var test = new BaseCnpOnlineTest();
            test.SetExceptionExpected(typeof(InvalidVersionException),null,"8.21");
            test.SetExpectedPopulated("id",transaction.id,null,null);
            test.SetExpectedPopulated("cnpTxnId","12.0",null);
            test.SetExpectedUnpopulated("customerId",null,null);
            test.SetExpectedPopulated("response","000","8.21",null);
            test.SetExpectedPopulated("responseTime","8.21",null);
            test.SetExpectedUnpopulated("giftCardResponse",null,"11.0");
            test.SetExpectedPopulated("giftCardResponse","11.0",null);
            test.SetExpectedPopulated("message","Approved","8.21",null);
            test.SetExpectedPopulated("litleTxnId","8.21","12.0");
            test.RunCnpTestThreaded<activateResponse>(transaction);
        }
        
        [Test]
        public void TestVirtualGiftCardActivate()
        {
            var transaction = new activate()
            {
                id = "1",
                orderId = "12344",
                amount = 1500,
                orderSource = orderSourceType.ecommerce,
                virtualGiftCard = new virtualGiftCardType
                {
                    accountNumberLength = 123,
                    giftCardBin = "123"
                }
            };

            var test = new BaseCnpOnlineTest();
            test.SetExceptionExpected(typeof(InvalidVersionException),null,"8.21");
            test.SetExceptionExpected(typeof(CnpOnlineException),"8.21","8.22");
            test.SetExpectedPopulated("id",transaction.id,null,null);
            test.SetExpectedPopulated("cnpTxnId","12.0",null);
            test.SetExpectedUnpopulated("customerId",null,null);
            test.SetExpectedPopulated("response","000","8.21",null);
            test.SetExpectedPopulated("responseTime","8.21",null);
            test.SetExpectedUnpopulated("giftCardResponse",null,"11.0");
            test.SetExpectedPopulated("giftCardResponse","11.0",null);
            test.SetExpectedPopulated("message","Approved","8.21",null);
            test.SetExpectedPopulated("litleTxnId","8.21","12.0");
            test.RunCnpTestThreaded<activateResponse>(transaction);
        }
    }
}