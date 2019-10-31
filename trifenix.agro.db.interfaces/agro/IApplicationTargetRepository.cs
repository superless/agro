﻿using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.interfaces.agro
{
    public interface IApplicationTargetRepository
    {
        Task<string> CreateUpdateTargetApp(ApplicationTarget target);

        Task<ApplicationTarget> GetTarget(string id);

        IQueryable<ApplicationTarget> GetTargets();
    }
}
