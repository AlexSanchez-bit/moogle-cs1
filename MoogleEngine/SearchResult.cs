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
	if(items.Length>=2){
	QuickSort(0,items.Length);
	}
    }



    private void QuickSort(int start,int end){
	int r=end-1,l=start;
	int pivot = start+((end-start)/2);
	if(l<r){
		while(l<r){
			while(r > start && items[r].Score<=items[pivot].Score )r--;
			while(l< end && items[l].Score>items[pivot].Score )l++;

			if(l<r){
			var aux = items[r];
			items[r]=items[l];
			items[l]=aux;
			}
		}

		QuickSort(start,pivot);
		QuickSort(pivot,end);
	}
    }

    public SearchResult() : this(new SearchItem[0]) {

    }

    public string Suggestion { get; private set; }

    public IEnumerable<SearchItem> Items() {
        return this.items.Where(elem=>elem.Score>0);
    }

    public int Count { get { return this.items.Length; } }
}
