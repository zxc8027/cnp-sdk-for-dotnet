/*
 * Zachary Cook
 *
 * Stores version information and compares versions.
 */

using System;

namespace Cnp.Sdk.VersionedXML
{
    public class XMLVersion
    {
        public int MainVersion = 0;
        public int SubVersion = 0;
        
        /*
         * Creates a version.
         */
        public XMLVersion()
        {
            
        }
        
        /*
         * Creates a version.
         */
        public XMLVersion(int mainVersion,int subVersion)
        {
            this.MainVersion = mainVersion;
            this.SubVersion = subVersion;
        }
        
        /*
         * Returns the version as a string.
         */
        public override string ToString()
        {
            return this.MainVersion + "." + this.SubVersion;
        }
        
        /*
         * Returns if it is equal to another version.
         */
        public override bool Equals(object obj){
            // Return false if the type is incorrect.
            if (!(obj is XMLVersion)) return false;
            
            // Compare the strings.
            return ((XMLVersion) obj).ToString() == this.ToString();
        }
        
        public static bool operator ==(XMLVersion version1,XMLVersion version2)
        {
            return !(version1 is null) && version1.Equals(version2);
        }

        public static bool operator !=(XMLVersion version1,XMLVersion version2)
        {
            return !(version1 == version2);
        }
    }
}