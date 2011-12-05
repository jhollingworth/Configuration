using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;

namespace Configuration
{
	public class Tokenizer
	{
		private const string TokenId = "$";
		
		private readonly Regex _regex = new Regex(@"\" + TokenId + "(.*?)\\" + TokenId, RegexOptions.Compiled);
		private readonly List<TokensFile> _masterTokens;
		private readonly string _environment;
		
		public Tokenizer (string environment, params TokensFile[] masterTokens)
		{
			_environment = environment;
			_masterTokens = masterTokens.ToList();
		}
		
		public string Tokenize(string input, params TokensFile[] tokens)
		{
			foreach(Match match in _regex.Matches(input)) 
			{
				var key = match.Groups[0].Value;
				var @value = TryGetValue(key.Replace(TokenId, string.Empty), tokens);
				
				if(null == @value)
				{
					throw new TokenizationException("Could not find a value for the token {0}".With (key));
				}
				
				input = input.Replace(key, @value);
			}
			
			return input;	
		}
		
		private string TryGetValue(string key, IEnumerable<TokensFile> tokens)
		{
			var token = TryGetValueFromTokensFiles(key, tokens);
			
			if(null == token)
			{
				token = TryGetValueFromTokensFiles(key, _masterTokens);
			}
			
			return token == null ? null : token.Value; 
		}
		
		private Token TryGetValueFromTokensFiles(string key, IEnumerable<TokensFile> tokenFiles)
		{
			return tokenFiles
				.Select(file => file.TryGetToken(_environment, key))
				.Where(t => null != t)
				.SingleOrDefault();
		}
	}
}

