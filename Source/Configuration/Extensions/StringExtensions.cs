using System;

namespace Configuration
{
	public static class StringExtensions
	{
		public static string With(this string format, params string[] args)
		{
			return string.Format(format, args);
		}
	}
}

