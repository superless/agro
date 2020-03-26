using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro;
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
    public class RootstockOperations : MainOperation<Rootstock, RootstockInput>, IGenericOperation<Rootstock, RootstockInput> {
        public RootstockOperations(IMainGenericDb<Rootstock> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<Rootstock> commonDb, IValidator validators) : base(repo, existElement, search, commonDb, validators) { }

        public Task Remove(string id) {
            throw new NotImplementedException();
        }

        public async Task<ExtPostContainer<string>> Save(Rootstock rootstock) {
            await repo.CreateUpdate(rootstock);
            search.AddElements(new List<EntitySearch> {
                new EntitySearch{
                    Id = rootstock.Id,
                    EntityIndex = (int)EntityRelated.ROOTSTOCK,
                    Created = DateTime.Now,
                    RelatedProperties = new Property[] {
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_NAME,
                            Value = rootstock.Name
                        },
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_ABBREVIATION,
                            Value = rootstock.Abbreviation
                        }
                    }
                }
            });
            return new ExtPostContainer<string> {
                IdRelated = rootstock.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(RootstockInput input, bool isBatch) {
            await Validate(input);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var rootstock = new Rootstock {
                Id = id,
                Name = input.Name,
                Abbreviation = input.Abbreviation
            };
            if (!isBatch)
                return await Save(rootstock);
            await repo.CreateEntityContainer(rootstock);
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok
            };
        }

    }

}