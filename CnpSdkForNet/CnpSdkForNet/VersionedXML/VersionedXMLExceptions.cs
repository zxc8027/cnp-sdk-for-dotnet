/*
 * Zachary Cook
 *
 * Exceptions that are raised when writing and parsing XML.
 */

using System;

namespace Cnp.Sdk.VersionedXML
{
    /*
     * Exception for a getter not being set.
     */
    public class MissingGetterException : Exception
    {
        public MissingGetterException(string propertyName) : base( propertyName + " can't be read from. Make sure it has \"{ get; set; }\" next to it.")
        {
            
        }
    }
    
    /*
     * Exception for more than 1 attributes' version being valid.
     */
    public class OverlappingVersionsException : Exception
    {
        public OverlappingVersionsException(string name,XMLVersion version) : base(name + " has overlapping version information for version " + version.MainVersion + "." + version.SubVersion + ". This makes the serialization ambiguous.")
        {
            
        }
    }
    
    /*
     * Exception for no version being valid.
     */
    public class InvalidVersionException : Exception
    {
        public InvalidVersionException(string name,XMLVersion version) : base(name + " can't be used for version " + version.MainVersion + "." + version.SubVersion + ".")
        {
            
        }
    }
}