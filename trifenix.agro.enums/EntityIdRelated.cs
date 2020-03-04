namespace trifenix.agro.enums {

    public enum EntityRelated {
        AAD,
        BARRACK,
        BUSINESSNAME,
        CATEGORY_INGREDIENT,
        CERTIFIED_ENTITY,
        COSTCENTER,
        DOSES,
        INGREDIENT,
        JOB,
        NEBULIZER,
        PHENOLOGICAL_EVENT,
        PLOTLAND,
        PRODUCT,
        ROLE,
        ROOTSTOCK,
        SEASON,
        SECTOR,
        PREORDER,
        TARGET,
        TRACTOR,
        USER,
        VARIETY,
        NOTIFICATION,
        POLLINATOR,
        ORDER_FOLDER,
        EXECUTION_ORDER,
        ORDER,
        BARRACK_EVENT,
        DOSES_ORDER,
        EXECUTION_ORDER_STATUS,
        SPECIE,
        GEOPOINT
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