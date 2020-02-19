using System;
using System.Collections.Generic;
using trifenix.agro.db.model.agro;
using trifenix.agro.db.model.agro.orders;

namespace trifenix.agro.model.external.output
{
    public class OutPutApplicationOrder {

        public string Id { get; set; }

        public int Correlative { get; set; }

        public string Specie { get; set; }

        public string Name { get; set; }

        public bool isPhenological { get; set; }

        public DateTime InitDate { get; set; }
        public DateTime EndDate { get; set; }

        private List<OutputBarrackInstance> _barracks;

        public List<OutputBarrackInstance> Barracks
        {
            get
            {
                _barracks = _barracks ?? new List<OutputBarrackInstance>();
                return _barracks;
            }
            set { _barracks = value; }
        }

        public string SeasonId { get; set; }


        public double Wetting { get; set; }


        private List<OutPutApplicationInOrder> _applicationsInOrder;

        public List<OutPutApplicationInOrder> ApplicationInOrders
        {
            get
            {

                _applicationsInOrder = _applicationsInOrder ?? new List<OutPutApplicationInOrder>();
                return _applicationsInOrder;
            }
            set { _applicationsInOrder = value; }
        }

        private List<PhenologicalPreOrder> _phenologicalPreOrders;

        public List<PhenologicalPreOrder> PhenologicalPreOrders
        {
            get
            {

                _phenologicalPreOrders = _phenologicalPreOrders ?? new List<PhenologicalPreOrder>();
                return _phenologicalPreOrders;
            }
            set { _phenologicalPreOrders = value; }
        }

    }




    public class OutputBarrackInstance : BarrackOrderInstance
    {
        public List<OutputOrderNotificationEvent> Events { get; set; }
    }

    public class OutPutApplicationInOrder : ApplicationsInOrder {

        public Product Product { get; set; }

    }

    

    public class OutputOrderNotificationEvent {
        public string Id { get; set; }

        public PhenologicalEvent PhenologicalEvent { get; set; }

        /// <summary>
        /// Ruta o Url en internet de la imagen subida.
        /// </summary>
        public string PicturePath { get; set; }

        /// <summary>
        /// Descripcion del evento
        /// </summary>
        public string Description { get; set; }


        /// <summary>
        /// Fecha de creación.
        /// </summary>
        public DateTime Created { get; set; }

    }
}
