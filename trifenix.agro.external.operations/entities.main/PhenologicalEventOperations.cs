using Cosmonaut.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces;
using trifenix.agro.external.interfaces.entities.main;
using trifenix.agro.external.operations.helper;
using trifenix.agro.external.operations.res;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;

namespace trifenix.agro.external.operations.entities.main
{
    public class PhenologicalEventOperations : MainReadOperationName<PhenologicalEvent, PhenologicalEventInput>, IGenericOperation<PhenologicalEvent, PhenologicalEventInput>
    {
        public PhenologicalEventOperations(IMainGenericDb<PhenologicalEvent> repo, IExistElement existElement, IAgroSearch search) : base(repo, existElement, search)
        {
        }

        public async Task<ExtPostContainer<string>> Save(PhenologicalEventInput input)
        {
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");

            var phenologicalEvent = new PhenologicalEvent
            {
                Id = id,
                Name = input.Name,
                InitDate = input.StartDate,
                EndDate = input.EndDate
            };

            var valida = await Validate(input);
            if (!valida) throw new Exception(string.Format(ErrorMessages.NotValid, phenologicalEvent.CosmosEntityName));

            if (input.EndDate < input.StartDate) throw new Exception(ErrorMessages.InitDateisOlderThenEndDate);

            await repo.CreateUpdate(phenologicalEvent);

            search.AddElements(new List<SimpleSearch>
            {
                new SimpleSearch{
                    Created = DateTime.Now,
                    Id = id,
                    Name = input.Name,
                    EntityName = phenologicalEvent.CosmosEntityName
                }
            });


            return new ExtPostContainer<string>
            {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok,
                Result = id
            };
        }

        
    }
}
