namespace trifenix.agro.enums {

    public enum EntityRelated {
        AAD = 0,
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
        VARIETY = 21,
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

    public enum PropertyRelated {
        GENERIC_GIRO,
        GENERIC_ABBREVIATION,
        GENERIC_BRAND,
        GENERIC_CODE,
        GENERIC_EMAIL,
        GENERIC_END_DATE,
        GENERIC_NAME,
        GENERIC_RUT,
        GENERIC_START_DATE,
        GENERIC_DESC,
        GENERIC_PATH,
        GENERIC_QUANTITY_HECTARE,
        GENERIC_COMMENT,
        GENERIC_PHONE,
        GENERIC_NUMBER_OF_PLANTS,
        GENERIC_PLANT_IN_YEAR,
        GENERIC_HECTARES,
        GENERIC_LATITUDE,
        GENERIC_LONGITUDE,
        GENERIC_WETTING
    }

    public enum EnumerationRelated {
        ORDER_TYPE= 1,
        SEASON_CURRENT=2, //0 false. 1 true
        NOTIFICATION_TYPE = 3,
        PREORDER_TYPE = 4,
        EXECUTION_STATUS = 5,
        EXECUTION_CLOSED_STATUS = 6,
        EXECUTION_FINISH_STATUS = 7
    }

    public enum OrderType { 
        DEFAULT = 0,
        PHENOLOGICAL = 1
    }

    public enum PreOrderType
    {
        DEFAULT = 0,
        PHENOLOGICAL = 1
    }

    public enum CurrentSeason {
        NOT_CURRENT = 0,
        CURRENT = 1
    }

    

}