using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode
{
	public struct CjkRadicalInfo
	{
		private readonly byte radicalIndex;
		private readonly CjkRadicalData radicalData;

		public byte RadicalIndex { get { return radicalIndex; } }

		public char TraditionalRadicalCodePoint { get { return radicalData.TraditionalRadicalCodePoint; } }
		public char TraditionalCharacterCodePoint { get { return radicalData.TraditionalCharacterCodePoint; } }
		public char SimplifiedRadicalCodePoint { get { return radicalData.SimplifiedRadicalCodePoint; } }
		public char SimplifiedCharacterCodePoint { get { return radicalData.SimplifiedCharacterCodePoint; } }

		public bool HasSimplifiedForm { get { return radicalData.HasSimplifiedForm; } }

		internal CjkRadicalInfo(byte radicalIndex, CjkRadicalData radicalData)
		{
			this.radicalIndex = radicalIndex;
			this.radicalData = radicalData;
		}
	}
}
