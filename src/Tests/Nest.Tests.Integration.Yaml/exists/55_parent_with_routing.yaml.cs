using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using NUnit.Framework;
using Nest.Tests.Integration.Yaml;


namespace Nest.Tests.Integration.Yaml.Exists4
{
	public partial class Exists4YamlTests
	{	


		[NCrunch.Framework.ExclusivelyUses("ElasticsearchYamlTests")]
		public class ParentWithRoutingTests : YamlTestsBase
		{
			[Test]
			public void ParentWithRoutingTest()
			{	

				//do indices.create 
				_body = new {
					mappings= new {
						test= new {
							_parent= new {
								type= "foo"
							}
						}
					},
					settings= new {
						number_of_replicas= "0"
					}
				};
				this.Do(()=> this._client.IndicesCreatePut("test_1", _body));

				//do cluster.health 
				this.Do(()=> this._client.ClusterHealthGet(nv=>nv
					.Add("wait_for_status", @"green")
				));

				//do index 
				_body = new {
					foo= "bar"
				};
				this.Do(()=> this._client.IndexPost("test_1", "test", "1", _body, nv=>nv
					.Add("parent", 5)
					.Add("routing", 4)
				));

				//is_true this._status; 
				this.IsTrue(this._status);

				//do exists 
				this.Do(()=> this._client.ExistsHead("test_1", "test", "1", nv=>nv
					.Add("parent", 5)
					.Add("routing", 4)
				));

				//is_true this._status; 
				this.IsTrue(this._status);

				//do exists 
				this.Do(()=> this._client.ExistsHead("test_1", "test", "1", nv=>nv
					.Add("parent", 5)
				));

				//is_false this._status; 
				this.IsFalse(this._status);

				//do exists 
				this.Do(()=> this._client.ExistsHead("test_1", "test", "1", nv=>nv
					.Add("routing", 4)
				));

				//is_true this._status; 
				this.IsTrue(this._status);

			}
		}
	}
}

