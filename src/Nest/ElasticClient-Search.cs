﻿using System;
using System.Diagnostics;
using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.CSharp.RuntimeBinder;
using System.Reflection;
using System.Linq;
using Nest.Resolvers;

namespace Nest
{
	public partial class ElasticClient
	{

		/// <summary>
		/// Synchronously search using T as the return type
		/// </summary>
		public IQueryResponse<T> Search<T>(Func<SearchDescriptor<T>, SearchDescriptor<T>> searchSelector) where T : class
		{
			return Search<T, T>(searchSelector);
		}

		/// <summary>
		/// Synchronously search using TResult as the return type and T to construct the query
		/// </summary>
		public IQueryResponse<TResult> Search<T, TResult>(Func<SearchDescriptor<T>, SearchDescriptor<T>> searchSelector)
			where T : class
			where TResult : class
		{
			searchSelector.ThrowIfNull("searchSelector");
			var descriptor = searchSelector(new SearchDescriptor<T>());
			var pathInfo = ((IPathInfo<SearchQueryString>)descriptor).ToPathInfo(this._connectionSettings);
			var status = this.RawDispatch.SearchDispatch(pathInfo, descriptor);
			return this.Serializer.DeserializeSearchResponse<T, TResult>(status, descriptor);
		}


		/// <summary>
		/// Asynchronously search using T as the return type
		/// </summary>
		public Task<IQueryResponse<T>> SearchAsync<T>(Func<SearchDescriptor<T>, SearchDescriptor<T>> searchSelector) where T : class
		{
			return SearchAsync<T, T>(searchSelector);
		}

		/// <summary>
		/// Asynchronously search using TResult as the return type and T to construct the query
		/// </summary>
		public Task<IQueryResponse<TResult>> SearchAsync<T, TResult>(Func<SearchDescriptor<T>, SearchDescriptor<T>> searchSelector)
			where T : class
			where TResult : class
		{
			searchSelector.ThrowIfNull("searchSelector");
			var descriptor = searchSelector(new SearchDescriptor<T>());
			var pathInfo = ((IPathInfo<SearchQueryString>)descriptor).ToPathInfo(this._connectionSettings);
			return this.RawDispatch.SearchDispatchAsync(pathInfo, descriptor)
				.ContinueWith(t=> this.Serializer.DeserializeSearchResponse<T, TResult>(t.Result, descriptor));
		}
	}
}