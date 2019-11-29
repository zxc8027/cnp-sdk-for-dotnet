/*
 * Zachary Cook
 * 
 * Fields for XML responses. Refer to the XML reference guides
 * for further documentation.
 */

using System;
using System.Xml.Serialization;
using Cnp.Sdk.VersionedXML;

namespace Cnp.Sdk
{
    [XMLElement(Name = "vendorDebitResponse")]
    public class vendorDebitResponse : transactionTypeWithReportGroup
    {
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

        [XMLAttribute(Name = "duplicate")]
        public bool? duplicate { get; set; }
    }

    public class cnpOnlineResponse
    {

        [XMLAttribute(Name = "response")]
        public string response { get; set; }

        [XMLAttribute(Name = "message")]
        public string message { get; set; }
        
        [XMLAttribute(Name = "version")]
        public string version { get; set; }

        [XMLElement(Name = "vendorDebitResponse")]
        public vendorDebitResponse vendorDebitResponse { get; set; }
    }
}