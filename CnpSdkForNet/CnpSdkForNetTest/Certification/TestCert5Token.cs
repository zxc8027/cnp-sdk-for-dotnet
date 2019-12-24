/*
 * Zachary Cook
 *
 * Set of legacy tests that go against the certification
 * environment instead of sandbox.
 */

using NUnit.Framework;

namespace Cnp.Sdk.Test.Certification
{
    [TestFixture]
    public class TestCert5Token : BaseCertificationOnlineTest
    {
        [Test]
        public void Test50()
        {
            var request = new registerTokenRequestType();
            request.id = "1";
            request.orderId = "50";
            request.accountNumber = "4457119922390123";

            var response = this.SendTransaction<registerTokenResponse>(request);
            Assert.AreEqual("445711", response.bin);
            Assert.AreEqual(methodOfPaymentTypeEnum.VI, response.type);
            //TODO: //Getting 802 instead
            //Assert.AreEqual("801", response.response);
            Assert.AreEqual("1111000276870123", response.cnpToken);
            //Assert.AreEqual("Account number was successfully registered", response.message);
        }

        [Test]
        public void Test51()
        {
            var request = new registerTokenRequestType();
            request.id = "1";
            request.orderId = "51";
            request.accountNumber = "4457119999999999";

            var response = this.SendTransaction<registerTokenResponse>(request);
            Assert.AreEqual("820", response.response);
            Assert.AreEqual("Credit card number was invalid", response.message);
        }

        [Test]
        public void Test52()
        {
            var request = new registerTokenRequestType();
            request.id = "1";
            request.orderId = "52";
            request.accountNumber = "4457119922390123";

            var response = this.SendTransaction<registerTokenResponse>(request);
            Assert.AreEqual("445711", response.bin);
            Assert.AreEqual(methodOfPaymentTypeEnum.VI, response.type);
            Assert.AreEqual("802", response.response);
            Assert.AreEqual("1111000276870123", response.cnpToken);
            Assert.AreEqual("Account number was previously registered", response.message);
        }

        [Test]
        public void Test53()
        {
            var request = new registerTokenRequestType();
            request.id = "1";
            request.orderId = "53";
            var echeck = new echeckForTokenType();
            echeck.accNum = "1099999998";
            echeck.routingNum = "114567895";
            request.echeckForToken = echeck; ;

            var response = this.SendTransaction<registerTokenResponse>(request);
            //TODO: //getting null as response type
            //Assert.AreEqual(methodOfPaymentTypeEnum.EC, response.type);
            //TODO: //getting null as eCheckAccountSuffix
            //Assert.AreEqual("998", response.eCheckAccountSuffix);
            //TODO: //getting 900 as response and corresponding message
            //Assert.AreEqual("801", response.response);
            //Assert.AreEqual("Account number was successfully registered", response.message);
            //TODO: //getting null as cnptoken
            //Assert.AreEqual("111922223333000998", response.cnpToken);
        }

        [Test]
        public void Test54()
        {
            var request = new registerTokenRequestType();
            request.id = "1";
            request.orderId = "54";
            var echeck = new echeckForTokenType();
            echeck.accNum = "1022222102";
            echeck.routingNum = "1145_7895";
            request.echeckForToken = echeck; ;

            var response = this.SendTransaction<registerTokenResponse>(request);
            Assert.AreEqual("900", response.response);
        }

        [Test]
        public void Test55()
        {
            var auth = new authorization();
            auth.id = "1";
            auth.orderId = "55";
            auth.amount = 15000;
            auth.orderSource = orderSourceType.ecommerce;
            var card = new cardType();
            card.number = "5435101234510196";
            card.expDate = "1121";
            card.cardValidationNum = "987";
            card.type = methodOfPaymentTypeEnum.MC;
            auth.card = card;

            var response = this.SendTransaction<authorizationResponse>(auth);
            Assert.AreEqual("000", response.response);
            Assert.AreEqual("Approved", response.message);
            //TODO: //Getting 802 instead
            //Assert.AreEqual("801", response.tokenResponse.tokenResponseCode);
            //Assert.AreEqual("Account number was successfully registered", response.tokenResponse.tokenMessage);
            Assert.AreEqual(methodOfPaymentTypeEnum.MC, response.tokenResponse.type);
            Assert.AreEqual("543510", response.tokenResponse.bin);
        }

        [Test]
        public void Test56()
        {
            var auth = new authorization();
            auth.id = "1";
            auth.orderId = "56";
            auth.amount = 15000;
            auth.orderSource = orderSourceType.ecommerce;
            var card = new cardType();
            card.number = "5435109999999999";
            card.expDate = "1112";
            card.cardValidationNum = "987";
            card.type = methodOfPaymentTypeEnum.MC;
            auth.card = card;

            var response = this.SendTransaction<authorizationResponse>(auth);
            Assert.AreEqual("301", response.response);
        }

        [Test]
        public void Test57()
        {
            var auth = new authorization();
            auth.id = "1";
            auth.orderId = "57";
            auth.amount = 15000;
            auth.orderSource = orderSourceType.ecommerce;
            var card = new cardType();
            card.number = "5435101234510196";
            card.expDate = "1112";
            card.cardValidationNum = "987";
            card.type = methodOfPaymentTypeEnum.MC;
            auth.card = card;

            var response = this.SendTransaction<authorizationResponse>(auth);
            Assert.AreEqual("000", response.response);
            Assert.AreEqual("Approved", response.message);
            Assert.AreEqual("802", response.tokenResponse.tokenResponseCode);
            Assert.AreEqual("Account number was previously registered", response.tokenResponse.tokenMessage);
            Assert.AreEqual(methodOfPaymentTypeEnum.MC, response.tokenResponse.type);
            Assert.AreEqual("543510", response.tokenResponse.bin);
        }

        [Test]
        public void Test59()
        {
            var auth = new authorization();
            auth.id = "1";
            auth.orderId = "59";
            auth.amount = 15000;
            auth.orderSource = orderSourceType.ecommerce;
            var token = new cardTokenType();
            token.cnpToken = "1111000100092332";
            token.expDate = "1121";
            auth.token = token;

            var response = this.SendTransaction<authorizationResponse>(auth);
            Assert.AreEqual("822", response.response);
            Assert.AreEqual("Token was not found", response.message);
        }

        [Test]
        public void Test60()
        {
            var auth = new authorization();
            auth.id = "1";
            auth.orderId = "60";
            auth.amount = 15000;
            auth.orderSource = orderSourceType.ecommerce;
            var token = new cardTokenType();
            token.cnpToken = "1112000100000085";
            token.expDate = "1121";
            auth.token = token;

            var response = this.SendTransaction<authorizationResponse>(auth);
            Assert.AreEqual("823", response.response);
        }

        [Test]
        public void Test61()
        {
            var sale = new echeckSale();
            sale.id = "1";
            sale.orderId = "61";
            sale.amount = 15000;
            sale.orderSource = orderSourceType.ecommerce;
            var billToAddress = new contact();
            billToAddress.firstName = "Tom";
            billToAddress.lastName = "Black";
            sale.billToAddress = billToAddress;
            var echeck = new echeckType();
            echeck.accType = echeckAccountTypeEnum.Checking; ;
            echeck.accNum = "1099999003";
            echeck.routingNum = "011100012";
            sale.echeck = echeck;

            var response = this.SendTransaction<echeckSalesResponse>(sale);
            //TODO: could not get token response
            //Assert.AreEqual("801", response.tokenResponse.tokenResponseCode);
            //Assert.AreEqual("Account number was successfully registered", response.tokenResponse.tokenMessage);
            //Assert.AreEqual(methodOfPaymentTypeEnum.EC, response.tokenResponse.type);
            //Assert.AreEqual("111922223333444003", response.tokenResponse.cnpToken);
        }

        [Test]
        public void Test62()
        {
            var sale = new echeckSale();
            sale.id = "1";
            sale.orderId = "62";
            sale.amount = 15000;
            sale.orderSource = orderSourceType.ecommerce;
            var billToAddress = new contact();
            billToAddress.firstName = "Tom";
            billToAddress.lastName = "Black";
            sale.billToAddress = billToAddress;
            var echeck = new echeckType();
            echeck.accType = echeckAccountTypeEnum.Checking; ;
            echeck.accNum = "1099999999";
            echeck.routingNum = "011100012";
            sale.echeck = echeck;

            var response = this.SendTransaction<echeckSalesResponse>(sale);
            //TODO: //Could not get token response
            //Assert.AreEqual("801", response.tokenResponse.tokenResponseCode);
            //Assert.AreEqual("Account number was successfully registered", response.tokenResponse.tokenMessage);
            //Assert.AreEqual(methodOfPaymentTypeEnum.EC, response.tokenResponse.type);
            //Assert.AreEqual("999", response.tokenResponse.eCheckAccountSuffix);
            //Assert.AreEqual("111922223333444999", response.tokenResponse.cnpToken);
        }

        [Test]
        public void Test63()
        {
            var sale = new echeckSale();
            sale.id = "1";
            sale.orderId = "63";
            sale.amount = 15000;
            sale.orderSource = orderSourceType.ecommerce;
            var billToAddress = new contact();
            billToAddress.firstName = "Tom";
            billToAddress.lastName = "Black";
            sale.billToAddress = billToAddress;
            var echeck = new echeckType();
            echeck.accType = echeckAccountTypeEnum.Checking; ;
            echeck.accNum = "1099999999";
            echeck.routingNum = "011100012";
            sale.echeck = echeck;

            var response = this.SendTransaction<echeckSalesResponse>(sale);
            //TODO: //could not get token response back
            //Assert.AreEqual("801", response.tokenResponse.tokenResponseCode);
            //Assert.AreEqual("Account number was successfully registered", response.tokenResponse.tokenMessage);
            //Assert.AreEqual(methodOfPaymentTypeEnum.EC, response.tokenResponse.type);
            //Assert.AreEqual("999", response.tokenResponse.eCheckAccountSuffix);
            //Assert.AreEqual("111922223333555999", response.tokenResponse.cnpToken);
        }
    }
}