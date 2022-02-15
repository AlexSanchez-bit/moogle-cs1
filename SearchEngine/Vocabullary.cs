using TextRepresentation;
namespace SearchEngine;
public class Vocabullary
{
	private Dictionary<string ,LinkedList<Document>> corpus;
	private Dictionary<string,Vector> vectorizedDocs;
	private int corpusSize;


	public Vocabullary(Document[] documents)
	{
		corpus=new Dictionary<string, LinkedList<Document>>();
		vectorizedDocs = new Dictionary<string, Vector>();

		foreach(var doc in documents)
		{
			foreach(var term in documents.GetTerms())
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

	public IEnumerable<Document> GetSearchSpace(Query query)
	{
		LinkedList<Document> docList = new LinkedList<Document>();
		foreach(var term in query.GetTerms)
		{		    			
			if(ContainsWord(query.GetOperatorWords("!"),term))continue;
			foreach(var doc in corpus[term])
			{
				if(!SatisfyOperatosHas(query.GetOperatorWords("^"),doc))continue;
			    if(!docList.Contains(doc))
			    {
				docList.AddLast(doc);
			    }
			}
		}

		return docList;
	}


	

	
	private bool ContainsWord(IEnumerable<string> words,string term)
	{
		return words.Contains(term);
	}
	private bool SatisfyOperatosHas(IEnumerable<string> words,Document doc)
	{
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
			ret_value[index++]=(float)text.GetTermFrequency(term)*CalculateIdf(term);
		}
		return ret_value;
	}

	private float CalculateIdf(string term)
	{
		return (float)Math.Log10((float)corpusSize/(float)corpus[term].Count);
	}

}
