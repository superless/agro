using Moq;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model.agro;
using trifenix.agro.external.operations.tests.helper.Moqs;

namespace trifenix.agro.external.operations.tests.helper.Instances
{
    public static class SpeciesIntances {
        public static Mock<ISpecieRepository> GetInstance(Results result) =>
           MoqGenerator.GetMoqResult<ISpecieRepository, Specie>(
               result,
               (s) => s.CreateUpdateSpecie(It.IsAny<Specie>()),
               (s) => s.GetSpecie(It.IsAny<string>()),
               s => s.GetSpecies());
    }
}
