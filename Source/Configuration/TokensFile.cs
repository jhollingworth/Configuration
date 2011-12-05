using System;
using System.Linq;
using System.Collections.Generic;

namespace Configuration
{
	public class TokensFile
	{
		private const string MasterTokens = "Master";
		public Dictionary<string, IEnumerable<Token>> Tokens { get; private set; }
		
		public TokensFile(Dictionary<string, IEnumerable<Token>> tokens)
		{
			Tokens = tokens;
		}
		
		public Token TryGetToken(string environment, string key)
		{
			if(false == Tokens.ContainsKey(environment))
			{
				if(environment == MasterTokens)
				{
					return null;
				}
				else
				{
					return TryGetToken(MasterTokens, key);
				}
			}
			   
			return Tokens[environment].SingleOrDefault(c => c.Key.Equals(key));
		}
	}
}

