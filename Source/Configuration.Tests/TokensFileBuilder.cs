using System;
using System.Linq;
using System.Collections.Generic;
using Configuration;

namespace Configuration.Tests
{
	public class TokensFileBuilder : ICanAddAnEnvironmentToATokensFile
	{
		private Dictionary<string, EnvironmentTokensBuilder> _environments = 
			new Dictionary<string, EnvironmentTokensBuilder>(StringComparer.InvariantCultureIgnoreCase);
		
		public ICanAddTokensToAnEnvironment Has_the_environment(string environment)
		{
			if(_environments.ContainsKey(environment))	
			{
				return _environments[environment];	
			}
			
			return _environments[environment] = new EnvironmentTokensBuilder(this);
		}
		
		public TokensFile Build()
		{
			var output = new Dictionary<string, IEnumerable<Token>>();
			
			foreach(var env in _environments) 
			{
				output[env.Key] = env.Value.Build();
			}
			
			return new TokensFile(output);
		}
	}
	
	public class EnvironmentTokensBuilder : ICanAddTokensToAnEnvironment
	{
		private readonly TokensFileBuilder _tokensFileBuilder;
		private List<Token> _tokens = new List<Token>();
		
		public ICanAddAnEnvironmentToATokensFile And
		{
			get { return _tokensFileBuilder; } 
		}
		
		public EnvironmentTokensBuilder (TokensFileBuilder tokensFileBuilder)
		{
			_tokensFileBuilder = tokensFileBuilder;
		}
			
		public ICanAddTokensToAnEnvironment That_contains_the_token(string key, string @value)
		{
			_tokens.Add(new Token(key, value));
			
			return this;	
		}
		
		public IEnumerable<Token> Build()
		{
			return _tokens;
		}
	}
	
	public interface ICanAddAnEnvironmentToATokensFile 
	{
		ICanAddTokensToAnEnvironment Has_the_environment(string environment);
	}
	
	public interface ICanAddTokensToAnEnvironment 
	{
		ICanAddAnEnvironmentToATokensFile And { get; }
		ICanAddTokensToAnEnvironment That_contains_the_token(string key, string value);
	}
}

