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
        public string FirstVersion { get; set; } = null;
        public string RemovedVersion { get; set; } = null;
        
        /*
         * Returns if a version is valid.
         */
        public bool IsVersionValid(XMLVersion otherVersion)
        {
            // Create the versions to compare. If they are null, the versions will be null.
            var firstVersion = XMLVersion.FromString(this.FirstVersion);
            var removedVersion = XMLVersion.FromString(this.RemovedVersion);
            
            // Return if the version is valid.
            if (firstVersion != null && removedVersion != null)
            {
                return firstVersion <= otherVersion && removedVersion > otherVersion;
            } else if (firstVersion != null)
            {
                return firstVersion <= otherVersion;
            } else if (removedVersion != null)
            {
                return removedVersion > otherVersion;
            }
            
            // Return true (first and removed versions not defined).
            return true;
        }
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
    
    /*
     * Stores information for encoding an enum.
     */
    public class XMLEnum : XMLVersionInformation
    {
        public string Name { get; set; }
    }
}