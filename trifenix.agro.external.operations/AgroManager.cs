using System;
using System.Collections.Generic;
using System.Text;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.external.interfaces;
using trifenix.agro.external.interfaces.entities.main;
using trifenix.agro.external.operations.entities.main;

namespace trifenix.agro.external.operations
{
    public class AgroManager : IAgroManager
    {

        private readonly IAgroRepository _repository;

        public AgroManager(IAgroRepository repository)
        {
            _repository = repository;
        }

        public IPhenologicalOperations PhenologicalEvents => new PhenologicalEventOperations(_repository.PhenologicalEvents);

        public IApplicationTargetOperations ApplicationTargets => new ApplicationTargetOperations(_repository.Targets);

        public ISpecieOperations Species => new SpecieOperations(_repository.SpecieRepository);

        public IIngredientCategoryOperations IngredientCategories => new IngredientCategoryOperations(_repository.Categories);

        public IIngredientsOperations Ingredients => new IngredientOperations(_repository.Ingredients, _repository.Categories);

        public ISeasonOperations Seasons => new SeasonOperations(_repository.Seasons);
    }
}
