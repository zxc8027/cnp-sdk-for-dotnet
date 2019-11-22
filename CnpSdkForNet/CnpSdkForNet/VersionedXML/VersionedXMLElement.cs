/*
 * Zachary Cook
 *
 * Base element that can be encoded as an XML object
 * with limitations based on the version.
 */

namespace Cnp.Sdk.VersionedXML
{
    [XMLElement(Name = "BaseXMLElement")]
    public class VersionedXMLElement
    {
        /*
         * Converts an object to a string.
         */
        public static string ConvertToString(object objectToConvert, double version)
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
            return objectToConvert.ToString();
        }
        
        /*
         * Serializes the element.
         */
        public string Serialize(double version)
        {
            // Get the name.
            var selfType = this.GetType();
            var name = selfType.Name;
            foreach (XMLElement attribute in selfType.GetCustomAttributes(typeof(XMLElement),true))
            {
                name = attribute.Name;
                break;
            }
            
            // Serialize the object.
            return this.Serialize(version,name);
        }
        
        /*
         * Serializes the element with a give name.
         */
        public string Serialize(double version, string elementName)
        {
            var selfType = this.GetType();
            var members = selfType.GetMembers();
            var xmlString = "<" + elementName;
            
            // Add the attributes.
            foreach (var member in members)
            {
                foreach (XMLAttribute attribute in member.GetCustomAttributes(typeof(XMLAttribute),true))
                {
                    var property = selfType.GetProperty(member.Name);
                    if (property != null)
                    {
                        var value = property.GetValue(this,null);
                        if (value != null)
                        {
                            // Add the attribute.
                            xmlString += " " + attribute.Name + "=\"" + ConvertToString(value,version) + "\"";
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
                foreach (XMLElement attribute in member.GetCustomAttributes(typeof(XMLElement),true))
                {
                    var property = selfType.GetProperty(member.Name);
                    if (property != null)
                    {
                        var value = property.GetValue(this, null);
                        if (value != null)
                        {
                            // Add the child element.
                            if (value is VersionedXMLElement)
                            {
                                xmlString += ((VersionedXMLElement) value).Serialize(version, attribute.Name);
                            }
                            else
                            {
                                xmlString += "<" + attribute.Name + ">" + ConvertToString(value,version) + "</" + attribute.Name + ">";
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
    }
}