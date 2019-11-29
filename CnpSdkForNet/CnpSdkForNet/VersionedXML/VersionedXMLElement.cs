/*
 * Zachary Cook
 *
 * Base element that can be encoded as an XML object
 * with limitations based on the version.
 */

using System;
using System.Security;

namespace Cnp.Sdk.VersionedXML
{
    [XMLElement(Name = "BaseXMLElement")]
    public class VersionedXMLElement
    {
        /*
         * Converts an object to a string.
         */
        public static string ConvertToString(object objectToConvert,XMLVersion version)
        {
            // Return an XML string if it is an XML object.
            if (objectToConvert is VersionedXMLElement)
            {
                return ((VersionedXMLElement) objectToConvert).Serialize(version);
            }
            
            // Return a boolean as lowercase if it is a bool.
            if (objectToConvert is bool)
            {
                return objectToConvert.ToString().ToLower();
            }

            // Return the ToString result.
            return SecurityElement.Escape(objectToConvert.ToString());
        }

        /*
         * Serializes the element.
         */
        public string Serialize(XMLVersion version)
        {
            // Get the name.
            string name = null;
            bool attributeDefined = false;
            var selfType = this.GetType();
            foreach (XMLElement attribute in selfType.GetCustomAttributes(typeof(XMLElement),true))
            {
                attributeDefined = true;
                if (attribute.IsVersionValid(version))
                {
                    if (name == null)
                    {
                        // Set the name if it is null.
                        name = attribute.Name;
                    }
                    else
                    {
                        // Throw an exception if overlapping names exist (could cause unexpected behavior).
                        throw new OverlappingVersionsException(selfType.Name,version);
                    }
                }
            }
            
            // Throw an exception if an attribute is defined but the name isn't. This happens
            // when the serializing version is invalid.
            if (name == null && attributeDefined)
            {
                throw new InvalidVersionException(selfType.Name, version);
            }

            // Set the name to the object name if no attribute is present.
            if (name == null)
            {
                name = selfType.Name;
            }
            
            // Serialize the object.
            return this.Serialize(version,name);
        }
        
        /*
         * Serializes the element with a give name.
         */
        public string Serialize(XMLVersion version,string elementName)
        {
            var selfType = this.GetType();
            var members = selfType.GetMembers();
            var xmlString = "<" + elementName;
            
            // Pre-serialize the object.
            this.PreSerialize(version);
            
            // Add the attributes.
            foreach (var member in members)
            {
                bool attributeAdded = false;
                foreach (XMLAttribute attribute in member.GetCustomAttributes(typeof(XMLAttribute),true))
                {
                    var property = selfType.GetProperty(member.Name);
                    if (property != null)
                    {
                        if (attribute.IsVersionValid(version)) {
                            if (attributeAdded)
                            {
                                // Throw an exception if the attribute overlaps.
                                throw new OverlappingVersionsException(member.Name,version);
                            }
                            else
                            {
                                // Add the attribute if it isn't null.
                                attributeAdded = true;
                                var value = property.GetValue(this, null);
                                if (value != null)
                                {
                                    // Add the attribute.
                                    xmlString += " " + attribute.Name + "=\"" + ConvertToString(value, version) + "\"";
                                }
                            }
                        }
                    }
                    else
                    {
                        // Throw an exception if the property is null ("{ get; set; }" is missing).
                        throw new MissingGetterException(member.Name);
                    }
                }
            }
            xmlString += ">";
            
            // Add the child elements.
            foreach (var member in members)
            {
                bool elementAdded = false;
                foreach (XMLElement attribute in member.GetCustomAttributes(typeof(XMLElement),true))
                {
                    var property = selfType.GetProperty(member.Name);
                    if (property != null)
                    {
                        var value = property.GetValue(this, null);
                        if (value != null)
                        {
                            if (attribute.IsVersionValid(version))
                            {
                                if (elementAdded)
                                {
                                    // Throw an exception if the element overlaps.
                                    throw new OverlappingVersionsException(member.Name,version);
                                }
                                else
                                {
                                    // Add the child element.
                                    elementAdded = true;
                                    if (value is VersionedXMLElement)
                                    {
                                        xmlString += ((VersionedXMLElement) value).Serialize(version, attribute.Name);
                                    }
                                    else
                                    {
                                        xmlString += "<" + attribute.Name + ">" + ConvertToString(value, version) +
                                                     "</" +
                                                     attribute.Name + ">";
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // Throw an exception if the property is null ("{ get; set; }" is missing).
                        throw new MissingGetterException(member.Name);
                    }
                }
            }

            // Return the final string with the ending tag.
            return xmlString + "</" + elementName + ">";
        }
        
        /*
         * Invoked before serializing the object to finalize
         * setting of elements.
         */
        public virtual void PreSerialize(XMLVersion version)
        {
            Console.WriteLine("CALL");
        }
    }
}