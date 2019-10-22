using System;
using System.Collections.Generic;
using System.Text;
using trifenix.agro.db.applicationsReference.agro.orders;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.interfaces.agro.orders;

namespace trifenix.agro.db.applicationsReference.agro
{
    public class AgroRepository : IAgroRepository
    {

        private AgroDbArguments _dbArguments;
        public AgroRepository(AgroDbArguments dbArguments)
        {
            _dbArguments = dbArguments;
        }
        public IApplicationTargetRepository Targets => new ApplicationTargetRepository(_dbArguments);

        public IIngredientCategoryRepository Categories => new IngredientCategoryRepository(_dbArguments);

        public IIngredientRepository Ingredients => new IngredientRepository(_dbArguments);

        public IPhenologicalEventRepository PhenologicalEvents => new PhenologicalEventRepository(_dbArguments);

        public ISpecieRepository Species => new SpecieRepository(_dbArguments);

        public ISeasonRepository Seasons => new SeasonRepository(_dbArguments);

        public IOrderFolderRepository OrderFolder => new OrderFolderRepository(_dbArguments);


    }
}
