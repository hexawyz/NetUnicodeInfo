namespace System.Unicode
{
	/// <summary>Declares a name for a specific value.</summary>
	/// <remarks>
	/// Since this project tries to stick to the .NET Framework naming conventions, this attribute may be used to indicate standard property names and values names where applicable.
	/// It may also be of use when aliases are available for a given property or value.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
	public sealed class ValueNameAttribute : Attribute
	{
		private readonly string name;

		/// <summary>The name given to the property or value.</summary>
		public string Name { get { return name; } }

		/// <summary>Initializes an instance of the class <see cref="ValueNameAttribute"/>.</summary>
		/// <param name="name">The name given to the property or value on which this attribute is to be applied.</param>
		public ValueNameAttribute(string name)
		{
			this.name = name;
		}
	}
}
