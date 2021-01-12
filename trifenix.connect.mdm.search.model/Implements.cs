
using Microsoft.Spatial;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using trifenix.connect.search_mdl;

namespace trifenix.connect.mdm.search.model
{
    public class ImplementsSearch : Implements<GeographyPoint>
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

        public Type geo => typeof(GeoProperty);


        // refactorizar.
        public Func<object, GeographyPoint> GeoObjetoToGeoSearch => (ob)=>GeographyPoint.Create(0,0);

        public Type GetEntitySearchImplementedType => typeof(EntitySearch);
    }





}