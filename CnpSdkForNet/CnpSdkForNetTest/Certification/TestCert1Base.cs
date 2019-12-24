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
    public class TestCert1Base : BaseCertificationOnlineTest
    {
        [Test]
        public void Test1Auth()
        {
           
            var authorization = new authorization();
            authorization.id = "1";
            authorization.orderId = "1";
            authorization.amount = 10100;
            authorization.orderSource = orderSourceType.ecommerce;
            var contact = new contact();
            contact.name = "John & Mary Smith";
            contact.addressLine1 = "1 Main St.";
            contact.city = "Burlington";
            contact.state = "MA";
            contact.zip = "01803-3747";
            contact.country = countryTypeEnum.US;
            authorization.billToAddress = contact;
            var card = new cardType();            
            card.type = methodOfPaymentTypeEnum.VI;
            card.number = "4457010000000009";
            card.expDate = "0121";
            card.cardValidationNum = "349";
            authorization.card = card;

            var response = this.SendTransaction<authorizationResponse>(authorization);
            Assert.AreEqual("000", response.response);
            Assert.AreEqual("Approved", response.message);
            Assert.AreEqual("11111 ", response.authCode);
            Assert.AreEqual("01", response.fraudResult.avsResult);
            Assert.AreEqual("M", response.fraudResult.cardValidationResult);

            var capture = new capture();
            capture.id = response.id;
            capture.cnpTxnId = response.cnpTxnId;
            var captureResponse = this.SendTransaction<captureResponse>(capture);
            Assert.AreEqual("000", captureResponse.response);
            Assert.AreEqual("Approved", captureResponse.message);

            var credit = new credit();
            credit.id = captureResponse.id;
            credit.cnpTxnId = captureResponse.cnpTxnId;
            var creditResponse = this.SendTransaction<creditResponse>(credit);
            Assert.AreEqual("000", creditResponse.response);
            Assert.AreEqual("Approved", creditResponse.message);

            var newvoid = new voidTxn();
            newvoid.id = creditResponse.id;
            newvoid.cnpTxnId = creditResponse.cnpTxnId;
            var voidResponse = this.SendTransaction<voidResponse>(newvoid);
            Assert.AreEqual("000", voidResponse.response);
            Assert.AreEqual("Approved", voidResponse.message);
        }

        [Test]
        public void Test1AVS()
        {
            var authorization = new authorization();
            authorization.id = "1";
            authorization.orderId = "1";
            authorization.amount = 0;
            authorization.orderSource = orderSourceType.ecommerce;
            var contact = new contact();
            contact.name = "John Smith";
            contact.addressLine1 = "1 Main St.";
            contact.city = "Burlington";
            contact.state = "MA";
            contact.zip = "01803-3747";
            contact.country = countryTypeEnum.US;
            authorization.billToAddress = contact;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.VI;
            card.number = "4457010000000009";
            card.expDate = "0112";
            card.cardValidationNum = "349";
            authorization.card = card;
            
            var response = this.SendTransaction<authorizationResponse>(authorization);
            Assert.AreEqual("000", response.response);
            Assert.AreEqual("Approved", response.message);
            Assert.AreEqual("11111 ", response.authCode);
            Assert.AreEqual("01", response.fraudResult.avsResult);
            Assert.AreEqual("M", response.fraudResult.cardValidationResult);
        }

        [Test]
        public void Test1Sale()
        {
            var sale = new sale();
            sale.id = "1";
            sale.orderId = "1";
            sale.amount = 10010;
            sale.orderSource = orderSourceType.ecommerce;
            var contact = new contact();
            contact.name = "John Smith";
            contact.addressLine1 = "1 Main St.";
            contact.city = "Burlington";
            contact.state = "MA";
            contact.zip = "01803-3747";
            contact.country = countryTypeEnum.US;
            sale.billToAddress = contact;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.VI;
            card.number = "4457010000000009";
            card.expDate = "0112";
            card.cardValidationNum = "349";
            sale.card = card;

            var response = this.SendTransaction<saleResponse>(sale);
            Assert.AreEqual("000", response.response);
            Assert.AreEqual("Approved", response.message);
            Assert.AreEqual("11111 ", response.authCode);
            Assert.AreEqual("01", response.fraudResult.avsResult);
            Assert.AreEqual("M", response.fraudResult.cardValidationResult);

            var credit = new credit();
            credit.id = response.id;
            credit.cnpTxnId = response.cnpTxnId;
            var creditResponse = this.SendTransaction<creditResponse>(credit);
            Assert.AreEqual("000", creditResponse.response);
            Assert.AreEqual("Approved", creditResponse.message);
            
            var newvoid = new voidTxn();
            newvoid.id = creditResponse.id;
            newvoid.cnpTxnId = creditResponse.cnpTxnId;
            var voidResponse = this.SendTransaction<voidResponse>(newvoid);
            Assert.AreEqual("000",voidResponse.response);
            Assert.AreEqual("Approved", voidResponse.message);
        }

        [Test]
        public void Test2Auth()
        {
            var authorization = new authorization();
            authorization.id = "1";
            authorization.orderId = "2";
            authorization.amount = 20020;
            authorization.orderSource = orderSourceType.ecommerce;
            var contact = new contact();
            contact.name = "Mike J. Hammer";
            contact.addressLine1 = "2 Main St.";
            contact.addressLine2 = "Apt. 222";
            contact.city = "Riverside";
            contact.state = "RI";
            contact.zip = "02915";
            contact.country = countryTypeEnum.US;
            authorization.billToAddress = contact;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.MC;
            card.number = "5112010000000003";
            card.expDate = "0212";
            card.cardValidationNum = "261";
            authorization.card = card;
            var authenticationvalue = new fraudCheckType();
            authenticationvalue.authenticationValue = "BwABBJQ1AgAAAAAgJDUCAAAAAAA=";
            authorization.cardholderAuthentication = authenticationvalue;

            var response = this.SendTransaction<authorizationResponse>(authorization);
            Assert.AreEqual("000", response.response);
            Assert.AreEqual("Approved", response.message);
            Assert.AreEqual("22222", response.authCode.Trim());
            Assert.AreEqual("10", response.fraudResult.avsResult);
            Assert.AreEqual("M", response.fraudResult.cardValidationResult);

            var capture = new capture();
            capture.id = response.id;
            capture.cnpTxnId = response.cnpTxnId;
            var captureresponse = this.SendTransaction<captureResponse>(capture);
            Assert.AreEqual("000", captureresponse.response);
            Assert.AreEqual("Approved", captureresponse.message);

            var credit = new credit();
            credit.id = captureresponse.id;
            credit.cnpTxnId = captureresponse.cnpTxnId;
            var creditResponse = this.SendTransaction<creditResponse>(credit);
            Assert.AreEqual("000", creditResponse.response);
            Assert.AreEqual("Approved", creditResponse.message);

            var newvoid = new voidTxn();
            newvoid.id = creditResponse.id;
            newvoid.cnpTxnId = creditResponse.cnpTxnId;
            var voidResponse = this.SendTransaction<voidResponse>(newvoid);
            Assert.AreEqual("000",voidResponse.response);
            Assert.AreEqual("Approved", voidResponse.message);
        }

        [Test]
        public void Test2AVS()
        {
            var authorization = new authorization();
            authorization.id = "1";
            authorization.orderId = "2";
            authorization.amount = 0;
            authorization.orderSource = orderSourceType.ecommerce;
            var contact = new contact();
            contact.name = "Mike J. Hammer";
            contact.addressLine1 = "2 Main St.";
            contact.addressLine2 = "Apt. 222";
            contact.city = "Riverside";
            contact.state = "RI";
            contact.zip = "02915";
            contact.country = countryTypeEnum.US;
            authorization.billToAddress = contact;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.MC;
            card.number = "5112010000000003";
            card.expDate = "0212";
            card.cardValidationNum = "261";
            authorization.card = card;
            var authenticationvalue = new fraudCheckType();
            authenticationvalue.authenticationValue = "BwABBJQ1AgAAAAAgJDUCAAAAAAA=";
            authorization.cardholderAuthentication = authenticationvalue;

            var response = this.SendTransaction<authorizationResponse>(authorization);
            Assert.AreEqual("000", response.response);
            Assert.AreEqual("Approved", response.message);
            Assert.AreEqual("22222", response.authCode.Trim());
            Assert.AreEqual("10", response.fraudResult.avsResult);
            Assert.AreEqual("M", response.fraudResult.cardValidationResult);

        }

        [Test]
        public void Test2Sale()
        {
            var sale = new sale();
            sale.id = "1";
            sale.orderId = "2";
            sale.amount = 20020;
            sale.orderSource = orderSourceType.ecommerce;
            var contact = new contact();
            contact.name = "Mike J. Hammer";
            contact.addressLine1 = "2 Main St.";
            contact.addressLine2 = "Apt. 222";
            contact.city = "Riverside";
            contact.state = "RI";
            contact.zip = "02915";
            contact.country = countryTypeEnum.US;
            sale.billToAddress = contact;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.MC;
            card.number = "5112010000000003";
            card.expDate = "0212";
            card.cardValidationNum = "261";
            sale.card = card;
            var authenticationvalue = new fraudCheckType();
            authenticationvalue.authenticationValue = "BwABBJQ1AgAAAAAgJDUCAAAAAAA=";
            sale.cardholderAuthentication = authenticationvalue;

            var response = this.SendTransaction<saleResponse>(sale);
            Assert.AreEqual("000", response.response);
            Assert.AreEqual("Approved", response.message);
            Assert.AreEqual("22222", response.authCode.Trim());
            Assert.AreEqual("10", response.fraudResult.avsResult);
            Assert.AreEqual("M", response.fraudResult.cardValidationResult);

            var credit = new credit();
            credit.id = response.id;
            credit.cnpTxnId = response.cnpTxnId;
            var creditResponse = this.SendTransaction<creditResponse>(credit);
            Assert.AreEqual("000", creditResponse.response);
            Assert.AreEqual("Approved", creditResponse.message);

            var newvoid = new voidTxn();
            newvoid.id = creditResponse.id;
            newvoid.cnpTxnId = creditResponse.cnpTxnId;
            var voidResponse = this.SendTransaction<voidResponse>(newvoid);
            Assert.AreEqual("000", voidResponse.response);
            Assert.AreEqual("Approved", voidResponse.message);
        }

        [Test]
        public void Test3Auth()
        {
            var authorization = new authorization();
            authorization.id = "1";
            authorization.orderId = "3";
            authorization.amount = 30030;
            authorization.orderSource = orderSourceType.ecommerce;
            var contact = new contact();
            contact.name = "Eileen Jones";
            contact.addressLine1 = "3 Main St.";
            contact.city = "Bloomfield";
            contact.state = "CT";
            contact.zip = "06002";
            contact.country = countryTypeEnum.US;
            authorization.billToAddress = contact;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.DI;
            card.number = "6011010000000003";
            card.expDate = "0312";
            card.cardValidationNum = "758";
            authorization.card = card;

            var response = this.SendTransaction<authorizationResponse>(authorization);
            Assert.AreEqual("000", response.response);
            Assert.AreEqual("Approved", response.message);
            Assert.AreEqual("33333", response.authCode.Trim());
            Assert.AreEqual("10", response.fraudResult.avsResult);
            Assert.AreEqual("M", response.fraudResult.cardValidationResult);

            var capture = new capture();
            capture.id = response.id;
            capture.cnpTxnId = response.cnpTxnId;
            var captureResponse = this.SendTransaction<captureResponse>(capture);
            Assert.AreEqual("000", captureResponse.response);
            Assert.AreEqual("Approved", captureResponse.message);

            var credit = new credit();
            credit.id = captureResponse.id;
            credit.cnpTxnId = captureResponse.cnpTxnId;
            var creditResponse = this.SendTransaction<creditResponse>(credit);
            Assert.AreEqual("000", creditResponse.response);
            Assert.AreEqual("Approved", creditResponse.message);

            var newvoid = new voidTxn();
            newvoid.id = creditResponse.id;
            newvoid.cnpTxnId = creditResponse.cnpTxnId;
            var voidResponse = this.SendTransaction<voidResponse>(newvoid);
            Assert.AreEqual("000", voidResponse.response);
            Assert.AreEqual("Approved", voidResponse.message);
        }

        [Test]
        public void Test3AVS()
        {
            var authorization = new authorization();
            authorization.id = "1";
            authorization.orderId = "3";
            authorization.amount = 0;
            authorization.orderSource = orderSourceType.ecommerce;
            var contact = new contact();
            contact.name = "Eileen Jones";
            contact.addressLine1 = "3 Main St.";
            contact.city = "Bloomfield";
            contact.state = "CT";
            contact.zip = "06002";
            contact.country = countryTypeEnum.US;
            authorization.billToAddress = contact;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.DI;
            card.number = "6011010000000003";
            card.expDate = "0312";
            card.cardValidationNum = "758";
            authorization.card = card;

            var response = this.SendTransaction<authorizationResponse>(authorization);
            Assert.AreEqual("000", response.response);
            Assert.AreEqual("Approved", response.message);
            Assert.AreEqual("33333", response.authCode.Trim());
            Assert.AreEqual("10", response.fraudResult.avsResult);
            Assert.AreEqual("M", response.fraudResult.cardValidationResult);

        }

        [Test]
        public void Test3Sale()
        {
            var sale = new sale();
            sale.id = "1";
            sale.orderId = "3";
            sale.amount = 30030;
            sale.orderSource = orderSourceType.ecommerce;
            var contact = new contact();
            contact.name = "Eileen Jones";
            contact.addressLine1 = "3 Main St.";
            contact.city = "Bloomfield";
            contact.state = "CT";
            contact.zip = "06002";
            contact.country = countryTypeEnum.US;
            sale.billToAddress = contact;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.DI;
            card.number = "6011010000000003";
            card.expDate = "0312";
            card.cardValidationNum = "758";
            sale.card = card;

            var response = this.SendTransaction<saleResponse>(sale);
            Assert.AreEqual("000", response.response);
            Assert.AreEqual("Approved", response.message);
            Assert.AreEqual("33333", response.authCode.Trim());
            Assert.AreEqual("10", response.fraudResult.avsResult);
            Assert.AreEqual("M", response.fraudResult.cardValidationResult);

            var credit = new credit();
            credit.id = response.id;
            credit.cnpTxnId = response.cnpTxnId;
            var creditResponse = this.SendTransaction<creditResponse>(credit);
            Assert.AreEqual("000", creditResponse.response);
            Assert.AreEqual("Approved", creditResponse.message);

            var newvoid = new voidTxn();
            newvoid.id = creditResponse.id;
            newvoid.cnpTxnId = creditResponse.cnpTxnId;
            var voidResponse = this.SendTransaction<voidResponse>(newvoid);
            Assert.AreEqual("000", voidResponse.response);
            Assert.AreEqual("Approved", voidResponse.message);
        }

        [Test]
        public void Test4Auth()
        {
            var authorization = new authorization();
            authorization.id = "1";
            authorization.orderId = "4";
            authorization.amount = 10100;
            authorization.orderSource = orderSourceType.ecommerce;
            var contact = new contact();
            contact.name = "Bob Black";
            contact.addressLine1 = "4 Main St.";
            contact.city = "Laurel";
            contact.state = "MD";
            contact.zip = "20708";
            contact.country = countryTypeEnum.US;
            authorization.billToAddress = contact;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.AX;
            card.number = "375001000000005";
            card.expDate = "0421";
            //card.cardValidationNum = "758";
            authorization.card = card;

            var response = this.SendTransaction<authorizationResponse>(authorization);
            Assert.AreEqual("000", response.response);
            Assert.AreEqual("Approved", response.message);
            Assert.AreEqual("44444", response.authCode.Trim());
            Assert.AreEqual("13", response.fraudResult.avsResult);

            var capture = new capture();
            capture.id = response.id;
            capture.cnpTxnId = response.cnpTxnId;
            var captureresponse = this.SendTransaction<captureResponse>(capture);
            Assert.AreEqual("000", captureresponse.response);
            Assert.AreEqual("Approved", captureresponse.message);

            var credit = new credit();
            credit.id = captureresponse.id;
            credit.cnpTxnId = captureresponse.cnpTxnId;
            var creditResponse = this.SendTransaction<creditResponse>(credit);
            Assert.AreEqual("000", creditResponse.response);
            Assert.AreEqual("Approved", creditResponse.message);

            var newvoid = new voidTxn();
            newvoid.id = creditResponse.id;
            newvoid.cnpTxnId = creditResponse.cnpTxnId;
            var voidResponse = this.SendTransaction<voidResponse>(newvoid);
            Assert.AreEqual("000", voidResponse.response);
            Assert.AreEqual("Approved", voidResponse.message);
        }

        [Test]
        public void Test4AVS()
        {
            var authorization = new authorization();
            authorization.id = "1";
            authorization.orderId = "4";
            authorization.amount = 0;
            authorization.orderSource = orderSourceType.ecommerce;
            var contact = new contact();
            contact.name = "Bob Black";
            contact.addressLine1 = "4 Main St.";
            contact.city = "Laurel";
            contact.state = "MD";
            contact.zip = "20708";
            contact.country = countryTypeEnum.US;
            authorization.billToAddress = contact;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.AX;
            card.number = "375001000000005";
            card.expDate = "0412";
            authorization.card = card;

            var response = this.SendTransaction<authorizationResponse>(authorization);
            Assert.AreEqual("000", response.response);
            Assert.AreEqual("Approved", response.message);
            Assert.AreEqual("44444", response.authCode.Trim());
            Assert.AreEqual("13", response.fraudResult.avsResult);
        }

        [Test]
        public void Test4Sale()
        {
            var sale = new sale();
            sale.id = "1";
            sale.orderId = "4";
            sale.amount = 40040;
            sale.orderSource = orderSourceType.ecommerce;
            var contact = new contact();
            contact.name = "Bob Black";
            contact.addressLine1 = "4 Main St.";
            contact.city = "Laurel";
            contact.state = "MD";
            contact.zip = "20708";
            contact.country = countryTypeEnum.US;
            sale.billToAddress = contact;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.AX;
            card.number = "375001000000005";
            card.expDate = "0412";
            card.cardValidationNum = "758";
            sale.card = card;

            var response = this.SendTransaction<saleResponse>(sale);
            Assert.AreEqual("000", response.response);
            Assert.AreEqual("Approved", response.message);
            Assert.AreEqual("44444", response.authCode.Trim());
            Assert.AreEqual("13", response.fraudResult.avsResult);

            var credit = new credit();
            credit.id = response.id;
            credit.cnpTxnId = response.cnpTxnId;
            var creditResponse = this.SendTransaction<creditResponse>(credit);
            Assert.AreEqual("000", creditResponse.response);
            Assert.AreEqual("Approved", creditResponse.message);

            var newvoid = new voidTxn();
            newvoid.id = creditResponse.id;
            newvoid.cnpTxnId = creditResponse.cnpTxnId;
            var voidResponse = this.SendTransaction<voidResponse>(newvoid);
            Assert.AreEqual("000", voidResponse.response);
            Assert.AreEqual("Approved", voidResponse.message);
        }

        [Test]
        public void Test5Auth()
        {
            var authorization = new authorization();
            authorization.id = "1";
            authorization.orderId = "5";
            authorization.amount = 50050;
            authorization.orderSource = orderSourceType.ecommerce;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.VI;
            card.number = "4457010200000007";
            card.expDate = "0512";
            card.cardValidationNum = "463";
            authorization.card = card;
            var authenticationvalue = new fraudCheckType();
            authenticationvalue.authenticationValue = "BwABBJQ1AgAAAAAgJDUCAAAAAAA=";
            authorization.cardholderAuthentication = authenticationvalue;

            var response = this.SendTransaction<authorizationResponse>(authorization);
            Assert.AreEqual("000", response.response);
            Assert.AreEqual("Approved", response.message);
            Assert.AreEqual("55555 ", response.authCode);
            Assert.AreEqual("32", response.fraudResult.avsResult);
            Assert.AreEqual("M", response.fraudResult.cardValidationResult);

            var capture = new capture();
            capture.id = response.id;
            capture.cnpTxnId = response.cnpTxnId;
            var captureresponse = this.SendTransaction<captureResponse>(capture);
            Assert.AreEqual("000", captureresponse.response);
            Assert.AreEqual("Approved", captureresponse.message);

            var credit = new credit();
            credit.id = captureresponse.id;
            credit.cnpTxnId = captureresponse.cnpTxnId;
            var creditResponse = this.SendTransaction<creditResponse>(credit);
            Assert.AreEqual("000", creditResponse.response);
            Assert.AreEqual("Approved", creditResponse.message);

            var newvoid = new voidTxn();
            newvoid.id = creditResponse.id;
            newvoid.cnpTxnId = creditResponse.cnpTxnId;
            var voidResponse = this.SendTransaction<voidResponse>(newvoid);
            Assert.AreEqual("000", voidResponse.response);
            Assert.AreEqual("Approved", voidResponse.message);
        }

        [Test]
        public void Test5AVS()
        {
            var authorization = new authorization();
            authorization.id = "1";
            authorization.orderId = "5";
            authorization.amount = 0;
            authorization.orderSource = orderSourceType.ecommerce;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.VI;
            card.number = "4457010200000007";
            card.expDate = "0512";
            card.cardValidationNum = "463";
            authorization.card = card;
            var authenticationvalue = new fraudCheckType();
            authenticationvalue.authenticationValue = "BwABBJQ1AgAAAAAgJDUCAAAAAAA=";
            authorization.cardholderAuthentication = authenticationvalue;

            var response = this.SendTransaction<authorizationResponse>(authorization);
            Assert.AreEqual("000", response.response);
            Assert.AreEqual("Approved", response.message);
            Assert.AreEqual("55555 ", response.authCode);
            Assert.AreEqual("32", response.fraudResult.avsResult);
            Assert.AreEqual("M", response.fraudResult.cardValidationResult);
        }

        [Test]
        public void Test5Sale()
        {
            var sale = new sale();
            sale.id = "1";
            sale.orderId = "5";
            sale.amount = 50050;
            sale.orderSource = orderSourceType.ecommerce;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.VI;
            card.number = "4457010200000007";
            card.expDate = "0512";
            card.cardValidationNum = "463";
            sale.card = card;
            var authenticationvalue = new fraudCheckType();
            authenticationvalue.authenticationValue = "BwABBJQ1AgAAAAAgJDUCAAAAAAA=";
            sale.cardholderAuthentication = authenticationvalue;

            var response = this.SendTransaction<saleResponse>(sale);
            Assert.AreEqual("000", response.response);
            Assert.AreEqual("Approved", response.message);
            Assert.AreEqual("55555 ", response.authCode);
            Assert.AreEqual("32", response.fraudResult.avsResult);
            Assert.AreEqual("M", response.fraudResult.cardValidationResult);

            var credit = new credit();
            credit.id = response.id;
            credit.cnpTxnId = response.cnpTxnId;
            var creditResponse = this.SendTransaction<creditResponse>(credit);
            Assert.AreEqual("000", creditResponse.response);
            Assert.AreEqual("Approved", creditResponse.message);

            var newvoid = new voidTxn();
            newvoid.id = creditResponse.id;
            newvoid.cnpTxnId = creditResponse.cnpTxnId;
            var voidResponse = this.SendTransaction<voidResponse>(newvoid);
            Assert.AreEqual("000", voidResponse.response);
            Assert.AreEqual("Approved", voidResponse.message);
        }

        [Test]
        public void Test6Auth()
        {
            var authorization = new authorization();
            authorization.id = "1";
            authorization.orderId = "6";
            authorization.amount = 60060;
            authorization.orderSource = orderSourceType.ecommerce;
            var contact = new contact();
            contact.name = "Joe Green";
            contact.addressLine1 = "6 Main St.";
            contact.city = "Derry";
            contact.state = "NH";
            contact.zip = "03038";
            contact.country = countryTypeEnum.US;
            authorization.billToAddress = contact;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.VI;
            card.number = "4457010100000008";
            card.expDate = "0612";
            card.cardValidationNum = "992";
            authorization.card = card;

            var response = this.SendTransaction<authorizationResponse>(authorization);
            Assert.AreEqual("110", response.response);
            Assert.AreEqual("Insufficient Funds", response.message);
            Assert.AreEqual("34", response.fraudResult.avsResult);
            Assert.AreEqual("P", response.fraudResult.cardValidationResult);
        }

        [Test]
        public void Test6Sale()
        {
            var sale = new sale();
            sale.id = "1";
            sale.orderId = "6";
            sale.amount = 60060;
            sale.orderSource = orderSourceType.ecommerce;
            var contact = new contact();
            contact.name = "Joe Green";
            contact.addressLine1 = "6 Main St.";
            contact.city = "Derry";
            contact.state = "NH";
            contact.zip = "03038";
            contact.country = countryTypeEnum.US;
            sale.billToAddress = contact;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.VI;
            card.number = "4457010100000008";
            card.expDate = "0612";
            card.cardValidationNum = "992";
            sale.card = card;

            var response = this.SendTransaction<saleResponse>(sale);
            Assert.AreEqual("110", response.response);
            Assert.AreEqual("Insufficient Funds", response.message);
            Assert.AreEqual("34", response.fraudResult.avsResult);
            Assert.AreEqual("P", response.fraudResult.cardValidationResult);

            var newvoid = new voidTxn();
            newvoid.id = response.id;
            newvoid.cnpTxnId = response.cnpTxnId;
            var voidResponse = this.SendTransaction<voidResponse>(newvoid);
            Assert.AreEqual("000", voidResponse.response);
            Assert.AreEqual("Approved", voidResponse.message);
        }

        [Test]
        public void Test7Auth()
        {
            var authorization = new authorization();
            authorization.id = "1";
            authorization.orderId = "7";
            authorization.amount = 70070;
            authorization.orderSource = orderSourceType.ecommerce;
            var contact = new contact();
            contact.name = "Jane Murray";
            contact.addressLine1 = "7 Main St.";
            contact.city = "Amesbury";
            contact.state = "MA";
            contact.zip = "01913";
            contact.country = countryTypeEnum.US;
            authorization.billToAddress = contact;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.MC;
            card.number = "5112010100000002";
            card.expDate = "0712";
            card.cardValidationNum = "251";
            authorization.card = card;

            var response = this.SendTransaction<authorizationResponse>(authorization);
            Assert.AreEqual("301", response.response);
            Assert.AreEqual("Invalid Account Number", response.message);
            Assert.AreEqual("34", response.fraudResult.avsResult);
            Assert.AreEqual("N", response.fraudResult.cardValidationResult);
        }

        [Test]
        public void Test7AVS()
        {
            var authorization = new authorization();
            authorization.id = "1";
            authorization.orderId = "7";
            authorization.amount = 0;
            authorization.orderSource = orderSourceType.ecommerce;
            var contact = new contact();
            contact.name = "Jane Murray";
            contact.addressLine1 = "7 Main St.";
            contact.city = "Amesbury";
            contact.state = "MA";
            contact.zip = "01913";
            contact.country = countryTypeEnum.US;
            authorization.billToAddress = contact;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.MC;
            card.number = "5112010100000002";
            card.expDate = "0712";
            card.cardValidationNum = "251";
            authorization.card = card;

            var response = this.SendTransaction<authorizationResponse>(authorization);
            Assert.AreEqual("301", response.response);
            Assert.AreEqual("Invalid Account Number", response.message);
            Assert.AreEqual("34", response.fraudResult.avsResult);
            Assert.AreEqual("N", response.fraudResult.cardValidationResult);
        }

        [Test]
        public void Test7Sale()
        {
            var sale = new sale();
            sale.id = "1";
            sale.orderId = "7";
            sale.amount = 70070;
            sale.orderSource = orderSourceType.ecommerce;
            var contact = new contact();
            contact.name = "Jane Murray";
            contact.addressLine1 = "7 Main St.";
            contact.city = "Amesbury";
            contact.state = "MA";
            contact.zip = "01913";
            contact.country = countryTypeEnum.US;
            sale.billToAddress = contact;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.MC;
            card.number = "5112010100000002";
            card.expDate = "0712";
            card.cardValidationNum = "251";
            sale.card = card;

            var response = this.SendTransaction<saleResponse>(sale);
            Assert.AreEqual("301", response.response);
            Assert.AreEqual("Invalid Account Number", response.message);
            Assert.AreEqual("34", response.fraudResult.avsResult);
            Assert.AreEqual("N", response.fraudResult.cardValidationResult);
        }

        [Test]
        public void Test8Auth()
        {
            var authorization = new authorization();
            authorization.id = "1";
            authorization.orderId = "8";
            authorization.amount = 80080;
            authorization.orderSource = orderSourceType.ecommerce;
            var contact = new contact();
            contact.name = "Mark Johnson";
            contact.addressLine1 = "8 Main St.";
            contact.city = "Manchester";
            contact.state = "NH";
            contact.zip = "03101";
            contact.country = countryTypeEnum.US;
            authorization.billToAddress = contact;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.DI;
            card.number = "6011010100000002";
            card.expDate = "0812";
            card.cardValidationNum = "184";
            authorization.card = card;

            var response = this.SendTransaction<authorizationResponse>(authorization);
            Assert.AreEqual("123", response.response);
            Assert.AreEqual("Call Discover", response.message);
            Assert.AreEqual("34", response.fraudResult.avsResult);
            Assert.AreEqual("P", response.fraudResult.cardValidationResult);
        }

        [Test]
        public void Test8AVS()
        {
            var authorization = new authorization();
            authorization.id = "1";
            authorization.orderId = "8";
            authorization.amount = 0;
            authorization.orderSource = orderSourceType.ecommerce;
            var contact = new contact();
            contact.name = "Mark Johnson";
            contact.addressLine1 = "8 Main St.";
            contact.city = "Manchester";
            contact.state = "NH";
            contact.zip = "03101";
            contact.country = countryTypeEnum.US;
            authorization.billToAddress = contact;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.DI;
            card.number = "6011010100000002";
            card.expDate = "0812";
            card.cardValidationNum = "184";
            authorization.card = card;

            var response = this.SendTransaction<authorizationResponse>(authorization);
            Assert.AreEqual("123", response.response);
            Assert.AreEqual("Call Discover", response.message);
            Assert.AreEqual("34", response.fraudResult.avsResult);
            Assert.AreEqual("P", response.fraudResult.cardValidationResult);
        }

        [Test]
        public void Test8Sale()
        {
            var sale = new sale();
            sale.id = "1";
            sale.orderId = "8";
            sale.amount = 80080;
            sale.orderSource = orderSourceType.ecommerce;
            var contact = new contact();
            contact.name = "Mark Johnson";
            contact.addressLine1 = "8 Main St.";
            contact.city = "Manchester";
            contact.state = "NH";
            contact.zip = "03101";
            contact.country = countryTypeEnum.US;
            sale.billToAddress = contact;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.DI;
            card.number = "6011010100000002";
            card.expDate = "0812";
            card.cardValidationNum = "184";
            sale.card = card;

            var response = this.SendTransaction<saleResponse>(sale);
            Assert.AreEqual("123", response.response);
            Assert.AreEqual("Call Discover", response.message);
            Assert.AreEqual("34", response.fraudResult.avsResult);
            Assert.AreEqual("P", response.fraudResult.cardValidationResult);
        }

        [Test]
        public void Test9Auth()
        {
            var authorization = new authorization();
            authorization.id = "1";
            authorization.orderId = "9";
            authorization.amount = 90090;
            authorization.orderSource = orderSourceType.ecommerce;
            var contact = new contact();
            contact.name = "James Miller";
            contact.addressLine1 = "9 Main St.";
            contact.city = "Boston";
            contact.state = "MA";
            contact.zip = "02134";
            contact.country = countryTypeEnum.US;
            authorization.billToAddress = contact;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.AX;
            card.number = "375001010000003";
            card.expDate = "0912";
            card.cardValidationNum = "0421";
            authorization.card = card;

            var response = this.SendTransaction<authorizationResponse>(authorization);
            Assert.AreEqual("303", response.response);
            Assert.AreEqual("Pick Up Card", response.message);
            Assert.AreEqual("34", response.fraudResult.avsResult);
        }

        [Test]
        public void Test9AVS()
        {
            var authorization = new authorization();
            authorization.id = "1";
            authorization.orderId = "9";
            authorization.amount = 0;
            authorization.orderSource = orderSourceType.ecommerce;
            var contact = new contact();
            contact.name = "James Miller";
            contact.addressLine1 = "9 Main St.";
            contact.city = "Boston";
            contact.state = "MA";
            contact.zip = "02134";
            contact.country = countryTypeEnum.US;
            authorization.billToAddress = contact;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.AX;
            card.number = "375001010000003";
            card.expDate = "0912";
            card.cardValidationNum = "0421";
            authorization.card = card;

            var response = this.SendTransaction<authorizationResponse>(authorization);
            Assert.AreEqual("303", response.response);
            Assert.AreEqual("Pick Up Card", response.message);
            Assert.AreEqual("34", response.fraudResult.avsResult);
        }

        [Test]
        public void Test9Sale()
        {
            var sale = new sale();
            sale.id = "1";
            sale.orderId = "9";
            sale.amount = 90090;
            sale.orderSource = orderSourceType.ecommerce;
            var contact = new contact();
            contact.name = "James Miller";
            contact.addressLine1 = "9 Main St.";
            contact.city = "Boston";
            contact.state = "MA";
            contact.zip = "02134";
            contact.country = countryTypeEnum.US;
            sale.billToAddress = contact;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.AX;
            card.number = "375001010000003";
            card.expDate = "0912";
            card.cardValidationNum = "0421";
            sale.card = card;

            var response = this.SendTransaction<saleResponse>(sale);
            Assert.AreEqual("303", response.response);
            Assert.AreEqual("Pick Up Card", response.message);
            Assert.AreEqual("34", response.fraudResult.avsResult);
        }

        [Test]
        public void Test10()
        {
            var authorization = new authorization();
            authorization.id = "1";
            authorization.orderId = "10";
            authorization.amount = 40000;
            authorization.orderSource = orderSourceType.ecommerce;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.VI;
            card.number = "4457010140000141";
            card.expDate = "0912";
            authorization.card = card;
            authorization.allowPartialAuth = true;

            var response = this.SendTransaction<authorizationResponse>(authorization);
            Assert.AreEqual("010", response.response);
            Assert.AreEqual("Partially Approved", response.message);
            Assert.AreEqual(32000, response.approvedAmount);
        }

        [Test]
        public void Test11()
        {
            var authorization = new authorization();
            authorization.id = "1";
            authorization.orderId = "11";
            authorization.amount = 60000;
            authorization.orderSource = orderSourceType.ecommerce;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.MC;
            card.number = "5112010140000004";
            card.expDate = "1111";
            authorization.card = card;
            authorization.allowPartialAuth = true;

            var response = this.SendTransaction<authorizationResponse>(authorization);
            Assert.AreEqual("010", response.response);
            Assert.AreEqual("Partially Approved", response.message);
            Assert.AreEqual(48000, response.approvedAmount);
        }

        [Test]
        public void Test12()
        {
            var authorization = new authorization();
            authorization.id = "1";
            authorization.orderId = "12";
            authorization.amount = 50000;
            authorization.orderSource = orderSourceType.ecommerce;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.AX;
            card.number = "375001014000009";
            card.expDate = "0412";
            authorization.card = card;
            authorization.allowPartialAuth = true;

            var response = this.SendTransaction<authorizationResponse>(authorization);
            Assert.AreEqual("010", response.response);
            Assert.AreEqual("Partially Approved", response.message);
            Assert.AreEqual(40000, response.approvedAmount);
        }

        [Test]
        public void Test13()
        {
            var authorization = new authorization();
            authorization.id = "1";
            authorization.orderId = "13";
            authorization.amount = 15000;
            authorization.orderSource = orderSourceType.ecommerce;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.DI;
            card.number = "6011010140000004";
            card.expDate = "0812";
            authorization.card = card;
            authorization.allowPartialAuth = true;

            var response = this.SendTransaction<authorizationResponse>(authorization);
            Assert.AreEqual("010", response.response);
            Assert.AreEqual("Partially Approved", response.message);
            Assert.AreEqual(12000, response.approvedAmount);

        }
    }
}