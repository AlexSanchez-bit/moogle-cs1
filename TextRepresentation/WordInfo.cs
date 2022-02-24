namespace TextRepresentation;
public class WordInfo
//clase para representar a un termino y sus metadatos
{
    private LinkedList<int> indexedPositions;//lista de posiciones donde aparece el termino
	private LinkedList<string> indexedWords;//lista de las palabras originales del termino



    public WordInfo(string originalWord ,int fila)//constrictor , inicializa las listas
	    					//y annade un dato a ellas
    {
	indexedWords = new LinkedList<string>();
	indexedPositions = new LinkedList<int>();
	AddPos(originalWord,fila);
    }

    public int[] GetPositions()//devuelve las posiciones del termino
    {
	    int[] positions=new int[indexedPositions.Count];
	    int index=0;
	    foreach(var position in indexedPositions)
	    {
		    positions[index++]=position;
	    }
	    return positions;
    }

    public string[] OriginalTerms()//devuelve las palabras originales
    {
	    string[] terms=new string[indexedWords.Count];
	    int index=0;
	    foreach(var term in indexedWords)
	    {
		    terms[index++]=term;
	    }
	    return terms;
    }

    public void AddPos(string originalWord,int position)//agrega una posicion del termino 
	    						//y su palabra original
    {
	indexedPositions.AddLast(position);
	if(!indexedWords.Contains(originalWord))
	{
		indexedWords.AddLast(originalWord);		
	}
    }

    public int GetFrequency()//devuelve la frecuencia del termino representado
    {
	return indexedPositions.Count;
    }

}
