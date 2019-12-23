/*
 * Zachary Cook
 *
 * Tests the BalanceInquery requests using Sandbox.
 */

using Cnp.Sdk.VersionedXML;
using NUnit.Framework;

namespace Cnp.Sdk.Test.Functional
{
    [TestFixture]
    public class TestBalanceInquiry
    {
        [Test]
        public void SimpleBalanceInquiry()
        {
            var transaction = new balanceInquiry
            {
                id = "1",
                orderId = "12344",
                orderSource = orderSourceType.ecommerce,
                card = new giftCardCardType
                {
                    type = methodOfPaymentTypeEnum.GC,
                    number = "414100000000000000",
                    cardValidationNum = "123",
                    expDate = "1215",
                }
            };
        
            var test = new BaseCnpOnlineTest();
            test.SetExceptionExpected(typeof(InvalidVersionException),null,"8.21");
            test.SetExpectedPopulated("id",transaction.id,null,null);
            test.SetExpectedPopulated("cnpTxnId","12.0",null);
            test.SetExpectedPopulated("response","000","8.21",null);
            test.SetExpectedPopulated("responseTime","8.21",null);
            test.SetExpectedPopulated("message","Approved","8.21",null);
            test.SetExpectedPopulated("balanceInquiryResponse","8.21",null);
            test.SetExpectedPopulated("litleTxnId","8.21","12.0");
            test.RunCnpTestThreaded<balanceInquiryResponse>(transaction);
        }
    }
}