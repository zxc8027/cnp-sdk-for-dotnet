/*
 * Zachary Cook
 *
 * Tests the Auth requests using Sandbox.
 */

using System;
using System.Collections.Generic;
using System.Data;
using Cnp.Sdk.VersionedXML;
using NUnit.Framework;

namespace Cnp.Sdk.Test.Functional
{
    [TestFixture]
    public class TestAuth
    {
        [Test]
        public void TestSimpleAuthWithCard()
        {
            
            var transaction = new authorization
            {
                id = "1",
                orderId = "12344",
                amount = 106,
                orderSource = orderSourceType.ecommerce,
                card = new cardType
                {
                    type = methodOfPaymentTypeEnum.VI,
                    number = "414100000000000000",
                    expDate = "1210"
                },
                customBilling = new customBilling {phone = "1112223333"}
            };
            
            var test = new BaseCnpOnlineTest();
            test.SetExpectedPopulated("id",transaction.id,null,null);
            test.SetExpectedPopulated("cnpTxnId","12.0",null);
            test.SetExpectedPopulated("orderId",transaction.orderId,null,null);
            test.SetExpectedPopulated("response","000",null,null);
            test.SetExpectedPopulated("responseTime",null,null);
            test.SetExpectedPopulated("message","Approved",null,null);
            test.SetExpectedPopulated("authCode",null,null);
            test.SetExpectedUnpopulated("networkTransactionId",null,"8.30");
            test.SetExpectedPopulated("networkTransactionId","8.30","9.0");
            test.SetExpectedUnpopulated("networkTransactionId","9.0","9.10");
            test.SetExpectedPopulated("networkTransactionId","9.10","10.0");
            test.SetExpectedUnpopulated("networkTransactionId","10.0","10.5");
            test.SetExpectedPopulated("networkTransactionId","10.5",null);
            test.SetExpectedPopulated("litleTxnId","8.21","12.0");
            test.RunCnpTestThreaded<authorizationResponse>(transaction);
        }
        
        [Test]
        public void TestSimpleAuthWithCardWithOrigTxnIdAndAmount()
        {
            var transaction = new authorization
            {
                id = "2",
                orderId = "5",
                amount = 106,
                orderSource = orderSourceType.ecommerce,
                originalNetworkTransactionId = "123456789012345678901234567890",
                originalTransactionAmount = 2500,
                card = new cardType
                {
                    type = methodOfPaymentTypeEnum.VI,
                    number = "410000000000000000",
                    expDate = "1210"
                },
                customBilling = new customBilling { phone = "1112223333" }
            };
            
            var test = new BaseCnpOnlineTest();
            test.SetExpectedPopulated("id",transaction.id,null,null);
            test.SetExpectedPopulated("cnpTxnId","12.0",null);
            test.SetExpectedPopulated("orderId",transaction.orderId,null,null);
            test.SetExpectedPopulated("response","000",null,null);
            test.SetExpectedPopulated("responseTime",null,null);
            test.SetExpectedPopulated("message","Approved",null,null);
            test.SetExpectedPopulated("authCode",null,null);
            test.SetExpectedUnpopulated("networkTransactionId",null,"8.30");
            test.SetExpectedPopulated("networkTransactionId","8.30","9.0");
            test.SetExpectedUnpopulated("networkTransactionId","9.0","9.10");
            test.SetExpectedPopulated("networkTransactionId","9.10","10.0");
            test.SetExpectedUnpopulated("networkTransactionId","10.0","10.5");
            test.SetExpectedPopulated("networkTransactionId","10.5",null);
            test.SetExpectedPopulated("litleTxnId","8.21","12.0");
            test.RunCnpTestThreaded<authorizationResponse>(transaction);
        }
        
        [Test]
        public void TestSimpleAuthWithMpos()
        {
            var transaction = new authorization
            {
                id = "3",
                orderId = "12344",
                amount = 200,
                orderSource = orderSourceType.ecommerce,
                mpos = new mposType
                {
                    ksn = "77853211300008E00016",
                    encryptedTrack =
                        "CASE1E185EADD6AFE78C9A214B21313DCD836FDD555FBE3A6C48D141FE80AB9172B963265AFF72111895FE415DEDA162CE8CB7AC4D91EDB611A2AB756AA9CB1A000000000000000000000000000000005A7AAF5E8885A9DB88ECD2430C497003F2646619A2382FFF205767492306AC804E8E64E8EA6981DD",
                    formatId = "30",
                    track1Status = 0,
                    track2Status = 0
                }
            };
            
            var test = new BaseCnpOnlineTest();
            test.SetExceptionExpected(typeof(CnpOnlineException),null,"8.25");
            test.SetExpectedPopulated("id",transaction.id,null,null);
            test.SetExpectedPopulated("cnpTxnId","12.0",null);
            test.SetExpectedPopulated("orderId",transaction.orderId,null,null);
            test.SetExpectedPopulated("response","000",null,null);
            test.SetExpectedPopulated("responseTime",null,null);
            test.SetExpectedPopulated("message","Approved",null,null);
            test.SetExpectedPopulated("authCode",null,null);
            test.SetExpectedUnpopulated("networkTransactionId",null,null);
            test.SetExpectedPopulated("litleTxnId","8.21","12.0");
            test.RunCnpTestThreaded<authorizationResponse>(transaction);
        }
        
        [Test]
        public void TestAuthWithAmpersand()
        {
            var transaction = new authorization
            {
                id = "4",
                orderId = "7",
                amount = 10010,
                orderSource = orderSourceType.ecommerce,
                billToAddress = new billToAddress()
                {
                    name = "John & Jane Smith",
                    addressLine1 = "1 Main St.",
                    city = "Burlington",
                    state = "MA",
                    zip = "01803-3747",
                    country = countryTypeEnum.US
                },
                card = new cardType
                {
                    type = methodOfPaymentTypeEnum.VI,
                    number = "4457010000000009",
                    expDate = "0112",
                    cardValidationNum = "349"
                }
            };
            
            var test = new BaseCnpOnlineTest();
            test.SetExpectedPopulated("id",transaction.id,null,null);
            test.SetExpectedPopulated("cnpTxnId","12.0",null);
            test.SetExpectedPopulated("orderId",transaction.orderId,null,null);
            test.SetExpectedPopulated("response","000",null,null);
            test.SetExpectedPopulated("responseTime",null,null);
            test.SetExpectedPopulated("message","Approved",null,null);
            test.SetExpectedPopulated("authCode",null,null);
            test.SetExpectedUnpopulated("networkTransactionId",null,"8.30");
            test.SetExpectedPopulated("networkTransactionId","8.30","9.0");
            test.SetExpectedUnpopulated("networkTransactionId","9.0","9.10");
            test.SetExpectedPopulated("networkTransactionId","9.10","10.0");
            test.SetExpectedUnpopulated("networkTransactionId","10.0","10.5");
            test.SetExpectedPopulated("networkTransactionId","10.5",null);
            test.SetExpectedPopulated("litleTxnId","8.21","12.0");
            test.RunCnpTestThreaded<authorizationResponse>(transaction);
        }
        
        [Test]
        public void TestSimpleAuthWithPaypal()
        {
            var transaction = new authorization
            {
                id = "5",
                orderId = "8",
                amount = 106,
                orderSource = orderSourceType.ecommerce,
                paypal = new paypal
                {
                    payerId = "1234",
                    token = "1234",
                    transactionId = "123456"
                },
                customBilling = new customBilling { phone = "1112223333" }
            };
            
            var test = new BaseCnpOnlineTest();
            test.SetExpectedPopulated("id",transaction.id,null,null);
            test.SetExpectedPopulated("cnpTxnId","12.0",null);
            test.SetExpectedPopulated("orderId",transaction.orderId,null,null);
            test.SetExpectedPopulated("response","000",null,null);
            test.SetExpectedPopulated("responseTime",null,null);
            test.SetExpectedPopulated("message","Approved",null,null);
            test.SetExpectedPopulated("authCode",null,null);
            test.SetExpectedUnpopulated("networkTransactionId",null,null);
            test.SetExpectedPopulated("litleTxnId","8.21","12.0");
            test.RunCnpTestThreaded<authorizationResponse>(transaction);
        }
        
        [Test]
        public void TestSimpleAuthWithAndroidPay()
        {
            var transaction = new authorization
            {
                id = "6",
                orderId = "9",
                amount = 106,
                orderSource = orderSourceType.androidpay,
                card = new cardType
                {
                    type = methodOfPaymentTypeEnum.VI,
                    number = "414100000000000000",
                    expDate = "1210"
                },
                customBilling = new customBilling { phone = "1112223333" }
            };
            
            var test = new BaseCnpOnlineTest();
            test.SetExceptionExpected(typeof(InvalidVersionException),null,"9.8");
            test.SetExceptionExpected(typeof(InvalidVersionException),"10.0","10.2");
            test.SetExceptionExpected(typeof(CnpOnlineException),"10.2","10.6"); // Sandbox fails with Android Pay for these versions.
            test.SetExpectedPopulated("id",transaction.id,null,null);
            test.SetExpectedPopulated("cnpTxnId","12.0",null);
            test.SetExpectedPopulated("orderId",transaction.orderId,null,null);
            test.SetExpectedPopulated("response","000",null,null);
            test.SetExpectedPopulated("responseTime",null,null);
            test.SetExpectedPopulated("message","Approved",null,null);
            test.SetExpectedPopulated("authCode",null,null);
            test.SetExpectedPopulated("androidpayResponse",null,null);
            test.SetExpectedUnpopulated("networkTransactionId",null,null);
            test.SetExpectedPopulated("litleTxnId","8.21","12.0");
            test.RunCnpTestThreaded<authorizationResponse>(transaction);
        }
        
        [Test]
        public void TestSimpleAuthWithApplePayAndSecondaryAmountAndWalletMasterPass()
        {
            var transaction = new authorization
            {
                id = "7",
                orderId = "10",
                amount = 110,
                secondaryAmount = 50,
                orderSource = orderSourceType.applepay,
                applepay = new applepayType
                {
                    data = "user",
                    signature = "sign",
                    version = "12345",
                    header = new applepayHeaderType
                    {
                        applicationData = "454657413164",
                        ephemeralPublicKey = "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855",
                        publicKeyHash = "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855",
                        transactionId = "1234"
                    }
                },
                wallet = new wallet
                {
                    walletSourceTypeId = "123",
                    walletSourceType = walletSourceType.MasterPass
                }
            };
            
            var test = new BaseCnpOnlineTest();
            test.SetExceptionExpected(typeof(InvalidVersionException),null,"8.28");
            test.SetExceptionExpected(typeof(CnpOnlineException),"8.28","8.29");
            test.SetExceptionExpected(typeof(InvalidVersionException),"9.0","9.1");
            test.SetExceptionExpected(typeof(CnpOnlineException),"9.1","9.2");
            test.SetExpectedPopulated("id",transaction.id,null,null);
            test.SetExpectedPopulated("cnpTxnId","12.0",null);
            test.SetExpectedPopulated("orderId",transaction.orderId,null,null);
            test.SetExpectedPopulated("response","110",null,null);
            test.SetExpectedPopulated("responseTime",null,null);
            test.SetExpectedPopulated("message","Insufficient Funds",null,null);
            test.SetExpectedPopulated("authCode",null,null);
            test.SetExpectedPopulated("applepayResponse",null,null);
            test.SetExpectedUnpopulated("networkTransactionId",null,null);
            test.SetExpectedPopulated("litleTxnId","8.21","12.0");
            test.RunCnpTestThreaded<authorizationResponse>(transaction);
        }
        
        [Test]
        public void TestSimpleAuthWithApplePayAndSecondaryAmountAndWalletVisaCheckout()
        {
            var transaction = new authorization
            {
                id = "8",
                orderId = "11",
                amount = 110,
                secondaryAmount = 50,
                orderSource = orderSourceType.applepay,
                applepay = new applepayType
                {
                    data = "user",
                    signature = "sign",
                    version = "12345",
                    header = new applepayHeaderType
                    {
                        applicationData = "454657413164",
                        ephemeralPublicKey = "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855",
                        publicKeyHash = "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855",
                        transactionId = "1234"
                    }
                },
                wallet = new wallet
                {
                    walletSourceTypeId = "123",
                    walletSourceType = walletSourceType.VisaCheckout
                }
            };
            
            var test = new BaseCnpOnlineTest();
            test.SetExceptionExpected(typeof(InvalidVersionException),null,"9.7");
            test.SetExpectedPopulated("id",transaction.id,null,null);
            test.SetExpectedPopulated("cnpTxnId","12.0",null);
            test.SetExpectedPopulated("orderId",transaction.orderId,null,null);
            test.SetExpectedPopulated("response","110",null,null);
            test.SetExpectedPopulated("responseTime",null,null);
            test.SetExpectedPopulated("message","Insufficient Funds",null,null);
            test.SetExpectedPopulated("authCode",null,null);
            test.SetExpectedPopulated("applepayResponse",null,null);
            test.SetExpectedUnpopulated("networkTransactionId",null,null);
            test.SetExpectedPopulated("litleTxnId","8.21","12.0");
            test.RunCnpTestThreaded<authorizationResponse>(transaction);
        }
        
        [Test]
        public void TestPosWithoutCapabilityAndEntryMode()
        {
            var transaction = new authorization
            {
                id = "9",
                orderId = "12",
                amount = 106,
                orderSource = orderSourceType.ecommerce,
                pos = new pos { cardholderId = posCardholderIdTypeEnum.pin },
                card = new cardType
                {
                    type = methodOfPaymentTypeEnum.VI,
                    number = "4100000000000002",
                    expDate = "1210"
                },
                customBilling = new customBilling { phone = "1112223333" }
            };
            
            var test = new BaseCnpOnlineTest();
            test.SetExceptionExpected(typeof(CnpOnlineException),null,null);
            test.RunCnpTestThreaded<authorizationResponse>(transaction);
        }
        
        [Test]
        public void TestTrackData()
        {
            var transaction = new authorization
            {
                id = "10",
                orderId = "13",
                amount = 12522,
                orderSource = orderSourceType.retail,
                billToAddress = new billToAddress { zip = "95032" },
                card = new cardType { track = "%B40000001^Doe/JohnP^06041...?;40001=0604101064200?" },
                pos = new pos
                {
                    capability = posCapabilityTypeEnum.magstripe,
                    entryMode = posEntryModeTypeEnum.completeread,
                    cardholderId = posCardholderIdTypeEnum.signature
                }
            };
            
            var test = new BaseCnpOnlineTest();
            test.SetExpectedPopulated("id",transaction.id,null,null);
            test.SetExpectedPopulated("cnpTxnId","12.0",null);
            test.SetExpectedPopulated("orderId",transaction.orderId,null,null);
            test.SetExpectedPopulated("response","000",null,null);
            test.SetExpectedPopulated("responseTime",null,null);
            test.SetExpectedPopulated("message","Approved",null,null);
            test.SetExpectedPopulated("authCode",null,null);
            test.SetExpectedUnpopulated("networkTransactionId",null,"8.30");
            test.SetExpectedPopulated("networkTransactionId","8.30","9.0");
            test.SetExpectedUnpopulated("networkTransactionId","9.0","9.10");
            test.SetExpectedPopulated("networkTransactionId","9.10","10.0");
            test.SetExpectedUnpopulated("networkTransactionId","10.0","10.5");
            test.SetExpectedPopulated("networkTransactionId","10.5",null);
            test.SetExpectedPopulated("litleTxnId","8.21","12.0");
            test.RunCnpTestThreaded<authorizationResponse>(transaction);
        }

        [Test]
        public void TestAuthHandleSpecialCharacters()
        {
            var transaction = new authorization
            {
                id = "11",
                reportGroup = "<'&\">",
                orderId = "14",
                amount = 106,
                orderSource = orderSourceType.ecommerce,
                paypal = new paypal
                {
                    payerId = "1234",
                    token = "1234",
                    transactionId = "123456"
                }
            };
            
            var test = new BaseCnpOnlineTest();
            test.SetExpectedPopulated("id",transaction.id,null,null);
            test.SetExpectedPopulated("reportGroup","<'&\">",null,null);
            test.SetExpectedPopulated("cnpTxnId","12.0",null);
            test.SetExpectedPopulated("orderId",transaction.orderId,null,null);
            test.SetExpectedPopulated("response","000",null,null);
            test.SetExpectedPopulated("responseTime",null,null);
            test.SetExpectedPopulated("message","Approved",null,null);
            test.SetExpectedPopulated("authCode",null,null);
            test.SetExpectedUnpopulated("networkTransactionId",null,null);
            test.SetExpectedPopulated("litleTxnId","8.21","12.0");
            test.RunCnpTestThreaded<authorizationResponse>(transaction);
        }
        
        [Test]
        public void TestEnhancedAuthResponse()
        {
            var transaction = new authorization
            {
                id = "12",
                reportGroup = "<'&\">",
                orderId = "12344",
                amount = 106,
                orderSource = orderSourceType.ecommerce,
                card = new cardType
                {
                    type = methodOfPaymentTypeEnum.VI,
                    number = "4100322311199000",
                    expDate = "1210",
                },
                originalNetworkTransactionId = "123456789123456789123456789",
                originalTransactionAmount = 12,
                processingType = processingTypeEnum.initialRecurring,
            };
            
            var test = new BaseCnpOnlineTest();
            test.SetExceptionExpected(typeof(InvalidVersionException),null,"8.30");
            test.SetExceptionExpected(typeof(InvalidVersionException),"9.0","9.10");
            test.SetExceptionExpected(typeof(InvalidVersionException),"10.0","10.5");
            test.SetExpectedPopulated("id",transaction.id,null,null);
            test.SetExpectedPopulated("reportGroup","<'&\">",null,null);
            test.SetExpectedPopulated("cnpTxnId","12.0",null);
            test.SetExpectedPopulated("orderId",transaction.orderId,null,null);
            test.SetExpectedPopulated("response","000",null,null);
            test.SetExpectedPopulated("responseTime",null,null);
            test.SetExpectedPopulated("message","Approved",null,null);
            test.SetExpectedPopulated("authCode",null,null);
            test.SetExpectedUnpopulated("networkTransactionId",null,"8.30");
            test.SetExpectedPopulated("networkTransactionId","8.30","9.0");
            test.SetExpectedUnpopulated("networkTransactionId","9.0","9.10");
            test.SetExpectedPopulated("networkTransactionId","9.10","10.0");
            test.SetExpectedUnpopulated("networkTransactionId","10.0","10.5");
            test.SetExpectedPopulated("networkTransactionId","10.5",null);
            test.SetExpectedPopulated("enhancedAuthResponse",null,null);
            test.SetExpectedPopulated("litleTxnId","8.21","12.0");
            test.RunCnpTestThreaded<authorizationResponse>(transaction);
        }
        
        [Test]
        public void TestSimpleAuthWithCardPin()
        {
            var transaction = new authorization
            {
                id = "13",
                reportGroup = "Planets",
                orderId = "12344",
                amount = 106,
                orderSource = orderSourceType.ecommerce,
                card = new cardType
                {
                    type = methodOfPaymentTypeEnum.MC,
                    number = "414100000000000000",
                    expDate = "1210",
                    pin = "1234",
                },
                customBilling = new customBilling { phone = "1112223333" }
            };
            
            var test = new BaseCnpOnlineTest();
            test.SetExpectedPopulated("id",transaction.id,null,null);
            test.SetExpectedPopulated("cnpTxnId","12.0",null);
            test.SetExpectedPopulated("orderId",transaction.orderId,null,null);
            test.SetExpectedPopulated("response","000",null,null);
            test.SetExpectedPopulated("responseTime",null,null);
            test.SetExpectedPopulated("message","Approved",null,null);
            test.SetExpectedPopulated("authCode",null,null);
            test.SetExpectedPopulated("litleTxnId","8.21","12.0");
            test.RunCnpTestThreaded<authorizationResponse>(transaction);
        }
        
        [Test]
        public void TestSimpleAuthWithLodgingInfo()
        {
            var transaction = new authorization
            {
                id = "14",
                orderId = "12344",
                amount = 106,
                orderSource = orderSourceType.androidpay,
                card = new cardType
                {
                    type = methodOfPaymentTypeEnum.MC,
                    number = "414100000000000000",
                    expDate = "1210",
                    pin = "1234",
                },
                customBilling = new customBilling { phone = "1112223333" },
                lodgingInfo = new lodgingInfo
                {
                    hotelFolioNumber = "12345",
                    checkInDate = new DateTime(2017, 1, 18),
                    customerServicePhone = "854213",
                    lodgingCharge = new List<lodgingCharge>(),

                },
            };
            
            var test = new BaseCnpOnlineTest();
            test.SetExceptionExpected(typeof(InvalidVersionException),null,"9.8");
            test.SetExceptionExpected(typeof(InvalidVersionException),"10.0","10.2");
            test.SetExceptionExpected(typeof(CnpOnlineException),"10.2","10.6"); // Sandbox fails with Android Pay for these versions.
            test.SetExpectedPopulated("id",transaction.id,null,null);
            test.SetExpectedPopulated("cnpTxnId","12.0",null);
            test.SetExpectedPopulated("orderId",transaction.orderId,null,null);
            test.SetExpectedPopulated("response","000",null,null);
            test.SetExpectedPopulated("responseTime",null,null);
            test.SetExpectedPopulated("message","Approved",null,null);
            test.SetExpectedPopulated("authCode",null,null);
            test.SetExpectedPopulated("androidpayResponse",null,null);
            test.SetExpectedUnpopulated("networkTransactionId",null,null);
            test.SetExpectedPopulated("litleTxnId","8.21","12.0");
            test.RunCnpTestThreaded<authorizationResponse>(transaction);
        }
        
        [Test]
        public void TestSimpleAuthWithSkipRealtimeAUTrue()
        {
            
            var transaction = new authorization
            {
                id = "15",
                orderId = "12344",
                amount = 106,
                orderSource = orderSourceType.ecommerce,
                card = new cardType
                {
                    type = methodOfPaymentTypeEnum.VI,
                    number = "414100000000000000",
                    expDate = "1210"
                },
                customBilling = new customBilling {phone = "1112223333"},
                skipRealtimeAU = true
            };
            
            var test = new BaseCnpOnlineTest();
            test.SetExpectedPopulated("id",transaction.id,null,null);
            test.SetExpectedPopulated("cnpTxnId","12.0",null);
            test.SetExpectedPopulated("orderId",transaction.orderId,null,null);
            test.SetExpectedPopulated("response","000",null,null);
            test.SetExpectedPopulated("responseTime",null,null);
            test.SetExpectedPopulated("message","Approved",null,null);
            test.SetExpectedPopulated("authCode",null,null);
            test.SetExpectedUnpopulated("networkTransactionId",null,"8.30");
            test.SetExpectedPopulated("networkTransactionId","8.30","9.0");
            test.SetExpectedUnpopulated("networkTransactionId","9.0","9.10");
            test.SetExpectedPopulated("networkTransactionId","9.10","10.0");
            test.SetExpectedUnpopulated("networkTransactionId","10.0","10.5");
            test.SetExpectedPopulated("networkTransactionId","10.5",null);
            test.SetExpectedPopulated("litleTxnId","8.21","12.0");
            test.RunCnpTestThreaded<authorizationResponse>(transaction);
        }
        
        [Test]
        public void TestSimpleAuthWithSkipRealtimeAUFalse()
        {
            
            var transaction = new authorization
            {
                id = "16",
                orderId = "12344",
                amount = 106,
                orderSource = orderSourceType.ecommerce,
                card = new cardType
                {
                    type = methodOfPaymentTypeEnum.VI,
                    number = "414100000000000000",
                    expDate = "1210"
                },
                customBilling = new customBilling {phone = "1112223333"},
                skipRealtimeAU = false
            };
            
            var test = new BaseCnpOnlineTest();
            test.SetExpectedPopulated("id",transaction.id,null,null);
            test.SetExpectedPopulated("cnpTxnId","12.0",null);
            test.SetExpectedPopulated("orderId",transaction.orderId,null,null);
            test.SetExpectedPopulated("response","000",null,null);
            test.SetExpectedPopulated("responseTime",null,null);
            test.SetExpectedPopulated("message","Approved",null,null);
            test.SetExpectedPopulated("authCode",null,null);
            test.SetExpectedUnpopulated("networkTransactionId",null,"8.30");
            test.SetExpectedPopulated("networkTransactionId","8.30","9.0");
            test.SetExpectedUnpopulated("networkTransactionId","9.0","9.10");
            test.SetExpectedPopulated("networkTransactionId","9.10","10.0");
            test.SetExpectedUnpopulated("networkTransactionId","10.0","10.5");
            test.SetExpectedPopulated("networkTransactionId","10.5",null);
            test.SetExpectedPopulated("litleTxnId","8.21","12.0");
            test.RunCnpTestThreaded<authorizationResponse>(transaction);
        }
    }
}