using trifenix.agro.db.interfaces.applicationOrders;
using trifenix.agro.db.interfaces.@base;
using trifenix.agro.db.interfaces.Field;
using trifenix.agro.db.interfaces.Helper;
using trifenix.agro.db.interfaces.Products;
using trifenix.agro.db.interfaces.stages;

namespace trifenix.agro.db.interfaces
{
    public interface IBaseContainer 
    {
        IAppPurposeContainter ApplicationPurposes { get; }

        IRefAppPhenologicalContainer PhenologicalRefApps { get;  }

        IRefAppContinuedContainer ContinuedRefApps { get;  }

        IRefAppDateContainer DateRefApps { get;  }

        IAgroSeasonContainer Seasons { get;}

        IAgroVarietyContainer Varieties { get; }


        IActiveIngredientContainer ActiveIngredients { get;  }

        IActiveIngredientCategoryContainer ActiveIngredientCategories { get; }

        IApplicationMethodContainer ApplicationMethods { get; }

        ICertifierRegionContainer CertifierRegions { get; }

        IPhenologicalEventContainer PhenologicalEvents { get; }

        IAgroSpecieContainer Species { get; }

        IAgroFieldContainer Fields { get; }

        ICounterContainer Counter { get; }

    }
}
