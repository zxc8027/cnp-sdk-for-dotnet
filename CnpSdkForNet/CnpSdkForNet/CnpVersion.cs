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
        public const string CurrentCNPXMLVersion = "12.10";
        public const string CurrentCNPSDKVersion = "DotNet;" + CurrentCNPXMLVersion + ".3";
    }
}