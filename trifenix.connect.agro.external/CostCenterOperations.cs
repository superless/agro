using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using trifenix.connect.agro.external.main;
using trifenix.connect.agro.interfaces.db;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.interfaces.db;
using trifenix.connect.interfaces.external;
using trifenix.connect.mdm.containers;
using trifenix.exception;

namespace trifenix.agro.external
{

    /// <summary>
    /// Operaciones para el ingreso correcto de un documento de bodega
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CostCenterOperations<T> : MainOperation<CostCenter, CostCenterInput, T>, IGenericOperation<CostCenter, CostCenterInput>
    {
        private readonly ICommonAgroQueries Queries;

        public CostCenterOperations(IMainGenericDb<CostCenter> repo, IAgroSearch<T> search, ICommonAgroQueries queries, IValidatorAttributes<CostCenterInput> validator, ILogger log) : base(repo, search, validator, log)
        {
            Queries = queries;
        }

        public async override Task Validate(CostCenterInput input)
        {
            await base.Validate(input);
            var bn = await Queries.GetCostCenterFromBusinessName(input.IdBusinessName);
            if (bn.Any())
            {
                throw new CustomException("Ya existe un cost center asociado a este business name");
            }
        }

        public override async Task<ExtPostContainer<string>> SaveInput(CostCenterInput input)
        {
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");

            await Validate(input);

            var costCenter = new CostCenter
            {
                Id = id,
                Name = input.Name,
                IdBusinessName = input.IdBusinessName
            };
            await SaveDb(costCenter);
            return await SaveSearch(costCenter);
        }
    }
}