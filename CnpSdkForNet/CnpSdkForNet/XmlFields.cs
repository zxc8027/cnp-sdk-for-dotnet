/*
 * Zachary Cook
 * 
 * Fields for XML requests and responses. Refer to the XML
 * reference guides for further documentation.
 */

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
    
    [XMLElement(Name = "echeckType")]
    public partial class echeckType : VersionedXMLElement
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
}