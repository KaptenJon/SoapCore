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
		/// Represents a WSDL operation definition.
		/// </summary>
		public class WsdlOperation : IElementWithSpecialTransforms
		{
			private string _namespace;

			/// <summary>
			/// Gets or sets the name of the operation.
			/// </summary>
			[XmlAttribute(AttributeName = "name")]
			public string Name { get; set; }

			/// <summary>
			/// Gets or sets the input message for the operation.
			/// </summary>
			[XmlElement(ElementName = "input")]
			public OperationItem Input { get; set; }

			/// <summary>
			/// Gets or sets the output message for the operation.
			/// </summary>
			[XmlElement(ElementName = "output")]
			public OperationItem Output { get; set; }

			/// <summary>
			/// Gets or sets the fault message for the operation.
			/// </summary>
			[XmlElement(ElementName = "fault")]
			public OperationItem Fault { get; set; }

			/// <summary>
			/// Gets or sets the SOAP operation details.
			/// </summary>
			[XmlElement(ElementName = "operation")]
			public SoapOperation Operation { get; set; }

			/// <summary>
			/// Deserializes XML elements to populate operation properties.
			/// </summary>
			/// <param name="element">The XML element to deserialize.</param>
			public void DeserializeElements(XmlElement element)
			{
				if (element.Name.EndsWith("operation"))
				{
					Operation = new SoapOperation
					{
						SoapAction = element.GetAttribute("soapAction"),
						Style = element.GetAttribute("style")
					};

					_namespace = element.NamespaceURI;
				}
			}

			/// <summary>
			/// Represents an operation input, output, or fault message.
			/// </summary>
			public class OperationItem : IElementWithSpecialTransforms
			{
				/// <summary>
				/// Gets or sets the message reference.
				/// </summary>
				[XmlAttribute(AttributeName = "message")]
				public string Message { get; set; }

				/// <summary>
				/// Gets or sets the operation body configuration.
				/// </summary>
				[XmlElement(ElementName = "body")]
				public OperationBody Body { get; set; }

				/// <summary>
				/// Deserializes XML elements to populate operation item properties.
				/// </summary>
				/// <param name="element">The XML element to deserialize.</param>
				public void DeserializeElements(XmlElement element)
				{
					if (element.Name.EndsWith("body"))
					{
						Body = new OperationBody
						{
							Use = element.GetAttribute("use")
						};
					}
				}

				/// <summary>
				/// Represents the body configuration for an operation message.
				/// </summary>
				public class OperationBody
				{
					/// <summary>
					/// Gets or sets the encoding style (literal or encoded).
					/// </summary>
					[XmlAttribute(AttributeName = "use")]
					public string Use { get; set; }
				}
			}
		}

		/// <summary>
		/// Represents SOAP operation configuration details.
		/// </summary>
		public class SoapOperation
		{
			/// <summary>
			/// Gets or sets the SOAP action URI.
			/// </summary>
			[XmlAttribute(AttributeName = "soapAction")]
			public string SoapAction { get; set; }

			/// <summary>
			/// Gets or sets the operation style (document or rpc).
			/// </summary>
			[XmlAttribute(AttributeName = "style")]
			public string Style { get; set; }
		}
	}
}
