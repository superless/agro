using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro.common;

using System.Linq;

namespace trifenix.agro.db.applicationsReference.agro.Common
{
    public class CosmosExistElement : IExistElement
    {
        public AgroDbArguments DbArguments { get; }

        public CosmosExistElement(AgroDbArguments dbArguments)
        {
            DbArguments = dbArguments;
        }

        public async Task<bool> ExistsElement<T>(string id) where T: DocumentBase
        {
            var db = new MainDb<T>(DbArguments);

            var result = await db.Store.QuerySingleAsync<long>($"SELECT value count(1) FROM c where c.Id = '{id}'");

            return result != 0;
        }

        public async Task<bool> ExistsElementAndOperator<T>(Dictionary<string, string> nameValue) where T : DocumentBase
        { 
            

            return await ExistsElement<T>(nameValue, true);
        }

        public async Task<bool> ExistsElementOrOperator<T>(Dictionary<string, string> nameValue) where T : DocumentBase
        {
            return await ExistsElement<T>(nameValue, false);
        }

        public async Task<bool> ExistsElement<T>(Dictionary<string, string> nameValue, bool isAnd) where T : DocumentBase
        {
            var queryProps = nameValue.Select(s => $"c.{s.Key} = {nameValue}").ToArray();

            var whereQuery = string.Join(isAnd?" and": " or ", queryProps);

            var db = new MainDb<T>(DbArguments);

            var result = await db.Store.QuerySingleAsync<long>($"SELECT value count(1) FROM c where {whereQuery}");

            return result != 0;
        }

        public async Task<bool> ExistsElement<T>(string namePropCheck, string valueCheck) where T : DocumentBase
        {
            var db = new MainDb<T>(DbArguments);
            var result = await db.Store.QuerySingleAsync<long>($"SELECT value count(1) FROM c where {namePropCheck} = '{valueCheck}'");

            return result != 0;
        }

        public async Task<bool> ExistsElement<T>(string namePropCheck, int valueCheck) where T : DocumentBase
        {
            var db = new MainDb<T>(DbArguments);
            var result = await db.Store.QuerySingleAsync<long>($"SELECT value count(1) FROM c where {namePropCheck} = {valueCheck}");

            return result != 0;
        }

        public async Task<bool> ExistsEditElement<T>(string id, string nameCheck, string valueCheck) where T : DocumentBase
        {
            var db = new MainDb<T>(DbArguments);
            var result = await db.Store.QuerySingleAsync<long>($"SELECT value count(1) FROM c where {nameCheck} = '{valueCheck}' and c.Id != '{id}'");

            return result != 0;
        }

        


    }
}
