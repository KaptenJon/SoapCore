using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace SoapCore
{
	public class WsdlFileOptions
	{
		public virtual Dictionary<string, WebServiceWSDLMapping> WebServiceWSDLMapping { get; set; } = new Dictionary<string, WebServiceWSDLMapping>();
		public string UrlOverride { get; set; }
		public string SchemeOverride { get; set; }
		public string VirtualPath { get; set; }
		public string AppPath { get; set; }
		public Func<WsdlFileOptions, HttpContext, string> UrlOverrideFunc { get; set; }
	}
}
