/*
 * Zachary Cook
 *
 * Deserializes strings into VersionedXMLElements.
 */

using System;
using System.Collections;
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
         * Returns if a property is of a type.
         */
        public static bool IsOfType(Type type,Type typeToMatch)
        {
            // Return if the interface is implemented.
            foreach (var interfaceImplemented in type.GetInterfaces())
            {
                if (interfaceImplemented == typeToMatch)
                {
                    return true;
                }
            }
            
            // Return if type type matches or it is a subclass.
            return type == typeToMatch || type.IsSubclassOf(typeToMatch);
        }
        
        /*
         * Converts a string to an object.
         */
        public static object DeserializeToObject(string value,Type propertyType,XMLVersion version)
        {
            // Get the underlying type if it is nullable.
            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                propertyType = propertyType.GetGenericArguments()[0];
            }
            
            // Return an enum if a name matches.
            if (propertyType.IsEnum)
            {
                // Return if an attribute matches.
                foreach (var enumItem in Enum.GetValues(propertyType))
                {
                    foreach (XMLEnum attribute in propertyType.GetField(enumItem.ToString()).GetCustomAttributes(typeof(XMLEnum),false))
                    {
                        if (attribute.IsVersionValid(version) && attribute.Name == value)
                        {
                            return enumItem;
                        }
                    }
                }
                
                // Return if a name matches.
                foreach (var enumItem in Enum.GetValues(propertyType))
                {
                    if (enumItem.ToString() == value && propertyType.GetField(enumItem.ToString()).GetCustomAttributes(typeof(XMLEnum), false).Length == 0)
                    {
                        return enumItem;
                    }
                }
                
                // Throw an exception.
                throw new InvalidVersionException(propertyType.Name + "." + value,version);
            }
            
            // Return a converted string.
            var typeConverter = TypeDescriptor.GetConverter(propertyType);
            return typeConverter.ConvertFromString(value);
        }

        /*
         * Converts an XML element to an object.
         */
        public static object DeserializeToObject(XElement xmlElement,Type propertyType,XMLVersion version)
        {
            // Return a parsed XML element.
            if (IsOfType(propertyType,typeof(VersionedXMLElement)))
            {
                if (xmlElement.Value != "" || xmlElement.FirstAttribute != null)
                {
                    return DeserializeType(xmlElement.ToString(),version,propertyType);
                }
                else
                {
                    return null;
                }
            }
            
            // Return a converted string.
            return DeserializeToObject(xmlElement.Value,propertyType,version);
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
                    var deserializedObject = DeserializeToObject(xmlAttribute.Value,property.PropertyType,version);
                    property.SetValue(newObject, deserializedObject);
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
                    var propertyType = property.PropertyType;
                    if (IsOfType(propertyType,typeof(IList)))
                    {
                        var deserializedObject = DeserializeToObject(xmlElement,propertyType.GetGenericArguments()[0],version);
                        ((IList) property.GetValue(newObject)).Add(deserializedObject);
                    }
                    else
                    {
                        var deserializedObject = DeserializeToObject(xmlElement,propertyType,version);
                        property.SetValue(newObject,deserializedObject);
                    }
                }
                else
                {
                    unknownElements.Add(xmlElement.ToString());
                }
            }

            // Parse unknown elements.
            var method = type.GetMethod("ParseAdditionalElements");
            if (method != null)
            {
                method.Invoke(newObject, new object[2] { version, unknownElements });
            }

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