/*
 * Zachary Cook
 * 
 * Fields for XML requests and responses. Refer to the XML
 * reference guides for further documentation.
 */

using System;
using System.Collections.Generic;
using System.Security;
using System.Xml.Linq;
using Cnp.Sdk.VersionedXML;

namespace Cnp.Sdk
{
    /*
     * Enum declarations.
     */
    public enum methodOfPaymentTypeEnum
    {
        MC,
        VI,
        AX,
        DC,
        DI,
        PP,
        JC,
        BL,
        EC,
        GC,
    }
    
    public enum countryTypeEnum
    {
        USA,
        AF,
        AX,
        AL,
        DZ,
        AS,
        AD,
        AO,
        AI,
        AQ,
        AG,
        AR,
        AM,
        AW,
        AU,
        AT,
        AZ,
        BS,
        BH,
        BD,
        BB,
        BY,
        BE,
        BZ,
        BJ,
        BM,
        BT,
        BO,
        BQ,
        BA,
        BW,
        BV,
        BR,
        IO,
        BN,
        BG,
        BF,
        BI,
        KH,
        CM,
        CA,
        CV,
        KY,
        CF,
        TD,
        CL,
        CN,
        CX,
        CC,
        CO,
        KM,
        CG,
        CD,
        CK,
        CR,
        CI,
        HR,
        CU,
        CW,
        CY,
        CZ,
        DK,
        DJ,
        DM,
        DO,
        TL,
        EC,
        EG,
        SV,
        GQ,
        ER,
        EE,
        ET,
        FK,
        FO,
        FJ,
        FI,
        FR,
        GF,
        PF,
        TF,
        GA,
        GM,
        GE,
        DE,
        GH,
        GI,
        GR,
        GL,
        GD,
        GP,
        GU,
        GT,
        GG,
        GN,
        GW,
        GY,
        HT,
        HM,
        HN,
        HK,
        HU,
        IS,
        IN,
        ID,
        IR,
        IQ,
        IE,
        IM,
        IL,
        IT,
        JM,
        JP,
        JE,
        JO,
        KZ,
        KE,
        KI,
        KP,
        KR,
        KW,
        KG,
        LA,
        LV,
        LB,
        LS,
        LR,
        LY,
        LI,
        LT,
        LU,
        MO,
        MK,
        MG,
        MW,
        MY,
        MV,
        ML,
        MT,
        MH,
        MQ,
        MR,
        MU,
        YT,
        MX,
        FM,
        MD,
        MC,
        MN,
        MS,
        MA,
        MZ,
        MM,
        NA,
        NR,
        NP,
        NL,
        AN,
        NC,
        NZ,
        NI,
        NE,
        NG,
        NU,
        NF,
        MP,
        NO,
        OM,
        PK,
        PW,
        PS,
        PA,
        PG,
        PY,
        PE,
        PH,
        PN,
        PL,
        PT,
        PR,
        QA,
        RE,
        RO,
        RU,
        RW,
        BL,
        KN,
        LC,
        MF,
        VC,
        WS,
        SM,
        ST,
        SA,
        SN,
        SC,
        SL,
        SG,
        SX,
        SK,
        SI,
        SB,
        SO,
        ZA,
        GS,
        ES,
        LK,
        SH,
        PM,
        SD,
        SR,
        SJ,
        SZ,
        SE,
        CH,
        SY,
        TW,
        TJ,
        TZ,
        TH,
        TG,
        TK,
        TO,
        TT,
        TN,
        TR,
        TM,
        TC,
        TV,
        UG,
        UA,
        AE,
        GB,
        US,
        UM,
        UY,
        UZ,
        VU,
        VA,
        VE,
        VN,
        VG,
        VI,
        WF,
        EH,
        YE,
        ZM,
        ZW,
        RS,
        ME,
    }
    
    public enum routingPreferenceEnum 
    {
        pinlessDebitOnly,
        signatureOnly,
        regular,
    }
    
    public enum echeckAccountTypeEnum
    {
        Checking,
        Savings,
        Corporate,
        CorpSavings,
    }
    
    public enum disbursementTypeEnum
    {
        VAA,
        VBB,
        VBI,
        VBP,
        VCC,
        VCI,
        VCO,
        VCP,
        VFD,
        VGD,
        VGP,
        VLO,
        VMA,
        VMD,
        VMI,
        VMP,
        VOG,
        VPD,
        VPG,
        VPP,
        VPS,
        VTU,
        VWT,
    }

    public enum actionTypeEnum
    {
        A,
        D,
        R,
        AR,
        G,
        I,
        J,
        L,
        LR,
        P,
        RR,
        S,
        T,
        UR,
        V,
        W,
        X
    }

    public enum yesNoTypeEnum
    {

        Y,
        N,
    }
    
    public enum customerInfoCustomerType
    {
        New,
        Existing,
    }

    public enum currencyCodeEnum
    {
        AUD,
        CAD,
        CHF,
        DKK,
        EUR,
        GBP,
        HKD,
        JPY,
        NOK,
        NZD,
        SEK,
        SGD,
        USD,
    }

    public enum customerInfoResidenceStatus
    {
        Own,
        Rent,
        Other,
    }

    public enum intervalType
    {
        ANNUAL,
        SEMIANNUAL,
        QUARTERLY,
        MONTHLY,
        WEEKLY
    }

    public enum trialIntervalType
    {
        MONTH,
        DAY
    }
    
    public enum lodgingProgramCodeType
    {
        LODGING = 0,
        NOSHOW,
        ADVANCEDDEPOSIT,
    }
    
    public enum lodgingExtraChargeEnum
    {
        OTHER = 0,
        RESTAURANT,
        GIFTSHOP,
        MINIBAR,
        TELEPHONE,
        LAUNDRY,

    }
    
    public enum walletWalletSourceType
    {
        MasterPass,
        VisaCheckout
    }
    
    public enum eventTypeEnum
    {
        payment,
        login,
        account_creation,
        details_change
    }

    public enum processingType
    {
        undefined,
        accountFunding,
        initialRecurring,
        initialInstallment,
        initialCOF,
        merchantInitiatedCOF,
        cardholderInitiatedCOF
    }

    public enum mandateProviderType
    {
        Merchant,
        Vantiv
    }

    public enum sequenceTypeType
    {
        OneTime,
        FirstRecurring,
        SubsequentRecurring,
        FinalRecurring
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
        private string value;

        public override string ToString()
        {
            return value;
        }
    }
    
    public enum ItemsChoiceType
    {
        expDate,
        number,
        track,
        type,
    }

    public enum posCapabilityTypeEnum
    {
        notused,
        magstripe,
        keyedonly,
    }

    public enum posEntryModeTypeEnum
    {
        notused,
        keyed,
        track1,
        track2,
        completeread,
    }

    public sealed class posCatLevelEnum
    {
        public static readonly posCatLevelEnum selfservice = new posCatLevelEnum("self service");

        private posCatLevelEnum(String value) { this.value = value; }
        private string value;

        public override string ToString()
        {
            return value;
        }
    }

    public enum posCardholderIdTypeEnum
    {
        signature,
        pin,
        nopin,
        directmarket,
    }

    public enum ItemChoiceType1
    {
        city,
        phone,
        url,
    }

    public enum govtTaxTypeEnum
    {
        payment,
        fee,
    }
    public enum enhancedDataDeliveryType
    {
        CNC,
        DIG,
        PHY,
        SVC,
        TBD,
    }
    
    
    public sealed class taxTypeIdentifierEnum
    {
        public static readonly taxTypeIdentifierEnum Item00 = new taxTypeIdentifierEnum("00");
        public static readonly taxTypeIdentifierEnum Item01 = new taxTypeIdentifierEnum("01");
        public static readonly taxTypeIdentifierEnum Item02 = new taxTypeIdentifierEnum("02");
        public static readonly taxTypeIdentifierEnum Item03 = new taxTypeIdentifierEnum("03");
        public static readonly taxTypeIdentifierEnum Item04 = new taxTypeIdentifierEnum("04");
        public static readonly taxTypeIdentifierEnum Item05 = new taxTypeIdentifierEnum("05");
        public static readonly taxTypeIdentifierEnum Item06 = new taxTypeIdentifierEnum("06");
        public static readonly taxTypeIdentifierEnum Item10 = new taxTypeIdentifierEnum("10");
        public static readonly taxTypeIdentifierEnum Item11 = new taxTypeIdentifierEnum("11");
        public static readonly taxTypeIdentifierEnum Item12 = new taxTypeIdentifierEnum("12");
        public static readonly taxTypeIdentifierEnum Item13 = new taxTypeIdentifierEnum("13");
        public static readonly taxTypeIdentifierEnum Item14 = new taxTypeIdentifierEnum("14");
        public static readonly taxTypeIdentifierEnum Item20 = new taxTypeIdentifierEnum("20");
        public static readonly taxTypeIdentifierEnum Item21 = new taxTypeIdentifierEnum("21");
        public static readonly taxTypeIdentifierEnum Item22 = new taxTypeIdentifierEnum("22");

        private taxTypeIdentifierEnum(String value) { this.value = value; }
        private string value;

        public override string ToString()
        {
            return value;
        }
    }

    public enum IIASFlagType
    {
        Y,
    }
    
    public enum recycleByTypeEnum
    {
        Merchant,
        Cnp,
        None,
    }
    
    public enum ItemChoiceType2
    {
        payerEmail,
        payerId,
    }

    public enum ItemsChoiceType1
    {
        amexAggregatorData,
        amount,
        billMeLaterRequest,
        billToAddress,
        card,
        customBilling,
        enhancedData,
        cnpTxnId,
        merchantData,
        orderId,
        orderSource,
        paypage,
        paypal,
        pos,
        processingInstructions,
        taxType,
        token,
    }

    public enum ItemsChoiceType3
    {
        amount,
        billToAddress,
        customBilling,
        echeck,
        echeckOrEcheckToken,
        echeckToken,
        cnpTxnId,
        merchantData,
        orderId,
        orderSource,
    }

    public enum ItemsChoiceType4
    {
        amount,
        billToAddress,
        customBilling,
        echeck,
        echeckOrEcheckToken,
        echeckToken,
        cnpTxnId,
        merchantData,
        orderId,
        orderSource,
        shipToAddress,
        verify,
    }

    public enum Item1ChoiceType
    {
        cardholderAuthentication,
        fraudCheck,
    }
    
    public enum ItemsChoiceType2
    {
        extendedCardResponse,
        newAccountInfo,
        newCardInfo,
        newCardTokenInfo,
        newTokenInfo,
        originalAccountInfo,
        originalCardInfo,
        originalCardTokenInfo,
        originalTokenInfo,
    }
    
    public enum accountUpdateSourceType {
        R,
        N,
    }
    
    public enum fundingSourceTypeEnum
    {
        UNKNOWN,
        PREPAID,
        FSA,
        CREDIT,
        DEBIT,
    }

    public enum affluenceTypeEnum
    {
        AFFLUENT,
        MASSAFFLUENT,
    }

    public enum cardProductTypeEnum
    {
        UNKNOWN,
        COMMERCIAL,
        CONSUMER,
    }
    
    /*
     * Element declarations.
     */
    [XMLElement(Name = "authentication")]
    public class authentication : VersionedXMLElement
    {
        [XMLElement(Name = "user")]
        public string user { get; set; }
        
        [XMLElement(Name = "password")]
        public string password { get; set; }
    }
    
    [XMLElement(Name = "fraudCheck")]
    public class fraudCheckType
    {
        [XMLElement(Name = "authenticationValue")]
        public string authenticationValue { get; set; }
        
        [XMLElement(Name = "authenticationTransactionId")]
        public string authenticationTransactionId { get; set; }
        
        [XMLElement(Name = "customerIpAddress")]
        public string customerIpAddress { get; set; }
        
        [XMLElement(Name = "authenticatedByMerchant")]
        public bool? authenticatedByMerchant { get; set; }
    }
    
    [XMLElement(Name = "customerInfo")]
    public class customerInfo : VersionedXMLElement
    {
        [XMLElement(Name = "ssn")]
        public string ssn { get; set; }
        
        [XMLElement(Name = "dob")]
        public DateTime dob { get; set; }
        
        [XMLElement(Name = "customerRegistrationDate")]
        public DateTime customerRegistrationDate { get; set; }
        
        [XMLElement(Name = "customerType")]
        public customerInfoCustomerType? customerType { get; set; }
        
        [XMLElement(Name = "incomeAmount")]
        public long? incomeAmount { get; set; }

        [XMLElement(Name = "incomeCurrency")]
        public currencyCodeEnum incomeCurrencyField { get; set; } = currencyCodeEnum.USD;
        
        [XMLElement(Name = "customerCheckingAccount")]
        public bool? customerCheckingAccount { get; set; }
        
        [XMLElement(Name = "customerSavingAccount")]
        public bool? customerSavingAccount { get; set; }
        
        [XMLElement(Name = "employerName")]
        public string employerName { get; set; }
        
        [XMLElement(Name = "customerWorkTelephone")]
        public string customerWorkTelephone { get; set; }
        
        [XMLElement(Name = "residenceStatus")]
        public customerInfoResidenceStatus? residenceStatus { get; set; }
        
        [XMLElement(Name = "yearsAtResidence")]
        public int? yearsAtResidence { get; set; }
        
        [XMLElement(Name = "yearsAtEmployer")]
        public int? yearsAtEmployer { get; set; }
    }
    
    [XMLElement(Name = "enhancedData")]
    public class enhancedData : VersionedXMLElement
    {
        [XMLElement(Name = "customerReference")]
        public string customerReference { get; set; }
        
        [XMLElement(Name = "salesTax")]
        public long? salesTax { get; set; }
        
        [XMLElement(Name = "deliveryType")]
        public enhancedDataDeliveryType? deliveryType { get; set; }
        
        [XMLElement(Name = "taxExempt")]
        public bool? taxExempt { get; set; }
        
        [XMLElement(Name = "discountAmount")]
        public long? discountAmount { get; set; }
        
        [XMLElement(Name = "shippingAmount")]
        public long? shippingAmount { get; set; }
        
        [XMLElement(Name = "dutyAmount")]
        public long? dutyAmount { get; set; }
        
        [XMLElement(Name = "shipFromPostalCode")]
        public string shipFromPostalCode { get; set; }
        
        [XMLElement(Name = "destinationPostalCode")]
        public string destinationPostalCode { get; set; }
        
        [XMLElement(Name = "destinationCountryCode")]
        public countryTypeEnum? destinationCountry { get; set; }
        
        [XMLElement(Name = "invoiceReferenceNumber")]
        public string invoiceReferenceNumber { get; set; }
        
        [XMLElement(Name = "orderDate")]
        public DateTime orderDate { get; set; }
        
        public List<detailTax> detailTaxes;
        public List<lineItemData> lineItems;
        
        public enhancedData()
        {
            this.lineItems = new List<lineItemData>();
            this.detailTaxes = new List<detailTax>();
        }

        /*
         * Returns additional elements to add when serializing.
         * This method must handle all escaping of special characters.
         */
        public override List<string> GetAdditionalElements(XMLVersion version)
        {
            // Serialize the elements.
            var elements = new List<string>();
            foreach (var element in this.lineItems)
            {
                elements.Add(element.Serialize(version));
            }
            foreach (var element in this.detailTaxes)
            {
                elements.Add(element.Serialize(version));
            }
            
            // Return the elements.
            return elements;
        }
    }
    
    [XMLElement(Name = "lineItemData")]
    public class lineItemData : VersionedXMLElement
    {
        [XMLElement(Name = "itemSequenceNumber")]
        public int? itemSequenceNumber { get; set; }
        
        [XMLElement(Name = "itemDescription")]
        public string itemDescription { get; set; }
        
        [XMLElement(Name = "productCode")]
        public string productCode { get; set; }
        
        [XMLElement(Name = "quantity")]
        public string quantity { get; set; }
        
        [XMLElement(Name = "unitOfMeasure")]
        public string unitOfMeasure { get; set; }
        
        [XMLElement(Name = "taxAmount")]
        public long? taxAmount { get; set; }
        
        [XMLElement(Name = "lineItemTotal")]
        public long? lineItemTotal { get; set; }
        
        [XMLElement(Name = "lineItemTotalWithTax")]
        public long? lineItemTotalWithTax { get; set; }
        
        [XMLElement(Name = "itemDiscountAmount")]
        public long? itemDiscountAmount { get; set; }
        
        [XMLElement(Name = "commodityCode")]
        public string commodityCode { get; set; }
        
        [XMLElement(Name = "unitCost")]
        public string unitCost { get; set; }
        
        List<detailTax> detailTaxes;

        public lineItemData()
        {
            detailTaxes = new List<detailTax>();
        }

        /*
         * Returns additional elements to add when serializing.
         * This method must handle all escaping of special characters.
         */
        public override List<string> GetAdditionalElements(XMLVersion version)
        {
            // Serialize the elements.
            var elements = new List<string>();
            foreach (var element in this.detailTaxes)
            {
                elements.Add(element.Serialize(version));
            }
            
            // Return the elements.
            return elements;
        }
    }
    
    [XMLElement(Name = "detailTax")]
    public class detailTax : VersionedXMLElement
    {
        [XMLElement(Name = "taxIncludedInTotal")]
        public bool? taxIncludedInTotal { get; set; }
        
        [XMLElement(Name = "taxAmount")]
        public long? taxAmount { get; set; }
        
        [XMLElement(Name = "taxRate")]
        public string taxRate { get; set; }
        
        [XMLElement(Name = "taxTypeIdentifier")]
        public taxTypeIdentifierEnum taxTypeIdentifier { get; set; }
        
        [XMLElement(Name = "cardAcceptorTaxId")]
        public string cardAcceptorTaxId { get; set; }
    }
    
    [XMLElement(Name = "echeckForToken")]
    public class echeckForTokenType : VersionedXMLElement
    {
        
        [XMLElement(Name = "accNum")]
        public string accNum { get; set; }
        
        [XMLElement(Name = "routingNum")]
        public string routingNum { get; set; }
    }
    
    [XMLElement(Name = "echeck")]
    public class echeckType : VersionedXMLElement
    {
        [XMLElement(Name = "accType")]
        public echeckAccountTypeEnum? accType { get; set; }

        [XMLElement(Name = "accNum")]
        public string accNum { get; set; }
        
        [XMLElement(Name = "routingNum")]
        public string routingNum { get; set; }
        
        [XMLElement(Name = "checkNum")]
        public string checkNum { get; set; }
        
        [XMLElement(Name = "ccdPaymentInformation")]
        public string ccdPaymentInformation { get; set; }
        
        public string[] ctxPaymentInformation { get; set; }
        
        /*
         * Returns additional elements to add when serializing.
         * This method must handle all escaping of special characters.
         */
        public override List<string> GetAdditionalElements(XMLVersion version)
        {
            // Serialize the information if it exists.
            if (this.ctxPaymentInformation != null)
            {
                var element = "<ctxPaymentInformation>";
                foreach (var detail in this.ctxPaymentInformation)
                {
                    element += "<ctxPaymentDetail>" + SecurityElement.Escape(detail) + "</ctxPaymentDetail>";
                }
                element += "<ctxPaymentInformation>";
                
                // Return the element.
                return new List<string>() { element };
            }
            
            // Return null (no elements to add).
            return null;
        }
    }
    
    [XMLElement(Name = "echeckToken")]
    public class echeckTokenType : VersionedXMLElement
    {
        [XMLElement(Name = "cnpToken")]
        public string cnpToken { get; set; }
        
        [XMLElement(Name = "routingNum")]
        public string routingNum { get; set; }
        
        [XMLElement(Name = "accType")]
        public echeckAccountTypeEnum? accType { get; set; }
        
        [XMLElement(Name = "checkNum")]
        public string checkNum { get; set; }
    }
    
    [XMLElement(Name = "pos")]
    public class pos : VersionedXMLElement
    {
        [XMLElement(Name = "capability")]
        public posCapabilityTypeEnum? capability { get; set; }
        
        [XMLElement(Name = "entryMode")]
        public posEntryModeTypeEnum? entryMode { get; set; }
        
        [XMLElement(Name = "cardholderId")]
        public posCardholderIdTypeEnum? cardholderId { get; set; }
        
        [XMLElement(Name = "terminalId")]
        public string terminalId { get; set; }
        
        [XMLElement(Name = "catLevel")]
        public posCatLevelEnum catLevel { get; set; }
    }
    
    [XMLElement(Name = "payPal")]
    public class payPal : VersionedXMLElement
    {
        [XMLElement(Name = "payerId")]
        public string payerId { get; set; }

        [XMLElement(Name = "payerEmail")]
        public string payerEmail { get; set; }

        [XMLElement(Name = "token")]
        public string token { get; set; }

        [XMLElement(Name = "transactionId")]
        public string transactionId { get; set; }
    }
    
    [XMLElement(Name = "merchantData")]
    public class merchantDataType : VersionedXMLElement
    {
        [XMLElement(Name = "campaign")]
        public string campaign { get; set; }
        
        [XMLElement(Name = "affiliate")]
        public string affiliate { get; set; }
        
        [XMLElement(Name = "merchantGroupingId")]
        public string merchantGroupingId { get; set; }
    }
    
    [XMLElement(Name = "cardToken")]
    public class cardTokenType : VersionedXMLElement
    {
        [XMLElement(Name = "cnpToken")]
        public string cnpToken { get; set; }
        
        [XMLElement(Name = "tokenURL")]
        public string tokenUrl { get; set; }
        
        [XMLElement(Name = "expDate")]
        public string expDate { get; set; }
        
        [XMLElement(Name = "cardValidationNum")]
        public string cardValidationNum { get; set; }
        
        [XMLElement(Name = "type")]
        public methodOfPaymentTypeEnum? type { get; set; }
        
        [XMLElement(Name = "checkoutId")]
        public string checkoutId { get; set; }
    }
    
    [XMLElement(Name = "cardPaypage")]
    public class cardPaypageType : VersionedXMLElement
    {
        [XMLElement(Name = "paypageRegistrationId")]
        public string paypageRegistrationId { get; set; }
        
        [XMLElement(Name = "expDate")]
        public string expDate { get; set; }
        
        [XMLElement(Name = "cardValidationNum")]
        public string cardValidationNum { get; set; }
        
        [XMLElement(Name = "type")]
        public methodOfPaymentTypeEnum? type { get; set; }
    }
    
    [XMLElement(Name = "customBilling")]
    public class customBilling : VersionedXMLElement
    {
        [XMLElement(Name = "phone")]
        public string phone { get; set; }
        
        [XMLElement(Name = "city")]
        public string city { get; set; }
        
        [XMLElement(Name = "url")]
        public string url { get; set; }
        
        [XMLElement(Name = "descriptor")]
        public string descriptor { get; set; }
    }
    
    [XMLElement(Name = "processingInstructions")]
    public class processingInstructions : VersionedXMLElement
    {
        [XMLElement(Name = "bypassVelocityCheck")]
        public bool? bypassVelocityCheck { get; set; }
    }

    [XMLElement(Name = "fraudResult")]
    public class fraudResult : VersionedXMLElement
    {
        [XMLElement(Name = "avsResult")]
        public string avsResult { get; set; }
        
        [XMLElement(Name = "cardValidationResult")]
        public string cardValidationResult { get; set; }
        
        [XMLElement(Name = "authenticationResult")]
        public string authenticationResult { get; set; }
        
        [XMLElement(Name = "advancedAVSResult")]
        public string advancedAVSResult { get; set; }
        
        [XMLElement(Name = "advancedFraudResults")]
        public advancedFraudResultsType advancedFraudResults { get; set; }
    }
    
    [XMLElement(Name = "advancedFraudResults")]
    public class advancedFraudResultsType : VersionedXMLElement
    {
        [XMLElement(Name = "deviceReviewStatus")]
        public string deviceReviewStatus { get; set; }
        
        [XMLElement(Name = "deviceReputationScore")]
        public int? deviceReputationScore { get; set; }
        
        public string[] triggeredRuleField;
            
        /*
         * Parses elements that aren't defined by properties.
         */
        public override void ParseAdditionalElements(XMLVersion version,List<string> elements)
        {
            // Get the rule field.
            var newTriggeredRuleField = new List<string>();
            foreach (var element in elements)
            {
                var xmlDocument = XDocument.Parse(element);
                var elementName = xmlDocument.Root.Name.LocalName;
                
                // Add the triggered rule field.
                newTriggeredRuleField.Add(xmlDocument.Root.Value);
            }
            
            // Set the rule fields.
            this.triggeredRuleField = newTriggeredRuleField.ToArray();
        }
    }
    
    [XMLElement(Name = "authInformation")]
    public class authInformation : VersionedXMLElement
    {
        [XMLElement(Name = "authDate")]
        public DateTime authDate { get; set; }
        
        [XMLElement(Name = "authCode")]
        public string authCode { get; set; }
        
        [XMLElement(Name = "fraudResult")]
        public fraudResult fraudResult { get; set; }
        
        [XMLElement(Name = "authAmount")]
        public long? authAmount { get; set; }
    }
    
    [XMLElement(Name = "recyclingRequest")]
    public class recyclingRequestType : VersionedXMLElement
    {
        [XMLElement(Name = "recycleBy")]
        public recycleByTypeEnum? recycleBy { get; set; }
        
        [XMLElement(Name = "recycleId")]
        public string recycleId { get; set; }
    }
    
    [XMLElement(Name = "cnpInternalRecurringRequest")]
    public class cnpInternalRecurringRequest : VersionedXMLElement
    {
        [XMLElement(Name = "subscriptionId")]
        public string subscriptionId { get; set; }
        
        [XMLElement(Name = "recurringTxnId")]
        public string recurringTxnId { get; set; }
        
        [XMLElement(Name = "finalPayment")]
        public bool? finalPayment { get; set; }
    }
    
    [XMLElement(Name = "createDiscount")]
    public class createDiscount : VersionedXMLElement
    {
        [XMLElement(Name = "discountCode")]
        public string discountCode { get; set; }
        
        [XMLElement(Name = "name")]
        public string name { get; set; }
        
        [XMLElement(Name = "amount")]
        public long? amount { get; set; }
        
        [XMLElement(Name = "startDate")]
        public DateTime startDate { get; set; }
        
        [XMLElement(Name = "endDate")]
        public DateTime endDate { get; set; }
    }
    
    [XMLElement(Name = "updateDiscount")]
    public class updateDiscount : VersionedXMLElement
    {
        [XMLElement(Name = "discountCode")]
        public string discountCode { get; set; }
        
        [XMLElement(Name = "name")]
        public string name { get; set; }
        
        [XMLElement(Name = "amount")]
        public long? amount { get; set; }
        
        [XMLElement(Name = "startDate")]
        public DateTime startDate { get; set; }
        
        [XMLElement(Name = "endDate")]
        public DateTime endDate { get; set; }
    }
    
    [XMLElement(Name = "deleteDiscount")]
    public class deleteDiscount : VersionedXMLElement
    {
        [XMLElement(Name = "discountCode")]
        public string discountCode { get; set; }
    }
    
    [XMLElement(Name = "createAddOn")]
    public class createAddOn : VersionedXMLElement
    {
        [XMLElement(Name = "addOnCode")]
        public string addOnCode { get; set; }
        
        [XMLElement(Name = "name")]
        public string name { get; set; }
        
        [XMLElement(Name = "amount")]
        public long? amount { get; set; }
        
        [XMLElement(Name = "startDate")]
        public DateTime startDate { get; set; }
        
        [XMLElement(Name = "endDate")]
        public DateTime endDate { get; set; }
    }
    
    [XMLElement(Name = "updateAddOn")]
    public class updateAddOn : VersionedXMLElement
    {
        [XMLElement(Name = "addOnCode")]
        public string addOnCode { get; set; }
        
        [XMLElement(Name = "name")]
        public string name { get; set; }
        
        [XMLElement(Name = "amount")]
        public long? amount { get; set; }
        
        [XMLElement(Name = "startDate")]
        public DateTime startDate { get; set; }
        
        [XMLElement(Name = "endDate")]
        public DateTime endDate { get; set; }
    }
    
    [XMLElement(Name = "deleteAddOn")]
    public class deleteAddOn : VersionedXMLElement
    {
        [XMLElement(Name = "addOnCode")]
        public string addOnCode { get; set; }
    }
    
    [XMLElement(Name = "subscription")]
    public class subscription : VersionedXMLElement
    {
        [XMLElement(Name = "planCode")]
        public string planCode { get; set; }
        
        [XMLElement(Name = "numberOfPayments")]
        public int? numberOfPayments { get; set; }
        
        [XMLElement(Name = "startDate")]
        public DateTime startDate { get; set; }
        
        [XMLElement(Name = "amount")]
        public long? amount { get; set; }

        public List<createDiscount> createDiscounts;
        public List<createAddOn> createAddOns;

        public subscription()
        {
            createDiscounts = new List<createDiscount>();
            createAddOns = new List<createAddOn>();
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
            foreach (var element in this.createAddOns)
            {
                elements.Add(element.Serialize(version));
            }
            
            // Return the elements.
            return elements;
        }
    }
    
    [XMLElement(Name = "filteringType")]
    public class filteringType : VersionedXMLElement
    {
        [XMLElement(Name = "prepaid")]
        public bool? prepaid { get; set; }
        
        [XMLElement(Name = "international")]
        public bool? international { get; set; }
        
        [XMLElement(Name = "chargeback")]
        public bool? chargeback { get; set; }
    }
    
    [XMLElement(Name = "healthcareIIAS")]
    public class healthcareIIAS : VersionedXMLElement
    {
        [XMLElement(Name = "healthcareAmounts")]
        public healthcareAmounts healthcareAmounts { get; set; }
        
        [XMLElement(Name = "IIASFlag")]
        public IIASFlagType? IIASFlag { get; set; }
    }
    
    [XMLElement(Name = "lodgingInfo")]
    public class lodgingInfo : VersionedXMLElement
    {
        [XMLElement(Name = "hotelFolioNumber")]
        public string hotelFolioNumber { get; set; }
        
        [XMLElement(Name = "checkInDate")]
        public DateTime checkInDate { get; set; }
        
        [XMLElement(Name = "checkOutDate")]
        public DateTime checkOutDate { get; set; }
        
        [XMLElement(Name = "duration")]
        public int? duration { get; set; }
        
        [XMLElement(Name = "customerServicePhone")]
        public string customerServicePhone { get; set; }

        [XMLElement(Name = "programCode")]
        public lodgingProgramCodeType? programCode { get; set; } = lodgingProgramCodeType.LODGING;
        
        [XMLElement(Name = "roomRate")]
        public int? roomRate { get; set; }
        
        [XMLElement(Name = "roomTax")]
        public int? roomTax { get; set; }
        
        [XMLElement(Name = "numAdults")]
        public int? numAdults { get; set; }
        
        [XMLElement(Name = "propertyLocalPhone")]
        public string propertyLocalPhone { get; set; }
        
        [XMLElement(Name = "fireSafetyIndicator")]
        public bool? fireSafetyIndicator { get; set; }

        public List<lodgingCharge> lodgingCharges;

        public lodgingInfo()
        {
            lodgingCharges = new List<lodgingCharge>();
        }
        
        /*
         * Returns additional elements to add when serializing.
         * This method must handle all escaping of special characters.
         */
        public override List<string> GetAdditionalElements(XMLVersion version)
        {
            // Serialize the elements.
            var elements = new List<string>();
            foreach (var element in this.lodgingCharges)
            {
                elements.Add(element.Serialize(version));
            }
            
            // Return the elements.
            return elements;
        }
    }
    
    [XMLElement(Name = "lodgingCharge")]
    public class lodgingCharge : VersionedXMLElement
    {
        [XMLElement(Name = "name")]
        public lodgingExtraChargeEnum? name { get; set; }
    }
    
    [XMLElement(Name = "recurringRequest")]
    public class recurringRequest : VersionedXMLElement
    {
        [XMLElement(Name = "subscription")]
        public subscription subscription { get; set; }
    }
    
    [XMLElement(Name = "healthcareAmounts")]
    public class healthcareAmounts : VersionedXMLElement
    {
        [XMLElement(Name = "totalHealthcareAmount")]
        public int? totalHealthcareAmount { get; set; }
        
        [XMLElement(Name = "RxAmount")]
        public int? RxAmount { get; set; }
        
        [XMLElement(Name = "visionAmount")]
        public int? visionAmount { get; set; }
        
        [XMLElement(Name = "clinicOtherAmount")]
        public int? clinicOtherAmount { get; set; }
        
        [XMLElement(Name = "dentalAmount")]
        public int? dentalAmount { get; set; }
    }
    
    [XMLElement(Name = "contact")]
    public class contact : VersionedXMLElement
    {
        [XMLElement(Name = "name")]
        public string name { get; set; }
        
        [XMLElement(Name = "firstName")]
        public string firstName { get; set; }
        
        [XMLElement(Name = "middleInitial")]
        public string middleInitial { get; set; }
        
        [XMLElement(Name = "lastName")]
        public string lastName { get; set; }
        
        [XMLElement(Name = "companyName")]
        public string companyName { get; set; }
        
        [XMLElement(Name = "addressLine1")]
        public string addressLine1 { get; set; }

        [XMLElement(Name = "addressLine2")]
        public string addressLine2 { get; set; }
        
        [XMLElement(Name = "addressLine3")]
        public string addressLine3 { get; set; }
        
        [XMLElement(Name = "city")]
        public string city { get; set; }
        
        [XMLElement(Name = "state")]
        public string state { get; set; }
        
        [XMLElement(Name = "zip")]
        public string zip { get; set; }
        
        [XMLElement(Name = "country")]
        public countryTypeEnum? country { get; set; }
        
        [XMLElement(Name = "email")]
        public string email { get; set; }
        
        [XMLElement(Name = "phone")]
        public string phone { get; set; }
    }

    [XMLElement(Name = "advancedFraudChecks")]
    public class advancedFraudChecksType : VersionedXMLElement
    {
        [XMLElement(Name = "threatMetrixSessionId")]
        public string threatMetrixSessionId { get; set; }
        
        [XMLElement(Name = "customAttribute1")]
        public string customAttribute1 { get; set; }
        
        [XMLElement(Name = "customAttribute2")]
        public string customAttribute2 { get; set; }
        
        [XMLElement(Name = "customAttribute3")]
        public string customAttribute3 { get; set; }
        
        [XMLElement(Name = "customAttribute4")]
        public string customAttribute4 { get; set; }
        
        [XMLElement(Name = "customAttribute5")]
        public string customAttribute5 { get; set; }
    }
    
    [XMLElement(Name = "mpos")]
    public class mposType : VersionedXMLElement
    {
        [XMLElement(Name = "ksn")]
        public string ksn { get; set; }
        
        [XMLElement(Name = "formatId")]
        public string formatId { get; set; }
        
        [XMLElement(Name = "encryptedTrack")]
        public string encryptedTrack { get; set; }
        
        [XMLElement(Name = "track1Status")]
        public int? track1Status { get; set; }
        
        [XMLElement(Name = "track2Status")]
        public int? track2Status { get; set; }
    }
    
    [XMLElement(Name = "card")]
    public class cardType : VersionedXMLElement
    {
        [XMLElement(Name = "type")]
        public methodOfPaymentTypeEnum? type { get; set; }
        
        [XMLElement(Name = "number")]
        public string number { get; set; }
        
        [XMLElement(Name = "expDate")]
        public string expDate { get; set; }
        
        [XMLElement(Name = "track")]
        public string track { get; set; }
        
        [XMLElement(Name = "cardValidationNum")]
        public string cardValidationNum { get; set; }
        
        [XMLElement(Name = "pin")]
        public string pin { get; set; }
    }
    
    [XMLElement(Name = "giftCardCard")]
    public class giftCardCardType : VersionedXMLElement
    {
        [XMLElement(Name = "type")]
        public methodOfPaymentTypeEnum? type { get; set; }
        
        [XMLElement(Name = "number")]
        public string number { get; set; }
        
        [XMLElement(Name = "expDate")]
        public string expDate { get; set; }
        
        [XMLElement(Name = "track")]
        public string track { get; set; }
        
        [XMLElement(Name = "cardValidationNum")]
        public string cardValidationNum { get; set; }
        
        [XMLElement(Name = "pin")]
        public string pin { get; set; }
    }
    
    [XMLElement(Name = "virtualGiftCard")]
    public class virtualGiftCardType : VersionedXMLElement
    {
        [XMLElement(Name = "accountNumberLength")]
        public int? accountNumberLength { get; set; }
        
        [XMLElement(Name = "giftCardBin")]
        public string giftCardBin { get; set; }
    }
    
    [XMLElement(Name = "accountUpdateFileRequestData")]
    public class accountUpdateFileRequestData : VersionedXMLElement
    {
        [XMLElement(Name = "merchantId")]
        public string merchantId { get; set; }
        
        [XMLElement(Name = "postDay")]
        public DateTime postDay { get; set; }
    }
    
    [XMLElement(Name = "applepay")]
    public class applepayType : VersionedXMLElement
    {
        [XMLElement(Name = "data")]
        public string data { get; set; }
        
        [XMLElement(Name = "header")]
        public applepayHeaderType header { get; set; }
        
        [XMLElement(Name = "signature")]
        public string signature { get; set; }
        
        [XMLElement(Name = "version")]
        public string version { get; set; }
    }
    
    [XMLElement(Name = "applepayHeader")]
    public class applepayHeaderType : VersionedXMLElement
    {
        [XMLElement(Name = "applicationData")]
        public string applicationData { get; set; }
        
        [XMLElement(Name = "ephemeralPublicKey")]
        public string ephemeralPublicKey { get; set; }
        
        [XMLElement(Name = "publicKeyHash")]
        public string publicKeyHash { get; set; }
        
        [XMLElement(Name = "transactionId")]
        public string transactionId { get; set; }
    }
    
    [XMLElement(Name = "wallet")]
    public class wallet : VersionedXMLElement
    {
        [XMLElement(Name = "walletSourceType")]
        public walletWalletSourceType walletSourceType { get; set; }
        
        [XMLElement(Name = "walletSourceTypeId")]
        public string walletSourceTypeId { get; set; }
    }
    
    [XMLElement(Name = "pinlessDebitRequest")]
    public class pinlessDebitRequestType : VersionedXMLElement
    {
        [XMLElement(Name = "routingPreference")]
        public routingPreferenceEnum? routingPreference { get; set; }
        
        [XMLElement(Name = "preferredDebitNetworks")]
        public preferredDebitNetworksType preferredDebitNetworks { get; set; }
    }
    
    [XMLElement(Name = "preferredDebitNetworks")]
    public class preferredDebitNetworksType : VersionedXMLElement
    {
        public List<string> debitNetworkName;

        public preferredDebitNetworksType()
        {
            debitNetworkName = new List<string>();
        }
            
        /*
         * Returns additional elements to add when serializing.
         * This method must handle all escaping of special characters.
         */
        public override List<string> GetAdditionalElements(XMLVersion version)
        {
            // Serialize the elements.
            var elements = new List<string>();
            foreach (var element in this.debitNetworkName)
            {
                elements.Add("<debitNetworkName>" + SecurityElement.Escape(element) + "</debitNetworkName>");
            }
            
            // Return the elements.
            return elements;
        }
    }
    
    [XMLElement(Name = "sepaDirectDebit")]
    public class sepaDirectDebitType : VersionedXMLElement
    {
        [XMLElement(Name = "mandateProvider")]
        public mandateProviderType mandateProvider { get; set; }
        
        [XMLElement(Name = "sequenceType")]
        public sequenceTypeType sequenceType { get; set; }
        
        [XMLElement(Name = "mandateReference")]
        public string mandateReference { get; set; }
        
        [XMLElement(Name = "mandateUrl")]
        public string mandateUrl { get; set; }
        
        [XMLElement(Name = "mandateSignatureDate")]
        public DateTime mandateSignatureDate { get; set; }
        
        [XMLElement(Name = "iban")]
        public string iban { get; set; }
        
        [XMLElement(Name = "preferredLanguage")]
        public countryTypeEnum preferredLanguage { get; set; }
    }
    
    [XMLElement(Name = "ideal")]
    public class idealType : VersionedXMLElement
    {
        [XMLElement(Name = "preferredLanguage")]
        public countryTypeEnum preferredLanguage { get; set; }
    }
    
    [XMLElement(Name = "giropay")]
    public class giropayType : VersionedXMLElement
    {
        [XMLElement(Name = "preferredLanguage")]
        public countryTypeEnum? preferredLanguage { get; set; }
    }
    
    [XMLElement(Name = "sofort")]
    public class sofortType : VersionedXMLElement
    {
        [XMLElement(Name = "preferredLanguage")]
        public countryTypeEnum? preferredLanguage { get; set; }
    }
    
    [XMLElement(Name = "driversLicenseInfo")]
    public class driversLicenseInfo : VersionedXMLElement
    {
        [XMLAttribute(Name = "licenseNumber")]
        public string licenseNumber { get; set; }
        
        [XMLAttribute(Name = "state")]
        public string state { get; set; }
        
        [XMLAttribute(Name = "dateOfBirth")]
        public string dateOfBirth { get; set; }
    }

    [XMLElement(Name = "recycleAdviceType")]
    public class recycleAdviceType : VersionedXMLElement
    {
        [XMLAttribute(Name = "nextRecycleTime")]
        public DateTime nextRecycleTime { get; set; }
        
        [XMLAttribute(Name = "recycleAdviceEnd")]
        public string recycleAdviceEnd { get; set; }
    }
    
    
    <xs:complexType name="recyclingResponseType">
    <xs:sequence>
    <xs:element name="recycleAdvice" type="xp:recycleAdviceType" minOccurs="0"/>
    <xs:element name="recycleEngineActive" type="xs:boolean" minOccurs="0"/>
    </xs:sequence>
    </xs:complexType>
}