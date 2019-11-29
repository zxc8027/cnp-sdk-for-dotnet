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
    public enum echeckAccountTypeEnum
    {
        Checking,
        Savings,
        Corporate,
        CorpSavings,
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