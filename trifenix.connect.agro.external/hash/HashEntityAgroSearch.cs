using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.interfaces.hash;
using trifenix.connect.search_mdl;
using trifenix.connect.util;

namespace trifenix.connect.agro.external.hash
{
    public class HashEntityAgroSearch : IHashSearchHelper
    {
        public string HashHeader(Type type) 
        {
            // obtenemos los diccionarios desde las enumeraciones.
            var dictRel = Mdm.Reflection.Enumerations.GetDictionaryFromEnum<EntityRelated>();
            var dictEnum = Mdm.Reflection.Enumerations.GetDictionaryFromEnum<EnumRelated>();
            var dictNum = Mdm.Reflection.Enumerations.GetDictionaryFromEnum<NumRelated>();
            var dictStr = Mdm.Reflection.Enumerations.GetDictionaryFromEnum<StringRelated>();
            var dictDbl = Mdm.Reflection.Enumerations.GetDictionaryFromEnum<DoubleRelated>();
            var dictDt = Mdm.Reflection.Enumerations.GetDictionaryFromEnum<DateRelated>();
            var dictGeo = Mdm.Reflection.Enumerations.GetDictionaryFromEnum<GeoRelated>();
            var dictBool = Mdm.Reflection.Enumerations.GetDictionaryFromEnum<BoolRelated>();

            // obtiene los índices de la clase
            var indexes = Mdm.PreLoadedDictionary(type);

            if (indexes == null)
            {
                return string.Empty;
            }



            // asigna colecciones de dictionary de los índices de propiedades de un objeto de una base de datos de persistencia.
            var dict = new JsonDictionaryHeaders
            {
                bl = indexes.bl.ToDictionary(s => s, s => dictBool[s]),
                dbl = indexes.dbl.ToDictionary(s => s, s => dictDbl[s]),
                dt = indexes.dt.ToDictionary(s => s, s => dictDt[s]),
                enm = indexes.enm.ToDictionary(s => s, s => dictEnum[s]),
                geo = indexes.geo.ToDictionary(s => s, s => dictGeo[s]),
                index = indexes.index,
                num64 = indexes.num32.ToDictionary(s => s, s => dictNum[s]),
                num32 = indexes.num64.ToDictionary(s => s, s => dictNum[s]),
                rel = indexes.rel.ToDictionary(s => s, s => dictRel[s]),
                str = indexes.str.ToDictionary(s => s, s => dictStr[s]),
                sug = indexes.sug.ToDictionary(s => s, s => dictStr[s])
            };



            
            // serializa para el hash
            var jsonDict = JsonConvert.SerializeObject(dict);


            // retorna hash
            return Mdm.Reflection.Cripto.ComputeSha256Hash(jsonDict);
        }

        
        public string HashModel(object model) 
        {
            var serialize = JsonConvert.SerializeObject(model, new JavaScriptDateTimeConverter());



            return Mdm.Reflection.Cripto.ComputeSha256Hash(serialize);
        }


    }

    

    
}
