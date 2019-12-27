using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.external.interfaces.entities.main;
using trifenix.agro.external.operations.helper;
using trifenix.agro.model.external;

namespace trifenix.agro.external.operations.entities.main
{
    public class RoleOperations : IRoleOperations
    {
        private readonly IRoleRepository _repo;
        private readonly ICommonDbOperations<Role> _commonDb;
        public RoleOperations(IRoleRepository repo, ICommonDbOperations<Role> commonDb)
        {
            _repo = repo;
            _commonDb = commonDb;
        }
        public async Task<ExtGetContainer<List<Role>>> GetRoles()
        {
            var queryTargets = _repo.GetRoles();
            var targets = await _commonDb.TolistAsync(queryTargets);
            return OperationHelper.GetElements(targets);
        }

        public async Task<ExtPostContainer<Role>> SaveEditRole(string id, string name)
        {
            var element = await _repo.GetRole(id);
            return await OperationHelper.EditElement(_commonDb, _repo.GetRoles(), 
                id,
                element,
                s => {
                    s.Name = name;
                    return s;
                },
                _repo.CreateUpdateRole,
                 $"No existe objetivo aplicación con id: {id}",
                s => s.Name.Equals(name) && name != element.Name,
                $"Ya existe rol con nombre: {name}"
            );

        }

        public async Task<ExtPostContainer<string>> SaveNewRole(string name)
        {
            return await OperationHelper.CreateElement(_commonDb,_repo.GetRoles(),
                async s => await _repo.CreateUpdateRole(new Role
                {
                    Id = s,
                    Name = name
                }),
                s => s.Name.Equals(name),
                $"Ya existe rol con nombre: {name}"

            );
        }
    }
}
