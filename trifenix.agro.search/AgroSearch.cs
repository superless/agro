using Microsoft.Azure.Search;
using System;

namespace trifenix.agro.search
{
    public class AgroSearch
    {
        public static SearchServiceClient GetSearchClient(string name, string key) => new SearchServiceClient(name, new SearchCredentials(key));


    }
}
