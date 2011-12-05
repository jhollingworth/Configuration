using System;

namespace Configuration
{
	public abstract class TokensFileReader
	{
		public TokensFileReader ()
		{
		}
		
		public abstract TokensFile Read(string path);
		
		public abstract string[] FileExtensions { get; }
	}
}

