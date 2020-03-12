using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace System.Unicode
{
	/// <summary>Represents a name alias for an Unicode code point.</summary>
	[DebuggerDisplay("{DisplayText,nq}")]
	public readonly struct UnicodeNameAlias : IEquatable<UnicodeNameAlias>
	{
		internal static readonly UnicodeNameAlias[] EmptyArray = new UnicodeNameAlias[0];

		/// <summary>Gets the alias name.</summary>
		/// <value>The name.</value>
		public UnicodeDataString Name { get; }

		/// <summary>Gets the kind of alias.</summary>
		/// <value>The kind of alias.</value>
		public UnicodeNameAliasKind Kind { get; }

		private string DisplayText => (Kind != 0 ? "<" + EnumHelper<UnicodeNameAliasKind>.GetValueNames(Kind).FirstOrDefault() + "> " : string.Empty) + Name.ToString();

		internal UnicodeNameAlias(UnicodeDataString name, UnicodeNameAliasKind kind)
		{
			Name = name;
			Kind = kind;
		}

		/// <summary>Determines whether the specified <see cref="object" />, is equal to this instance.</summary>
		/// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
		/// <returns><see langword="true" /> if the specified <see cref="object" /> is equal to this instance; otherwise, <see langword="false" />.</returns>
		public override bool Equals(object obj)
			=> obj is UnicodeNameAlias alias && Equals(alias);

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns><see langword="true" /> if the current object is equal to the other parameter; otherwise, <see langword="false" />.</returns>
		public bool Equals(UnicodeNameAlias other)
			=> Name.Equals(other.Name) && Kind == other.Kind;

		/// <summary>Returns a hash code for this instance.</summary>
		/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
#if NS2_1_COMPATIBLE
		public override int GetHashCode() => HashCode.Combine(_dataOffset, _dataLength);
#else
		public override int GetHashCode()
		{
			int hashCode = 1612084825;
			hashCode = hashCode * -1521134295 + EqualityComparer<UnicodeDataString>.Default.GetHashCode(Name);
			hashCode = hashCode * -1521134295 + Kind.GetHashCode();
			return hashCode;
		}
#endif

		public static bool operator ==(UnicodeNameAlias left, UnicodeNameAlias right)
			=> left.Equals(right);

		public static bool operator !=(UnicodeNameAlias left, UnicodeNameAlias right)
			=> !(left == right);
	}
}
