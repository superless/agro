namespace trifenix.agro.enums {

    public enum EntityIdRelated { 
        CERTIFIED_ENTITY = 0,
        SPECIE = 1,
        TARGET = 2,
        VARIETY = 3,
        INGREDIENT = 4,
        BARRACK = 5,
        PRODUCT = 6,
        JOB = 7,
        TRACTOR = 8,
        NEBULIZER = 9,
        ROLE = 10,
        AAD = 11,
        SEASON = 12,
        COSTCENTER = 13,

    }

    public enum EntityRelated {
        PRODUCT_BRAND = 0,
        USER_RUT = 1,
        USER_EMAIL = 10,
        NEBULIZER_BRAND = 2,
        NEBULIZER_CODE = 3,
        TRACTOR_BRAND = 4,
        TRACTOR_CODE = 5,
        BUSINESSNAME_EMAIL = 6,
        BUSINESSNAME_RUT = 7,
        BUSINESSNAME_GIRO = 8,
        SPECIE_CODE = 9,
        GENERIC_NA = 11,

    }

    public enum NumberRelated {
        ORDER_TYPE = 1,
        SEASON_CURRENT = 2,

    }

    public enum OrderType { 
        DEFAULT = 0,
        PHENOLOGICAL = 1
    }

}