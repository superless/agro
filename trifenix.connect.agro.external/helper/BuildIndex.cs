namespace trifenix.connect.agro.external.helper
{
    public class BuildIndex<GeoPointType> //: IBuildIndex<GeoPointType>
    {
        //private readonly IEntitySearchOper<GeoPointType> baseSearch;

        //public BuildIndex(IEntitySearchOper<GeoPointType> baseSearch)
        //{
        //    this.baseSearch = baseSearch;
        //}

        //public BuildIndex(string SearchServiceName, string SearchServiceKey, string entityIndex, CorsOptions corsOptions)
        //{
        //    this.baseSearch = new EntitySearchMgmt<GeoPointType>(SearchServiceName, SearchServiceKey, entityIndex, corsOptions);
        //}
        ///// <summary>
        ///// Agrega al índice los datos de cosmosDb.
        ///// </summary>
        ///// <param name="agro">IAgroManager, el cual conecta a Operations</param>
        //public async Task GenerateIndex(IAgroManager<GeoPointType> agro)
        //{
        //    var assm = typeof(BusinessName).Assembly;
        //    var types = assm.GetTypes().Where(type => type.GetProperty("CosmosEntityName") != null && !(new[] { typeof(EntityContainer), typeof(User), typeof(UserActivity), typeof(Comment) }).Contains(type)).ToList();
        //    foreach (var type in types)
        //    {
        //        try
        //        {
        //            var extGetContainer = await agro.GetOperationByDbType(type).GetElements();
        //            var elements = (IEnumerable<dynamic>)extGetContainer.Result;
        //            elements?.ToList().ForEach(element => baseSearch.AddDocument(element));
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.StackTrace);
        //        }
        //    }
        //}

        ///// <summary>
        ///// Regenera el índice a partir de los datos de cosmosDb.
        ///// </summary>
        ///// <param name="agro">IAgroManager, el cual conecta a Operations</param>
        //public async Task RegenerateIndex(IAgroManager<GeoPointType> agro)
        //{
        //    baseSearch.BaseSearch.EmptyIndex();
        //    await GenerateIndex(agro);
        //}
    }
}
