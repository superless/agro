using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro.core;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;

namespace trifenix.agro.external.operations.entities.main {
    public class BusinessNameOperations : MainOperation<BusinessName, BusinessNameInput>, IGenericOperation<BusinessName, BusinessNameInput> {

        public BusinessNameOperations(IMainGenericDb<BusinessName> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<BusinessName> commonDb) : base(repo, existElement, search, commonDb) { }
        
        public async Task<ExtPostContainer<string>> Save(BusinessName businessName) {
            await repo.CreateUpdate(businessName);
            var properties = new List<Property> {
                new Property { PropertyIndex = (int)PropertyRelated.GENERIC_RUT, Value = businessName.Rut },
                new Property { PropertyIndex = (int)PropertyRelated.GENERIC_NAME, Value = businessName.Name },       
                new Property { PropertyIndex = (int)PropertyRelated.GENERIC_EMAIL, Value = businessName.Email }
            };
            if (!string.IsNullOrWhiteSpace(businessName.WebPage))
                properties.Add(new Property { PropertyIndex = (int)PropertyRelated.GENERIC_WEBPAGE, Value = businessName.WebPage });
            if (!string.IsNullOrWhiteSpace(businessName.Phone))
                properties.Add(new Property { PropertyIndex = (int)PropertyRelated.GENERIC_PHONE, Value = businessName.Phone });
            if (!string.IsNullOrWhiteSpace(businessName.Giro))
                properties.Add(new Property { PropertyIndex = (int)PropertyRelated.GENERIC_GIRO, Value = businessName.Giro });
            search.AddElements(new List<EntitySearch> {
                new EntitySearch{
                    Id = businessName.Id,
                    EntityIndex = (int)EntityRelated.BUSINESSNAME,
                    Created = DateTime.Now,
                    RelatedProperties = properties.ToArray()
                }
            });
            return new ExtPostContainer<string> {
                IdRelated = businessName.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(BusinessNameInput input, bool isBatch) {
            await Validate(input, isBatch);
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
            if (!isBatch)
                return await Save(businessName);
            await repo.CreateEntityContainer(businessName);
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok
            };
        }

    }

}