namespace trifenix.agro.db.model.local {

   /// <summary>
   /// Días de espera antes de cosechar
   /// </summary>
    public class WaitingHarvest {
        
        /// <summary>
        /// días de espera antes de la cosecha
        /// </summary>
        public int WaitingDays { get; set; }

        /// <summary>
        /// Entidad certificadora (opcional), si es indicado en la etiqueta, probablemente no sea de una entidad certificadora.
        /// </summary>
        public CertifiedEntity CertifiedEntity { get; set; }

    }
}