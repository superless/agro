using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.tests.data;
using trifenix.connect.agro.tests.mock;
using trifenix.connect.agro_model_input;
using trifenix.connect.util;
using Xunit;

namespace trifenix.connect.agro.tests
{
    public class UsuarioTest
        {
        [Fact]
        public async Task InsertUserSuccess()
        {

            //assign
            var agroManager = MockHelper.AgroManager;

            //Trabajo
            var jobInput = await agroManager.Job.SaveInput(new JobInput
            {
                Name = "Agronomo"
            });

            var job = await agroManager.Job.Get(jobInput.IdRelated);

            Assert.True(job.Result.Name.Equals("Agronomo"));

            //Rol
            var roleInput1 = await agroManager.Role.SaveInput(new RoleInput
            {
                Name = "Recogedor",
            });

            var role1 = await agroManager.Role.Get(roleInput1.IdRelated);

            Assert.True(role1.Result.Name.Equals("Recogedor"));

            var roleInput2 = await agroManager.Role.SaveInput(new RoleInput
            {
                Name = "Cultivador",
            });

            var role2 = await agroManager.Role.Get(roleInput2.IdRelated);

            Assert.True(role2.Result.Name.Equals("Cultivador"));

            //Tractor
            var tractorInput = await agroManager.Tractor.SaveInput(new TractorInput
            {
                Brand = "Kubota",
                Code = "B2320"
            });

            var tractor = await agroManager.Tractor.Get(tractorInput.IdRelated);

            Assert.True(tractor.Result.Brand.Equals("Kubota"));

            //Nebulizadora
            var nebulizerInput = await agroManager.Nebulizer.SaveInput(new NebulizerInput
            {
                Brand = "AM162",
                Code = "927002"
            });

            var nebulizer = await agroManager.Nebulizer.Get(nebulizerInput.IdRelated);

            Assert.True(nebulizer.Result.Brand.Equals("AM162"));

            //Usuario
            var userInput = new UserApplicatorInput
            {
                IdNebulizer = nebulizer.Result.Id,
                IdTractor = tractor.Result.Id,
                Name = "Trifenix",
                Email = "Trifenix2@trifenix.io",
                Rut = "20100100-9",
                IdJob = job.Result.Id,
                IdsRoles = new List<string> {
                        role1.Result.Id, role2.Result.Id
                     }

            };
            var userInputTest = await agroManager.UserApplicator.SaveInput(userInput);

            var user = await agroManager.UserApplicator.Get(userInputTest.IdRelated);

            Assert.Equal(userInput.Name, user.Result.Name);

            var compareModel = Mdm.Validation.CompareModel(
                userInput,
                user.Result,
                new Dictionary<Type, Func<object, IEnumerable<object>>>
                {
                }
                );
            Assert.True(compareModel);

        }
    }
}
