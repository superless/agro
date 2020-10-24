using System;
using trifenix.connect.mdm.entity_model;
using trifenix.connect.mdm.ts_model;

namespace trifenix.connect.tests.mock
{
    /// <summary>
    /// implementación mocking de propiedad geo.
    /// </summary>
    public class GeoPropertyMock : IProperty<GeoPointTs>
    {
        public int index { get; set; }
        
        public GeoPointTs value { get; set; }
    }
}
