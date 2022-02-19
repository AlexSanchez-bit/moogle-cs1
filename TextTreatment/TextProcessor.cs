namespace TextTreatment;
public static class TextProcessor
{

	public static IEnumerable<string> ProcessWords(string[] words)
	{
	 return words.Select(elem=>{
		return ProcessWord(elem);
			 }); 
	}

	public static string GetStem(string Original)
	{
	    return Stemmer.Stemize(Original);
	}

	public static string ProcessWord(string word)
	{
		return RemoveSigns(ProcessNumbers(word));
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
	    while(numberPosition<word.Length && char.IsNumber(word[numberPosition++])){}
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
