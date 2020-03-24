namespace trifenix.agro.enums {

    public enum EntityRelated {
        WAITINGHARVEST = 0,
        BARRACK = 1,
        BUSINESSNAME = 2,
        CATEGORY_INGREDIENT = 3,
        CERTIFIED_ENTITY = 4,
        COSTCENTER = 5,
        DOSES = 6,
        INGREDIENT = 7,
        JOB = 8,
        NEBULIZER = 9,
        PHENOLOGICAL_EVENT = 10,
        PLOTLAND = 11,
        PRODUCT = 12,
        ROLE = 13,
        ROOTSTOCK = 14,
        SEASON = 15,
        SECTOR = 16,
        PREORDER = 17,
        TARGET = 18,
        TRACTOR = 19,
        USER = 20,
        VARIETY = 21,  // el entity index
        NOTIFICATION_EVENT = 22,
        POLLINATOR = 23,
        ORDER_FOLDER = 24,
        EXECUTION_ORDER = 25,
        ORDER = 26,
        BARRACK_EVENT = 27,
        DOSES_ORDER = 28,
        EXECUTION_ORDER_STATUS = 29,
        SPECIE = 30,
        GEOPOINT = 31
    }

    public enum StringRelated {
        GENERIC_CODE = 0,
        GENERIC_ABBREVIATION = 1,
        OBJECTID_AAD = 2,
        RUT = 3,
        GENERIC_NAME = 4,
        EMAIL = 5,
        GENERIC_BRAND = 6,
    }
}

    public enum Num64Related {
        
    }

    public enum Num32Related {

    }

    public enum DoubleRelated{
        QUANTITY_CONTAINER = 0,
    }

    public enum BoolRelated {
        CURRENT = 0,
    }

    public enum GeoRelated { 
    
    }

    public enum DateRelated
    {
        END_DATE = 0,
    }

    public enum EnumRelated { 
        GENERIC_MEASURE_TYPE = 0,
        GENERIC_KIND_CONTAINER=1,
    PRE_ORDER_TYPE = 2,
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
        DEFAULT = 0,
        PHENOLOGICAL = 1
    }

    public enum PreOrderType {
        DEFAULT = 0,
        PHENOLOGICAL = 1
    }

    public enum CurrentSeason {
        NOT_CURRENT = 0,
        CURRENT = 1
    }

}