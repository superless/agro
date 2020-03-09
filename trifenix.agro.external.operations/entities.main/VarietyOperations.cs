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
    public class VarietyOperations : MainReadOperationName<Variety, VarietyInput>, IGenericOperation<Variety, VarietyInput>
    {
        public VarietyOperations(IMainGenericDb<Variety> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<Variety> commonDb) : base(repo, existElement, search, commonDb)
        {
        }

        public async Task<ExtPostContainer<string>> Save(VarietyInput input)
        {
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");

            var variety = new Variety
            {
                Id = id,
                Name = input.Name,
                Abbreviation = input.Abbreviation,
                IdSpecie = input.IdSpecie
            };

            var valida = await Validate(input);
            if (!valida) throw new Exception(string.Format(ErrorMessages.NotValid, variety.CosmosEntityName));

            var existsSector = await existElement.ExistsById<Specie>(input.IdSpecie);
            if (!existsSector) throw new Exception(string.Format(ErrorMessages.NotValidId, "Especie"));

            if (string.IsNullOrWhiteSpace(input.Id))
            {
                var validaAbbv = await existElement.ExistsWithPropertyValue<Variety>("Abbreviation", input.Abbreviation);
                if (validaAbbv) throw new Exception(string.Format(ErrorMessages.NotValidAbbreviation, variety.CosmosEntityName));

            }
            else
            {
                var validaAbbv = await existElement.ExistsWithPropertyValue<Variety>("Abbreviation", input.Abbreviation, input.Id);
                if (validaAbbv) throw new Exception(string.Format(ErrorMessages.NotValidAbbreviation, variety.CosmosEntityName));
            }


            await repo.CreateUpdate(variety);

            var varietySearch = new List<EntitySearch> {
                new EntitySearch{
                    Id = id,
                    EntityIndex = (int)EntityRelated.VARIETY,
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
                    },
                    RelatedIds = new RelatedId[]{
                        new RelatedId{
                            EntityIndex = (int)EntityRelated.SPECIE,
                            EntityId = input.IdSpecie
                        }
                    }
                }
            };



            search.AddElements(varietySearch);


            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok,
                Result = id
            };
        }
    }
}
