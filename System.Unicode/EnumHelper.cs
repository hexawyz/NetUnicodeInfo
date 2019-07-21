using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System.Unicode
{
	internal static class EnumHelper<T>
		where T : struct, Enum
	{
		private static readonly Dictionary<T, string[]> ValueNameDictionary = CreateValueNameDictionary();

		private static Dictionary<T, string[]> CreateValueNameDictionary()
		{
			var type = typeof(T).GetTypeInfo();

			if (!type.IsEnum) throw new InvalidOperationException();

			return
			(
				from field in type.DeclaredFields
				where field.IsPublic && field.IsLiteral
				select new KeyValuePair<T, string[]>
				(
					(T)field.GetValue(null),
					(
						from attr in field.GetCustomAttributes<ValueNameAttribute>()
						where attr.Name != null
						select attr.Name
					).ToArray()
				)
			).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
		}

		public static string[] GetValueNames(T value) => ValueNameDictionary.TryGetValue(value, out string[] names) ? names : null;
	}
}
