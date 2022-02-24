namespace MoogleEngine;

public class SearchResult
{
    private SearchItem[] items;

    public SearchResult(SearchItem[] items, string suggestion="")
    {
        if (items == null) {
            throw new ArgumentNullException("items");
        }
        this.items = items;
        this.Suggestion = suggestion;
    }



    private void QuickSort(SearchItem[] items ,int start,int end){
	    if(start>=end)return;	    
	    float piv = items[start].Score;
	    int pos = Particiona(items,start,end,piv);
	    QuickSort(items,start,pos);
	    QuickSort(items,pos+1,end);
    }

    private int Particiona(SearchItem[] items,int start,int end,float pivot)
    {
	int i=start;
	int j=end-1;
	while(true)
	{
		while(items[i].Score > pivot)i++;
		while(items[j].Score < pivot)j--;
		if(i>=j)return j;
		var temp = items[i];
		items[i]=items[j];
		items[j]=temp;
	}

    }

    public SearchResult() : this(new SearchItem[0]) {

    }

    public string Suggestion { get; private set; }

    public IEnumerable<SearchItem> Items() {
	QuickSort(items,0,items.Length);
        return this.items;
    }

    public int Count { get { return this.items.Length; } }
}
