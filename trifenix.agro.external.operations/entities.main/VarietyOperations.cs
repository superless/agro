using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model;
using trifenix.agro.enums;
using trifenix.agro.enums.input;
using trifenix.agro.enums.searchModel;
using trifenix.agro.external.interfaces;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;
using trifenix.agro.validator.interfaces;

namespace trifenix.agro.external.operations.entities.main {
    public class VarietyOperations : MainOperation<Variety, VarietyInput>, IGenericOperation<Variety, VarietyInput> {
        public VarietyOperations(IMainGenericDb<Variety> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<Variety> commonDb, IValidator validators) : base(repo, existElement, search, commonDb, validators) { }

        public Task Remove(string id) {
            throw new NotImplementedException();
        }

        public async Task<ExtPostContainer<string>> Save(Variety variety) {
            await repo.CreateUpdate(variety);
            search.AddElements(new List<EntitySearch> {
                new EntitySearch{
                    Id = variety.Id,
                    EntityIndex = (int)EntityRelated.VARIETY,
                    Created = DateTime.Now,
                    RelatedProperties = new Property[] {
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_NAME,
                            Value = variety.Name
                        },
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_ABBREVIATION,
                            Value = variety.Abbreviation
                        }
                    },
                    RelatedIds = new RelatedId[]{
                        new RelatedId{
                            EntityIndex = (int)EntityRelated.SPECIE,
                            EntityId = variety.IdSpecie
                        }
                    }
                }
            });
            return new ExtPostContainer<string> {
                IdRelated = variety.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(VarietyInput input, bool isBatch) {
            await Validate(input);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var variety = new Variety {
                Id = id,
                Name = input.Name,
                Abbreviation = input.Abbreviation,
                IdSpecie = input.IdSpecie
            };
            if (!isBatch)
                return await Save(variety);
            await repo.CreateEntityContainer(variety);
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok
            };
        }

    }

}