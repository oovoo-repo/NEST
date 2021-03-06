﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Reflection;

namespace Nest.Resolvers
{
	public class IndexNameResolver
	{
		private readonly IConnectionSettings _connectionSettings;

		public IndexNameResolver(IConnectionSettings connectionSettings)
		{
			connectionSettings.ThrowIfNull("hasDefaultIndices");
			this._connectionSettings = connectionSettings;
		}

		public string GetIndexForType<T>()
		{
			return this.GetIndexForType(typeof(T));
		}

		public string GetIndexForType(Type type)
		{
			var defaultIndices = this._connectionSettings.DefaultIndices;

			if (defaultIndices == null)
				return this._connectionSettings.DefaultIndex;
			if (defaultIndices.ContainsKey(type) && !string.IsNullOrWhiteSpace(defaultIndices[type]))
				return defaultIndices[type];
			return this._connectionSettings.DefaultIndex;
		}


		internal string GetIndexForType(IndexNameMarker i)
		{
			return i.Name ?? this.GetIndexForType(i.Type);
		}
	}
}
