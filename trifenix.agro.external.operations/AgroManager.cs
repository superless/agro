using Cosmonaut;
using System;
using System.Collections.Generic;
using trifenix.agro.db;
using trifenix.agro.db.applicationsReference;
using trifenix.agro.db.applicationsReference.agro.Common;
using trifenix.agro.db.applicationsReference.common;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model;
using trifenix.agro.db.model.agro;
using trifenix.agro.db.model.agro.core;
using trifenix.agro.db.model.agro.orders;
using trifenix.agro.email.interfaces;
using trifenix.agro.external.interfaces;
using trifenix.agro.external.operations.entities;
using trifenix.agro.external.operations.entities.events;
using trifenix.agro.external.operations.entities.ext;
using trifenix.agro.external.operations.entities.fields;
using trifenix.agro.external.operations.entities.main;
using trifenix.agro.external.operations.entities.orders;
using trifenix.agro.microsoftgraph.operations;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.storage.interfaces;
using trifenix.agro.validator.interfaces;
using trifenix.agro.validator.operations;
using trifenix.agro.weather.interfaces;

namespace trifenix.agro.external.operations {
    public class AgroManager : IAgroManager {

        private readonly AgroDbArguments Arguments;
        private readonly IEmail _email;
        private readonly IUploadImage _uploadImage;
        private readonly IWeatherApi _weatherApi;
        private readonly IAgroSearch _searchServiceInstance;
        private readonly string UserId;
        private readonly bool isBatch;

        public AgroManager(AgroDbArguments arguments, IEmail email, IUploadImage uploadImage, IWeatherApi weatherApi, IAgroSearch searchServiceInstance, string ObjectIdAAD, bool _isBatch) {
            Arguments = arguments;
            _email = email;
            _uploadImage = uploadImage;
            _weatherApi = weatherApi;
            _searchServiceInstance = searchServiceInstance;
            UserId = CommonQueries.GetUserIdFromAAD(ObjectIdAAD).Result;
            isBatch = _isBatch;
        }

        private IMainGenericDb<T> GetMainDb<T>() where T : DocumentBase => new MainGenericDb<T>(Arguments);

        private ICommonDbOperations<T> GetCommonDbOp<T>() where T : DocumentBase => new CommonDbOperations<T>();

        public ICosmosStore<EntityContainer> BatchStore => new CosmosStore<EntityContainer>(new CosmosStoreSettings(Arguments.NameDb, Arguments.EndPointUrl, Arguments.PrimaryKey));

        public IExistElement ExistsElements => isBatch ? (IExistElement)new BatchExistsElements(Arguments) : new CosmosExistElement(Arguments);

        private ICommonQueries CommonQueries => new CommonQueries(Arguments);

        private IValidator Validators => new Validator(new Dictionary<string, IValidate> { { "Reference", new ReferenceValidation(ExistsElements) }, { "Required", new RequiredValidation() }, { "Unique", new UniqueValidation(ExistsElements) } });


        public IGenericOperation<UserActivity, UserActivityInput> UserActivity => new UserActivityOperations(GetMainDb<UserActivity>(), ExistsElements, _searchServiceInstance, GetCommonDbOp<UserActivity>(), UserId, Validators);

        public IGenericOperation<Sector, SectorInput> Sector => new SectorOperations(GetMainDb<Sector>(), ExistsElements, _searchServiceInstance, GetCommonDbOp<Sector>(), Validators);
        
        public IGenericOperation<PlotLand, PlotLandInput> PlotLand => new PlotLandOperations(GetMainDb<PlotLand>(), ExistsElements, _searchServiceInstance, GetCommonDbOp<PlotLand>(), Validators);

        public IGenericOperation<Specie, SpecieInput> Specie => new SpecieOperations(GetMainDb<Specie>(), ExistsElements, _searchServiceInstance, GetCommonDbOp<Specie>(), Validators);

        public IGenericOperation<Variety, VarietyInput> Variety => new VarietyOperations(GetMainDb<Variety>(), ExistsElements, _searchServiceInstance, GetCommonDbOp<Variety>(), Validators);

        public IGenericOperation<ApplicationTarget, ApplicationTargetInput> ApplicationTarget => new ApplicationTargetOperations(GetMainDb<ApplicationTarget>(), ExistsElements, _searchServiceInstance, GetCommonDbOp<ApplicationTarget>(), Validators);

        public IGenericOperation<PhenologicalEvent, PhenologicalEventInput> PhenologicalEvent => new PhenologicalEventOperations(GetMainDb<PhenologicalEvent>(), ExistsElements, _searchServiceInstance, GetCommonDbOp<PhenologicalEvent>(), Validators);

        public IGenericOperation<CertifiedEntity, CertifiedEntityInput> CertifiedEntity => new CertifiedEntityOperations(GetMainDb<CertifiedEntity>(), ExistsElements, _searchServiceInstance, GetCommonDbOp<CertifiedEntity>(), Validators);

        public IGenericOperation<IngredientCategory, IngredientCategoryInput> IngredientCategory => new IngredientCategoryOperations(GetMainDb<IngredientCategory>(), ExistsElements, _searchServiceInstance, GetCommonDbOp<IngredientCategory>(), Validators);

        public IGenericOperation<Ingredient, IngredientInput> Ingredient => new IngredientOperations(GetMainDb<Ingredient>(), ExistsElements, _searchServiceInstance, GetCommonDbOp<Ingredient>(), Validators);

        public IGenericOperation<Product, ProductInput> Product => new ProductOperations(GetMainDb<Product>(), ExistsElements, _searchServiceInstance, Dose, GetCommonDbOp<Product>(), CommonQueries, Validators);

        public IGenericOperation<Dose, DosesInput> Dose => new DosesOperations(GetMainDb<Dose>(), ExistsElements, _searchServiceInstance, GetCommonDbOp<Dose>(), new Counters(Arguments), Validators);

        public IGenericOperation<Role, RoleInput> Role => new RoleOperations(GetMainDb<Role>(), ExistsElements, _searchServiceInstance, GetCommonDbOp<Role>(), Validators);

        public IGenericOperation<Job, JobInput> Job => new JobOperations(GetMainDb<Job>(), ExistsElements, _searchServiceInstance, GetCommonDbOp<Job>(), Validators);

        public IGenericOperation<UserApplicator, UserApplicatorInput> UserApplicator => new UserOperations(GetMainDb<UserApplicator>(), ExistsElements, _searchServiceInstance, new GraphApi(Arguments), GetCommonDbOp<UserApplicator>(), Validators);

        public IGenericOperation<Nebulizer, NebulizerInput> Nebulizer => new NebulizerOperations(GetMainDb<Nebulizer>(), ExistsElements, _searchServiceInstance, GetCommonDbOp<Nebulizer>(), Validators);
        
        public IGenericOperation<Tractor, TractorInput> Tractor => new TractorOperations(GetMainDb<Tractor>(), ExistsElements, _searchServiceInstance, GetCommonDbOp<Tractor>(), Validators);

        public IGenericOperation<BusinessName, BusinessNameInput> BusinessName => new BusinessNameOperations(GetMainDb<BusinessName>(), ExistsElements, _searchServiceInstance, GetCommonDbOp<BusinessName>(), Validators);

        public IGenericOperation<CostCenter, CostCenterInput> CostCenter => new CostCenterOperations(GetMainDb<CostCenter>(), ExistsElements, _searchServiceInstance, GetCommonDbOp<CostCenter>(), Validators);

        public IGenericOperation<Season, SeasonInput> Season => new SeasonOperations(GetMainDb<Season>(), ExistsElements, _searchServiceInstance, GetCommonDbOp<Season>(), Validators);

        public IGenericOperation<Rootstock, RootstockInput> Rootstock => new RootstockOperations(GetMainDb<Rootstock>(), ExistsElements, _searchServiceInstance, GetCommonDbOp<Rootstock>(), Validators);

        public IGenericOperation<OrderFolder, OrderFolderInput> OrderFolder => new OrderFolderOperations(GetMainDb<OrderFolder>(), ExistsElements, _searchServiceInstance, CommonQueries, GetCommonDbOp<OrderFolder>(), Validators);

        public IGenericOperation<Barrack, BarrackInput> Barrack => new BarrackOperations(GetMainDb<Barrack>(), ExistsElements, _searchServiceInstance, CommonQueries, GetCommonDbOp<Barrack>(), Validators);

        public IGenericOperation<NotificationEvent, NotificationEventInput> NotificationEvent => new NotificationEventOperations(GetMainDb<NotificationEvent>(), ExistsElements, _searchServiceInstance, CommonQueries, _email, _uploadImage, _weatherApi, GetCommonDbOp<NotificationEvent>(), Validators);

        public IGenericOperation<PreOrder, PreOrderInput> PreOrder => new PreOrdersOperations(GetMainDb<PreOrder>(), ExistsElements, _searchServiceInstance, CommonQueries, GetCommonDbOp<PreOrder>(), Validators);

        public IGenericOperation<ApplicationOrder, ApplicationOrderInput> ApplicationOrder => new ApplicationOrderOperations(GetMainDb<ApplicationOrder>(), ExistsElements, _searchServiceInstance, CommonQueries, GetCommonDbOp<ApplicationOrder>(), Validators);

        public IGenericOperation<ExecutionOrder, ExecutionOrderInput> ExecutionOrder => new ExecutionOrderOperations(GetMainDb<ExecutionOrder>(), ExistsElements, _searchServiceInstance, CommonQueries, GetCommonDbOp<ExecutionOrder>(), Validators);

        public IGenericOperation<ExecutionOrderStatus, ExecutionOrderStatusInput> ExecutionOrderStatus => new ExecutionOrderStatusOperations(GetMainDb<ExecutionOrderStatus>(), ExistsElements, _searchServiceInstance, GetCommonDbOp<ExecutionOrderStatus>(), Validators);
        
        public IGenericOperation<Comment, CommentInput> Comments => new CommentOperation(GetMainDb<Comment>(), ExistsElements, _searchServiceInstance, GetCommonDbOp<Comment>(), Validators);

    }

}