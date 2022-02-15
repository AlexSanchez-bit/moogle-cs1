using System.IO;
namespace SearchEngine;
public class SearchEngine
{

	private static SearchEngine singleInstance =null;


	public SearchEngine GetSingleInstance()
	{
		if(singleInstance==null)
		{
			singleInstance = new SearchEngine();
		}		

		return singleInstance;
	}

	private SearchEngine()
	{
		Directory.Di 
		    
	}

	

}
