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
		/// Represents a SOAP message definition in WSDL.
		/// </summary>
		public class SoapMessage
		{
			/// <summary>
			/// Gets or sets the name of the message.
			/// </summary>
			[XmlAttribute(AttributeName = "name")]
			public string Name { get; set; }

			/// <summary>
			/// Gets or sets the list of message parts.
			/// </summary>
			[XmlElement(ElementName = "part")]
			public List<SoapMessagePart> Part { get; set; }

			/// <summary>
			/// Represents a part of a SOAP message.
			/// </summary>
			public class SoapMessagePart
			{
				/// <summary>
				/// Gets or sets the name of the message part.
				/// </summary>
				[XmlAttribute(AttributeName = "name")]
				public string Name { get; set; }

				/// <summary>
				/// Gets or sets the element reference for the message part.
				/// </summary>
				[XmlAttribute(AttributeName = "element")]
				public string Element { get; set; }
			}
		}
	}
}
