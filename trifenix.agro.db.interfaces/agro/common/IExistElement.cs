using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace trifenix.agro.db.interfaces.agro.common
{
    public interface IExistElement
    {
        Task<bool> ExistsElement<T>(string id) where T : DocumentBase;

        Task<bool> ExistsEditElement<T>(string id, string nameCheck, string valueCheck) where T : DocumentBase;

        Task<bool> ExistsElementAndOperator<T>(Dictionary<string, string> NameValue) where T : DocumentBase;

        Task<bool> ExistsElementOrOperator<T>(Dictionary<string, string> NameValue) where T : DocumentBase;

        Task<bool> ExistsElement<T>(string namePropCheck, string valueCheck) where T : DocumentBase;

        Task<bool> ExistsElement<T>(string namePropCheck, int valueCheck) where T : DocumentBase;
    }
}
