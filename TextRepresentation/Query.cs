using TextTreatment;
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
		
	public IEnumerable<string> GetOperatorWords(string Operator)
	{
		if(operatorList.ContainsKey(Operator))return operatorList[Operator];
		return new string[0];
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
				while(position<text.Length && text[position]=='*')
				{
					finaloperator+='*';
					position++;
				}
				 toSaveWord = TextProcessor.ProcessWord(GetWord(text,position,Direction.Forward));
				SaveOperatorWord(finaloperator,toSaveWord);
				break;

				case '~':
				var toSaveWord1 = (GetWord(text,position+1,Direction.Forward));
				var toSaveWord2 = (GetWord(text,position-1,Direction.Backward));
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
		int i=position;
		string retVal="";
		if(direction==Direction.Forward)
		{
			if(text[i]==' ')i++;
			for(;i<text.Length && text[i]!=' ';i++)
			{
			  		retVal+=text[i];		
			}

		}else{
			if(text[i]==' ')i--;
			for(;i>=0 && text[i]!=' ';i--)
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
