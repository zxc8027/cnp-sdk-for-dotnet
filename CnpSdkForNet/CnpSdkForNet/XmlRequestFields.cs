/*
 * Zachary Cook
 * 
 * Fields for XML requests. Refer to the XML reference guides
 * for further documentation.
 */

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
}