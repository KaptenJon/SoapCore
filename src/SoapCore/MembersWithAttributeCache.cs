using System;
using System.Collections.Concurrent;

namespace SoapCore
{
	/// <summary>Extensions to <see cref="Type"/>.</summary>
	internal static partial class ReflectionExtensions
	{
		private static class MembersWithAttributeCache<TAttribute>
			where TAttribute : Attribute
		{
#pragma warning disable SA1401 // Fields should be private
			internal static ConcurrentDictionary<Type, MemberWithAttribute<TAttribute>[]> CacheEntries = new ();
#pragma warning restore SA1401
		}
	}
}
