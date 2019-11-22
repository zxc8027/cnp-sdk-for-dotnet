/*
 * Zachary Cook
 *
 * Tests the XMLVersion class.
 */

using Cnp.Sdk.VersionedXML;
using NUnit.Framework;

namespace Cnp.Sdk.Test.Unit.VersionedXML
{
    [TestFixture]
    public class TestXMLVersion
    {
        /*
         * Tests the constructors.
         */
        [Test]
        public void TestConstructors()
        {
            // Test the empty constructor.
            var version = new XMLVersion();
            Assert.AreEqual(version.MainVersion,0);
            Assert.AreEqual(version.SubVersion,0);
            Assert.AreEqual(version.ToString(),"0.0");
            
            // Test the filled constructor.
            version = new XMLVersion(1,2);
            Assert.AreEqual(version.MainVersion,1);
            Assert.AreEqual(version.SubVersion,2);
            Assert.AreEqual(version.ToString(),"1.2");
        }
        
        /*
         * Tests comparing versions.
         */
        [Test]
        public void TestComparisons()
        {
            // Create 3 versions.
            var version1 = new XMLVersion(1,2);
            var version2 = new XMLVersion(1,5);
            var version3 = new XMLVersion(2,2);
            
            // Assert that comparing equals is correct.
            Assert.AreEqual(version1,new XMLVersion(1,2));
            Assert.AreEqual(version1,version1);
            Assert.AreNotEqual(version1,version2);
            Assert.AreNotEqual(version1,version3);
            Assert.AreNotEqual(version1,"1.2");
            
            // Assert the comparisons are correct.
            Assert.IsTrue(version1 == new XMLVersion(1,2));
            Assert.IsTrue(version1 != version2);
            Assert.IsTrue(version1 != version3);
            Assert.IsTrue(version2 == new XMLVersion(1,5));
            Assert.IsTrue(version2 != version1);
            Assert.IsTrue(version2 != version3);
            Assert.IsTrue(version3 == new XMLVersion(2,2));
            Assert.IsTrue(version3 != version1);
            Assert.IsTrue(version3 != version2);
        }
    }
}