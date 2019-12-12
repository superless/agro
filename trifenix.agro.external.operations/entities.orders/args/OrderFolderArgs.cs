using System;
using System.Collections.Generic;
using System.Text;
using trifenix.agro.db.applicationsReference.agro;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.interfaces.agro.events;
using trifenix.agro.db.interfaces.agro.orders;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.microsoftgraph.interfaces;

namespace trifenix.agro.external.operations.entities.orders.args
{
    public class OrderFolderArgs
    {
        public IPhenologicalEventRepository PhenologicalEvent { get; set; }

        public IApplicationTargetRepository Target { get; set; }

        public ISpecieRepository Specie { get; set; }

        public IRootstockRepository Rootstock { get; set; }

        public IIngredientRepository Ingredient { get; set; }

        public IIngredientCategoryRepository IngredientCategory { get; set; }

        public IOrderFolderRepository OrderFolder { get; set; }

        public IGraphApi GraphApi { get; set; }

        public INotificationEventRepository NotificationEvent { get; set; }

        public string IdSeason { get; set; }

        public ICommonDbOperations<OrderFolder> CommonDb { get; set; }

        public ICommonDbOperations<NotificationEvent> CommonDbNotifications { get; set; }

    }
}
