/*
 * Zachary Cook
 *
 * Base element that can be encoded as an XML object
 * with limitations based on the version.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;

namespace Cnp.Sdk.VersionedXML
{
    [XMLElement(Name = "BaseXMLElement")]
    public class VersionedXMLElement
    {
        private Dictionary<string,string> additionalAttributes = new Dictionary<string, string>();
        private List<string> additionalElements = new List<string>();
        
        /*
         * Converts an object to a string.
         */
        public static string ConvertToString(object objectToConvert, XMLVersion version)
        {
            // Return an XML string if it is an XML object.
            if (objectToConvert is VersionedXMLElement)
            {
                return ((VersionedXMLElement) objectToConvert).Serialize(version);
            }

            // Return an XSD time if it is a DateTime.
            if (objectToConvert is DateTime)
            {
                // Get the components.
                var dateTime = (DateTime) objectToConvert;
                var year = dateTime.Year.ToString();
                var month = dateTime.Month.ToString();
                var day = dateTime.Day.ToString();
                
                // Add the leading zeros.
                if (dateTime.Month < 10)
                {
                    month = "0" + month;
                }
                if (dateTime.Day < 10)
                {
                    day = "0" + day;
                }

                // Return the formatted time.
                return year + "-" + month + "-" + day;
            }

            // Return a boolean as lowercase if it is a bool.
            if (objectToConvert is bool)
            {
                return objectToConvert.ToString().ToLower();
            }
            
            // Return an enum value if it is an enum.
            if (objectToConvert.GetType().IsEnum)
            {
                // Get the name of the enum.
                var enumType = objectToConvert.GetType();
                var enumName = Enum.GetName(enumType,objectToConvert);
                var attributeDefined = false;
                var nameDefined = false;
                foreach (XMLEnum attribute in enumType.GetField(enumName).GetCustomAttributes(typeof(XMLEnum),false))
                {
                    attributeDefined = true;
                    if (attribute.IsVersionValid(version))
                    {
                        if (nameDefined == false)
                        {
                            // Set the name if it is null.
                            nameDefined = true;
                            enumName = attribute.Name;
                        }
                        else
                        {
                            // Throw an exception if overlapping names exist (could cause unexpected behavior).
                            throw new OverlappingVersionsException(enumType.Name + "." + Enum.GetName(enumType,objectToConvert),version);
                        }
                    }
                }
                
                // Throw an exception if the attribute is defined but not set.
                if (attributeDefined && !nameDefined)
                {
                    throw new InvalidVersionException(objectToConvert.GetType().Name + "." +  enumName,version);
                }
                
                // Return the escaped enum value.
                return SecurityElement.Escape(enumName);
            }

            // Return the ToString result.
            return SecurityElement.Escape(objectToConvert.ToString());
        }
        
        /*
         * Converts an object to an XML element string.
         */
        public static string ConvertToElement(string name,object objectToConvert,XMLVersion version)
        {
            // Return a serialized XML element if it is an XML element.
            if (objectToConvert is VersionedXMLElement)
            {
                return ((VersionedXMLElement) objectToConvert).Serialize(version,name);
            }
            
            // Return the object as a string.
            return "<" + name + ">" + ConvertToString(objectToConvert,version) + "</" + name + ">";
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
         * Sets an additional attribute.
         */
        public void SetAdditionalAttribute(string name,string value)
        {
            this.additionalAttributes.Add(name,SecurityElement.Escape(value));
        }
        
        /*
         * Adds an additional element.
         */
        public void AddAdditionalElement(string element)
        {
            this.additionalElements.Add(element);
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
            
            // Add the additional attributes.
            foreach (var attributeName in this.additionalAttributes.Keys)
            {
                xmlString += " " + attributeName + "=\"" + this.additionalAttributes[attributeName] + "\"";
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
                                    if (value is IList)
                                    {
                                        foreach (var listObject in (IList) value)
                                        {
                                            xmlString += ConvertToElement(attribute.Name,listObject,version);
                                        }
                                    } else {
                                        xmlString += ConvertToElement(attribute.Name,value,version);
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
            
            // Add additional child elements.
            var additionalElements = this.GetAdditionalElements(version);
            if (additionalElements != null)
            {
                foreach (var additionalElement in additionalElements)
                {
                    xmlString += additionalElement;
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
            
        }
        
        /*
         * Returns additional elements to add when serializing.
         * This method must handle all escaping of special characters.
         */
        public virtual List<string> GetAdditionalElements(XMLVersion version)
        {
            return this.additionalElements;
        }
        
        /*
         * Parses elements that aren't defined by properties.
         */
        public virtual void ParseAdditionalElements(XMLVersion version, List<string> elements)
        {
            
        }
    }
}