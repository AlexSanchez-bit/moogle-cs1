using TextTreatment;
namespace TextRepresentation;
enum Direction{Forward,Backward}
public class Query:BaseText
{
	Dictionary<string,LinkedList<string>> operatorList;
	private int highestFrequency;
	public Query(string text):base()
	{
		text=RemoveOperators(text.ToLower());
		FillTerms(text);	
		highestFrequency=GetHighestFrequency();
	}

    public IEnumerable<string> GetOperatorList()
	{
		return operatorList.Keys;		
	}
		
	public IEnumerable<string> GetOperatorWords(string Operator)
	{
		if(operatorList.ContainsKey(Operator))return operatorList[Operator];
		return new string[0];
	}

	public override int GetTermFrequency(string term)
	{
		var original = this.GetFrequency(term);
	   foreach(var aux in operatorList.Where(elem=>elem.Key[0]=='*'))
	   {
		   if(aux.Value.Contains(term))
		   {
			highestFrequency*=aux.Key.Length;
			original+=highestFrequency;
		   }
	   }
	   return original;
	}


	private int GetHighestFrequency()
	{
		int maximun=0;
		foreach(var aux in this.GetTerms())
		{
		   int frequency = this.GetFrequency(aux);
		   if(frequency>maximun)maximun=frequency;
		}
		return maximun;
	}

	private string RemoveOperators(string text)
	{
		operatorList= new Dictionary<string,LinkedList<string>>();
	  string retValue="";

	  for(int i=0;i<text.Length;i++)
	  {
		if(!IsOperator(text[i]))
		{
			retValue+=text[i];
		}
		else
		{
		    ProcessOperator(text,text[i],ref i); 
			retValue+=" ";
		}

	  }
	  return retValue;
	}

	private void ProcessOperator(string text,char op,ref int position)
	{
		string toSaveWord ="";
			switch(op)
			{
				case '^':
				 toSaveWord = GetWord(text,position+1,Direction.Forward);
				SaveOperatorWord("^",toSaveWord);
				break;

				case '!':
				 toSaveWord = TextProcessor.ProcessWord(GetWord(text,position+1,Direction.Forward));
				SaveOperatorWord("!",toSaveWord);
				break;

				case '*':
				string finaloperator ="*";
				while(position < text.Length-1 && text[position+1]=='*')
				{
					finaloperator+='*';
					position++;
				}
				 toSaveWord = TextProcessor.ProcessWord(GetWord(text,position+1,Direction.Forward));
				SaveOperatorWord(finaloperator,toSaveWord);
				break;

				case '~':
				var toSaveWord1 = TextProcessor.ProcessWord(GetWord(text,position+1,Direction.Forward));
				var toSaveWord2 = TextProcessor.ProcessWord(GetWord(text,position-1,Direction.Backward));
				SaveOperatorWord("~",toSaveWord1);
				SaveOperatorWord("~",toSaveWord2);				
				break;
			}		
	}

	private void SaveOperatorWord(string Operator,string word)
	{
		if(word==null || word=="")return;
	   if(!operatorList.ContainsKey(Operator))
	   {
		   operatorList.Add(Operator,new LinkedList<string>());
	   }		
	   operatorList[Operator].AddLast(word);
	}

	private string GetWord(string text,int position,Direction direction)
	{
		if(position < 0 || position > text.Length || text.Length<2)return "";

		int i=position;
		string retVal="";
		if(direction==Direction.Forward)
		{
			if(text[i]==' ')i++;
			for(;i<text.Length && (text[i]!=' ' && !IsOperator(text[i]));i++)
			{
			  		retVal+=text[i];		
			}

		}else{
			if(text[i]==' ')i--;
			for(;i>=0 &&(text[i]!=' ' && !IsOperator(text[i]));i--)
			{
			  	retVal+=text[i];	
			}		
		retVal=Reverse(retVal);	
		}
		Console.WriteLine(retVal);
		return retVal;
	}

	private string Reverse(string word)
	{
		string retval="";
		for(int i =word.Length-1 ;i>=0;i--)
		{
		  retval+=word[i];
		}
		return retval;
	}

    private bool IsOperator(char op)
    {
	return op=='!' || op=='^' || op=='~' || op=='*';
    }
}
