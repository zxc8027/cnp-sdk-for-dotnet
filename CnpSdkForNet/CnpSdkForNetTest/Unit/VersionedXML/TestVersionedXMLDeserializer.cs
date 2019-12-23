/*
 * Zachary Cook
 *
 * Tests the VersionedXMLDeserializer class.
 */

using System.Collections.Generic;
using System.Linq;
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
            
            [XMLAttribute(Name = "switchingItem",RemovedVersion = "2.0")]
            [XMLElement(Name = "switchingItem",FirstVersion = "2.0")]
            public string switchingItem { get; set; }
        }
        
        /*
         * Tests the Deserialize method without custom names.
         */
        [Test]
        public void TestDeserializeWithoutCustomNames()
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
        public void TestDeserializeWithCustomNames()
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
        public void TestDeserializeNoAttributes()
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
        public void TestDeserializeNoElements()
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
            
            // Deserialize an object with attributes.
            xmlObject = VersionedXMLDeserializer.Deserialize<TestXMLElement>("<TestXMLElement TestAttribute1= \"1\" TestAttribute2 ='Test \"1\"' TestAttribute3= \"true\"><TestElement1>2</TestElement1> <TestElement3>false</TestElement3><TestElement4 TestAttribute3=\"true\"></TestElement4></TestXMLElement>", new XMLVersion());
            
            // Assert the element was deserialized correctly.
            Assert.AreEqual(xmlObject.TestAttribute1,1,"Attribute wasn't deserialized correctly.");
            Assert.AreEqual(xmlObject.TestAttribute2,"Test \"1\"","Attribute wasn't deserialized correctly.");
            Assert.AreEqual(xmlObject.TestAttribute3,true,"Attribute wasn't deserialized correctly.");
            Assert.AreEqual(xmlObject.TestElement1,2,"Element wasn't deserialized correctly.");
            Assert.AreEqual(xmlObject.TestElement2,null,"Element was initialized (should be null).");
            Assert.AreEqual(xmlObject.TestElement3,false,"Element wasn't deserialized correctly.");
            Assert.AreEqual(xmlObject.TestElement4.TestAttribute3,true,"Element wasn't deserialized correctly.");
        }
        
        /*
         * Tests the Deserialize method with child XML.
         */
        [Test]
        public void TestDeserializeChildXML()
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
        
        /*
         * Tests an item and switches from between attribute and element between versions.
         */
        [Test]
        public void TestDeserializeSwitchingType()
        {
            // Deserialize 2 objects.
            var xmlObject1 = VersionedXMLDeserializer.Deserialize<TestXMLElement>("<TestXMLElement switchingItem=\"Test\"></TestXMLElement>", new XMLVersion(1,0));
            var xmlObject2 = VersionedXMLDeserializer.Deserialize<TestXMLElement>("<TestXMLElement><switchingItem>Test</switchingItem></TestXMLElement>", new XMLVersion(2,0));
            
            // Assert the elements were deserialized correctly.
            Assert.AreEqual(xmlObject1.switchingItem,"Test","Item wasn't deserialized correctly.");
            Assert.AreEqual(xmlObject2.switchingItem,"Test","Item wasn't deserialized correctly.");
        }
        
        /*
         * XML element used for TestDeserializeUnknownElements.
         */
        [XMLElement(Name = "TestXMLElement")]
        public class TestXMLElementUnknownElements : VersionedXMLElement
        {
            public string TestAttribute { get; set; }
            
            /*
             * Parses elements that aren't defined by properties.
             */
            public override void ParseAdditionalElements(XMLVersion version,List<string> elements)
            {
                this.TestAttribute = string.Concat(elements.ToArray());
            }
        }
        
        /*
         * Tests deserializing with unknown elements.
         */
        [Test]
        public void TestDeserializeUnknownElements()
        {
            // Deserialize two objects.
            var xmlObject1 = VersionedXMLDeserializer.Deserialize<TestXMLElementUnknownElements>("<TestXMLElement></TestXMLElement>", new XMLVersion());
            var xmlObject2 = VersionedXMLDeserializer.Deserialize<TestXMLElementUnknownElements>("<TestXMLElement><Element1>Test1</Element1><Element2>Test1</Element2></TestXMLElement>", new XMLVersion());

            // Assert the attributes are set correctly.
            Assert.AreEqual(xmlObject1.TestAttribute,"");
            Assert.AreEqual(xmlObject2.TestAttribute,"<Element1>Test1</Element1><Element2>Test1</Element2>");
        }
        
        /*
         * XML element used for TestDeserializeToList.
         */
        [XMLElement(Name = "TestXMLElement")]
        public class TestXMLElementListElements : VersionedXMLElement
        {
            [XMLElement(Name = "testElement")]
            public List<string> TestElement { get; set; } = new List<string>();
        }
        
        /*
         * Tests deserializing to a list.
         */
        [Test]
        public void TestDeserializeToList()
        {
            // Deserialize two objects.
            var xmlObject1 = VersionedXMLDeserializer.Deserialize<TestXMLElementListElements>("<TestXMLElement><testElement>String1</testElement></TestXMLElement>", new XMLVersion());
            var xmlObject2 = VersionedXMLDeserializer.Deserialize<TestXMLElementListElements>("<TestXMLElement><testElement>String1</testElement><testElement>String2</testElement><testElement>String3</testElement></TestXMLElement>", new XMLVersion());
            
            // Assert the attributes are set correctly.
            Assert.AreEqual(xmlObject1.TestElement,new List<string>() { "String1" });
            Assert.AreEqual(xmlObject2.TestElement,new List<string>() { "String1","String2","String3" });
        }
        
        /*
         * Enums for TestDeserializeEnums.
         */
        public enum TestEnum
        {
            Value1,
            
            [XMLEnum(Name = "TestValue")]
            Value2,
            
            [XMLEnum(Name = "Value3",FirstVersion="0.0",RemovedVersion = "2.0")]
            Value3,
            
            [XMLEnum(Name = "Value3",FirstVersion = "2.0")]
            Value4,
            
            [XMLEnum(Name = "Value6",RemovedVersion = "3.0")]
            [XMLEnum(Name = "Value7",FirstVersion = "2.0")]
            Value5,
        }
        
        /*
         * Test class for TestDeserializeEnums.
         */
        [XMLElement(Name = "TestXMLElement")]
        public class XMLWithEnums : VersionedXMLElement
        {
            [XMLAttribute(Name = "testAttribute")]
            public TestEnum? TestAttribute { get; set; }
            
            [XMLElement(Name = "testElement")]
            public TestEnum? TestElement { get; set; }
        }
        
        /*
         * Tests deserializing enums.
         */
        [Test]
        public void TestDeserializeEnums()
        {
            // Deserialize two objects.
            var xmlObject1 = VersionedXMLDeserializer.Deserialize<XMLWithEnums>("<TestXMLElement testAttribute=\"Value1\"><testElement>Value1</testElement></TestXMLElement>", new XMLVersion());
            var xmlObject2 = VersionedXMLDeserializer.Deserialize<XMLWithEnums>("<TestXMLElement testAttribute=\"TestValue\"><testElement>TestValue</testElement></TestXMLElement>", new XMLVersion());
            var xmlObject3 = VersionedXMLDeserializer.Deserialize<XMLWithEnums>("<TestXMLElement testAttribute=\"Value3\"><testElement>Value3</testElement></TestXMLElement>", new XMLVersion());
            var xmlObject4 = VersionedXMLDeserializer.Deserialize<XMLWithEnums>("<TestXMLElement testAttribute=\"Value3\"><testElement>Value3</testElement></TestXMLElement>", new XMLVersion(3,0));
            var xmlObject5 = VersionedXMLDeserializer.Deserialize<XMLWithEnums>("<TestXMLElement testAttribute=\"Value6\"><testElement>Value6</testElement></TestXMLElement>", new XMLVersion());
            var xmlObject6 = VersionedXMLDeserializer.Deserialize<XMLWithEnums>("<TestXMLElement testAttribute=\"Value7\"><testElement>Value7</testElement></TestXMLElement>", new XMLVersion(3,0));

            // Assert the attributes are set correctly.
            Assert.AreEqual(xmlObject1.TestAttribute,TestEnum.Value1);
            Assert.AreEqual(xmlObject1.TestElement,TestEnum.Value1);
            Assert.AreEqual(xmlObject2.TestAttribute,TestEnum.Value2);
            Assert.AreEqual(xmlObject2.TestElement,TestEnum.Value2);
            Assert.AreEqual(xmlObject3.TestAttribute,TestEnum.Value3);
            Assert.AreEqual(xmlObject3.TestElement,TestEnum.Value3);
            Assert.AreEqual(xmlObject4.TestAttribute,TestEnum.Value4);
            Assert.AreEqual(xmlObject4.TestElement,TestEnum.Value4);
            Assert.AreEqual(xmlObject5.TestAttribute,TestEnum.Value5);
            Assert.AreEqual(xmlObject5.TestElement,TestEnum.Value5);
            Assert.AreEqual(xmlObject6.TestAttribute,TestEnum.Value5);
            Assert.AreEqual(xmlObject6.TestElement,TestEnum.Value5);
            
            //  Assert that exceptions are thrown for invalid versions.
            Assert.Catch<InvalidVersionException>(() =>
            {
                VersionedXMLDeserializer.Deserialize<XMLWithEnums>("<TestXMLElement testAttribute=\"Value6\"><testElement>Value3</testElement></TestXMLElement>",new XMLVersion(-1, 0));
            });
            Assert.Catch<InvalidVersionException>(() =>
            {
                VersionedXMLDeserializer.Deserialize<XMLWithEnums>("<TestXMLElement testAttribute=\"Value3\"><testElement>Value6</testElement></TestXMLElement>", new XMLVersion(-1,0));
            });

            // Assert that exceptions are thrown for unusable enums.
            Assert.Catch<InvalidVersionException>(() =>
            {
                VersionedXMLDeserializer.Deserialize<XMLWithEnums>("<TestXMLElement testAttribute=\"Value5\"><testElement>Value5</testElement></TestXMLElement>", new XMLVersion());
            });
        }
    }
}