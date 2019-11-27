/*
 * Zachary Cook
 *
 * Tests the VersionedXMLDeserializer class.
 */

using Cnp.Sdk.VersionedXML;
using NUnit.Framework;

namespace Cnp.Sdk.Test.Unit.VersionedXML
{
    public class TestVersionedXMLDeserializer
    {
        /*
         * XML element used for tests.
         */
        [XMLElement(Name = "TestXMLElement")]
        public class TestXMLElement : VersionedXMLElement
        {
            [XMLAttribute(Name = "testAttribute1",RemovedVersion = "1.0")]
            [XMLAttribute(Name = "customAttribute1",FirstVersion = "1.0")]
            public int TestAttribute1 { get; set; }
            
            [XMLAttribute(Name = "testAttribute2",RemovedVersion = "1.0")]
            [XMLAttribute(Name = "customAttribute2",FirstVersion = "1.0")]
            public string TestAttribute2 { get; set; }
            
            [XMLAttribute(Name = "testAttribute3",RemovedVersion = "1.0")]
            [XMLAttribute(Name = "customAttribute3",FirstVersion = "1.0")]
            public bool TestAttribute3 { get; set; }
            
            [XMLElement(Name = "testElement1",RemovedVersion = "1.0")]
            [XMLElement(Name = "customElement1",FirstVersion = "1.0")]
            public int TestElement1 { get; set; }
            
            [XMLElement(Name = "testElement2",RemovedVersion = "1.0")]
            [XMLElement(Name = "customElement3",FirstVersion = "1.0")]
            public string TestElement2 { get; set; }
            
            [XMLElement(Name = "testElement3",RemovedVersion = "1.0")]
            [XMLElement(Name = "customElement2",FirstVersion = "1.0")]
            public bool TestElement3 { get; set; }
            
            [XMLElement(Name = "testElement4",RemovedVersion = "1.0")]
            [XMLElement(Name = "customElement4",FirstVersion = "1.0")]
            public TestXMLElement TestElement4 { get; set; }
            
            [XMLElement(Name = "switchingAttribute",RemovedVersion = "1.0")]
            [XMLElement(Name = "switchingElement",FirstVersion = "1.0")]
            public string switchingItem { get; set; }
        }
        
        /*
         * Tests the Deserialize method without custom names.
         */
        [Test]
        public void DeserializeWithoutCustomNames()
        {
            // Deserialize an object.
            var xmlObject = VersionedXMLDeserializer.Deserialize<TestXMLElement>("<TestXMLElement TestAttribute1= \"1\" TestAttribute2 ='Test \"1\"' TestAttribute3= \"true\"><TestElement1>2</TestElement1>  <TestElement2>Test 2</TestElement2><TestElement3>false</TestElement3></TestXMLElement>  ", new XMLVersion());
            
            // Assert the element was deserialized correctly.
            Assert.AreEqual(xmlObject.TestAttribute1,1,"Attribute wasn't deserialized correctly.");
            Assert.AreEqual(xmlObject.TestAttribute2,"Test \"1\"","Attribute wasn't deserialized correctly.");
            Assert.AreEqual(xmlObject.TestAttribute3,true,"Attribute wasn't deserialized correctly.");
            Assert.AreEqual(xmlObject.TestElement1,2,"Element wasn't deserialized correctly.");
            Assert.AreEqual(xmlObject.TestElement2,"Test 2","Element wasn't deserialized correctly.");
            Assert.AreEqual(xmlObject.TestElement3,false,"Element wasn't deserialized correctly.");
        }
        
        /*
         * Tests the Deserialize method with custom names.
         */
        [Test]
        public void DeserializeWithCustomNames()
        {
            // Deserialize an object.
            var xmlObject = VersionedXMLDeserializer.Deserialize<TestXMLElement>("<TestXMLElement CustomAttribute1= \"1\" CustomAttribute2 ='Test \"1\"' CustomAttribute3= \"true\"><CustomElement1>2</CustomElement1>  <CustomElement2>false</CustomElement2><CustomElement3>Test 2</CustomElement3></TestXMLElement>  ", new XMLVersion(1,0));
            
            // Assert the element was deserialized correctly.
            Assert.AreEqual(xmlObject.TestAttribute1,1,"Attribute wasn't deserialized correctly.");
            Assert.AreEqual(xmlObject.TestAttribute2,"Test \"1\"","Attribute wasn't deserialized correctly.");
            Assert.AreEqual(xmlObject.TestAttribute3,true,"Attribute wasn't deserialized correctly.");
            Assert.AreEqual(xmlObject.TestElement1,2,"Element wasn't deserialized correctly.");
            Assert.AreEqual(xmlObject.TestElement2,"Test 2","Element wasn't deserialized correctly.");
            Assert.AreEqual(xmlObject.TestElement3,false,"Element wasn't deserialized correctly.");
        }
        
        /*
         * Tests the Deserialize method with no attributes.
         */
        [Test]
        public void DeserializeNoAttributes()
        {
            // Deserialize an object.
            var xmlObject = VersionedXMLDeserializer.Deserialize<TestXMLElement>("<TestXMLElement><CustomElement1>2</CustomElement1>  <CustomElement2>false</CustomElement2><CustomElement3>Test 2</CustomElement3></TestXMLElement>  ", new XMLVersion(1,0));
            
            // Assert the element was deserialized correctly.
            Assert.AreEqual(xmlObject.TestElement1,2,"Element wasn't deserialized correctly.");
            Assert.AreEqual(xmlObject.TestElement2,"Test 2","Element wasn't deserialized correctly.");
            Assert.AreEqual(xmlObject.TestElement3,false,"Element wasn't deserialized correctly.");
        }
        
        /*
         * Tests the Deserialize method with no elements.
         */
        [Test]
        public void DeserializeNoElements()
        {
            // Deserialize an object.
            var xmlObject = VersionedXMLDeserializer.Deserialize<TestXMLElement>("<TestXMLElement TestAttribute1= \"1\" TestAttribute2 ='Test \"1\"' TestAttribute3= \"true\"/>  ", new XMLVersion());
            
            // Assert the element was deserialized correctly.
            Assert.AreEqual(xmlObject.TestAttribute1,1,"Attribute wasn't deserialized correctly.");
            Assert.AreEqual(xmlObject.TestAttribute2,"Test \"1\"","Attribute wasn't deserialized correctly.");
            Assert.AreEqual(xmlObject.TestAttribute3,true,"Attribute wasn't deserialized correctly.");
        }
        
        /*
         * Tests the Deserialize method with null child elements.
         */
        [Test]
        public void DeserializeNullChildElements()
        {
            // Deserialize an object.
            var xmlObject = VersionedXMLDeserializer.Deserialize<TestXMLElement>("<TestXMLElement TestAttribute1= \"1\" TestAttribute2 ='Test \"1\"' TestAttribute3= \"true\"><TestElement1>2</TestElement1> <TestElement3>false</TestElement3><TestElement4/></TestXMLElement>", new XMLVersion());
            
            // Assert the element was deserialized correctly.
            Assert.AreEqual(xmlObject.TestAttribute1,1,"Attribute wasn't deserialized correctly.");
            Assert.AreEqual(xmlObject.TestAttribute2,"Test \"1\"","Attribute wasn't deserialized correctly.");
            Assert.AreEqual(xmlObject.TestAttribute3,true,"Attribute wasn't deserialized correctly.");
            Assert.AreEqual(xmlObject.TestElement1,2,"Element wasn't deserialized correctly.");
            Assert.AreEqual(xmlObject.TestElement2,null,"Element was initialized (should be null).");
            Assert.AreEqual(xmlObject.TestElement3,false,"Element wasn't deserialized correctly.");
            Assert.AreEqual(xmlObject.TestElement4,null,"Element was initialized (should be null).");
        }
        
        /*
         * Tests the Deserialize method with child XML.
         */
        [Test]
        public void DeserializeChildXML()
        {
            // Deserialize an object.
            var xmlObject = VersionedXMLDeserializer.Deserialize<TestXMLElement>("<TestXMLElement><TestElement4 TestAttribute1= \"1\" TestAttribute2 ='Test \"1\"' TestAttribute3= \"true\"><TestElement1>2</TestElement1> <TestElement3>false</TestElement3></TestElement4></TestXMLElement>", new XMLVersion());
            
            // Assert the element was deserialized correctly.
            Assert.AreEqual(xmlObject.TestElement4.TestAttribute1,1,"Attribute wasn't deserialized correctly.");
            Assert.AreEqual(xmlObject.TestElement4.TestAttribute2,"Test \"1\"","Attribute wasn't deserialized correctly.");
            Assert.AreEqual(xmlObject.TestElement4.TestAttribute3,true,"Attribute wasn't deserialized correctly.");
            Assert.AreEqual(xmlObject.TestElement4.TestElement1,2,"Element wasn't deserialized correctly.");
            Assert.AreEqual(xmlObject.TestElement4.TestElement3,false,"Element wasn't deserialized correctly.");
        }
        
        // TODO: Recursive element
        // TODO: Attribute one version, element another
    }
}