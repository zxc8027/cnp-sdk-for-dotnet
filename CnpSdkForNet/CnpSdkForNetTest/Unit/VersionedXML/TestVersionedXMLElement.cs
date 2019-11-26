/*
 * Zachary Cook
 *
 * Tests the VersionedXMLElement class.
 */

using System;
using Cnp.Sdk.VersionedXML;
using NUnit.Framework;

namespace Cnp.Sdk.Test.Unit.VersionedXML
{
    [TestFixture]
    public class TestVersionedXMLElement
    {
        /*
         * Test class for TestBasicNoVersion.
         */
        [XMLElement(Name = "TestXMLElement")]
        public class BasicNoVersion : VersionedXMLElement
        {
            [XMLAttribute(Name = "testAttribute1")]
            public int TestAttribute1 { get; set; }
            
            [XMLAttribute(Name = "testAttribute2")]
            public string TestAttribute2 { get; set; }
            
            [XMLElement(Name = "testElement1")]
            public int TestElement1 { get; set; }
            
            [XMLElement(Name = "testElement2")]
            public string TestElement2 { get; set; }
        }
        
        /*
         * Tests the base case without versions.
         */
        [Test]
        public void TestBasicNoVersion()
        {
            // Create the object.
            var xmlObject = new BasicNoVersion();
            xmlObject.TestAttribute1 = 1;
            xmlObject.TestAttribute2 = "Test 1";
            xmlObject.TestElement1 = 2;
            xmlObject.TestElement2 = "Test 2";
            
            // Assert the element is generated correctly.
            Assert.AreEqual(xmlObject.Serialize(0),"<TestXMLElement testAttribute1=\"1\" testAttribute2=\"Test 1\"><testElement1>2</testElement1><testElement2>Test 2</testElement2></TestXMLElement>");
            Assert.AreEqual(xmlObject.Serialize(0,"CustomName"),"<CustomName testAttribute1=\"1\" testAttribute2=\"Test 1\"><testElement1>2</testElement1><testElement2>Test 2</testElement2></CustomName>");
        }
        
        /*
         * Tests the base case without versions with xml characters.
         */
        [Test]
        public void TestBasicNoVersionXMLCharacters()
        {
            // Create the object.
            var xmlObject = new BasicNoVersion();
            xmlObject.TestAttribute1 = 1;
            xmlObject.TestAttribute2 = "Test 1>";
            xmlObject.TestElement1 = 2;
            xmlObject.TestElement2 = "Test <2";
            
            // Assert the element is generated correctly.
            Assert.AreEqual(xmlObject.Serialize(0),"<TestXMLElement testAttribute1=\"1\" testAttribute2=\"Test 1&gt;\"><testElement1>2</testElement1><testElement2>Test &lt;2</testElement2></TestXMLElement>");
            Assert.AreEqual(xmlObject.Serialize(0,"CustomName"),"<CustomName testAttribute1=\"1\" testAttribute2=\"Test 1&gt;\"><testElement1>2</testElement1><testElement2>Test &lt;2</testElement2></CustomName>");
        }
        
        /*
         * Test class for TestInheritance.
         */
        [XMLElement(Name = "TestExtendedXMLElement")]
        public class ExtendedNoVersion : BasicNoVersion
        {
            [XMLAttribute(Name = "testAttribute3")]
            public bool TestAttribute3 { get; set; }
            
            [XMLElement(Name = "testElement3")]
            public bool TestElement3 { get; set; }
        }
        
        /*
         * Tests extending another element.
         */
        [Test]
        public void TestExtending()
        {
            // Create the object.
            var xmlObject = new ExtendedNoVersion();
            xmlObject.TestAttribute1 = 1;
            xmlObject.TestAttribute2 = "Test 1";
            xmlObject.TestAttribute3 = false;
            xmlObject.TestElement1 = 2;
            xmlObject.TestElement2 = "Test 2";
            xmlObject.TestElement3 = true;
            
            // Assert the element is generated correctly.
            Assert.AreEqual(xmlObject.Serialize(0),"<TestExtendedXMLElement testAttribute3=\"false\" testAttribute1=\"1\" testAttribute2=\"Test 1\"><testElement3>true</testElement3><testElement1>2</testElement1><testElement2>Test 2</testElement2></TestExtendedXMLElement>");
            Assert.AreEqual(xmlObject.Serialize(0,"CustomName"),"<CustomName testAttribute3=\"false\" testAttribute1=\"1\" testAttribute2=\"Test 1\"><testElement3>true</testElement3><testElement1>2</testElement1><testElement2>Test 2</testElement2></CustomName>");
        }
        
        /*
         * Test class for TestNesting.
         */
        [XMLElement(Name = "NestingXMLElement")]
        public class NestingElement : VersionedXMLElement
        {
            [XMLAttribute(Name = "testAttribute")]
            public int TestAttribute { get; set; }
            
            [XMLElement(Name = "testElement1")]
            public int TestElement1 { get; set; }
            
            [XMLElement(Name = "testElement2")]
            public BasicNoVersion TestElement2 { get; set; }
        }
        
        /*
         * Tests nesting elements.
         */
        [Test]
        public void TestNesting()
        {
            // Create the objects.
            var xmlObject1 = new BasicNoVersion();
            xmlObject1.TestAttribute1 = 1;
            xmlObject1.TestAttribute2 = "Test 1";
            xmlObject1.TestElement1 = 2;
            xmlObject1.TestElement2 = "Test 2";
            
            var xmlObject2 = new NestingElement();
            xmlObject2.TestAttribute = 1;
            xmlObject2.TestElement1 = 2;
            xmlObject2.TestElement2 = xmlObject1;
            
            // Assert the element is generated correctly.
            Assert.AreEqual(xmlObject2.Serialize(0),"<NestingXMLElement testAttribute=\"1\"><testElement1>2</testElement1><testElement2 testAttribute1=\"1\" testAttribute2=\"Test 1\"><testElement1>2</testElement1><testElement2>Test 2</testElement2></testElement2></NestingXMLElement>");
        }
        
        /*
         * Test class for TestNullable.
         */
        [XMLElement(Name = "NullableXMLElement")]
        public class NullableElement : VersionedXMLElement
        {
            [XMLAttribute(Name = "testAttribute")]
            public int? TestAttribute { get; set; }
            
            [XMLElement(Name = "testElement")]
            public int? TestElement { get; set; }
        }
        
        /*
         * Tests elements and attributes being nullable.
         */
        [Test]
        public void TestNullable()
        {
            // Create the objects.
            var xmlObject1 = new NullableElement();
            xmlObject1.TestAttribute = 1;
            
            var xmlObject2 = new NullableElement();
            xmlObject2.TestElement = 2;
            
            var xmlObject3 = new NullableElement();
            xmlObject3.TestAttribute = 1;
            xmlObject3.TestElement = 2;
            
            // Assert the element is generated correctly.
            Assert.AreEqual(xmlObject1.Serialize(0),"<NullableXMLElement testAttribute=\"1\"></NullableXMLElement>");
            Assert.AreEqual(xmlObject2.Serialize(0),"<NullableXMLElement><testElement>2</testElement></NullableXMLElement>");
            Assert.AreEqual(xmlObject3.Serialize(0),"<NullableXMLElement testAttribute=\"1\"><testElement>2</testElement></NullableXMLElement>");
        }
        
        /*
         * Test class for TestMissingGetter.
         */
        [XMLElement(Name = "TestMissingGetter")]
        public class MissingGetter1 : VersionedXMLElement
        {
            [XMLAttribute(Name = "testAttribute")]
            public int TestAttribute;
        }
        
        [XMLElement(Name = "TestMissingGetter")]
        public class MissingGetter2 : VersionedXMLElement
        {
            [XMLElement(Name = "testElement")]
            public int TestElement;
        }
        
        /*
         * Tests a getter being missing.
         */
        [Test]
        public void TestMissingGetter()
        {
            // Create the objects.
            var xmlObject1 = new MissingGetter1();
            xmlObject1.TestAttribute = 1;
            
            var xmlObject2 = new MissingGetter2();
            xmlObject2.TestElement = 1;
            
            // Assert a custom exception is thrown for an attribute missing a getter.
            try
            {
                xmlObject1.Serialize(0);
                Assert.Fail("No exception thrown.");
            }
            catch (MissingGetterException e)
            {
                Assert.IsTrue(e.Message.Contains("TestAttribute"),"TestAttribute is not in the exception message: " + e.Message);
            }
            catch (Exception e)
            {
                Assert.Fail("Incorrect exception thrown: " + e);
            }
            
            // Assert a custom exception is thrown for an element missing a getter.
            try
            {
                xmlObject2.Serialize(0);
                Assert.Fail("No exception thrown.");
            }
            catch (MissingGetterException e)
            {
                Assert.IsTrue(e.Message.Contains("TestElement"),"TestElement is not in the exception message: " + e.Message);
            }
            catch (Exception e)
            {
                Assert.Fail("Incorrect exception thrown: " + e);
            }
        }
    }
}