/*
 * Zachary Cook
 *
 * Attributes for objects and properties of versioned
 * XML elements.
 */

using System;

namespace Cnp.Sdk.VersionedXML
{
    /*
     * Base XML attribute for version information.
     */
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    public class XMLVersionInformation : Attribute
    {
        public double FirstVersion { get; set; } = double.MinValue;
        public double RemovedVersion { get; set; } = double.MaxValue;
    }
    
    /*
     * Stores information for encoding an element. This
     * is used for the root element or child elements.
     */
    public class XMLElement : XMLVersionInformation
    {
        public string Name { get; set; }
    }
    
    /*
     * Stores information for encoding an attribute.
     */
    public class XMLAttribute : XMLVersionInformation
    {
        public string Name { get; set; }
    }
}