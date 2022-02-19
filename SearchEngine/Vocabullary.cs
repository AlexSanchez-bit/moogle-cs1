using TextRepresentation;
using AlgeTool;
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
		corpusSize = documents.Length;

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

	public IEnumerable<Document> GetSearchSpace(Query query)
	{
		LinkedList<Document> docList = new LinkedList<Document>();
		foreach(var term in query.GetTerms())
		{		    			
			Console.WriteLine("["+term+"]");
			if(!corpus.ContainsKey(term))continue;
			if(IsForbiddenWord(query.GetOperatorWords("!"),term))continue;
			foreach(var doc in corpus[term])
			{
			if(ContainsWords(query.GetOperatorWords("!"),doc))continue;
				if(!SatisfyOperatosHas(query.GetOperatorWords("^"),doc))continue;
			    if(!docList.Contains(doc))
			    {
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
			ret_value[index++]=(float)text.GetTermFrequency(term.Key)*CalculateIdf(term.Key);
		}
		return ret_value;
	}

	private float CalculateIdf(string term)
	{
		return (float)Math.Log10((float)(corpusSize/corpus[term].Count));
	}

}
