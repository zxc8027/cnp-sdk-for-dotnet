/*
 * Zachary Cook
 *
 * Stores version information and compares versions.
 */

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
        
        /*
         * Returns if two versions are equals.
         */
        public static bool operator ==(XMLVersion version1,XMLVersion version2)
        {
            return !(version1 is null) && version1.Equals(version2);
        }
        
        /*
         * Returns if two versions are not equals.
         */
        public static bool operator !=(XMLVersion version1,XMLVersion version2)
        {
            return !(version1 == version2);
        }
        
        /*
         * Returns if a version is greater than another version.
         */
        public static bool operator >(XMLVersion version1,XMLVersion version2)
        {
            return version1.MainVersion > version2.MainVersion || (version1.MainVersion == version2.MainVersion && version1.SubVersion > version2.SubVersion);
        }
        
        /*
         * Returns if a version is less than another version.
         */
        public static bool operator <(XMLVersion version1,XMLVersion version2)
        {
            return !(version1 > version2 || version1 == version2);
        }
        
        /*
         * Returns if a version is greater or equal to than another version.
         */
        public static bool operator >=(XMLVersion version1,XMLVersion version2)
        {
            return !(version1 < version2);
        }
        
        /*
         * Returns if a version is less or equal to than another version.
         */
        public static bool operator <=(XMLVersion version1,XMLVersion version2)
        {
            return !(version1 > version2);
        }
    }
}