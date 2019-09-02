namespace trifenix.agro.model.enforcements.products
{
    /// <summary>
    /// days before harvest
    /// </summary>
    public class Cadence {
        public string Id { get; set; }
        public int Days { get; set; }

        public CertifierRegion CertifierRegion { get; set; }


    }

}
