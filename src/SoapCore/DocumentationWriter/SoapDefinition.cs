using System.Collections.Generic;
using System.Xml.Serialization;

namespace SoapCore.DocumentationWriter
{
	/// <summary>
	/// Represents a SOAP service definition in WSDL format.
	/// </summary>
	[XmlRoot(ElementName = "definitions", Namespace = "http://schemas.xmlsoap.org/wsdl/")]
	public partial class SoapDefinition
	{
		/// <summary>
		/// Gets or sets the name of the service definition.
		/// </summary>
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the target namespace for the service definition.
		/// </summary>
		[XmlAttribute(AttributeName = "targetNamespace")]
		public string TargetNamespace { get; set; }

		/// <summary>
		/// Gets or sets the types definitions for the service.
		/// </summary>
		[XmlElement(ElementName = "types")]
		public SoapTypes Types { get; set; }

		/// <summary>
		/// Gets or sets the list of messages used in the service.
		/// </summary>
		[XmlElement(ElementName = "message")]
		public List<SoapMessage> Messages { get; set; }

		/// <summary>
		/// Gets or sets the port type definition for the service.
		/// </summary>
		[XmlElement(ElementName = "portType")]
		public SoapPortType PortType { get; set; }

		/// <summary>
		/// Gets or sets the list of bindings for the service.
		/// </summary>
		[XmlElement(ElementName = "binding")]
		public List<SoapBinding> Bindings { get; set; }

		/// <summary>
		/// Gets or sets the service definition.
		/// </summary>
		[XmlElement(ElementName = "service")]
		public SoapService Service { get; set; }
	}
}
