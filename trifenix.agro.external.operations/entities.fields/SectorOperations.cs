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

namespace trifenix.agro.external.operations.entities.fields {

    public class SectorOperations : MainOperation<Sector, SectorInput>, IGenericOperation<Sector, SectorInput> {
        public SectorOperations(IMainGenericDb<Sector> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<Sector> commonDb) : base(repo, existElement, search, commonDb) { }
        
        public async Task<ExtPostContainer<string>> Save(Sector sector) {
            await repo.CreateUpdate(sector);
            search.AddElements(new List<EntitySearch> {
                new EntitySearch{
                    Id = sector.Id,
                    EntityIndex = (int)EntityRelated.SECTOR,
                    Created = DateTime.Now,
                    RelatedProperties = new Property[] {
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_NAME,
                            Value = sector.Name
                        }
                    }
                }
            });
            return new ExtPostContainer<string> {
                IdRelated = sector.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(SectorInput input, bool isBatch) {
            await Validate(input, isBatch);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var sector = new Sector {
                Id = id,
                Name = input.Name
            };
            if (!isBatch)
                return await Save(sector);
            await repo.CreateEntityContainer(sector);
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok
            };
        }
   
    }

}