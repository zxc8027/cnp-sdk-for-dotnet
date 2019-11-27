/*
 * Zachary Cook
 *
 * Deserializes strings into VersionedXMLElements.
 */

using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Cnp.Sdk.VersionedXML
{
    public class VersionedXMLDeserializer
    {
        /*
         * Returns if a property is an XML element.
         */
        public static bool IsXMLElement(PropertyInfo property)
        {
            var type = property.PropertyType;

            // Move up the type until null is reached.
            while (type != null)
            {
                // If the type matches, return true.
                if (type == typeof(VersionedXMLElement))
                {
                    return true;
                }

                // Move up the type.
                type = type.BaseType;
            }

            // Return false (base case).
            return false;
        }

        /*
         * Deserializes a string to an XML element.
         */
        public static T Deserialize<T>(string xmlString,XMLVersion version)
        {
            xmlString = xmlString.Trim();
            
            // Create the new object.
            var newObject = (T) Activator.CreateInstance(typeof(T),new object[] {});
            var selfType = newObject.GetType();
            var members = selfType.GetMembers();
            
            // Create an XML parser and get the attributes and elements.
            var xmlDocument = XDocument.Parse(xmlString);
            var elements = xmlDocument.Root.Elements().ToArray();
            var attributes = xmlDocument.Root.Attributes().ToArray();
            
            // Set the attributes.
            foreach (var xmlAttribute in attributes)
            {
                // Find the attribute to add to.
                MemberInfo memberToSet = null; 
                foreach (var member in members)
                {
                    foreach (XMLAttribute attribute in member.GetCustomAttributes(typeof(XMLAttribute),true))
                    {
                        var property = selfType.GetProperty(member.Name);
                        if (property != null)
                        {
                            if (attribute.IsVersionValid(version) && attribute.Name.ToLower() == xmlAttribute.Name.LocalName.ToLower()) {
                                if (memberToSet != null)
                                {
                                    // Throw an exception if the attribute overlaps.
                                    throw new OverlappingVersionsException(member.Name,version);
                                }
                                else
                                {
                                    memberToSet = member;
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
                
                // If the attribute exists, set the value.
                if (memberToSet != null)
                {
                    var property = selfType.GetProperty(memberToSet.Name);
                    var typeConverter = TypeDescriptor.GetConverter(property.PropertyType);
                    property.SetValue(newObject,typeConverter.ConvertFromString(xmlAttribute.Value));
                }
            }
            
            // Set the elements.
            foreach (var xmlElement in elements)
            {
                // Find the attribute to add to.
                MemberInfo memberToSet = null; 
                foreach (var member in members)
                {
                    foreach (XMLElement attribute in member.GetCustomAttributes(typeof(XMLElement),true))
                    {
                        var property = selfType.GetProperty(member.Name);
                        if (property != null)
                        {
                            if (attribute.IsVersionValid(version) && attribute.Name.ToLower() == xmlElement.Name.LocalName.ToLower()) {
                                if (memberToSet != null)
                                {
                                    // Throw an exception if the attribute overlaps.
                                    throw new OverlappingVersionsException(member.Name,version);
                                }
                                else
                                {
                                    memberToSet = member;
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
                
                // If the attribute exists, set the value.
                if (memberToSet != null)
                {
                    // Set the value.
                    var property = selfType.GetProperty(memberToSet.Name);
                    if (IsXMLElement(property))
                    {
                        if (xmlElement.Value.Length != 0)
                        {
                            throw new Exception("Can't parse XML yet: " + xmlElement);
                        }
                    }
                    else
                    {
                        var typeConverter = TypeDescriptor.GetConverter(property.PropertyType);
                        property.SetValue(newObject, typeConverter.ConvertFromString(xmlElement.Value));
                    }
                }
            }
            
            // Return the new object.
            return newObject;
        }
    }
}