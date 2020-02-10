using System;

namespace trifenix.agro.model.external.Input {

    public class ApplicationOrderInput {
        public string Name { get; set; }

        public bool isPhenological { get; set; }

        public DateTime InitDate { get; set; }

        public DateTime EndDate { get; set; }

        public double Wetting { get; set; }

        public BarrackEventInput[] BarracksInput { get; set; }

        public ApplicationInOrderInput[] Applications { get; set; }

        public string[] PreOrdersId { get; set; }

    }

    public class ApplicationInOrderInput {
        public string ProductId { get; set; }
        public double QuantityByHectare { get; set; }
        public DosesInput Doses { get; set; }

    }

    public class BarrackEventInput {

        public string IdBarrack { get; set; }
        public string[] EventsId { get; set; }

    }

}