using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces;
using trifenix.agro.external.operations.res;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;

namespace trifenix.agro.external.operations.entities.main
{
    public class RootstockOperations : MainReadOperationName<Rootstock, RootStockInput>, IGenericOperation<Rootstock, RootStockInput>
    {
        public RootstockOperations(IMainGenericDb<Rootstock> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<Rootstock> commonDb) : base(repo, existElement, search, commonDb)
        {
        }
        public async Task Remove(string id)
        {

        }
        public async Task<ExtPostContainer<string>> Save(RootStockInput input)
        {
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");

            var rootstock = new Rootstock
            {
                Id = id,
                Name = input.Name,
                Abbreviation = input.Abbreviation
            };

            var valida = await Validate(input);
            if (!valida) throw new Exception(string.Format(ErrorMessages.NotValid, rootstock.CosmosEntityName));
            if (string.IsNullOrWhiteSpace(input.Id))
            {
                var validaAbbv = await existElement.ExistsWithPropertyValue<Rootstock>("Abbreviation", input.Abbreviation);
                if (validaAbbv) throw new Exception(string.Format(ErrorMessages.NotValidAbbreviation, rootstock.CosmosEntityName));
            }
            else
            {
                var validaAbbv = await existElement.ExistsWithPropertyValue<Rootstock>("Abbreviation", input.Abbreviation, input.Id);
                if (validaAbbv) throw new Exception(string.Format(ErrorMessages.NotValidAbbreviation, rootstock.CosmosEntityName));
            }


            await repo.CreateUpdate(rootstock);

            search.AddElements(new List<EntitySearch>
            {
                new EntitySearch{
                    Id = id,
                    EntityIndex = (int)EntityRelated.ROOTSTOCK,
                    Created = DateTime.Now,
                    RelatedProperties = new Property[] {
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_NAME,
                            Value = input.Name
                        },
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_ABBREVIATION,
                            Value = input.Abbreviation
                        }
                    }
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
