using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode
{
	internal sealed class EnumHelper<T>
		where T : struct
	{
		private static readonly Dictionary<string, T> namedValueDictionary = CreateNamedValueDictionary();

		private static Dictionary<string, T> CreateNamedValueDictionary()
		{
			var type = typeof(T).GetTypeInfo();

			if (!type.IsEnum) throw new InvalidOperationException();

			return
			(
				from field in type.DeclaredFields
				from attr in field.GetCustomAttributes<ValueNameAttribute>()
				where attr.Name != null
				select new KeyValuePair<string, T>(attr.Name, (T)field.GetValue(null))
			).ToDictionary(kvp => kvp.Key, kvp => kvp.Value, StringComparer.OrdinalIgnoreCase);
        }

		public static bool TryGetNamedValue(string name, out T value)
		{
			return namedValueDictionary.TryGetValue(name, out value);
		}
	}
}
