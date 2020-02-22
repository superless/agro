using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro.ext;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model;
using trifenix.agro.external.interfaces.entities.ext;
using trifenix.agro.external.operations.helper;
using trifenix.agro.model.external;

namespace trifenix.agro.external.operations.entities.ext
{
    public class CertifiedEntityOperations : ICertifiedEntityOperations
    {
        private readonly ICertifiedEntityRepository _repo;
        private readonly ICommonDbOperations<CertifiedEntity> _commonDb;


        public CertifiedEntityOperations(ICertifiedEntityRepository repo, ICommonDbOperations<CertifiedEntity> commonDb )
        {
            _repo = repo;
            _commonDb = commonDb;
        }

        public async Task<ExtGetContainer<List<CertifiedEntity>>> GetCertifiedEntities()
        {
            try
            {
                var certifiedEntitiesQuery = _repo.GetCertifiedEntities();
                var certifiedEntities = await _commonDb.TolistAsync(certifiedEntitiesQuery);
                return OperationHelper.GetElements(certifiedEntities);
            }
            catch (Exception e)
            {

                return OperationHelper.GetException<List<CertifiedEntity>>(e);
            }


        }

        public async Task<ExtGetContainer<CertifiedEntity>> GetCertifiedEntity(string id)
        {
            try
            {
                var certifiedEntity = await _repo.GetCertifiedEntity(id);

                return OperationHelper.GetElement(certifiedEntity);
            }
            catch (Exception e)
            {

                return OperationHelper.GetException<CertifiedEntity>(e);
            }
        }

        public async Task<ExtPostContainer<CertifiedEntity>> SaveEditCertifiedEntity(string id, string name, string abbreviation)
        {
            try
            {
                var element = await _repo.GetCertifiedEntity(id);
                return await OperationHelper.EditElement(_commonDb, _repo.GetCertifiedEntities(), id,
                    element,
                    s =>
                    {
                        s.Name = name;
                        s.Abbreviation = abbreviation;
                        return s;
                    },
                    _repo.CreateUpdateCertifiedEntity,
                    $"no existe entidad certificadora con nombre {name}",
                    s => s.Name.Equals(name) && name!=element.Name,
                    $"Este nombre ya existe"
             );

            }
            catch (Exception e)
            {
                return OperationHelper.GetPostException<CertifiedEntity>(e);
            }



        }

        public async Task<ExtPostContainer<string>> SaveNewCertifiedEntity(string name, string abbreviation)
        {
            return await OperationHelper.CreateElement(_commonDb, _repo.GetCertifiedEntities(),
                async s => await _repo.CreateUpdateCertifiedEntity(new CertifiedEntity {
                    Id = s,
                    Name = name,
                    Abbreviation = abbreviation
                }),
                s => s.Name.Equals(name),
                $"ya existe entidad certificadora con nombre {name}"

                );
        }
    }
}
