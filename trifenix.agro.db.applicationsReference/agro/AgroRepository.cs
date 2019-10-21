using System;
using System.Collections.Generic;
using System.Text;
using trifenix.agro.db.interfaces.agro;

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

        public IPhenologicalEventRepositoy PhenologicalEvents => new PhenologicalEventRepository(_dbArguments);

        public ISpecieRepository SpecieRepository => new SpecieRepository(_dbArguments);

         
    }
}
