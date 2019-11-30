/*
 * Zachary Cook
 * 
 * Fields for XML requests. Refer to the XML reference guides
 * for further documentation.
 *
 * TODO: Need to convert DateTime string
 */

using System;
using System.Collections.Generic;
using System.Security;
using Cnp.Sdk.VersionedXML;

namespace Cnp.Sdk
{
    [XMLElement(Name = "cnpOnlineRequest", FirstVersion = "12.0")]
    [XMLElement(Name = "litleOnlineRequest", RemovedVersion = "12.0")]
    public class cnpOnlineRequest : VersionedXMLElement
    {
        [XMLAttribute(Name = "merchantId")]
        public string merchantId { get; set; }
        
        [XMLAttribute(Name = "version")]
        public XMLVersion version { get; set; }
        
        [XMLAttribute(Name = "merchantSdk")]
        public string merchantSdk { get; set; }

        [XMLAttribute(Name = "xmlns")]
        public string xmlns { get; set; } = "http://www.vantivcnp.com/schema";
        
        [XMLElement(Name = "authentication")]
        public authentication authentication { get; set; }

        private List<transactionRequest> transactions = new List<transactionRequest>();
        
        /*
         * Invoked before serializing the object to finalize
         * setting of elements.
         */
        public override void PreSerialize(XMLVersion version)
        {
            this.version = version;
            this.merchantSdk = CnpVersion.CurrentCNPSDKVersion;
        }
        
        /*
         * Returns additional elements to add when serializing.
         * This method must handle all escaping of special characters.
         */
        public override List<string> GetAdditionalElements(XMLVersion version)
        {
            // Serialize the transactions.
            var elements = new List<string>();
            foreach (var transaction in this.transactions)
            {
                elements.Add(transaction.Serialize(version));
            }
            
            // Return the elements.
            return elements;
        }
        
        /*
         * Adds a transaction to the request.
         */
        public void AddTransaction(transactionRequest transaction)
        {
            this.transactions.Add(transaction);
        }
    }
    
    public class transactionRequest : VersionedXMLElement
    {
        
    }
    
    public class transactionType : transactionRequest
    {
        [XMLAttribute(Name = "id")]
        public string id { get; set; }
        
        [XMLAttribute(Name = "customerId")]
        public string customerId { get; set; }
    }
    
    public class transactionTypeWithReportGroup : transactionType
    {
        [XMLAttribute(Name = "reportGroup")]
        public string reportGroup { get; set; }
    }
    
    public class transactionTypeWithReportGroupAndPartial : transactionType
    {
        [XMLAttribute(Name = "reportGroup")]
        public string reportGroup { get; set; }
        
        [XMLAttribute(Name = "partial")]
        public bool? partial { get; set; }
    }
    
    public class recurringTransactionType : transactionRequest
    {
        
    }
    
    [XMLElement(Name = "activate")]
    public class activate : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "orderId")]
        public string orderId { get; set; }
        
        [XMLElement(Name = "amount")]
        public long amount { get; set; }
        
        [XMLElement(Name = "orderSource")]
        public orderSourceType orderSource { get; set; }
        
        [XMLElement(Name = "card")]
        public giftCardCardType card { get; set; }
        
        [XMLElement(Name = "virtualGiftCard")]
        public virtualGiftCardType virtualGiftCard { get; set; }
    }
    
    [XMLElement(Name = "activateReversal")]
    public class activateReversal : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "cnpTxnId")]
        public long? cnpTxnId { get; set; }
        
        [XMLElement(Name = "card")]
        public giftCardCardType card { get; set; }
            
        [XMLElement(Name = "virtualGiftCardBin")]
        public string virtualGiftCardBin { get; set; }
        
        [XMLElement(Name = "")]
        public string originalRefCode { get; set; }
        
        [XMLElement(Name = "")]
        public long originalAmount { get; set; }
        
        [XMLElement(Name = "")]
        public DateTime originalTxnTime { get; set; }
        
        [XMLElement(Name = "")]
        public int originalSystemTraceId { get; set; }
        
        [XMLElement(Name = "")]
        public string originalSequenceNumber { get; set; }
    }

    [XMLElement(Name = "authorization")]
    public class authorization : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "cnpTxnId")]
        public long? cnpTxnId { get; set; }
        
        [XMLElement(Name = "orderId")]
        public string orderId;
        
        [XMLElement(Name = "amount")]
        public long amount { get; set; }
        
        [XMLElement(Name = "secondaryAmount")]
        public long? secondaryAmount { get; set; }
        
        [XMLElement(Name = "surchargeAmount")]
        public long? surchargeAmount { get; set; }
        
        [XMLElement(Name = "orderSource")]
        public orderSourceType orderSource { get; set; }
        
        [XMLElement(Name = "customerInfo")]
        public customerInfo customerInfo  {get; set; }
        
        [XMLElement(Name = "billToAddress")]
        public contact billToAddress { get; set; }
        
        [XMLElement(Name = "shipToAddress")]
        public contact shipToAddress { get; set; }
        
        [XMLElement(Name = "mpos")]
        public mposType? mpos { get; set; }
        
        [XMLElement(Name = "card")]
        public cardType? card { get; set; }
        
        [XMLElement(Name = "paypal")]
        public payPal paypal { get; set; }
        
        [XMLElement(Name = "token")]
        public cardTokenType? token { get; set; }
        
        [XMLElement(Name = "paypage")]
        public cardPaypageType? paypage { get; set; }
        
        [XMLElement(Name = "applepay")]
        public applepayType? applepay { get; set; }
        
        [XMLElement(Name = "cardholderAuthentication")]
        public fraudCheckType cardholderAuthentication { get; set; }
        
        [XMLElement(Name = "processingInstructions")]
        public processingInstructions processingInstructions { get; set; }
        
        [XMLElement(Name = "pos")]
        public pos pos { get; set; }
        
        [XMLElement(Name = "customBilling")]
        public customBilling customBilling { get; set; }

        [XMLElement(Name = "taxType")]
        private govtTaxTypeEnum? taxType { get; set; }
        
        [XMLElement(Name = "enhancedData")]
        public enhancedData enhancedData { get; set; }
        
        [XMLElement(Name = "allowPartialAuth")]
        public bool? allowPartialAuth { get; set; }
        
        [XMLElement(Name = "healthcareIIAS")]
        public healthcareIIAS? healthcareIIAS { get; set; }
        
        [XMLElement(Name = "lodgingInfo")]
        public lodgingInfo? lodgingInfo { get; set; }
        
        [XMLElement(Name = "filtering")]
        public filteringType? filtering { get; set; }
        
        [XMLElement(Name = "merchantData")]
        public merchantDataType? merchantData { get; set; }
        
        [XMLElement(Name = "recyclingRequest")]
        public recyclingRequestType? recyclingRequest {get; set; }
        
        [XMLElement(Name = "fraudFilterOverride")]
        public bool? fraudFilterOverride {get; set; }
        
        [XMLElement(Name = "recurringRequest")]
        public recurringRequest? recurringRequest {get; set; }
        
        [XMLElement(Name = "debtRepayment")]
        public bool? debtRepayment {get; set; }
        
        [XMLElement(Name = "advancedFraudChecks")]
        public advancedFraudChecksType? advancedFraudChecks {get; set; }
        
        [XMLElement(Name = "wallet")]
        public wallet? wallet {get; set; }
        
        [XMLElement(Name = "processingType")]
        public processingType? processingType {get; set; }
        
        [XMLElement(Name = "originalNetworkTransactionId")]
        public string originalNetworkTransactionId {get; set; }
        
        [XMLElement(Name = "originalTransactionAmount")]
        public long? originalTransactionAmount {get; set; }
        
        [XMLElement(Name = "pinlessDebitRequest")]
        public pinlessDebitRequestType? pinlessDebitRequest {get; set; }
        
        [XMLElement(Name = "skipRealtimeAU")]
        public bool? skipRealtimeAU {get; set; }
    }
    
    [XMLElement(Name = "authReversal")]
    public class authReversal : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "cnpTxnId")]
        public long? cnpTxnId { get; set; }
        
        [XMLElement(Name = "amount")]
        public long? amount { get; set; }
        
        [XMLElement(Name = "surchargeAmount")]
        public long? surchargeAmount { get; set; }
        
        [XMLElement(Name = "payPalNotes")]
        public string payPalNotes { get; set; }
        
        [XMLElement(Name = "actionReason")]
        public string actionReason { get; set; }
    }
    
    [XMLElement(Name = "balanceInquiry")]
    public class balanceInquiry : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "orderId")]
        public string orderId;
        
        [XMLElement(Name = "orderSource")]
        public orderSourceType orderSource;
        
        [XMLElement(Name = "card")]
        public giftCardCardType card;
    }
    
    [XMLElement(Name = "cancelSubscription")]
    public class cancelSubscription : recurringTransactionType
    {
        [XMLElement(Name = "subscriptionId")]
        public long subscriptionId { get; set; }
    }
    
    [XMLElement(Name = "capture")]
    public class capture : transactionTypeWithReportGroupAndPartial
    {
        public long cnpTxnId;
        private long amountField;
        private bool amountSet;
        public long amount
        {
            get { return amountField; }
            set { amountField = value; amountSet = true; }
        }
        private bool surchargeAmountSet;
        private long surchargeAmountField;
        public long surchargeAmount
        {
            get { return surchargeAmountField; }
            set { surchargeAmountField = value; surchargeAmountSet = true; }
        }
        public enhancedData enhancedData;
        public processingInstructions processingInstructions;
        private bool payPalOrderCompleteField;
        private bool payPalOrderCompleteSet;
        public bool payPalOrderComplete
        {
            get { return payPalOrderCompleteField; }
            set { payPalOrderCompleteField = value; payPalOrderCompleteSet = true; }
        }
        public string payPalNotes;
        public customBilling customBilling;
        public lodgingInfo lodgingInfo;
        public string pin;

        public override string Serialize()
        {
            var xml = "\r\n<capture";
            xml += " id=\"" + SecurityElement.Escape(id) + "\"";
            if (customerId != null)
            {
                xml += " customerId=\"" + SecurityElement.Escape(customerId) + "\"";
            }
            xml += " reportGroup=\"" + SecurityElement.Escape(reportGroup) + "\"";
            if (partialSet)
            {
                xml += " partial=\"" + partial.ToString().ToLower() + "\"";
            }
            xml += ">";
            xml += "\r\n<cnpTxnId>" + cnpTxnId + "</cnpTxnId>";
            if (amountSet) xml += "\r\n<amount>" + amountField + "</amount>";
            if (surchargeAmountSet) xml += "\r\n<surchargeAmount>" + surchargeAmountField + "</surchargeAmount>";
            if (enhancedData != null) xml += "\r\n<enhancedData>" + enhancedData.Serialize() + "\r\n</enhancedData>";
            if (processingInstructions != null) xml += "\r\n<processingInstructions>" + processingInstructions.Serialize() + "\r\n</processingInstructions>";
            if (payPalOrderCompleteSet) xml += "\r\n<payPalOrderComplete>" + payPalOrderCompleteField.ToString().ToLower() + "</payPalOrderComplete>";
            if (payPalNotes != null) xml += "\r\n<payPalNotes>" + SecurityElement.Escape(payPalNotes) + "</payPalNotes>";
            if (customBilling != null) xml += "\r\n<customBilling>" + customBilling.Serialize() + "\r\n</customBilling>";
            if (lodgingInfo != null)
            {
                xml += "\r\n<lodgingInfo>" + lodgingInfo.Serialize() + "\r\n</lodgingInfo>";
            }
            if (pin != null) xml += "\r\n<pin>" + pin + "</pin>";
            xml += "\r\n</capture>";

            return xml;
        }
    }
    
    [XMLElement(Name = "captureGivenAuth")]
    public class captureGivenAuth : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "orderId")]
        public string orderId { get; set; }
        
        [XMLElement(Name = "authInformation")]
        public authInformation authInformation { get; set; }
        
        [XMLElement(Name = "amount")]
        public long amount { get; set; }
        
        [XMLElement(Name = "secondaryAmount")]
        public long? secondaryAmount { get; set; }
        
        [XMLElement(Name = "surchargeAmount")]
        public long? surchargeAmount { get; set; }
        
        [XMLElement(Name = "orderSource")]
        public orderSourceType orderSource { get; set; }
        
        [XMLElement(Name = "billToAddress")]
        public contact billToAddress { get; set; }
        
        [XMLElement(Name = "shipToAddress")]
        public contact shipToAddress { get; set; }
        
        [XMLElement(Name = "mpos")]
        public mposType? mpos { get; set; }
        
        [XMLElement(Name = "card")]
        public cardType? card { get; set; }
        
        [XMLElement(Name = "token")]
        public cardTokenType? token { get; set; }
        
        [XMLElement(Name = "paypage")]
        public cardPaypageType? paypage { get; set; }
        
        [XMLElement(Name = "customBilling")]
        public customBilling? customBilling { get; set; }
        
        [XMLElement(Name = "taxType")]
        public govtTaxTypeEnum? taxType { get; set; }
        
        [XMLElement(Name = "enhancedData")]
        public enhancedData? enhancedData { get; set; }
        
        [XMLElement(Name = "lodgingInfo")]
        public lodgingInfo? lodgingInfo { get; set; }
        
        [XMLElement(Name = "processingInstructions")]
        public processingInstructions? processingInstructions { get; set; }
        
        [XMLElement(Name = "pos")]
        public pos? pos { get; set; }
        
        [XMLElement(Name = "merchantData")]
        public merchantDataType? merchantData { get; set; }
        
        [XMLElement(Name = "debtRepayment")]
        public bool? debtRepayment { get; set; }
        
        [XMLElement(Name = "processingType")]
        public processingType? processingType { get; set; }
        
        [XMLElement(Name = "originalNetworkTransactionId")]
        public string originalNetworkTransactionId { get; set; }
        
        [XMLElement(Name = "originalTransactionAmount")]
        public long? originalTransactionAmount { get; set; }
    }
    
    [XMLElement(Name = "createPlan")]
    public class createPlan : recurringTransactionType
    {
        [XMLElement(Name = "planCode")]
        public string planCode { get; set; }
        
        [XMLElement(Name = "name")]
        public string name { get; set; }
        
        [XMLElement(Name = "description")]
        public string description { get; set; }
        
        [XMLElement(Name = "intervalType")]
        public intervalType intervalType { get; set; }
        
        [XMLElement(Name = "amount")]
        public long amount { get; set; }
        
        [XMLElement(Name = "numberOfPayments")]
        public int? numberOfPayments { get; set; }
        
        [XMLElement(Name = "trialNumberOfIntervals")]
        public int? trialNumberOfIntervals { get; set; }
        
        [XMLElement(Name = "trialIntervalType")]
        public trialIntervalType? trialIntervalType { get; set; }

        [XMLElement(Name = "active")]
        public bool active { get; set; }
    }

    [XMLElement(Name = "credit")]
    public class credit : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "cnpTxnId")]
        public long cnpTxnId { get; set; }
        
        [XMLElement(Name = "amount")]
        public long? amount { get; set; }
        
        [XMLElement(Name = "secondaryAmount")]
        public long? secondaryAmount { get; set; }
        
        [XMLElement(Name = "surchargeAmount")]
        public long? secondaryAmountField { get; set; }
        
        [XMLElement(Name = "customBilling")]
        public customBilling? customBilling { get; set; }
        
        [XMLElement(Name = "enhancedData")]
        public enhancedData? enhancedData { get; set; }
        
        [XMLElement(Name = "lodgingInfo")]
        public lodgingInfo? lodgingInfo { get; set; }
        
        [XMLElement(Name = "processingInstructions")]
        public processingInstructions? processingInstructions { get; set; }
        
        [XMLElement(Name = "pos")]
        public pos pos { get; set; }
        
        [XMLElement(Name = "pin")]
        public string pin { get; set; }
        
        [XMLElement(Name = "orderId")]
        public string orderId { get; set; }
        
        [XMLElement(Name = "orderSource")]
        public orderSourceType orderSource { get; set; }
        
        [XMLElement(Name = "billToAddress")]
        public contact billToAddress { get; set; }
        
        [XMLElement(Name = "mpos")]
        public mposType? mpos { get; set; }
        
        [XMLElement(Name = "card")]
        public cardType? card { get; set; }
        
        [XMLElement(Name = "token")]
        public cardTokenType? token { get; set; }
        
        [XMLElement(Name = "paypage")]
        public cardPaypageType? paypage { get; set; }
        
        [XMLElement(Name = "paypal")]
        public payPal paypal { get; set; }
        
        [XMLElement(Name = "taxType")]
        public taxTypeIdentifierEnum? taxType { get; set; }
        
        [XMLElement(Name = "merchantData")]
        public merchantDataType merchantData { get; set; }
        
        [XMLElement(Name = "payPalNotes")]
        public string payPalNotes { get; set; }
        
        [XMLElement(Name = "actionReason")]
        public string actionReason { get; set; }
    }

    [XMLElement(Name = "deactivate")]
    public class deactivate : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "orderId")]
        public string orderId { get; set; }
        
        [XMLElement(Name = "orderSource")]
        public orderSourceType orderSource { get; set; }
        
        [XMLElement(Name = "card")]
        public giftCardCardType card { get; set; }
    }
    
    [XMLElement(Name = "deactivateReversal")]
    public class deactivateReversal : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "cnpTxnId")]
        public long? cnpTxnId { get; set; }

        [XMLElement(Name = "card")]
        public giftCardCardType card { get; set; }
            
        [XMLElement(Name = "originalRefCode")]
        public string originalRefCode { get; set; }
            
        [XMLElement(Name = "originalAmount")]
        public long originalAmount { get; set; }
            
        [XMLElement(Name = "originalTxnTime")]
        public DateTime originalTxnTime { get; set; }
            
        [XMLElement(Name = "originalSystemTraceId")]
        public int originalSystemTraceId { get; set; }
        
        [XMLElement(Name = "originalSequenceNumber")]
        public string originalSequenceNumber { get; set; }
    }
    
    [XMLElement(Name = "depositReversal")]
    public class depositReversal : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "cnpTxnId")]
        public long? cnpTxnId { get; set; }
        
        [XMLElement(Name = "card")]
        public giftCardCardType card { get; set; }
        
        [XMLElement(Name = "originalRefCode")]
        public string originalRefCode { get; set; }
        
        [XMLElement(Name = "originalAmount")]
        public long originalAmount { get; set; }
        
        [XMLElement(Name = "originalTxnTime")]
        public DateTime originalTxnTime { get; set; }
        
        [XMLElement(Name = "originalSystemTraceId")]
        public int originalSystemTraceId { get; set; }
        
        [XMLElement(Name = "originalSequenceNumber")]
        public string originalSequenceNumber { get; set; }
    }
    
    [XMLElement(Name = "echeckCredit")]
    public class echeckCredit : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "cnpTxnId")]
        public long? cnpTxnId { get; set; }
        
        [XMLElement(Name = "amount")]
        public long amount { get; set; }
        
        [XMLElement(Name = "secondaryAmount")]
        public long secondaryAmount { get; set; }
        
        [XMLElement(Name = "customBilling")]
        public customBilling customBilling { get; set; }
        
        [XMLElement(Name = "customIdentifier")]
        public string customIdentifier { get; set; }
        
        [XMLElement(Name = "orderId")]
        public string orderId { get; set; }

        [XMLElement(Name = "orderSource")]
        public orderSourceType orderSource { get; set; }
        
        [XMLElement(Name = "billToAddress")]
        public contact billToAddress { get; set; }
        
        [XMLElement(Name = "echeck")]
        public echeckType echeck { get; set; }
        
        [XMLElement(Name = "echeckToken")]
        public echeckTokenType echeckToken { get; set; }
        
        [XMLElement(Name = "merchantData")]
        public merchantDataType merchantData { get; set; }
    }
    
    [XMLElement(Name = "echeckRedeposit")]
    public class echeckRedeposit : baseRequestTransactionEcheckRedeposit
    {
        [XMLElement(Name = "cnpTxnId")]
        public long? cnpTxnId { get; set; }
        
        [XMLElement(Name = "echeck")]
        public echeckType echeck { get; set; }
        
        [XMLElement(Name = "echeckToken")]
        public echeckTokenType token { get; set; }
        
        [XMLElement(Name = "merchantData")]
        public merchantDataType merchantData { get; set; }
        
        [XMLElement(Name = "customIdentifier")]
        public string customIdentifier { get; set; }
    }
    
    [XMLElement(Name = "echeckSale")]
    public class echeckSale : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "cnpTxnId")]
        public long? cnpTxnId { get; set; }
        
        [XMLElement(Name = "amount")]
        public long? amount { get; set; }
        
        [XMLElement(Name = "customBilling")]
        public customBilling? customBilling { get; set; }
        
        [XMLElement(Name = "customIdentifier")]
        public string customIdentifier { get; set; }
        
        [XMLElement(Name = "orderId")]
        public string orderId { get; set; }
        
        [XMLElement(Name = "verify")]
        public bool? verify { get; set; }
        
        [XMLElement(Name = "secondaryAmount")]
        public long? secondaryAmount { get; set; }
        
        [XMLElement(Name = "orderSource")]
        public orderSourceType orderSource { get; set; }
        
        [XMLElement(Name = "billToAddress")]
        public contact billToAddress { get; set; }
        
        [XMLElement(Name = "shipToAddress")]
        public contact shipToAddress { get; set; }
        
        [XMLElement(Name = "echeck")]
        public echeckType echeck { get; set; }
        
        [XMLElement(Name = "echeckToken")]
        public echeckTokenType token { get; set; }
        
        [XMLElement(Name = "merchantData")]
        public merchantDataType? merchantData { get; set; }
    }
    
    [XMLElement(Name = "echeckVerification")]
    public class echeckVerification : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "orderId")]
        public string orderId { get; set; }
        
        [XMLElement(Name = "amount")]
        public long amount { get; set; }
        
        [XMLElement(Name = "orderSource")]
        public orderSourceType orderSource { get; set; }
        
        [XMLElement(Name = "billToAddress")]
        public contact billToAddress { get; set; }
        
        [XMLElement(Name = "echeck")]
        public echeckType echeck { get; set; }
        
        [XMLElement(Name = "echeckToken")]
        public echeckTokenType token { get; set; }
        
        [XMLElement(Name = "merchantData")]
        public merchantDataType merchantData { get; set; }
    }
    
    [XMLElement(Name = "echeckVoid")]
    public class echeckVoid : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "cnpTxnId")]
        public long cnpTxnId { get; set; }
    }

    [XMLElement(Name = "forceCapture")]
    public class forceCapture : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "orderId")]
        public string orderId { get; set; }
        
        [XMLElement(Name = "amount")]
        public long amount { get; set; }
        
        [XMLElement(Name = "secondaryAmount")]
        public long? secondaryAmount { get; set; }
        
        [XMLElement(Name = "surchargeAmount")]
        public long? surchargeAmount { get; set; }
        
        [XMLElement(Name = "orderSource")]
        public orderSourceType orderSource { get; set; }
        
        [XMLElement(Name = "billToAddress")]
        public contact billToAddress { get; set; }
        
        [XMLElement(Name = "mpos")]
        public mposType? mpos { get; set; }
        
        [XMLElement(Name = "card")]
        public cardType? card { get; set; }
        
        [XMLElement(Name = "token")]
        public cardTokenType? token { get; set; }
        
        [XMLElement(Name = "paypage")]
        public cardPaypageType? paypage { get; set; }
        
        [XMLElement(Name = "customBilling")]
        public customBilling? customBilling { get; set; }
        
        [XMLElement(Name = "taxType")]
        public govtTaxTypeEnum? taxType { get; set; }
        
        [XMLElement(Name = "enhancedData")]
        public enhancedData enhancedData { get; set; }
        
        [XMLElement(Name = "lodgingInfo")]
        public lodgingInfo lodgingInfo { get; set; }
        
        [XMLElement(Name = "processingInstructions")]
        public processingInstructions processingInstructions { get; set; }
        
        [XMLElement(Name = "pos")]
        public pos pos { get; set; }
        
        [XMLElement(Name = "merchantData")]
        public merchantDataType? merchantData { get; set; }
        
        [XMLElement(Name = "debtRepayment")]
        public bool? debtRepayment { get; set; }
        
        [XMLElement(Name = "processingType")]
        public processingType processingType { get; set; }
    }

    [XMLElement(Name = "giftCardAuthReversal")]
    public class giftCardAuthReversal : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "cnpTxnId")]
        public long? cnpTxnId { get; set; }
        
        [XMLElement(Name = "card")]
        public giftCardCardType card { get; set; }
        
        [XMLElement(Name = "originalRefCode")]
        public string originalRefCode { get; set; }
        
        [XMLElement(Name = "originalAmount")]
        public long originalAmount { get; set; }
        
        [XMLElement(Name = "originalTxnTime")]
        public DateTime originalTxnTime { get; set; }
        
        [XMLElement(Name = "originalSystemTraceId")]
        public int originalSystemTraceIdField { get; set; }
        
        [XMLElement(Name = "originalSequenceNumber")]
        public bool originalSystemTraceIdSet { get; set; }
    }
    
    [XMLElement(Name = "giftCardCapture")]
    public class giftCardCapture : transactionTypeWithReportGroupAndPartial
    {
        [XMLElement(Name = "cnpTxnId")]
        public long? cnpTxnId { get; set; }
        
        [XMLElement(Name = "captureAmount")]
        public long captureAmount { get; set; }
        
        [XMLElement(Name = "card")]
        public giftCardCardType card { get; set; }
        
        [XMLElement(Name = "originalRefCode")]
        public string originalRefCode { get; set; }
        
        [XMLElement(Name = "originalAmount")]
        public long originalAmount { get; set; }
        
        [XMLElement(Name = "originalTxnTime")]
        public DateTime originalTxnTime { get; set; }
    }
    
    [XMLElement(Name = "giftCardCredit")]
    public class giftCardCredit : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "cnpTxnId")]
        public long? cnpTxnId { get; set; }
    
        [XMLElement(Name = "creditAmount")]
        public long creditAmount { get; set; }
        
        [XMLElement(Name = "card")]
        public giftCardCardType card { get; set; }
    
        [XMLElement(Name = "orderId")]
        public string orderId { get; set; }
    
        [XMLElement(Name = "orderSource")]
        public orderSourceType? orderSource { get; set; }
    }
    
    [XMLElement(Name = "load")]
    public class load : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "orderId")]
        public string orderId { get; set; }
        
        [XMLElement(Name = "amount")]
        public long amount { get; set; }
        
        [XMLElement(Name = "orderSource")]
        public orderSourceType orderSource { get; set; }
        
        [XMLElement(Name = "card")]
        public giftCardCardType card { get; set; }
    }
    
    [XMLElement(Name = "loadReversal")]
    public class loadReversal : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "cnpTxnId")]
        public long? cnpTxnId { get; set; }
        
        [XMLElement(Name = "card")]
        public giftCardCardType card { get; set; }
        
        [XMLElement(Name = "originalRefCode")]
        public string originalRefCode { get; set; }
        
        [XMLElement(Name = "originalAmount")]
        public long originalAmount { get; set; }
        
        [XMLElement(Name = "originalTxnTime")]
        public DateTime originalTxnTime { get; set; }
        
        [XMLElement(Name = "originalSystemTraceId")]
        public int originalSystemTraceId { get; set; }
        
        [XMLElement(Name = "originalSequenceNumber")]
        public string originalSequenceNumber { get; set; }
    }
    
    [XMLElement(Name = "registerTokenRequestType")]
    public class registerTokenRequestType : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "encryptionKeyId")]
        public string encryptionKeyId { get; set; }
        
        [XMLElement(Name = "orderId")]
        public string orderId { get; set; }
        
        [XMLElement(Name = "mpos")]
        public mposType mpos { get; set; }
        
        [XMLElement(Name = "accountNumber")]
        public string accountNumber { get; set; }
        
        [XMLElement(Name = "encryptedAccountNumber")]
        public string encryptedAccountNumber { get; set; }
        
        [XMLElement(Name = "echeckForToken")]
        public echeckForTokenType echeckForToken { get; set; }
        
        [XMLElement(Name = "paypageRegistrationId")]
        public string paypageRegistrationId { get; set; }
        
        [XMLElement(Name = "applepay")]
        public applepayType applepay { get; set; }
        
        [XMLElement(Name = "cardValidationNum")]
        public string cardValidationNum { get; set; }
        
        [XMLElement(Name = "encryptedCardValidationNum")]
        public string encryptedCardValidationNum { get; set; }
    }
    
    [XMLElement(Name = "refundReversal")]
    public class refundReversal : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "cnpTxnId")]
        public long? cnpTxnId { get; set; }
        
        [XMLElement(Name = "card")]
        public giftCardCardType card { get; set; }
        
        [XMLElement(Name = "originalRefCode")]
        public string originalRefCode { get; set; }
        
        [XMLElement(Name = "originalAmount")]
        public long originalAmount { get; set; }
        
        [XMLElement(Name = "originalTxnTime")]
        public DateTime originalTxnTime { get; set; }
        
        [XMLElement(Name = "originalSystemTraceId")]
        public int originalSystemTraceId { get; set; }
        
        [XMLElement(Name = "originalSequenceNumber")]
        public string originalSequenceNumber { get; set; }
    }
    
    [XMLElement(Name = "sale")]
    public class sale : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "cnpTxnId")]
        public long? cnpTxnId { get; set; }
        
        [XMLElement(Name = "orderId")]
        public string orderId { get; set; }
        
        [XMLElement(Name = "amount")]
        public long amount { get; set; }
        
        [XMLElement(Name = "secondaryAmount")]
        public long? secondaryAmount { get; set; }
        
        [XMLElement(Name = "surchargeAmount")]
        public long? surchargeAmount { get; set; }
        
        [XMLElement(Name = "orderSource")]
        public orderSourceType? orderSource { get; set; }
        
        [XMLElement(Name = "customerInfo")]
        public customerInfo customerInfo { get; set; }
        
        [XMLElement(Name = "billToAddress")]
        public contact? billToAddress { get; set; }
        
        [XMLElement(Name = "shipToAddress")]
        public contact shipToAddress { get; set; }
        
        [XMLElement(Name = "mpos")]
        public mposType mpos { get; set; }
        
        [XMLElement(Name = "card")]
        public cardType? card { get; set; }
        
        [XMLElement(Name = "paypal")]
        public payPal paypal { get; set; }
        
        [XMLElement(Name = "token")]
        public cardTokenType? token { get; set; }
        
        [XMLElement(Name = "paypage")]
        public cardPaypageType? paypage { get; set; }
        
        [XMLElement(Name = "applepay")]
        public applepayType? applepay { get; set; }
        
        [XMLElement(Name = "sepaDirectDebit")]
        public sepaDirectDebitType? sepaDirectDebit { get; set; }
        
        [XMLElement(Name = "ideal")]
        public idealType? ideal { get; set; }
        
        [XMLElement(Name = "giropay")]
        public giropayType? giropay { get; set; }
        
        [XMLElement(Name = "sofort")]
        public sofortType? sofort { get; set; }
        
        [XMLElement(Name = "cardholderAuthentication")]
        public fraudCheckType cardholderAuthentication { get; set; }
        
        [XMLElement(Name = "customBilling")]
        public customBilling customBilling { get; set; }
        
        [XMLElement(Name = "taxType")]
        public govtTaxTypeEnum taxType { get; set; }
        
        [XMLElement(Name = "enhancedData")]
        public enhancedData enhancedData { get; set; }
        
        [XMLElement(Name = "processingInstructions")]
        public processingInstructions processingInstructions { get; set; }
        
        [XMLElement(Name = "pos")]
        public pos pos { get; set; }
        
        [XMLElement(Name = "payPalOrderComplete")]
        public bool? payPalOrderComplete { get; set; }
        
        [XMLElement(Name = "payPalNotes")]
        public string payPalNotes { get; set; }
        
        [XMLElement(Name = "allowPartialAuth")]
        public bool? allowPartialAuth { get; set; }
        
        [XMLElement(Name = "healthcareIIAS")]
        public healthcareIIAS? healthcareIIAS { get; set; }
        
        [XMLElement(Name = "lodgingInfo")]
        public lodgingInfo lodgingInfo { get; set; }
        
        [XMLElement(Name = "filtering")]
        public filteringType filtering { get; set; }
        
        [XMLElement(Name = "merchantData")]
        public merchantDataType? merchantData { get; set; }
        
        [XMLElement(Name = "recyclingRequest")]
        public recyclingRequestType? recyclingRequest { get; set; }
        
        [XMLElement(Name = "fraudFilterOverride")]
        public bool? fraudFilterOverride { get; set; }
        
        [XMLElement(Name = "recurringRequest")]
        public recurringRequest? recurringRequest { get; set; }
        
        [XMLElement(Name = "cnpInternalRecurringRequest")]
        public cnpInternalRecurringRequest? cnpInternalRecurringRequest { get; set; }
        
        [XMLElement(Name = "debtRepayment")]
        public bool? debtRepayment { get; set; }
        
        [XMLElement(Name = "advancedFraudChecks")]
        public advancedFraudChecksType? advancedFraudChecks { get; set; }
        
        [XMLElement(Name = "wallet")]
        public wallet wallet { get; set; }
        
        [XMLElement(Name = "processingType")]
        public processingType? processingType { get; set; }
        
        [XMLElement(Name = "originalNetworkTransactionId")]
        public string originalNetworkTransactionId { get; set; }
        
        [XMLElement(Name = "originalTransactionAmount")]
        public long originalTransactionAmount? { get; set; }
        
        [XMLElement(Name = "pinlessDebitRequest")]
        public pinlessDebitRequestType? pinlessDebitRequest { get; set; }
        
        [XMLElement(Name = "skipRealtimeAU")]
        public bool? skipRealtimeAU { get; set; }
    }
    
    [XMLElement(Name = "unload")]
    public class unload : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "orderId")]
        public string orderId { get; set; }
        
        [XMLElement(Name = "amount")]
        public long amount { get; set; }
        
        [XMLElement(Name = "orderSource")]
        public orderSourceType orderSource { get; set; }
        
        [XMLElement(Name = "card")]
        public giftCardCardType card { get; set; }
    }

    [XMLElement(Name = "updateCardValidationNumOnToken")]
    public class updateCardValidationNumOnToken : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "orderId")]
        public string orderId { get; set; }

        [XMLElement(Name = "cnpToken")]
        public string cnpToken { get; set; }

        [XMLElement(Name = "cardValidationNum")]
        public string cardValidationNum { get; set; }
    }
    
    [XMLElement(Name = "updatePlan")]
    public class updatePlan : recurringTransactionType
    {
        [XMLElement(Name = "planCode")]
        public string planCode { get; set; }
        
        [XMLElement(Name = "active")]
        public bool active { get; set; }
    }
    
    [XMLElement(Name = "updateSubscription")]
    public class updateSubscription : recurringTransactionType
    {
        [XMLElement(Name = "subscriptionId")]
        public long subscriptionId { get; set; }
        
        [XMLElement(Name = "planCode")]
        public string planCode { get; set; }
        
        [XMLElement(Name = "billToAddress")]
        public contact billToAddress { get; set; }
        
        [XMLElement(Name = "card")]
        public cardType? card { get; set; }
        
        [XMLElement(Name = "token")]
        public cardTokenType? token { get; set; }
        
        [XMLElement(Name = "paypage")]
        public cardPaypageType? paypage { get; set; }
        
        [XMLElement(Name = "billingDate")]
        public DateTime billingDate { get; set; }
        
        public List<createDiscount> createDiscounts;
        public List<updateDiscount> updateDiscounts;
        public List<deleteDiscount> deleteDiscounts;
        public List<createAddOn> createAddOns;
        public List<updateAddOn> updateAddOns;
        public List<deleteAddOn> deleteAddOns;

        public updateSubscription()
        {
            createDiscounts = new List<createDiscount>();
            updateDiscounts = new List<updateDiscount>();
            deleteDiscounts = new List<deleteDiscount>();
            createAddOns = new List<createAddOn>();
            updateAddOns = new List<updateAddOn>();
            deleteAddOns = new List<deleteAddOn>();
        }

        /*
         * Returns additional elements to add when serializing.
         * This method must handle all escaping of special characters.
         */
        public override List<string> GetAdditionalElements(XMLVersion version)
        {
            // Serialize the elements.
            var elements = new List<string>();
            foreach (var element in this.createDiscounts)
            {
                elements.Add(element.Serialize(version));
            }
            foreach (var element in this.updateDiscounts)
            {
                elements.Add(element.Serialize(version));
            }
            foreach (var element in this.deleteDiscounts)
            {
                elements.Add(element.Serialize(version));
            }
            foreach (var element in this.createAddOns)
            {
                elements.Add(element.Serialize(version));
            }
            foreach (var element in this.updateAddOns)
            {
                elements.Add(element.Serialize(version));
            }
            foreach (var element in this.deleteAddOns)
            {
                elements.Add(element.Serialize(version));
            }
            
            // Return the elements.
            return elements;
        }
    }
    
    [XMLElement(Name = "unloadReversal")]
    public class unloadReversal : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "cnpTxnId")]
        public long? cnpTxnId { get; set; }
        
        [XMLElement(Name = "card")]
        public giftCardCardType card { get; set; }
        
        [XMLElement(Name = "originalRefCode")]
        public string originalRefCode { get; set; }
        
        [XMLElement(Name = "originalAmount")]
        public long originalAmount { get; set; }
        
        [XMLElement(Name = "originalTxnTime")]
        public DateTime originalTxnTime { get; set; }
        
        [XMLElement(Name = "originalSystemTraceId")]
        public int originalSystemTraceId { get; set; }
        
        [XMLElement(Name = "originalSequenceNumber")]
        public string originalSequenceNumber { get; set; }
    }
    
    [XMLElement(Name = "void")]
    public class voidTxn : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "cnpTxnId")]
        public long cnpTxnId { get; set; }
        
        [XMLElement(Name = "processingInstructions")]
        public processingInstructions processingInstructions { get; set; }
    }
    
    [XMLElement(Name = "fastAccessFunding")]
    public class fastAccessFunding : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "fundingCustomerId")]
        public string fundingCustomerId { get; set; }
        
        [XMLElement(Name = "fundingSubmerchantId")]
        public string fundingSubmerchantId { get; set; }
        
        [XMLElement(Name = "customerName")]
        public string customerName { get; set; }
        
        [XMLElement(Name = "submerchantName")]
        public string submerchantName { get; set; }
        
        [XMLElement(Name = "fundsTransferId")]
        public string fundsTransferId { get; set; }
        
        [XMLElement(Name = "amount")]
        public int? amount { get; set; }
        
        [XMLElement(Name = "disbursementType")]
        public disbursementTypeEnum? disbursementType { get; set; }
        
        [XMLElement(Name = "card")]
        public cardType? card { get; set; }
        
        [XMLElement(Name = "token")]
        public cardTokenType? token { get; set; }
        
        [XMLElement(Name = "paypage")]
        public cardPaypageType? paypage { get; set; }
    }
    
    [XMLElement(Name = "translateToLowValueTokenRequest")]
    public class translateToLowValueTokenRequest : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "orderId")]
        public string orderId { get; set; }
        
        [XMLElement(Name = "token")]
        public string token { get; set; }
    }
    
    [XMLElement(Name = "echeckPreNoteCredit")]
    public class echeckPreNoteCredit : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "orderId")]
        public string orderIdField { get; set; }
        
        [XMLElement(Name = "orderSource")]
        public orderSourceType? orderSourceField { get; set; }
        
        [XMLElement(Name = "billToAddress")]
        public contact billToAddressField { get; set; }
        
        [XMLElement(Name = "echeck")]
        public echeckType echeckField { get; set; }
        
        [XMLElement(Name = "merchantData")]
        public merchantDataType merchantDataField { get; set; }
    }
    
    [XMLElement(Name = "echeckPreNoteSale")]
    public class echeckPreNoteSale : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "orderId")]
        public string orderIdField { get; set; }
        
        [XMLElement(Name = "orderSource")]
        public orderSourceType orderSourceField { get; set; }
        
        [XMLElement(Name = "billToAddress")]
        public contact billToAddressField { get; set; }
        
        [XMLElement(Name = "echeck")]
        public echeckType echeckField { get; set; }
        
        [XMLElement(Name = "merchantData")]
        public merchantDataType? merchantDataField { get; set; }
    }
    
    [XMLElement(Name = "submerchantCredit")]
    public class submerchantCredit : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "fundingSubmerchantId")]
        public string fundingSubmerchantId { get; set; }
        
        [XMLElement(Name = "submerchantName")]
        public string submerchantName { get; set; }
        
        [XMLElement(Name = "fundsTransferId")]
        public string fundsTransferId { get; set; }
        
        [XMLElement(Name = "amount")]
        public long? amount { get; set; }
        
        [XMLElement(Name = "accountInfo")]
        public echeckType accountInfo { get; set; }
        
        [XMLElement(Name = "customIdentifier")]
        public string customIdentifier { get; set; }
    }
    
    [XMLElement(Name = "submerchantDebit")]
    public class submerchantDebit : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "fundingSubmerchantId")]
        public string fundingSubmerchantId { get; set; }
        
        [XMLElement(Name = "submerchantName")]
        public string submerchantName { get; set; }
        
        [XMLElement(Name = "fundsTransferId")]
        public string fundsTransferId { get; set; }
        
        [XMLElement(Name = "amount")]
        public long? amount { get; set; }
        
        [XMLElement(Name = "accountInfo")]
        public echeckType accountInfo { get; set; }
        
        [XMLElement(Name = "customIdentifier")]
        public string customIdentifier { get; set; }
    }

    [XMLElement(Name = "payFacCredit")]
    public class payFacCredit : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "fundingSubmerchantId")]
        public string fundingSubmerchantId { get; set; }
        
        [XMLElement(Name = "fundsTransferId")]
        public string fundsTransferId { get; set; }
        
        [XMLElement(Name = "amount")]
        public long? amount { get; set; }
    }
    
    [XMLElement(Name = "payFacDebit")]
    public class payFacDebit : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "fundingSubmerchantId")]
        public string fundingSubmerchantId { get; set; }
        
        [XMLElement(Name = "fundsTransferId")]
        public string fundsTransferId { get; set; }
        
        [XMLElement(Name = "amount")]
        public long? amount { get; set; }
    }
    
    [XMLElement(Name = "physicalCheckCredit")]
    public class physicalCheckCredit : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "fundingCustomerId")]
        public string fundingCustomerId { get; set; }
        
        [XMLElement(Name = "fundingSubmerchantId")]
        public string fundingSubmerchantId { get; set; }
        
        [XMLElement(Name = "fundsTransferId")]
        public string fundsTransferId { get; set; }
        
        [XMLElement(Name = "amount")]
        public long? amount { get; set; }
    }
    
    [XMLElement(Name = "physicalCheckDebit")]
    public class physicalCheckDebit : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "fundingCustomerId")]
        public string fundingCustomerId { get; set; }
        
        [XMLElement(Name = "fundingSubmerchantId")]
        public string fundingSubmerchantId { get; set; }
        
        [XMLElement(Name = "fundsTransferId")]
        public string fundsTransferId { get; set; }
        
        [XMLElement(Name = "amount")]
        public long? amount { get; set; }
    }
    
    [XMLElement(Name = "reserveCredit")]
    public class reserveCredit : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "fundingCustomerId")]
        public string fundingCustomerId { get; set; }
        
        [XMLElement(Name = "fundingSubmerchantId")]
        public string fundingSubmerchantId { get; set; }
        
        [XMLElement(Name = "fundsTransferId")]
        public string fundsTransferId { get; set; }
        
        [XMLElement(Name = "amount")]
        public long? amount { get; set; }
    }
    
    [XMLElement(Name = "reserveDebit")]
    public class reserveDebit : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "fundingCustomerId")]
        public string fundingCustomerId { get; set; }
        
        [XMLElement(Name = "fundingSubmerchantId")]
        public string fundingSubmerchantId { get; set; }
        
        [XMLElement(Name = "fundsTransferId")]
        public string fundsTransferId { get; set; }
        
        [XMLElement(Name = "amount")]
        public long? amount { get; set; }
    }
    
    [XMLElement(Name = "vendorCredit")]
    public class vendorCredit : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "fundingSubmerchantId")]
        public string fundingSubmerchantId { get; set; }

        [XMLElement(Name = "fundingCustomerId")]
        public string fundingCustomerId { get; set; }

        [XMLElement(Name = "vendorName")]
        public string vendorName { get; set; }

        [XMLElement(Name = "fundsTransferId")]
        public string fundsTransferId { get; set; }

        [XMLElement(Name = "amount")]
        public long? amount { get; set; }

        [XMLElement(Name = "accountInfo")]
        public echeckType accountInfo { get; set; }
    }
    
    [XMLElement(Name = "vendorDebit")]
    public class vendorDebit : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "fundingSubmerchantId")]
        public string fundingSubmerchantId { get; set; }

        [XMLElement(Name = "fundingCustomerId")]
        public string fundingCustomerId { get; set; }

        [XMLElement(Name = "vendorName")]
        public string vendorName { get; set; }

        [XMLElement(Name = "fundsTransferId")]
        public string fundsTransferId { get; set; }

        [XMLElement(Name = "amount")]
        public long? amount { get; set; }

        [XMLElement(Name = "accountInfo")]
        public echeckType accountInfo { get; set; }
    }
    
    [XMLElement(Name = "customerCredit")]
    public class customerCredit : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "fundingCustomerId")]
        public string fundingCustomerId { get; set; }
        
        [XMLElement(Name = "customerName")]
        public string customerName { get; set; }
        
        [XMLElement(Name = "fundsTransferId")]
        public string fundsTransferId { get; set; }
        
        [XMLElement(Name = "amount")]
        public long? amount { get; set; }
        
        [XMLElement(Name = "accountInfo")]
        public echeckType accountInfo { get; set; }
        
        [XMLElement(Name = "customIdentifier")]
        public string customIdentifier { get; set; }
    }
    
    [XMLElement(Name = "customerDebit")]
    public class customerDebit : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "fundingCustomerId")]
        public string fundingCustomerId { get; set; }
        
        [XMLElement(Name = "customerName")]
        public string customerName { get; set; }
        
        [XMLElement(Name = "fundsTransferId")]
        public string fundsTransferId { get; set; }
        
        [XMLElement(Name = "amount")]
        public long? amount { get; set; }
        
        [XMLElement(Name = "accountInfo")]
        public echeckType accountInfo { get; set; }
        
        [XMLElement(Name = "customIdentifier")]
        public string customIdentifier { get; set; }
    }
    
    [XMLElement(Name = "payoutOrgCredit")]
    public class payoutOrgCredit : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "fundingCustomerId")]
        public string fundingCustomerId { get; set; }
        
        [XMLElement(Name = "fundsTransferId")]
        public string fundsTransferId { get; set; }
        
        [XMLElement(Name = "amount")]
        public long? amount { get; set; }
    }
    
    [XMLElement(Name = "payoutOrgDebit")]
    public class payoutOrgDebit : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "fundingCustomerId")]
        public string fundingCustomerId { get; set; }
        
        [XMLElement(Name = "fundsTransferId")]
        public string fundsTransferId { get; set; }
        
        [XMLElement(Name = "amount")]
        public long? amount { get; set; }
    }
    
    [XMLElement(Name = "fundingInstructionVoid")]
    public class fundingInstructionVoid : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "cnpTxnId")]
        public long? cnpTxnId { get; set; }
    }
    
    [XMLElement(Name = "queryTransaction")]
    public class queryTransaction : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "origId")]
        public string origId { get; set; }
        
        [XMLElement(Name = "origActionType")]
        public actionTypeEnum? origActionType { get; set; }
        
        [XMLElement(Name = "origCnpTxnId")]
        public long? origCnpTxnId { get; set; }
        
        [XMLElement(Name = "showStatusOnly")]
        public yesNoTypeEnum? showStatusOnly { get; set; }
    }
}


/*  
    public class cnpInternalRecurringRequest
    {
        public string subscriptionId;
        public string recurringTxnId;

        private bool finalPaymentField;
        private bool finalPaymentSet;
        public bool finalPayment
        {
            get { return finalPaymentField; }
            set { finalPaymentField = value; finalPaymentSet = true; }
        }

        public string Serialize()
        {
            var xml = "";
            if (subscriptionId != null) xml += "\r\n<subscriptionId>" + SecurityElement.Escape(subscriptionId) + "</subscriptionId>";
            if (recurringTxnId != null) xml += "\r\n<recurringTxnId>" + SecurityElement.Escape(recurringTxnId) + "</recurringTxnId>";
            if (finalPaymentSet) xml += "\r\n<finalPayment>" + finalPaymentField.ToString().ToLower() + "</finalPayment>";
            return xml;
        }
    }

    public class createDiscount
    {
        public string discountCode;
        public string name;
        public long amount;
        public DateTime startDate;
        public DateTime endDate;

        public string Serialize()
        {
            var xml = "";
            xml += "\r\n<discountCode>" + SecurityElement.Escape(discountCode) + "</discountCode>";
            xml += "\r\n<name>" + SecurityElement.Escape(name) + "</name>";
            xml += "\r\n<amount>" + amount + "</amount>";
            xml += "\r\n<startDate>" + XmlUtil.toXsdDate(startDate) + "</startDate>";
            xml += "\r\n<endDate>" + XmlUtil.toXsdDate(endDate) + "</endDate>";
            return xml;
        }
    }

    public class updateDiscount
    {
        public string discountCode;

        private string nameField;
        private bool nameSet;
        public string name
        {
            get { return nameField; }
            set { nameField = value; nameSet = true; }
        }

        private long amountField;
        private bool amountSet;
        public long amount
        {
            get { return amountField; }
            set { amountField = value; amountSet = true; }
        }

        private DateTime startDateField;
        private bool startDateSet;
        public DateTime startDate
        {
            get { return startDateField; }
            set { startDateField = value; startDateSet = true; }
        }

        private DateTime endDateField;
        private bool endDateSet;
        public DateTime endDate
        {
            get { return endDateField; }
            set { endDateField = value; endDateSet = true; }
        }

        public string Serialize()
        {
            var xml = "";
            xml += "\r\n<discountCode>" + SecurityElement.Escape(discountCode) + "</discountCode>";
            if (nameSet) xml += "\r\n<name>" + SecurityElement.Escape(nameField) + "</name>";
            if (amountSet) xml += "\r\n<amount>" + amountField + "</amount>";
            if (startDateSet) xml += "\r\n<startDate>" + XmlUtil.toXsdDate(startDateField) + "</startDate>";
            if (endDateSet) xml += "\r\n<endDate>" + XmlUtil.toXsdDate(endDateField) + "</endDate>";
            return xml;
        }
    }

    public class deleteDiscount
    {
        public string discountCode;

        public string Serialize()
        {
            var xml = "";
            xml += "\r\n<discountCode>" + SecurityElement.Escape(discountCode) + "</discountCode>";
            return xml;
        }
    }

    public class createAddOn
    {
        public string addOnCode;
        public string name;
        public long amount;
        public DateTime startDate;
        public DateTime endDate;

        public string Serialize()
        {
            var xml = "";
            xml += "\r\n<addOnCode>" + SecurityElement.Escape(addOnCode) + "</addOnCode>";
            xml += "\r\n<name>" + SecurityElement.Escape(name) + "</name>";
            xml += "\r\n<amount>" + amount + "</amount>";
            xml += "\r\n<startDate>" + XmlUtil.toXsdDate(startDate) + "</startDate>";
            xml += "\r\n<endDate>" + XmlUtil.toXsdDate(endDate) + "</endDate>";
            return xml;
        }
    }

    public class updateAddOn
    {
        public string addOnCode;

        private string nameField;
        private bool nameSet;
        public string name
        {
            get { return nameField; }
            set { nameField = value; nameSet = true; }
        }

        private long amountField;
        private bool amountSet;
        public long amount
        {
            get { return amountField; }
            set { amountField = value; amountSet = true; }
        }

        private DateTime startDateField;
        private bool startDateSet;
        public DateTime startDate
        {
            get { return startDateField; }
            set { startDateField = value; startDateSet = true; }
        }

        private DateTime endDateField;
        private bool endDateSet;
        public DateTime endDate
        {
            get { return endDateField; }
            set { endDateField = value; endDateSet = true; }
        }

        public string Serialize()
        {
            var xml = "";
            xml += "\r\n<addOnCode>" + SecurityElement.Escape(addOnCode) + "</addOnCode>";
            if (nameSet) xml += "\r\n<name>" + SecurityElement.Escape(nameField) + "</name>";
            if (amountSet) xml += "\r\n<amount>" + amountField + "</amount>";
            if (startDateSet) xml += "\r\n<startDate>" + XmlUtil.toXsdDate(startDateField) + "</startDate>";
            if (endDateSet) xml += "\r\n<endDate>" + XmlUtil.toXsdDate(endDateField) + "</endDate>";
            return xml;
        }
    }

    public class deleteAddOn
    {
        public string addOnCode;

        public string Serialize()
        {
            var xml = "";
            xml += "\r\n<addOnCode>" + SecurityElement.Escape(addOnCode) + "</addOnCode>";
            return xml;
        }
    }

    public class subscription
    {
        public string planCode;
        private bool numberOfPaymentsSet;
        private int numberOfPaymentsField;
        public int numberOfPayments
        {
            get { return numberOfPaymentsField; }
            set { numberOfPaymentsField = value; numberOfPaymentsSet = true; }
        }
        private bool startDateSet;
        private DateTime startDateField;
        public DateTime startDate
        {
            get { return startDateField; }
            set { startDateField = value; startDateSet = true; }
        }
        private bool amountSet;
        private long amountField;
        public long amount
        {
            get { return amountField; }
            set { amountField = value; amountSet = true; }
        }

        public List<createDiscount> createDiscounts;
        public List<createAddOn> createAddOns;

        public subscription()
        {
            createDiscounts = new List<createDiscount>();
            createAddOns = new List<createAddOn>();
        }


        public string Serialize()
        {
            var xml = "";
            xml += "\r\n<planCode>" + planCode + "</planCode>";
            if (numberOfPaymentsSet) xml += "\r\n<numberOfPayments>" + numberOfPayments + "</numberOfPayments>";
            if (startDateSet) xml += "\r\n<startDate>" + XmlUtil.toXsdDate(startDateField) + "</startDate>";
            if (amountSet) xml += "\r\n<amount>" + amountField + "</amount>";
            foreach (var createDiscount in createDiscounts)
            {
                xml += "\r\n<createDiscount>" + createDiscount.Serialize() + "\r\n</createDiscount>";
            }
            foreach (var createAddOn in createAddOns)
            {
                xml += "\r\n<createAddOn>" + createAddOn.Serialize() + "\r\n</createAddOn>";
            }

            return xml;
        }
    }

    public class filteringType
    {
        private bool prepaidField;
        private bool prepaidSet;
        public bool prepaid
        {
            get { return prepaidField; }
            set { prepaidField = value; prepaidSet = true; }
        }

        private bool internationalField;
        private bool internationalSet;
        public bool international
        {
            get { return internationalField; }
            set { internationalField = value; internationalSet = true; }
        }

        private bool chargebackField;
        private bool chargebackSet;
        public bool chargeback
        {
            get { return chargebackField; }
            set { chargebackField = value; chargebackSet = true; }
        }

        public string Serialize()
        {
            var xml = "";
            if (prepaidSet) xml += "\r\n<prepaid>" + prepaidField.ToString().ToLower() + "</prepaid>";
            if (internationalSet) xml += "\r\n<international>" + internationalField.ToString().ToLower() + "</international>";
            if (chargebackSet) xml += "\r\n<chargeback>" + chargebackField.ToString().ToLower() + "</chargeback>";
            return xml;
        }

    }

    public class healthcareIIAS
    {
        public healthcareAmounts healthcareAmounts;
        private IIASFlagType IIASFlagField;
        private bool IIASFlagSet;
        public IIASFlagType IIASFlag
        {
            get { return IIASFlagField; }
            set { IIASFlagField = value; IIASFlagSet = true; }
        }

        public string Serialize()
        {
            var xml = "";
            if (healthcareAmounts != null) xml += "\r\n<healthcareAmounts>" + healthcareAmounts.Serialize() + "</healthcareAmounts>";
            if (IIASFlagSet) xml += "\r\n<IIASFlag>" + IIASFlagField + "</IIASFlag>";
            return xml;
        }
    }

    public class lodgingInfo
    {
        public string hotelFolioNumber;
        public DateTime checkInDateField;
        public bool checkInDateSet;
        public DateTime checkInDate
        {
            get
            {
                return checkInDateField;
            }
            set
            {
                checkInDateField = value;
                checkInDateSet = true;
            }
        }

        public DateTime checkOutDateField;
        public bool checkOutDateSet;
        public DateTime checkOutDate
        {
            get
            {
                return checkOutDateField;
            }
            set
            {
                checkOutDateField = value;
                checkOutDateSet = true;
            }
        }

        private int durationField;
        private bool durationSet;
        public int duration
        {
            get
            {
                return durationField;
            }
            set
            {
                durationField = value;
                durationSet = true;
            }
        }

        public string customerServicePhone;
        public lodgingProgramCodeType programCode;

        private int roomRateField;
        private bool roomRateSet;
        public int roomRate
        {
            get
            {
                return roomRateField;
            }
            set
            {
                roomRateField = value;
                roomRateSet = true;
            }
        }


        private int roomTaxField;
        private bool roomTaxSet;
        public int roomTax
        {
            get
            {
                return roomTaxField;
            }
            set
            {
                roomTaxField = value;
                roomTaxSet = true;
            }
        }

        private int numAdultsField;
        private bool numAdultsSet;
        public int numAdults
        {
            get
            {
                return numAdultsField;
            }
            set
            {
                numAdultsField = value;
                numAdultsSet = true;
            }
        }

        public string propertyLocalPhone;

        private bool fireSafetyIndicatorField;
        private bool fireSafetyIndicatorSet;
        public bool fireSafetyIndicator
        {
            get
            {
                return fireSafetyIndicatorField;
            }
            set
            {
                fireSafetyIndicatorField = value;
                fireSafetyIndicatorSet = true;
            }
        }

        public List<lodgingCharge> lodgingCharges;

        public lodgingInfo()
        {

            lodgingCharges = new List<lodgingCharge>();

        }

        public string Serialize()
        {
            var xml = "";
            if (hotelFolioNumber != null) xml += "\r\n<hotelFolioNumber>" + SecurityElement.Escape(hotelFolioNumber) + "</hotelFolioNumber>";
            if (checkInDateSet) xml += "\r\n<checkInDate>" + checkInDate.ToString("yyyy-MM-dd") + "</checkInDate>";
            if (checkOutDateSet) xml += "\r\n<checkOutDate>" + checkOutDate.ToString("yyyy-MM-dd") + "</checkOutDate>";
            if (durationSet) xml += "\r\n<duration>" + durationField + "</duration>";
            if (customerServicePhone != null) xml += "\r\n<customerServicePhone>" + SecurityElement.Escape(customerServicePhone) + "</customerServicePhone>";
            xml += "\r\n<programCode>" + programCode + "</programCode>";
            if (roomRateSet) xml += "\r\n<roomRate>" + roomRateField + "</roomRate>";
            if (roomTaxSet) xml += "\r\n<roomTax>" + roomTaxField + "</roomTax>";
            if (numAdultsSet) xml += "\r\n<numAdults>" + numAdultsField + "</numAdults>";
            if (propertyLocalPhone != null) xml += "\r\n<propertyLocalPhone>" + propertyLocalPhone + "</propertyLocalPhone>";
            if (fireSafetyIndicatorSet) xml += "\r\n<fireSafetyIndicator>" + fireSafetyIndicatorField + "</fireSafetyIndicator>";

            foreach (var lodgingCharge in lodgingCharges)
            {
                xml += "\r\n<lodgingCharge>" + lodgingCharge.Serialize() + "</lodgingCharge>";
            }

            return xml;
        }
    }

    

    public class lodgingCharge
    {

        private lodgingExtraChargeEnum nameField;
        private bool nameSet;
        public lodgingExtraChargeEnum name
        {
            get { return nameField; }
            set { nameField = value; nameSet = true; }
        }

        public string Serialize()
        {
            var xml = "";

            if (nameSet) xml += "\r\n<name>" + nameField + "</name>";
            return xml;

        }

    }

    



    public class recurringRequest
    {
        public subscription subscription;

        public string Serialize()
        {
            var xml = "";
            if (subscription != null) xml += "\r\n<subscription>" + subscription.Serialize() + "\r\n</subscription>";
            return xml;
        }
    }

    public class healthcareAmounts
    {
        private int totalHealthcareAmountField;
        private bool totalHealthcareAmountSet;
        public int totalHealthcareAmount
        {
            get { return totalHealthcareAmountField; }
            set { totalHealthcareAmountField = value; totalHealthcareAmountSet = true; }
        }

        private int RxAmountField;
        private bool RxAmountSet;
        public int RxAmount
        {
            get { return RxAmountField; }
            set { RxAmountField = value; RxAmountSet = true; }
        }

        private int visionAmountField;
        private bool visionAmountSet;
        public int visionAmount
        {
            get { return visionAmountField; }
            set { visionAmountField = value; visionAmountSet = true; }
        }

        private int clinicOtherAmountField;
        private bool clinicOtherAmountSet;
        public int clinicOtherAmount
        {
            get { return clinicOtherAmountField; }
            set { clinicOtherAmountField = value; clinicOtherAmountSet = true; }
        }

        private int dentalAmountField;
        private bool dentalAmountSet;
        public int dentalAmount
        {
            get { return dentalAmountField; }
            set { dentalAmountField = value; dentalAmountSet = true; }
        }

        public string Serialize()
        {
            var xml = "";
            if (totalHealthcareAmountSet) xml += "\r\n<totalHealthcareAmount>" + totalHealthcareAmountField + "</totalHealthcareAmount>";
            if (RxAmountSet) xml += "\r\n<RxAmount>" + RxAmountField + "</RxAmount>";
            if (visionAmountSet) xml += "\r\n<visionAmount>" + visionAmountField + "</visionAmount>";
            if (clinicOtherAmountSet) xml += "\r\n<clinicOtherAmount>" + clinicOtherAmountField + "</clinicOtherAmount>";
            if (dentalAmountSet) xml += "\r\n<dentalAmount>" + dentalAmountField + "</dentalAmount>";
            return xml;
        }
    }

    public sealed class orderSourceType
    {
        public static readonly orderSourceType ecommerce = new orderSourceType("ecommerce");
        public static readonly orderSourceType installment = new orderSourceType("installment");
        public static readonly orderSourceType mailorder = new orderSourceType("mailorder");
        public static readonly orderSourceType recurring = new orderSourceType("recurring");
        public static readonly orderSourceType retail = new orderSourceType("retail");
        public static readonly orderSourceType telephone = new orderSourceType("telephone");
        public static readonly orderSourceType item3dsAuthenticated = new orderSourceType("3dsAuthenticated");
        public static readonly orderSourceType item3dsAttempted = new orderSourceType("3dsAttempted");
        public static readonly orderSourceType recurringtel = new orderSourceType("recurringtel");
        public static readonly orderSourceType echeckppd = new orderSourceType("echeckppd");
        public static readonly orderSourceType applepay = new orderSourceType("applepay");
        public static readonly orderSourceType androidpay = new orderSourceType("androidpay");

        private orderSourceType(string value) { this.value = value; }
        public string Serialize() { return value; }
        private string value;
    }

    public class contact
    {

        public string name;
        public string firstName;
        public string middleInitial;
        public string lastName;
        public string companyName;
        public string addressLine1;
        public string addressLine2;
        public string addressLine3;
        public string city;
        public string state;
        public string zip;
        private countryTypeEnum countryField;
        private bool countrySpecified;
        public countryTypeEnum country
        {
            get { return countryField; }
            set { countryField = value; countrySpecified = true; }
        }
        public string email;
        public string phone;

        public string Serialize()
        {
            var xml = "";
            if (name != null) xml += "\r\n<name>" + SecurityElement.Escape(name) + "</name>";
            if (firstName != null) xml += "\r\n<firstName>" + SecurityElement.Escape(firstName) + "</firstName>";
            if (middleInitial != null) xml += "\r\n<middleInitial>" + SecurityElement.Escape(middleInitial) + "</middleInitial>";
            if (lastName != null) xml += "\r\n<lastName>" + SecurityElement.Escape(lastName) + "</lastName>";
            if (companyName != null) xml += "\r\n<companyName>" + SecurityElement.Escape(companyName) + "</companyName>";
            if (addressLine1 != null) xml += "\r\n<addressLine1>" + SecurityElement.Escape(addressLine1) + "</addressLine1>";
            if (addressLine2 != null) xml += "\r\n<addressLine2>" + SecurityElement.Escape(addressLine2) + "</addressLine2>";
            if (addressLine3 != null) xml += "\r\n<addressLine3>" + SecurityElement.Escape(addressLine3) + "</addressLine3>";
            if (city != null) xml += "\r\n<city>" + SecurityElement.Escape(city) + "</city>";
            if (state != null) xml += "\r\n<state>" + SecurityElement.Escape(state) + "</state>";
            if (zip != null) xml += "\r\n<zip>" + SecurityElement.Escape(zip) + "</zip>";
            if (countrySpecified) xml += "\r\n<country>" + countryField + "</country>";
            if (email != null) xml += "\r\n<email>" + SecurityElement.Escape(email) + "</email>";
            if (phone != null) xml += "\r\n<phone>" + SecurityElement.Escape(phone) + "</phone>";
            return xml;
        }
    }

    

    public class advancedFraudChecksType
    {
        public string threatMetrixSessionId;
        private string customAttribute1Field;
        private bool customAttribute1Set;
        public string customAttribute1
        {
            get { return customAttribute1Field; }
            set { customAttribute1Field = value; customAttribute1Set = true; }
        }
        private string customAttribute2Field;
        private bool customAttribute2Set;
        public string customAttribute2
        {
            get { return customAttribute2Field; }
            set { customAttribute2Field = value; customAttribute2Set = true; }
        }
        private string customAttribute3Field;
        private bool customAttribute3Set;
        public string customAttribute3
        {
            get { return customAttribute3Field; }
            set { customAttribute3Field = value; customAttribute3Set = true; }
        }
        private string customAttribute4Field;
        private bool customAttribute4Set;
        public string customAttribute4
        {
            get { return customAttribute4Field; }
            set { customAttribute4Field = value; customAttribute4Set = true; }
        }
        private string customAttribute5Field;
        private bool customAttribute5Set;
        public string customAttribute5
        {
            get { return customAttribute5Field; }
            set { customAttribute5Field = value; customAttribute5Set = true; }
        }

        public string Serialize()
        {
            var xml = "";
            if (threatMetrixSessionId != null) xml += "\r\n<threatMetrixSessionId>" + SecurityElement.Escape(threatMetrixSessionId) + "</threatMetrixSessionId>";
            if (customAttribute1Set) xml += "\r\n<customAttribute1>" + SecurityElement.Escape(customAttribute1Field) + "</customAttribute1>";
            if (customAttribute2Set) xml += "\r\n<customAttribute2>" + SecurityElement.Escape(customAttribute2Field) + "</customAttribute2>";
            if (customAttribute3Set) xml += "\r\n<customAttribute3>" + SecurityElement.Escape(customAttribute3Field) + "</customAttribute3>";
            if (customAttribute4Set) xml += "\r\n<customAttribute4>" + SecurityElement.Escape(customAttribute4Field) + "</customAttribute4>";
            if (customAttribute5Set) xml += "\r\n<customAttribute5>" + SecurityElement.Escape(customAttribute5Field) + "</customAttribute5>";
            return xml;
        }
    }

    public class mposType
    {
        public string ksn;
        public string formatId;
        public string encryptedTrack;
        public int track1Status;
        public int track2Status;

        public string Serialize()
        {
            var xml = "";
            if (ksn != null)
            {
                xml += "\r\n<ksn>" + ksn + "</ksn>";
            }
            if (formatId != null)
            {
                xml += "\r\n<formatId>" + formatId + "</formatId>";
            }
            if (encryptedTrack != null)
            {
                xml += "\r\n<encryptedTrack>" + SecurityElement.Escape(encryptedTrack) + "</encryptedTrack>";
            }
            if (track1Status == 0 || track1Status == 1)
            {
                xml += "\r\n<track1Status>" + track1Status + "</track1Status>";
            }
            if (track2Status == 0 || track2Status == 1)
            {
                xml += "\r\n<track2Status>" + track2Status + "</track2Status>";
            }

            return xml;
        }
    }

    public class cardType
    {
        public methodOfPaymentTypeEnum type;
        public string number;
        public string expDate;
        public string track;
        public string cardValidationNum;
        public string pin;

        public string Serialize()
        {
            var xml = "";
            if (track == null)
            {
                xml += "\r\n<type>" + methodOfPaymentSerializer.Serialize(type) + "</type>";
                if (number != null)
                {
                    xml += "\r\n<number>" + SecurityElement.Escape(number) + "</number>";
                }
                if (expDate != null)
                {
                    xml += "\r\n<expDate>" + SecurityElement.Escape(expDate) + "</expDate>";
                }
            }
            else
            {
                xml += "\r\n<track>" + SecurityElement.Escape(track) + "</track>";
            }
            if (cardValidationNum != null)
            {
                xml += "\r\n<cardValidationNum>" + SecurityElement.Escape(cardValidationNum) + "</cardValidationNum>";
            }
            if (pin != null)
            {
                xml += "\r\n<pin>" + pin + "</pin>";
            }
            return xml;
        }
    }

    public class giftCardCardType
    {
        public methodOfPaymentTypeEnum type;
        public string number;
        public string expDate;
        public string track;
        public string cardValidationNum;
        public string pin;

        public string Serialize()
        {
            var xml = "";
            if (track == null)
            {
                xml += "\r\n<type>" + methodOfPaymentSerializer.Serialize(type) + "</type>";
                if (number != null)
                {
                    xml += "\r\n<number>" + SecurityElement.Escape(number) + "</number>";
                }
                if (expDate != null)
                {
                    xml += "\r\n<expDate>" + SecurityElement.Escape(expDate) + "</expDate>";
                }
            }
            else
            {
                xml += "\r\n<track>" + SecurityElement.Escape(track) + "</track>";
            }
            if (cardValidationNum != null)
            {
                xml += "\r\n<cardValidationNum>" + SecurityElement.Escape(cardValidationNum) + "</cardValidationNum>";
            }
            if (pin != null)
            {
                xml += "\r\n<pin>" + pin + "</pin>";
            }
            return xml;
        }
    }

    public class virtualGiftCardType
    {
        public int accountNumberLength
        {
            get { return accountNumberLengthField; }
            set { accountNumberLengthField = value; accountNumberLengthSet = true; }
        }
        private int accountNumberLengthField;
        private bool accountNumberLengthSet;

        public string giftCardBin;

        public string Serialize()
        {
            var xml = "";
            if (accountNumberLengthSet) xml += "\r\n<accountNumberLength>" + accountNumberLengthField + "</accountNumberLength>";
            if (giftCardBin != null) xml += "\r\n<giftCardBin>" + SecurityElement.Escape(giftCardBin) + "</giftCardBin>";
            return xml;
        }

    }

    public class accountUpdate : transactionTypeWithReportGroup
    {
        public string orderId;
        public cardType card;
        public cardTokenType token;

        public override string Serialize()
        {
            var xml = "\r\n<accountUpdate ";

            if (id != null)
            {
                xml += "id=\"" + SecurityElement.Escape(id) + "\" ";
            }
            if (customerId != null)
            {
                xml += "customerId=\"" + SecurityElement.Escape(customerId) + "\" ";
            }
            xml += "reportGroup=\"" + SecurityElement.Escape(reportGroup) + "\">";

            xml += "\r\n<orderId>" + SecurityElement.Escape(orderId) + "</orderId>";

            if (card != null)
            {
                xml += "\r\n<card>";
                xml += card.Serialize();
                xml += "\r\n</card>";
            }
            else if (token != null)
            {
                xml += "\r\n<token>";
                xml += token.Serialize();
                xml += "\r\n</token>";
            }

            xml += "\r\n</accountUpdate>";

            return xml;
        }
    }

    public class accountUpdateFileRequestData
    {
        public string merchantId;
        public accountUpdateFileRequestData()
        {
            merchantId = Properties.Settings.Default.merchantId;
        }
        public accountUpdateFileRequestData(Dictionary<string, string> config)
        {
            merchantId = config["merchantId"];
        }
        public DateTime postDay; //yyyy-MM-dd

        public string Serialize()
        {
            var xml = "\r\n<merchantId>" + SecurityElement.Escape(merchantId) + "</merchantId>";

            if (postDay != null)
            {
                xml += "\r\n<postDay>" + postDay.ToString("yyyy-MM-dd") + "</postDay>";
            }

            return xml;
        }
    }

    public class applepayType
    {
        public string data;
        public applepayHeaderType header;
        public string signature;
        public string version;

        public string Serialize()
        {
            var xml = "";
            if (data != null) xml += "\r\n<data>" + SecurityElement.Escape(data) + "</data>";
            if (header != null) xml += "\r\n<header>" + header.Serialize() + "</header>";
            if (signature != null) xml += "\r\n<signature>" + SecurityElement.Escape(signature) + "</signature>";
            if (version != null) xml += "\r\n<version>" + SecurityElement.Escape(version) + "</version>";
            return xml;
        }
    }

    public class applepayHeaderType
    {
        public string applicationData;
        public string ephemeralPublicKey;
        public string publicKeyHash;
        public string transactionId;

        public string Serialize()
        {
            var xml = "";
            if (applicationData != null) xml += "\r\n<applicationData>" + SecurityElement.Escape(applicationData) + "</applicationData>";
            if (ephemeralPublicKey != null) xml += "\r\n<ephemeralPublicKey>" + SecurityElement.Escape(ephemeralPublicKey) + "</ephemeralPublicKey>";
            if (publicKeyHash != null) xml += "\r\n<publicKeyHash>" + SecurityElement.Escape(publicKeyHash) + "</publicKeyHash>";
            if (transactionId != null) xml += "\r\n<transactionId>" + SecurityElement.Escape(transactionId) + "</transactionId>";
            return xml;
        }
    }

    public class wallet
    {
        public walletWalletSourceType walletSourceType;
        public string walletSourceTypeId;

        public string Serialize()
        {
            var xml = "";
            xml += "\r\n<walletSourceType>" + walletSourceType + "</walletSourceType>";
            if (walletSourceTypeId != null) xml += "\r\n<walletSourceTypeId>" + SecurityElement.Escape(walletSourceTypeId) + "</walletSourceTypeId>";
            return xml;
        }
    }


    public class pinlessDebitRequestType
    {

        public routingPreferenceEnum routingPreferenceField;
        public bool routingPreferenceSet;
        public routingPreferenceEnum routingPreference
        {
            get
            {
                return routingPreferenceField;
            }
            set
            {
                routingPreferenceField = value;
                routingPreferenceSet = true;
            }
        }

        public preferredDebitNetworksType preferredDebitNetworks;


        public string Serialize()
        {
            var xml = "";
            if (routingPreferenceSet) xml += "\r\n<routingPreference>" + routingPreferenceField + "</routingPreference>";
            if (preferredDebitNetworks != null) xml += "\r\n<preferredDebitNetworks>" + preferredDebitNetworks.Serialize() + "</preferredDebitNetworks>";

            return xml;

        }

    }

    public class preferredDebitNetworksType
    {

        public List<string> debitNetworkName;

        public preferredDebitNetworksType()
        {

            debitNetworkName = new List<string>();

        }

        public string Serialize()
        {
            var xml = "";
            if (debitNetworkName.Count > 0)
            {
                foreach (string dnn in debitNetworkName)
                {
                    xml += "\r\n<debitNetworkName>" + dnn + "</debitNetworkName>";
                }
            }

            return xml;

        }

    }


    

    public class preferredDebitNetworksType
    { }



    public class fraudCheck : transactionTypeWithReportGroup
    {

        public advancedFraudChecksType advancedFraudChecks;

        private contact billToAddressField;
        private bool billToAddressSet;
        public contact billToAddress
        {
            get
            {
                return billToAddressField;
            }
            set
            {
                billToAddressField = value; billToAddressSet = true;
            }
        }

        private contact shipToAddressField;
        private bool shipToAddressSet;
        public contact shipToAddress
        {
            get
            {
                return shipToAddressField;
            }
            set
            {
                shipToAddressField = value; shipToAddressSet = true;
            }
        }

        private int amountField;
        private bool amountSet;
        public int amount
        {
            get
            {
                return amountField;
            }
            set
            {
                amountField = value; amountSet = true;
            }
        }

        private eventTypeEnum eventTypeField;
        private bool eventTypeSet;

        public eventTypeEnum eventType
        {
            get
            {
                return eventTypeField;
            }
            set
            {
                eventTypeField = value;
                eventTypeSet = true;

            }
        }

        public string accountLogin;
        public string accountPasshash;

        public override string Serialize()
        {
            var xml = "\r\n<fraudCheck";
            xml += " id=\"" + SecurityElement.Escape(id) + "\"";
            if (customerId != null)
            {
                xml += " customerId=\"" + SecurityElement.Escape(customerId) + "\"";
            }
            xml += " reportGroup=\"" + SecurityElement.Escape(reportGroup) + "\">";
            if (advancedFraudChecks != null) xml += "\r\n<advancedFraudChecks>" + advancedFraudChecks.Serialize() + "\r\n</advancedFraudChecks>";
            if (billToAddressSet) xml += "\r\n<billToAddress>" + billToAddressField.Serialize() + "</billToAddress>";
            if (shipToAddressSet) xml += "\r\n<shipToAddress>" + shipToAddressField.Serialize() + "</shipToAddress>";
            if (amountSet) xml += "\r\n<amount>" + amountField.ToString() + "</amount>";
            if (eventTypeSet) xml += "\r\n<eventType>" + eventTypeField + "</eventType>";
            if (accountLogin != null) xml += "\r\n<accountLogin>" + SecurityElement.Escape(accountLogin) + "</accountLogin>";
            if (accountPasshash != null) xml += "\r\n<accountPasshash>" + SecurityElement.Escape(accountPasshash) + "</accountPasshash>";

            xml += "\r\n</fraudCheck>";
            return xml;
        }
    }

    

    public class sepaDirectDebitType
    {
        public mandateProviderType mandateProvider;
        public sequenceTypeType sequenceType;
        public string mandateReferenceField;
        public bool mandateReferenceSet;
        public string mandateReference
        {
            get
            {
                return mandateReferenceField;
            }
            set
            {
                mandateReferenceField = value;
                mandateReferenceSet = true;
            }
        }
        public string mandateUrlField;
        public bool mandateUrlSet;
        public string mandateUrl
        {
            get
            {
                return mandateUrlField;
            }
            set
            {
                mandateUrlField = value;
                mandateUrlSet = true;
            }
        }
        // CES does this work
        public DateTime mandateSignatureDateField;
        public bool mandateSignatureDateSet;
        public DateTime mandateSignatureDate
        {
            get
            {
                return mandateSignatureDateField;
            }
            set
            {
                mandateSignatureDateField = value;
                mandateSignatureDateSet = true;
            }
        }
        public string iban;
        public countryTypeEnum preferredLanguageField;
        public bool preferredLanguageSet;
        public countryTypeEnum preferredLanguage
        {
            get
            {
                return preferredLanguageField;
            }
            set
            {
                preferredLanguageField = value;
                preferredLanguageSet = true;
            }
        }
        public string Serialize()
        {
            var xml = "";
            xml += "\r\n<mandateProvider>" + mandateProvider + "</mandateProvider>";
            xml += "\r\n<sequenceType>" + sequenceType + "</sequenceType>";
            if (mandateReferenceSet)
            {
                xml += "\r\n<mandateReference>" + mandateReference + "</mandateReference>";
            }
            if (mandateUrlSet)
            {
                xml += "\r\n<mandateUrl>" + mandateUrl + "</mandateUrl>";
            }
            if (mandateSignatureDateSet)
            {
                xml += "\r\n<mandateSignatureDate>" + mandateSignatureDate + "</mandateSignatureDate>";
            }
            if (iban != null)
            {
                xml += "\r\n<iban>" + iban + "</iban>";
            }
            if (preferredLanguageSet)
            {
                xml += "\r\n<preferredLanguage>" + preferredLanguage + "</preferredLanguage>";
            }
            return xml;
        }
    }


    public class idealType
    {
        public countryTypeEnum preferredLanguageField;
        public bool preferredLanguageSet;
        public countryTypeEnum preferredLanguage
        {
            get
            {
                return preferredLanguageField;
            }
            set
            {
                preferredLanguageField = value;
                preferredLanguageSet = true;
            }
        }

        public string Serialize()
        {
            var xml = "";
            if (preferredLanguageSet)
            {
                xml += "\r\n<preferredLanguage>" + preferredLanguage + "</preferredLanguage>";
            }
            return xml;
        }
    }

    public class giropayType
    {
        public countryTypeEnum preferredLanguageField;
        public bool preferredLanguageSet;
        public countryTypeEnum preferredLanguage
        {
            get
            {
                return preferredLanguageField;
            }
            set
            {
                preferredLanguageField = value;
                preferredLanguageSet = true;
            }
        }

        public string Serialize()
        {
            var xml = "";
            if (preferredLanguageSet)
            {
                xml += "\r\n<preferredLanguage>" + preferredLanguage + "</preferredLanguage>";
            }
            return xml;
        }
    }

    // The sofort element is a child of the sale transaction that, through its child elements,
    // defines information needed to process an SOFORT (Real-time Bank Transfer) transaction.
    // At this time, you can use the iDeal method of payment in Online transactions only.
    public class sofortType
    {
        public countryTypeEnum preferredLanguageField;
        public bool preferredLanguageSet;
        public countryTypeEnum preferredLanguage
        {
            get
            {
                return preferredLanguageField;
            }
            set
            {
                preferredLanguageField = value;
                preferredLanguageSet = true;
            }
        }

        public string Serialize()
        {
            var xml = "";
            if (preferredLanguageSet)
            {
                xml += "\r\n<preferredLanguage>" + preferredLanguage + "</preferredLanguage>";
            }
            return xml;
        }
    }

    #endregion

    public class XmlUtil
    {
        public static string toXsdDate(DateTime dateTime)
        {
            var year = dateTime.Year.ToString();
            var month = dateTime.Month.ToString();
            if (dateTime.Month < 10)
            {
                month = "0" + month;
            }
            var day = dateTime.Day.ToString();
            if (dateTime.Day < 10)
            {
                day = "0" + day;
            }
            return year + "-" + month + "-" + day;
        }
    }
}
*/