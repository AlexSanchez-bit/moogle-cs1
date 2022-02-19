using System.IO;
using TextRepresentation;
namespace SearchEngine;
public class Searcher
{

	private static Searcher singleInstance =null;
	Vocabullary bagOfWords;

	public static Searcher GetSingleInstance()
	{
		if(singleInstance==null)
		{
			singleInstance = new Searcher();
		}		

		return singleInstance;
	}

	private Searcher()
	{
		var directory = Directory.GetFiles(Path.Join("../Content")); 
		Document[] docs = new Document[directory.Length];
		for(int i=0;i<docs.Length;i++)
		{
			docs[i] = new Document(directory[i]);
		}
		Console.WriteLine("Documentos Guardados");
				
		bagOfWords = new Vocabullary(docs); 
		Console.WriteLine("TF-IDF Calculados");
		}


	public IEnumerable<(string,string,float)> Search(string query)
	{
	 var queryObj = new Query(query);
	 var queryVector = bagOfWords.VectorizeDoc(queryObj);
	 var searchEspace = bagOfWords.GetSearchSpace(queryObj);
	 var lista = new LinkedList<(string,string,float)>();

	 foreach(var doc in searchEspace)
	 {
		 var documentVector =bagOfWords.GetDocVector(doc.Name);		 
		 var snippet = doc.Snippet +" "+ (documentVector*queryVector);		

		lista.AddLast((doc.Name,snippet,documentVector*queryVector));
	 }
	return lista;
	}

	private int GetMinDistance(string word,Document doc,Query queryObj)
	{
	  int MinDistance = 0;  
	  return MinDistance;
	}
	

}
