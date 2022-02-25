using TextRepresentation;
using AlgeTool;
namespace SearchEngine;
public class Vocabullary
{
	private Dictionary<string ,LinkedList<Document>> corpus; //corpus de documentos
	private Dictionary<string,Vector> vectorizedDocs;//vectores de cada documento indexados por su nombre
	private int corpusSize;//tamanno del corpus(cantidad de documentos)
	private LinkedList<string> QueryErrors;//palabras del query a arreglar


	public Vocabullary(Document[] documents)//constructor de la clase
		//almacena los documentos en dependencia de sus terminos
	{
		corpus=new Dictionary<string, LinkedList<Document>>();
		vectorizedDocs = new Dictionary<string, Vector>();
		corpusSize = documents.Length;
		QueryErrors = new LinkedList<string>();

		foreach(var doc in documents)
		{
			foreach(var term in doc.GetTerms())
			{
			   if(!corpus.ContainsKey(term))
			   {
				corpus.Add(term,new LinkedList<Document>());
			   }
			   corpus[term].AddLast(doc);
			}
		}

		foreach(var doc in documents)
		{
			vectorizedDocs.Add(doc.Name,VectorizeDoc(doc));			
		}

	}

	public string FixQuery(Query query)//repara la consulta en caso de haber algun error
	{
		if(QueryErrors.Count==0)return"";
		string retValue="";	   
		foreach(var term in query.GetTerms())
		{
			retValue+=" ";
			if(QueryErrors.Contains(term))
			{
			   retValue += GetSimilarWord(term);
			}else
			{
				retValue+=query.GetTerm(term).OriginalTerms()[0];	
			}
		}
		QueryErrors.Clear();
		return retValue;
	}

	private string GetSimilarWord(string word)//devuelve la palabra que sea mas parecida
						//a la que tenia error en la consulta
	{
		int minimal_distance=int.MaxValue;
		string bestWord="";
		foreach(var term in corpus.Keys)
		{
		 int dist =TextTreatment.TextProcessor.DistanceBetweenWords(word,term);
		 if(dist<minimal_distance)
		 {
			minimal_distance=dist;
			bestWord=term;

		 }
		}
		return corpus[bestWord].First.Value.GetTerm(bestWord).OriginalTerms()[0];
	}

	public IEnumerable<Document> GetSearchSpace(Query query)//reduce el espacio de busqueda
		//retorna un iterador que apunta a los documentos que tiene al menos los terminos 
		//de la consulta y los filtra por los operadores ^ y !
	{
		LinkedList<Document> docList = new LinkedList<Document>();
		foreach(var term in query.GetTerms())
		{		    			
			if(!corpus.ContainsKey(term))
			{
				QueryErrors.AddLast(term);
			}else{				
			if(IsForbiddenWord(query.GetOperatorWords("!"),term))continue;
			foreach(var doc in corpus[term])
			{
			    if(docList.Contains(doc))continue;
			if(ContainsWords(query.GetOperatorWords("!"),doc))continue;
				if(!SatisfyOperatosHas(query.GetOperatorWords("^"),doc))continue;
				docList.AddLast(doc);
			}
			}
		}

		return docList;
	}

	public Vector GetDocVector(string name)//obtiene el vector numerico del documento especificado
	{
		return vectorizedDocs[name];
	}

	private bool IsForbiddenWord(IEnumerable<string> words,string term)//determina si la palabra 
		//esta incluida en un iterador de palabras (lo uso para las palabras del operador !)
	{
		if(words==null)return false;
		return words.Contains(term);
	}

	private bool ContainsWords(IEnumerable<string> words,Document document)//retorna true si una 
		//palabra aparece en un documento
	{
		foreach(var word in words)
		{
			if(document.GetTermFrequency(word)>0)
			{
				return true;
			}
		}
		return false;
	}
	private bool SatisfyOperatosHas(IEnumerable<string> words,Document doc)//devuelve true si 
		//el documento satisface el operador ^
	{
		if(words==null)return true;
		foreach(var wrd in words)
		{
			if(!doc.ContainsWord(wrd))return false;
		}
		return true;
	}

	public Vector VectorizeDoc(BaseText text)//devuelve un vector numerico n-dimencional donde n
		//es la cantidad de terminos del vocabulario y en cada componente almacena el valor TF-IDF
		//de cada termino , recibe un objeto de tipo BaseText para poder usarlo con objetos tanto
		//de tipo document como de tipo Query
	{
		Vector ret_value= new Vector(corpus.Count);
		int index=0;
		foreach(var term in corpus)
		{
	ret_value[index++]=(float)((float)text.GetTermFrequency(term.Key)/(float)text.WordCount())*CalculateIdf(term.Key);//calculando pesos TF-IDF por cada termino
		}
		return ret_value;
	}

	private float CalculateIdf(string term)//calcula el IDF de un termino en el corpus 
	{		
		return (float)Math.Log10((float)(corpusSize/corpus[term].Count)+0.0005f);
	}

}
