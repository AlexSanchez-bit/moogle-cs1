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



 ### Operadores de búsqueda
Para Obtener mejores resultados , moogle cuenta con una serie de operadores que facilitan la obtencion de informacion
`!` filtra los documentos para obtener solo aquellos que no tengan la palabra que sigue al operador
`^` permite obtener solo documentos que cumplan con la condicion de tener la palabra seguida por el operador
`~` aumenta el `score` o relevancia de un documento , mientras mas cercanas esten las palabras a sus laterales dentro del documento
`*` son acumulativos y aumentan la relevancia de un termino en especifico 






