using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicodeInformation.Tests
{
	public static class AssertEx
	{
		public static void ThrowsExactly<TException>(Action action, string methodName)
			where TException : Exception
		{
			try
			{
				action();
			}
			catch (TException ex)
			{
				if (ex.GetType() != typeof(TException))
					Assert.Fail("The " + methodName + " method should throw an exception of type " + typeof(TException).Name + " but got " + ex.GetType().Name + ".");
				else
					return;
			}

			Assert.Fail("The " + methodName + " method should throw an exception of type " + typeof(TException).Name + ".");
		}
	}
}
