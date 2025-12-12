using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace SoapCore.DocumentationWriter
{
	/// <summary>
	/// Represents a SOAP service definition in WSDL format.
	/// </summary>
	public partial class SoapDefinition
	{
		/// <summary>
		/// Represents a SOAP service definition in WSDL.
		/// </summary>
		public class SoapService
		{
			/// <summary>
			/// Gets or sets the name of the service.
			/// </summary>
			[XmlAttribute(AttributeName = "name")]
			public string Name { get; set; }

			/// <summary>
			/// Gets or sets the list of service ports.
			/// </summary>
			[XmlElement(ElementName = "port")]
			public List<SoapServicePort> Ports { get; set; }

			/// <summary>
			/// Represents a service port definition.
			/// </summary>
			public class SoapServicePort : IElementWithSpecialTransforms
			{
				/// <summary>
				/// Gets or sets the name of the port.
				/// </summary>
				[XmlAttribute(AttributeName = "name")]
				public string Name { get; set; }

				/// <summary>
				/// Gets or sets the binding reference for this port.
				/// </summary>
				[XmlAttribute(AttributeName = "binding")]
				public string Binding { get; set; }

				/// <summary>
				/// Gets or sets the address information for this port.
				/// </summary>
				[XmlElement(ElementName = "address")]
				public SoapServicePortAddress Address { get; set; }

				/// <summary>
				/// Deserializes XML elements to populate port properties.
				/// </summary>
				/// <param name="element">The XML element to deserialize.</param>
				public void DeserializeElements(XmlElement element)
				{
					if (element.Name.EndsWith("address"))
					{
						Address = new SoapServicePortAddress
						{
							Location = element.GetAttribute("location"),
							Namespace = element.NamespaceURI
						};
					}
				}

				/// <summary>
				/// Represents the address configuration for a service port.
				/// </summary>
				public class SoapServicePortAddress
				{
					/// <summary>
					/// Gets or sets the location URL for the service port.
					/// </summary>
					[XmlAttribute(AttributeName = "location")]
					public string Location { get; set; }

					/// <summary>
					/// Gets or sets the namespace for the service port address.
					/// </summary>
					public string Namespace { get; set; }
				}
			}
		}
	}
}
