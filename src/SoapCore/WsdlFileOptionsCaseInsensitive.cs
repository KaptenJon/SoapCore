using System;
using System.Collections.Generic;

namespace SoapCore
{
	public class WsdlFileOptionsCaseInsensitive : WsdlFileOptions
	{
		public override Dictionary<string, WebServiceWSDLMapping> WebServiceWSDLMapping { get; set; } = new Dictionary<string, WebServiceWSDLMapping>(StringComparer.OrdinalIgnoreCase);
	}
}
