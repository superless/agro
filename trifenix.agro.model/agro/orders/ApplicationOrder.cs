using Cosmonaut;
using Cosmonaut.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using trifenix.agro.db.model.agro.local;

namespace trifenix.agro.db.model.agro.orders
{
    [SharedCosmosCollection("agro", "ApplicationOrder")]
    public class ApplicationOrder : DocumentBase, ISharedCosmosEntity
    {
        /// <summary>
        /// Identificador de la entidad
        /// </summary>
        public override string Id { get; set; }

        public string Name { get; set; }

        private List<BarrackOrderInstance> _barracks;

        public List<BarrackOrderInstance> Barracks
        {
            get {
                _barracks = _barracks ?? new List<BarrackOrderInstance>();
                return _barracks; }
            set { _barracks = value; }
        }

        public string SeasonId { get; set; }


        public double Wetting { get; set; }


        private List<ApplicationsInOrder> _applicationsInOrder;

        public List<ApplicationsInOrder> ApplicationInOrders
        {
            get {

                _applicationsInOrder = _applicationsInOrder ?? new List<ApplicationsInOrder>();
                return _applicationsInOrder; }
            set { _applicationsInOrder = value; }
        }

        private List<PhenologicalPreOrder> _phenologicalPreOrders;

        public List<PhenologicalPreOrder> PhenologicalPreOrders
        {
            get {

                _phenologicalPreOrders = _phenologicalPreOrders ?? new List<PhenologicalPreOrder>();
                return _phenologicalPreOrders; }
            set { _phenologicalPreOrders = value; }
        }

    }


    public class ApplicationsInOrder {

        public string ProductId { get; set; }


        public double QuantityByHectare { get; set; }



        //nullable
        public Doses Doses { get; set; }




    }


    public class BarrackOrderInstance {

        public Barrack Barrack { get; set; }


        //nullable
        private List<string> _eventsId;

        public List<string> EventsId
        {
            get {

                _eventsId = _eventsId ?? new List<string>();
                return _eventsId; }
            set { _eventsId = value; }
        }








    }
}
