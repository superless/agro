using Cosmonaut;
using Cosmonaut.Attributes;
using System;
using trifenix.agro.enums;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.db.model.agro.orders {

    [SharedCosmosCollection("agro", "ApplicationOrder")]
    public class ApplicationOrder : DocumentBaseName, ISharedCosmosEntity
    {
        /// <summary>
        /// Identificador de la entidad
        /// </summary>
        public override string Id { get; set; }

        public OrderType OrderType { get; set; }

        public override string Name { get; set; }

        public DateTime InitDate { get; set; }

        public DateTime EndDate { get; set; }

        public double Wetting { get; set; }

        public DosesOrder[] DosesOrder { get; set; }

        public string[] IdsPhenologicalPreOrder { get; set; }

        

        public BarrackOrderInstance[] Barracks { get; set; }

    }


    public class BarrackOrderInstance {

        public string  IdBarrack { get; set; }
        public string[] IdNotificationEvents { get; set; }

    }

    public class DosesOrder {

        public string IdDoses { get; set; }
        public double QuantityByHectare { get; set; }

    }

}