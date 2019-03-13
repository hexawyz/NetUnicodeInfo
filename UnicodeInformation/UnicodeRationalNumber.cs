namespace System.Unicode
{
	/// <summary>Represents a rational number in a format compatible with the Unicode standard.</summary>
	public struct UnicodeRationalNumber : IEquatable<UnicodeRationalNumber>
	{
		/// <summary>Parses a rational number from a string representation.</summary>
		/// <remarks>
		/// Valid text representations should match the regex pattern /-?[0-9]+(?:\/[0-9]+)/.
		/// The numerator part should fit in a <see cref="long"/>, and the denominator part should fit in a <see cref="byte"/>.
		/// </remarks>
		/// <param name="s">The string to parse.</param>
		/// <returns>The rational number parsed from the string.</returns>
		/// <exception cref="ArgumentNullException">The parameter <paramref name="s"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException">The parameter <paramref name="s"/> is empty.</exception>
		public static UnicodeRationalNumber Parse(string s)
		{
			if (s == null) throw new ArgumentNullException(nameof(s));
			if (s.Length == 0) throw new ArgumentException();

			int fractionBarIndex = s.IndexOf('/');

			return new UnicodeRationalNumber(long.Parse(fractionBarIndex >= 0 ? s.Substring(0, fractionBarIndex) : s), fractionBarIndex >= 0 ? ushort.Parse(s.Substring(fractionBarIndex + 1)) : (byte)1);
		}

		/// <summary>The numerator of the fraction.</summary>
		public readonly long Numerator;
		/// <summary>The denominator of the fraction.</summary>
		public readonly ushort Denominator;

		/// <summary>Initializes a new instance of the <see cref="UnicodeRationalNumber"/> structure that represents a signed integer..</summary>
		/// <param name="number">The number which should be represented as a rational number.</param>
		public UnicodeRationalNumber(long number)
		{
			this.Numerator = number;
			this.Denominator = 1;
		}

		/// <summary>Initializes a new instance of the <see cref="UnicodeRationalNumber"/> structure that represents a signed integer..</summary>
		/// <param name="numerator">The number which should be used as numerator in the rational number.</param>
		/// <param name="denominator">The number which should be used as denominator in the rational number.</param>
		public UnicodeRationalNumber(long numerator, ushort denominator)
		{
			this.Numerator = numerator;
			this.Denominator = denominator;
		}

		/// <summary>Gets a value indicating whether the current value is the default value of the type.</summary>
		/// <remarks>The default value is an invalid fraction of 0/0.</remarks>
		public bool IsDefaultValue { get { return Numerator == 0 && Denominator == 0; } }

		/// <summary>Creates a string representation of the current rational number.</summary>
		/// <returns>The created representation is culture invariant, and will be parsable by the <see cref="Parse(string)"/> method.</returns>
		public override string ToString()
		{
			return !IsDefaultValue ? Denominator != 1 ? Numerator.ToString() + "/" + Denominator.ToString() : Numerator.ToString() : string.Empty;
		}

		/// <summary>Determines whether the specified rational number is equal to the current value.</summary>
		/// <param name="other">The other value to compare to the current one.</param>
		/// <returns><see langword="true"/> if the two values are the same; <see langword="false"/> otherwise.</returns>
		public bool Equals(UnicodeRationalNumber other)
		{
			// We don't consider 1/2 and 2/4 equal here, as, that wouldn't be the same character.
			return other.Numerator == Numerator && other.Denominator == Denominator;
		}

		/// <summary>Determines whether the specified object is equal to the current rational number.</summary>
		/// <param name="obj">The object to compare to the current rational number.</param>
		/// <returns><see langword="true"/> if the object represents the same rational number; <see langword="false"/> otherwise.</returns>
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		/// <summary>Returns the hash code for the current rational number.</summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode()
		{
			return (int)(Numerator << 8) | (Denominator) ^ (byte)(Numerator >> 56);
		}
	}
}
