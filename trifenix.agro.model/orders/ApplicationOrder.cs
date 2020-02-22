using Cosmonaut;
using Cosmonaut.Attributes;
using System;
using System.Collections.Generic;
using trifenix.agro.db.model.local;

namespace trifenix.agro.db.model.agro.orders {
    [SharedCosmosCollection("agro", "ApplicationOrder")]
    public class ApplicationOrder : DocumentBase, ISharedCosmosEntity {
        /// <summary>
        /// Identificador de la entidad
        /// </summary>
        public override string Id { get; set; }

        public int Correlative { get; set; }

        public int InnerCorrelative { get; set; }

        public bool IsPhenological { get; set; }

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

        public UserActivity Creator { get; set; }

        private List<UserActivity> _modifyBy;
        public List<UserActivity> ModifyBy
        {
            get
            {
                _modifyBy = _modifyBy ?? new List<UserActivity>();
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

        private List<string> _varietyAbb;


        /// <summary>
        /// variedades dentro de las dosis
        /// </summary>
        public List<string> VarietyAbb {
            get {
                _varietyAbb = _varietyAbb ?? new List<string>();
                return _varietyAbb;
            }
            set { _varietyAbb = value; }
        }
        
        /// <summary>
        /// Especie dentro de los cuarteles
        /// </summary>
        public string SpecieAbb { get; set; }

        public ApplicationOrder(string id, int correlative, List<string> certifiedEntitiesIds, string specieAbb, List<BarrackOrderInstance> barracksInstances, List<string>  targetIds, List<string> varietyAbb, string seasonId, string name, bool isPhenological, DateTime initDate, DateTime endDate, double wetting, List<ApplicationsInOrder> applications, UserActivity creator, List<PhenologicalPreOrder> phenologicalPreOrders) {
            Id = id;
            Correlative = correlative;
            IdsCertifiedEntities = certifiedEntitiesIds;
            SpecieAbb = specieAbb;
            Barracks = barracksInstances;
            IdsTargets = targetIds;
            VarietyAbb = varietyAbb;
            SeasonId = seasonId;
            Name = name;
            IsPhenological = isPhenological;
            InitDate = initDate;
            EndDate = endDate;
            Wetting = wetting;
            ApplicationInOrders = applications;
            Creator = creator;
            PhenologicalPreOrders = phenologicalPreOrders;
        }

        public ApplicationOrder(){}
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