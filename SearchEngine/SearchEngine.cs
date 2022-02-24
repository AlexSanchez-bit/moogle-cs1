using System.IO;
using TextRepresentation;
using System.Diagnostics;
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
		int loadedDocs=0;		
		var chrono = new Stopwatch();
		chrono.Start();

		for(int i=0;i<docs.Length;i++)
		{
			docs[i] = new Document(directory[i]);
			loadedDocs++;
			Console.Clear();
			Console.SetCursorPosition((Console.WindowWidth/2)-10,(Console.WindowHeight/2)-1);
			Console.Write("Cargando Documentos");
			for(int k=0;k<i%4;k++)
			{
				Console.Write(".");
			}
			Console.WriteLine();
			Console.SetCursorPosition((Console.WindowWidth/2)-10,Console.WindowHeight/2);
			Console.BackgroundColor = ConsoleColor.Green;
			Console.ForegroundColor = ConsoleColor.Red;
			float percentage = (float)loadedDocs/(float)docs.Length;
			Console.Write('[');
			for(int j =0;j<20;j++)			
			{
				if(j<(int)(percentage*20))Console.Write('*');
				else Console.Write(" ");
			
			}
			Console.Write(']');
			Console.BackgroundColor = ConsoleColor.Black;
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write(" "+(int)(percentage*100)+"%\n");

		}
		Console.WriteLine("Documentos Guardados");
				
		bagOfWords = new Vocabullary(docs); 
		Console.WriteLine("TF-IDF Calculados");
	        chrono.Stop();
		Console.WriteLine("Cargado en: {0} segundos",chrono.ElapsedMilliseconds/1000);
		}


	public IEnumerable<(string,string,float)> Search(ref string query)
	{
	 var queryObj = new Query(query);
	 var queryVector = bagOfWords.VectorizeDoc(queryObj);
	 var searchEspace = bagOfWords.GetSearchSpace(queryObj);
	 var lista = new LinkedList<(string,string,float)>();
	 var crono = new Stopwatch();
		crono.Start();
	 foreach(var doc in searchEspace)
	 {
		 var documentVector =bagOfWords.GetDocVector(doc.Name);		 
		 float distance = (documentVector*queryVector);
		 distance/=(float)GetMinDistance(queryObj,doc);
		 if(distance==0)continue;
		 var snippet = "< "+ distance+" >\n "+doc.Snippet(queryObj.GetTerms()) ;		
		lista.AddLast((doc.Name,snippet,distance));
	 }
		crono.Stop();
	Console.BackgroundColor = ConsoleColor.Green;
	Console.ForegroundColor = ConsoleColor.Black;
	Console.WriteLine("busqueda realizada en {0} milisegundos , {1} resultados Obtenidos",crono.ElapsedMilliseconds,lista.Count);
	Console.WriteLine();
		query = bagOfWords.FixQuery(queryObj);
	return lista;
	}

	private int GetMinDistance(Query queryObj,Document doc)
	{
		var words = queryObj.GetOperatorWords("~");		
		int MinDistance = int.MaxValue;		
		bool huboCambio=false;
		string ant="";
		foreach(var aux in words)
		{
			if(ant=="")ant=aux;
			else
			{
				int dist = doc.GetMinDistance(aux,ant);
				ant="";				
				if(dist>0 && dist<MinDistance)
				{
					MinDistance=dist;
					huboCambio=true;
				}
			}
			
		}
	return huboCambio?MinDistance:1;
	}
	

}
