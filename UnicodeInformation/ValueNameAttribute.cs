using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
	public sealed class ValueNameAttribute : Attribute
	{
		private readonly string name;

		public string Name { get { return name; } }

		public ValueNameAttribute(string name)
		{
			this.name = name;
		}
	}
}
