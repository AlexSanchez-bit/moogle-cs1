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
		 float distance = (documentVector*queryVector);
			distance/=GetMinDistance(queryObj,doc);
		 var snippet = doc.Snippet +" "+ distance ;		

		lista.AddLast((doc.Name,snippet,distance));
	 }
	return lista;
	}

	private int GetMinDistance(Query queryObj,Document doc)
	{
		var words = queryObj.GetOperatorWords("~");		
		int MinDistance = int.MaxValue;		
		string ant="";
		foreach(var aux in words)
		{
			if(ant=="")ant=aux;
			if(ant!="")
			{
				int dist = doc.GetMinDistance(aux,ant);
				ant="";				
				if(dist>0 && dist<MinDistance)MinDistance=dist;
			}
			
		}
	return MinDistance==int.MaxValue?1:MinDistance;
	}
	

}
