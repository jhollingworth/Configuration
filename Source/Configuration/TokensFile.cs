using System;
using System.Linq;
using System.Collections.Generic;

namespace Configuration
{
	public class TokensFile
	{
		private const string MasterTokens = "Master";
		private readonly Dictionary<string, IEnumerable<Token>> _tokens;
		
		public TokensFile(Dictionary<string, IEnumerable<Token>> tokens)
		{
			_tokens = tokens;
		}
		
		public Token TryGetToken(string environment, string key)
		{
			if(false == _tokens.ContainsKey(environment))
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
			   
			return _tokens[environment].SingleOrDefault(c => c.Key.Equals(key));
		}
	}
}

