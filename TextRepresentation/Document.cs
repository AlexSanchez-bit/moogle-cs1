using System.IO;
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
	  var tp1 = this.GetTerm(term).GetPositions();
	  var tp2 = this.GetTerm(term2).GetPositions();
          
	  return CalculateMinDistance(tp1,tp2);

	}

	public bool ContainsWord(string word)
	{
		
		var transformedWord=TextProcessor.ProcessWord(word);
		if(this.GetTerm(transformedWord)==null)return false;
		var info = this.GetTerm(transformedWord).OriginalTerms();
		foreach(var aux in info)
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
	    int minDistance=int.MaxValue;
	    for(int i=0;i< positions1.Length;i++)
	    {
		for(int j=0;j<positions2.Length;i++)
		{
			int dist = Math.Abs(positions2[i]-positions2[j]);
			if(dist<minDistance)
			{
			minDistance = dist;
			}
		}
	    }
	    return minDistance;
	}
}
