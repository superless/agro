using System;
using System.Collections.Generic;
using System.Text;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.external.interfaces;
using trifenix.agro.external.interfaces.entities;
using trifenix.agro.external.interfaces.entities.main;
using trifenix.agro.external.operations.entities;
using trifenix.agro.external.operations.entities.args;
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

        public ISpecieOperations Species => new SpecieOperations(_repository.Species);

        public IIngredientCategoryOperations IngredientCategories => new IngredientCategoryOperations(_repository.Categories);

        public IIngredientsOperations Ingredients => new IngredientOperations(_repository.Ingredients, _repository.Categories);

        public ISeasonOperations Seasons => new SeasonOperations(_repository.Seasons);

        public IOrderFolderOperations OrderFolder => new OrderFolderOperations(new OrderFolderArgs {
            Ingredient = _repository.Ingredients,
            IngredientCategory = _repository.Categories,
            OrderFolder = _repository.OrderFolder,
            PhenologicalEvent = _repository.PhenologicalEvents,
            Season = _repository.Seasons,
            Specie = _repository.Species,
            Target = _repository.Targets
        });
    }
}
