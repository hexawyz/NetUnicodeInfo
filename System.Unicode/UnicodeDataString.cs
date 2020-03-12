using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Unicode
{
	/// <summary>An UTF-8 string part of the unicode database.</summary>
	/// <remarks>
	/// <para>
	/// All strings provided by the unicode database are store in UTF-8.
	/// </para>
	/// <para>
	/// Storing strings as UTF-8 servers three main advantages:
	/// <list type="bullet">
	/// <item><description>Greatly reduce memory costs related to string storage.</description></item>
	/// <item><description>Avoid allocating many permanent objects.</description></item>
	/// <item><description>Reduce start up times of the library.</description></item>
	/// </list>
	/// </para>
	/// <para>
	/// As a consequence, allocation of string objects will occur when calling the <see cref="ToString"/> method.
	/// This is not ideal, but the general memory savings should be well worth it.
	/// </para>
	/// <para>
	/// This class exposes methods <see cref="AsSpan"/> to provide direct access to the string contents.
	/// However, these methods may still need to allocate (like <see cref="ToString"/>) if contents need to be lazily generated.
	/// Please check the result of the property <see cref="IsLazy"/> to determine if those method will allocate.
	/// </para>
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public readonly struct UnicodeDataString : IEquatable<UnicodeDataString>
	{
		// The address should mostly be considered as a opaque handle instead of a real address.
		// Most of the time, the address will represent an offset into some data block.
		// However, strings for unicode character names may be generated lazily (by allocating 😢) in some cases.
		// In such cases, the adress will not directly represent the address of a string, but of some data block instead.
		// The code point will also be stored inside the data string so that it can be recovered.
		private readonly int _dataAddress;
		private readonly byte _dataLength; // For now, we're fine with strings less than 256 bytes long.
		private readonly byte _codePointUpper; // Highest bits of the 21 bit code point.
		private readonly ushort _codePointLower; // Lowest bits of the 21 bit code point.

		/// <summary>Gets the length of the current string.</summary>
		public int Length => _dataLength;

		/// <summary>Gets a value indicating whether the contents of this string are lazily generated.</summary>
		/// <remarks>
		/// <para>
		/// Callers may want to be careful when contents of a data string are lazily allocated.
		/// When contents are lazily allocated, each call to <see cref="AsSpan"/> will allocate a new object on the GC heap.
		/// </para>
		/// <para>
		/// Lazy allocation of string contents will always occur for the names of code points in certain ranges.
		/// Names for such code points can be generated algorithmically as per the Unicode specification, which helps saving memory.
		/// </para>
		/// </remarks>
		public bool IsLazy => !UnicodeData.IsStaticString(_dataAddress);

		/// <summary>Gets a value indicating whether the current value is the default.</summary>
		public bool IsDefault => _dataLength == 0 && _dataAddress == 0;

		private int CodePoint => (_codePointUpper << 16) | _codePointLower;

		internal UnicodeDataString(int dataAddress, byte dataLength)
		{
			_dataAddress = dataAddress;
			_dataLength = dataLength;
			_codePointUpper = 0;
			_codePointLower = 0;
		}

		internal UnicodeDataString(int dataAddress, byte dataLength, int codePoint)
		{
			_dataAddress = dataAddress;
			_dataLength = dataLength;
			_codePointUpper = (byte)(codePoint >> 16);
			_codePointLower = unchecked((ushort)codePoint);
		}

		/// <summary>Gets a span that represents the UTF-8 string data.</summary>
		/// <returns>A span that can be used to directly address the raw data.</returns>
		public ReadOnlySpan<byte> AsSpan()
			=> UnicodeData.GetStringSpan(_dataAddress, _dataLength);

		/// <summary>Copies the contents of the current string into the specified buffer.</summary>
		/// <exception cref="ArgumentException"><paramref name="destination"/> is smaller than the source <see cref="UnicodeDataString"/>.</exception>
		/// <param name="destination">The span that will receive the contents.</param>
		public void CopyTo(Span<byte> destination)
		{
			if (UnicodeData.IsStaticString(_dataAddress))
			{
				UnicodeData.GetStringSpan(_dataAddress, _dataLength).CopyTo(destination);
			}
			else if (!UnicodeData.TryWriteString(destination, _dataAddress, _dataLength, CodePoint))
			{
				throw new ArgumentException();
			}
		}

		/// <summary>Converts the current value to a string representation.</summary>
		/// <returns>A string representation of the current value, or <c>null</c> if it is the default value.</returns>
#if !(NETFRAMEWORK || NETSTANDARD1_1 || NETSTANDARD2_0)
		public override string ToString() => IsDefault ? null : Encoding.UTF8.GetString(AsSpan());
#else
		public unsafe override string ToString()
		{
			if (IsDefault) return null;

			fixed (byte* rawData = AsSpan())
			{
				return Encoding.UTF8.GetString(rawData, _dataLength);
			}
		}
#endif

		/// <summary>Determines whether the specified <see cref="object" />, is equal to this instance.</summary>
		/// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
		/// <returns><see langword="true" /> if the specified <see cref="object" /> is equal to this instance; otherwise, <see langword="false" />.</returns>
		public override bool Equals(object obj)
			=> obj is UnicodeDataString @string && Equals(@string);

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns><see langword="true" /> if the current object is equal to the other parameter; otherwise, <see langword="false" />.</returns>
		public bool Equals(UnicodeDataString other)
			=> _dataAddress == other._dataAddress && _dataLength == other._dataLength;

		/// <summary>Returns a hash code for this instance.</summary>
		/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
#if NS2_1_COMPATIBLE
		public override int GetHashCode() => HashCode.Combine(_dataOffset, _dataLength);
#else
		public override int GetHashCode()
		{
			int hashCode = -1836601301;
			hashCode = hashCode * -1521134295 + _dataAddress.GetHashCode();
			hashCode = hashCode * -1521134295 + _dataLength.GetHashCode();
			return hashCode;
		}
#endif

		public static bool operator ==(UnicodeDataString left, UnicodeDataString right)
			=> left.Equals(right);

		public static bool operator !=(UnicodeDataString left, UnicodeDataString right)
			=> !(left == right);
	}
}
