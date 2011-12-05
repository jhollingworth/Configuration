using System;

namespace Configuration
{
	public class Token
	{
		public string Key { get; set; } 
		public string Value { get; set; }	
	
		public Token (string key, string @value)
		{
			Key = key;
			Value = @value;
		}
	}
}

