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

        public int GetCorrelativePosition<T>(string specieAbb) {
            string entity = typeof(T).Name;
            var counter = _repo.GetCounter();
            if (!counter.Count.ContainsKey(entity) || !counter.Count[entity].ContainsKey(specieAbb))
                IncreaseCorrelativePosition<T>(specieAbb);
            return counter.Count[entity][specieAbb];
        }

        public async void IncreaseCorrelativePosition<T>(string specieAbb) {
            string entity = typeof(T).Name;
            var counter = _repo.GetCounter();
            if (counter.Count.ContainsKey(entity))
                if (counter.Count[entity].ContainsKey(specieAbb))
                    counter.Count[entity][specieAbb]++;
                else
                    counter.Count[entity].Add(specieAbb,1);
            else
                counter.Count.Add(entity, new Dictionary<string,int> { { specieAbb, 1 } });
            await _repo.CreateUpdateCounter(counter);
        }

        public bool RemoveEntityFromCounter<T>() {
            string entity = typeof(T).Name;
            var counter = _repo.GetCounter();
            bool flag = counter.Count.Remove(entity);
            if (flag) _repo.CreateUpdateCounter(counter);
            return flag;
        }

        public bool RemoveSpecieFromCounter(string specieAbb) {
            var counter = _repo.GetCounter();
            bool flag = false;
            foreach (var entity in counter.Count.Values)
                if (entity.ContainsKey(specieAbb))
                    flag = entity.Remove(specieAbb);
            if(flag) _repo.CreateUpdateCounter(counter);
            return flag;
        }

    }
}