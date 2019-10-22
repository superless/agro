using trifenix.agro.db.interfaces.agro;
using trifenix.agro.external.interfaces;
using trifenix.agro.external.interfaces.entities.events;
using trifenix.agro.external.interfaces.entities.fields;
using trifenix.agro.external.interfaces.entities.main;
using trifenix.agro.external.interfaces.entities.orders;
using trifenix.agro.external.operations.entities.events;
using trifenix.agro.external.operations.entities.fields;
using trifenix.agro.external.operations.entities.main;
using trifenix.agro.external.operations.entities.orders;
using trifenix.agro.external.operations.entities.orders.args;
using trifenix.agro.storage.interfaces;

namespace trifenix.agro.external.operations
{
    public class AgroManager : IAgroManager
    {

        private readonly IAgroRepository _repository;
        private readonly string _idSeason;
        private readonly IUploadImage _uploadImage;

        public AgroManager(IAgroRepository repository, string idSeason, IUploadImage uploadImage = null)
        {
            _repository = repository;
            _idSeason = idSeason;
            _uploadImage = uploadImage;
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
            Specie = _repository.Species,
            Target = _repository.Targets,
            IdSeason = _idSeason
        });

        public ISectorOperations Sectors => new SectorOperations(_repository.Sectors, _idSeason);

        public IPlotLandOperations PlotLands => new PlotLandOperations(_repository.PlotLands, _repository.Sectors, _idSeason);

        public IBarrackOperations Barracks => new BarrackOperations(_repository.Barracks, _repository.Varieties, _repository.PlotLands, _idSeason);

        public IPhenologicalPreOrderOperations PhenologicalPreOrders => throw new System.NotImplementedException();

        public INotificatonEventOperations NotificationEvents => new NotificationEventOperations(_repository.NotificationEvents, _repository.Barracks, _repository.PhenologicalEvents, _uploadImage);

        public IVarietyOperations Varieties => new VarietyOperations(_repository.Varieties, _repository.Species);
    }
}
