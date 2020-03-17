using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;

namespace trifenix.agro.external.operations.entities.main
{
    public class SpecieOperations : MainOperation<Specie, SpecieInput>, IGenericOperation<Specie, SpecieInput> {
        public SpecieOperations(IMainGenericDb<Specie> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<Specie> commonDb) : base(repo, existElement, search, commonDb) { }

        public async Task<ExtPostContainer<string>> SaveInput(SpecieInput input, bool isBatch) {
            await Validate(input, isBatch);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var specie = new Specie {
                Id = id,
                Name = input.Name,
                Abbreviation = input.Abbreviation
            };
            await repo.CreateUpdate(specie, isBatch);
            search.AddElements(new List<EntitySearch> {
                new EntitySearch{
                    Id = id,
                    EntityIndex = (int)EntityRelated.SPECIE,
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
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok,
                Result = id
            };
        }

    }

}