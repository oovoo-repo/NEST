using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using NUnit.Framework;
using Nest.Tests.Integration.Yaml;


namespace Nest.Tests.Integration.Yaml.Index11
{
	public partial class Index11YamlTests
	{	


		[NCrunch.Framework.ExclusivelyUses("ElasticsearchYamlTests")]
		public class Ttl1Tests : YamlTestsBase
		{
			[Test]
			public void Ttl1Test()
			{	

				//do indices.create 
				_body = new {
					mappings= new {
						test= new {
							_ttl= new {
								enabled= "1",
								store= "yes",
								@default= "10s"
							}
						}
					}
				};
				this.Do(()=> this._client.IndicesCreatePut("test_1", _body));

				//do cluster.health 
				this.Do(()=> this._client.ClusterHealthGet(nv=>nv
					.Add("wait_for_status", @"yellow")
				));

				//do index 
				_body = new {
					foo= "bar"
				};
				this.Do(()=> this._client.IndexPost("test_1", "test", "1", _body));

				//do get 
				this.Do(()=> this._client.Get("test_1", "test", "1", nv=>nv
					.Add("fields", @"_ttl")
				));

				//lt _response.fields._ttl: 10000; 
				this.IsLowerThan(_response.fields._ttl, 10000);

				//gt _response.fields._ttl: 0; 
				this.IsGreaterThan(_response.fields._ttl, 0);

				//do index 
				_body = new {
					foo= "bar"
				};
				this.Do(()=> this._client.IndexPost("test_1", "test", "1", _body, nv=>nv
					.Add("ttl", 100000)
				));

				//do get 
				this.Do(()=> this._client.Get("test_1", "test", "1", nv=>nv
					.Add("fields", @"_ttl")
				));

				//lt _response.fields._ttl: 100000; 
				this.IsLowerThan(_response.fields._ttl, 100000);

				//gt _response.fields._ttl: 0; 
				this.IsGreaterThan(_response.fields._ttl, 0);

				//do index 
				_body = new {
					foo= "bar"
				};
				this.Do(()=> this._client.IndexPost("test_1", "test", "1", _body, nv=>nv
					.Add("ttl", @"20s")
				));

				//do get 
				this.Do(()=> this._client.Get("test_1", "test", "1", nv=>nv
					.Add("fields", @"_ttl")
				));

				//lt _response.fields._ttl: 20000; 
				this.IsLowerThan(_response.fields._ttl, 20000);

				//gt _response.fields._ttl: 0; 
				this.IsGreaterThan(_response.fields._ttl, 0);

				//do index 
				_body = new {
					foo= "bar"
				};
				this.Do(()=> this._client.IndexPost("test_1", "test", "1", _body, nv=>nv
					.Add("ttl", @"20s")
					.Add("timestamp", @"2013-06-23T18:14:40")
				), shouldCatch: @"/AlreadyExpiredException/");

			}
		}
	}
}

