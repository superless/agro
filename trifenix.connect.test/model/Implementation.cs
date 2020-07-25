using System;
using System.Collections.Generic;
using System.Text;
using trifenix.connect.mdm.entity_model;
using trifenix.connect.mdm.ts_model;
using trifenix.connect.search_mdl;

namespace trifenix.connect.test.model
{
    public class Implementation : Implements<GeoPointTs>
    {
        public Type num32 => typeof(Num32BaseProperty);

        public Type dbl => typeof(DblBaseProperty);

        public Type bl => typeof(BoolBaseProperty);

        public Type num64 => typeof(Num64BaseProperty);

        public Type dt => typeof(DtBaseProperty);

        public Type enm => typeof(EnumBaseProperty);

        public Type rel => typeof(RelatedBaseId);

        public Type str => typeof(StrBaseProperty);

        public Type sug => typeof(StrBaseProperty);

        public Type geo => typeof(GeographyProperty);

        public Func<object, GeoPointTs> GeoObjetoToGeoSearch => (elem)=>(GeoPointTs)elem;

    }
}
