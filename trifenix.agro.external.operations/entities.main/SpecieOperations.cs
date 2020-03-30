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

namespace trifenix.agro.external.operations.entities.main
{
    public class SpecieOperations : MainOperation<Specie, SpecieInput>, IGenericOperation<Specie, SpecieInput> {
        public SpecieOperations(IMainGenericDb<Specie> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<Specie> commonDb, IValidator validators) : base(repo, existElement, search, commonDb, validators) { }

        public Task Remove(string id) {
            throw new NotImplementedException();
        }

        public async Task<ExtPostContainer<string>> Save(Specie specie) {
            await repo.CreateUpdate(specie);
            search.AddElements(new List<EntitySearch> {
                new EntitySearch{
                    Id = specie.Id,
                    EntityIndex = (int)EntityRelated.SPECIE,
                    Created = DateTime.Now,
                    RelatedProperties = new Property[] {
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_NAME,
                            Value = specie.Name
                        },
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_ABBREVIATION,
                            Value = specie.Abbreviation
                        }
                    }
                }
            });
            return new ExtPostContainer<string> {
                IdRelated = specie.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(SpecieInput input, bool isBatch) {
            await Validate(input);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var specie = new Specie {
                Id = id,
                Name = input.Name,
                Abbreviation = input.Abbreviation
            };
            if (!isBatch)
                return await Save(specie);
            await repo.CreateEntityContainer(specie);
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok
            };
        }

    }

}