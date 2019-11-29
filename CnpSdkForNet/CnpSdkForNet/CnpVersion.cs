/*
 * Zachary Cook
 *
 * Stores the current version of the SDK.
 */

using Cnp.Sdk.VersionedXML;

namespace Cnp.Sdk
{
    public class CnpVersion
    {
        public static readonly XMLVersion CurrentCNPXMLVersion = new XMLVersion(12,10);
        public static readonly string CurrentCNPSDKVersion = "DotNet;" + CurrentCNPXMLVersion + ".3";
    }
}