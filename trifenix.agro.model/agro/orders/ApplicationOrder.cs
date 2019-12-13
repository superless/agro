using Cosmonaut;
using Cosmonaut.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using trifenix.agro.db.model.agro.local;
using trifenix.agro.microsoftgraph.model;

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
        public DateTime InitDate { get; set; }
        public DateTime EndDate { get; set; }

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

        public UserInfo Creator { get; set; }

        private List<UserInfo> _modifyBy;
        public List<UserInfo> ModifyBy
        {
            get
            {
                _modifyBy = _modifyBy ?? new List<UserInfo>();
                return _modifyBy;
            }
            set { _modifyBy = value; }
        }

        private List<PhenologicalPreOrder> _phenologicalPreOrders;

        public List<PhenologicalPreOrder> PhenologicalPreOrders
        {
            get {

                _phenologicalPreOrders = _phenologicalPreOrders ?? new List<PhenologicalPreOrder>();
                return _phenologicalPreOrders; }
            set { _phenologicalPreOrders = value; }
        }



        private List<string> _idsCertifiedEntities;

        /// <summary>
        /// Ids de las entidades certificadoras, dentro de las dosis
        /// </summary>
        public List<string> IdsCertifiedEntities
        {
            get
            {
                _idsCertifiedEntities = _idsCertifiedEntities ?? new List<string>();
                return _idsCertifiedEntities;
            }
            set { _idsCertifiedEntities = value; }
        }


        private List<string> _idsTargets;


        /// <summary>
        /// Ids de las enfermedades dentro de las dosis
        /// </summary>
        public List<string> IdsTargets
        {
            get
            {
                _idsTargets = _idsTargets ?? new List<string>();
                return _idsTargets;
            }
            set { _idsTargets = value; }
        }

        private List<string> _idVarieties;


        /// <summary>
        /// variedades dentro de las dosis
        /// </summary>
        public List<string> IdVarieties
        {
            get
            {
                _idVarieties = _idVarieties ?? new List<string>();
                return _idVarieties;
            }
            set { _idVarieties = value; }
        }

        private List<string> __idsSpecies;


        /// <summary>
        /// especies dentro de las dosis
        /// </summary>
        public List<string> IdsSpecies
        {
            get
            {
                __idsSpecies = __idsSpecies ?? new List<string>();
                return __idsSpecies;
            }
            set { __idsSpecies = value; }
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
