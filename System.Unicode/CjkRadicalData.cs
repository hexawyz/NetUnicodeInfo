namespace System.Unicode
{
	internal readonly struct CjkRadicalData
	{
		public readonly char TraditionalRadicalCodePoint;
		public readonly char TraditionalCharacterCodePoint;
		public readonly char SimplifiedRadicalCodePoint;
		public readonly char SimplifiedCharacterCodePoint;

		internal CjkRadicalData(char radicalCodePoint, char characterCodePoint)
		{
			TraditionalRadicalCodePoint = radicalCodePoint;
			TraditionalCharacterCodePoint = characterCodePoint;
			SimplifiedRadicalCodePoint = radicalCodePoint;
			SimplifiedCharacterCodePoint = characterCodePoint;
		}

		internal CjkRadicalData(char traditionalRadicalCodePoint, char traditionalCharacterCodePoint, char simplifiedRadicalCodePoint, char simplifiedCharacterCodePoint)
		{
			TraditionalRadicalCodePoint = traditionalRadicalCodePoint;
			TraditionalCharacterCodePoint = traditionalCharacterCodePoint;
			SimplifiedRadicalCodePoint = simplifiedCharacterCodePoint;
			SimplifiedCharacterCodePoint = simplifiedCharacterCodePoint;
		}

		public bool HasSimplifiedForm
			=> SimplifiedRadicalCodePoint != TraditionalRadicalCodePoint
			|| SimplifiedCharacterCodePoint != TraditionalCharacterCodePoint;
	}
}
