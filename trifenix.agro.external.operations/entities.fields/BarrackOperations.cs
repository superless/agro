using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces;
using trifenix.agro.external.operations.res;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;

namespace trifenix.agro.external.operations.entities.fields
{
    public class BarrackOperations : MainReadOperationName<Barrack, BarrackInput>, IGenericOperation<Barrack, BarrackInput>
    {
        private readonly ICommonQueries commonQueries;

        public BarrackOperations(IMainGenericDb<Barrack> repo, IExistElement existElement, IAgroSearch search, ICommonQueries commonQueries) : base(repo, existElement, search)
        {
            this.commonQueries = commonQueries;
        }


        private async Task<string> ValidaBarrack(BarrackInput input) {

            var seasonExists = await existElement.ExistsElement<Season>(input.SeasonId);

            if (!seasonExists) return "No existe la temporada";

            var plotLandExists = await existElement.ExistsElement<PlotLand>(input.IdPlotLand);

            if (!plotLandExists) return "No existe parcela";

            var varietyExists = await existElement.ExistsElement<Variety>(input.IdVariety);

            if (!varietyExists) return "no existe variedad";

            var rootStockExists = await existElement.ExistsElement<Rootstock>(input.IdRootstock);

            if (!rootStockExists) return "no existe portainjerto";


            var existsPollinator = await existElement.ExistsElement<Variety>(input.IdPollinator);

            if (!existsPollinator) return "No existe polinizador";

            return string.Empty;

        }

        public async Task<ExtPostContainer<string>> Save(BarrackInput input)
        {

            
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");

            var specie = new Barrack
            {
                Id = id,
                Name = input.Name,
                
            };

            var valida = await Validate(input);
            if (!valida) throw new Exception(string.Format(ErrorMessages.NotValid, specie.CosmosEntityName));
            
            var validaBarrak = await ValidaBarrack(input);

            if (!string.IsNullOrWhiteSpace(validaBarrak)) throw new Exception(validaBarrak);



            await repo.CreateUpdate(specie);


            var specieAbbv = await commonQueries.GetSpecieAbbreviationFromVariety(input.IdVariety);


            search.AddElements(new List<EntitySearch>
            {
                new EntitySearch{
                    Created = DateTime.Now,
                    Id = id,
                    ElementsRelated= new ElementRelated[]{ 
                        new ElementRelated{ 
                            EntityIndex = (int)PropertyRelated.GENERIC_NAME,
                            Name = input.Name
                        },
                        new ElementRelated{ 
                            EntityIndex = (int)PropertyRelated.SPECIE_CODE,
                            Name = specieAbbv
                        }
                    },
                    EntityIndex = (int)EntityRelated.BARRACK,
                    IdsRelated = new IdsRelated[]{ 
                        new IdsRelated{ EntityIndex = (int)EntityRelated.SEASON , EntityId = input.SeasonId },
                        new IdsRelated{ EntityIndex = (int)EntityRelated.PLOTLAND, EntityId = input.IdPlotLand },
                        new IdsRelated { EntityIndex = (int)EntityRelated.VARIETY, EntityId = input.IdVariety },
                        new IdsRelated { EntityIndex = (int)EntityRelated.POLLINATOR, EntityId = input.IdPollinator },
                        new IdsRelated { EntityIndex = (int)EntityRelated.ROOTSTOCK, EntityId = input.IdRootstock }

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
