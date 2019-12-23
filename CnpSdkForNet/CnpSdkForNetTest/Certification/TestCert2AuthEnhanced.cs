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
    public class TestCert2AuthEnhanced : BaseCertificationOnlineTest
    {
        [Test]
        public void Test14()
        {
            var authorization = new authorization();
            authorization.id = "1";
            authorization.orderId = "14";
            authorization.amount = 10100;
            authorization.orderSource = orderSourceType.ecommerce;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.VI;
            card.number = "4457010200000247";
            card.expDate = "0821";
            authorization.card = card;

            var response = this.SendTransaction<authorizationResponse>(authorization);
            Assert.AreEqual("000", response.response);
            Assert.AreEqual("Approved", response.message);
           //TODO: // Need to find why enhanced auth response is not generated for this merchant. Probably config issue
            Assert.AreEqual(fundingSourceTypeEnum.PREPAID, response.enhancedAuthResponse.fundingSource.type);
            Assert.AreEqual("2000", response.enhancedAuthResponse.fundingSource.availableBalance);
            Assert.AreEqual("NO", response.enhancedAuthResponse.fundingSource.reloadable);
            Assert.AreEqual("GIFT", response.enhancedAuthResponse.fundingSource.prepaidCardType);
        }

        [Test]
        public void Test15()
        {
            var authorization = new authorization();
            authorization.id = "1";
            authorization.orderId = "15";
            authorization.amount = 3000;
            authorization.orderSource = orderSourceType.ecommerce;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.MC;
            card.number = "5500000254444445";
            card.expDate = "0312";
            authorization.card = card;

            var response = this.SendTransaction<authorizationResponse>(authorization);
            Assert.AreEqual("000", response.response);
            Assert.AreEqual("Approved", response.message);
            //TODO: // Need to find why enhanced auth response is not generated for this merchant. Probably config issue
            Assert.AreEqual(fundingSourceTypeEnum.PREPAID, response.enhancedAuthResponse.fundingSource.type);
            Assert.AreEqual("2000", response.enhancedAuthResponse.fundingSource.availableBalance);
            Assert.AreEqual("YES", response.enhancedAuthResponse.fundingSource.reloadable);
            Assert.AreEqual("PAYROLL", response.enhancedAuthResponse.fundingSource.prepaidCardType);
        }

        [Test]
        public void Test16()
        {
            var authorization = new authorization();
            authorization.id = "1";
            authorization.orderId = "16";
            authorization.amount = 3000;
            authorization.orderSource = orderSourceType.ecommerce;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.MC;
            card.number = "5592106621450897";
            card.expDate = "0312";
            authorization.card = card;

            var response = this.SendTransaction<authorizationResponse>(authorization);
            Assert.AreEqual("000", response.response);
            Assert.AreEqual("Approved", response.message);
            //TODO: // Need to find why enhanced auth response is not generated for this merchant. Probably config issue
            Assert.AreEqual(fundingSourceTypeEnum.PREPAID, response.enhancedAuthResponse.fundingSource.type);
            Assert.AreEqual("0", response.enhancedAuthResponse.fundingSource.availableBalance);
            Assert.AreEqual("YES", response.enhancedAuthResponse.fundingSource.reloadable);
            Assert.AreEqual("PAYROLL", response.enhancedAuthResponse.fundingSource.prepaidCardType);
        }

        [Test]
        public void Test17()
        {
            var authorization = new authorization();
            authorization.id = "1";
            authorization.orderId = "17";
            authorization.amount = 3000;
            authorization.orderSource = orderSourceType.ecommerce;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.MC;
            card.number = "5590409551104142";
            card.expDate = "0312";
            authorization.card = card;

            var response = this.SendTransaction<authorizationResponse>(authorization);
            Assert.AreEqual("000", response.response);
            Assert.AreEqual("Approved", response.message);
            //TODO: // Need to find why enhanced auth response is not generated for this merchant. Probably config issue
            Assert.AreEqual(fundingSourceTypeEnum.PREPAID, response.enhancedAuthResponse.fundingSource.type);
            Assert.AreEqual("6500", response.enhancedAuthResponse.fundingSource.availableBalance);
            Assert.AreEqual("YES", response.enhancedAuthResponse.fundingSource.reloadable);
            Assert.AreEqual("PAYROLL", response.enhancedAuthResponse.fundingSource.prepaidCardType);
        }

        [Test]
        public void Test18()
        {
            var authorization = new authorization();
            authorization.id = "1";
            authorization.orderId = "18";
            authorization.amount = 3000;
            authorization.orderSource = orderSourceType.ecommerce;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.MC;
            card.number = "5587755665222179";
            card.expDate = "0312";
            authorization.card = card;

            var response = this.SendTransaction<authorizationResponse>(authorization);
            Assert.AreEqual("000", response.response);
            Assert.AreEqual("Approved", response.message);
            //TODO: // Need to find why enhanced auth response is not generated for this merchant. Probably config issue
            Assert.AreEqual(fundingSourceTypeEnum.PREPAID, response.enhancedAuthResponse.fundingSource.type);
            Assert.AreEqual("12200", response.enhancedAuthResponse.fundingSource.availableBalance);
            Assert.AreEqual("YES", response.enhancedAuthResponse.fundingSource.reloadable);
            Assert.AreEqual("PAYROLL", response.enhancedAuthResponse.fundingSource.prepaidCardType);
        }

        [Test]
        public void Test19()
        {
            var authorization = new authorization();
            authorization.id = "1";
            authorization.orderId = "19";
            authorization.amount = 3000;
            authorization.orderSource = orderSourceType.ecommerce;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.MC;
            card.number = "5445840176552850";
            card.expDate = "0312";
            authorization.card = card;

            var response = this.SendTransaction<authorizationResponse>(authorization);
            Assert.AreEqual("000", response.response);
            Assert.AreEqual("Approved", response.message);
            //TODO: // Need to find why enhanced auth response is not generated for this merchant. Probably config issue
            Assert.AreEqual(fundingSourceTypeEnum.PREPAID, response.enhancedAuthResponse.fundingSource.type);
            Assert.AreEqual("20000", response.enhancedAuthResponse.fundingSource.availableBalance);
            Assert.AreEqual("YES", response.enhancedAuthResponse.fundingSource.reloadable);
            Assert.AreEqual("PAYROLL", response.enhancedAuthResponse.fundingSource.prepaidCardType);
        }

        [Test]
        public void Test20()
        {
            var authorization = new authorization();
            authorization.id = "1";
            authorization.orderId = "20";
            authorization.amount = 3000;
            authorization.orderSource = orderSourceType.ecommerce;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.MC;
            card.number = "5390016478904678";
            card.expDate = "0312";
            authorization.card = card;

            var response = this.SendTransaction<authorizationResponse>(authorization);
            Assert.AreEqual("000", response.response);
            Assert.AreEqual("Approved", response.message);
            //TODO: // Need to find why enhanced auth response is not generated for this merchant. Probably config issue
            Assert.AreEqual(fundingSourceTypeEnum.PREPAID, response.enhancedAuthResponse.fundingSource.type);
            Assert.AreEqual("10050", response.enhancedAuthResponse.fundingSource.availableBalance);
            Assert.AreEqual("YES", response.enhancedAuthResponse.fundingSource.reloadable);
            Assert.AreEqual("PAYROLL", response.enhancedAuthResponse.fundingSource.prepaidCardType);
        }

        [Test]
        public void Test21()
        {
            var authorization = new authorization();
            authorization.id = "1";
            authorization.orderId = "21";
            authorization.amount = 5000;
            authorization.orderSource = orderSourceType.ecommerce;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.VI;
            card.number = "4457010201000246";
            card.expDate = "0912";
            authorization.card = card;

            var response = this.SendTransaction<authorizationResponse>(authorization);
            Assert.AreEqual("000", response.response);
            Assert.AreEqual("Approved", response.message);
            //TODO: // Need to find why enhanced auth response is not generated for this merchant. Probably config issue
            Assert.AreEqual(affluenceTypeEnum.AFFLUENT, response.enhancedAuthResponse.affluence);
        }

        [Test]
        public void Test22()
        {
            var authorization = new authorization();
            authorization.id = "1";
            authorization.orderId = "22";
            authorization.amount = 5000;
            authorization.orderSource = orderSourceType.ecommerce;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.VI;
            card.number = "4457010202000245";
            card.expDate = "1111";
            authorization.card = card;

            var response = this.SendTransaction<authorizationResponse>(authorization);
            Assert.AreEqual("000", response.response);
            Assert.AreEqual("Approved", response.message);
            //TODO: // Need to find why enhanced auth response is not generated for this merchant. Probably config issue
            Assert.AreEqual(affluenceTypeEnum.MASSAFFLUENT, response.enhancedAuthResponse.affluence);
        }

        [Test]
        public void Test23()
        {
            var authorization = new authorization();
            authorization.id = "1";
            authorization.orderId = "23";
            authorization.amount = 5000;
            authorization.orderSource = orderSourceType.ecommerce;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.MC;
            card.number = "5112010201000109";
            card.expDate = "0412";
            authorization.card = card;

            var response = this.SendTransaction<authorizationResponse>(authorization);
            Assert.AreEqual("000", response.response);
            Assert.AreEqual("Approved", response.message);
            //TODO: // Need to find why enhanced auth response is not generated for this merchant. Probably config issue
            Assert.AreEqual(affluenceTypeEnum.AFFLUENT, response.enhancedAuthResponse.affluence);

        }

        [Test]
        public void Test24()
        {
            var authorization = new authorization();
            authorization.id = "1";
            authorization.orderId = "24";
            authorization.amount = 5000;
            authorization.orderSource = orderSourceType.ecommerce;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.MC;
            card.number = "5112010202000108";
            card.expDate = "0812";
            authorization.card = card;

            var response = this.SendTransaction<authorizationResponse>(authorization);
            Assert.AreEqual("000", response.response);
            Assert.AreEqual("Approved", response.message);
            //TODO: // Need to find why enhanced auth response is not generated for this merchant. Probably config issue
            Assert.AreEqual(affluenceTypeEnum.MASSAFFLUENT, response.enhancedAuthResponse.affluence);

        }

        [Test]
        public void Test25()
        {
            var authorization = new authorization();
            authorization.id = "1";
            authorization.orderId = "25";
            authorization.amount = 5000;
            authorization.orderSource = orderSourceType.ecommerce;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.VI;
            card.number = "4100204446270000";
            card.expDate = "1112";
            authorization.card = card;

            var response = this.SendTransaction<authorizationResponse>(authorization);
            Assert.AreEqual("000", response.response);
            Assert.AreEqual("Approved", response.message);
            //TODO: // Need to find why enhanced auth response is not generated for this merchant. Probably config issue
            Assert.AreEqual("BRA", response.enhancedAuthResponse.issuerCountry);

        }

         //Excluding health care related tests.
        [Test]
        public void Test26()
        {
            var authorization = new authorization();
            authorization.id = "1";
            authorization.orderId = "26";
            authorization.amount = 18698;
            authorization.orderSource = orderSourceType.ecommerce;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.MC;
            card.number = "5194560012341234";
            card.expDate = "1212";
            authorization.card = card;
            authorization.allowPartialAuth = true;
            var healthcareiias = new healthcareIIAS();
            var healthcareamounts = new healthcareAmounts();
            healthcareamounts.totalHealthcareAmount = 20000;
            healthcareiias.healthcareAmounts = healthcareamounts;
            healthcareiias.IIASFlag = IIASFlagType.Y;
            authorization.healthcareIIAS = healthcareiias;

            var response = this.SendTransaction<authorizationResponse>(authorization);
            Assert.AreEqual("341", response.response);
         }

        [Test]
        public void Test27()
        {
            var authorization = new authorization();
            authorization.id = "1";
            authorization.orderId = "27";
            authorization.amount = 18698;
            authorization.orderSource = orderSourceType.ecommerce;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.MC;
            card.number = "5194560012341234";
            card.expDate = "1212";
            authorization.card = card;
            authorization.allowPartialAuth = true;
            var healthcareiias = new healthcareIIAS();
            var healthcareamounts = new healthcareAmounts();
            healthcareamounts.totalHealthcareAmount = 15000;
            healthcareamounts.RxAmount = 16000;
            healthcareiias.healthcareAmounts = healthcareamounts;
            healthcareiias.IIASFlag = IIASFlagType.Y;
            authorization.healthcareIIAS = healthcareiias;

            var response = this.SendTransaction<authorizationResponse>(authorization);
            Assert.AreEqual("341", response.response);
        }

        [Test]
        public void Test28()
        {
            var authorization = new authorization();
            authorization.id = "1";
            authorization.orderId = "28";
            authorization.amount = 15000;
            authorization.orderSource = orderSourceType.ecommerce;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.MC;
            card.number = "5194560012341234";
            card.expDate = "1212";
            authorization.card = card;
            authorization.allowPartialAuth = true;
            var healthcareiias = new healthcareIIAS();
            var healthcareamounts = new healthcareAmounts();
            healthcareamounts.totalHealthcareAmount = 15000;
            healthcareamounts.RxAmount = 3698;
            healthcareiias.healthcareAmounts = healthcareamounts;
            healthcareiias.IIASFlag = IIASFlagType.Y;
            authorization.healthcareIIAS = healthcareiias;

            var response = this.SendTransaction<authorizationResponse>(authorization);
            Assert.AreEqual("000", response.response);
            Assert.AreEqual("Approved", response.message);
        }

        [Test]
        public void Test29()
        {
            var authorization = new authorization();
            authorization.id = "1";
            authorization.orderId = "29";
            authorization.amount = 18699;
            authorization.orderSource = orderSourceType.ecommerce;
            var card = new cardType();
            card.type = methodOfPaymentTypeEnum.VI;
            card.number = "4024720001231239";
            card.expDate = "1212";
            authorization.card = card;
            authorization.allowPartialAuth = true;
            var healthcareiias = new healthcareIIAS();
            var healthcareamounts = new healthcareAmounts();
            healthcareamounts.totalHealthcareAmount = 31000;
            healthcareamounts.RxAmount = 1000;
            healthcareamounts.visionAmount = 19901;
            healthcareamounts.clinicOtherAmount = 9050;
            healthcareamounts.dentalAmount = 1049;
            healthcareiias.healthcareAmounts = healthcareamounts;
            healthcareiias.IIASFlag = IIASFlagType.Y;
            authorization.healthcareIIAS = healthcareiias;

            var response = this.SendTransaction<authorizationResponse>(authorization);
            Assert.AreEqual("341", response.response);
        }
    }
}