using System;
using trifenix.agro.db.applicationsReference.ApplicationOrders;
using trifenix.agro.db.applicationsReference.@base;
using trifenix.agro.db.applicationsReference.Field;
using trifenix.agro.db.applicationsReference.Helper;
using trifenix.agro.db.applicationsReference.Products;
using trifenix.agro.db.applicationsReference.Stages;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.applicationOrders;
using trifenix.agro.db.interfaces.@base;
using trifenix.agro.db.interfaces.Field;
using trifenix.agro.db.interfaces.Helper;
using trifenix.agro.db.interfaces.Products;
using trifenix.agro.db.interfaces.stages;

namespace trifenix.agro.db.applicationsReference
{
    public class BaseContainers : IBaseContainer
    {

        private AgroDbArguments _dbArguments;
        public BaseContainers(AgroDbArguments dbArguments)
        {
            _dbArguments = dbArguments;
        }
        public IAppPurposeContainter ApplicationPurposes => new AppPurposeContainer(_dbArguments);

        public IRefAppPhenologicalContainer PhenologicalRefApps => new RefAppPhenologicalContainer(_dbArguments);

        public IRefAppContinuedContainer ContinuedRefApps => new RefAppContinuedContainer(_dbArguments);

        public IRefAppDateContainer DateRefApps => new RefAppDateContainer(_dbArguments);

        public IAgroSeasonContainer Seasons => new AgroSeasonContainer(_dbArguments);

        public IAgroVarietyContainer Varieties => new AgroVarietyContainer(_dbArguments);

        public IActiveIngredientContainer ActiveIngredients => new ActiveIngredientContainer(_dbArguments);

        public IActiveIngredientCategoryContainer ActiveIngredientCategories => new ActiveIngredientCategoryContainer(_dbArguments);

        public IApplicationMethodContainer ApplicationMethods => new ApplicationMethodContainer(_dbArguments);

        public ICertifierRegionContainer CertifierRegions => new CertifierRegionContainer(_dbArguments);

        public IPhenologicalEventContainer PhenologicalEvents => new PhenologicalEventContainer(_dbArguments);

        public IAgroSpecieContainer Species => new AgroSpecieContainer(_dbArguments);

        public IAgroFieldContainer Fields => new AgroFieldContainer(_dbArguments);

        public ICounterContainer Counter => new CounterTask(_dbArguments);
    }
}
