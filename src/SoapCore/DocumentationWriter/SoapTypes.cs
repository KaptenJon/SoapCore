using System.Collections.Generic;
using System.Xml.Serialization;

namespace SoapCore.DocumentationWriter
{
	/// <summary>
	/// Represents a SOAP service definition in WSDL format.
	/// </summary>
	public partial class SoapDefinition
	{
		/// <summary>
		/// Represents the type definitions section of a WSDL document.
		/// </summary>
		public class SoapTypes
		{
			/// <summary>
			/// Gets or sets the list of XML schema definitions.
			/// </summary>
			[XmlElement(ElementName = "schema", Namespace = "http://www.w3.org/2001/XMLSchema")]
			public List<SoapTypeSchema> Schema { get; set; }

			/// <summary>
			/// Represents an XML schema definition.
			/// </summary>
			public class SoapTypeSchema
			{
				/// <summary>
				/// Gets or sets the element form default attribute.
				/// </summary>
				[XmlAttribute(AttributeName = "elementFormDefault")]
				public string ElementFormDefault { get; set; }

				/// <summary>
				/// Gets or sets the target namespace for the schema.
				/// </summary>
				[XmlAttribute(AttributeName = "targetNamespace")]
				public string TargetNamespace { get; set; }

				/// <summary>
				/// Gets or sets the list of schema imports.
				/// </summary>
				[XmlElement(ElementName = "import")]
				public List<SoapTypeSchemaImport> Imports { get; set; }

				/// <summary>
				/// Gets or sets the list of schema elements.
				/// </summary>
				[XmlElement(ElementName = "element")]
				public List<SoapTypeSchemaElement> Elements { get; set; }

				/// <summary>
				/// Gets or sets the list of complex type definitions.
				/// </summary>
				[XmlElement(ElementName = "complexType")]
				public List<ComplexType> ComplexTypes { get; set; }

				/// <summary>
				/// Gets or sets the list of simple type definitions.
				/// </summary>
				[XmlElement(ElementName = "simpleType")]
				public List<SimpleType> SimpleTypes { get; set; }

				/// <summary>
				/// Represents a schema import statement.
				/// </summary>
				public class SoapTypeSchemaImport
				{
					/// <summary>
					/// Gets or sets the namespace being imported.
					/// </summary>
					[XmlAttribute(AttributeName = "namespace")]
					public string Namespace { get; set; }
				}

				/// <summary>
				/// Represents a schema element definition.
				/// </summary>
				public class SoapTypeSchemaElement
				{
					/// <summary>
					/// Gets or sets the complex type for this element.
					/// </summary>
					[XmlElement(ElementName = "complexType")]
					public ComplexType ComplexElementType { get; set; }

					/// <summary>
					/// Gets or sets the name of the element.
					/// </summary>
					[XmlAttribute(AttributeName = "name")]
					public string Name { get; set; }
				}

				/// <summary>
				/// Represents a simple type definition.
				/// </summary>
				public class SimpleType
				{
					/// <summary>
					/// Gets or sets the name of the simple type.
					/// </summary>
					[XmlAttribute(AttributeName = "name")]
					public string Name { get; set; }

					/// <summary>
					/// Gets or sets the value restriction for the simple type.
					/// </summary>
					[XmlElement(ElementName = "restriction")]
					public ValueRestriction Restriction { get; set; }

					/// <summary>
					/// Represents value restrictions for a simple type.
					/// </summary>
					public class ValueRestriction
					{
						/// <summary>
						/// Gets or sets the list of enumeration values.
						/// </summary>
						[XmlElement(ElementName = "enumeration")]
						public List<EnumValue> EnumerationValue { get; set; }

						/// <summary>
						/// Represents an enumeration value.
						/// </summary>
						public class EnumValue
						{
							/// <summary>
							/// Gets or sets the enumeration value.
							/// </summary>
							[XmlAttribute(AttributeName = "value")]
							public string Value { get; set; }
						}
					}
				}

				/// <summary>
				/// Represents a complex type definition.
				/// </summary>
				public class ComplexType
				{
					/// <summary>
					/// Gets or sets the name of the complex type.
					/// </summary>
					[XmlAttribute(AttributeName = "name")]
					public string Name { get; set; }

					/// <summary>
					/// Gets or sets the sequence of elements in the complex type.
					/// </summary>
					[XmlElement(ElementName = "sequence")]
					public Sequence TypeInformation { get; set; }

					/// <summary>
					/// Represents a sequence of elements.
					/// </summary>
					public class Sequence
					{
						/// <summary>
						/// Gets or sets the list of elements in the sequence.
						/// </summary>
						[XmlElement(ElementName = "element")]
						public List<SequenceElement> Element { get; set; }

						/// <summary>
						/// Represents an element in a sequence.
						/// </summary>
						public class SequenceElement
						{
							/// <summary>
							/// Gets or sets the name of the element.
							/// </summary>
							[XmlAttribute(AttributeName = "name")]
							public string Name { get; set; }

							/// <summary>
							/// Gets or sets the minimum number of occurrences.
							/// </summary>
							[XmlAttribute(AttributeName = "minOccurs")]
							public string MinimumOccurences { get; set; }

							/// <summary>
							/// Gets or sets the maximum number of occurrences.
							/// </summary>
							[XmlAttribute(AttributeName = "maxOccurs")]
							public string MaximumOccurences { get; set; }

							/// <summary>
							/// Gets or sets the type of the element.
							/// </summary>
							[XmlAttribute(AttributeName = "type")]
							public string Type { get; set; }

							/// <summary>
							/// Gets or sets the reference to another element.
							/// </summary>
							[XmlAttribute(AttributeName = "ref")]
							public string Ref { get; set; }

							/// <summary>
							/// Gets or sets a value indicating whether the element is nullable.
							/// </summary>
							[XmlAttribute(AttributeName = "nillable")]
							public bool Nullable { get; set; } = false;

							/// <summary>
							/// Gets or sets a value indicating whether the element is abstract.
							/// </summary>
							[XmlAttribute(AttributeName = "abstract")]
							public bool Abstract { get; set; } = false;
						}
					}
				}
			}
		}
	}
}
