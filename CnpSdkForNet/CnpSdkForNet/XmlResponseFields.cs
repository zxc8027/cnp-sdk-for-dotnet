/*
 * Zachary Cook
 * 
 * Fields for XML responses. Refer to the XML reference guides
 * for further documentation.
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
}