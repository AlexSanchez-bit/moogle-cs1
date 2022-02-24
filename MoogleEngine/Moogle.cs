using SearchEngine;
namespace MoogleEngine;


public static class Moogle
{
    public static SearchResult Query(string query) {
	var searcher = Searcher.GetSingleInstance(); 
	var results = searcher.Search(ref query);
	SearchItem[] items = new SearchItem[results.Count()];
	int index =0;
	foreach(var a in results)
	{
		items[index++] = new SearchItem(a.Item1,a.Item2,a.Item3);
	}
        return new SearchResult(items, query);
    }
}
