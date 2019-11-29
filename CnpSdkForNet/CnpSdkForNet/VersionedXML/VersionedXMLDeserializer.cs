/*
 * Zachary Cook
 *
 * Deserializes strings into VersionedXMLElements.
 */

using System;
using System.Collections.Generic;
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
         * Deserializes an object.
         */
        public static object DeserializeType(string xmlString, XMLVersion version, Type type)
        {
            xmlString = xmlString.Trim();

            // Create the new object.
            var newObject = Convert.ChangeType(Activator.CreateInstance(type, new object[] { }), type);
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
                    foreach (XMLAttribute attribute in member.GetCustomAttributes(typeof(XMLAttribute), true))
                    {
                        var property = selfType.GetProperty(member.Name);
                        if (property != null)
                        {
                            if (attribute.IsVersionValid(version) &&
                                attribute.Name.ToLower() == xmlAttribute.Name.LocalName.ToLower())
                            {
                                if (memberToSet != null)
                                {
                                    // Throw an exception if the attribute overlaps.
                                    throw new OverlappingVersionsException(member.Name, version);
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
                    property.SetValue(newObject, typeConverter.ConvertFromString(xmlAttribute.Value));
                }
            }

            // Set the elements.
            var unknownElements = new List<string>();
            foreach (var xmlElement in elements)
            {
                // Find the attribute to add to.
                MemberInfo memberToSet = null;
                foreach (var member in members)
                {
                    foreach (XMLElement attribute in member.GetCustomAttributes(typeof(XMLElement), true))
                    {
                        var property = selfType.GetProperty(member.Name);
                        if (property != null)
                        {
                            if (attribute.IsVersionValid(version) &&
                                attribute.Name.ToLower() == xmlElement.Name.LocalName.ToLower())
                            {
                                if (memberToSet != null)
                                {
                                    // Throw an exception if the attribute overlaps.
                                    throw new OverlappingVersionsException(member.Name, version);
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
                            property.SetValue(newObject,
                                DeserializeType(xmlElement.ToString(), version, property.PropertyType));
                        }
                    }
                    else
                    {
                        var typeConverter = TypeDescriptor.GetConverter(property.PropertyType);
                        property.SetValue(newObject, typeConverter.ConvertFromString(xmlElement.Value));
                    }
                }
                else
                {
                    unknownElements.Add(xmlElement.ToString());
                }
            }

            // Parse unknown elements.
            var method = type.GetMethod("ParseAdditionalElements");
            method.Invoke(newObject, new object[2] { version, unknownElements });
            
            // Return the new object.
            return newObject;
        }

        /*
         * Deserializes a string to an XML element.
         */
        public static T Deserialize<T>(string xmlString,XMLVersion version)
        {
            return (T) DeserializeType(xmlString,version,typeof(T));
        }
    }
}