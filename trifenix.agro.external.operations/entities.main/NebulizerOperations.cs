using System;
using System.Collections.Generic;
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

namespace trifenix.agro.external.operations.entities.main {
    public class NebulizerOperations : MainReadOperation<Nebulizer>, IGenericOperation<Nebulizer, NebulizerInput>
    {
        public NebulizerOperations(IMainGenericDb<Nebulizer> repo, IExistElement existElement, IAgroSearch search) : base(repo, existElement, search)
        {
        }

        public async Task<ExtPostContainer<string>> Save(NebulizerInput input)
        {
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");

            var nebulizer = new Nebulizer
            {
                Id = id,
                Brand = input.Brand,
                Code = input.Code
            };

            if (!string.IsNullOrWhiteSpace(input.Id))
            {
                var existsId = await existElement.ExistsElement<Nebulizer>(input.Id);

                if (!existsId) throw new Exception(string.Format(ErrorMessages.NotValidId, "Nebulizadora"));

                var existsCode = await existElement.ExistsEditElement<Nebulizer>(input.Id, "Code", input.Code);

                if (existsCode) throw new Exception("El código ya existe en otra nebulizadora");
            } else {
                var existsCode = await existElement.ExistsElement<Nebulizer>("Code", input.Code);

                if (existsCode) throw new Exception("El código ya existe en otra nebulizadora");
            }

            await repo.CreateUpdate(nebulizer);

            search.AddElements(new List<EntitySearch>
            {
                new EntitySearch{
                    Created = DateTime.Now,
                    Id = id,
                    Name = input.Code,
                    EntityIndex = nebulizer.CosmosEntityName,
                    ElementsRelated = new List<ElementRelated>(){ 
                        new ElementRelated{ EntityIndex = (int)EntityRelated.NEBULIZER_BRAND, Name = input.Brand },
                        new ElementRelated{ EntityIndex = (int)EntityRelated.NEBULIZER_CODE, Name = input.Code },
                    }.ToArray()
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
