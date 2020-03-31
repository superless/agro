using System.ComponentModel;

namespace trifenix.agro.enums.searchModel {
    public enum Related {
        REFERENCE = 0,
        LOCAL_REFERENCE = 1,
        STR = 2,
        SUGGESTION = 3,
        NUM64 = 4,
        NUM32 = 5,
        DBL = 6,
        BOOL = 7,
        GEO = 8,
        ENUM = 9,
        DATE = 10
    }


    public enum EntityRelated {

        [Description("Espera Cosecha")]
        WAITINGHARVEST = 0,

        [Description("Cuartel")]
        BARRACK = 1,

        [Description("Unidad de Negocios")]
        BUSINESSNAME = 2,

        [Description("Categoria Ingrediente")]
        CATEGORY_INGREDIENT = 3,

        [Description("Entidad Certificadora")]
        CERTIFIED_ENTITY = 4,


        [Description("Centro de Costos")]
        COSTCENTER = 5,

        [Description("Dosis")]
        DOSES = 6,

        [Description("Ingrediente")]
        INGREDIENT = 7,

        [Description("Trabajo")]
        JOB = 8,

        [Description("Nebulizadora")]
        NEBULIZER = 9,

        [Description("Evento Fenológico")]
        PHENOLOGICAL_EVENT = 10,

        [Description("Parcela")]
        PLOTLAND = 11,

        [Description("Producto")]
        PRODUCT = 12,

        [Description("Rol")]
        ROLE = 13,

        [Description("Raíz")]
        ROOTSTOCK = 14,

        [Description("Temporada")]
        SEASON = 15,

        [Description("Sector")]
        SECTOR = 16,

        [Description("Pre-Orden")]
        PREORDER = 17,

        [Description("Objetivo Aplicación")]
        TARGET = 18,

        [Description("Tractor")]
        TRACTOR = 19,

        [Description("Usuario")]
        USER = 20,

        [Description("Variedad")]
        VARIETY = 21,  // el entity index

        [Description("Notificación de Evento")]
        NOTIFICATION_EVENT = 22,

        [Description("Polinizador")]
        POLLINATOR = 23,

        [Description("Carpeta de PreOrdenes")]
        ORDER_FOLDER = 24,

        [Description("Orden de ejecución")]
        EXECUTION_ORDER = 25,

        [Description("Orden")]
        ORDER = 26,

        [Description("Evento de Cuartel")]
        BARRACK_EVENT = 27,

        [Description("Dosis en Orden")]
        DOSES_ORDER = 28,

        [Description("Estatus de Orden de ejecución")]
        EXECUTION_ORDER_STATUS = 29,

        [Description("Especie")]
        SPECIE = 30,

        [Description("Ubicación Geográfica")]
        GEOPOINT = 31
    }

    public enum StringRelated {

        [Description("Identificador Azure Active Directory")]
        OBJECTID_AAD = 0,

        [Description("Abreviación")]
        GENERIC_ABBREVIATION = 1,

        [Description("Código")]
        GENERIC_CODE = 2,

        [Description("Estatus de Orden de ejecución")]
        GENERIC_RUT = 3,

        [Description("Nombre")]
        GENERIC_NAME = 4,

        [Description("Email")]
        GENERIC_EMAIL = 5,

        [Description("Marca")]
        GENERIC_BRAND = 6,

        [Description("Página Web")]
        GENERIC_WEBPAGE = 7,

        [Description("giro")]
        GENERIC_GIRO = 8,

        [Description("Teléfono")]
        GENERIC_PHONE = 9,

        [Description("Imagen")]
        GENERIC_PICTURE_PATH = 10,

        [Description("Descripción")]
        GENERIC_DESC = 11,

        [Description("Comentario")]
        GENERIC_COMMENT = 12,
    }

    public enum NumRelated {
        [Description("Días de espera")]
        WAITING_DAYS = 0,

        [Description("Número Correlativo")]
        GENERIC_CORRELATIVE = 1,

        [Description("Horas de reingreso")]
        HOURS_TO_ENTRY = 2,

        [Description("Intervalo de días")]
        DAYS_INTERVAL = 3,

        [Description("Número de Repetición")]
        NUMBER_OF_SECQUENTIAL_APPLICATION = 4,

        [Description("Mojado Recomendado")]
        WETTING_RECOMMENDED = 5,
        [Description("Número de Plantas")]
        NUMBER_OF_PLANTS = 6,

        [Description("Año de plantación")]
        PLANTING_YEAR = 7
    }

    public enum DoubleRelated{

        [Description("Cantidad")]
        QUANTITY_CONTAINER = 0,

        [Description("Partes por Millón (PPM)")]
        PPM = 1,

        [Description("Mojado")]
        WETTING = 2,

        [Description("Cantidad Aplicada")]
        QUANTITY_APPLIED = 3,

        [Description("Cantidad Mínima")]
        QUANTITY_MIN = 4,

        [Description("Cantidad Máxima")]
        QUANTITY_MAX = 5,

        [Description("Hectareas")]
        HECTARES = 6,
    }

    public enum BoolRelated {

        [Description("Es actual")]
        CURRENT = 0,
        [Description("Bypass")]
        BYPASS = 1,

        [Description("Por Defecto")]
        GENERIC_DEFAULT = 2,

        [Description("Activa")]
        GENERIC_ACTIVE = 3
    }

    public enum GeoRelated {

        [Description("Ubicación Geográfica")]
        GENERIC_LOCATION = 0
    }

    public enum DateRelated {

        [Description("Fin")]
        END_DATE = 0,
        [Description("Inicio")]
        START_DATE = 1,

        [Description("Ultima modificación")]
        LAST_MODIFIED = 2,
    }

    public enum EnumRelated {
        [Description("Tipo Medida")]
        GENERIC_MEASURE_TYPE = 0,

        [Description("Tipo Contenedor")]
        GENERIC_KIND_CONTAINER =1,

        [Description("Tipo de PreOrden")]
        PRE_ORDER_TYPE = 2,

        [Description("Tipo de Notificación")]
        NOTIFICATION_TYPE = 3,

        [Description("Tipo de Orden")]
        ORDER_TYPE = 4,

        [Description("Dosis aplicada a")]
        DOSES_APPLICATED_TO = 5,

        [Description("Estatus Cierre ")]
        CLOSED_STATUS = 6,

        [Description("Estatus Finalizado")]
        FINISH_STATUS = 7,

        [Description("Estatus Ejecución")]
        EXECUTION_STATUS = 8,
    }

    public enum PropertyRelated {
        GENERIC_GIRO = 0,
        GENERIC_ABBREVIATION = 1,
        GENERIC_BRAND = 2,
        GENERIC_CODE = 3,
        GENERIC_EMAIL = 4 ,
        GENERIC_END_DATE= 5,
        GENERIC_NAME = 6,
        GENERIC_RUT = 7,
        GENERIC_START_DATE = 8,
        GENERIC_DESC = 9,
        GENERIC_PATH = 10,
        GENERIC_QUANTITY_HECTARE = 11,
        GENERIC_COMMENT = 12,
        GENERIC_PHONE=13,
        GENERIC_NUMBER_OF_PLANTS=14,
        GENERIC_PLANT_IN_YEAR = 15,
        GENERIC_HECTARES = 16,
        GENERIC_LATITUDE = 17,
        GENERIC_LONGITUDE = 18,
        GENERIC_WETTING = 19,
        OBJECT_ID_AAD = 20,
        GENERIC_WEBPAGE = 21,
        GENERIC_QUANTITY = 22,
        DOSES_HOURSENTRYBARRACK= 23,
        DOSES_DAYSINTERVAL = 24,
        DOSES_SEQUENCE = 25,
        DOSES_WETTINGRECOMMENDED = 26,        
        DOSES_WAITINGDAYSLABEL = 27,        
        GENERIC_COUNTER = 28,
        WAITINGHARVEST_DAYS = 29,
        WAITINGHARVEST_PPM = 30,
        DOSES_MIN = 31,
        DOSES_MAX= 32,
        PRODUCT_NAME = 33
    }

    public enum EnumerationRelated {
        ORDER_TYPE= 1,
        SEASON_CURRENT=2, //0 false. 1 true
        NOTIFICATION_TYPE = 3,
        PREORDER_TYPE = 4,
        EXECUTION_STATUS = 5,
        EXECUTION_CLOSED_STATUS = 6,
        EXECUTION_FINISH_STATUS = 7,
        PRODUCT_KINDOFBOTTLE = 8,
        PRODUCT_MEASURETYPE = 9,
        GENERIC_ACTIVE = 10, //0 false, 1 true,
        GENERIC_DEFAULT = 11, //0 false, 1 true
        DOSES_DOSESAPPLICATEDTO = 12,
        
    }

    public enum OrderType {

        [Description("Orden por Defecto")]
        DEFAULT = 0,
        [Description("Orden Fenológica")]
        PHENOLOGICAL = 1
    }

    public enum PreOrderType {
        [Description("Pre-Orden por Defecto")]
        DEFAULT = 0,

        [Description("Pre-Orden Fenológica")]
        PHENOLOGICAL = 1
    }

    public enum CurrentSeason {
        
        NOT_CURRENT = 0,

        CURRENT = 1
    }

    public enum Device { 
        WEB = 0,
        MOBILE = 1
    }
}