using System.IO;
using TextTreatment;
namespace TextRepresentation;
public class Document:BaseText
{
	private string name;
	private string snippet;
	private string path;

	public Document(string path):base()
	{
		var route = path.Split("/");
		this.path=path;
		name = route[route.Length-1];	
	  StreamReader sr = new StreamReader(path);	
	  var lecture =sr.ReadToEnd();
	  this.FillTerms(lecture);
	}

  	public string Name{get{return name;}}	
	public string Route{get{return path;}}
	public string Snippet(IEnumerable<string> terms)
	{
		StreamReader str = new StreamReader(this.path);
		string lecture = str.ReadToEnd();
		var processlecture = GetTokens(lecture);
		int position=0;
		foreach(var term in terms)
		{
			if(this.GetFrequency(term)>0)
			{
				position=GetTerm(term).GetPositions()[0];
				break;
			}
		}
		string snippet ="";
		for(int i = -20; position+i<processlecture.Length && i<20;i++)
		{
			if(position+i<=0)continue;
		  snippet+=processlecture[position+i]+" ";
		}
		return snippet;
	}

	public int GetMinDistance(string term,string term2)
	{
	  var tp1 = this.GetTerm(term);
	  var tp2 = this.GetTerm(term2);
         	if(tp1==null || tp2==null)return -1; 
	  return CalculateMinDistance(tp1.GetPositions(),tp2.GetPositions());

	}

	public bool ContainsWord(string word)
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

	private int CalculateMinDistance(int[] positions1,int[] positions2)
	{
		int interestTerms = Math.Min(positions1.Length,positions2.Length);
		int minDistance=int.MaxValue;
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
