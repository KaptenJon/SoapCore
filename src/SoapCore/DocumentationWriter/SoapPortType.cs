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
		/// Represents a SOAP port type definition in WSDL.
		/// </summary>
		public class SoapPortType
		{
			/// <summary>
			/// Gets or sets the name of the port type.
			/// </summary>
			[XmlAttribute(AttributeName = "name")]
			public string Name { get; set; }

			/// <summary>
			/// Gets or sets the list of operations for this port type.
			/// </summary>
			[XmlElement(ElementName = "operation")]
			public List<WsdlOperation> Operations { get; set; }
		}
	}
}
