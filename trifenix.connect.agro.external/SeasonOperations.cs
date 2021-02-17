﻿using System;
using System.Linq;
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
    /// Operaciones para el ingreso correcto de una temporada
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SeasonOperations<T> : MainOperation<Season, SeasonInput, T>, IGenericOperation<Season, SeasonInput>
    {
        private readonly ICommonAgroQueries Queries;

        public SeasonOperations(IDbExistsElements existsElement, IMainGenericDb<Season> repo, IAgroSearch<T> search, ICommonDbOperations<Season> commonDb, ICommonAgroQueries queries, IValidatorAttributes<SeasonInput> validator) : base(repo, search, commonDb, validator)
        {
            Queries = queries;
        }

        public async override Task Validate(SeasonInput input)
        {
            await base.Validate(input);
            if (input.Current)
            {
                var season = await Queries.GetCostCenterActiveSeason(input.IdCostCenter);
                if (season.Any())
                {
                    throw new Exception("Ya existe una temporada activa en este centro de costos");
                }
            }

            if (input.StartDate >= input.EndDate)
            {
                throw new Exception("Fecha invalida");
            }
        
            // 6 meses de intervalo minimo
            if (input.StartDate.AddMonths(6) > input.EndDate)
            {
                throw new Exception("Fecha invalida");
            }
        
        }

        public override async Task<ExtPostContainer<string>> SaveInput(SeasonInput input)
        {
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            await Validate(input);

            var season = new Season
            {
                Id = id,
                StartDate = input.StartDate,
                EndDate = input.EndDate,
                Current = input.Current,
                IdCostCenter = input.IdCostCenter
            };
            await SaveDb(season);
            return await SaveSearch(season);
        }
    }
}