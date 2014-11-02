using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode
{
	public struct UnicodeRationalNumber : IEquatable<UnicodeRationalNumber>
	{
		public static UnicodeRationalNumber Parse(string s)
		{
			if (s == null) throw new ArgumentNullException("s");
			if (s.Length == 0) throw new ArgumentException();

			int fractionBarIndex = s.IndexOf('/');

			return new UnicodeRationalNumber(long.Parse(fractionBarIndex >= 0 ? s.Substring(0, fractionBarIndex) : s), fractionBarIndex >= 0 ? byte.Parse(s.Substring(fractionBarIndex + 1)) : (byte)1);
		}

		public readonly long Numerator;
		public readonly byte Denominator;

		public UnicodeRationalNumber(long number)
		{
			this.Numerator = number;
			this.Denominator = 1;
		}

		public UnicodeRationalNumber(long numerator, byte denominator)
		{
			this.Numerator = numerator;
			this.Denominator = denominator;
		}

		public bool IsDefaultValue { get { return Numerator == 0 && Denominator == 0; } }

		public override string ToString()
		{
			return !IsDefaultValue ? Numerator.ToString() + "/" + Denominator.ToString() : string.Empty;
		}

		public bool Equals(UnicodeRationalNumber other)
		{
			// We don't consider 1/2 and 2/4 equal here, as, that wouldn't be the same character.
			return other.Numerator == Numerator && other.Denominator == Denominator;
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return (int)(Numerator << 8) | (Denominator) ^ (byte)(Numerator >> 56);
		}
	}
}
