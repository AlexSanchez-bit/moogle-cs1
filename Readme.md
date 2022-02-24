# Moogle!

![](moogle.png)

> Proyecto de Programación I. Facultad de Matemática y Computación. Universidad de La Habana. Curso 2021.

Moogle es un buscador de Documentos , desarrollado en el lenguaje de programacion c# , utilizando lo aprendido en el curso de programacion de la carrera Ciencias de la Computación 2021

## Como se estructura el Proyecto?
- `MoogleServer` es un servidor web que renderiza la interfaz gráfica y sirve los resultados.
- `MoogleEngine` es una biblioteca de clases donde esta la logica asociada a la busqueda
- `TextRepresentation` es una libreria de clases donde se encuentra la logica asociada al almacenamiento del texto 
- `TextTretment` como su nombre indica esta libreria de clases contiene la logica asociada al tratamiento del texto
- `SearchEngine` libreria de clases asociada a procesar las busquedas 


 ## Sobre la búsqueda
La busqueda se realiza introduciendo una frase o palabra a buscar en el campo de texto de la pagina principal de moogle .Para realizar las busquedas he utilizado `el modelo vectorial` de recuperacion de la informacion el cual consiste en crear un espacio vectorial donde cada dimension corresponde a un termino del `corpus` textual , midiendo la relevancia de cada `termino` en dicho corpus , con esto ya creado se puede trarar cada documento como un vector n-dimensional , un proceso parecido se le aplica a la busqueda , para luego , aplicando una medida de similitud entre estos vectores , encontrar aquellos documentos que son relevantes respecto a la busqueda.
 - En general un usuario puede buscar no solo una palabra sino una frase cualquiera
 - Los Documentos devueltos no tiene que tener todas las palabras , pero su `score` disminuye mientras menos palabras tenga
 - Si dos documentos tienen el mismo score pero uno tiene una palabra mas 'importante' que otra (mas rara) , este es devuelto con un score mayor
 - Las palabras demasiado comunes como las preposiciones conjunciones etc son ignoradas y no aportan relevancia al score del documento


 ### Operadores de búsqueda
Para Obtener mejores resultados , moogle cuenta con una serie de operadores que facilitan la obtencion de informacion
`!` filtra los documentos para obtener solo aquellos que **no tengan la palabra que sigue** al operador
`^` permite obtener solo documentos que cumplan con la condicion de **tener la palabra seguida por el operador**
`~` aumenta el `score` o relevancia de un documento , mientras **mas cercanas** esten las palabras a sus laterales dentro del documento
`*` son acumulativos y aumentan la relevancia de un termino en especifico 

###Evaluacion del Score
Para evaluar el `Score` utilizo el **TF-IDF** apoyado por el **modelo vectorial** de recuperacion de la informacion
Sabiendo cuantas veces se repite una palabra dentro del documento ( Frequency Term o TF) puedo saber su relevancia dentro de ese documento , luego 
teniendo calculadas las frecuencias de termino (TF) en todos los terminos de los documentos que forman el corpus textual calculo el IDF o frecuencia inversa
para determinar la relevancia de cada termino en el corpus general , con todo esto armo un vocabulario que contiene todas las palabras de los documentos
y convierto estos a vectores de R*n* donde n es la cantidad de terminos entre los documentos , Haciendo este mismo proceso a la Query o consulta del usuario
obtengo 2 vectores de Rn (el de Query y el de el Documento i-esimo) a los cuales se les puede hallar la distancia coseno que me permite determinar el Coseno del Angulo entre dos Vectores del mismo espacio , mientras mayor es el valor obtenido de la distancia coseno , menor es el angulo entre dichos vectores en su espacio 
,por tanto estan mas cerca y el Query es mas relevante a la busqueda

###Algoritmo de Busqueda 
Para ejecutar la busqueda , una vez cargados los documentos y calculados los valores de TF-IDF de cada termino en cada documento
basta con filtrar los documentos en aquellos que cumplan las condiciones de tener al menos un termino en comun con la consulta del usuario
y que cumplan con los operadores `^` y `!` para asi no verificar documentos innecesarios y evitar exceso de calculos , hecho esto convierto la query en un vector tomando en cuenta los operadores `*` para determinar la relevancia de cada termino en la query , y convierto esta en un vector bajo la misma norma que los documentos (haciendo las mismas operaciones) , luego calculo la distancia coseno entre cada vector de mi espacio de busqueda ya filtrado y el query para obtener su `score` , estos resultados son ordenados antes de volver a la interfaz desde la cual el usuario hizo su consulta originalmente

###Ingenieria de Software
Para mantener mi proyecto lo mas encapsulado y facil de mantener posible , abstraje la logica de cada componente en distintas clases y distintos `classlibrary`
para mantener un orden en el proyecto
 ##ClassLibrarys:
 - `Algetool` : Contiene la logica asociada a los calculos algbraicos necesarios como calcular la norma de un vector Hallar la distancia coseno entre dos vectores
 - `TextRepresentation`: Contiene la logica asociada a la representacion de los textos y su almacenamieto correcto para ser utilizados en el buscador
 - `TextTreatment`: abstrae de la logica de procesar y tratar los textos 
 - `SearchEngine`: Contiene la logica necesara para hacer la busqueda y depende de el resto de class librarys

##Clases:
-Cada clase se usa para abstraer de una logica distinta que al final se unen para hacer funcionar el proyecro
