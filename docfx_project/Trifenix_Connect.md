# Trifenix Connect
---

Trifenix connect tiene una arquitectura y metodología de desarrollo de sistemas, utilizando  la  [nube nativa](https://en.wikipedia.org/wiki/Cloud_native_computing) con base [devops](https://es.wikipedia.org/wiki/DevOps) y [scrum](https://es.wikipedia.org/wiki/Scrum_(desarrollo_de_software)), con el fin de desarollar  aplicaciones escalables en [low-code](https://en.wikipedia.org/wiki/Low-code_development_platform). 

# Principios de la Arquitectura
---
La arquitectura de Trifenix Connect presenta tres principios principales los cuales son:
 - Asincronia de datos.
 - Modelo de Metadatos.
 - Estructura única de documentos.

A continuacón se analizara cada principio con un ejemplo en el cual fue ejecutado.

# Asincronía de Datos
---

Las operaciones del sistema se envían de manera asincrónica, por lo que se puede visualizar en la siguiente imagen un panel de notificaciones para verificar si dicha operación tuvo éxito, esto permite que no exista una dependencia de la velocidad de la  base de datos para realizar los procesos de inserción, eliminacion o actualización.
![Asincrono](images/asincrono.gif)

# Modelo de Metadatos
---

Con el objetivo de unificar en un solo modelo de desarrollo de aplicaciones, basado en los metadatos de las entidades de la aplicacion con el fin de homogenizar la estructura del desarrollo.

A continuación se presenta un modelo, señalando la estructura del metadato:

![metadata](images/metadata.png)

# Estructura única de documentos
---

Modelo único de Documentos Entity Search.


# Estructura Trifenix
---

Trifenix tiene una estructura de empresa determinada por los siguientes enfoques:

- Automatización
- Gestión de proyectos
- Nube
- Interfaz de usuario

## Automatización

Todos las operaciones de automatización, Integración continua / Entrega continua son llevadas actualmente sobre [Azure Devops](https://dev.azure.com), es fundamental la automatizacion en los sistemas que se desarrollan para la velocidad de contrucción y el exito de estos.

Los proyectos que se desarrollan contienen scripts de automatización co

## Gestión de proyectos

Los proyectos se basan en metodologías agiles, tales como son scrum y Kanban, actualmente la gestion se desarrolla en [Azure Devops](https://dev.azure.com)  logrando el objetivo de dar segimiento de cada fase del ciclo de vida de cada una de las funcionalidades de la aplicación que se desarrolla.

## Nube

Actualmente se utiliza azure como motor principal de Trifenix Connect, Todas las operaciones de backen se desarrollan en C#.

## Interfaz de usuario (UX)

Actualmente nuestras interfaces son construidas en [React](https://es.wikipedia.org/wiki/React), sin embargo nuestro modelo es ampliable a otros frameworks UX .


# Velocidad de los datos (a buen costo)
---

La mayor parte de los programas que conocemos usan una base de datos llamada [SQL (Structured Query Language)](https://es.wikipedia.org/wiki/SQL) la cual hoy se encuentra tanto on  Premise, como en la nube, en el primer caso el costo de velocidad de estas bases de datos es muy elevado.  

El apogeo de las bases de datos SQL fue hace 30 años, pero ya hace 10 años las bases de datos documentales han tenido mayor popularidad por ser mas eficientes, con una mayor cantidad de opciones en su uso, permitiendo ser escalables sin alguna limitante. 

El modelo desarrollado por Trifenix Connect, también puede ser usado por base de datos SQL, pero nuestro enfoque son las documentales, donde actualmente utilizamos Azure Cosmo DB.

El principio de las bases de datos documentales es que son libres y se basan en una *Key* y un *Value*

Bueno, las bases de datos documentales son libres, su base fundamental se basa en un key y un value, normalmente el key será el id del elemento y el value será una estructura json con la entidad. 

El lenguaje de consultas utilizado por la base de datos depende bastante de la base de datos escogida, pero la mayoría tiene lenguajes de consultas que permiten llevar todos los filtros que se podrían imaginar.

# Aplicación de Base de datos 
---

La primera experiencia con base de datos documentales fue con [Azure CosmosDb](https://azure.microsoft.com/es-es/services/cosmos-db/), por medio de un plan de [año gratis de Cosmos DB](https://azure.microsoft.com/en-us/free/).

El beneficio de las bases de datos documentales es que son exactas, el plan gratis de Cosmo DB contiene 400 RU por segundo, donde 1 RU equivale a una lectura de 1 KB, 5 RU equivale a una escritura de un 1 KB.

Al inicio con las 400 RU que nos ofrecia este plan, nos parecia bien para realizar nuestras operaciones, pero con las diversas lecturas de datos  nos dimos cuenta que tarde o temprano, deberiamos cambiar de plan, ya que el gratis se limita a 400 RU, pero existen otros planes donde nos permiten no tener una limitante de los RU.

Al realizar las diversas pruebas de velocidad de lectura con Cosmo DB nos frustraba el nivel que mostraba, pero con **Azure Search** era distinto, ya que las operaciones que realizabamos era de forma instantanea y su lenguaje de consultas tenia un magnifico funcionamiento.

**Azure Search** es una base de datos documental, tal como lo es Cosmo DB, pero de una velocidad mayor a este, este tipo de bases de datos se puede reconocer en cualquier busquedor de alguna aplicación de rrss o delivery. La lectur ade datos siempre será más rápida, cuando exista indeación, la indexación se basa en una estructura definida.

Hablando de indexación, un índice es una estructura definida previamente, donde cada elemento insertado debe obedecer a esa estructura.

Por ejemplo
![estructura](images/estructura.png)

De acuerdo a las variables indicadas, se indexara cada campo, mientras más indexado sea un campo, más espacio ocuparará en la base de datos y más demorará en la inserción. 

Por ejemplo se podran insertar elementos como este:

```
"value": [
        {
            "@search.score": 1,
            "Nombre": "Bebida fantasía",
            "Updated": "2020-10-24T05:36:38.354Z",
            "Stock_Fisico": 24,
            "Precio": 800,
            "Super_Categoria": "ALIM",
            "Categoria": "BEBI",
            "Sub_Categoria": "BGAS",
            "Id": "BEBIDA"
        }
    ]
```

El modelo de una aplicacion tiene mas de una entidad, pero la versión gratuita nos limitaba a solo 3 entidades.

Anteriormente, ya nos habia ocurrido lo mismo con Cosmo Db, por lo que tuvimos que ocupar CosmoNaut para crear distintos tipos de documentos(entidad) y CosmoNaut usa el tipo de dato como partición, de esa manera nos permite usar solo una colección.

Pero Azure Search es diferente, se debe pagar si se ocupan mas de tres entidades, entonces si solo se utiliza el plan gratis se debe ocupar muy cuidadosamente los tres indices que te dan, por lo que siempre se tendra que crear objetos para lecturas paticulares, este proceso no es muy automizable.

Uno de los principios de Trifenix se basa en obtener la excelencia técnica con el menor costo posible, para replicar nuestra base de datos Cosmo DB en Azure Search, de tal manera que las inserciones no fueran nuestra preocupación, porque son asíncronas y en el caso de que sea mas rápido, solo sera necesario aumentar el plan de Cosmo DB para tener una mayor cantidad de RU en las operaciones que se desea ejecutar, también si el modelo se encunetra replicado en el Azure Search,las consultas tendrian un precio constante.

Nos planteamos un desafío que nacia de que nuestras operaciones de lecturas iban a una velociad lenta, sin la necesidad de aumentar los costos, ya que sabiamos que para mejorar la velocidad de lectura era necesario aumentar los costos, pero sabiamos que deberia haber otra forma de obtener lo mismo sin necesidad de aumentar los costos con el objetivo de tener un modelo lo mas eficiente, en donde la inserción para históricos y lectura en base de datos en alta velocidad.

Luego de mucho tiempo y de realizar varios prototipos para satisfacer nuestro objetivo, se desarrollo el modelo llamado **Trifenix IMes**, el que lleva consigo un modelo basado en metadatos con el nombre de **Trifenix Mdm**.


# IMes
---

![Imes](images/IMes.PNG)

El modelo **IMes** lleva consigo varios conceptos que se encuentran en su mismo nombre, estos conceptos son los siguiente: 

- Input
- Model
- EntitySearch

## Input 

Se refiere a las entradas de un sistema, una entidad que determina cuales son los valores ingresados por un usuario o máquina, en esta caso es un input de una temporada agricola. Un input debe heredar de InputBase, después veremos en detalle los atributos de cada propiedad, por ahora se debe entender que son parte del metadata model y permiten conectar los input, el model y los entitySearch, además de otros metadatos útiles dentro de **Trifenix Connect**.

```
[ReferenceSearchHeader(EntityRelated.SEASON)]
public class SeasonInput : InputBase {

    [Required]
    [DateSearch(DateRelated.START_DATE_SEASON)]
    public DateTime  StartDate { get; set; }

    [Required]
    [DateSearch(DateRelated.END_DATE_SEASON)]
    public DateTime EndDate { get; set; }

    [BoolSearch(BoolRelated.CURRENT)]
    public bool? Current { get; set; }

    [Required, Reference(typeof(CostCenter))]
    [ReferenceSearch(EntityRelated.COSTCENTER)]
    public string IdCostCenter { get; set; }

}
```

## Model

El modelo refiere a la entidad que será guardada en una base de datos, un input puede originar uno o mas entidades de base de datos. A continuación se puede ver la misma temporada, pero en este caso tiene un campo ClientId, que es un auto numérico, por tanto no se requiere en la entrada.

También se puede notar como algunos metadatos vinculan los mismos campos que el input, a diferencia del input, esta clase usa un atributo de Cosmonaut, que lo identifica como entidad de una base de datos.

```
[SharedCosmosCollection("agro", "Season")]
[ReferenceSearchHeader(EntityRelated.SEASON, PathName = "seasons", Kind = EntityKind.CUSTOM_ENTITY)]
public class Season : DocumentBase, ISharedCosmosEntity {

    /// <summary>
    /// identificador.
    /// </summary>
    public override string Id { get; set; }

    /// <summary>
    /// Autonumérico del identificador del cliente.
    /// </summary>
    [AutoNumericSearch(StringRelated.GENERIC_CORRELATIVE)]
    public override string ClientId { get; set; }


    /// <summary>
    /// fecha de inicio
    /// </summary>
    [DateSearch(DateRelated.START_DATE_SEASON)]
    public DateTime StartDate { get; set; }


    /// <summary>
    /// fecha fin
    /// </summary>
    [DateSearch(DateRelated.END_DATE_SEASON)]
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Identifica si el agricola es el actual.
    /// </summary>
    [BoolSearch(BoolRelated.CURRENT)]
    public bool Current { get; set; }

    /// <summary>
    /// identificador del costcenter.
    /// </summary>
    [ReferenceSearch(EntityRelated.COSTCENTER)]
    public string IdCostCenter { get; set; }

}
```

## EntitySearch

Un entitySearch es una estructura definida para todo tipo de entidades, como se puedo apreciar en los ejemplos de input y model, cada propiedad fué identificada con un valor en su atributo, por ejemplo Current, encima de esta propiedad se encuentra el atributo BoolSearch en el que se indica dentro de un diccionario cual es el índice que representa a ese campo, en este caso BoolRelated.CURRENT que es igual a 0 (es el primero en el diccionario). Asi también en StarDate, EndDate  e IdCostCenter, se podrá dar cuenta que el atributo determina el tipo de dato y el índice al que pertenece.

A través de estos índices otorgados por el metadata model se puede transformar un objeto a un entitySearch y un entitySearch en un objeto. 

Un entitySearch es una estructura que puede albergar cualquier modelo, para esto debe tener una estrucutura definida, con el fin de que cada objeto sea convertido a un entitySearch y almacenado en el índice.

El inconveniente que presenta este tipo de modelo, es que siempre existirá un tamaño mínimo, porque la estructura se mantiene, tenga datos o no, por eso se ha hecho el esfuerzo en hacerla lo más simplicado posible para que ocupe el menor espacio, el beneficio es que el entitySearch tiene previamente asignado los índices, por tanto no habrá trabajo manual posterior. 

Antes de pasar a la explicación de las propiedades de un entitySearch, debemos conocer que es un facet, dentro de las bases de datos de busqueda.

![facet](images/facet.png)

Como lo muestra la imágen son agrupaciones asociadas a una busqueda en partícular, por ejemplo, los facets de una consulta de productos, podrían ser las el listado de marcas encontradas en la consulta, otro facet puede ser el tipo de frutas al que puede ser aplicado, etc.

Las propiedads de EntitySearch son las siguientes: 

- Id
- Index
- Created
- Rel

## Id

El identificador de un elemento, todo elemento en entitySearch tiene un identificador.

## Index

Siempre una entidad de base de datos tendrá un índice asignado por el modelo de metadatos, este índice será asociado a un diccionario que determinará la clase vinculada.
Por ejemplo, la entidad producto podría tener el índice 22.

## Created

Este campo identifica la fecha de creación de un elemento.

## Rel 

Un entitySearch es un grafo que se relaciona con otras entidades a través de esta propiedad, esta propiedad es una colección por tanto, un elemento se puede relacionar con 0 o más.
Una relación se compone de los siguientes campos.





### id
identificador del elemento relacionado a esta entidad

### index
índice del elemento relacionado, recordemos que todas las entidades de la base de datos están asociadas con un índice.

### facet
el facet es un campo de tipo string que tiene el índice "," el id del elemento, generando un valor como este "1,9aebaf15-eb85-49d7-acca-643329d4078b" 
Cuando hagamos una consulta podremos usar esta propiedad como facet, se agruparán los resultados por tipos de valor de esta propiedad, por ejemplo, digamos que queremos las marcas
en una consulta de procutos y las marcas tienen el índice 12, y digamos que dentro de la busqueda, se encontraron 2 marcas, la primera con 3 productos y la segunda con 7, como lo que sigue:

```
[
    {"12,9aebaf15-eb85-49d7-acca-643329d4078b":3},
    {"12,7990893f-74e1-45d6-8f3d-af1c9896842c":7},
]
```
Al asignar este campo como facet, poniendo el índice y el id, se puede identificar las dos marcas y el número de registros encontrados.