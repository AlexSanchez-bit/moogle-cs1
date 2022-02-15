namespace TextRepresentation;
enum Direction{Forward,Backward}
public class Query:BaseText
{
	Dictionary<string,LinkedList<string>> operatorList;
	public Query(string text):base()
	{
		text=RemoveOperators(text.ToLower());
		FillTerms(text);	
	}

    public IEnumerable<string> GetOperatorList()
	{
		return operatorList.Keys;		
	}
		
	public IEnumerable<string> GetOperatorWords(string operator)
	{
		return operatorList[operator];
	}

	private string RemoveOperators(string text)
	{
		operatorList= new Dictionary<string,LinkedList<string>>();
	  string retValue="";

	  foreach(char a in text)
	  {
		if(!IsOperator(a))
		{
			retValue+=a;
		}
		else
		{
		    ProcessOperator(a); 
		}

	  }
	  return retValue;
	}

	private void ProcessOperator(string text,char op,int position)
	{
			switch(op)
			{
				case '^':
				var toSaveWord = GetWord(text,position+1,Direction.Forward);
				SaveOperatorWord("^",toSaveWord);
				break;

				case '!':
				var toSaveWord = TextProcessor.ProcessWord(GetWord(text,position+1,Direction.Forward));
				SaveOperatorWord("!",toSaveWord);
				break;

				case '*':
				string finaloperator ="*";
				while(text[position++]<text.Length && text[position]=='*')finaloperator+='*';
				var toSaveWord = TextProcessor.ProcessWord(GetWord(text,position,Direction.Forward));
				SaveOperatorWord(finaloperator,toSaveWord);
				break;

				case '~':
				var toSaveWord1 = TextProcessor.ProcessWord(GetWord(text,position+1,Direction.Forward));
				var toSaveWord2 = TextProcessor.ProcessWord(GetWord(text,position-1,Direction.Backward));
				SaveOperatorWord("~",toSaveWord1);
				SaveOperatorWord("~",toSaveWord2);
			}		
	}

	private void SaveOperatorWord(string operator,string word)
	{
		if(word==null || word=="")return;
	   if(!operatorList.ContainsKey(operator))
	   {
		   operatorList.Add(operator,new LinkedList<string>());
	   }		
	   operatorList[operator].AddLast(word);
	}

	private string GetWord(string text,int position,Direction direction)
	{
		int i=position;
		string retVal="";
		if(direction==Direction.Forward)
		{
			for(;i<text.Length && text[i]!=" ";i++)
			{
			  		retVal+=text[i];		
			}

		}else{
			for(;i>0 && text[i]!=" ";i--)
			{
			  	retVal+=text[i];	
			}
		}
		return retVal;
	}

    private bool IsOperator(char op)
    {
	return op=='!' || op=='^' || op=='~' || op=='*';
    }
}
