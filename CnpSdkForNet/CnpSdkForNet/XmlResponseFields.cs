/*
 * Zachary Cook
 * 
 * Fields for XML responses. Refer to the XML reference guides
 * for further documentation.
 *
 * TODO: Validate inheritance and properties
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using System.Xml.Serialization;
using Cnp.Sdk.VersionedXML;

namespace Cnp.Sdk
{
    [XMLElement(Name = "recurringTransactionResponseType")]
    public class recurringTransactionResponseType : VersionedXMLElement
    {
        [XMLElement(Name = "cnpTxnId")]
        public string cnpTxnId { get; set; }
        
        [XMLElement(Name = "response")]
        public string response { get; set; }
        
        [XMLElement(Name = "message")]
        public string message { get; set; }
        
        [XMLElement(Name = "responseTime")]
        public DateTime responseTime { get; set; }
    }
    
    [XMLElement(Name = "cnpOnlineResponse")]
    public class cnpOnlineResponse : VersionedXMLElement
    {
        [XMLAttribute(Name = "response")]
        public string response { get; set; }

        [XMLAttribute(Name = "message")]
        public string message { get; set; }
        
        [XMLAttribute(Name = "version")]
        public string version { get; set; }

        private Dictionary<Type,transactionType> responses = new Dictionary<Type,transactionType>();
        
        /*
         * Parses elements that aren't defined by properties.
         */
        public override void ParseAdditionalElements(XMLVersion version,List<string> elements)
        {
            foreach (var element in elements)
            {
                // Get the element name.
                var xmlDocument = XDocument.Parse(element);
                var elementName = xmlDocument.Root.Name.LocalName;
                
                // Get the type.
                var assembly = typeof(cnpOnlineResponse).Assembly;
                Type responseType = null;
                foreach (var type in assembly.GetTypes())
                {
                    // Get the name from the attribute.
                    foreach (XMLElement attribute in type.GetCustomAttributes(typeof(XMLElement),true))
                    {
                        if (attribute.IsVersionValid(version) && attribute.Name.ToLower() == elementName.ToLower())
                        {
                            responseType = type;
                        }
                    }
                    
                    // Set the name if it isn't defined.
                    if (responseType == null && type.Name == elementName)
                    {
                        responseType = type;
                    }
                }

                // Deserialize the object.
                if (responseType == null)
                {
                    throw new CnpOnlineException("Unable to parse CNP Online response \"" + elementName + "\". Contact SDK support.");
                }
                else
                {
                    var responseObject = VersionedXMLDeserializer.DeserializeType(element,version,responseType);
                    this.responses.Add(responseType,(transactionType) responseObject);
                }
            }
        }
        
        /*
         * Returns the response for a given type.
         */
        public T GetResponse<T>()
        {
            // Throw an exception if the type doesn't exist.
            if (!this.responses.ContainsKey(typeof(T)))
            {
                if (this.responses.Keys.Count > 0)
                {
                    throw new CnpOnlineException("\"" + typeof(T) + "\" isn't in the response. Did you mean to use \"" + this.responses.Keys.First() + "\"?");
                }
                else
                {
                    throw new CnpOnlineException("\"" + typeof(T) + "\" isn't in the response. The response didn't have any child responses.");
                }
                
            }
            
            // Return the response.
            var responseObject = this.responses[typeof(T)];
            return (T) Convert.ChangeType(responseObject,typeof(T));
        }
    }
    
    [XMLElement(Name = "vendorDebitResponse")]
    public class recurringResponse : VersionedXMLElement
    {
        [XMLAttribute(Name = "subscriptionIdField")]
        public long? subscriptionIdField { get; set; }
        
        [XMLElement(Name = "responseCodeField")]
        public string responseCodeField { get; set; }

        [XMLElement(Name = "responseMessageField")]
        public string responseMessageField { get; set; }

        [XMLElement(Name = "recurringTxnIdField")]
        public long? recurringTxnIdField { get; set; }
    }
    
    [XMLElement(Name = "voidRecyclingResponseType")]
    public class voidRecyclingResponseType : VersionedXMLElement
    {
        [XMLAttribute(Name = "creditCnpTxnId")]
        public long? creditCnpTxnId { get; set; }
    }
    
    [XMLElement(Name = "tokenResponseType")]
    public class tokenResponseType : VersionedXMLElement
    {
        [XMLAttribute(Name = "cnpToken")]
        public string cnpToken { get; set; }
        
        [XMLAttribute(Name = "tokenResponseCode")]
        public string tokenResponseCode { get; set; }
        
        [XMLAttribute(Name = "tokenMessage")]
        public string tokenMessage { get; set; }
        
        [XMLAttribute(Name = "type")]
        public methodOfPaymentTypeEnum? type { get; set; }
        
        [XMLAttribute(Name = "typeSpecified")]
        public bool? typeSpecified { get; set; }
        
        [XMLAttribute(Name = "bin")]
        public string bin { get; set; }
        
        [XMLAttribute(Name = "eCheckAccountSuffix")]
        public string eCheckAccountSuffix { get; set; }
    }
    
    [XMLElement(Name = "cardTokenInfoType")]
    public class cardTokenInfoType : VersionedXMLElement
    {
        [XMLAttribute(Name = "cnpToken")]
        public string cnpToken { get; set; }
        
        [XMLAttribute(Name = "type")]
        public methodOfPaymentTypeEnum? type { get; set; }
        
        [XMLAttribute(Name = "expDate")]
        public string expDate { get; set; }
        
        [XMLAttribute(Name = "bin")]
        public string bin { get; set; }
    }
    
    [XMLElement(Name = "extendedCardResponseType")]
    public class extendedCardResponseType : VersionedXMLElement
    {
        [XMLAttribute(Name = "message")]
        public string message { get; set; }
        
        [XMLAttribute(Name = "code")]
        public string code { get; set; }
    }
    
    [XMLElement(Name = "cardAccountInfoType")]
    public class cardAccountInfoType : VersionedXMLElement
    {
        [XMLAttribute(Name = "type")]
        public methodOfPaymentTypeEnum? type { get; set; }
        
        [XMLAttribute(Name = "number")]
        public string number { get; set; }
        
        [XMLAttribute(Name = "expDate")]
        public string expDate { get; set; }
    }
    
    [XMLElement(Name = "echeckTokenInfoType")]
    public class echeckTokenInfoType : VersionedXMLElement
    {
        [XMLAttribute(Name = "accType")]
        public echeckAccountTypeEnum? accType { get; set; }
        
        [XMLAttribute(Name = "cnpToken")]
        public string cnpToken { get; set; }
        
        [XMLAttribute(Name = "routingNum")]
        public string routingNum { get; set; }
    }
    
    [XMLElement(Name = "echeckAccountInfoType")]
    public class echeckAccountInfoType : VersionedXMLElement
    {
        [XMLAttribute(Name = "accType")]
        public echeckAccountTypeEnum? accType { get; set; }
        
        [XMLAttribute(Name = "accNum")]
        public string accNum { get; set; }
        
        [XMLAttribute(Name = "routingNum")]
        public string routingNum { get; set; }
    }
    
    [XMLElement(Name = "accountInfoType")]
    public class accountInfoType : VersionedXMLElement
    {
        [XMLAttribute(Name = "type")]
        public methodOfPaymentTypeEnum? type { get; set; }
        
        [XMLAttribute(Name = "number")]
        public string number { get; set; }
    }
    
    [XMLElement(Name = "registerTokenResponse")]
    public class registerTokenResponse : VersionedXMLElement
    {
        [XMLAttribute(Name = "cnpTxnId")]
        public long cnpTxnId { get; set; }
        
        [XMLAttribute(Name = "cnpToken")]
        public string cnpToken { get; set; }
        
        [XMLAttribute(Name = "bin")]
        public string bin { get; set; }
        
        [XMLAttribute(Name = "type")]
        public methodOfPaymentTypeEnum? type { get; set; }
        
        [XMLAttribute(Name = "eCheckAccountSuffix")]
        public string eCheckAccountSuffix { get; set; }
        
        [XMLAttribute(Name = "response")]
        public string response { get; set; }
        
        [XMLAttribute(Name = "message")]
        public string message { get; set; }
        
        [XMLAttribute(Name = "responseTime")]
        public DateTime responseTime { get; set; }
        
        [XMLAttribute(Name = "applepayResponse")]
        public applepayResponse applepayResponse { get; set; }
        
        [XMLAttribute(Name = "androidpayResponse")]
        public androidpayResponse androidpayResponse { get; set; }
        
        [XMLAttribute(Name = "accountRangeId")]
        public long? accountRangeId { get; set; }
    }
    
    [XMLElement(Name = "updateCardValidationNumOnTokenResponse")]
    public class updateCardValidationNumOnTokenResponse : transactionTypeWithReportGroup
    {
        [XMLAttribute(Name = "cnpTxnId")]
        public long? cnpTxnId { get; set; }
        
        [XMLAttribute(Name = "response")]
        public string response { get; set; }
        
        [XMLAttribute(Name = "message")]
        public string message { get; set; }
        
        [XMLAttribute(Name = "responseTime")]
        public DateTime responseTime { get; set; }
    }
    
    [XMLElement(Name = "authorizationResponse")]
    public class authorizationResponse : transactionTypeWithReportGroup
    {
        [XMLElement(Name = "cnpTxnId")]
        public long? cnpTxnId { get; set; }
        
        [XMLElement(Name = "orderId")]
        public string orderId { get; set; }
        
        [XMLElement(Name = "response")]
        public string response { get; set; }
        
        [XMLElement(Name = "responseTime")]
        public DateTime responseTime { get; set; }
        
        [XMLElement(Name = "cardProductId")]
        public string cardProductId { get; set; }
        
        [XMLElement(Name = "postDate")]
        public DateTime postDate { get; set; }
        
        [XMLElement(Name = "message")]
        public string message { get; set; }
        
        [XMLElement(Name = "cardSuffix")]
        public string cardSuffix { get; set; }
        
        [XMLElement(Name = "authCode")]
        public string authCode { get; set; }
        
        [XMLElement(Name = "authorizationResponseSubCode")]
        public string authorizationResponseSubCode { get; set; }
        
        [XMLElement(Name = "approvedAmount")]
        public string approvedAmount { get; set; }
        
        [XMLElement(Name = "accountInformation")]
        public accountInfoType accountInformation { get; set; }
        
        [XMLElement(Name = "accountUpdater")]
        public accountUpdater accountUpdater { get; set; }
        
        [XMLElement(Name = "fraudResult")]
        public fraudResult fraudResult { get; set; }
        
        [XMLElement(Name = "tokenResponse")]
        public tokenResponseType tokenResponse { get; set; }
        
        [XMLElement(Name = "enhancedAuthResponse")]
        public enhancedAuthResponse enhancedAuthResponse { get; set; }
        
        [XMLElement(Name = "recyclingResponse")]
        public recyclingResponseType recyclingResponse { get; set; }
        
        [XMLElement(Name = "recurringResponse")]
        public recurringResponse recurringResponse { get; set; }
        
        [XMLElement(Name = "giftCardResponse")]
        public giftCardResponse giftCardResponse { get; set; }
        
        [XMLElement(Name = "applepayResponse")]
        public applepayResponse applepayResponse { get; set; }
        
        [XMLElement(Name = "androidpayResponse")]
        public androidpayResponse androidpayResponse { get; set; }
        
        [XMLElement(Name = "networkTransactionId")]
        public string networkTransactionId { get; set; }
        
        [XMLElement(Name = "paymentAccountReferenceNumber")]
        public string paymentAccountReferenceNumber { get; set; }
    }
    
    [XMLElement(Name = "cancelSubscriptionResponse")]
    public class cancelSubscriptionResponse : VersionedXMLElement
    {
        [XMLElement(Name = "subscriptionId")]
        public string subscriptionId { get; set; }
        
        [XMLElement(Name = "cnpTxnId")]
        public string cnpTxnId { get; set; }
        
        [XMLElement(Name = "response")]
        public string response { get; set; }
        
        [XMLElement(Name = "message")]
        public string message { get; set; }
        
        [XMLElement(Name = "responseTime")]
        public DateTime responseTime { get; set; }
    }

    [XMLElement(Name = "updateSubscriptionResponse")]
    public class updateSubscriptionResponse : recurringTransactionResponseType
    {
        [XMLElement(Name = "subscriptionId")]
        public string subscriptionId { get; set; }

        [XMLElement(Name = "tokenResponse")] 
        public tokenResponseType tokenResponse { get; set; }
    }
    
    [XMLElement(Name = "accountUpdater")]
    public class accountUpdater : VersionedXMLElement
    {
        [XMLElement(Name = "extendedCardResponse")] 
        public extendedCardResponseType extendedCardResponse { get; set; }
        
        [XMLElement(Name = "newAccountInfo")] 
        public echeckAccountInfoType newAccountInfo { get; set; }
        
        [XMLElement(Name = "newCardInfo")] 
        public cardAccountInfoType newCardInfo { get; set; }
        
        [XMLElement(Name = "newCardTokenInfo")] 
        public cardTokenInfoType newCardTokenInfo { get; set; }
        
        [XMLElement(Name = "newTokenInfo")] 
        public echeckTokenInfoType newTokenInfo { get; set; }
        
        [XMLElement(Name = "originalAccountInfo")]
        public echeckAccountInfoType originalAccountInfo { get; set; }
        
        [XMLElement(Name = "originalCardInfo")]
        public cardAccountInfoType originalCardInfo { get; set; }
        
        [XMLElement(Name = "originalCardTokenInfo")]
        public cardTokenInfoType originalCardTokenInfo { get; set; }
        
        [XMLElement(Name = "originalTokenInfo")]
        public echeckTokenInfoType originalTokenInfo { get; set; }
        
        [XMLElement(Name = "accountUpdateSource")]
        public accountUpdateSourceType? accountUpdateSource { get; set; }
    }

    [XMLElement(Name = "vendorDebitResponse")]
    public class vendorDebitResponse : transactionTypeWithReportGroup
    {
        [XMLAttribute(Name = "duplicate")]
        public bool? duplicate { get; set; }
        
        [XMLElement(Name = "cnpTxnId")]
        public long cnpTxnId { get; set; }

        [XMLElement(Name = "fundsTransferId")]
        public string fundsTransferId { get; set; }

        [XMLElement(Name = "response")]
        public string response { get; set; }

        [XMLElement(Name = "responseTime")]
        public DateTime responseTime { get; set; }

        [XMLElement(Name = "postDate")]
        public DateTime postDate { get; set; }
        
        [XMLElement(Name = "message")]
        public string message { get; set; }
    }

}



/*

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class enhancedAuthResponse
    {
        //public string cnpTxnId;
        //public string response;
        //public string message;
        //public System.DateTime responseTime;
        //public tokenResponseType tokenResponse;
        //public string virtualAccountNumber;
        //public enhancedAuthResponseFundingSource fundingSource;
        //public affluenceTypeEnum affluence;
        //public string issuerCountry;

        private enhancedAuthResponseFundingSource fundingSourceField;

        private affluenceTypeEnum? affluenceField;

        private bool affluenceFieldSpecified;

        private string issuerCountryField;

        private cardProductTypeEnum? cardProductTypeField;

        private bool cardProductTypeFieldSpecified;

        private bool virtualAccountNumberField;

        private bool virtualAccountNumberFieldSpecified;

        private networkRespnse networkResponseField;

        private long accountRangeIdField;

        private bool accountRangeIdFieldSpecified;

        /// <remarks/>
        public enhancedAuthResponseFundingSource fundingSource
        {
            get
            {
                return this.fundingSourceField;
            }
            set
            {
                this.fundingSourceField = value;
            }
        }

        /// <remarks/>
        public affluenceTypeEnum? affluence
        {
            get
            {
                return this.affluenceFieldSpecified ? this.affluenceField : null;
                //(!null)?return ((affluenceTypeEnum?)this).affluenceField:return null;
            }
            set
            {
                this.affluenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool affluenceSpecified
        {
            get
            {
                return this.affluenceFieldSpecified;
            }
            set
            {
                this.affluenceFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string issuerCountry
        {
            get
            {
                return this.issuerCountryField;
            }
            set
            {
                this.issuerCountryField = value;
            }
        }

        /// <remarks/>
        public cardProductTypeEnum? cardProductType
        {
            get
            {
                return this.cardProductTypeFieldSpecified ? this.cardProductTypeField : null;
            }
            set
            {
                this.cardProductTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool cardProductTypeSpecified
        {
            get
            {
                return this.cardProductTypeFieldSpecified;
            }
            set
            {
                this.cardProductTypeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool virtualAccountNumber
        {
            get
            {
                return this.virtualAccountNumberField;
            }
            set
            {
                this.virtualAccountNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool virtualAccountNumberSpecified
        {
            get
            {
                return this.virtualAccountNumberFieldSpecified;
            }
            set
            {
                this.virtualAccountNumberFieldSpecified = value;
            }
        }

        public networkRespnse networkResponse
        {
            get
            {
                return this.networkResponseField;
            }
            set
            {
                this.networkResponseField = value;
            }
        }

        /// <remarks/>
        public long accountRangeId
        {
            get
            {
                return this.accountRangeIdField;
            }
            set
            {
                this.accountRangeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool accountRangeIdSpecified
        {
            get
            {
                return this.accountRangeIdFieldSpecified;
            }
            set
            {
                this.accountRangeIdFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    public class enhancedAuthResponseFundingSource
    {

        private fundingSourceTypeEnum typeField;

        private string availableBalanceField;

        private string reloadableField;

        private string prepaidCardTypeField;

        /// <remarks/>
        public fundingSourceTypeEnum type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        public string availableBalance
        {
            get
            {
                return this.availableBalanceField;
            }
            set
            {
                this.availableBalanceField = value;
            }
        }

        /// <remarks/>
        public string reloadable
        {
            get
            {
                return this.reloadableField;
            }
            set
            {
                this.reloadableField = value;
            }
        }

        /// <remarks/>
        public string prepaidCardType
        {
            get
            {
                return this.prepaidCardTypeField;
            }
            set
            {
                this.prepaidCardTypeField = value;
            }
        }
    }

    

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class authReversalResponse : transactionTypeWithReportGroup
    {
        private long cnpTxnIdField;

        private string responseField;

        private System.DateTime responseTimeField;

        private System.DateTime postDateField;

        private bool postDateFieldSpecified;

        private string messageField;

        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }

        /// <remarks/>
        public string response
        {
            get
            {
                return this.responseField;
            }
            set
            {
                this.responseField = value;
            }
        }

        /// <remarks/>
        public System.DateTime responseTime
        {
            get
            {
                return this.responseTimeField;
            }
            set
            {
                this.responseTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime postDate
        {
            get
            {
                return this.postDateField;
            }
            set
            {
                this.postDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool postDateSpecified
        {
            get
            {
                return this.postDateFieldSpecified;
            }
            set
            {
                this.postDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]

    public class giftCardAuthReversalResponse : transactionTypeWithReportGroup
    {
        private long cnpTxnIdField;

        private string responseField;

        private System.DateTime responseTimeField;

        private System.DateTime postDateField;

        private bool postDateFieldSpecified;

        private string messageField;

        private giftCardResponse giftCardResponseField;


        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }

        /// <remarks/>
        public string response
        {
            get
            {
                return this.responseField;
            }
            set
            {
                this.responseField = value;
            }
        }

        /// <remarks/>
        public System.DateTime responseTime
        {
            get
            {
                return this.responseTimeField;
            }
            set
            {
                this.responseTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime postDate
        {
            get
            {
                return this.postDateField;
            }
            set
            {
                this.postDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool postDateSpecified
        {
            get
            {
                return this.postDateFieldSpecified;
            }
            set
            {
                this.postDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

        public giftCardResponse giftCardResponse
        {
            get
            {
                return this.giftCardResponseField;
            }
            set
            {
                this.giftCardResponseField = value;
            }
        }


    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]

    public class captureResponse : transactionTypeWithReportGroup
{
        // public giftCardResponse giftCardResponse;

       

        private long cnpTxnIdField;

        private string responseField;

        private System.DateTime responseTimeField;

        private System.DateTime postDateField;

        private bool postDateFieldSpecified;

        private string messageField;

        private accountUpdater accountUpdaterField;

        private fraudResult fraudResultField;

        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }

        /// <remarks/>
        public string response
        {
            get
            {
                return this.responseField;
            }
            set
            {
                this.responseField = value;
            }
        }

        /// <remarks/>
        public System.DateTime responseTime
        {
            get
            {
                return this.responseTimeField;
            }
            set
            {
                this.responseTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime postDate
        {
            get
            {
                return this.postDateField;
            }
            set
            {
                this.postDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool postDateSpecified
        {
            get
            {
                return this.postDateFieldSpecified;
            }
            set
            {
                this.postDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

        /// <remarks/>
        public accountUpdater accountUpdater
        {
            get
            {
                return this.accountUpdaterField;
            }
            set
            {
                this.accountUpdaterField = value;
            }
        }

        public fraudResult fraudResult
        {
            get
            {
                return this.fraudResultField;
            }
            set
            {
                this.fraudResultField = value;
            }
        }

    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]

    public class giftCardCaptureResponse : transactionTypeWithReportGroup
    {

        private long cnpTxnIdField;

        private string responseField;

        private System.DateTime responseTimeField;

        private System.DateTime postDateField;

        private bool postDateFieldSpecified;

        private string messageField;

        private accountUpdater accountUpdaterField;

        public fraudResult fraudResultField;
        // Do I need a getter and setter method
        private giftCardResponse giftCardResponseField;

        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }

        /// <remarks/>
        public string response
        {
            get
            {
                return this.responseField;
            }
            set
            {
                this.responseField = value;
            }
        }

        /// <remarks/>
        public System.DateTime responseTime
        {
            get
            {
                return this.responseTimeField;
            }
            set
            {
                this.responseTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime postDate
        {
            get
            {
                return this.postDateField;
            }
            set
            {
                this.postDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool postDateSpecified
        {
            get
            {
                return this.postDateFieldSpecified;
            }
            set
            {
                this.postDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

        /// <remarks/>
        public accountUpdater accountUpdater
        {
            get
            {
                return this.accountUpdaterField;
            }
            set
            {
                this.accountUpdaterField = value;
            }
        }

        public fraudResult fraudResult
        {
            get
            {
                return this.fraudResultField;
            }
            set
            {
                this.fraudResultField = value;
            }
        }

        public giftCardResponse giftCardResponse
        {
            get
            {
                return this.giftCardResponseField;
            }
            set
            {
                this.giftCardResponseField = value;
            }
        }

    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]

    public class forceCaptureResponse : transactionTypeWithReportGroup
    {
        public giftCardResponse giftCardResponse;

        public fraudResult fraudResult;

        private long cnpTxnIdField;

        private string responseField;

        private System.DateTime responseTimeField;

        private System.DateTime postDateField;

        private bool postDateFieldSpecified;

        private string messageField;

        private tokenResponseType tokenResponseField;

        private accountUpdater accountUpdaterField;

        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }

        /// <remarks/>
        public string response
        {
            get
            {
                return this.responseField;
            }
            set
            {
                this.responseField = value;
            }
        }

        /// <remarks/>
        public System.DateTime responseTime
        {
            get
            {
                return this.responseTimeField;
            }
            set
            {
                this.responseTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime postDate
        {
            get
            {
                return this.postDateField;
            }
            set
            {
                this.postDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool postDateSpecified
        {
            get
            {
                return this.postDateFieldSpecified;
            }
            set
            {
                this.postDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

        /// <remarks/>
        public tokenResponseType tokenResponse
        {
            get
            {
                return this.tokenResponseField;
            }
            set
            {
                this.tokenResponseField = value;
            }
        }

        /// <remarks/>
        public accountUpdater accountUpdater
        {
            get
            {
                return this.accountUpdaterField;
            }
            set
            {
                this.accountUpdaterField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class captureGivenAuthResponse : transactionTypeWithReportGroup
    {
        public giftCardResponse giftCardResponse;
        public fraudResult fraudResult;
        private long cnpTxnIdField;

        private string responseField;

        private System.DateTime responseTimeField;

        private System.DateTime postDateField;

        private bool postDateFieldSpecified;

        private string messageField;

        private tokenResponseType tokenResponseField;

        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }

        /// <remarks/>
        public string response
        {
            get
            {
                return this.responseField;
            }
            set
            {
                this.responseField = value;
            }
        }

        /// <remarks/>
        public System.DateTime responseTime
        {
            get
            {
                return this.responseTimeField;
            }
            set
            {
                this.responseTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime postDate
        {
            get
            {
                return this.postDateField;
            }
            set
            {
                this.postDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool postDateSpecified
        {
            get
            {
                return this.postDateFieldSpecified;
            }
            set
            {
                this.postDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

        /// <remarks/>
        public tokenResponseType tokenResponse
        {
            get
            {
                return this.tokenResponseField;
            }
            set
            {
                this.tokenResponseField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class saleResponse : transactionTypeWithReportGroup
    {

        private long cnpTxnIdField;

        private string orderIdField;

        private string responseField;

        private System.DateTime responseTimeField;

        private string cardProductIdField;

        private System.DateTime postDateField;

        private bool postDateFieldSpecified;

        private string messageField;

        private String cardSuffixField;

        private string authCodeField;

        private string authorizationResponseSubCodeField;

        private string approvedAmountField;

        private accountInfoType accountInformationField;

        private fraudResult fraudResultField;

        private billMeLaterResponseData billMeLaterResponseDataField;

        private tokenResponseType tokenResponseField;

        private enhancedAuthResponse enhancedAuthResponseField;

        private accountUpdater accountUpdaterField;

        private recyclingResponseType recyclingResponseField;

        private recurringResponse recurringResponseField;

        private giftCardResponse giftCardResponseField;

        private applepayResponse applepayResponseField;

        private androidpayResponse androidpayResponseField;

        private sepaDirectDebitResponse sepaDirectDebitResponseField;

        private idealResponse idealResponseField;

        private giropayResponse giropayResponseField;

        private sofortResponse sofortResponseField;

        private bool duplicateField;

        private bool duplicateFieldSpecified;

        private string networkTransactionIdField;
        
        private pinlessDebitResponse pinlessDebitResponseField;

        private string paymentAccountReferenceNumberField;

        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }

        /// <remarks/>
        public string orderId
        {
            get
            {
                return this.orderIdField;
            }
            set
            {
                this.orderIdField = value;
            }
        }

        /// <remarks/>
        public string response
        {
            get
            {
                return this.responseField;
            }
            set
            {
                this.responseField = value;
            }
        }

        /// <remarks/>
        public System.DateTime responseTime
        {
            get
            {
                return this.responseTimeField;
            }
            set
            {
                this.responseTimeField = value;
            }
        }

        /// <remarks/>
        public string cardProductId
        {
            get
            {
                return this.cardProductIdField;
            }
            set
            {
                this.cardProductIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime postDate
        {
            get
            {
                return this.postDateField;
            }
            set
            {
                this.postDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool postDateSpecified
        {
            get
            {
                return this.postDateFieldSpecified;
            }
            set
            {
                this.postDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }


        /// <remarks/>
        public string cardSuffix
        {
            get
            {
                return this.cardSuffixField;
            }
            set
            {
                this.cardSuffixField = value;
            }
        }

        /// <remarks/>
        public string authCode
        {
            get
            {
                return this.authCodeField;
            }
            set
            {
                this.authCodeField = value;
            }
        }

        /// <remarks/>
        public string authorizationResponseSubCode
        {
            get
            {
                return this.authorizationResponseSubCodeField;
            }
            set
            {
                this.authorizationResponseSubCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string approvedAmount
        {
            get
            {
                return this.approvedAmountField;
            }
            set
            {
                this.approvedAmountField = value;
            }
        }

        /// <remarks/>
        public accountInfoType accountInformation
        {
            get
            {
                return this.accountInformationField;
            }
            set
            {
                this.accountInformationField = value;
            }
        }

        /// <remarks/>
        public fraudResult fraudResult
        {
            get
            {
                return this.fraudResultField;
            }
            set
            {
                this.fraudResultField = value;
            }
        }

        /// <remarks/>
        public billMeLaterResponseData billMeLaterResponseData
        {
            get
            {
                return this.billMeLaterResponseDataField;
            }
            set
            {
                this.billMeLaterResponseDataField = value;
            }
        }

        /// <remarks/>
        public tokenResponseType tokenResponse
        {
            get
            {
                return this.tokenResponseField;
            }
            set
            {
                this.tokenResponseField = value;
            }
        }

        /// <remarks/>
        public enhancedAuthResponse enhancedAuthResponse
        {
            get
            {
                return this.enhancedAuthResponseField;
            }
            set
            {
                this.enhancedAuthResponseField = value;
            }
        }

        /// <remarks/>
        public accountUpdater accountUpdater
        {
            get
            {
                return this.accountUpdaterField;
            }
            set
            {
                this.accountUpdaterField = value;
            }
        }

        /// <remarks/>
        public recyclingResponseType recyclingResponse
        {
            get
            {
                return this.recyclingResponseField;
            }
            set
            {
                this.recyclingResponseField = value;
            }
        }

        /// <remarks/>
        public recurringResponse recurringResponse
        {
            get
            {
                return this.recurringResponseField;
            }
            set
            {
                this.recurringResponseField = value;
            }
        }

        /// <remarks/>
        public giftCardResponse giftCardResponse
        {
            get
            {
                return this.giftCardResponseField;
            }
            set
            {
                this.giftCardResponseField = value;
            }
        }

        /// <remarks/>
        public applepayResponse applepayResponse
        {
            get
            {
                return this.applepayResponseField;
            }
            set
            {
                this.applepayResponseField = value;
            }
        }

        /// <remarks/>
        public androidpayResponse androidpayResponse
        {
            get
            {
                return this.androidpayResponseField;
            }
            set
            {
                this.androidpayResponseField = value;
            }
        }

        /// <remarks/>
        public sepaDirectDebitResponse sepaDirectDebitResponse
        {
            get
            {
                return this.sepaDirectDebitResponseField;
            }
            set
            {
                this.sepaDirectDebitResponseField = value;
            }
        }

        /// <remarks/>
        public idealResponse idealResponse
        {
            get
            {
                return this.idealResponseField;
            }
            set
            {
                this.idealResponseField = value;
            }
        }

        /// <remarks/>
        public giropayResponse giropayResponse
        {
            get
            {
                return this.giropayResponseField;
            }
            set
            {
                this.giropayResponseField = value;
            }
        }

        /// <remarks/>
        public sofortResponse sofortResponse
        {
            get
            {
                return this.sofortResponseField;
            }
            set
            {
                this.sofortResponseField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool duplicate
        {
            get
            {
                return this.duplicateField;
            }
            set
            {
                this.duplicateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool duplicateSpecified
        {
            get
            {
                return this.duplicateFieldSpecified;
            }
            set
            {
                this.duplicateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string networkTransactionId
        {
            get
            {
                return this.networkTransactionIdField;
            }
            set
            {
                this.networkTransactionIdField = value;
            }
        }
        
        /// <remarks/>
        public pinlessDebitResponse pinlessDebitResponse {
            get {
                return this.pinlessDebitResponseField;
            }
            set {
                this.pinlessDebitResponseField = value;
            }
        }

        public string paymentAccountReferenceNumber
        {
            get { return paymentAccountReferenceNumberField; }
            set { paymentAccountReferenceNumberField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class creditResponse : transactionTypeWithReportGroup
    {
        private long cnpTxnIdField;

        private string responseField;

        private System.DateTime responseTimeField;

        private System.DateTime postDateField;

        private bool postDateFieldSpecified;

        private string messageField;

        private tokenResponseType tokenResponseField;

        private fraudResult fraudResultField;

        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }


        /// <remarks/>
        public string response
        {
            get
            {
                return this.responseField;
            }
            set
            {
                this.responseField = value;
            }
        }

        /// <remarks/>
        public System.DateTime responseTime
        {
            get
            {
                return this.responseTimeField;
            }
            set
            {
                this.responseTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime postDate
        {
            get
            {
                return this.postDateField;
            }
            set
            {
                this.postDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool postDateSpecified
        {
            get
            {
                return this.postDateFieldSpecified;
            }
            set
            {
                this.postDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

        /// <remarks/>
        public tokenResponseType tokenResponse
        {
            get
            {
                return this.tokenResponseField;
            }
            set
            {
                this.tokenResponseField = value;
            }
        }

        public fraudResult fraudResult
        {
            get
            {
                    return this.fraudResultField;
            }
            set
            {
                    this.fraudResultField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]

    public class giftCardCreditResponse : transactionTypeWithReportGroup
    {
        private long cnpTxnIdField;

        private string responseField;

        private System.DateTime responseTimeField;

        private System.DateTime postDateField;

        private bool postDateFieldSpecified;

        private string messageField;

        private tokenResponseType tokenResponseField;

        private fraudResult fraudResultField;

        private giftCardResponse giftCardResponseField;

        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }


        /// <remarks/>
        public string response
        {
            get
            {
                return this.responseField;
            }
            set
            {
                this.responseField = value;
            }
        }

        /// <remarks/>
        public System.DateTime responseTime
        {
            get
            {
                return this.responseTimeField;
            }
            set
            {
                this.responseTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime postDate
        {
            get
            {
                return this.postDateField;
            }
            set
            {
                this.postDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool postDateSpecified
        {
            get
            {
                return this.postDateFieldSpecified;
            }
            set
            {
                this.postDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

        /// <remarks/>
        public tokenResponseType tokenResponse
        {
            get
            {
                return this.tokenResponseField;
            }
            set
            {
                this.tokenResponseField = value;
            }
        }

        /// <remarks/>
        public fraudResult fraudResult
        {
            get
            {
                return this.fraudResultField;
            }
            set
            {
                this.fraudResultField = value;
            }
        }

        public giftCardResponse giftCardResponse
        {
            get
            {
                return this.giftCardResponseField;
            }
            set
            {
                this.giftCardResponseField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class echeckSalesResponse : transactionTypeWithReportGroup
    {

        private long cnpTxnIdField;

        private string responseField;

        private System.DateTime responseTimeField;

        private string messageField;

        private string verificationCodeField;

        private System.DateTime postDateField;

        private bool postDateFieldSpecified;

        private accountUpdater accountUpdaterField;

        private tokenResponseType tokenResponseField;

        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }

        /// <remarks/>
        public string response
        {
            get
            {
                return this.responseField;
            }
            set
            {
                this.responseField = value;
            }
        }

        /// <remarks/>
        public System.DateTime responseTime
        {
            get
            {
                return this.responseTimeField;
            }
            set
            {
                this.responseTimeField = value;
            }
        }

        /// <remarks/>
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

        /// <remarks/>
        public string verificationCode
        {
            get
            {
                return this.verificationCodeField;
            }
            set
            {
                this.verificationCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime postDate
        {
            get
            {
                return this.postDateField;
            }
            set
            {
                this.postDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool postDateSpecified
        {
            get
            {
                return this.postDateFieldSpecified;
            }
            set
            {
                this.postDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public accountUpdater accountUpdater
        {
            get
            {
                return this.accountUpdaterField;
            }
            set
            {
                this.accountUpdaterField = value;
            }
        }

        /// <remarks/>
        public tokenResponseType tokenResponse
        {
            get
            {
                return this.tokenResponseField;
            }
            set
            {
                this.tokenResponseField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class echeckCreditResponse : transactionTypeWithReportGroup
    {

        private long cnpTxnIdField;

        private string responseField;

        private System.DateTime responseTimeField;

        private string messageField;

        private System.DateTime postDateField;

        private bool postDateFieldSpecified;

        private accountUpdater accountUpdaterField;

        private tokenResponseType tokenResponseField;

        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }

        /// <remarks/>
        public string response
        {
            get
            {
                return this.responseField;
            }
            set
            {
                this.responseField = value;
            }
        }

        /// <remarks/>
        public System.DateTime responseTime
        {
            get
            {
                return this.responseTimeField;
            }
            set
            {
                this.responseTimeField = value;
            }
        }

        /// <remarks/>
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime postDate
        {
            get
            {
                return this.postDateField;
            }
            set
            {
                this.postDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool postDateSpecified
        {
            get
            {
                return this.postDateFieldSpecified;
            }
            set
            {
                this.postDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public accountUpdater accountUpdater
        {
            get
            {
                return this.accountUpdaterField;
            }
            set
            {
                this.accountUpdaterField = value;
            }
        }

        /// <remarks/>
        public tokenResponseType tokenResponse
        {
            get
            {
                return this.tokenResponseField;
            }
            set
            {
                this.tokenResponseField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class echeckVerificationResponse : transactionTypeWithReportGroup
    {

        private long cnpTxnIdField;

        private string responseField;

        private System.DateTime responseTimeField;

        private string messageField;

        private System.DateTime postDateField;

        private bool postDateFieldSpecified;

        private tokenResponseType tokenResponseField;

        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }

        /// <remarks/>
        public string response
        {
            get
            {
                return this.responseField;
            }
            set
            {
                this.responseField = value;
            }
        }

        /// <remarks/>
        public System.DateTime responseTime
        {
            get
            {
                return this.responseTimeField;
            }
            set
            {
                this.responseTimeField = value;
            }
        }

        /// <remarks/>
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime postDate
        {
            get
            {
                return this.postDateField;
            }
            set
            {
                this.postDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool postDateSpecified
        {
            get
            {
                return this.postDateFieldSpecified;
            }
            set
            {
                this.postDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public tokenResponseType tokenResponse
        {
            get
            {
                return this.tokenResponseField;
            }
            set
            {
                this.tokenResponseField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class echeckRedepositResponse : transactionTypeWithReportGroup
    {

        private long cnpTxnIdField;

        private string responseField;

        private System.DateTime responseTimeField;

        private string messageField;

        private System.DateTime postDateField;

        private bool postDateFieldSpecified;

        private accountUpdater accountUpdaterField;

        private tokenResponseType tokenResponseField;

        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }

        /// <remarks/>
        public string response
        {
            get
            {
                return this.responseField;
            }
            set
            {
                this.responseField = value;
            }
        }

        /// <remarks/>
        public System.DateTime responseTime
        {
            get
            {
                return this.responseTimeField;
            }
            set
            {
                this.responseTimeField = value;
            }
        }

        /// <remarks/>
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime postDate
        {
            get
            {
                return this.postDateField;
            }
            set
            {
                this.postDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool postDateSpecified
        {
            get
            {
                return this.postDateFieldSpecified;
            }
            set
            {
                this.postDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public accountUpdater accountUpdater
        {
            get
            {
                return this.accountUpdaterField;
            }
            set
            {
                this.accountUpdaterField = value;
            }
        }

        /// <remarks/>
        public tokenResponseType tokenResponse
        {
            get
            {
                return this.tokenResponseField;
            }
            set
            {
                this.tokenResponseField = value;
            }
        }
    }


    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRoot("cnpResponse", Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class cnpResponse
    {
        public string id;
        public long cnpBatchId;
        public long cnpSessionId;
        public string merchantId;
        public string response;
        public string message;
        public string version;

        private XmlReader originalXmlReader;
        private XmlReader batchResponseReader;
        private XmlReader rfrResponseReader;
        private string filePath;

        public cnpResponse()
        {
        }

        public cnpResponse(string filePath)
        {
            XmlTextReader reader = new XmlTextReader(filePath);
            readXml(reader, filePath);
        }

        public cnpResponse(XmlReader reader, string filePath)
        {
            readXml(reader, filePath);
        }

        public void setBatchResponseReader(XmlReader xmlReader)
        {
            this.batchResponseReader = xmlReader;
        }

        public void setRfrResponseReader(XmlReader xmlReader)
        {
            this.rfrResponseReader = xmlReader;
        }

        public void readXml(XmlReader reader, string filePath)
        {
            if (reader.ReadToFollowing("cnpResponse"))
            {
                version = reader.GetAttribute("version");
                message = reader.GetAttribute("message");
                response = reader.GetAttribute("response");

                string rawCnpSessionId = reader.GetAttribute("cnpSessionId");
                if (rawCnpSessionId != null)
                {
                    cnpSessionId = Int64.Parse(rawCnpSessionId);
                }
            }
            else
            {
                reader.Close();
            }

            this.originalXmlReader = reader;
            this.filePath = filePath;

            this.batchResponseReader = new XmlTextReader(filePath);
            if (!batchResponseReader.ReadToFollowing("batchResponse"))
            {
                batchResponseReader.Close();
            }

            this.rfrResponseReader = new XmlTextReader(filePath);
            if (!rfrResponseReader.ReadToFollowing("RFRResponse"))
            {
                rfrResponseReader.Close();
            }

        }

        virtual public batchResponse nextBatchResponse()
        {
            if (batchResponseReader.ReadState != ReadState.Closed)
            {
                batchResponse cnpBatchResponse = new batchResponse(batchResponseReader, filePath);
                if (!batchResponseReader.ReadToFollowing("batchResponse"))
                {
                    batchResponseReader.Close();
                }

                return cnpBatchResponse;
            }

            return null;
        }

        virtual public RFRResponse nextRFRResponse()
        {
            if (rfrResponseReader.ReadState != ReadState.Closed)
            {
                string response = rfrResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(RFRResponse));
                StringReader reader = new StringReader(response);
                RFRResponse rfrResponse = (RFRResponse)serializer.Deserialize(reader);

                if (!rfrResponseReader.ReadToFollowing("RFRResponse"))
                {
                    rfrResponseReader.Close();
                }

                return rfrResponse;
            }

            return null;
        }
    }

    [System.Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class batchResponse
    {
        public string id;
        public long cnpBatchId;
        public string merchantId;
        public long? numAccountUpdates;

        private XmlReader originalXmlReader;
        private XmlReader accountUpdateResponseReader;
        private XmlReader authorizationResponseReader;
        private XmlReader authReversalResponseReader;
        private XmlReader translateToLowValueTokenResponseReader;
        private XmlReader giftCardAuthReversalResponseReader;
        private XmlReader captureResponseReader;
        private XmlReader giftCardCaptureResponseReader;
        private XmlReader captureGivenAuthResponseReader;
        private XmlReader forceCaptureResponseReader;
        private XmlReader creditResponseReader;
        private XmlReader giftCardCreditResponseReader;
        private XmlReader echeckCreditResponseReader;
        private XmlReader echeckRedepositResponseReader;
        private XmlReader echeckSalesResponseReader;
        private XmlReader echeckVerificationResponseReader;
        private XmlReader saleResponseReader;
        private XmlReader registerTokenResponseReader;
        private XmlReader updateCardValidationNumOnTokenResponseReader;
        private XmlReader cancelSubscriptionResponseReader;
        private XmlReader updateSubscriptionResponseReader;
        private XmlReader createPlanResponseReader;
        private XmlReader updatePlanResponseReader;
        private XmlReader activateResponseReader;
        private XmlReader deactivateResponseReader;
        private XmlReader loadResponseReader;
        private XmlReader echeckPreNoteSaleResponseReader;
        private XmlReader echeckPreNoteCreditResponseReader;
        private XmlReader unloadResponseReader;
        private XmlReader balanceInquiryResponseReader;
        private XmlReader submerchantCreditResponseReader;
        private XmlReader payFacCreditResponseReader;
        private XmlReader vendorCreditResponseReader;
        private XmlReader customerCreditResponseReader;
        private XmlReader reserveCreditResponseReader;
        private XmlReader payoutOrgCreditResponseReader;
        private XmlReader physicalCheckCreditResponseReader;
        private XmlReader submerchantDebitResponseReader;
        private XmlReader payFacDebitResponseReader;
        private XmlReader vendorDebitResponseReader;
        private XmlReader customerDebitResponseReader;
        private XmlReader reserveDebitResponseReader;
        private XmlReader payoutOrgDebitResponseReader;
        private XmlReader physicalCheckDebitResponseReader;
        private XmlReader fundingInstructionVoidResponseReader;
        private XmlReader fastAccessFundingResponseReader;

        public batchResponse()
        {
        }

        public batchResponse(XmlReader xmlReader, string filePath)
        {
            readXml(xmlReader, filePath);
        }

        public void setAccountUpdateResponseReader(XmlReader xmlReader)
        {
            this.accountUpdateResponseReader = xmlReader;
        }

        public void setAuthorizationResponseReader(XmlReader xmlReader)
        {
            this.authorizationResponseReader = xmlReader;
        }

        public void setAuthReversalResponseReader(XmlReader xmlReader)
        {
            this.authReversalResponseReader = xmlReader;
        }

        public void setTranslateToLowValueTokenResponseReader(XmlReader xmlReader)
        {
            this.translateToLowValueTokenResponseReader = xmlReader;
        }

        public void setGiftCardAuthReversalResponseReader(XmlReader xmlReader)
        {
            this.giftCardAuthReversalResponseReader = xmlReader;
        }

        public void setCaptureResponseReader(XmlReader xmlReader)
        {
            this.captureResponseReader = xmlReader;
        }

        public void setForceCaptureResponseReader(XmlReader xmlReader)
        {
            this.forceCaptureResponseReader = xmlReader;
        }

        public void setGiftCardCaptureResponseReader(XmlReader xmlReader)
        {
            this.giftCardCaptureResponseReader = xmlReader;
        }

        public void setCaptureGivenAuthResponseReader(XmlReader xmlReader)
        {
            this.captureGivenAuthResponseReader = xmlReader;
        }

        public void setCreditResponseReader(XmlReader xmlReader)
        {
            this.creditResponseReader = xmlReader;
        }

        public void setGiftCardCreditResponseReader (XmlReader xmlReader)
        {
            this.giftCardCreditResponseReader = xmlReader;
        }

        public void setEcheckCreditResponseReader(XmlReader xmlReader)
        {
            this.echeckCreditResponseReader = xmlReader;
        }

        public void setEcheckRedepositResponseReader(XmlReader xmlReader)
        {
            this.echeckRedepositResponseReader = xmlReader;
        }

        public void setEcheckSalesResponseReader(XmlReader xmlReader)
        {
            this.echeckSalesResponseReader = xmlReader;
        }

        public void setEcheckVerificationResponseReader(XmlReader xmlReader)
        {
            this.echeckVerificationResponseReader = xmlReader;
        }

        public void setSaleResponseReader(XmlReader xmlReader)
        {
            this.saleResponseReader = xmlReader;
        }

        public void setRegisterTokenResponseReader(XmlReader xmlReader)
        {
            this.registerTokenResponseReader = xmlReader;
        }

        public void setUpdateCardValidationNumOnTokenResponseReader(XmlReader xmlReader)
        {
            this.updateCardValidationNumOnTokenResponseReader = xmlReader;
        }

        public void setCancelSubscriptionResponseReader(XmlReader xmlReader)
        {
            this.cancelSubscriptionResponseReader = xmlReader;
        }

        public void setUpdateSubscriptionResponseReader(XmlReader xmlReader)
        {
            this.updateSubscriptionResponseReader = xmlReader;
        }

        public void setCreatePlanResponseReader(XmlReader xmlReader)
        {
            this.createPlanResponseReader = xmlReader;
        }

        public void setUpdatePlanResponseReader(XmlReader xmlReader)
        {
            this.updatePlanResponseReader = xmlReader;
        }

        public void setActivateResponseReader(XmlReader xmlReader)
        {
            this.activateResponseReader = xmlReader;
        }

        public void setDeactivateResponseReader(XmlReader xmlReader)
        {
            this.deactivateResponseReader = xmlReader;
        }

        public void setLoadResponseReader(XmlReader xmlReader)
        {
            this.loadResponseReader = xmlReader;
        }

        public void setEcheckPreNoteSaleResponseReader(XmlReader xmlReader)
        {
            this.echeckPreNoteSaleResponseReader = xmlReader;
        }

        public void setEcheckPreNoteCreditResponseReader(XmlReader xmlReader)
        {
            this.echeckPreNoteCreditResponseReader = xmlReader;
        }

        public void setUnloadResponseReader(XmlReader xmlReader)
        {
            this.unloadResponseReader = xmlReader;
        }

        public void setBalanceInquiryResponseReader(XmlReader xmlReader)
        {
            this.balanceInquiryResponseReader = xmlReader;
        }

        public void setSubmerchantCreditResponseReader(XmlReader xmlReader)
        {
            this.submerchantCreditResponseReader = xmlReader;
        }

        public void setPayFacCreditResponseReader(XmlReader xmlReader)
        {
            this.payFacCreditResponseReader = xmlReader;
        }

        public void setReserveCreditResponseReader(XmlReader xmlReader)
        {
            this.reserveCreditResponseReader = xmlReader;
        }

        public void setVendorCreditResponseReader(XmlReader xmlReader)
        {
            this.vendorCreditResponseReader = xmlReader;
        }

        public void setCustomerCreditResponseReader(XmlReader xmlReader)
        {
            this.customerCreditResponseReader = xmlReader;
        }

        public void setPayoutOrgCreditResponseReader(XmlReader xmlReader)
        {
            this.payoutOrgCreditResponseReader = xmlReader;
        }

        public void setPhysicalCheckCreditResponseReader(XmlReader xmlReader)
        {
            this.physicalCheckCreditResponseReader = xmlReader;
        }

        public void setSubmerchantDebitResponseReader(XmlReader xmlReader)
        {
            this.submerchantDebitResponseReader = xmlReader;
        }

        public void setPayFacDebitResponseReader(XmlReader xmlReader)
        {
            this.payFacDebitResponseReader = xmlReader;
        }

        public void setReserveDebitResponseReader(XmlReader xmlReader)
        {
            this.reserveDebitResponseReader = xmlReader;
        }

        public void setVendorDebitResponseReader(XmlReader xmlReader)
        {
            this.vendorDebitResponseReader = xmlReader;
        }

        public void setCustomerDebitResponseReader(XmlReader xmlReader)
        {
            this.customerDebitResponseReader = xmlReader;
        }

        public void setPayoutOrgDebitResponseReader(XmlReader xmlReader)
        {
            this.payoutOrgDebitResponseReader = xmlReader;
        }        

        public void setPhysicalCheckDebitResponseReader(XmlReader xmlReader)
        {
            this.physicalCheckDebitResponseReader = xmlReader;
        }

        public void setFundingInstructionVoidResponseReader(XmlReader xmlReader)
        {
            this.fundingInstructionVoidResponseReader = xmlReader;
        }
        
        public void setFastAccessFundingResponseReader(XmlReader xmlReader)
        {
            this.fastAccessFundingResponseReader = xmlReader;
        }


        public void readXml(XmlReader reader, string filePath)
        {
            id = reader.GetAttribute("id");
            cnpBatchId = Int64.Parse(reader.GetAttribute("cnpBatchId"));
            merchantId = reader.GetAttribute("merchantId");
            if (reader.GetAttribute("numAccountUpdates") != null) {
                numAccountUpdates = Int64.Parse(reader.GetAttribute("numAccountUpdates"));
            }

            originalXmlReader = reader;
            accountUpdateResponseReader = new XmlTextReader(filePath);
            authorizationResponseReader = new XmlTextReader(filePath);
            translateToLowValueTokenResponseReader = new XmlTextReader(filePath);
            authReversalResponseReader = new XmlTextReader(filePath);
            giftCardAuthReversalResponseReader = new XmlTextReader(filePath);
            captureResponseReader = new XmlTextReader(filePath);
            giftCardCaptureResponseReader = new XmlTextReader(filePath);
            captureGivenAuthResponseReader = new XmlTextReader(filePath);
            creditResponseReader = new XmlTextReader(filePath);
            giftCardCreditResponseReader = new XmlTextReader(filePath);
            forceCaptureResponseReader = new XmlTextReader(filePath);
            echeckCreditResponseReader = new XmlTextReader(filePath);
            echeckRedepositResponseReader = new XmlTextReader(filePath);
            echeckSalesResponseReader = new XmlTextReader(filePath);
            echeckVerificationResponseReader = new XmlTextReader(filePath);
            saleResponseReader = new XmlTextReader(filePath);
            registerTokenResponseReader = new XmlTextReader(filePath);
            updateCardValidationNumOnTokenResponseReader = new XmlTextReader(filePath);
            cancelSubscriptionResponseReader = new XmlTextReader(filePath);
            updateSubscriptionResponseReader = new XmlTextReader(filePath);
            createPlanResponseReader = new XmlTextReader(filePath);
            updatePlanResponseReader = new XmlTextReader(filePath);
            activateResponseReader = new XmlTextReader(filePath);
            deactivateResponseReader = new XmlTextReader(filePath);
            loadResponseReader = new XmlTextReader(filePath);
            echeckPreNoteSaleResponseReader = new XmlTextReader(filePath);
            echeckPreNoteCreditResponseReader = new XmlTextReader(filePath);
            unloadResponseReader = new XmlTextReader(filePath);
            balanceInquiryResponseReader = new XmlTextReader(filePath);
            submerchantCreditResponseReader = new XmlTextReader(filePath);
            payFacCreditResponseReader = new XmlTextReader(filePath);
            reserveCreditResponseReader = new XmlTextReader(filePath);
            vendorCreditResponseReader = new XmlTextReader(filePath);
            customerCreditResponseReader = new XmlTextReader(filePath);
            payoutOrgDebitResponseReader = new XmlTextReader(filePath);
            physicalCheckCreditResponseReader = new XmlTextReader(filePath);
            submerchantDebitResponseReader = new XmlTextReader(filePath);
            payFacDebitResponseReader = new XmlTextReader(filePath);
            reserveDebitResponseReader = new XmlTextReader(filePath);
            vendorDebitResponseReader = new XmlTextReader(filePath);
            customerDebitResponseReader = new XmlTextReader(filePath);
            payoutOrgCreditResponseReader = new XmlTextReader(filePath);
            physicalCheckDebitResponseReader = new XmlTextReader(filePath);
            fundingInstructionVoidResponseReader = new XmlTextReader(filePath);
            fastAccessFundingResponseReader = new XmlTextReader(filePath);

            if (!accountUpdateResponseReader.ReadToFollowing("accountUpdateResponse"))
            {
                accountUpdateResponseReader.Close();
            }
            if (!authorizationResponseReader.ReadToFollowing("authorizationResponse"))
            {
                authorizationResponseReader.Close();
            }
            if (!translateToLowValueTokenResponseReader.ReadToFollowing("translateToLowValueTokenResponse"))
            {
                translateToLowValueTokenResponseReader.Close();
            }
            if (!authReversalResponseReader.ReadToFollowing("authReversalResponse"))
            {
                authReversalResponseReader.Close();
            }
            if (!giftCardAuthReversalResponseReader.ReadToFollowing("giftCardAuthReversalResponse"))
            {
                giftCardAuthReversalResponseReader.Close();
            }
            if (!captureResponseReader.ReadToFollowing("captureResponse"))
            {
                captureResponseReader.Close();
            }
            if (!giftCardCaptureResponseReader.ReadToFollowing("giftCardCaptureResponse"))
            {
                giftCardCaptureResponseReader.Close();
            }
            if (!captureGivenAuthResponseReader.ReadToFollowing("captureGivenAuthResponse"))
            {
                captureGivenAuthResponseReader.Close();
            }
            if (!creditResponseReader.ReadToFollowing("creditResponse"))
            {
                creditResponseReader.Close();
            }
            if (!giftCardCreditResponseReader.ReadToFollowing("giftCardCreditResponse"))
            {
                giftCardCreditResponseReader.Close();
            }
            if (!forceCaptureResponseReader.ReadToFollowing("forceCaptureResponse"))
            {
                forceCaptureResponseReader.Close();
            }
            if (!echeckCreditResponseReader.ReadToFollowing("echeckCreditResponse"))
            {
                echeckCreditResponseReader.Close();
            }
            if (!echeckRedepositResponseReader.ReadToFollowing("echeckRedepositResponse"))
            {
                echeckRedepositResponseReader.Close();
            }
            if (!echeckSalesResponseReader.ReadToFollowing("echeckSalesResponse"))
            {
                echeckSalesResponseReader.Close();
            }
            if (!echeckVerificationResponseReader.ReadToFollowing("echeckVerificationResponse"))
            {
                echeckVerificationResponseReader.Close();
            }
            if (!saleResponseReader.ReadToFollowing("saleResponse"))
            {
                saleResponseReader.Close();
            }
            if (!registerTokenResponseReader.ReadToFollowing("registerTokenResponse"))
            {
                registerTokenResponseReader.Close();
            }
            if (!updateCardValidationNumOnTokenResponseReader.ReadToFollowing("updateCardValidationNumOnTokenResponse"))
            {
                updateCardValidationNumOnTokenResponseReader.Close();
            }
            if (!cancelSubscriptionResponseReader.ReadToFollowing("cancelSubscriptionResponse"))
            {
                cancelSubscriptionResponseReader.Close();
            }
            if (!updateSubscriptionResponseReader.ReadToFollowing("updateSubscriptionResponse"))
            {
                updateSubscriptionResponseReader.Close();
            }
            if (!createPlanResponseReader.ReadToFollowing("createPlanResponse"))
            {
                createPlanResponseReader.Close();
            }
            if (!updatePlanResponseReader.ReadToFollowing("updatePlanResponse"))
            {
                updatePlanResponseReader.Close();
            }
            if (!activateResponseReader.ReadToFollowing("activateResponse"))
            {
                activateResponseReader.Close();
            }
            if (!loadResponseReader.ReadToFollowing("loadResponse"))
            {
                loadResponseReader.Close();
            }
            if (!deactivateResponseReader.ReadToFollowing("deactivateResponse"))
            {
                deactivateResponseReader.Close();
            }
            if (!echeckPreNoteSaleResponseReader.ReadToFollowing("echeckPreNoteSaleResponse"))
            {
                echeckPreNoteSaleResponseReader.Close();
            }
            if (!echeckPreNoteCreditResponseReader.ReadToFollowing("echeckPreNoteCreditResponse"))
            {
                echeckPreNoteCreditResponseReader.Close();
            } if (!unloadResponseReader.ReadToFollowing("unloadResponse"))
            {
                unloadResponseReader.Close();
            }
            if (!balanceInquiryResponseReader.ReadToFollowing("balanceInquiryResponse"))
            {
                balanceInquiryResponseReader.Close();
            }
            if (!submerchantCreditResponseReader.ReadToFollowing("submerchantCreditResponse"))
            {
                submerchantCreditResponseReader.Close();
            }
            if (!payFacCreditResponseReader.ReadToFollowing("payFacCreditResponse"))
            {
                payFacCreditResponseReader.Close();
            }
            if (!vendorCreditResponseReader.ReadToFollowing("vendorCreditResponse"))
            {
                vendorCreditResponseReader.Close();
            }
            if (!customerCreditResponseReader.ReadToFollowing("customerCreditResponse"))
            {
                customerCreditResponseReader.Close();
            }
            if (!reserveCreditResponseReader.ReadToFollowing("reserveCreditResponse"))
            {
                reserveCreditResponseReader.Close();
            }
            if (!physicalCheckCreditResponseReader.ReadToFollowing("physicalCheckCreditResponse"))
            {
                physicalCheckCreditResponseReader.Close();
            }
            if (!submerchantDebitResponseReader.ReadToFollowing("submerchantDebitResponse"))
            {
                submerchantDebitResponseReader.Close();
            }
            if (!payFacDebitResponseReader.ReadToFollowing("payFacDebitResponse"))
            {
                payFacDebitResponseReader.Close();
            }
            if (!vendorDebitResponseReader.ReadToFollowing("vendorDebitResponse"))
            {
                vendorDebitResponseReader.Close();
            }
            if (!customerDebitResponseReader.ReadToFollowing("customerDebitResponse"))
            {
                customerDebitResponseReader.Close();
            }
            if (!reserveDebitResponseReader.ReadToFollowing("reserveDebitResponse"))
            {
                reserveDebitResponseReader.Close();
            }
            if (!physicalCheckDebitResponseReader.ReadToFollowing("physicalCheckDebitResponse"))
            {
                physicalCheckDebitResponseReader.Close();
            }
            if (!fundingInstructionVoidResponseReader.ReadToFollowing("fundingInstructionVoidResponse"))
            {
                fundingInstructionVoidResponseReader.Close();
            }
            if (!fastAccessFundingResponseReader.ReadToFollowing("fastAccessFundingResponse"))
            {
                fastAccessFundingResponseReader.Close();
            }
        }

        virtual public accountUpdateResponse nextAccountUpdateResponse()
        {
            if (accountUpdateResponseReader.ReadState != ReadState.Closed)
            {
                string response = accountUpdateResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(accountUpdateResponse));
                StringReader reader = new StringReader(response);
                accountUpdateResponse i = (accountUpdateResponse)serializer.Deserialize(reader);

                if (!accountUpdateResponseReader.ReadToFollowing("accountUpdateResponse"))
                {
                    accountUpdateResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public authorizationResponse nextAuthorizationResponse()
        {
            if (authorizationResponseReader.ReadState != ReadState.Closed)
            {
                string response = authorizationResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(authorizationResponse));
                StringReader reader = new StringReader(response);
                authorizationResponse i = (authorizationResponse)serializer.Deserialize(reader);

                if (!authorizationResponseReader.ReadToFollowing("authorizationResponse"))
                {
                    authorizationResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public authReversalResponse nextAuthReversalResponse()
        {
            if (authReversalResponseReader.ReadState != ReadState.Closed)
            {
                string response = authReversalResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(authReversalResponse));
                StringReader reader = new StringReader(response);
                authReversalResponse i = (authReversalResponse)serializer.Deserialize(reader);

                if (!authReversalResponseReader.ReadToFollowing("authReversalResponse"))
                {
                    authReversalResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public translateToLowValueTokenResponse nextTranslateToLowValueTokenResponse()
        {
            if (translateToLowValueTokenResponseReader.ReadState != ReadState.Closed)
            {
                string response = translateToLowValueTokenResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(translateToLowValueTokenResponse));
                StringReader reader = new StringReader(response);
                translateToLowValueTokenResponse i = (translateToLowValueTokenResponse)serializer.Deserialize(reader);

                if (!translateToLowValueTokenResponseReader.ReadToFollowing("translateToLowValueTokenResponse"))
                {
                    translateToLowValueTokenResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public giftCardAuthReversalResponse nextGiftCardAuthReversalResponse()
        {
            if (giftCardAuthReversalResponseReader.ReadState != ReadState.Closed)
            {
                string response = giftCardAuthReversalResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(giftCardAuthReversalResponse));
                StringReader reader = new StringReader(response);
                giftCardAuthReversalResponse i = (giftCardAuthReversalResponse)serializer.Deserialize(reader);

                if (!giftCardAuthReversalResponseReader.ReadToFollowing("giftCardAuthReversalResponse"))
                {
                    giftCardAuthReversalResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public captureResponse nextCaptureResponse()
        {
            if (captureResponseReader.ReadState != ReadState.Closed)
            {
                string response = captureResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(captureResponse));
                StringReader reader = new StringReader(response);
                captureResponse i = (captureResponse)serializer.Deserialize(reader);

                if (!captureResponseReader.ReadToFollowing("captureResponse"))
                {
                    captureResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public giftCardCaptureResponse nextGiftCardCaptureResponse()
        {
            if (giftCardCaptureResponseReader.ReadState != ReadState.Closed)
            {
                string response = giftCardCaptureResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(giftCardCaptureResponse));
                StringReader reader = new StringReader(response);
                giftCardCaptureResponse i = (giftCardCaptureResponse)serializer.Deserialize(reader);

                if (!giftCardCaptureResponseReader.ReadToFollowing("giftCardCaptureResponse"))
                {
                    giftCardCaptureResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public captureGivenAuthResponse nextCaptureGivenAuthResponse()
        {
            if (captureGivenAuthResponseReader.ReadState != ReadState.Closed)
            {
                string response = captureGivenAuthResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(captureGivenAuthResponse));
                StringReader reader = new StringReader(response);
                captureGivenAuthResponse i = (captureGivenAuthResponse)serializer.Deserialize(reader);

                if (!captureGivenAuthResponseReader.ReadToFollowing("captureGivenAuthResponse"))
                {
                    captureGivenAuthResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public creditResponse nextCreditResponse()
        {
            if (creditResponseReader.ReadState != ReadState.Closed)
            {
                string response = creditResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(creditResponse));
                StringReader reader = new StringReader(response);
                creditResponse i = (creditResponse)serializer.Deserialize(reader);

                if (!creditResponseReader.ReadToFollowing("creditResponse"))
                {
                    creditResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public giftCardCreditResponse nextGiftCardCreditResponse()
        {
            if (giftCardCreditResponseReader.ReadState != ReadState.Closed)
            {
                string response = giftCardCreditResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(giftCardCreditResponse));
                StringReader reader = new StringReader(response);
                giftCardCreditResponse i = (giftCardCreditResponse)serializer.Deserialize(reader);

                if (!giftCardCreditResponseReader.ReadToFollowing("giftCardCreditResponse"))
                {
                    giftCardCreditResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public echeckCreditResponse nextEcheckCreditResponse()
        {
            if (echeckCreditResponseReader.ReadState != ReadState.Closed)
            {
                string response = echeckCreditResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(echeckCreditResponse));
                StringReader reader = new StringReader(response);
                echeckCreditResponse i = (echeckCreditResponse)serializer.Deserialize(reader);

                if (!echeckCreditResponseReader.ReadToFollowing("echeckCreditResponse"))
                {
                    echeckCreditResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public echeckRedepositResponse nextEcheckRedepositResponse()
        {
            if (echeckRedepositResponseReader.ReadState != ReadState.Closed)
            {
                string response = echeckRedepositResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(echeckRedepositResponse));
                StringReader reader = new StringReader(response);
                echeckRedepositResponse i = (echeckRedepositResponse)serializer.Deserialize(reader);

                if (!echeckRedepositResponseReader.ReadToFollowing("echeckRedepositResponse"))
                {
                    echeckRedepositResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public echeckSalesResponse nextEcheckSalesResponse()
        {
            if (echeckSalesResponseReader.ReadState != ReadState.Closed)
            {
                string response = echeckSalesResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(echeckSalesResponse));
                StringReader reader = new StringReader(response);
                echeckSalesResponse i = (echeckSalesResponse)serializer.Deserialize(reader);

                if (!echeckSalesResponseReader.ReadToFollowing("echeckSalesResponse"))
                {
                    echeckSalesResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public echeckVerificationResponse nextEcheckVerificationResponse()
        {
            if (echeckVerificationResponseReader.ReadState != ReadState.Closed)
            {
                string response = echeckVerificationResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(echeckVerificationResponse));
                StringReader reader = new StringReader(response);
                echeckVerificationResponse i = (echeckVerificationResponse)serializer.Deserialize(reader);

                if (!echeckVerificationResponseReader.ReadToFollowing("echeckVerificationResponse"))
                {
                    echeckVerificationResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public forceCaptureResponse nextForceCaptureResponse()
        {
            if (forceCaptureResponseReader.ReadState != ReadState.Closed)
            {
                string response = forceCaptureResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(forceCaptureResponse));
                StringReader reader = new StringReader(response);
                forceCaptureResponse i = (forceCaptureResponse)serializer.Deserialize(reader);

                if (!forceCaptureResponseReader.ReadToFollowing("forceCaptureResponse"))
                {
                    forceCaptureResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public registerTokenResponse nextRegisterTokenResponse()
        {
            if (registerTokenResponseReader.ReadState != ReadState.Closed)
            {
                string response = registerTokenResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(registerTokenResponse));
                StringReader reader = new StringReader(response);
                registerTokenResponse i = (registerTokenResponse)serializer.Deserialize(reader);

                if (!registerTokenResponseReader.ReadToFollowing("registerTokenResponse"))
                {
                    registerTokenResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public saleResponse nextSaleResponse()
        {
            if (saleResponseReader.ReadState != ReadState.Closed)
            {
                string response = saleResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(saleResponse));
                StringReader reader = new StringReader(response);
                saleResponse i = (saleResponse)serializer.Deserialize(reader);

                if (!saleResponseReader.ReadToFollowing("saleResponse"))
                {
                    saleResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public updateCardValidationNumOnTokenResponse nextUpdateCardValidationNumOnTokenResponse()
        {
            if (updateCardValidationNumOnTokenResponseReader.ReadState != ReadState.Closed)
            {
                string response = updateCardValidationNumOnTokenResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(updateCardValidationNumOnTokenResponse));
                StringReader reader = new StringReader(response);
                updateCardValidationNumOnTokenResponse i = (updateCardValidationNumOnTokenResponse)serializer.Deserialize(reader);

                if (!updateCardValidationNumOnTokenResponseReader.ReadToFollowing("updateCardValidationNumOnTokenResponse"))
                {
                    updateCardValidationNumOnTokenResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public updateSubscriptionResponse nextUpdateSubscriptionResponse()
        {
            if (updateSubscriptionResponseReader.ReadState != ReadState.Closed)
            {
                string response = updateSubscriptionResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(updateSubscriptionResponse));
                StringReader reader = new StringReader(response);
                updateSubscriptionResponse i = (updateSubscriptionResponse)serializer.Deserialize(reader);

                if (!updateSubscriptionResponseReader.ReadToFollowing("updateSubscriptionResponse"))
                {
                    updateSubscriptionResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public cancelSubscriptionResponse nextCancelSubscriptionResponse()
        {
            if (cancelSubscriptionResponseReader.ReadState != ReadState.Closed)
            {
                string response = cancelSubscriptionResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(cancelSubscriptionResponse));
                StringReader reader = new StringReader(response);
                cancelSubscriptionResponse i = (cancelSubscriptionResponse)serializer.Deserialize(reader);

                if (!cancelSubscriptionResponseReader.ReadToFollowing("cancelSubscriptionResponse"))
                {
                    cancelSubscriptionResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public createPlanResponse nextCreatePlanResponse()
        {
            if (createPlanResponseReader.ReadState != ReadState.Closed)
            {
                string response = createPlanResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(createPlanResponse));
                StringReader reader = new StringReader(response);
                createPlanResponse i = (createPlanResponse)serializer.Deserialize(reader);

                if (!createPlanResponseReader.ReadToFollowing("createPlanResponse"))
                {
                    createPlanResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public updatePlanResponse nextUpdatePlanResponse()
        {
            if (updatePlanResponseReader.ReadState != ReadState.Closed)
            {
                string response = updatePlanResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(updatePlanResponse));
                StringReader reader = new StringReader(response);
                updatePlanResponse i = (updatePlanResponse)serializer.Deserialize(reader);

                if (!updatePlanResponseReader.ReadToFollowing("updatePlanResponse"))
                {
                    updatePlanResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public activateResponse nextActivateResponse()
        {
            if (activateResponseReader.ReadState != ReadState.Closed)
            {
                string response = activateResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(activateResponse));
                StringReader reader = new StringReader(response);
                activateResponse i = (activateResponse)serializer.Deserialize(reader);

                if (!activateResponseReader.ReadToFollowing("activateResponse"))
                {
                    activateResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public deactivateResponse nextDeactivateResponse()
        {
            if (deactivateResponseReader.ReadState != ReadState.Closed)
            {
                string response = deactivateResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(deactivateResponse));
                StringReader reader = new StringReader(response);
                deactivateResponse i = (deactivateResponse)serializer.Deserialize(reader);

                if (!deactivateResponseReader.ReadToFollowing("deactivateResponse"))
                {
                    deactivateResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public echeckPreNoteSaleResponse nextEcheckPreNoteSaleResponse()
        {
            if (echeckPreNoteSaleResponseReader.ReadState != ReadState.Closed)
            {
                string response = echeckPreNoteSaleResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(echeckPreNoteSaleResponse));
                StringReader reader = new StringReader(response);
                echeckPreNoteSaleResponse i = (echeckPreNoteSaleResponse)serializer.Deserialize(reader);

                if (!echeckPreNoteSaleResponseReader.ReadToFollowing("echeckPreNoteSaleResponse"))
                {
                    echeckPreNoteSaleResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public echeckPreNoteCreditResponse nextEcheckPreNoteCreditResponse()
        {
            if (echeckPreNoteCreditResponseReader.ReadState != ReadState.Closed)
            {
                string response = echeckPreNoteCreditResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(echeckPreNoteCreditResponse));
                StringReader reader = new StringReader(response);
                echeckPreNoteCreditResponse i = (echeckPreNoteCreditResponse)serializer.Deserialize(reader);

                if (!echeckPreNoteCreditResponseReader.ReadToFollowing("echeckPreNoteCreditResponse"))
                {
                    echeckPreNoteCreditResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public loadResponse nextLoadResponse()
        {
            if (loadResponseReader.ReadState != ReadState.Closed)
            {
                string response = loadResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(loadResponse));
                StringReader reader = new StringReader(response);
                loadResponse i = (loadResponse)serializer.Deserialize(reader);

                if (!loadResponseReader.ReadToFollowing("loadResponse"))
                {
                    loadResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public unloadResponse nextUnloadResponse()
        {
            if (unloadResponseReader.ReadState != ReadState.Closed)
            {
                string response = unloadResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(unloadResponse));
                StringReader reader = new StringReader(response);
                unloadResponse i = (unloadResponse)serializer.Deserialize(reader);

                if (!unloadResponseReader.ReadToFollowing("unloadResponse"))
                {
                    unloadResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public balanceInquiryResponse nextBalanceInquiryResponse()
        {
            if (balanceInquiryResponseReader.ReadState != ReadState.Closed)
            {
                string response = balanceInquiryResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(balanceInquiryResponse));
                StringReader reader = new StringReader(response);
                balanceInquiryResponse i = (balanceInquiryResponse)serializer.Deserialize(reader);

                if (!balanceInquiryResponseReader.ReadToFollowing("balanceInquiryResponse"))
                {
                    balanceInquiryResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public submerchantCreditResponse nextSubmerchantCreditResponse()
        {
            if (submerchantCreditResponseReader.ReadState != ReadState.Closed)
            {
                string response = submerchantCreditResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(submerchantCreditResponse));
                StringReader reader = new StringReader(response);
                submerchantCreditResponse i = (submerchantCreditResponse)serializer.Deserialize(reader);

                if (!submerchantCreditResponseReader.ReadToFollowing("submerchantCreditResponse"))
                {
                    submerchantCreditResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public payFacCreditResponse nextPayFacCreditResponse()
        {
            if (payFacCreditResponseReader.ReadState != ReadState.Closed)
            {
                string response = payFacCreditResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(payFacCreditResponse));
                StringReader reader = new StringReader(response);
                payFacCreditResponse i = (payFacCreditResponse)serializer.Deserialize(reader);

                if (!payFacCreditResponseReader.ReadToFollowing("payFacCreditResponse"))
                {
                    payFacCreditResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public vendorCreditResponse nextVendorCreditResponse()
        {
            if (vendorCreditResponseReader.ReadState != ReadState.Closed)
            {
                string response = vendorCreditResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(vendorCreditResponse));
                StringReader reader = new StringReader(response);
                vendorCreditResponse i = (vendorCreditResponse)serializer.Deserialize(reader);

                if (!vendorCreditResponseReader.ReadToFollowing("vendorCreditResponse"))
                {
                    vendorCreditResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public customerCreditResponse nextCustomerCreditResponse()
        {
            if (customerCreditResponseReader.ReadState != ReadState.Closed)
            {
                string response = customerCreditResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(customerCreditResponse));
                StringReader reader = new StringReader(response);
                customerCreditResponse i = (customerCreditResponse)serializer.Deserialize(reader);

                if (!customerCreditResponseReader.ReadToFollowing("customerCreditResponse"))
                {
                    customerCreditResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public reserveCreditResponse nextReserveCreditResponse()
        {
            if (reserveCreditResponseReader.ReadState != ReadState.Closed)
            {
                string response = reserveCreditResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(reserveCreditResponse));
                StringReader reader = new StringReader(response);
                reserveCreditResponse i = (reserveCreditResponse)serializer.Deserialize(reader);

                if (!reserveCreditResponseReader.ReadToFollowing("reserveCreditResponse"))
                {
                    reserveCreditResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public payoutOrgCreditResponse nextPayoutOrgCreditResponse()
        {
            if (payoutOrgCreditResponseReader.ReadState != ReadState.Closed)
            {
                string response = payoutOrgCreditResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(payoutOrgCreditResponse));
                StringReader reader = new StringReader(response);
                payoutOrgCreditResponse i = (payoutOrgCreditResponse)serializer.Deserialize(reader);

                if (!payoutOrgCreditResponseReader.ReadToFollowing("payoutOrgCreditResponse"))
                {
                    payoutOrgCreditResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public physicalCheckCreditResponse nextPhysicalCheckCreditResponse()
        {
            if (physicalCheckCreditResponseReader.ReadState != ReadState.Closed)
            {
                string response = physicalCheckCreditResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(physicalCheckCreditResponse));
                StringReader reader = new StringReader(response);
                physicalCheckCreditResponse i = (physicalCheckCreditResponse)serializer.Deserialize(reader);

                if (!physicalCheckCreditResponseReader.ReadToFollowing("physicalCheckCreditResponse"))
                {
                    physicalCheckCreditResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public submerchantDebitResponse nextSubmerchantDebitResponse()
        {
            if (submerchantDebitResponseReader.ReadState != ReadState.Closed)
            {
                string response = submerchantDebitResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(submerchantDebitResponse));
                StringReader reader = new StringReader(response);
                submerchantDebitResponse i = (submerchantDebitResponse)serializer.Deserialize(reader);

                if (!submerchantDebitResponseReader.ReadToFollowing("submerchantDebitResponse"))
                {
                    submerchantDebitResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public payFacDebitResponse nextPayFacDebitResponse()
        {
            if (payFacDebitResponseReader.ReadState != ReadState.Closed)
            {
                string response = payFacDebitResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(payFacDebitResponse));
                StringReader reader = new StringReader(response);
                payFacDebitResponse i = (payFacDebitResponse)serializer.Deserialize(reader);

                if (!payFacDebitResponseReader.ReadToFollowing("payFacDebitResponse"))
                {
                    payFacDebitResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public vendorDebitResponse nextVendorDebitResponse()
        {
            if (vendorDebitResponseReader.ReadState != ReadState.Closed)
            {
                string response = vendorDebitResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(vendorDebitResponse));
                StringReader reader = new StringReader(response);
                vendorDebitResponse i = (vendorDebitResponse)serializer.Deserialize(reader);

                if (!vendorDebitResponseReader.ReadToFollowing("vendorDebitResponse"))
                {
                    vendorDebitResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public customerDebitResponse nextCustomerDebitResponse()
        {
            if (customerDebitResponseReader.ReadState != ReadState.Closed)
            {
                string response = customerDebitResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(customerDebitResponse));
                StringReader reader = new StringReader(response);
                customerDebitResponse i = (customerDebitResponse)serializer.Deserialize(reader);

                if (!customerDebitResponseReader.ReadToFollowing("customerDebitResponse"))
                {
                    customerDebitResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public reserveDebitResponse nextReserveDebitResponse()
        {
            if (reserveDebitResponseReader.ReadState != ReadState.Closed)
            {
                string response = reserveDebitResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(reserveDebitResponse));
                StringReader reader = new StringReader(response);
                reserveDebitResponse i = (reserveDebitResponse)serializer.Deserialize(reader);

                if (!reserveDebitResponseReader.ReadToFollowing("reserveDebitResponse"))
                {
                    reserveDebitResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public payoutOrgDebitResponse nextPayoutOrgDebitResponse()
        {
            if (payoutOrgDebitResponseReader.ReadState != ReadState.Closed)
            {
                string response = payoutOrgDebitResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(payoutOrgDebitResponse));
                StringReader reader = new StringReader(response);
                payoutOrgDebitResponse i = (payoutOrgDebitResponse)serializer.Deserialize(reader);

                if (!payoutOrgDebitResponseReader.ReadToFollowing("payoutOrgDebitResponse"))
                {
                    payoutOrgDebitResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public physicalCheckDebitResponse nextPhysicalCheckDebitResponse()
        {
            if (physicalCheckDebitResponseReader.ReadState != ReadState.Closed)
            {
                string response = physicalCheckDebitResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(physicalCheckDebitResponse));
                StringReader reader = new StringReader(response);
                physicalCheckDebitResponse i = (physicalCheckDebitResponse)serializer.Deserialize(reader);

                if (!physicalCheckDebitResponseReader.ReadToFollowing("physicalCheckDebitResponse"))
                {
                    physicalCheckDebitResponseReader.Close();
                }

                return i;
            }

            return null;
        }

        virtual public fundingInstructionVoidResponse nextFundingInstructionVoidResponse()
        {
            if (fundingInstructionVoidResponseReader.ReadState != ReadState.Closed)
            {
                string response = fundingInstructionVoidResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(fundingInstructionVoidResponse));
                StringReader reader = new StringReader(response);
                fundingInstructionVoidResponse i = (fundingInstructionVoidResponse)serializer.Deserialize(reader);

                if (!fundingInstructionVoidResponseReader.ReadToFollowing("fundingInstructionVoidResponse"))
                {
                    fundingInstructionVoidResponseReader.Close();
                }

                return i;
            }

            return null;
        }
        
        virtual public fastAccessFundingResponse nextFastAccessFundingResponse()
        {
            if (fastAccessFundingResponseReader.ReadState != ReadState.Closed)
            {
                string response = fastAccessFundingResponseReader.ReadOuterXml();
                XmlSerializer serializer = new XmlSerializer(typeof(fastAccessFundingResponse));
                StringReader reader = new StringReader(response);
                fastAccessFundingResponse i = (fastAccessFundingResponse)serializer.Deserialize(reader);

                if (!fastAccessFundingResponseReader.ReadToFollowing("fastAccessFundingResponse"))
                {
                    fastAccessFundingResponseReader.Close();
                }

                return i;
            }

            return null;
        }
    
    }



    [System.Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class RFRResponse
    {
        [XmlAttribute]
        public string response;
        [XmlAttribute]
        public string message;
    }

    [System.Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class accountUpdateResponseCardTokenType : cardTokenType
    {
        public string bin;
    }

    [System.Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class accountUpdateResponse : transactionTypeWithReportGroup
    {
        public long cnpTxnId;
        public string orderId;
        public string response;
        public DateTime responseTime;
        public string message;

        //Optional child elements
        public cardType updatedCard;
        public cardType originalCard;
        public accountUpdateResponseCardTokenType originalToken;
        public accountUpdateResponseCardTokenType updatedToken;
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute("echeckVoidResponse", Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class echeckVoidResponse : transactionTypeWithReportGroup
    {

        private long cnpTxnIdField;

        private string responseField;

        private System.DateTime responseTimeField;

        private System.DateTime postDateField;

        private string messageField;

        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }

        /// <remarks/>
        public string response
        {
            get
            {
                return this.responseField;
            }
            set
            {
                this.responseField = value;
            }
        }

        /// <remarks/>
        public System.DateTime responseTime
        {
            get
            {
                return this.responseTimeField;
            }
            set
            {
                this.responseTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime postDate
        {
            get
            {
                return this.postDateField;
            }
            set
            {
                this.postDateField = value;
            }
        }

        /// <remarks/>
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute("voidResponse", Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class voidResponse : transactionTypeWithReportGroup
    {

        private long cnpTxnIdField;

        private string responseField;

        private System.DateTime responseTimeField;

        private System.DateTime postDateField;

        private string messageField;

        private voidRecyclingResponseType recyclingResponseField;

        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }

        /// <remarks/>
        public string response
        {
            get
            {
                return this.responseField;
            }
            set
            {
                this.responseField = value;
            }
        }

        /// <remarks/>
        public System.DateTime responseTime
        {
            get
            {
                return this.responseTimeField;
            }
            set
            {
                this.responseTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime postDate
        {
            get
            {
                return this.postDateField;
            }
            set
            {
                this.postDateField = value;
            }
        }

        /// <remarks/>
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

        
        //private voidRecyclingResponseType recycling;
        public voidRecyclingResponseType recyclingResponse
        {
            get
            {
                return this.recyclingResponseField;
            }
            set
            {
                this.recyclingResponseField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    public class giftCardResponse
    {
        public DateTime txnTime;
        public string refCode;
        public int systemTraceId;
        public string sequenceNumber;
        public String availableBalance;
        public String beginningBalance;
        public String endingBalance;
        public String cashBackAmount;
    }

        /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    public class virtualGiftCardResponseType
    {
        public String accountNumber;
        public String cardValidationNum;
        public string pin;
    }

        /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema")]
    public class activateResponse : transactionTypeWithReportGroup
    {
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public long cnpTxnIdField;
        public string message;
        public string response;
        public DateTime responseTime;
        public DateTime postDate;
        public fraudResult fraudResult;
        public giftCardResponse giftCardResponse;
        public virtualGiftCardResponseType virtualGiftCardResponse;

        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }


    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class loadResponse : transactionTypeWithReportGroup
    {
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public long cnpTxnIdField; 
        public string message; 
        
        public string response;
        public DateTime responseTime;
        public DateTime postDate;
        public fraudResult fraudResult;
        public giftCardResponse giftCardResponse;

        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class unloadResponse : transactionTypeWithReportGroup
    {
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public long cnpTxnIdField;
        public string response;
        public DateTime responseTime;
        public DateTime postDate;
        public string message;
        public fraudResult fraudResult;
        public giftCardResponse giftCardResponse;

        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class balanceInquiryResponse : transactionTypeWithReportGroup
    {
        public long cnpTxnIdField;
        public string response;
        public DateTime responseTime;
        public DateTime postDate;
        public string message;
        public fraudResult fraudResult;
        public giftCardResponse giftCardResponse;

        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class deactivateResponse : transactionTypeWithReportGroup
    {
        public long cnpTxnIdField;
        public string response;
        public DateTime responseTime;
        public DateTime postDate;
        public string message;
        public fraudResult fraudResult;
        public giftCardResponse giftCardResponse;

        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class createPlanResponse : recurringTransactionResponseType
    {
        public string planCode;
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class updatePlanResponse : recurringTransactionResponseType
    {
        public string planCode;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.vantivcnp.com/schema", IsNullable=false)]
    public class loadReversalResponse : transactionTypeWithReportGroup {
        
        private long cnpTxnIdField;
        
        private string responseField;
        
        private System.DateTime responseTimeField;
        
        private System.DateTime postDateField;
        
        private bool postDateFieldSpecified;
        
        private string messageField;
        
        private giftCardResponse giftCardResponseField;
        
        /// <remarks/>
        public long cnpTxnId {
            get {
                return this.cnpTxnIdField;
            }
            set {
                this.cnpTxnIdField = value;
            }
        }
        
        /// <remarks/>
        public string response {
            get {
                return this.responseField;
            }
            set {
                this.responseField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime responseTime {
            get {
                return this.responseTimeField;
            }
            set {
                this.responseTimeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date")]
        public System.DateTime postDate {
            get {
                return this.postDateField;
            }
            set {
                this.postDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool postDateSpecified {
            get {
                return this.postDateFieldSpecified;
            }
            set {
                this.postDateFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        public string message {
            get {
                return this.messageField;
            }
            set {
                this.messageField = value;
            }
        }
        
        /// <remarks/>
        public giftCardResponse giftCardResponse {
            get {
                return this.giftCardResponseField;
            }
            set {
                this.giftCardResponseField = value;
            }
        }
    }


    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class unloadReversalResponse : transactionTypeWithReportGroup
    {
        public long cnpTxnIdField;
        public string response;
        public DateTime responseTime;
        public DateTime postDate;
        public string message;
        public fraudResult fraudResult;
        public giftCardResponse giftCardResponse;

        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class deactivateReversalResponse : transactionTypeWithReportGroup
    {
        public long cnpTxnIdField;
        public string response;
        public DateTime responseTime;
        public DateTime postDate;
        public string message;
        public fraudResult fraudResult;
        public giftCardResponse giftCardResponse;

        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class activateReversalResponse : transactionTypeWithReportGroup
    {
        public long cnpTxnIdField;
        public string response;
        public DateTime responseTime;
        public DateTime postDate;
        public string message;
        public fraudResult fraudResult;
        public giftCardResponse giftCardResponse;

        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class refundReversalResponse : transactionTypeWithReportGroup
    {
        public long cnpTxnIdField;
        public string response;
        public DateTime responseTime;
        public DateTime postDate;
        public string message;
        public fraudResult fraudResult;
        public giftCardResponse giftCardResponse;

        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class depositReversalResponse : transactionTypeWithReportGroup
    {
        public long cnpTxnIdField;
        public string response;
        public DateTime responseTime;
        public DateTime postDate;
        public string message;
        public fraudResult fraudResult;
        public giftCardResponse giftCardResponse;

        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class applepayResponse
    {

        private string applicationPrimaryAccountNumberField;

        private string applicationExpirationDateField;

        private string currencyCodeField;

        private string transactionAmountField;

        private string cardholderNameField;

        private string deviceManufacturerIdentifierField;

        private string paymentDataTypeField;

        private byte[] onlinePaymentCryptogramField;

        private string eciIndicatorField;

        /// <remarks/>
        public string applicationPrimaryAccountNumber
        {
            get
            {
                return this.applicationPrimaryAccountNumberField;
            }
            set
            {
                this.applicationPrimaryAccountNumberField = value;
            }
        }

        /// <remarks/>
        public string applicationExpirationDate
        {
            get
            {
                return this.applicationExpirationDateField;
            }
            set
            {
                this.applicationExpirationDateField = value;
            }
        }

        /// <remarks/>
        public string currencyCode
        {
            get
            {
                return this.currencyCodeField;
            }
            set
            {
                this.currencyCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string transactionAmount
        {
            get
            {
                return this.transactionAmountField;
            }
            set
            {
                this.transactionAmountField = value;
            }
        }

        /// <remarks/>
        public string cardholderName
        {
            get
            {
                return this.cardholderNameField;
            }
            set
            {
                this.cardholderNameField = value;
            }
        }

        /// <remarks/>
        public string deviceManufacturerIdentifier
        {
            get
            {
                return this.deviceManufacturerIdentifierField;
            }
            set
            {
                this.deviceManufacturerIdentifierField = value;
            }
        }

        /// <remarks/>
        public string paymentDataType
        {
            get
            {
                return this.paymentDataTypeField;
            }
            set
            {
                this.paymentDataTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary")]
        public byte[] onlinePaymentCryptogram
        {
            get
            {
                return this.onlinePaymentCryptogramField;
            }
            set
            {
                this.onlinePaymentCryptogramField = value;
            }
        }

        /// <remarks/>
        public string eciIndicator
        {
            get
            {
                return this.eciIndicatorField;
            }
            set
            {
                this.eciIndicatorField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute("echeckPreNoteSaleResponse", Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]


    public class androidpayResponse
    {
        private string cryptogramField;

        private string expMonthField;

        private string expYearField;

        private string eciIndicatorField;

        public string cryptogram
        {
            get
            {
                return this.cryptogramField;
            }
            set
            {
                this.cryptogramField = value;
            }
        }

        public string expMonth
        {
            get
            {
                return this.expMonthField;
            }
            set
            {
                this.expMonthField = value;
            }
        }

        public string expYear
        {
            get
            {
                return this.expYearField;
            }
            set
            {
                this.expYearField = value;
            }
        }

        public string eciIndicator
        {
            get
            {
                return this.eciIndicatorField;
            }
            set
            {
                this.eciIndicatorField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute("echeckPreNoteSaleResponse", Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]

    public class sepaDirectDebitResponse
    {
        private string redirectUrlField;

        private string redirectTokenField;

        private string mandateReferenceField;

        public string redirectUrl
        {
            get
            {
                return this.redirectUrlField;
            }
            set
            {
                this.redirectUrlField = value;
            }
        }
        public string redirectToken
        {
            get
            {
                return this.redirectTokenField;
            }
            set
            {
                this.redirectTokenField = value;
            }
        }
        public string mandateReference
        {
            get
            {
                return this.mandateReferenceField;
            }
            set
            {
                this.mandateReferenceField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute("echeckPreNoteSaleResponse", Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]

    public class echeckPreNoteSaleResponse : transactionTypeWithReportGroup
    {

        private long cnpTxnIdField;

        private string responseField;

        private System.DateTime responseTimeField;

        private string messageField;


        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }

        /// <remarks/>
        public string response
        {
            get
            {
                return this.responseField;
            }
            set
            {
                this.responseField = value;
            }
        }

        /// <remarks/>
        public System.DateTime responseTime
        {
            get
            {
                return this.responseTimeField;
            }
            set
            {
                this.responseTimeField = value;
            }
        }

        /// <remarks/>
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }


    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute("echeckPreNoteCreditResponse", Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class echeckPreNoteCreditResponse : transactionTypeWithReportGroup
    {

        private long cnpTxnIdField;

        private string responseField;

        private System.DateTime responseTimeField;

        private string messageField;


        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }

       

        /// <remarks/>
        public string response
        {
            get
            {
                return this.responseField;
            }
            set
            {
                this.responseField = value;
            }
        }

        /// <remarks/>
        public System.DateTime responseTime
        {
            get
            {
                return this.responseTimeField;
            }
            set
            {
                this.responseTimeField = value;
            }
        }

        /// <remarks/>
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class fraudCheckResponse : transactionTypeWithReportGroup
    {

        private long cnpTxnIdField;

        private string responseField;

        private string messageField;

        private System.DateTime responseTimeField;

        private advancedFraudResultsType advancedFraudResultsField;

        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }

        /// <remarks/>
        public string response
        {
            get
            {
                return this.responseField;
            }
            set
            {
                this.responseField = value;
            }
        }

        /// <remarks/>
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

        /// <remarks/>
        public System.DateTime responseTime
        {
            get
            {
                return this.responseTimeField;
            }
            set
            {
                this.responseTimeField = value;
            }
        }

        /// <remarks/>
        public advancedFraudResultsType advancedFraudResults
        {
            get
            {
                return this.advancedFraudResultsField;
            }
            set
            {
                this.advancedFraudResultsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class submerchantCreditResponse : transactionTypeWithReportGroup
    {

        private long cnpTxnIdField;

        private string fundsTransferIdField;

        private string responseField;

        private System.DateTime responseTimeField;

        private System.DateTime postDateField;

        private bool postDateFieldSpecified;

        private string messageField;

        private bool duplicateField;

        private bool duplicateFieldSpecified;

        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }

        /// <remarks/>
        public string fundsTransferId
        {
            get
            {
                return this.fundsTransferIdField;
            }
            set
            {
                this.fundsTransferIdField = value;
            }
        }

        /// <remarks/>
        public string response
        {
            get
            {
                return this.responseField;
            }
            set
            {
                this.responseField = value;
            }
        }

        /// <remarks/>
        public System.DateTime responseTime
        {
            get
            {
                return this.responseTimeField;
            }
            set
            {
                this.responseTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime postDate
        {
            get
            {
                return this.postDateField;
            }
            set
            {
                this.postDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool postDateSpecified
        {
            get
            {
                return this.postDateFieldSpecified;
            }
            set
            {
                this.postDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool duplicate
        {
            get
            {
                return this.duplicateField;
            }
            set
            {
                this.duplicateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool duplicateSpecified
        {
            get
            {
                return this.duplicateFieldSpecified;
            }
            set
            {
                this.duplicateFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class payFacCreditResponse : transactionTypeWithReportGroup
    {

        private long cnpTxnIdField;

        private string fundsTransferIdField;

        private string responseField;

        private System.DateTime responseTimeField;

        private string messageField;

        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }

        /// <remarks/>
        public string fundsTransferId
        {
            get
            {
                return this.fundsTransferIdField;
            }
            set
            {
                this.fundsTransferIdField = value;
            }
        }

        /// <remarks/>
        public string response
        {
            get
            {
                return this.responseField;
            }
            set
            {
                this.responseField = value;
            }
        }

        /// <remarks/>
        public System.DateTime responseTime
        {
            get
            {
                return this.responseTimeField;
            }
            set
            {
                this.responseTimeField = value;
            }
        }

        /// <remarks/>
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class vendorCreditResponse : transactionTypeWithReportGroup
    {

        private long cnpTxnIdField;

        private string fundsTransferIdField;

        private string responseField;

        private System.DateTime responseTimeField;

        private System.DateTime postDateField;

        private bool postDateFieldSpecified;

        private string messageField;

        private bool duplicateField;

        private bool duplicateFieldSpecified;

        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }

        /// <remarks/>
        public string fundsTransferId
        {
            get
            {
                return this.fundsTransferIdField;
            }
            set
            {
                this.fundsTransferIdField = value;
            }
        }

        /// <remarks/>
        public string response
        {
            get
            {
                return this.responseField;
            }
            set
            {
                this.responseField = value;
            }
        }

        /// <remarks/>
        public System.DateTime responseTime
        {
            get
            {
                return this.responseTimeField;
            }
            set
            {
                this.responseTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime postDate
        {
            get
            {
                return this.postDateField;
            }
            set
            {
                this.postDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool postDateSpecified
        {
            get
            {
                return this.postDateFieldSpecified;
            }
            set
            {
                this.postDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool duplicate
        {
            get
            {
                return this.duplicateField;
            }
            set
            {
                this.duplicateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool duplicateSpecified
        {
            get
            {
                return this.duplicateFieldSpecified;
            }
            set
            {
                this.duplicateFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class customerCreditResponse : transactionTypeWithReportGroup
    {

        private long cnpTxnIdField;

        private string fundsTransferIdField;

        private string responseField;

        private System.DateTime responseTimeField;

        private System.DateTime postDateField;

        private bool postDateFieldSpecified;

        private string messageField;

        private bool duplicateField;

        private bool duplicateFieldSpecified;

        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }

        /// <remarks/>
        public string fundsTransferId
        {
            get
            {
                return this.fundsTransferIdField;
            }
            set
            {
                this.fundsTransferIdField = value;
            }
        }

        /// <remarks/>
        public string response
        {
            get
            {
                return this.responseField;
            }
            set
            {
                this.responseField = value;
            }
        }

        /// <remarks/>
        public System.DateTime responseTime
        {
            get
            {
                return this.responseTimeField;
            }
            set
            {
                this.responseTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime postDate
        {
            get
            {
                return this.postDateField;
            }
            set
            {
                this.postDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool postDateSpecified
        {
            get
            {
                return this.postDateFieldSpecified;
            }
            set
            {
                this.postDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool duplicate
        {
            get
            {
                return this.duplicateField;
            }
            set
            {
                this.duplicateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool duplicateSpecified
        {
            get
            {
                return this.duplicateFieldSpecified;
            }
            set
            {
                this.duplicateFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class reserveCreditResponse : transactionTypeWithReportGroup {
    
    private long cnpTxnIdField;
    
    private string fundsTransferIdField;
    
    private string responseField;
    
    private System.DateTime responseTimeField;
    
    private string messageField;
    
    /// <remarks/>
    public long cnpTxnId {
        get {
            return this.cnpTxnIdField;
        }
        set {
            this.cnpTxnIdField = value;
        }
    }
    
    /// <remarks/>
    public string fundsTransferId {
        get {
            return this.fundsTransferIdField;
        }
        set {
            this.fundsTransferIdField = value;
        }
    }
    
    /// <remarks/>
    public string response {
        get {
            return this.responseField;
        }
        set {
            this.responseField = value;
        }
    }
    
    /// <remarks/>
    public System.DateTime responseTime {
        get {
            return this.responseTimeField;
        }
        set {
            this.responseTimeField = value;
        }
    }
    
    /// <remarks/>
    public string message {
        get {
            return this.messageField;
        }
        set {
            this.messageField = value;
        }
    }
}
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class payoutOrgCreditResponse : transactionTypeWithReportGroup
    {

        private long cnpTxnIdField;

        private string fundsTransferIdField;

        private string responseField;

        private System.DateTime responseTimeField;

        private string messageField;

        private bool duplicateField;

        private bool duplicateFieldSpecified;

        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }

        /// <remarks/>
        public string fundsTransferId
        {
            get
            {
                return this.fundsTransferIdField;
            }
            set
            {
                this.fundsTransferIdField = value;
            }
        }

        /// <remarks/>
        public string response
        {
            get
            {
                return this.responseField;
            }
            set
            {
                this.responseField = value;
            }
        }

        /// <remarks/>
        public System.DateTime responseTime
        {
            get
            {
                return this.responseTimeField;
            }
            set
            {
                this.responseTimeField = value;
            }
        }

        /// <remarks/>
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

        public bool duplicate
        {
            get
            {
                return this.duplicateField;
            }
            set
            {
                this.duplicateField = value;
            }
        }

        public bool duplicateSpecified
        {
            get
            {
                return this.duplicateFieldSpecified;
            }
            set
            {
                this.duplicateFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class physicalCheckCreditResponse : transactionTypeWithReportGroup
    {

        private long cnpTxnIdField;

        private string fundsTransferIdField;

        private string responseField;

        private System.DateTime responseTimeField;

        private string messageField;

        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }

        /// <remarks/>
        public string fundsTransferId
        {
            get
            {
                return this.fundsTransferIdField;
            }
            set
            {
                this.fundsTransferIdField = value;
            }
        }

        /// <remarks/>
        public string response
        {
            get
            {
                return this.responseField;
            }
            set
            {
                this.responseField = value;
            }
        }

        /// <remarks/>
        public System.DateTime responseTime
        {
            get
            {
                return this.responseTimeField;
            }
            set
            {
                this.responseTimeField = value;
            }
        }

        /// <remarks/>
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class submerchantDebitResponse : transactionTypeWithReportGroup
    {

        private long cnpTxnIdField;

        private string fundsTransferIdField;

        private string responseField;

        private System.DateTime responseTimeField;

        private System.DateTime postDateField;

        private bool postDateFieldSpecified;

        private string messageField;

        private bool duplicateField;

        private bool duplicateFieldSpecified;

        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }

        /// <remarks/>
        public string fundsTransferId
        {
            get
            {
                return this.fundsTransferIdField;
            }
            set
            {
                this.fundsTransferIdField = value;
            }
        }

        /// <remarks/>
        public string response
        {
            get
            {
                return this.responseField;
            }
            set
            {
                this.responseField = value;
            }
        }

        /// <remarks/>
        public System.DateTime responseTime
        {
            get
            {
                return this.responseTimeField;
            }
            set
            {
                this.responseTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime postDate
        {
            get
            {
                return this.postDateField;
            }
            set
            {
                this.postDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool postDateSpecified
        {
            get
            {
                return this.postDateFieldSpecified;
            }
            set
            {
                this.postDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool duplicate
        {
            get
            {
                return this.duplicateField;
            }
            set
            {
                this.duplicateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool duplicateSpecified
        {
            get
            {
                return this.duplicateFieldSpecified;
            }
            set
            {
                this.duplicateFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class payFacDebitResponse : transactionTypeWithReportGroup
    {

        private long cnpTxnIdField;

        private string fundsTransferIdField;

        private string responseField;

        private System.DateTime responseTimeField;

        private string messageField;

        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }

        /// <remarks/>
        public string fundsTransferId
        {
            get
            {
                return this.fundsTransferIdField;
            }
            set
            {
                this.fundsTransferIdField = value;
            }
        }

        /// <remarks/>
        public string response
        {
            get
            {
                return this.responseField;
            }
            set
            {
                this.responseField = value;
            }
        }

        /// <remarks/>
        public System.DateTime responseTime
        {
            get
            {
                return this.responseTimeField;
            }
            set
            {
                this.responseTimeField = value;
            }
        }

        /// <remarks/>
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class vendorDebitResponse : transactionTypeWithReportGroup
    {

        private long cnpTxnIdField;

        private string fundsTransferIdField;

        private string responseField;

        private System.DateTime responseTimeField;

        private System.DateTime postDateField;

        private bool postDateFieldSpecified;

        private string messageField;

        private bool duplicateField;

        private bool duplicateFieldSpecified;

        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }

        /// <remarks/>
        public string fundsTransferId
        {
            get
            {
                return this.fundsTransferIdField;
            }
            set
            {
                this.fundsTransferIdField = value;
            }
        }

        /// <remarks/>
        public string response
        {
            get
            {
                return this.responseField;
            }
            set
            {
                this.responseField = value;
            }
        }

        /// <remarks/>
        public System.DateTime responseTime
        {
            get
            {
                return this.responseTimeField;
            }
            set
            {
                this.responseTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime postDate
        {
            get
            {
                return this.postDateField;
            }
            set
            {
                this.postDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool postDateSpecified
        {
            get
            {
                return this.postDateFieldSpecified;
            }
            set
            {
                this.postDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool duplicate
        {
            get
            {
                return this.duplicateField;
            }
            set
            {
                this.duplicateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool duplicateSpecified
        {
            get
            {
                return this.duplicateFieldSpecified;
            }
            set
            {
                this.duplicateFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class customerDebitResponse : transactionTypeWithReportGroup
    {

        private long cnpTxnIdField;

        private string fundsTransferIdField;

        private string responseField;

        private System.DateTime responseTimeField;

        private System.DateTime postDateField;

        private bool postDateFieldSpecified;

        private string messageField;

        private bool duplicateField;

        private bool duplicateFieldSpecified;

        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }

        /// <remarks/>
        public string fundsTransferId
        {
            get
            {
                return this.fundsTransferIdField;
            }
            set
            {
                this.fundsTransferIdField = value;
            }
        }

        /// <remarks/>
        public string response
        {
            get
            {
                return this.responseField;
            }
            set
            {
                this.responseField = value;
            }
        }

        /// <remarks/>
        public System.DateTime responseTime
        {
            get
            {
                return this.responseTimeField;
            }
            set
            {
                this.responseTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime postDate
        {
            get
            {
                return this.postDateField;
            }
            set
            {
                this.postDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool postDateSpecified
        {
            get
            {
                return this.postDateFieldSpecified;
            }
            set
            {
                this.postDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool duplicate
        {
            get
            {
                return this.duplicateField;
            }
            set
            {
                this.duplicateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool duplicateSpecified
        {
            get
            {
                return this.duplicateFieldSpecified;
            }
            set
            {
                this.duplicateFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class reserveDebitResponse : transactionTypeWithReportGroup
    {

        private long cnpTxnIdField;

        private string fundsTransferIdField;

        private string responseField;

        private System.DateTime responseTimeField;

        private string messageField;

        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }

        /// <remarks/>
        public string fundsTransferId
        {
            get
            {
                return this.fundsTransferIdField;
            }
            set
            {
                this.fundsTransferIdField = value;
            }
        }

        /// <remarks/>
        public string response
        {
            get
            {
                return this.responseField;
            }
            set
            {
                this.responseField = value;
            }
        }

        /// <remarks/>
        public System.DateTime responseTime
        {
            get
            {
                return this.responseTimeField;
            }
            set
            {
                this.responseTimeField = value;
            }
        }

        /// <remarks/>
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class payoutOrgDebitResponse : transactionTypeWithReportGroup
    {

        private long cnpTxnIdField;

        private string fundsTransferIdField;

        private string responseField;

        private System.DateTime responseTimeField;

        private string messageField;

        private bool duplicateField;

        private bool duplicateFieldSpecified;

        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }

        /// <remarks/>
        public string fundsTransferId
        {
            get
            {
                return this.fundsTransferIdField;
            }
            set
            {
                this.fundsTransferIdField = value;
            }
        }

        /// <remarks/>
        public string response
        {
            get
            {
                return this.responseField;
            }
            set
            {
                this.responseField = value;
            }
        }

        /// <remarks/>
        public System.DateTime responseTime
        {
            get
            {
                return this.responseTimeField;
            }
            set
            {
                this.responseTimeField = value;
            }
        }

        /// <remarks/>
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

        public bool duplicate
        {
            get
            {
                return this.duplicateField;
            }
            set
            {
                this.duplicateField = value;
            }
        }

        public bool duplicateSpecified
        {
            get
            {
                return this.duplicateFieldSpecified;
            }
            set
            {
                this.duplicateFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class physicalCheckDebitResponse : transactionTypeWithReportGroup
    {

        private long cnpTxnIdField;

        private string fundsTransferIdField;

        private string responseField;

        private System.DateTime responseTimeField;

        private string messageField;

        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }

        /// <remarks/>
        public string fundsTransferId
        {
            get
            {
                return this.fundsTransferIdField;
            }
            set
            {
                this.fundsTransferIdField = value;
            }
        }

        /// <remarks/>
        public string response
        {
            get
            {
                return this.responseField;
            }
            set
            {
                this.responseField = value;
            }
        }

        /// <remarks/>
        public System.DateTime responseTime
        {
            get
            {
                return this.responseTimeField;
            }
            set
            {
                this.responseTimeField = value;
            }
        }

        /// <remarks/>
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class fundingInstructionVoidResponse : transactionTypeWithReportGroup
    {

        private long cnpTxnIdField;


        private string responseField;

        private System.DateTime responseTimeField;

        private string messageField;

        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }

        /// <remarks/>
        public string response
        {
            get
            {
                return this.responseField;
            }
            set
            {
                this.responseField = value;
            }
        }

        /// <remarks/>
        public System.DateTime responseTime
        {
            get
            {
                return this.responseTimeField;
            }
            set
            {
                this.responseTimeField = value;
            }
        }

        /// <remarks/>
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class queryTransactionResponse : transactionTypeWithReportGroup
    {


        private string responseField;

        private System.DateTime responseTimeField;

        private string messageField;

        private string matchCountField;

        private ArrayList results_max10Field;

        /// <remarks/>
        public string matchCount
        {
            get
            {
                return this.matchCountField;
            }
            set
            {
                this.matchCountField = value;
            }
        }

        /// <remarks/>
        public string response
        {
            get
            {
                return this.responseField;
            }
            set
            {
                this.responseField = value;
            }
        }

        /// <remarks/>
        public System.DateTime responseTime
        {
            get
            {
                return this.responseTimeField;
            }
            set
            {
                this.responseTimeField = value;
            }
        }

        /// <remarks/>
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }
        
                /// <remarks/>
        [XmlArray("results_max10")]
        [XmlArrayItem("authorizationResponse", typeof(authorizationResponse))]
        [XmlArrayItem("captureResponse", typeof(captureResponse))]
        [XmlArrayItem("giftCardCaptureResponse", typeof(giftCardCaptureResponse))]
        [XmlArrayItem("recurringResponse", typeof(recurringResponse))]
        [XmlArrayItem("registerTokenResponse", typeof(registerTokenResponse))]
        [XmlArrayItem("authReversalResponse", typeof(authReversalResponse))]
        [XmlArrayItem("giftCardAuthReversalResponse", typeof(giftCardAuthReversalResponse))]
        [XmlArrayItem("captureGivenAuthResponse", typeof(captureGivenAuthResponse))]
        [XmlArrayItem("updateCardValidationNumOnTokenResponse", typeof(updateCardValidationNumOnTokenResponse))]
        [XmlArrayItem("cancelSubscriptionResponse", typeof(cancelSubscriptionResponse))]
        [XmlArrayItem("updateSubscriptionResponse", typeof(updateSubscriptionResponse))]
        [XmlArrayItem("createPlanResponse", typeof(createPlanResponse))]
        [XmlArrayItem("updatePlanResponse", typeof(updatePlanResponse))]
        [XmlArrayItem("activateResponse", typeof(activateResponse))]
        [XmlArrayItem("deactivateResponse", typeof(deactivateResponse))]
        [XmlArrayItem("loadResponse", typeof(loadResponse))]
        [XmlArrayItem("echeckPreNoteSaleResponse", typeof(echeckPreNoteSaleResponse))]
        [XmlArrayItem("echeckPreNoteCreditResponse", typeof(echeckPreNoteCreditResponse))]
        [XmlArrayItem("unloadResponse", typeof(unloadResponse))]
        [XmlArrayItem("balanceInquiryResponse", typeof(balanceInquiryResponse))]
        [XmlArrayItem("payFacCreditResponse", typeof(payFacCreditResponse))]
        [XmlArrayItem("vendorDebitResponse", typeof(vendorDebitResponse))]
        [XmlArrayItem("reserveDebitResponse", typeof(reserveDebitResponse))]
        [XmlArrayItem("creditResponse", typeof(creditResponse))]
        [XmlArrayItem("giftCardCreditResponse", typeof(giftCardCreditResponse))]
        [XmlArrayItem("forceCaptureResponse", typeof(forceCaptureResponse))]
        [XmlArrayItem("echeckCreditResponse", typeof(echeckCreditResponse))]
        [XmlArrayItem("echeckRedepositResponse", typeof(echeckRedepositResponse))]
        [XmlArrayItem("echeckSalesResponse", typeof(echeckSalesResponse))]
        [XmlArrayItem("saleResponse", typeof(saleResponse))]
        [XmlArrayItem("fastAccessFundingResponse", typeof(fastAccessFundingResponse))]

        public ArrayList results_max10
        {
            get
            {
                return this.results_max10Field;
            }
            set
            {
                results_max10Field = value;
            }

        }
 
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.vantivcnp.com/schema", IsNullable=false)]
    public class fastAccessFundingResponse : transactionTypeWithReportGroup {
        
        private long cnpTxnIdField;
        
        private string fundsTransferIdField;
        
        private string responseField;
        
        private System.DateTime responseTimeField;
        
        private System.DateTime postDateField;
        
        private bool postDateFieldSpecified;
        
        private string messageField;
        
        private tokenResponseType tokenResponseField;
        
        private bool duplicateField;
        
        private bool duplicateFieldSpecified;
        
        /// <remarks/>
        public long cnpTxnId {
            get {
                return this.cnpTxnIdField;
            }
            set {
                this.cnpTxnIdField = value;
            }
        }
        
        /// <remarks/>
        public string fundsTransferId {
            get {
                return this.fundsTransferIdField;
            }
            set {
                this.fundsTransferIdField = value;
            }
        }
        
        /// <remarks/>
        public string response {
            get {
                return this.responseField;
            }
            set {
                this.responseField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime responseTime {
            get {
                return this.responseTimeField;
            }
            set {
                this.responseTimeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date")]
        public System.DateTime postDate {
            get {
                return this.postDateField;
            }
            set {
                this.postDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool postDateSpecified {
            get {
                return this.postDateFieldSpecified;
            }
            set {
                this.postDateFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        public string message {
            get {
                return this.messageField;
            }
            set {
                this.messageField = value;
            }
        }
        
        /// <remarks/>
        public tokenResponseType tokenResponse {
            get {
                return this.tokenResponseField;
            }
            set {
                this.tokenResponseField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool duplicate {
            get {
                return this.duplicateField;
            }
            set {
                this.duplicateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool duplicateSpecified {
            get {
                return this.duplicateFieldSpecified;
            }
            set {
                this.duplicateFieldSpecified = value;
            }
        }
    }


    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class queryTransactionUnavailableResponse : transactionTypeWithReportGroup
    {

        private long cnpTxnIdField;

        private string responseField;

        private string messageField;

        /// <remarks/>
        public long cnpTxnId
        {
            get
            {
                return this.cnpTxnIdField;
            }
            set
            {
                this.cnpTxnIdField = value;
            }
        }
        

        /// <remarks/>
        public string response
        {
            get
            {
                return this.responseField;
            }
            set
            {
                this.responseField = value;
            }
        }

        /// <remarks/>
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }
    }

    public class networkRespnse
    {
        private string endpointField;
        private networkField networkFieldField;

        public string endpoint
        {
            get
            {
                return this.endpointField;
            }
            set
            {
                this.endpointField = value;
            }
        }

        public networkField networkField
        {
            get
            {
                return this.networkFieldField;
            }
            set
            {
                this.networkFieldField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]

    public class networkField
    {
        private string fieldValueField;
        private networkSubField networkSubFieldField;
        private int fieldNumberField;
        private string fieldNameField;

        public string fieldValue
        {
            get
            {
                return this.fieldValueField;
            }
            set
            {
                this.fieldValueField = value;
            }
        }

        public networkSubField networkSubField
        {
            get
            {
                return this.networkSubFieldField;
            }
            set
            {
                this.networkSubFieldField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int fieldNumber
        {
            get
            {
                return this.fieldNumberField;
            }
            set
            {
                this.fieldNumberField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string fieldName
        {
            get
            {
                return this.fieldNameField;
            }
            set
            {
                this.fieldNameField = value;
            }
        }
    }
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]

    public class networkSubField
    {
        private string fieldValueField;
        private int fieldNumberField;

        public string fieldValue
        {
            get
            {
                return this.fieldValueField;
            }
            set
            {
                this.fieldValueField = value;
            }
        }

        public int fieldNumber
        {
            get
            {
                return this.fieldNumberField;
            }
            set
            {
                this.fieldNumberField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.vantivcnp.com/schema", IsNullable=false)]
    public class pinlessDebitResponse {
    
        private string networkNameField;
    
        /// <remarks/>
        public string networkName {
            get {
                return this.networkNameField;
            }
            set {
                this.networkNameField = value;
            }
        }
    }

    public class idealResponse
    {
        private string redirectUrlField;
        private string redirectTokenField;
        private string paymentPurposeField;

        public string redirectUrl
        {
            get
            {
                return this.redirectUrlField;
            }
            set
            {
                this.redirectUrlField = value;
            }
        }

        public string redirectToken
        {
            get
            {
                return this.redirectTokenField;
            }
            set
            {
                this.redirectTokenField = value;
            }
        }

        public string paymentPurpose
        {
            get
            {
                return this.paymentPurposeField;
            }
            set
            {
                this.paymentPurposeField = value;
            }
        }
    }

    public class giropayResponse
    {
        private string redirectUrlField;
        private string redirectTokenField;
        private string paymentPurposeField;

        public string redirectUrl
        {
            get
            {
                return this.redirectUrlField;
            }
            set
            {
                this.redirectUrlField = value;
            }
        }

        public string redirectToken
        {
            get
            {
                return this.redirectTokenField;
            }
            set
            {
                this.redirectTokenField = value;
            }
        }

        public string paymentPurpose
        {
            get
            {
                return this.paymentPurposeField;
            }
            set
            {
                this.paymentPurposeField = value;
            }
        }
    }

    public class sofortResponse
    {
        private string redirectUrlField;
        private string redirectTokenField;
        private string paymentPurposeField;

        public string redirectUrl
        {
            get
            {
                return this.redirectUrlField;
            }
            set
            {
                this.redirectUrlField = value;
            }
        }

        public string redirectToken
        {
            get
            {
                return this.redirectTokenField;
            }
            set
            {
                this.redirectTokenField = value;
            }
        }

        public string paymentPurpose
        {
            get
            {
                return this.paymentPurposeField;
            }
            set
            {
                this.paymentPurposeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute("translateToLowValueTokenRequest", Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class translateToLowValueTokenRequestType : transactionTypeWithReportGroup
    {

        private string orderIdField;

        private string tokenField;

        /// <remarks/>
        public string orderId
        {
            get
            {
                return this.orderIdField;
            }
            set
            {
                this.orderIdField = value;
            }
        }

        /// <remarks/>
        public string token
        {
            get
            {
                return this.tokenField;
            }
            set
            {
                this.tokenField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vantivcnp.com/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vantivcnp.com/schema", IsNullable = false)]
    public class translateToLowValueTokenResponse : transactionTypeWithReportGroup
    {

        private string orderIdField;

        private string paypageRegistrationIdField;

        private string responseField;

        private string messageField;

        private System.DateTime responseTimeField;

        /// <remarks/>
        public string orderId
        {
            get
            {
                return this.orderIdField;
            }
            set
            {
                this.orderIdField = value;
            }
        }

        /// <remarks/>
        public string paypageRegistrationId
        {
            get
            {
                return this.paypageRegistrationIdField;
            }
            set
            {
                this.paypageRegistrationIdField = value;
            }
        }

        /// <remarks/>
        public string response
        {
            get
            {
                return this.responseField;
            }
            set
            {
                this.responseField = value;
            }
        }

        /// <remarks/>
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

        /// <remarks/>
        public System.DateTime responseTime
        {
            get
            {
                return this.responseTimeField;
            }
            set
            {
                this.responseTimeField = value;
            }
        }
    }
}

*/