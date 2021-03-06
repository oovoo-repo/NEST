using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using NUnit.Framework;
using Nest.Tests.Integration.Yaml;


namespace Nest.Tests.Integration.Yaml.ClusterNodeInfo
{
	public partial class ClusterNodeInfoTests
	{	


		[NCrunch.Framework.ExclusivelyUses("ElasticsearchYamlTests")]
		public class NodeInfoTestTests : YamlTestsBase
		{
			[Test]
			public void NodeInfoTestTest()
			{	

				//do cluster.node_info 
				this.Do(()=> this._client.ClusterNodeInfoGet());

				//is_true _response.ok; 
				this.IsTrue(_response.ok);

				//is_true _response.nodes; 
				this.IsTrue(_response.nodes);

				//is_true _response.cluster_name; 
				this.IsTrue(_response.cluster_name);

			}
		}
	}
}

