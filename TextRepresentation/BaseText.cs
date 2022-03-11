using TextTreatment;
namespace TextRepresentation;
public class BaseText 
/***
 * Clase Base que abstrae la logica de lectura de texto y su almacenamiento
 */
{
	private Dictionary<string,WordInfo> terms;//Diccionario que tendra para cada termino , sus metadatos

	protected BaseText() //constructor basico que inicializa el Diccionario
	{

		terms=new Dictionary<string,WordInfo>();
	}
	protected BaseText(string text):this()//constructor que recive el texto , llama al constructor
					//basico y llena los campos de la clase
	{
		FillTerms(text);
	}

	public int WordCount()//retorna la cantidad de palabras asociadas al texto procesado
	{
		return this.terms.Count();
	}

	protected void FillTerms(string text)//Funcion para rellenar los terminos en el documento
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


	protected string[] GetTokens(string text)//retorna los tokens del texto (un array con sus palabras)
	{
	return ReduceText(text).Split(" ");
	}

	private string ReduceText(string text)//elimina caracteres innecesarios
	{
	  return text
		  .Replace('\n',' ')
		  .Replace(',',' ')
		  .Replace(';',' ')		
		  .Replace('-',' ')
		  .Replace('_',' ')		
		  ;
	}

	public IEnumerable<string> GetTerms()//retorna los terminos almacenados como llaves del diccionario
	{
	  return terms.Keys;
	}

	public WordInfo GetTerm(string term)//permite obtenet los metadatos de un termino especifico
		                            //o null si no se encuentra

	{
		if(terms.ContainsKey(term))
		{
		return terms[term];
		}
		return null;
	}

	public virtual  int GetTermFrequency(string term)//metodo para obtener la frecuencia de 
							//cada termino es virtual para que las clases 
							//hijas puedan modificar el metodo
	{
		return GetFrequency(term);
	}
	 

	protected int GetFrequency(string term)//metodo para obtener la frecuencia de un termino 
						//de manera interna 
	{

		if(terms.ContainsKey(term))
		{
		return terms[term].GetFrequency();
		}
		return 0;
	}
}
