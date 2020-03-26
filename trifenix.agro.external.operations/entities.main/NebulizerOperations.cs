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
using trifenix.agro.external.operations.res;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;

namespace trifenix.agro.external.operations.entities.main
{
    public class NebulizerOperations : MainReadOperation<Nebulizer>, IGenericOperation<Nebulizer, NebulizerInput>
    {
        public NebulizerOperations(IMainGenericDb<Nebulizer> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<Nebulizer> commonDb) : base(repo, existElement, search, commonDb)
        {
        }
        public async Task Remove(string id)
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
                var existsId = await existElement.ExistsById<Nebulizer>(input.Id);

                if (!existsId) throw new Exception(string.Format(ErrorMessages.NotValidId, "Nebulizadora"));

                var existsCode = await existElement.ExistsWithPropertyValue<Nebulizer>("Code", input.Code, input.Id);

                if (existsCode) throw new Exception("El código ya existe en otra nebulizadora");
            } else {
                var existsCode = await existElement.ExistsWithPropertyValue<Nebulizer>("Code", input.Code);

                if (existsCode) throw new Exception("El código ya existe en otra nebulizadora");
            }

            await repo.CreateUpdate(nebulizer);

            search.AddElements(new List<EntitySearch>
            {
                new EntitySearch{
                    Id = id,
                    EntityIndex = (int)EntityRelated.NEBULIZER,
                    Created = DateTime.Now,
                    RelatedProperties = new Property[] {
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_BRAND,
                            Value = input.Brand
                        },
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_CODE,
                            Value = input.Code
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
