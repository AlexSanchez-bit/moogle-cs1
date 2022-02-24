﻿namespace TextTreatment;
public static class TextProcessor
{

	public static IEnumerable<string> ProcessWords(string[] words)
	{
	 return words.Select(elem=>{
		return ProcessWord(elem);
			 }); 
	}

	private static string GetStem(string Original)
	{
	    return Stemmer.Stemize(Original);
	}


	public static string ProcessWord(string word)
	{
		return GetStem(RemoveSigns(ProcessNumbers(Normalize(word))));
	}	

	private static string Normalize(string original)
	{
		return original
			.ToLower()
			.Replace('á','a')
			.Replace('é','e')
			.Replace('í','i')
			.Replace('ó','o')
			.Replace('ú','u')
			.Replace('ñ','n');

	}

	private static string RemoveSigns(string wrd)
	{
		string retstring ="";
		foreach(char a in wrd)
		{
			if(char.IsLetter(a))
			{
				retstring+=a;
			}
		}
return retstring;
	}

	private static string ProcessNumbers(string word)
	{		
            int numberPosition=0;
	    if(word.Length==0)
	    {
		return word;
	    }
	    while(numberPosition<word.Length && char.IsNumber(word[numberPosition]))numberPosition++;	    
	    if(numberPosition>=word.Length)
	    {
		return "NUMBER";
	    }
	    else if(word[numberPosition]==':')
	    {
		return "TIME";
	    }else if(word[numberPosition]=='.')
	    {
			return "CODE";
	     }
	else if(word[numberPosition]=='/')
	    {
		return "DATE";
	    }
	    else
	    {
		return word;
	    }
	} 

}
