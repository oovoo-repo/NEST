using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using NUnit.Framework;
using Nest.Tests.Integration.Yaml;


namespace Nest.Tests.Integration.Yaml.GetSource7
{
	public partial class GetSource7YamlTests
	{	


		[NCrunch.Framework.ExclusivelyUses("ElasticsearchYamlTests")]
		public class SourceFiltering1Tests : YamlTestsBase
		{
			[Test]
			public void SourceFiltering1Test()
			{	

				//do index 
				_body = new {
					include= new {
						field1= "v1",
						field2= "v2"
					},
					count= "1"
				};
				this.Do(()=> this._client.IndexPost("test_1", "test", "1", _body));

				//do get_source 
				this.Do(()=> this._client.GetSource("test_1", "test", "1", nv=>nv
					.Add("_source_include", @"include.field1")
				));

				//match _response.include.field1: 
				this.IsMatch(_response.include.field1, @"v1");

				//is_false _response.include.field2; 
				this.IsFalse(_response.include.field2);

				//do get_source 
				this.Do(()=> this._client.GetSource("test_1", "test", "1", nv=>nv
					.Add("_source_include", @"include.field1,include.field2")
				));

				//match _response.include.field1: 
				this.IsMatch(_response.include.field1, @"v1");

				//match _response.include.field2: 
				this.IsMatch(_response.include.field2, @"v2");

				//is_false _response.count; 
				this.IsFalse(_response.count);

				//do get_source 
				this.Do(()=> this._client.GetSource("test_1", "test", "1", nv=>nv
					.Add("_source_include", @"include")
					.Add("_source_exclude", @"*.field2")
				));

				//match _response.include.field1: 
				this.IsMatch(_response.include.field1, @"v1");

				//is_false _response.include.field2; 
				this.IsFalse(_response.include.field2);

				//is_false _response.count; 
				this.IsFalse(_response.count);

			}
		}
	}
}

