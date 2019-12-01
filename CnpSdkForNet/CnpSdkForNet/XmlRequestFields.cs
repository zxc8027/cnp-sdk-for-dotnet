/*
 * Zachary Cook
 * 
 * Fields for XML requests. Refer to the XML reference guides
 * for further documentation.
 */

using System;
using System.Collections.Generic;
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
        public mposType mpos { get; set; }
        
        [XMLElement(Name = "card")]
        public cardType card { get; set; }
        
        [XMLElement(Name = "paypal")]
        public payPal paypal { get; set; }
        
        [XMLElement(Name = "token")]
        public cardTokenType token { get; set; }
        
        [XMLElement(Name = "paypage")]
        public cardPaypageType paypage { get; set; }
        
        [XMLElement(Name = "applepay")]
        public applepayType applepay { get; set; }
        
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
        public healthcareIIAS healthcareIIAS { get; set; }
        
        [XMLElement(Name = "lodgingInfo")]
        public lodgingInfo lodgingInfo { get; set; }
        
        [XMLElement(Name = "filtering")]
        public filteringType filtering { get; set; }
        
        [XMLElement(Name = "merchantData")]
        public merchantDataType merchantData { get; set; }
        
        [XMLElement(Name = "recyclingRequest")]
        public recyclingRequestType recyclingRequest {get; set; }
        
        [XMLElement(Name = "fraudFilterOverride")]
        public bool? fraudFilterOverride {get; set; }
        
        [XMLElement(Name = "recurringRequest")]
        public recurringRequest recurringRequest {get; set; }
        
        [XMLElement(Name = "debtRepayment")]
        public bool? debtRepayment {get; set; }
        
        [XMLElement(Name = "advancedFraudChecks")]
        public advancedFraudChecksType advancedFraudChecks {get; set; }
        
        [XMLElement(Name = "wallet")]
        public wallet wallet {get; set; }
        
        [XMLElement(Name = "processingType")]
        public processingType? processingType {get; set; }
        
        [XMLElement(Name = "originalNetworkTransactionId")]
        public string originalNetworkTransactionId {get; set; }
        
        [XMLElement(Name = "originalTransactionAmount")]
        public long? originalTransactionAmount {get; set; }
        
        [XMLElement(Name = "pinlessDebitRequest")]
        public pinlessDebitRequestType pinlessDebitRequest {get; set; }
        
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
        [XMLElement(Name = "cnpTxnId")]
        public long? cnpTxnId { get; set; }
        
        [XMLElement(Name = "amount")]
        public long? amount { get; set; }
        
        [XMLElement(Name = "surchargeAmount")]
        public long? surchargeAmount { get; set; }
        
        [XMLElement(Name = "enhancedData")]
        public enhancedData enhancedData { get; set; }
        
        [XMLElement(Name = "processingInstructions")]
        public processingInstructions processingInstructions { get; set; }
        
        [XMLElement(Name = "payPalOrderComplete")]
        public bool? payPalOrderComplete { get; set; }
        
        [XMLElement(Name = "payPalNotes")]
        public string payPalNotes { get; set; }
        
        [XMLElement(Name = "customBilling")]
        public customBilling customBilling { get; set; }
        
        [XMLElement(Name = "lodgingInfo")]
        public lodgingInfo lodgingInfo { get; set; }
        
        [XMLElement(Name = "pin")]
        public string pin { get; set; }
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
        public mposType mpos { get; set; }
        
        [XMLElement(Name = "card")]
        public cardType card { get; set; }
        
        [XMLElement(Name = "token")]
        public cardTokenType token { get; set; }
        
        [XMLElement(Name = "paypage")]
        public cardPaypageType paypage { get; set; }
        
        [XMLElement(Name = "customBilling")]
        public customBilling customBilling { get; set; }
        
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
        public merchantDataType merchantData { get; set; }
        
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
        public customBilling customBilling { get; set; }
        
        [XMLElement(Name = "enhancedData")]
        public enhancedData enhancedData { get; set; }
        
        [XMLElement(Name = "lodgingInfo")]
        public lodgingInfo lodgingInfo { get; set; }
        
        [XMLElement(Name = "processingInstructions")]
        public processingInstructions processingInstructions { get; set; }
        
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
        public mposType mpos { get; set; }
        
        [XMLElement(Name = "card")]
        public cardType card { get; set; }
        
        [XMLElement(Name = "token")]
        public cardTokenType token { get; set; }
        
        [XMLElement(Name = "paypage")]
        public cardPaypageType paypage { get; set; }
        
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
        public customBilling customBilling { get; set; }
        
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
        public merchantDataType merchantData { get; set; }
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
        public mposType mpos { get; set; }
        
        [XMLElement(Name = "card")]
        public cardType card { get; set; }
        
        [XMLElement(Name = "token")]
        public cardTokenType token { get; set; }
        
        [XMLElement(Name = "paypage")]
        public cardPaypageType paypage { get; set; }
        
        [XMLElement(Name = "customBilling")]
        public customBilling customBilling { get; set; }
        
        [XMLElement(Name = "taxType")]
        public govtTaxTypeEnum taxType { get; set; }
        
        [XMLElement(Name = "enhancedData")]
        public enhancedData enhancedData { get; set; }
        
        [XMLElement(Name = "lodgingInfo")]
        public lodgingInfo lodgingInfo { get; set; }
        
        [XMLElement(Name = "processingInstructions")]
        public processingInstructions processingInstructions { get; set; }
        
        [XMLElement(Name = "pos")]
        public pos pos { get; set; }
        
        [XMLElement(Name = "merchantData")]
        public merchantDataType merchantData { get; set; }
        
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
        public orderSourceType orderSource { get; set; }
        
        [XMLElement(Name = "customerInfo")]
        public customerInfo customerInfo { get; set; }
        
        [XMLElement(Name = "billToAddress")]
        public contact billToAddress { get; set; }
        
        [XMLElement(Name = "shipToAddress")]
        public contact shipToAddress { get; set; }
        
        [XMLElement(Name = "mpos")]
        public mposType mpos { get; set; }
        
        [XMLElement(Name = "card")]
        public cardType card { get; set; }
        
        [XMLElement(Name = "paypal")]
        public payPal paypal { get; set; }
        
        [XMLElement(Name = "token")]
        public cardTokenType token { get; set; }
        
        [XMLElement(Name = "paypage")]
        public cardPaypageType paypage { get; set; }
        
        [XMLElement(Name = "applepay")]
        public applepayType applepay { get; set; }
        
        [XMLElement(Name = "sepaDirectDebit")]
        public sepaDirectDebitType sepaDirectDebit { get; set; }
        
        [XMLElement(Name = "ideal")]
        public idealType ideal { get; set; }
        
        [XMLElement(Name = "giropay")]
        public giropayType giropay { get; set; }
        
        [XMLElement(Name = "sofort")]
        public sofortType sofort { get; set; }
        
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
        public healthcareIIAS healthcareIIAS { get; set; }
        
        [XMLElement(Name = "lodgingInfo")]
        public lodgingInfo lodgingInfo { get; set; }
        
        [XMLElement(Name = "filtering")]
        public filteringType filtering { get; set; }
        
        [XMLElement(Name = "merchantData")]
        public merchantDataType merchantData { get; set; }
        
        [XMLElement(Name = "recyclingRequest")]
        public recyclingRequestType recyclingRequest { get; set; }
        
        [XMLElement(Name = "fraudFilterOverride")]
        public bool? fraudFilterOverride { get; set; }
        
        [XMLElement(Name = "recurringRequest")]
        public recurringRequest recurringRequest { get; set; }
        
        [XMLElement(Name = "cnpInternalRecurringRequest")]
        public cnpInternalRecurringRequest cnpInternalRecurringRequest { get; set; }
        
        [XMLElement(Name = "debtRepayment")]
        public bool? debtRepayment { get; set; }
        
        [XMLElement(Name = "advancedFraudChecks")]
        public advancedFraudChecksType advancedFraudChecks { get; set; }
        
        [XMLElement(Name = "wallet")]
        public wallet wallet { get; set; }
        
        [XMLElement(Name = "processingType")]
        public processingType? processingType { get; set; }
        
        [XMLElement(Name = "originalNetworkTransactionId")]
        public string originalNetworkTransactionId { get; set; }
        
        [XMLElement(Name = "originalTransactionAmount")]
        public long? originalTransactionAmount { get; set; }
        
        [XMLElement(Name = "pinlessDebitRequest")]
        public pinlessDebitRequestType pinlessDebitRequest { get; set; }
        
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
        public cardType card { get; set; }
        
        [XMLElement(Name = "token")]
        public cardTokenType token { get; set; }
        
        [XMLElement(Name = "paypage")]
        public cardPaypageType paypage { get; set; }
        
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
        public cardType card { get; set; }
        
        [XMLElement(Name = "token")]
        public cardTokenType token { get; set; }
        
        [XMLElement(Name = "paypage")]
        public cardPaypageType paypage { get; set; }
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
        public orderSourceType orderSourceField { get; set; }
        
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
        public merchantDataType merchantDataField { get; set; }
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

    [XMLElement(Name = "accountUpdate")]
    public class accountUpdate : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "orderId")]
        public string orderId { get; set; }
        
        [XMLElement(Name = "card")]
        public cardType card { get; set; }
        
        [XMLElement(Name = "token")]
        public cardTokenType token { get; set; }
    }

    [XMLElement(Name = "fraudCheck")]
    public class fraudCheck : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "advancedFraudChecks")]
        public advancedFraudChecksType advancedFraudChecks { get; set; }
        
        [XMLElement(Name = "billToAddress")]
        public contact billToAddress { get; set; }
        
        [XMLElement(Name = "shipToAddress")]
        public contact shipToAddress { get; set; }
        
        [XMLElement(Name = "amount")]
        public int amount { get; set; }
        
        [XMLElement(Name = "eventType")]
        public eventTypeEnum? eventType { get; set; }
        
        [XMLElement(Name = "accountLogin")]
        public string accountLogin { get; set; }
        
        [XMLElement(Name = "accountPasshash")]
        public string accountPasshash { get; set; }
    }
}