using System;
using System.IO;
using System.Collections.Generic;
using Yaml;

namespace Configuration
{
	public class YamlTokensFileReader : TokensFileReader
	{
		public YamlTokensFileReader ()
		{
		}
		
		public override TokensFile Read(string path)
		{
			var node = Node.FromFile(path);	
		
			var tokens = new Dictionary<string, IEnumerable<Token>>();
			
			return new TokensFile(tokens);
		}
		
		public override string[] FileExtensions 
		{
			get { return new [] { "yml", "yaml" }; }	
		}
	}
}

