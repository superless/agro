using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;
using trifenix.agro.db.model.agro.local;
using trifenix.agro.external.interfaces.entities.orders;
using trifenix.agro.external.operations.entities.orders.args;
using trifenix.agro.external.operations.helper;
using trifenix.agro.microsoftgraph.model;
using trifenix.agro.model.external;

namespace trifenix.agro.external.operations.entities.orders
{
    public class OrderFolderOperations : IOrderFolderOperations
    {
        private readonly OrderFolderArgs _args;

        public OrderFolderOperations(OrderFolderArgs args)
        {
            _args = args;
        }        

        public async Task<ExtGetContainer<OrderFolder>> GetOrderFolder(string id)
        {
            var order = await _args.OrderFolder.GetOrderFolder(id);
            return OperationHelper.GetElement(order);
        }

        public async Task<ExtGetContainer<List<OrderFolder>>> GetOrderFolders()
        {
            var orderFoldersQuery = _args.OrderFolder.GetOrderFolders();
            var orderFolders = await _args.CommonDb.TolistAsync(orderFoldersQuery);
            var notificationsQuery = _args.NotificationEvent.GetNotificationEvents();
            var notifications = await _args.CommonDbNotifications.TolistAsync(notificationsQuery.Where(s => s.Barrack.SeasonId == _args.IdSeason));
            var orders = orderFolders.Select(s =>
            {
                var nw = DateTime.Now;
                var stage = notifications.Any(a => a.PhenologicalEvent.Id.Equals(s.PhenologicalEvent.Id)) ? PhenologicalStage.Success : nw > s.PhenologicalEvent.InitDate ? PhenologicalStage.Warning : PhenologicalStage.Waiting;
                s.Stage = stage;
                return s;
            }).ToList();
            return OperationHelper.GetElements(orders);
        }

        public async Task<ExtPostContainer<OrderFolder>> SaveEditOrderFolder(string id, string idPhenologicalEvent, string idApplicationTarget, string categoryId, string idSpecie, string idIngredient){
            var modifier = await _args.GraphApi.GetUserInfo();
            try
            {
                var elements = await GetElementsToFolder(idPhenologicalEvent, idApplicationTarget, categoryId, idSpecie, idIngredient);
                if (!elements.Success)
                {
                    return OperationHelper.PostNotFoundElementException<OrderFolder>(elements.Message, elements.IdNotfound);
                }
                var order = await _args.OrderFolder.GetOrderFolder(id);
                if (order == null)
                {
                    return OperationHelper.PostNotFoundElementException<OrderFolder>($"orden con id {id} no encontrada", id);
                }
                order.Stage = PhenologicalStage.Waiting;
                order.SeasonId = _args.IdSeason;
                order.PhenologicalEvent = elements.PhenologicalEvent;
                order.ModifyBy.Add(new UserActivity(DateTime.Now, modifier));
                order.Specie = elements.Specie;
                order.Ingredient = elements.Ingredient != null ? new LocalIngredient { Id = idIngredient, Name = elements.Ingredient.Name } : null;
                order.Category = elements.Category;
                order.ApplicationTarget = elements.Target;
                await _args.OrderFolder.CreateUpdateOrderFolder(order);
                return new ExtPostContainer<OrderFolder>
                {
                    IdRelated = id,
                    Result = order,
                    MessageResult = ExtMessageResult.Ok
                };
            }
            catch (Exception exc)
            {
                return OperationHelper.GetPostException<OrderFolder>(exc);
            }
        }

        public async Task<ExtPostContainer<string>> SaveNewOrderFolder(string idPhenologicalEvent, string idApplicationTarget, string categoryId, string idSpecie, string idIngredient){
            var creator = await _args.GraphApi.GetUserInfo();
            try{
                var elements = await GetElementsToFolder(idPhenologicalEvent, idApplicationTarget, categoryId, idSpecie, idIngredient);

                if (!elements.Success)
                {
                    return OperationHelper.PostNotFoundElementException<string>(elements.Message, elements.IdNotfound);
                }
                var order = new OrderFolder
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ApplicationTarget = elements.Target,
                    Creator = new UserActivity(DateTime.Now, creator),
                    Category = elements.Category,
                    Ingredient = elements.Ingredient != null ? new LocalIngredient { Id = idIngredient, Name = elements.Ingredient.Name } : null,
                    PhenologicalEvent = elements.PhenologicalEvent,
                    SeasonId = _args.IdSeason,
                    Specie = elements.Specie,
                    Stage = PhenologicalStage.Waiting
                };
                var idResult = await _args.OrderFolder.CreateUpdateOrderFolder(order);
                return new ExtPostContainer<string>
                {
                    IdRelated = idResult,
                    Result = idResult,
                    MessageResult = ExtMessageResult.Ok
                };
            }
            catch (Exception exc)
            {
                return OperationHelper.GetPostException<string>(exc);
            }
        }

        private async Task<ElementsFolder> GetElementsToFolder(string idPhenologicalEvent, string idApplicationTarget, string categoryId, string idSpecie, string idIngredient, string idSeason = null) {
            var elementFolder = new ElementsFolder();
            elementFolder.PhenologicalEvent = await _args.PhenologicalEvent.GetPhenologicalEvent(idPhenologicalEvent);
            if (elementFolder.PhenologicalEvent == null)
            {
                elementFolder.IdNotfound = idPhenologicalEvent;
                elementFolder.Message = $"Evento fenológico con id {idPhenologicalEvent} no encontrado";
                elementFolder.Success = false;
                return elementFolder;
            }
            elementFolder.Target =  await _args.Target.GetTarget(idApplicationTarget);
            if (elementFolder.Target == null)
            {
                elementFolder.IdNotfound = idApplicationTarget;
                elementFolder.Message = $"Objetivo de aplicación con id {idApplicationTarget} no encontrado";
                elementFolder.Success = false;
                return elementFolder;
            }
            elementFolder.Specie = await _args.Specie.GetSpecie(idSpecie);
            if (elementFolder.Specie == null)
            {
                elementFolder.IdNotfound = idSpecie;
                elementFolder.Message = $"Especie con id {idApplicationTarget} no encontrado";
                elementFolder.Success = false;
                return elementFolder;
            }
            elementFolder.Category = await _args.IngredientCategory.GetIngredientCategory(categoryId);
            if (elementFolder.Category == null)
            {
                elementFolder.IdNotfound = idSpecie;
                elementFolder.Message = $"Categoría de ingrediente con id {categoryId} no encontrado";
                elementFolder.Success = false;
                return elementFolder;
            }          
            elementFolder.Success = true;
            if (string.IsNullOrWhiteSpace(idIngredient))
            {
                return elementFolder;
            }
            elementFolder.Ingredient = await _args.Ingredient.GetIngredient(idIngredient);
            return elementFolder;
        }
    }

    public class ElementsFolder {
        public bool Success { get; set; }

        public PhenologicalEvent PhenologicalEvent { get; set; }
        public ApplicationTarget Target { get; set; }

        public IngredientCategory Category { get; set; }

        public Specie Specie { get; set; }

        public Rootstock Rootstock { get; set; }

        public Ingredient Ingredient { get; set; }

        public string Message { get; set; }

        public string IdNotfound { get; set; }
    }
}
