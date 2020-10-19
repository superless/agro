using System;
using System.Collections.Generic;
using System.Text;
using trifenix.connect.mdm.entity_model;
using trifenix.connect.mdm.search.model;
using trifenix.connect.mdm.ts_model;
using trifenix.connect.search_mdl;

namespace trifenix.connect.agro.tests
{
    public class ImplementMock : Implements<GeoPointTs>
    {
        public Type num32 => typeof(Num32Property);

        public Type dbl => typeof(DblProperty);

        public Type bl => typeof(BoolProperty);

        public Type num64 => typeof(Num64Property);

        public Type dt => typeof(DtProperty);

        public Type enm => typeof(EnumProperty);

        public Type rel => typeof(RelatedId);

        public Type str => typeof(StrProperty);

        public Type sug => typeof(StrProperty);

        public Type geo => typeof(GeoPropertyMock);


        // refactorizar.
        public Func<object, GeoPointTs> GeoObjetoToGeoSearch => (ob) => new GeoPointTs { latitude =0, longitude = 0 };
    }
    public class GeoPropertyMock : BaseProperty<GeoPointTs>, IProperty<GeoPointTs> { }
}
