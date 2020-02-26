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
        SPECIE,
        TARGET,
        TRACTOR,
        USER,
        VARIETY,


    }

    public enum PropertyRelated {
        BUSINESSNAME_GIRO,
        GENERIC_ABBREVIATION,
        GENERIC_BRAND,
        GENERIC_CODE,
        GENERIC_EMAIL,
        GENERIC_END_DATE,
        GENERIC_NAME,
        GENERIC_RUT,
        GENERIC_START_DATE,

    }

    public enum EnumerationRelated {
        ORDER_TYPE= 1,
        SEASON_CURRENT=2,

    }

    public enum OrderType { 
        DEFAULT =0,
        PHENOLOGICAL =1
    }

    public enum CurrentSeason {
        NOT_CURRENT = 0,
        CURRENT = 1
    }

}