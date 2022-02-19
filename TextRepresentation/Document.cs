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
		var route = path.Split(Path.PathSeparator);
		name = route[route.Length-1];	
	  StreamReader sr = new StreamReader(path);	
	  var lecture =sr.ReadToEnd();
		snippet = lecture.Length>100?lecture.Substring(0,100):lecture;
	  this.FillTerms(lecture);
	}

  	public string Name{get{return name;}}	
	public string Snippet{get{return snippet;}}

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
			int aux=Math.Abs(positions1[i]-positions2[i]);
		  if(aux< minDistance)
		  {
			minDistance=aux;
		  }
		}
		return minDistance;
	}
}
