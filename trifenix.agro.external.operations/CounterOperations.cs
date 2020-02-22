using System.Collections.Generic;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model;
using trifenix.agro.external.interfaces;

namespace trifenix.agro.external.operations {
    public class CounterOperations : ICounterOperations {

        private readonly ICounterRepository _repo;

        public CounterOperations(ICounterRepository CounterRepo) {
            _repo = CounterRepo;
        }

        public Counter GetCounter() {
            var counter = _repo.GetCounter();
            return counter;
        }

        public int GetCorrelativePosition(string entity, string specieAbb) {
            var counter = _repo.GetCounter();
            if (counter.Count.ContainsKey(entity))
                if (counter.Count[entity].ContainsKey(specieAbb))
                    return counter.Count[entity][specieAbb];
                else
                    return -2;
            else
                return -1;
        }

        public void IncreaseCorrelativePosition(string entity, string specieAbb) {
            var counter = _repo.GetCounter() ?? new Counter();
            if (counter.Count.ContainsKey(entity))
                if (counter.Count[entity].ContainsKey(specieAbb))
                    counter.Count[entity][specieAbb]++;
                else
                    counter.Count[entity].Add(specieAbb,1);
            else
                counter.Count.Add(entity, new Dictionary<string,int> { { specieAbb, 1 } });
        }

        public bool RemoveEntityFromCounter(string entity) {
            var counter = _repo.GetCounter();
            return counter.Count.Remove(entity);
        }

        public bool RemoveSpecieFromCounter(string specieAbb) {
            var counter = _repo.GetCounter();
            bool flag = false;
            foreach (var entity in counter.Count.Values) {
                if (entity.ContainsKey(specieAbb)) {
                    flag = entity.Remove(specieAbb);
                }
            }
            return flag;
        }

    }
}