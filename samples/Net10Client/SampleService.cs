using Models;
using System.Xml.Linq;

namespace Net10Client
{
	public class SampleService : ISampleService
	{
		public string Ping(string s)
		{
			Console.WriteLine("NET 10 Server - Exec ping method");
			return $"NET 10 Response: {s}";
		}

		public ComplexModelResponse PingComplexModel(ComplexModelInput inputModel)
		{
			Console.WriteLine("NET 10 Server - Input data. IntProperty: {0}, StringProperty: {1}", 
				inputModel.IntProperty, inputModel.StringProperty);

			return new ComplexModelResponse
			{
				FloatProperty = float.MaxValue / 2,
				StringProperty = inputModel.StringProperty,
				ListProperty = inputModel.ListProperty,
				DateTimeOffsetProperty = inputModel.DateTimeOffsetProperty
			};
		}

		public int[] IntArray()
		{
			return new int[] { 123, 456, 789 };
		}

		public void VoidMethod(out string s)
		{
			s = "Value from NET 10 server";
		}

		public Task<int> AsyncMethod()
		{
			return Task.FromResult(42);
		}

		public int? NullableMethod(bool? arg)
		{
			return null;
		}

		public void XmlMethod(XElement xml)
		{
			Console.WriteLine("NET 10 Server - XML: {0}", xml);
		}

		public ComplexReturnModel[] ComplexReturnModel()
		{
			return new ComplexReturnModel[]
			{
				new ComplexReturnModel { Id = 1, Name = "NET 10 Item 1" },
				new ComplexReturnModel { Id = 2, Name = "NET 10 Item 2" }
			};
		}
	}
}
