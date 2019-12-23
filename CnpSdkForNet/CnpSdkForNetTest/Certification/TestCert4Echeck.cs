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
    public class TestCert4Echeck : BaseCertificationOnlineTest
    {
        [Test]
        public void Test37()
        {
            var verification = new echeckVerification();
            verification.id = "1";
            verification.orderId = "37";
            verification.amount = 3001;
            verification.orderSource = orderSourceType.telephone;
            var billToAddress = new billToAddress();
            billToAddress.firstName = "Tom";
            billToAddress.lastName = "Black";
            verification.billToAddress = billToAddress;
            var echeck = new echeckType();
            echeck.accNum = "10@BC99999";
            echeck.accType = echeckAccountTypeEnum.Checking;
            echeck.routingNum = "053100300";
            verification.echeck = echeck;

            var response = this.SendTransaction<echeckVerificationResponse>(verification);
            Assert.AreEqual("301", response.response);
            Assert.AreEqual("Invalid Account Number", response.message);
        }

        [Test]
        public void Test38()
        {
            var verification = new echeckVerification();
            verification.id = "1";
            verification.orderId = "38";
            verification.amount = 3002;
            verification.orderSource = orderSourceType.telephone;
            var billToAddress = new billToAddress();
            billToAddress.firstName = "John";
            billToAddress.lastName = "Smith";
            billToAddress.phone = "999-999-9999";
            verification.billToAddress = billToAddress;
            var echeck = new echeckType();
            echeck.accNum = "1099999999";
            echeck.accType = echeckAccountTypeEnum.Checking;
            echeck.routingNum = "053000219";
            verification.echeck = echeck;

            var response = this.SendTransaction<echeckVerificationResponse>(verification);
            Assert.AreEqual("000", response.response);
            Assert.AreEqual("Approved", response.message);
        }

        [Test]
        public void Test39()
        {
            var verification = new echeckVerification();
            verification.id = "1";
            verification.orderId = "39";
            verification.amount = 3003;
            verification.orderSource = orderSourceType.telephone;
            var billToAddress = new billToAddress();
            billToAddress.firstName = "Robert";
            billToAddress.lastName = "Jones";
            billToAddress.companyName = "Good Goods Inc";
            billToAddress.phone = "9999999999";
            verification.billToAddress = billToAddress;
            var echeck = new echeckType();
            echeck.accNum = "3099999999";
            echeck.accType = echeckAccountTypeEnum.Corporate;
            echeck.routingNum = "053100300";
            verification.echeck = echeck;

            var response = this.SendTransaction<echeckVerificationResponse>(verification);
            Assert.AreEqual("950", response.response);
        }

        [Test]
        public void Test40()
        {
            var verification = new echeckVerification();
            verification.id = "1";
            verification.orderId = "40";
            verification.amount = 3004;
            verification.orderSource = orderSourceType.telephone;
            var billToAddress = new billToAddress();
            billToAddress.firstName = "Peter";
            billToAddress.lastName = "Green";
            billToAddress.companyName = "Green Co";
            billToAddress.phone = "9999999999";
            verification.billToAddress = billToAddress;
            var echeck = new echeckType();
            echeck.accNum = "8099999999";
            echeck.accType = echeckAccountTypeEnum.Corporate;
            echeck.routingNum = "063102152";
            verification.echeck = echeck;

            var response = this.SendTransaction<echeckVerificationResponse>(verification);
            Assert.AreEqual("951", response.response);
            Assert.AreEqual("Absolute Decline", response.message);
        }

        [Test]
        public void Test41()
        {
            var sale = new echeckSale();
            sale.id = "1";
            sale.orderId = "41";
            sale.amount = 2008;
            sale.orderSource = orderSourceType.telephone;
            var billToAddress = new billToAddress();
            billToAddress.firstName = "Mike";
            billToAddress.middleInitial = "J";
            billToAddress.lastName = "Hammer";
            sale.billToAddress = billToAddress;
            var echeck = new echeckType();
            echeck.accNum = "10@BC99999";
            echeck.accType = echeckAccountTypeEnum.Checking;
            echeck.routingNum = "053100300";
            sale.echeck = echeck;

            var response = this.SendTransaction<echeckSalesResponse>(sale);
            Assert.AreEqual("301", response.response);
            Assert.AreEqual("Invalid Account Number", response.message);
        }

        [Test]
        public void Test42()
        {
            var sale = new echeckSale();
            sale.id = "1";
            sale.orderId = "42";
            sale.amount = 2004;
            sale.orderSource = orderSourceType.telephone;
            var billToAddress = new billToAddress();
            billToAddress.firstName = "Tom";
            billToAddress.lastName = "Black";
            sale.billToAddress = billToAddress;
            var echeck = new echeckType();
            echeck.accNum = "4099999992";
            echeck.accType = echeckAccountTypeEnum.Checking;
            echeck.routingNum = "211370545";
            sale.echeck = echeck;

            var response = this.SendTransaction<echeckSalesResponse>(sale);
            Assert.AreEqual("000", response.response);
            Assert.AreEqual("Approved", response.message);
        }

        [Test]
        public void Test43()
        {
            var sale = new echeckSale();
            sale.id = "1";
            sale.orderId = "43";
            sale.amount = 2007;
            sale.orderSource = orderSourceType.telephone;
            var billToAddress = new billToAddress();
            billToAddress.firstName = "Peter";
            billToAddress.lastName = "Green";
            billToAddress.companyName = "Green Co";
            sale.billToAddress = billToAddress;
            var echeck = new echeckType();
            echeck.accNum = "6099999992";
            echeck.accType = echeckAccountTypeEnum.Corporate;
            echeck.routingNum = "211370545";
            sale.echeck = echeck;

            var response = this.SendTransaction<echeckSalesResponse>(sale);
            Assert.AreEqual("000", response.response);
            Assert.AreEqual("Approved", response.message);
        }

        [Test]
        public void Test44()
        {
            var sale = new echeckSale();
            sale.id = "1";
            sale.orderId = "44";
            sale.amount = 2009;
            sale.orderSource = orderSourceType.telephone;
            var billToAddress = new billToAddress();
            billToAddress.firstName = "Peter";
            billToAddress.lastName = "Green";
            billToAddress.companyName = "Green Co";
            sale.billToAddress = billToAddress;
            var echeck = new echeckType();
            echeck.accNum = "9099999992";
            echeck.accType = echeckAccountTypeEnum.Corporate;
            echeck.routingNum = "053133052";
            sale.echeck = echeck;

            var response = this.SendTransaction<echeckSalesResponse>(sale);
            Assert.AreEqual("900", response.response);
            Assert.AreEqual("Invalid Bank Routing Number", response.message);
        }

        [Test]
        public void Test45()
        {
            var credit = new echeckCredit();
            credit.id = "1";
            credit.orderId = "45";
            credit.amount = 1001;
            credit.orderSource = orderSourceType.telephone;
            var billToAddress = new billToAddress();
            billToAddress.firstName = "John";
            billToAddress.lastName = "Smith";
            credit.billToAddress = billToAddress;
            var echeck = new echeckType();
            echeck.accNum = "10@BC99999";
            echeck.accType = echeckAccountTypeEnum.Checking;
            echeck.routingNum = "053100300";
            credit.echeck = echeck;

            var response = this.SendTransaction<echeckCreditResponse>(credit);
            Assert.AreEqual("301", response.response);
        }

        [Test]
        public void Test46()
        {
            var credit = new echeckCredit();
            credit.id = "1";
            credit.orderId = "46";
            credit.amount = 1003;
            credit.orderSource = orderSourceType.telephone;
            var billToAddress = new billToAddress();
            billToAddress.firstName = "Robert";
            billToAddress.lastName = "Jones";
            billToAddress.companyName = "Widget Inc";
            credit.billToAddress = billToAddress;
            var echeck = new echeckType();
            echeck.accNum = "3099999999";
            echeck.accType = echeckAccountTypeEnum.Corporate;
            echeck.routingNum = "063102152";
            credit.echeck = echeck;

            var response = this.SendTransaction<echeckCreditResponse>(credit);
            Assert.AreEqual("000", response.response);
            Assert.AreEqual("Approved", response.message);
        }

        [Test]
        public void Test47()
        {
            var credit = new echeckCredit();
            credit.id = "1";
            credit.orderId = "47";
            credit.amount = 1007;
            credit.orderSource = orderSourceType.telephone;
            var billToAddress = new billToAddress();
            billToAddress.firstName = "Peter";
            billToAddress.lastName = "Green";
            billToAddress.companyName = "Green Co";
            credit.billToAddress = billToAddress;
            var echeck = new echeckType();
            echeck.accNum = "6099999993";
            echeck.accType = echeckAccountTypeEnum.Corporate;
            echeck.routingNum = "211370545";
            credit.echeck = echeck;

            var response = this.SendTransaction<echeckCreditResponse>(credit);
            Assert.AreEqual("000", response.response);
            Assert.AreEqual("Approved", response.message);
        }

        [Test]
        public void Test48()
        {
            var credit = new echeckCredit();
            credit.id = "1";
            credit.cnpTxnId = 430000000000000001L;

            var response = this.SendTransaction<echeckCreditResponse>(credit);
            Assert.AreEqual("000", response.response);
            Assert.AreEqual("Approved", response.message);
        }

        [Test]
        public void Test49()
        {
            var credit = new echeckCredit();
            credit.id = "1";
            credit.cnpTxnId = 2L;

            var response = this.SendTransaction<echeckCreditResponse>(credit);
            Assert.AreEqual("000", response.response);
        }
    }
}