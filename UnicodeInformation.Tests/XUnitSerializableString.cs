using System;
using System.Unicode;
using System.Globalization;
using System.Collections.Generic;
using System.Collections;
using Xunit;
using System.Linq;
using Xunit.Abstractions;
using System.Text;

namespace UnicodeInformation.Tests
{
	// This class is needed because apparently, somewhere in the process of unit testing, strings with invalid UTF-16 sequences are "fixed", which totally messes up the tests here.
	// This is just a wrapper over regular strings… Data is serialized as an array of chars instead of a string. This seems to do the trick.
	public class XUnitSerializableString : IEquatable<XUnitSerializableString>, IXunitSerializable
	{
		private string value;

		public XUnitSerializableString() : this(null) { }

		public XUnitSerializableString(string value)
		{
			this.value = value;
		}

		void IXunitSerializable.Deserialize(IXunitSerializationInfo info)
		{
			var chars = info.GetValue<char[]>("Chars");

			value = chars != null ?
				new string(chars) :
				null;
		}

		void IXunitSerializable.Serialize(IXunitSerializationInfo info)
		{
			info.AddValue("Chars", value?.ToCharArray(), typeof(char[]));
		}

		public override string ToString()
		{
			if (string.IsNullOrEmpty(value)) return value;

			var sb = new StringBuilder(value.Length * 6);

			foreach (char c in value)
			{
				sb.Append(@"\u")
					.Append(((ushort)c).ToString("X4"));
			}

			return sb.ToString();
		}

		public bool Equals(XUnitSerializableString other) => value == other.value;
		public override bool Equals(object obj) => obj is XUnitSerializableString && Equals((XUnitSerializableString)obj);
		public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(value);

		public static implicit operator string(XUnitSerializableString text) => text.value;
		public static implicit operator XUnitSerializableString(string text) => new XUnitSerializableString(text);
	}
}
