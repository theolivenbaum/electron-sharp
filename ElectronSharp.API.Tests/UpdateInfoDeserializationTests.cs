using SocketIOClient.Newtonsoft.Json;
using static ElectronSharp.API.BridgeConnector;

namespace ElectronSharp.API.Tests
{
	public class UpdateInfoDeserializationTests
	{
		private readonly NewtonsoftJsonSerializer _serializer;

		public UpdateInfoDeserializationTests()
		{
			_serializer = new CamelCaseNewtonsoftJsonSerializer();
		}

		// TODO: add tests
		[Fact]
		public void Test1()
		{
		}

		private static string GetUpdateInfoJson(string embeddedFileName)
		{
			string manifestResourceName = $"{typeof(UpdateInfoDeserializationTests).Namespace}.EmbeddedResources.{embeddedFileName}";
			using var stream = typeof(UpdateInfoDeserializationTests).Assembly.GetManifestResourceStream(manifestResourceName);
			if (stream is null)
				throw new InvalidOperationException($"Embedded file `{embeddedFileName}` was not found.");

			using var streamReader = new StreamReader(stream);
			return streamReader.ReadToEnd();
		}
	}
}