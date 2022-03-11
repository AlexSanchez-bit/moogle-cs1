using TextTreatment;
namespace TextRepresentation;
enum Direction{Forward,Backward}
public class Query:BaseText
		   //hereda de BaseText 
		   //Query implementa la logica asociada a la consulta del usuario
{
	Dictionary<string,LinkedList<string>> operatorList;//diccionario para asociar a cada operador
							//con una lista de las palabras a las que afecta
	private int highestFrequency;//la frecuencia mas alta de los elementos del query

	public Query(string text):base()//Constructor de la clase , procesa los operadores y 
				  //luego el texto de la consulta , utilizando los metodos heredados de BaseText
	{
		text=RemoveOperators(text.ToLower());
		FillTerms(text);	
		highestFrequency=GetHighestFrequency();
	}

    public IEnumerable<string> GetOperatorList()//obtiene los operadores del query almacenados en 
	    					//las llaves del diccionario
	{
		return operatorList.Keys;		
	}
		
	public IEnumerable<string> GetOperatorWords(string Operator)
	{
		if(operatorList.ContainsKey(Operator))return operatorList[Operator];
		return new string[0];
	}

	public override int GetTermFrequency(string term)//sobreescribe el metodo definido en BaseText 
							//para obtener la frecuencia y variar 
							//la frecuencia en funcion del operador *
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


	private int GetHighestFrequency()//calcula la mayor de las frecuencias  de los documentos
	{
		int maximun=0;
		foreach(var aux in this.GetTerms())
		{
		   int frequency = this.GetFrequency(aux);
		   if(frequency>maximun)maximun=frequency;
		}
		return maximun;
	}

	private string RemoveOperators(string text)//elimina los operadores de la consulta original	
						//y los almacena en el diccionario con sus 
						//respectivas listas de palabras
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

	private void ProcessOperator(string text,char op,ref int position)//procesa cada operador 
									//y guarda sus palabras asociadas
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

	private void SaveOperatorWord(string Operator,string word)//Guarda el operador o 
								//agrega una palabra a su lista 
	{
		if(word==null || word=="")return;
	   if(!operatorList.ContainsKey(Operator))
	   {
		   operatorList.Add(Operator,new LinkedList<string>());
	   }		
	   operatorList[Operator].AddLast(word);
	}

	private string GetWord(string text,int position,Direction direction)//obtiene la palabra en 
						//funcion de la posicion del operador en la consulta
	{
		if(position <=0 || position >=text.Length)return "";

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
		return retVal;
	}

	private string Reverse(string word)//revierte un string
	{
		string retval="";
		for(int i =word.Length-1 ;i>=0;i--)
		{
		  retval+=word[i];
		}
		return retval;
	}

    private bool IsOperator(char op)//retorna true si el caracter es un operador false en caso contrario
    {
	return op=='!' || op=='^' || op=='~' || op=='*';
    }
}
