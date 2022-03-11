using System.IO;
using TextTreatment;
namespace TextRepresentation;
public class Document:BaseText
		      //hereda de BaseText donde se encuentra la logica de procesamiento de texto
		      //la clase Document representa un documento individual 
{
	private string name;
	private string path;

	public Document(string path):base()//constructor de la clase guarda datos de interes
				     	  //nombre del documento , ruta ,etc;
					  //y procesa el texto leido con los metodos heredados
	{
		var route = path.Split("/");
		this.path=path;
		name = route[route.Length-1];	
	  StreamReader sr = new StreamReader(path);	
	  var lecture =sr.ReadToEnd();
	  this.FillTerms(lecture);
	}

  	public string Name{get{return name;}}//propiedades para obtener el nombre	
	public string Route{get{return path;}}//propiedad para obtener la ruta del documento

	public string Snippet(IEnumerable<string> terms)//metodo para obtener el snippet
	{
		StreamReader str = new StreamReader(this.path);
		string lecture = str.ReadToEnd();
		if(lecture.Length<40) return lecture;
		var processlecture = GetTokens(lecture);
		int position=0;
		foreach(var term in terms)
		{
			if(this.GetFrequency(term)>0)
			{								
				position=Array.IndexOf(processlecture,GetTerm(term).OriginalTerms()[0]);
				break;
			}
		}
		string snippet ="";
		for(int i =-10; (position+i) < processlecture.Length && i<20 ;i++)
		{
			if((position+i)<=0)continue;
		  snippet+=processlecture[position+i]+" ";
		}
		return snippet;
	}

	public int GetMinDistance(string term,string term2)//dadas dos palabras , clacula 
							//la distancia minima entre ellas
	{
	  var tp1 = this.GetTerm(term);
	  var tp2 = this.GetTerm(term2);
         	if(tp1==null || tp2==null)return -1; 
	  return CalculateMinDistance(tp1.GetPositions(),tp2.GetPositions());

	}

	public bool ContainsWord(string word)//retorna true si el documento contiene la palabra 
					//, false en caso contrario
	{
		
		var transformedWord=TextProcessor.ProcessWord(word);
		var info = this.GetTerm(transformedWord);
		if(info==null)return false;
		foreach(var aux in info.OriginalTerms())
		{
			if(aux==word)
			{
			  return true;				
			}

		}
return false;

	}

	private int CalculateMinDistance(int[] positions1,int[] positions2)//calcula la distacia entre 
									//los array de posiciones de dos
									//palabras
	{
		int interestTerms = Math.Min(positions1.Length,positions2.Length);
		int minDistance=int.MaxValue;
		
		if(interestTerms==1)return(int) Math.Abs(positions1[0]-positions2[0]);

		for(int i=1;i<interestTerms;i++)
		{
			int aux =(int) Math.Abs(positions1[i]-positions2[i-1]);
		  if(aux<minDistance)
		  {
			minDistance=aux;
		  }
		}
		return minDistance;
	}
}
