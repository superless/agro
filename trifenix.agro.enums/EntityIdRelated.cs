namespace trifenix.agro.enums {

    public enum EntityRelated { 
        CERTIFIED_ENTITY = 0,
        SPECIE=1,
        TARGET=2,
        VARIETY=3,
        INGREDIENT=4,
        BARRACK =5,
        PRODUCT=6,
        JOB = 7,
        TRACTOR = 8,
        NEBULIZER = 9,
        ROLE = 10,
        AAD = 11,
        SEASON = 12,
        COSTCENTER = 13,
        CATEGORY_INGREDIENT=14,
        PHENOLOGICAL_EVENT=15,
        DOSES=16

    }

    public enum PropertyRelated {
        PRODUCT_BRAND = 0,
        USER_RUT = 1,
        USER_EMAIL=2,
        NEBULIZER_BRAND=3,
        NEBULIZER_CODE = 4,
        TRACTOR_BRAND = 5,
        TRACTOR_CODE = 6,
        BUSINESSNAME_EMAIL=7,
        BUSINESSNAME_RUT = 8,
        BUSINESSNAME_GIRO = 9,
        SPECIE_CODE = 10,
        GENERIC_NAME = 11,

    }

    public enum EnumerationRelated {
        ORDER_TYPE= 1,
        SEASON_CURRENT=2,

    }

    public enum OrderType { 
        DEFAULT =0,
        PHENOLOGICAL =1
    }

}