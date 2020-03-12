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
    public class BusinessNameOperations : MainOperation<BusinessName>, IGenericOperation<BusinessName, BusinessNameInput> {

        public BusinessNameOperations(IMainGenericDb<BusinessName> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<BusinessName> commonDb) : base(repo, existElement, search, commonDb) { }
        
        public async Task<string> Validate(BusinessNameInput input) {
            string errors = string.Empty;
            if (!string.IsNullOrWhiteSpace(input.Id)) {
                var existsId = await existElement.ExistsById<BusinessName>(input.Id);
                if (!existsId)
                    errors += "No existe el id de la razón social a modificar.";
                var existEditName = await existElement.ExistsWithPropertyValue<BusinessName>("Name", input.Name, input.Id);
                if (existEditName)
                    errors += "El nombre de la razón social, ya existe en otra.";
                var existsEmail = await existElement.ExistsWithPropertyValue<BusinessName>("Email", input.Email, input.Id);
                if (existsEmail)
                    errors += "El correo electrónico de la razón social, ya existe en otra.";
                var existsRut = await existElement.ExistsWithPropertyValue<BusinessName>("Rut", input.Rut, input.Id);
                if (existsRut)
                    errors += "El rut de la razón social, ya existe en otra.";
            }
            else {
                var existsName = await existElement.ExistsWithPropertyValue<BusinessName>("Name", input.Name);
                if(existsName)
                    errors += "El nombre de la razón social, ya existe en otra.";
                var existsEmail = await existElement.ExistsWithPropertyValue<BusinessName>("Email", input.Email);
                if (existsEmail)
                    errors += "El correo electrónico de la razón social, ya existe en otra.";
                var existsRut = await existElement.ExistsWithPropertyValue<BusinessName>( "Rut", input.Rut);
                if (existsRut)
                    errors += "El rut de la razón social, ya existe en otra.";
            }

            return errors.Replace(".",".\r\n");  
        }

        public async Task<ExtPostContainer<string>> Save(BusinessNameInput input) {
            var validaBusinessName = await Validate(input);
            if (!string.IsNullOrEmpty(validaBusinessName))
                throw new Exception(validaBusinessName);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var businessName = new BusinessName {
                Id = id,
                Name = input.Name,
                Email = input.Email,
                Giro = input.Giro,
                Phone = input.Phone,
                Rut = input.Rut,
                WebPage = input.WebPage
            };
            await repo.CreateUpdate(businessName);
            var properties = new List<Property> {
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
            if (!string.IsNullOrWhiteSpace(input.WebPage)) {
                properties.Add(new Property {
                    PropertyIndex = (int)PropertyRelated.GENERIC_WEBPAGE,
                    Value = input.WebPage
                });
            }
            if (!string.IsNullOrWhiteSpace(input.Phone)) {
                properties.Add(new Property {
                    PropertyIndex = (int)PropertyRelated.GENERIC_PHONE,
                    Value = input.Phone
                });
            }
            if (!string.IsNullOrWhiteSpace(input.Giro)) {
                properties.Add(new Property {
                    PropertyIndex = (int)PropertyRelated.GENERIC_GIRO,
                    Value = input.Giro
                });
            }
            search.AddElements(new List<EntitySearch> {
                new EntitySearch{
                    Id = id,
                    EntityIndex = (int)EntityRelated.BUSINESSNAME,
                    Created = DateTime.Now,
                    RelatedProperties = properties.ToArray()
                }
            });
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok,
                Result = id
            };
        }

    }

}