﻿using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Nest.Tests.Unit.Cluster
{
	[TestFixture]
	public class NodeTests : BaseJsonTests
	{
		[Test]
		public void NodeInfoSimple()
		{
			var r = this._client.NodesInfo();
			var status = r.ConnectionStatus;
			var url = new Uri(status.RequestUrl);
			url.AbsolutePath.Should().StartWith("/_nodes");
		}

		[Test]
		public void NodeInfoFlags()
		{
			var r = this._client.NodesInfo(c=>c.Metrics(NodesInfoMetric.Process, NodesInfoMetric.Os));
			var status = r.ConnectionStatus;
			var url = new Uri(status.RequestUrl);
			url.AbsolutePath.Should().StartWith("/_nodes/process%2Cos");
		}
		[Test]
		public void NodeInfoFlagsAndNodes()
		{	
			var r = this._client.NodesInfo(c=>c
				.NodeId("127.0.0.1") //Node id's don't look like this
				.Metrics(NodesInfoMetric.Process, NodesInfoMetric.Os)
			);
			var status = r.ConnectionStatus;
			var url = new Uri(status.RequestUrl);
			url.AbsolutePath.Should().StartWith("/_nodes/127.0.0.1/process%2Cos");
		}

		[Test]
		public void NodeStats()
		{
			var r = this._client.NodesStats();
			var status = r.ConnectionStatus;
			var url = new Uri(status.RequestUrl);
			url.AbsolutePath.Should().StartWith("/_nodes/stats");
			
		}
		[Test]
		public void NodeStatsSpecificNodeWithFlags()
		{
			var r = this._client.NodesStats(c=>c
				.NodeId("127.0.0.1")
				.Metrics(NodesStatsMetric.Network, NodesStatsMetric.Http)
			);
			var status = r.ConnectionStatus;
			var url = new Uri(status.RequestUrl);
			url.AbsolutePath.Should().StartWith("/_nodes/127.0.0.1/stats/network%2Chttp");
		}

	}
}