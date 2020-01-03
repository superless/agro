using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.interfaces.agro.orders;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.db.model.agro.orders;
using trifenix.agro.external.interfaces.entities.orders;
using trifenix.agro.external.operations.helper;
using trifenix.agro.microsoftgraph.interfaces;
using trifenix.agro.microsoftgraph.model;
using trifenix.agro.model.external;

namespace trifenix.agro.external.operations.entities.orders
{
    public class ExecutionOrderOperations : IExecutionOrderOperations {

        private readonly IExecutionOrderRepository _repo;
        private readonly IApplicationOrderRepository _repoOrders;
        private readonly IUserRepository _repoUsers;
        private readonly INebulizerRepository _repoNebulizers;
        private readonly ITractorRepository _repoTractors;
        private readonly ICommonDbOperations<ExecutionOrder> _commonDb;
        private readonly IGraphApi _graphApi;


        public ExecutionOrderOperations(IExecutionOrderRepository repo, IApplicationOrderRepository repoOrders, IUserRepository repoUsers, INebulizerRepository repoNebulizers, ITractorRepository repoTractors, ICommonDbOperations<ExecutionOrder> commonDb, IGraphApi graphApi) {
            _repo = repo;
            _repoOrders = repoOrders;
            _repoUsers = repoUsers;
            _repoNebulizers = repoNebulizers;
            _repoTractors = repoTractors;
            _commonDb = commonDb;
            _graphApi = graphApi;
        }

        public async Task<ExtGetContainer<ExecutionOrder>> GetExecutionOrderOrder(string id)
        {
            var executionOrder = await _repo.GetExecutionOrder(id);
            return OperationHelper.GetElement(executionOrder);
        }

        public async Task<ExtGetContainer<List<ExecutionOrder>>> GetExecutionOrderOrders()
        {
            var executionOrdersQuery = _repo.GetExecutionOrders();
            var executionOrders = await _commonDb.TolistAsync(executionOrdersQuery);
            return OperationHelper.GetElements(executionOrders);
        }

        public async Task<ExtPostContainer<string>> SaveNewExecutionOrder(string idOrder, string idUserApplicator, string idNebulizer, string idTractor, string commentary)
        {
            var creator = await _graphApi.GetUserInfo();
            var userActivity = new UserActivity(DateTime.Now, creator);
            ApplicationOrder order = await _repoOrders.GetApplicationOrder(idOrder);
            if (order == null)
                return OperationHelper.PostNotFoundElementException<string>($"No se encontró la orden de aplicacion con id {idOrder}", idOrder);
            UserApplicator userApplicator = await _repoUsers.GetUser(idUserApplicator);
            if (userApplicator == null)
                return OperationHelper.PostNotFoundElementException<string>($"No se encontró el usuario aplicador con id {idUserApplicator}", idUserApplicator);
            Nebulizer nebulizer = null;
            Tractor tractor = null;
            if (!String.IsNullOrWhiteSpace(idNebulizer))
            {
                nebulizer = await _repoNebulizers.GetNebulizer(idNebulizer);
                if (nebulizer == null)
                    return OperationHelper.PostNotFoundElementException<string>($"No se encontró la nebulizadora con id {idNebulizer}", idNebulizer);
            }
            if (!String.IsNullOrWhiteSpace(idTractor))
            {
                tractor = await _repoTractors.GetTractor(idTractor);
                if (tractor == null)
                    return OperationHelper.PostNotFoundElementException<string>($"No se encontró el tractor con id {idTractor}", idTractor);
            }
            List<Comments> comments = new List<Comments>();
            if (!String.IsNullOrWhiteSpace(commentary))
                comments.Add(new Comments(userActivity, commentary));
            return await OperationHelper.CreateElement(_commonDb, _repo.GetExecutionOrders(),
               async s => await _repo.CreateUpdateExecutionOrder(new ExecutionOrder
               {
                   Id = s,
                   IdOrder = idOrder,
                   ExecutionStatus = 0,
                   FinishStatus = 0,
                   ClosedStatus = 0,
                   UserApplicator = userApplicator,
                   Nebulizer = nebulizer,
                   Tractor = tractor,
                   Comments = comments,
                   Creator = userActivity
               }),
               s => false,
               ""
           );
        }

        public async Task<ExtPostContainer<ExecutionOrder>> SaveEditExecutionOrder(string id, string idOrder, ExecutionStatus executionStatus, FinishStatus finishStatus, ClosedStatus closedStatus, string idUserApplicator, string idNebulizer, string idTractor)
        {
            var element = await _repo.GetExecutionOrder(id);
            var modifier = await _graphApi.GetUserInfo();
            var userActivity = new UserActivity(DateTime.Now, modifier);
            ApplicationOrder order = await _repoOrders.GetApplicationOrder(idOrder);
            if (order == null)
                return OperationHelper.PostNotFoundElementException<ExecutionOrder>($"No se encontró la orden de aplicacion con id {idOrder}", idOrder);
            UserApplicator userApplicator = await _repoUsers.GetUser(idUserApplicator);
            if (userApplicator == null)
                return OperationHelper.PostNotFoundElementException<ExecutionOrder>($"No se encontró el usuario aplicador con id {idUserApplicator}", idUserApplicator);
            Nebulizer nebulizer = null;
            Tractor tractor = null;
            if (!String.IsNullOrWhiteSpace(idNebulizer))
            {
                nebulizer = await _repoNebulizers.GetNebulizer(idNebulizer);
                if (nebulizer == null)
                    return OperationHelper.PostNotFoundElementException<ExecutionOrder>($"No se encontró la nebulizadora con id {idNebulizer}", idNebulizer);
            }
            if (!String.IsNullOrWhiteSpace(idTractor))
            {
                tractor = await _repoTractors.GetTractor(idTractor);
                if (tractor == null)
                    return OperationHelper.PostNotFoundElementException<ExecutionOrder>($"No se encontró el tractor con id {idTractor}", idTractor);
            }
            return await OperationHelper.EditElement(_commonDb, _repo.GetExecutionOrders(),
                id,
                element,
                s => {
                    s.IdOrder = idOrder;
                    s.ExecutionStatus = executionStatus;
                    s.FinishStatus = finishStatus;
                    s.ClosedStatus = closedStatus;
                    s.UserApplicator = userApplicator;
                    s.Nebulizer = nebulizer;
                    s.Tractor = tractor;
                    s.ModifyBy.Add(userActivity);
                    return s;
                },
                _repo.CreateUpdateExecutionOrder,
                 $"No existe orden de ejecucion con id: {id}",
                s => false,
                ""
            );
        }

        public async Task<ExtPostContainer<ExecutionOrder>> AddCommentaryToExecutionOrder(string idExecutionOrder, string commentary) {
            var element = await _repo.GetExecutionOrder(idExecutionOrder);
            var modifier = await _graphApi.GetUserInfo();
            var userActivity = new UserActivity(DateTime.Now, modifier);
            if (String.IsNullOrWhiteSpace(commentary))
                return OperationHelper.PostNotFoundElementException<ExecutionOrder>("El comentario no puede estar vacio.");
            return await OperationHelper.EditElement(_commonDb, _repo.GetExecutionOrders(),
                idExecutionOrder,
                element,
                s => {
                    s.Comments.Add(new Comments(userActivity, commentary));
                    return s;
                },
                _repo.CreateUpdateExecutionOrder,
                 $"No existe orden de ejecucion con id: {idExecutionOrder}",
                s => false,
                ""
            );
        }

    }
}
