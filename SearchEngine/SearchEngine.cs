using System.IO;
using TextRepresentation;
using System.Diagnostics;
namespace SearchEngine;
//clase que representa el motor de busqueda y sus funcionalidades
public class Searcher
{

	private static Searcher singleInstance =null;//instancia estatica para el singleton
	Vocabullary bagOfWords;//instancia de vocabulario para procesar las busquedas

	public static Searcher GetSingleInstance()//funcion para obtener una instancia de la clase
						//si ya esta creada , te devuelve esa instancia 
						//caso contrario crea una , la almacena y la devuelve
	{
		if(singleInstance==null)
		{
			singleInstance = new Searcher();
		}		

		return singleInstance;
	}

	private Searcher()//crea el Objeto , lee el directorio , crea las instancias de Document y 
			//Vocabullary 
	{
		var directory = Directory.GetFiles(Path.Join("../Content")); 
		Document[] docs = new Document[directory.Length];
		int loadedDocs=0;		
		var chrono = new Stopwatch();




		chrono.Start();

		for(int i=0;i<docs.Length;i++)//lee los directorios , y hace una pequena animacion 
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


	public IEnumerable<(string,string,float)> Search(ref string query)//Realiza la busqueda y 
							//en caso de haber errores en la consulta
							//los repara
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
		 if(distance==0)continue;
		 distance/=(float)GetMinDistance(queryObj,doc);
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

	private int GetMinDistance(Query queryObj,Document doc)//obtiene las distancias minimas de 
				//las palabras afectadas por el operador ~ en un documento
	{		
		var words = queryObj.GetOperatorWords("~");		
		int MinDistance = 1;		
		string ant="";
		foreach(var aux in words)
		{
			if(ant=="")ant=aux;
			else
			{
				 int dist = doc.GetMinDistance(aux,ant);
				 MinDistance*= dist>0?dist:1;
			}
			
		}		
	return MinDistance;
	}
	

}
