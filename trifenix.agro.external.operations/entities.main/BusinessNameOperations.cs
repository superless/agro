using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro.core;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces;
using trifenix.agro.external.operations.res;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;

namespace trifenix.agro.external.operations.entities.main
{
    public class BusinessNameOperations : MainReadOperationName<BusinessName, BusinessNameInput>, IGenericOperation<BusinessName, BusinessNameInput>
    {
        public BusinessNameOperations(IMainGenericDb<BusinessName> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<BusinessName> commonDb) : base(repo, existElement, search, commonDb)
        {
        }
        public async Task Remove(string id)
        {

        }

        private async Task<string> ValidaBusinessName(BusinessNameInput input) {
            if (!string.IsNullOrWhiteSpace(input.Id))
            {
                var existsId = await existElement.ExistsById<BusinessName>(input.Id);

                if (!existsId) return "No existe el id de la razón social a modificar";

                var existsEmail = await existElement.ExistsWithPropertyValue<BusinessName>("Email", input.Email, input.Id);

                if (existsEmail) return "El correo electrónico de la razón social, ya existe en otra";

                var existsRut = await existElement.ExistsWithPropertyValue<BusinessName>("Rut", input.Rut, input.Id);

                if (existsRut) return "El rut de la razón social, ya existe en otra";
            }
            else {
                

                var existsEmail = await existElement.ExistsWithPropertyValue<BusinessName>("Email", input.Email);

                if (existsEmail) return "El correo electrónico de la razón social, ya existe en otra";

                var existsRut = await existElement.ExistsWithPropertyValue<BusinessName>( "Rut", input.Rut);

                if (existsRut) return "El rut de la razón social, ya existe en otra";
            }

            return string.Empty;
        
        }

        public async Task<ExtPostContainer<string>> Save(BusinessNameInput input)
        {
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");

            var businessName = new BusinessName
            {
                Id = id,
                Name = input.Name,
                Email = input.Email,
                Giro = input.Giro,
                Phone = input.Phone,
                Rut = input.Rut,
                WebPage = input.WebPage
            };

            var valida = await Validate(input);

            if (!valida) throw new Exception(string.Format(ErrorMessages.NotValid, businessName.CosmosEntityName));

            var validaBusinessName = await ValidaBusinessName(input);

            if (!string.IsNullOrWhiteSpace(validaBusinessName)) throw new Exception(validaBusinessName);


            await repo.CreateUpdate(businessName);

            var properties = new List<Property>
            {
                 new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_RUT,
                            Value = input.Rut
                        },
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_NAME,
                            Value = input.Name
                        },
                        
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_EMAIL,
                            Value = input.Email
                        }
            };

            if (!string.IsNullOrWhiteSpace(input.WebPage))
            {
                properties.Add(new Property
                {
                    PropertyIndex = (int)PropertyRelated.GENERIC_WEBPAGE,
                    Value = input.WebPage
                });
            }

            if (!string.IsNullOrWhiteSpace(input.Phone))
            {
                properties.Add(new Property
                {
                    PropertyIndex = (int)PropertyRelated.GENERIC_PHONE,
                    Value = input.Phone
                });
            }

            if (!string.IsNullOrWhiteSpace(input.Giro))
            {
                properties.Add(new Property
                {
                    PropertyIndex = (int)PropertyRelated.GENERIC_GIRO,
                    Value = input.Giro
                });
            }

            search.AddElements(new List<EntitySearch>
            {
                new EntitySearch{
                    Id = id,
                    EntityIndex = (int)EntityRelated.BUSINESSNAME,
                    Created = DateTime.Now,
                    RelatedProperties = properties.ToArray()
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