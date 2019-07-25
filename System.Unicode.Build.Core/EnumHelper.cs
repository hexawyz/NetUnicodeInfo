using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System.Unicode.Build.Core
{
	internal static class EnumHelper<T>
		where T : struct, Enum
	{
		private static readonly Dictionary<string, T> NamedValueDictionary = CreateNamedValueDictionary();

		private static Dictionary<string, T> CreateNamedValueDictionary()
		{
			var type = typeof(T).GetTypeInfo();

			if (!type.IsEnum) throw new InvalidOperationException();

			return
			(
				from field in type.DeclaredFields
				where field.IsPublic && field.IsLiteral
				from attr in field.GetCustomAttributes<ValueNameAttribute>()
				where attr.Name != null
				select new KeyValuePair<string, T>(attr.Name, (T)field.GetValue(null))
			).ToDictionary(kvp => kvp.Key, kvp => kvp.Value, StringComparer.OrdinalIgnoreCase);
		}

		public static bool TryGetNamedValue(string name, out T value)
		{
			return NamedValueDictionary.TryGetValue(name, out value);
		}
	}
}
