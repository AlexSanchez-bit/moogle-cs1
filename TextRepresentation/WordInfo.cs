namespace TextRepresentation;
public class WordInfo
{
    private LinkedList<int> indexedPositions;
	private LinkedList<string> indexedWords;



    public WordInfo(string originalWord ,int fila)
    {
	indexedWords = new LinkedList<string>();
	indexedPositions = new LinkedList<int>();
	AddPos(originalWord,fila);
    }

    public int[] GetPositions()
    {
	    int[] positions=new int[indexedPositions.Count];
	    int index=0;
	    foreach(var position in indexedPositions)
	    {
		    positions[index++]=position;
	    }
	    return positions;
    }

    public string[] OriginalTerms()
    {
	    string[] terms=new string[indexedWords.Count];
	    int index=0;
	    foreach(var term in indexedWords)
	    {
		    terms[index++]=term;
	    }
	    return terms;
    }

    public void AddPos(string originalWord,int position)
    {
	indexedPositions.AddLast(position);
	if(!indexedWords.Contains(originalWord))
	{
		indexedWords.AddLast(originalWord);		
	}
    }

    public int GetFrequency()
    {
	return indexedPositions.Count;
    }

}
