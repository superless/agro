using System;
using System.Threading.Tasks;
using trifenix.connect.agro.external.main;
using trifenix.connect.agro.interfaces;
using trifenix.connect.agro.interfaces.cosmos;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.interfaces.db.cosmos;
using trifenix.connect.interfaces.external;
using trifenix.connect.mdm.containers;

namespace trifenix.agro.external
{

    /// <summary>
    /// Operaciones para el ingreso correcto de un documento de bodega
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CostCenterOperations<T> : MainOperation<CostCenter, CostCenterInput, T>, IGenericOperation<CostCenter, CostCenterInput>
    {
        private readonly ICommonAgroQueries Queries;

        public CostCenterOperations(IDbExistsElements existsElement, IMainGenericDb<CostCenter> repo, IAgroSearch<T> search, ICommonDbOperations<CostCenter> commonDb, ICommonAgroQueries queries, IValidatorAttributes<CostCenterInput> validator) : base(repo, search, commonDb, validator)
        {
            Queries = queries;
        }

        public async override Task Validate(CostCenterInput input)
        {
            var bn = await Queries.GetCostCenterFromBusinessName(input.IdBusinessName);
            var activeBn = int.Parse(bn);
            if (activeBn != 0)
            {
                throw new Exception("Ya existe un cost center asociado a este business name");
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