using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using NUnit.Framework;


namespace Nest.Tests.Integration.Yaml.IndicesDeleteMapping
{
	public partial class IndicesDeleteMapping10BasicYaml10Tests
	{
		
		public class DeleteMappingTests10Tests
		{
			private readonly RawElasticClient _client;
			private object _body;
			private ConnectionStatus _status;
			private dynamic _response;
		
			public DeleteMappingTests10Tests()
			{
				var uri = new Uri("http:localhost:9200");
				var settings = new ConnectionSettings(uri, "nest-default-index");
				_client = new RawElasticClient(settings);
			}

			[Test]
			public void DeleteMappingTestsTests()
			{

				//do indices.create 
				_body = new {
					mappings= new {
						test_type= new {
							properties= new {
								text= new {
									type= "string",
									analyzer= "whitespace"
								}
							}
						}
					}
				};
				_status = this._client.IndicesCreatePost("test_index", _body);
				_response = _status.Deserialize<dynamic>();

				//do indices.exists_type 
				
				_status = this._client.IndicesExistsTypeHead("test_index", "test_type");
				_response = _status.Deserialize<dynamic>();

				//do indices.delete_mapping 
				
				_status = this._client.IndicesDeleteMapping("test_index", "test_type");
				_response = _status.Deserialize<dynamic>();

				//do indices.exists_type 
				
				_status = this._client.IndicesExistsTypeHead("test_index", "test_type");
				_response = _status.Deserialize<dynamic>();
			}
		}
	}
}