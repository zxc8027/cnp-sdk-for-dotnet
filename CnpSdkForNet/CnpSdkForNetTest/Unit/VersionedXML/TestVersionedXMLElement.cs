/*
 * Zachary Cook
 *
 * Tests the VersionedXMLElement class.
 */

using System;
using System.Collections.Generic;
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
            Assert.AreEqual(xmlObject.Serialize(new XMLVersion()),"<TestXMLElement testAttribute1=\"1\" testAttribute2=\"Test 1\"><testElement1>2</testElement1><testElement2>Test 2</testElement2></TestXMLElement>");
            Assert.AreEqual(xmlObject.Serialize(new XMLVersion(),"CustomName"),"<CustomName testAttribute1=\"1\" testAttribute2=\"Test 1\"><testElement1>2</testElement1><testElement2>Test 2</testElement2></CustomName>");
            
            // Add an additional attribute and assert it is generated correctly.
            xmlObject.SetAdditionalAttribute("testAttribute3", "test");
            Assert.AreEqual(xmlObject.Serialize(new XMLVersion()),"<TestXMLElement testAttribute1=\"1\" testAttribute2=\"Test 1\" testAttribute3=\"test\"><testElement1>2</testElement1><testElement2>Test 2</testElement2></TestXMLElement>");
            Assert.AreEqual(xmlObject.Serialize(new XMLVersion(),"CustomName"),"<CustomName testAttribute1=\"1\" testAttribute2=\"Test 1\" testAttribute3=\"test\"><testElement1>2</testElement1><testElement2>Test 2</testElement2></CustomName>");

            // Add an additional element and assert it is generated correctly.
            xmlObject.AddAdditionalElement("<testElement3>Test 3</testElement3>");
            Assert.AreEqual(xmlObject.Serialize(new XMLVersion()),"<TestXMLElement testAttribute1=\"1\" testAttribute2=\"Test 1\" testAttribute3=\"test\"><testElement1>2</testElement1><testElement2>Test 2</testElement2><testElement3>Test 3</testElement3></TestXMLElement>");
            Assert.AreEqual(xmlObject.Serialize(new XMLVersion(),"CustomName"),"<CustomName testAttribute1=\"1\" testAttribute2=\"Test 1\" testAttribute3=\"test\"><testElement1>2</testElement1><testElement2>Test 2</testElement2><testElement3>Test 3</testElement3></CustomName>");
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
            Assert.AreEqual(xmlObject.Serialize(new XMLVersion()),"<TestXMLElement testAttribute1=\"1\" testAttribute2=\"Test 1&gt;\"><testElement1>2</testElement1><testElement2>Test &lt;2</testElement2></TestXMLElement>");
            Assert.AreEqual(xmlObject.Serialize(new XMLVersion(),"CustomName"),"<CustomName testAttribute1=\"1\" testAttribute2=\"Test 1&gt;\"><testElement1>2</testElement1><testElement2>Test &lt;2</testElement2></CustomName>");
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
            Assert.AreEqual(xmlObject.Serialize(new XMLVersion()),"<TestExtendedXMLElement testAttribute3=\"false\" testAttribute1=\"1\" testAttribute2=\"Test 1\"><testElement3>true</testElement3><testElement1>2</testElement1><testElement2>Test 2</testElement2></TestExtendedXMLElement>");
            Assert.AreEqual(xmlObject.Serialize(new XMLVersion(),"CustomName"),"<CustomName testAttribute3=\"false\" testAttribute1=\"1\" testAttribute2=\"Test 1\"><testElement3>true</testElement3><testElement1>2</testElement1><testElement2>Test 2</testElement2></CustomName>");
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
            Assert.AreEqual(xmlObject2.Serialize(new XMLVersion()),"<NestingXMLElement testAttribute=\"1\"><testElement1>2</testElement1><testElement2 testAttribute1=\"1\" testAttribute2=\"Test 1\"><testElement1>2</testElement1><testElement2>Test 2</testElement2></testElement2></NestingXMLElement>");
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
            Assert.AreEqual(xmlObject1.Serialize(new XMLVersion()),"<NullableXMLElement testAttribute=\"1\"></NullableXMLElement>");
            Assert.AreEqual(xmlObject2.Serialize(new XMLVersion()),"<NullableXMLElement><testElement>2</testElement></NullableXMLElement>");
            Assert.AreEqual(xmlObject3.Serialize(new XMLVersion()),"<NullableXMLElement testAttribute=\"1\"><testElement>2</testElement></NullableXMLElement>");
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
                xmlObject1.Serialize(new XMLVersion());
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
                xmlObject2.Serialize(new XMLVersion());
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
        
        /*
         * Test class for TestBasicVersion.
         */
        [XMLElement(Name = "TestXMLElement1",FirstVersion="1.0",RemovedVersion="1.5")]
        [XMLElement(Name = "TestXMLElement2",FirstVersion="1.4",RemovedVersion="2.0")]
        [XMLElement(Name = "TestXMLElement3",FirstVersion="2.0")]
        public class BasicVersion : VersionedXMLElement
        {
            [XMLAttribute(Name = "testAttribute1",RemovedVersion="1.8")]
            public int TestAttribute1 { get; set; }
            
            [XMLAttribute(Name = "testAttribute2A",FirstVersion = "1.2",RemovedVersion = "1.7")]
            [XMLAttribute(Name = "testAttribute2B",FirstVersion = "1.6")]
            public string TestAttribute2 { get; set; }
            
            [XMLElement(Name = "testElement1",FirstVersion="1.8")]
            public int TestElement1 { get; set; }
            
            [XMLElement(Name = "testAttribute2A",FirstVersion = "2.2",RemovedVersion = "2.7")]
            [XMLElement(Name = "testAttribute2B",FirstVersion = "2.5")]
            public string TestElement2 { get; set; }
        }
        
        /*
         * Tests the base case with versions.
         */
        [Test]
        public void TestBasicVersion()
        {
            // Create the object.
            var xmlObject = new BasicVersion();
            xmlObject.TestAttribute1 = 1;
            xmlObject.TestAttribute2 = "Test 1";
            xmlObject.TestElement1 = 2;
            xmlObject.TestElement2 = "Test 2";
            
            // Assert the element is generated correctly.
            Assert.AreEqual(xmlObject.Serialize(new XMLVersion(1,0)),"<TestXMLElement1 testAttribute1=\"1\"></TestXMLElement1>");
            Assert.AreEqual(xmlObject.Serialize(new XMLVersion(1,8)),"<TestXMLElement2 testAttribute2B=\"Test 1\"><testElement1>2</testElement1></TestXMLElement2>");
            Assert.AreEqual(xmlObject.Serialize(new XMLVersion(2,0)),"<TestXMLElement3 testAttribute2B=\"Test 1\"><testElement1>2</testElement1></TestXMLElement3>");
            
            // Assert that overlapped versions throw errors.
            try
            {
                xmlObject.Serialize(new XMLVersion(1, 4));
                Assert.Fail("Exception not thrown.");
            }
            catch (OverlappingVersionsException e)
            {
                Assert.IsTrue(e.Message.Contains("1.4"),"Version is not included:\n" + e.Message);
                Assert.IsTrue(e.Message.Contains("BasicVersion"),"Element name is not included:\n" + e.Message);
            }
            try
            {
                xmlObject.Serialize(new XMLVersion(1, 6));
                Assert.Fail("Exception not thrown.");
            }
            catch (OverlappingVersionsException e)
            {
                Assert.IsTrue(e.Message.Contains("1.6"),"Version is not included:\n" + e.Message);
                Assert.IsTrue(e.Message.Contains("TestAttribute2"),"Attribute name is not included:\n" + e.Message);
            }
            try
            {
                xmlObject.Serialize(new XMLVersion(2, 6));
                Assert.Fail("Exception not thrown.");
            }
            catch (OverlappingVersionsException e)
            {
                Assert.IsTrue(e.Message.Contains("2.6"),"Version is not included:\n" + e.Message);
                Assert.IsTrue(e.Message.Contains("TestElement2"),"Child element name is not included:\n" + e.Message);
            }
            
            // Assert that no valid version throws error.
            try
            {
                xmlObject.Serialize(new XMLVersion(-1, 0));
                Assert.Fail("Exception not thrown.");
            }
            catch (InvalidVersionException e)
            {
                Assert.IsTrue(e.Message.Contains("-1.0"),"Version is not included:\n" + e.Message);
                Assert.IsTrue(e.Message.Contains("BasicVersion"),"Element name is not included:\n" + e.Message);
            }
        }
        
        /*
        * Test class for TestPreSerialize.
        */
        [XMLElement(Name = "TestXMLElement")]
        public class PreSerializeElement : VersionedXMLElement
        {
            [XMLAttribute(Name = "testAttribute")]
            public XMLVersion TestAttribute { get; set; }
            
            [XMLElement(Name = "testElement")]
            public XMLVersion TestElement { get; set; }
            
            /*
             * Invoked before serializing the object to finalize
             * setting of elements.
             */
            public override void PreSerialize(XMLVersion version)
            {
                this.TestAttribute = version;
                this.TestElement = version;
            }
        }
        
        /*
         * Tests the PreSerialize method.
         */
        [Test]
        public void TestPreSerialize()
        {
            // Create the object.
            var xmlObject = new PreSerializeElement();
            
            // Assert the element is generated correctly.
            Assert.AreEqual(xmlObject.Serialize(new XMLVersion(1,2)),"<TestXMLElement testAttribute=\"1.2\"><testElement>1.2</testElement></TestXMLElement>");
            Assert.AreEqual(xmlObject.Serialize(new XMLVersion(1,2),"CustomName"),"<CustomName testAttribute=\"1.2\"><testElement>1.2</testElement></CustomName>");
        }
        
        /*
        * Test class for TestGetAdditionalElements.
        */
        [XMLElement(Name = "TestXMLElement")]
        public class AdditionalElementsElement : VersionedXMLElement
        {
            [XMLAttribute(Name = "testAttribute")]
            public string TestAttribute { get; set; }
            
            [XMLElement(Name = "testElement")]
            public string TestElement { get; set; }
            
            /*
             * Returns additional elements to add when serializing.
             * This method must handle all escaping of special characters.
             */
            public override List<string> GetAdditionalElements(XMLVersion version)
            {
                return new List<string>
                {
                    "<testElement>Test 3</testElement>",
                    "<testElement>Test 4</testElement>",
                };
            }
        }
        
        /*
         * Tests the GetAdditionalElements method.
         */
        [Test]
        public void TestGetAdditionalElements()
        {
            // Create the object.
            var xmlObject = new AdditionalElementsElement();
            xmlObject.TestAttribute = "Test 1";
            xmlObject.TestElement = "Test 2";

            // Assert the element is generated correctly.
            Assert.AreEqual(xmlObject.Serialize(new XMLVersion()), "<TestXMLElement testAttribute=\"Test 1\"><testElement>Test 2</testElement><testElement>Test 3</testElement><testElement>Test 4</testElement></TestXMLElement>");
        }
        
        /*
         * Test class for TestDateTime.
         */
        [XMLElement(Name = "TestXMLElement")]
        public class DateTimeElement : VersionedXMLElement
        {
            [XMLAttribute(Name = "testAttribute")]
            public DateTime TestAttribute { get; set; }

            [XMLElement(Name = "testElement")]
            public DateTime TestElement { get; set; }
        }
        
        /*
         * Tests elements with DateTime.
         */
        [Test]
        public void TestDateTime()
        {
            // Create the object.
            var xmlObject = new DateTimeElement();
            xmlObject.TestAttribute = new DateTime(2020,4,23);
            xmlObject.TestElement = new DateTime(2020,11,4);;
            
            // Assert the element is generated correctly.
            Assert.AreEqual(xmlObject.Serialize(new XMLVersion()),"<TestXMLElement testAttribute=\"2020-04-23\"><testElement>2020-11-04</testElement></TestXMLElement>");
        }
        
        /*
         * Test class for TestList.
         */
        [XMLElement(Name = "TestXMLElement")]
        public class ListElement : VersionedXMLElement
        {
            [XMLElement(Name = "testElement")]
            public List<string> TestElement { get; set; } = new List<string>();
        }
        
        /*
         * Tests elements with Lists.
         */
        [Test]
        public void TestList()
        {
            // Create the object.
            var xmlObject = new ListElement();
            xmlObject.TestElement.Add("element1");
            xmlObject.TestElement.Add("element1");
            xmlObject.TestElement.Add("element2");
            
            // Assert the element is generated correctly.
            Assert.AreEqual(xmlObject.Serialize(new XMLVersion()),"<TestXMLElement><testElement>element1</testElement><testElement>element1</testElement><testElement>element2</testElement></TestXMLElement>");
        }
    }
}