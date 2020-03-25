using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model;
using trifenix.agro.db.model.agro;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;

namespace trifenix.agro.external.operations.entities.orders
{
    public class OrderFolderOperations : MainReadOperation<OrderFolder>, IGenericOperation<OrderFolder, OrderFolderInput>
    {
        private readonly ICommonQueries commonQueries;

        public OrderFolderOperations(IMainGenericDb<OrderFolder> repo, IExistElement existElement, IAgroSearch search, ICommonQueries commonQueries, ICommonDbOperations<OrderFolder> commonDb) : base(repo, existElement, search, commonDb)
        {
            this.commonQueries = commonQueries;
        }
        public async Task Remove(string id)
        {

        }
        private async Task<string> Valida(OrderFolderInput input)
        {
            if (string.IsNullOrWhiteSpace(input.IdCategoryIngredient) && string.IsNullOrWhiteSpace(input.IdIngredient))
            {
                return "la categoría de ingrediente y el ingrediente ";
            }

            if (!string.IsNullOrWhiteSpace(input.IdIngredient))
            {
                var existsIngredient = await existElement.ExistsById<Ingredient>(input.IdIngredient);

                if (!existsIngredient) return "El ingrediente no existe";
            }

            if (!string.IsNullOrWhiteSpace(input.IdCategoryIngredient))
            {
                var existsCategory = await existElement.ExistsById<IngredientCategory>(input.IdCategoryIngredient);

                if (!existsCategory) return "La categoría no existe";

            }

            var existsPhenological = await existElement.ExistsById<PhenologicalEvent>(input.IdPhenologicalEvent);


            if (!existsPhenological) return "El evento fenológico no existe";

            var existsSpecie = await existElement.ExistsById<Specie>(input.IdSpecie);

            if (!existsSpecie) return "Especie no existe";

            var existsTarget = await existElement.ExistsById<ApplicationTarget>(input.IdApplicationTarget);

            if (!existsTarget) return "No existe objetivo de la aplicación";

            if (!string.IsNullOrWhiteSpace(input.Id))
            {
                var existsId = await existElement.ExistsById<OrderFolder>(input.Id);

                if (!existsId) return "No existe el id de la carpeta a modificar";
            }

            return string.Empty;


        }

        public async Task<ExtPostContainer<string>> Save(OrderFolderInput input)
        {
            var valida = await Valida(input);
            if (!string.IsNullOrWhiteSpace(valida)) throw new Exception(valida);

            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");

            var specie = new OrderFolder
            {
                Id = id,
                IdApplicationTarget = input.IdApplicationTarget,
                IdIngredientCategory = input.IdCategoryIngredient,
                IdIngredient = input.IdIngredient,
                IdPhenologicalEvent = input.IdPhenologicalEvent,
                IdSpecie = input.IdSpecie
            };

            await repo.CreateUpdate(specie);


            var specieAbbv = await commonQueries.GetSpecieAbbreviation(input.IdSpecie);




            search.AddElements(new List<EntitySearch>
            {
                new EntitySearch{
                    Created = DateTime.Now,
                    Id = id,
                    RelatedIds = new RelatedId[]{ 
                        new RelatedId{ EntityIndex = (int)EntityRelated.TARGET, EntityId = input.IdApplicationTarget  },
                        new RelatedId{ EntityIndex = (int)EntityRelated.CATEGORY_INGREDIENT, EntityId = input.IdCategoryIngredient  },
                        new RelatedId{ EntityIndex = (int)EntityRelated.PHENOLOGICAL_EVENT, EntityId = input.IdPhenologicalEvent },
                        new RelatedId{ EntityIndex = (int)EntityRelated.INGREDIENT, EntityId = input.IdIngredient },
                        new RelatedId{ EntityIndex = (int)EntityRelated.PREORDER, EntityId = input.IdSpecie}
                    },
                    RelatedProperties = new Property[]{ 
                        new Property{ 
                            PropertyIndex = (int)PropertyRelated.GENERIC_ABBREVIATION,
                            Value = specieAbbv
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
