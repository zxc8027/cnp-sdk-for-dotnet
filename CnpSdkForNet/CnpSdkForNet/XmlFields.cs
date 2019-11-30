/*
 * Zachary Cook
 * 
 * Fields for XML requests and responses. Refer to the XML
 * reference guides for further documentation.
 */

using System;
using System.Collections.Generic;
using System.Security;
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
    
    [XMLElement(Name = "fraudCheckType")]
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
        public taxTypeIdentifierEnum? taxTypeIdentifier { get; set; }
        
        [XMLElement(Name = "cardAcceptorTaxId")]
        public string cardAcceptorTaxId { get; set; }
    }
    
    [XMLElement(Name = "echeckForTokenType")]
    public class echeckForTokenType : VersionedXMLElement
    {
        
        [XMLElement(Name = "accNum")]
        public string accNum { get; set; }
        
        [XMLElement(Name = "routingNum")]
        public string routingNum { get; set; }
    }
    
    [XMLElement(Name = "echeckType")]
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
    
    [XMLElement(Name = "echeckTokenType")]
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
        public posCatLevelEnum? catLevel { get; set; }
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
    
    [XMLElement(Name = "merchantDataType")]
    public class merchantDataType : VersionedXMLElement
    {
        [XMLElement(Name = "campaign")]
        public string campaign { get; set; }
        
        [XMLElement(Name = "affiliate")]
        public string affiliate { get; set; }
        
        [XMLElement(Name = "merchantGroupingId")]
        public string merchantGroupingId { get; set; }
    }
    
    [XMLElement(Name = "cardTokenType")]
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
    
    [XMLElement(Name = "cardPaypageType")]
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
        public authenticationResultType? authenticationResult { get; set; }
        
        [XMLElement(Name = "advancedAVSResult")]
        public string advancedAVSResult { get; set; }
        
        [XMLElement(Name = "advancedFraudResults")]
        public advancedFraudResultsType? advancedFraudResults { get; set; }
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
    
    [XMLElement(Name = "recyclingRequestType")]
    public class recyclingRequestType : VersionedXMLElement
    {
        [XMLElement(Name = "recycleBy")]
        public recycleByTypeEnum? recycleBy { get; set; }
        
        [XMLElement(Name = "recycleId")]
        public string recycleId { get; set; }
    }

}