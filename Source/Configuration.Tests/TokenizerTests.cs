using System;
using NUnit.Framework;
using Machine.Specifications;

namespace Configuration.Tests
{
	public abstract class TokenizerContext
	{
		private TokensFileBuilder _masterTokensFile, _tokensFile;
		private string _input;
		private bool _expectingException;
		
		protected Exception Exception { get; private set; }
		protected string Result { get; private set; }
		
		public TokenizerContext()
		{
			_masterTokensFile = new TokensFileBuilder();
			_tokensFile = new TokensFileBuilder();
			
		}
		
		protected ICanAddAnEnvironmentToATokensFile Given_the_master_tokens_file
		{
			get { return _masterTokensFile; }
		}
		
		protected ICanAddAnEnvironmentToATokensFile Given_the_tokens_file
		{
			get { return _tokensFile; }
		}
		
		protected void Given_the_file_to_tokenize_contains(string input)
		{
			_input = input;		
		}
	
		protected void Given_I_am_expecting_an_exception()
		{
			_expectingException = true;
		}
		
		protected void When_I_tokenize_the_file_with_environment(string environment)
		{
			try
			{
				Result = new Tokenizer(environment, _masterTokensFile.Build()).Tokenize(_input, _tokensFile.Build());
			}
			catch(Exception ex)
			{
				if(_expectingException)	
				{
					Exception = ex;
				}
				else
				{
					throw;
				}
			}
		}
	}
	
	[TestFixture]
	public class When_I_tokenize_a_file_with_a_master_tokens_file : TokenizerContext
	{
		public When_I_tokenize_a_file_with_a_master_tokens_file ()
		{
			Given_the_file_to_tokenize_contains("<a>$Foo$</a>");
			Given_the_master_tokens_file
				.Has_the_environment("Dev")
					.That_contains_the_token("Foo" ,"Bar");
			
			When_I_tokenize_the_file_with_environment("Dev");
		}
		
		[Test]
		public void Then_the_file_should_be_tokenized()
		{
			Result.ShouldEqual("<a>Bar</a>");
		}
	}
	
	[TestFixture]
	public class When_I_tokenize_a_file_with_a_tokens_file : TokenizerContext
	{
		public When_I_tokenize_a_file_with_a_tokens_file ()
		{
			Given_the_file_to_tokenize_contains("<a>$Win$</a>");
			Given_the_tokens_file
				.Has_the_environment("Test")
					.That_contains_the_token("Win" ,"Doop");
		
			When_I_tokenize_the_file_with_environment("Test");
		}
		
		[Test]
		public void Then_the_file_should_be_tokenized()
		{
			Result.ShouldEqual("<a>Doop</a>");
		}
	}
	
	[TestFixture]
	public class When_I_tokenize_a_file_which_contains_a_token_which_isnt_in_a_tokens_file : TokenizerContext
	{
		public When_I_tokenize_a_file_which_contains_a_token_which_isnt_in_a_tokens_file ()
		{
			Given_the_file_to_tokenize_contains("<a>$Fin$</a>");
			Given_the_tokens_file
				.Has_the_environment("Test")
					.That_contains_the_token("Win" ,"Doop");
			Given_I_am_expecting_an_exception();
			
			When_I_tokenize_the_file_with_environment("Test");
		}
		
		[Test]
		public void Then_it_should_throw_an_exception()
		{
			Exception.ShouldBeOfType<TokenizationException>();
		}
	}
}

