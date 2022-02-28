namespace TextTreatment;
public static class TextProcessor
{
	public static bool Stemin=true;

	public static IEnumerable<string> ProcessWords(string[] words)//devuelve un iterable de las palabras procesadas 
	{
	 return words.Select(elem=>{
		return ProcessWord(elem);
			 }); 
	}

	private static string GetStem(string Original)//obtiene el Stem o raiz comun de una palabra
	{
	    return Stemmer.Stemize(Original);
	}

	public static string ProcessWord(string word)//dada una palabra cualquiera devuelve la palabra procesada
	{
		if(Stemin)
		return GetStem(RemoveSigns(ProcessNumbers(Normalize(word))));

		return (RemoveSigns(ProcessNumbers(Normalize(word))));

	}	

	public static int DistanceBetweenWords(string word1,string word2)//usando el algoritmo de levensgtein devuelve la medida de igualdad entre dos palabras
	{
		int n= word1.Length;
		int m= word2.Length;

		int[,] dp = new int[n+1,m+1];

		for(int i=0;i<=n;i++)
		{
			dp[i,0]=i;
		}

		for(int j=0;j<=m;j++)
		{
		  dp[0,j]=j;
		}

		for(int i=1;i<=n;i++)
		{
		  for(int j=1;j<=m;j++)
		  {
			if(word1[i-1]==word2[j-1])
			{
			  dp[i,j]=dp[i-1,j-1];
			}else{
			  int replaceCost=1+dp[i-1,j-1];
			  int insertionCost=1+dp[i,j-1];
			  int deletCost=1+dp[i-1,j];

			  dp[i,j] = Math.Min(replaceCost,Math.Min(insertionCost,deletCost));
			}
		  }
		}
	return dp[n,m];
	}



	private static string Normalize(string original)//lleva la palabra a minusculas y cambia las tildes y caracteres especificos del espannol
	{
		return original
			.ToLower()
			.Replace('á','a')
			.Replace('é','e')
			.Replace('í','i')
			.Replace('ó','o')
			.Replace('ú','u')
			.Replace('ñ','n');

	}

	private static string RemoveSigns(string wrd)//elimina los simbolos raros y todo lo que no sea una letra
	{
		string retstring ="";
		foreach(char a in wrd)
		{
			if(char.IsLetter(a))
			{
				retstring+=a;
			}
		}
return retstring;
	}

	private static string ProcessNumbers(string word)//procesa los numeros codigos fechas etc
	{		
            int numberPosition=0;
	    if(word.Length==0)
	    {
		return word;
	    }
	    while(numberPosition<word.Length && char.IsNumber(word[numberPosition]))numberPosition++;	    
	    if(numberPosition>=word.Length)
	    {
		return "NUMBER";
	    }
	    else if(word[numberPosition]==':')
	    {
		return "TIME";
	    }else if(word[numberPosition]=='.')
	    {
			return "CODE";
	     }
	else if(word[numberPosition]=='/')
	    {
		return "DATE";
	    }
	    else
	    {
		return word;
	    }
	} 

}
