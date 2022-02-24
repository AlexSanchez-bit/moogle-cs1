using TextRepresentation;
using AlgeTool;
namespace SearchEngine;
public class Vocabullary
{
	private Dictionary<string ,LinkedList<Document>> corpus; //corpus de documentos
	private Dictionary<string,Vector> vectorizedDocs;//vectores de cada documento indexados por su nombre
	private int corpusSize;//tamanno del corpus(cantidad de documentos)
	private LinkedList<string> QueryErrors;//palabras del query a arreglar


	public Vocabullary(Document[] documents)
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

	public string FixQuery(Query query)
	{
	   
	}

	public IEnumerable<Document> GetSearchSpace(Query query)
	{
		LinkedList<Document> docList = new LinkedList<Document>();
		foreach(var term in query.GetTerms())
		{		    			
			if(!corpus.ContainsKey(term))
			{

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

	public Vector GetDocVector(string name)
	{
		return vectorizedDocs[name];
	}

	private bool IsForbiddenWord(IEnumerable<string> words,string term)
	{
		if(words==null)return false;
		return words.Contains(term);
	}

	private bool ContainsWords(IEnumerable<string> words,Document document)
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
	private bool SatisfyOperatosHas(IEnumerable<string> words,Document doc)
	{
		if(words==null)return true;
		foreach(var wrd in words)
		{
			if(!doc.ContainsWord(wrd))return false;
		}
		return true;
	}

	public Vector VectorizeDoc(BaseText text)
	{
		Vector ret_value= new Vector(corpus.Count);
		int index=0;
		foreach(var term in corpus)
		{
	ret_value[index++]=(float)((float)text.GetTermFrequency(term.Key)/(float)text.WordCount())*CalculateIdf(term.Key)+0.0005f;//calculando pesos TF-IDF y agregando 0.0005 para evitar tener muchos 0
		}
		return ret_value;
	}

	private float CalculateIdf(string term)
	{
		return (float)Math.Log10((float)(corpusSize/corpus[term].Count));
	}

}
