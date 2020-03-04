using System;
using System.Collections.Generic;
using System.Linq;
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

namespace trifenix.agro.external.operations.entities.orders
{
    public class PreOrdersOperations : MainReadOperationName<PreOrder, PreOrderInput>, IGenericOperation<PreOrder, PreOrderInput>
    {
        private readonly ICommonQueries commonQueries;

        public PreOrdersOperations(IMainGenericDb<PreOrder> repo, IExistElement existElement, IAgroSearch search, ICommonQueries commonQueries) : base(repo, existElement, search)
        {
            this.commonQueries = commonQueries;
        }


        private async Task<string> ValidaPreOrder(PreOrderInput input) {

            if (input.PreOrderType == PreOrderType.DEFAULT)
            {
                var existsIngredient = await existElement.ExistsById<Ingredient>(input.IdIngredient);

                if (!existsIngredient) return "No existe ingrediente en PreOrden Normal";
            }

            if (input.PreOrderType == PreOrderType.PHENOLOGICAL)
            {
                var existsOrderFolder = await existElement.ExistsById<OrderFolder>(input.OrderFolderId);

                if (!existsOrderFolder) return "No existe id de carpeta de orden";
            }

            if (!input.BarracksId.Any()) return "Los cuarteles son obligatorios";


            foreach (var idBarrack in input.BarracksId)
            {
                var exists = await existElement.ExistsById<Barrack>(idBarrack);

                if (!exists) return $"el cuartel con id {idBarrack} no existe";
            }

            return string.Empty;

        }

        public async Task<ExtPostContainer<string>> Save(PreOrderInput input)
        {
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");


            var valida = await Validate(input);

            if (!valida) throw new Exception(string.Format(ErrorMessages.NotValid, "PreOrden"));

            var validaPreOrder = await ValidaPreOrder(input);

            if (!string.IsNullOrWhiteSpace(validaPreOrder)) throw new Exception(validaPreOrder);


            var preOrder = new PreOrder
            {
                Id = id,
                IdIngredient = input.IdIngredient,
                BarracksId = input.BarracksId,
                PreOrderType = input.PreOrderType,
                Name = input.Name,
                OrderFolderId = input.OrderFolderId
            };

            await repo.CreateUpdate(preOrder);


            var specieAbbv = await commonQueries.GetSpecieAbbreviationFromBarrack(input.BarracksId.First());


            var entity = new EntitySearch
            {
                Id = id,
                EntityIndex = (int)EntityRelated.PREORDER,
                Created = DateTime.Now,
                RelatedProperties = new Property[] {
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_NAME,
                            Value = input.Name
                        },
                        new Property { 
                            PropertyIndex = (int)PropertyRelated.GENERIC_ABBREVIATION,
                            Value = specieAbbv
                        }
                    },
                RelatedEnumValues = new RelatedEnumValue[] { 
                    new RelatedEnumValue{ EnumerationIndex = (int)EnumerationRelated.PREORDER_TYPE, Value = (int)input.PreOrderType }
                }
            };

            var entities = input.BarracksId.Select(s => new RelatedId { EntityIndex = (int)EntityRelated.BARRACK, EntityId = s }).ToList();


            if (input.PreOrderType == PreOrderType.DEFAULT)
            {
                entities.Add(new RelatedId
                {
                    EntityIndex = (int)EntityRelated.INGREDIENT,
                    EntityId = input.IdIngredient
                 });
            }

            if (input.PreOrderType == PreOrderType.PHENOLOGICAL)
            {
                entities.Add(new RelatedId
                {
                    EntityIndex = (int)EntityRelated.ORDER_FOLDER,
                    EntityId = input.OrderFolderId
                });
            }

            var idSeason = await commonQueries.GetSeasonId(input.BarracksId.First());

            entities.Add(new RelatedId { EntityIndex = (int)EntityRelated.SEASON, EntityId = idSeason });

            entity.RelatedIds = entities.ToArray();

            search.AddElements(new List<EntitySearch> { 
                entity             
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