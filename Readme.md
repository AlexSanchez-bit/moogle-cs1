# Moogle!

![](moogle.png)

> Proyecto de Programación I. Facultad de Matemática y Computación. Universidad de La Habana. Curso 2021.

Moogle es un buscador de Documentos , desarrollado en el lenguaje de programación C# , utilizando lo aprendido en el curso de programacion de la carrera Ciencias de la Computación curso 2021


# Flujo de Funcionamiento
#### Al iniciar el servidor , este crea una instancia de Searcher el cual carga los Documentos  y los procesa individualmente para obtener los datos relevantes sobre ellos , tales como su nombre, ruta y frecuencia de sus terminos , estos luego son utilizados para crear un vocabulario o `"bag of words"` que contiene todos los terminos del corpus textual (los documentos)  , con esto hecho se vectorizan los documentos calculando para cada termino su frecuencia inversa o IDF y se utiliza esta metrica para normalizar la frecuencia del termino en el documento (TF) y asi definir la relevancia de cada termino en el documento , estos vectores son almacenados para utilizarse en las busquedas .

#### Cuando un usuario realiza una consulta en la interfaz gráfica esta pasa al servidor donde se procesan los datos introducidos por el usuario tales como: separar los operadores procesar los terminos y calcular a cada uno de ellos sus frecuencias utilizando el vocabulario. Tras desarrollarlo se procede a reducir el espacio de búsqueda seleccionando los documentos que cumplan con los operadores `*` y `!` luego utilizando los vectores de los documentos previamente calculados y el de la consulta se procede a hacer el cálculo de la relevancia o score de cada documento respecto al query , esto se cubre utilizndo la distancia coseno entre vectores la cual es una medida de similitud entre vectores , este score se afecta en dependencia de los operadores `*` y `~` , luego de calcular todos los scores , se organiza la lista de documentos segun su score de mayor a menor y se devuelven al usuario , si hubo alguna palabra que no se pudo encontrar utilizando el algoritmo de Levengstein trato de encontrar alguna palabra que arregle la consulta


# Arquitectura básica
Este proyecto es una implementación del modelo vectorial de recuperación de la información , el cual consite en modelar los documentos y la consulta como vectores en un espacio donde cada componente del vector representa el grado de relevancia de un término , la dimension de este espacio sera igual a la cantidad de términos distintos que existan en el corpus de documentos. Para medir la relevancia se utilizó los pesos TF-IDF , TF significa Term Frequency o frecuencia del término en español y es la cantidad de veces que se repite un término dentro de un documento dado , IDF significa Inverse Document Frecuency o Frecuencia inversa de documento en español y se calcula con la siguiente fórmula Log10(n/d) , donde n es la cantidad total de documentos y d la cantidad de documentos es los que aparece el término , esto representa la relevancia del término en los documentos .Estas dos medidas de relevancia se multiplican y se guardan en las componentes de los vectores de los documentos y la consulta como medida final de la relevancia de cada término.

# Funcionalidades Implementadas
## Operadores
- '^' 
   ### Este operador filtra los resultados de la búsqueda y devulve solo los documentos que contengan la palabra que sigue al operador

- '!' 
    ### Este operador filtra los resultados de la búsqueda y devulve solo los documentos que no contengan la palabra que sigue al operador

- '~' 
    ### Este operador Aumenta en Score de los documentos en dependencia de la distancia entre las palabras que esten antes y despues del operador , dentro de los documentos , de forma tal que mientras mas cerca están dichas palabras mas alto es el Score del documento
	
- '*' 
   ### Este operador aumenta la relevancia de un término en la búsqueda , es acumulativo y funciona agregandole a la relevancia del término afectado la mayor relevancia de los terminos de la consulta y multiplicandola por la cantidad de * que tenga delante

## Snippet
 ### - Después de obtener los resultado de la búsqueda los documentos se representan en la página de moogle para que los resultados sean más claros , se calcula un snippet que contenga información sobre el documento que se obtuvo , para el cálculo del snippet , se busca una vecindad de alguna de las palabras de la consulta que contenga el documento y se representa junto con el nombre del mismo

## Sugerencias
 ##### Si durante la búsqueda hubo alguna palabra que no se pudo encontrar en ninguno de los documentos , a través del algoritmo de Levengstein se busca la palabra mas cercana a esta entre los términos del corpus para así dar una sugerencia de cuál podría ser la consulta real que quiso hacer el usuario

## Stemming o Reducción Morfológica
### Para reducir la cantidad de términos y el tamaño de los vectores se utilizó el Stemming el cual reduce las palabras a sus raíces morfológicas , esto lo hace eliminando los prefijos , sufijos etc

# Algoritmos utilizados 

## Algoritmo de Levengstein
### Este Algoritmo funciona calculando la cantidad mínima de operaciones para convertir una palabra en otra , esta medida de semejanza se le llama distancia de levengstein y las operaciones que se tienen en cuenta son la insercción de un caracter , la eliminación de un caracter y el reemplazamiento de un caracter , para este proyecto se utilizó la implementación de este algoritmo utilizando progrmación dinámica , tomando el costo de cada una de las operaciones como 1 , si se representa la operación de hallar la distancia de levengstein como una matriz , donde en cada fila-columna se asigna el minimo costo de reaizar alguna de las 3 operaciones ya mencionadas y construyendo la primera fila y columna de dicha matriz , podria llenarse el resto , llegando a la distancia mínima (cantidad mínima de operaciones) basado en este concepto funciona la implementación utilizada en este proyecto
## Algoritmo de Porter
### Anteriormente se mencionó la reducción morfológica , este algoritmo es el encargado de reducir las palabras a su raíz morfológica común o stem . este algoritmo funciona calculando 3 regiones en cada palabra , la primera región (r1) corresponde a la primera consonante precedida por una vocal , la segunda (r2) es la región asociada a la siguiente consonante que este precedida por una vocal y la última región (rv) equivale al tamanno de la palabra restandole la posición de la primera consonante antecedida por una vocal .Luego se hacen varios pasos en los cuales se analiza el contenido de estas regiones y se recalculan para así eliminar los prefijos y sufijos de la palabra que se esté analizando 



# Ingeniería de Software
Para mantener mi proyecto lo más encapsulado y fácil de mantener posible, abstraje la lógica de cada componente en distintas clases y distintos `classlibrary`
y mantener un orden en el proyecto
 ## ClassLibrarys:
 - `Algetool` : Contiene la lógica asociada a los cálculos algbraicos necesarios como calcular la norma de un vector y Hallar la distancia coseno entre dos vectores
 - `TextRepresentation`: Contiene la lógica asociada a la representación de los textos y su almacenamieto correcto para ser utilizados en el buscador
 - `TextTreatment`: abstrae de la lógica de procesar y tratar los textos 
 - `SearchEngine`: Contiene la lógica necesaria para hacer la búsqueda y une el resto de class librarys

# Clases creadas por mi:
## Cada clase se usa para abstraer de una logica distinta que al final se unen para hacer funcionar el proyecto
- ### clase TextRepresentation/WordInfo
    - **clase para almacenar metadatos de los terminos** 
    ```cs
        public class WordInfo
        {        
           public WordInfo(string originalWord ,int fila)//constructor , inicializa las listas y añade un dato a ellas  

           public int[] GetPositions()//devuelve las posiciones del termino
    
            public string[] OriginalTerms()//devuelve las palabras originales    

    		public void AddPos(string originalWord,int position)//agrega una posicion del término y su palabra original

      		public int GetFrequency()//devuelve la frecuencia del termino representado    
     	}

    ```
- ### clase TextRepresentation/BaseText
    - **abstrae la logica de leer y procesar texto** 
     ```cs
        public class BaseText 
		{	
	    protected BaseText() //constructor basico , inicializa las propiedades

	    protected BaseText(string text):this()//constructor que recibe el texto , llama al constructor basico y llena los campos de la clase	

	    public int WordCount()//retorna la cantidad de palabras asociadas al texto hhprocesado	

	    protected void FillTerms(string text)//Funcion para rellenar los terminos en el documento	


	    protected string[] GetTokens(string text)//retorna los tokens del texto (un array con sus palabras)	
	    private string ReduceText(string text)//elimina caracteres innecesarios	

	    public IEnumerable<string> GetTerms()//retorna los terminos 

	    public WordInfo GetTerm(string term)//permite obtener los metadatos de un termino especifico	
        public virtual  int GetTermFrequency(string term)

	    protected int GetFrequency(string term)//metodo para obtener la frecuencia de un termino 	
       }
    ```

- ### clase TextRepresentation/Document
    - **abstrae la logica propia de cada documento y guarda datos relevantes como su titulo ruta etc** 
     ```cs
	public class Document:BaseText		     
	{	
	public Document(string path):base()//constructor de la clase guarda datos de interes nombre del documento , ruta ,etc; y procesa el texto leido con los metodos heredados


  	public string Name{get{return name;}}//propiedades para obtener el nombre	
	public string Route{get{return path;}}//propiedad para obtener la ruta del documento

	public string Snippet(IEnumerable<string> terms)//metodo para 

	public int GetMinDistance(string term,string term2)//dadas dos palabras , clacula la distancia minima entre ellas
	

	public bool ContainsWord(string word)//retorna true si el documento contiene la palabra  false en caso contrario

	private int CalculateMinDistance(int[] positions1,int[] positions2)//calcula la distacia entre los array de posiciones de dos palabras	
      }

	```
- ### clase TextRepresentation/Query
    - **define la logica asociada a la consulta hecha por el usuario como procesar los operadores etc** 
	```cs
	public class Query:BaseText		   
	{
	
	public Query(string text):base()//Constructor de la clase			

    public IEnumerable<string> GetOperatorList()//obtiene los operadores del query 
		
	public IEnumerable<string> GetOperatorWords(string Operator)//retorna un iterador que apunta a las palabras asociadas a cada operador

	public override int GetTermFrequency(string term) //metodo para obtener la frecuencia y variarla en funcion del operador *

	private string RemoveOperators(string text)//elimina los operadores de la consulta original	y los almacena con sus respectivas listas de palabras afectadas
	
	private void ProcessOperator(string text,char op,ref int position)//procesa cada operador de manera individual										
	
    }

	```
- ### clase TextTreatment/Stemmer
  - **Implementacion sencilla del algoritmo de porter para la reduccion de palabras a sus raices comunes**
	```cs
	public static class Stemmer 
	{
		  public static string Stemize(string word) //devuelve el Stem o raiz comun de una palabra
	}
	``` 
- ### clase TextTreatment/TextProcessor
  - **procesador de textos para eliminar caracteres invalidos procesar numeros etc**
	```cs
	public static class TextProcessor
      {

	   public static IEnumerable<string> ProcessWords(string[] words)//devuelve un iterable de las palabras procesadas 	

	   public static string ProcessWord(string word)//dada una palabra cualquiera devuelve la palabra procesada	

	  public static int DistanceBetweenWords(string word1,string word2)//usando el algoritmo de levensgtein devuelve la medida de igualdad entre dos palabras	

	  private static string Normalize(string original)//lleva la palabra a minusculas y cambia las tildes y caracteres especificos del espannol

	  private static string RemoveSigns(string wrd)//elimina los simbolos raros y todo lo que no sea una letra
	

	  private static string ProcessNumbers(string word)//procesa los numeros fechas horas etc

     }
	```
- ### clase SearchEngine/Vocabullary
 - **Donde ocurre la magia, clase que procesa el corpus textual y asigna pesos a los terminos de cada documento**
     ```cs
	  public class Vocabullary
      {		   
	   public Vocabullary(Document[] documents)//constructor de la clase almacena los documentos 

	  public IEnumerable<Document> GetSearchSpace(Query query)//reduce el espacio de busqueda de la consulta y filtra los documentos por los operadores ^ y !
	
	  public Vector GetDocVector(string name)//obtiene el vector numerico del documento especificado

	  public Vector VectorizeDoc(BaseText text)//devuelve un vector numerico n-dimencional donde n es la cantidad de terminos del vocabulario y en cada componente almacena el valor TF-IDF  de cada termino 

     }
    ```
- ### clase SearchEngine/Searcher
  - **En esta clase se unen todos los componentes para realizar la busqueda y devolver el peso asociado a cada documento relacionado con la consulta**
    ```cs
	 public class Searcher
     {
	  public static Searcher GetSingleInstance()//funcion para obtener una instancia de la clase 

	    private Searcher()//constructor del objeto

	   public IEnumerable<(string,string,float)> Search(ref string query)//Realiza la busqueda y en caso de haber errores en la consulta los repara	
      } 
     ```
- ### clase Algetool/Vector
  - **Representacion de un vector matematico que contiene sus operaciones y propiedades**
    ```cs
	 public class Vector
     {
	  public Vector(float[] vector)//constructor

	  public static float CosDistance(Vector vec1, Vector vec2)//Calcular el coseno del angulo entre dos vectores

      private static Vector ScalarProduct(Vector v1, float scalar)//producto de un vector por un escalar

      private static float DotProduct(Vector vec1,Vector vect2)//producto punto entre vectores
      }

     ```






# Ejemplo del proyecto en funcionamiento

## ejemplo operador "^"
![](ejemploophas.png)
## ejemplo de la obtencion de una sugestion
![](ejemplosuggestion.png)
## ejemplo de una busqueda utilizando operadores
![](ejemplooperadores.png)

## Mostrando resultados y velocidad de la busqueda
![](ejemplobusquedas.png)

## Pantalla de carga al iniciar el servidor
![](ejemplocarga.png)


# Fuentes que utilicé

 - ##### Wikipedia
 - ##### StackOverflow
 - ##### Snowball.com
 - ##### Github
 - ##### Procesamiento de Texto y Modelo VectorialFelipe Bravo Márquez
- ##### Técnicas avanzadas derecuperación de informaciónProcesos, técnicas y métodosManuel Blázquez Ochando
