using TextTreatment;
namespace TextRepresentation;
public class BaseText 
{
	private Dictionary<string,WordInfo> terms;

	protected BaseText()
	{

		terms=new Dictionary<string,WordInfo>();
	}
	protected BaseText(string text):this()
	{
		FillTerms(text);
	}

	public int WordCount()
	{
		return this.terms.Count()>0?terms.Count():1;
	}

	protected void FillTerms(string text)
	{
		var Tokens = GetTokens(text);
                 int index =0;
		foreach(var token in Tokens)
		{
			if(token == null || token=="")continue;
			var processedWord = TextProcessor.ProcessWord(token);
			if(!terms.ContainsKey(processedWord))
			{
				terms.Add(processedWord,new WordInfo(token,index++));
				continue;
			}

			terms[processedWord].AddPos(token,index++);
		}
	}


	protected string[] GetTokens(string text)
	{
	return ReduceText(text).Split(" ");
	}

	private string ReduceText(string text)
	{
	  return text
		  .Replace('\n',' ')
		  .Replace(',',' ')
		  .Replace(';',' ')		
		  .Replace('-',' ')
		  .Replace('_',' ')		
		  ;
	}

	public IEnumerable<string> GetTerms()
	{
	  return terms.Keys;
	}

	public WordInfo GetTerm(string term)
	{
		if(terms.ContainsKey(term))
		{
		return terms[term];
		}
		return null;
	}

	public virtual  int GetTermFrequency(string term)
	{
		return GetFrequency(term);
	}
	 

	protected int GetFrequency(string term)
	{

		if(terms.ContainsKey(term))
		{
		return terms[term].GetFrequency();
		}
		return 0;
	}
}
