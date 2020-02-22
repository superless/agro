using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model;
using trifenix.agro.db.model.agro.core;
using trifenix.agro.db.model.local;
using trifenix.agro.external.interfaces.entities.core;
using trifenix.agro.external.operations.helper;
using trifenix.agro.microsoftgraph.interfaces;
using trifenix.agro.model.external;

namespace trifenix.agro.external.operations.entities.main {
    public class BusinessNameOperations : IBusinessNameOperations {

        private readonly IBusinessNameRepository _repo;
        private readonly ICommonDbOperations<BusinessName> _commonDb;
        private readonly IGraphApi _graphApi;

        public BusinessNameOperations(IBusinessNameRepository repo, ICommonDbOperations<BusinessName> commonDb, IGraphApi graphApi) {
            _repo = repo;
            _commonDb = commonDb;
            _graphApi = graphApi;
        }

        public async Task<ExtGetContainer<BusinessName>> GetBusinessName(string id) {
            var businessName = await _repo.GetBusinessName(id);
            return OperationHelper.GetElement(businessName);
        }

        public async Task<ExtGetContainer<List<BusinessName>>> GetBusinessNames() {
            var queryTargets = _repo.GetBusinessNames();
            var targets = await _commonDb.TolistAsync(queryTargets);
            return OperationHelper.GetElements(targets);
        }

        public async Task<ExtPostContainer<BusinessName>> SaveEditBusinessName(string id, string name, string rut, string phone, string email, string webPage, string giro) {
            UserApplicator modifier = await _graphApi.GetUserFromToken();
            UserActivity userActivity = new UserActivity(DateTime.Now, modifier);
            var businessName = await _repo.GetBusinessName(id);
            return await OperationHelper.EditElement(_commonDb, _repo.GetBusinessNames(), 
                id,
                businessName,
                s => {
                    s.Name = name;
                    s.Rut = rut;
                    s.Phone = phone;
                    s.Email = email;
                    s.WebPage = webPage;
                    s.Giro = giro;
                    s.Modify = userActivity;
                    return s;
                },
                _repo.CreateUpdateBusinessName,
                $"No existe objetivo aplicación con id: {id}",
                s => (s.Name.Equals(name) && !businessName.Name.Equals(name)) || (s.Rut.Equals(rut) && !businessName.Rut.Equals(rut)),
               $"Ya existe rol social con nombre: {name} o rut: {rut}. Ambos campos deben ser unicos."
            );

        }

        public async Task<ExtPostContainer<string>> SaveNewBusinessName(string name, string rut, string phone, string email, string webPage, string giro) {
            UserApplicator creator = await _graphApi.GetUserFromToken();
            UserActivity userActivity = new UserActivity(DateTime.Now, creator); 
            return await OperationHelper.CreateElement(_commonDb,_repo.GetBusinessNames(),
                async s => await _repo.CreateUpdateBusinessName(new BusinessName {
                    Id = s,
                    Name = name,
                    Rut = rut,
                    Phone = phone,
                    Email = email,
                    WebPage = webPage,
                    Giro = giro,
                    Modify = userActivity
                }),
                s => s.Name.Equals(name) || s.Rut.Equals(rut),
                $"Ya existe rol social con nombre: {name} o rut: {rut}. Ambos campos deben ser unicos."
            );
        }

    }
}