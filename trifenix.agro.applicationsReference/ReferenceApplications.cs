using System;
using System.Threading.Tasks;
using trifenix.agro.db.exceptions;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.model.enforcements.ApplicationOrders;
using trifenix.agro.db.model.enforcements.stages;
using trifenix.agro.model.external;

using Cosmonaut.Extensions;
using System.Linq;
using System.Collections.Generic;
using trifenix.agro.db.model.enforcements.products;
using trifenix.agro.db.model.enforcements.Fields;
using trifenix.agro.db.model.enforcements.@base;
using trifenix.agro.model.external.Helper;
using trifenix.agro.db.model.enforcements.Applications;
using trifenix.agro.model.external.@base;
using trifenix.agro.model.external.applications;
using trifenix.agro.external.interfaces;
using trifenix.agro.db.model.enforcements;

namespace trifenix.agro.applicationsReference
{
    public class ReferenceApplications : IReferenceApplications
    {
        
        private readonly IBaseContainer _baseContainer;
        public ReferenceApplications(IBaseContainer baseContainer)
        {  
            _baseContainer = baseContainer;
        }

        public ReferenceApplications()
        {
        }

        public async Task<ExtGetContainer<List<ApplicationPurpose>>> GetApplicationPurposes()
        {
            try
            {
                var elements = await _baseContainer.ApplicationPurposes.GetApplicationPurposes().ToListAsync();

                return new ExtGetContainer<List<ApplicationPurpose>>
                {
                    Result = elements,
                    StatusResult = elements.Any() ? ExtGetDataResult.Success : ExtGetDataResult.EmptyResults
                };
            }
            catch (Exception exception)
            {

                return new ExtGetErrorContainer<List<ApplicationPurpose>>
                {
                    StatusResult = ExtGetDataResult.Error,
                    ErrorMessage = exception.Message,
                    InternalException = exception
                };
            }
        }

        public async Task<ExtGetContainer<List<ActiveIngredientCategory>>> GetCategories()
        {
            try
            {
                var elements = await _baseContainer.ActiveIngredientCategories.GetCategories().ToListAsync();
                return new ExtGetContainer<List<ActiveIngredientCategory>>
                {
                    Result = elements,
                    StatusResult = elements.Any() ? ExtGetDataResult.Success : ExtGetDataResult.EmptyResults
                };
            }
            catch (Exception exception)
            {
                return new ExtGetErrorContainer<List<ActiveIngredientCategory>>
                {
                    StatusResult = ExtGetDataResult.Error,
                    ErrorMessage = exception.Message,
                    InternalException = exception
                };
            }
        }

        public async Task<ExtGetContainer<List<ActiveIngredient>>> GetIngredients()
        {
            try
            {
                var elements = await _baseContainer.ActiveIngredients.GetActiveIngredients().ToListAsync();

                return new ExtGetContainer<List<ActiveIngredient>>
                {
                    Result = elements,
                    StatusResult = elements.Any() ? ExtGetDataResult.Success : ExtGetDataResult.EmptyResults
                };
            }
            catch (Exception exception)
            {
                return new ExtGetErrorContainer<List<ActiveIngredient>>
                {
                    StatusResult = ExtGetDataResult.Error,
                    ErrorMessage = exception.Message,
                    InternalException = exception
                };
            }
        }

        public async Task<ExtGetContainer<List<PhenologicalEvent>>> GetPhenologicalEvents()
        {
            try
            {
                var elements = await _baseContainer.PhenologicalEvents.GetPhenologicalEvents().ToListAsync();

                return new ExtGetContainer<List<PhenologicalEvent>> {
                    Result = elements,
                    StatusResult = elements.Any()? ExtGetDataResult.Success : ExtGetDataResult.EmptyResults
                };
            }
            catch (Exception exception)
            {
                return new ExtGetErrorContainer<List<PhenologicalEvent>>
                {
                    StatusResult = ExtGetDataResult.Error,
                    ErrorMessage = exception.Message,
                    InternalException = exception
                };
            }
        }

        

        public async Task<ExtPostContainer<string>> SaveActiveIngredient(string name, string idCategory)
        {
            try
            {
                var category = await _baseContainer.ActiveIngredientCategories.GetCategory(idCategory);
                if (category == null)
                {
                    return new ExtPostContainer<string>
                    {
                        IdRelated = string.Empty,
                        Result = string.Empty,
                        MessageResult = ExtMessageResult.ChildRequiredDoesNotExists,
                        Message = $"No existe categoria con id {idCategory}"
                    };
                }

                var ingredientFound = await _baseContainer.ActiveIngredients.GetActiveIngredients().FirstOrDefaultAsync(s => s.Name.ToLower().Equals(name.ToLower()));

                if (ingredientFound != null)
                {
                    return new ExtPostContainer<string>
                    {
                        IdRelated = ingredientFound.Id,
                        Message = $"Ya existe ingrediente activo con nombre {name}",
                        MessageResult = ExtMessageResult.ElementAlreadyExists
                    };
                }
                var newIngredient = new ActiveIngredient
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Category = category,
                    Name = name
                };

                var resultDb = await _baseContainer.ActiveIngredients.CreateUpdateActiveIngredient(newIngredient);
                return new ExtPostContainer<string>
                {
                    IdRelated = resultDb,
                    Result = resultDb,
                    Message = string.Empty,
                    MessageResult = ExtMessageResult.Ok
                };


            }
            catch (Exception e)
            {

                return new ExtPostErrorContainer<string>
                {
                    IdRelated = string.Empty,
                    Message = e.Message,
                    MessageResult = ExtMessageResult.Error,
                    InternalException = new DbException<ActiveIngredient>(new ActiveIngredient
                    {
                        Id = Guid.NewGuid().ToString("N"),                        
                        Name = name
                    }, e)
                };
            }
        }

        public async Task<ExtPostContainer<string>> SaveActiveIngredientCategory(string name)
        {
            var category = new ActiveIngredientCategory
            {
                Id = Guid.NewGuid().ToString("N"),
                Name = name
            };

            try
            {
                var categoryFound = await _baseContainer.ActiveIngredientCategories.GetCategories().FirstOrDefaultAsync(s => s.Name.ToLower().Equals(name.ToLower()));
                if (categoryFound != null)
                {
                    return new ExtPostContainer<string>
                    {
                        IdRelated = categoryFound.Id,
                        Result = categoryFound.Id,
                        MessageResult = ExtMessageResult.ElementAlreadyExists,
                        Message = $"La categoría con nombre {name} ya existe"
                    };
                }

                var resultDb = await _baseContainer.ActiveIngredientCategories.CreateUpdateCategory(category);
                return new ExtPostContainer<string>
                {
                    IdRelated = resultDb,
                    Result = resultDb,
                    Message = string.Empty,
                    MessageResult = ExtMessageResult.Ok
                };
            }
            catch (Exception e)
            {

                return new ExtPostErrorContainer<string>
                {
                    IdRelated = category.Id,
                    Message = e.Message,
                    MessageResult = ExtMessageResult.Error,
                    InternalException = new DbException<ActiveIngredientCategory>(category, e)
                };
            }
        }

        public async Task<ExtPostContainer<string>> SaveApplicationPurpose(string name)
        {
            var applicationPurpose = new ApplicationPurpose
            {
                Id = Guid.NewGuid().ToString("N"),
                Name = name
            };
            try
            {
                var appPurposeFound = await _baseContainer.ApplicationPurposes.GetApplicationPurposes().FirstOrDefaultAsync(s => s.Name.ToLower().Equals(name.ToLower()));
                if (appPurposeFound !=null)
                {
                    return new ExtPostContainer<string>
                    {
                        IdRelated = appPurposeFound.Id,
                        Result = appPurposeFound.Id,
                        MessageResult = ExtMessageResult.ElementAlreadyExists,
                        Message = $"El propósito de la aplicación con nombre {name} ya existe"
                    };
                }

                var resultDb = await _baseContainer.ApplicationPurposes.CreateUpdateApplicationPurpose(applicationPurpose);
                return new ExtPostContainer<string>
                {
                    IdRelated = resultDb,
                    Message = string.Empty,
                    MessageResult = ExtMessageResult.Ok,
                    Result = resultDb
                };

            }
            catch (Exception e)
            {

                return new ExtPostErrorContainer<string>
                {
                    IdRelated = applicationPurpose.Id,
                    Message = e.Message,
                    MessageResult = ExtMessageResult.Error,
                    InternalException = new DbException<ApplicationPurpose>(applicationPurpose, e)
                };

            }

        }

        public async Task<ExtPostContainer<string>> SavePhenologicalEvent(string name, DateTime date)
        {


            var phenologicalEvent = new PhenologicalEvent
            {
                Name = name,
                Id = Guid.NewGuid().ToString("N"),
                InitDate = date
            };



            try
            {
                var phenologicalFound = await _baseContainer.PhenologicalEvents.GetPhenologicalEvents().FirstOrDefaultAsync(s => s.Name.ToLower().Equals(name.ToLower()));

                if (phenologicalFound != null)
                {
                    return new ExtPostContainer<string>
                    {
                        IdRelated = phenologicalFound.Id,
                        Result = phenologicalFound.Id,
                        MessageResult = ExtMessageResult.ElementAlreadyExists,
                        Message = $"El evento fenológico con nombre {name} ya existe"
                    };
                }

                var resultDb = await _baseContainer.PhenologicalEvents.CreateUpdatePhenologicalEvent(phenologicalEvent);

                return new ExtPostContainer<string>
                {
                    IdRelated = resultDb,
                    MessageResult = ExtMessageResult.Ok,
                    Result  = resultDb
                };

            }
            catch (Exception e)
            {

                return new ExtPostErrorContainer<string>
                {
                    IdRelated = phenologicalEvent.Id,
                    Message = e.Message,
                    MessageResult = ExtMessageResult.Error,
                    InternalException = new DbException<PhenologicalEvent>(phenologicalEvent, e)
                };
            }
        }

        public async Task<ExtPostContainer<string>> SaveSpecie(string name, string abbreviation)
        {
            var specie = new AgroSpecie
            {
                Id = Guid.NewGuid().ToString("N"),
                Name = name,
                Abbreviation = abbreviation
                
            };

            try
            {
                var specieFound = await _baseContainer.Species.GetSpecies().FirstOrDefaultAsync(s => s.Name.ToLower().Equals(name.ToLower()) || s.Abbreviation.ToLower().Equals(abbreviation.ToLower()));
                if (specieFound != null)
                {
                    return new ExtPostContainer<string>
                    {
                        IdRelated = specieFound.Id,
                        Result = specieFound.Id,
                        MessageResult = ExtMessageResult.ElementAlreadyExists,
                        Message = $"La Especie con nombre {name} o abreviación {abbreviation} ya existe"
                    };
                }

                var resultDb = await _baseContainer.Species.CreateUpdateSpecie(specie);
                return new ExtPostContainer<string>
                {
                    IdRelated = resultDb,
                    Result = resultDb,
                    Message = string.Empty,
                    MessageResult = ExtMessageResult.Ok
                };
            }
            catch (Exception e)
            {

                return new ExtPostErrorContainer<string>
                {
                    IdRelated = specie.Id,
                    Message = e.Message,
                    MessageResult = ExtMessageResult.Error,
                    InternalException = new DbException<AgroSpecie>(specie, e)
                };
            }
        }
        public async Task<ExtPostContainer<string>> SaveSeason(DateTime init, DateTime end)
        {
            var season = new AgroYear
            {
                Id = Guid.NewGuid().ToString("N"),
                Start = init,
                End = end,
                Current = true
            };
            try
            {
                

                if (end < DateTime.Now)
                {
                    return new ExtPostContainer<string>
                    {
                        IdRelated = string.Empty,
                        Message = "La fecha no puede ser menor al día de hoy",
                        MessageResult = ExtMessageResult.Error
                    };
                }

                var seasonFound = await _baseContainer.Seasons.GetSeasons().FirstOrDefaultAsync(s => s.Current && s.End > DateTime.Now);
                if (seasonFound != null)
                {
                    return new ExtPostContainer<string>
                    {
                        IdRelated = string.Empty,
                        Message = $"ya existe una temporada activa y actual",
                        MessageResult = ExtMessageResult.Error
                    };
                }

                var resultDb = await _baseContainer.Seasons.CreateUpdateSeason(season);




                return new ExtPostContainer<string>
                {
                    IdRelated = resultDb,
                    Result = resultDb,
                    Message = string.Empty,
                    MessageResult = ExtMessageResult.Ok
                };
            }
            catch (Exception e)
            {

                return new ExtPostErrorContainer<string>
                {
                    IdRelated = string.Empty,
                    Message = e.Message,
                    MessageResult = ExtMessageResult.Error,
                    InternalException = new DbException<AgroYear>(season, e)
                };
            }

        }
        public async Task<ExtPostContainer<string>> SaveVariety(string name, string abbreviation, string idSpecie)
        {
            try
            {
                var specieFound = await _baseContainer.Species.Getspecie(idSpecie);
                if (specieFound == null)
                {
                    return new ExtPostContainer<string>
                    {
                        IdRelated = string.Empty,
                        Result = string.Empty,
                        MessageResult = ExtMessageResult.ChildRequiredDoesNotExists,
                        Message = $"No existe especie con id {idSpecie}"
                    };
                }

                var varieryFound = await _baseContainer.Varieties.GetVarieties().FirstOrDefaultAsync(s => s.Name.ToLower().Equals(name.ToLower()) || s.Abbreviation.ToLower().Equals(abbreviation.ToLower()));

                if (varieryFound != null)
                {
                    return new ExtPostContainer<string>
                    {
                        IdRelated = varieryFound.Id,
                        Message = $"Ya existe una variedad con nombre {name} o con abreviacion {abbreviation}",
                        MessageResult = ExtMessageResult.ElementAlreadyExists
                    };
                }
                var newIngredient = new AgroVariety
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Specie = specieFound,
                    Name = name,
                    Abbreviation = abbreviation
                };

                var resultDb = await _baseContainer.Varieties.CreateUpdateVariety(newIngredient);
                return new ExtPostContainer<string>
                {
                    IdRelated = resultDb,
                    Result = resultDb,
                    Message = string.Empty,
                    MessageResult = ExtMessageResult.Ok
                };


            }
            catch (Exception e)
            {

                return new ExtPostErrorContainer<string>
                {
                    IdRelated = string.Empty,
                    Message = e.Message,
                    MessageResult = ExtMessageResult.Error,
                    InternalException = new DbException<AgroVariety>(new AgroVariety
                    {
                        Id = Guid.NewGuid().ToString("N"),
                        Name = name,
                        Abbreviation = abbreviation
                    }, e)
                };
            }
        }

        public async Task<ExtGetContainer<List<AgroSpecie>>> GetSpecies()
        {
            try
            {
                var elements = await _baseContainer.Species.GetSpecies().ToListAsync();

                return new ExtGetContainer<List<AgroSpecie>>
                {
                    Result = elements,
                    StatusResult = elements.Any() ? ExtGetDataResult.Success : ExtGetDataResult.EmptyResults
                };
            }
            catch (Exception exception)
            {
                return new ExtGetErrorContainer<List<AgroSpecie>>
                {
                    StatusResult = ExtGetDataResult.Error,
                    ErrorMessage = exception.Message,
                    InternalException = exception
                };
            }
        }

        public async Task<ExtGetContainer<List<AgroVariety>>> GetVarieties()
        {
            try
            {
                var elements = await _baseContainer.Varieties.GetVarieties().ToListAsync();

                return new ExtGetContainer<List<AgroVariety>>
                {
                    Result = elements,
                    StatusResult = elements.Any() ? ExtGetDataResult.Success : ExtGetDataResult.EmptyResults
                };
            }
            catch (Exception exception)
            {
                return new ExtGetErrorContainer<List<AgroVariety>>
                {
                    StatusResult = ExtGetDataResult.Error,
                    ErrorMessage = exception.Message,
                    InternalException = exception
                };
            }
        }

        

        public async Task<ExtGetContainer<bool>>  CurrentSeasonExists()
        {
            try
            {
                var season = await _baseContainer.Seasons.GetSeasons().FirstOrDefaultAsync(s => s.Current && s.End > DateTime.Now);
                return new ExtGetContainer<bool>
                {
                    Result = season!=null,
                    StatusResult = ExtGetDataResult.Success
                };
            }
            catch (Exception e)
            {

                return new ExtGetErrorContainer<bool>
                {
                    StatusResult = ExtGetDataResult.Error,
                    ErrorMessage = e.Message,
                    InternalException = e
                };
            }
        }

        public async Task<ExtPostContainer<string>> SaveField(string name, string abbreviation, double hectares, string[] varieties, string precessor = null)
        {
            try
            {
                var currentSeason = await _baseContainer.Seasons.GetSeasons().FirstOrDefaultAsync(s => s.End > DateTime.Now && s.Current);
                if (currentSeason == null)
                {
                    return new ExtPostContainer<string>
                    {
                        IdRelated = string.Empty,
                        MessageResult = ExtMessageResult.ChildRequiredDoesNotExists,
                        Message = $"No existe temporada para agregar un campo"
                    };
                }

                var fieldFound = await _baseContainer.Fields.GetAgroFields().FirstOrDefaultAsync(s => s.Abbreviation.ToLower().Equals(abbreviation) || s.Name.ToLower().Equals(name.ToLower()));

                if (fieldFound != null)
                {
                    return new ExtPostContainer<string>
                    {
                        Message = $"Ya existe un campo con el mismo nombre ({name}) o abreviación ({abbreviation})",
                        MessageResult = ExtMessageResult.ElementAlreadyExists
                    };
                }

                var varietiesFound = await _baseContainer.Varieties.GetVarieties().ToListAsync();

                if (!varieties.All(v => varietiesFound.Any(s => s.Id.Equals(v))))
                {
                    return new ExtPostContainer<string>
                    {
                        MessageResult = ExtMessageResult.ChildRequiredDoesNotExists,
                        Message = $"Existen variedades que no existen en la base de datos"
                    };
                }

                var varietiesElements = varieties.Select(s => varietiesFound.First(a => a.Id.Equals(s))).ToList();

                var field = new AgroField
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Abbreviation = abbreviation,
                    Hectares = hectares,
                    Name = name,
                    Precessor = precessor,
                    Season = currentSeason,
                    Varieties = varietiesElements
                };
                var resultDb = await _baseContainer.Fields.CreateUpdateAgroField(field);
                return new ExtPostContainer<string>
                {
                    IdRelated = resultDb,
                    Result = resultDb,
                    Message = string.Empty,
                    MessageResult = ExtMessageResult.Ok
                };
            }
            catch (Exception e)
            {

                return new ExtPostErrorContainer<string>
                {
                    IdRelated = string.Empty,
                    Message = e.Message,
                    MessageResult = ExtMessageResult.Error,
                    InternalException = new DbException<AgroField>(new AgroField
                    {
                        Id = Guid.NewGuid().ToString("N"),
                        Name = name,
                        Abbreviation = abbreviation,
                        Hectares = hectares,
                        Precessor = precessor
                    }, e)
                };
            }


        }

        public async Task<ExtGetContainer<List<AgroField>>> GetFields()
        {
            try
            {
                var elements = await _baseContainer.Fields.GetAgroFields().Where(s=>s.Season.Current).ToListAsync();

                return new ExtGetContainer<List<AgroField>>
                {
                    Result = elements,
                    StatusResult = elements.Any() ? ExtGetDataResult.Success : ExtGetDataResult.EmptyResults
                };
            }
            catch (Exception exception)
            {
                return new ExtGetErrorContainer<List<AgroField>>
                {
                    StatusResult = ExtGetDataResult.Error,
                    ErrorMessage = exception.Message,
                    InternalException = exception
                };
            }
        }

        public async Task<ExtGetContainer<ExtSeason>> GetCurrentSeason()
        {
            try
            {
                var currentSeason = await _baseContainer.Seasons.GetSeasons().FirstOrDefaultAsync(s => s.Current);

                if (currentSeason == null)
                {
                    return new ExtGetContainer<ExtSeason>
                    {
                        Result = null,
                        StatusResult = ExtGetDataResult.EmptyResults
                    };
                }

                var extSeason = new ExtSeason
                {
                    InitDate = currentSeason.Start,
                    EndDate = currentSeason.End
                };


                var allPhenologicalOrdersAsync = await _baseContainer.PhenologicalRefApps.GetPhenologicalAppRefs().ToListAsync();
                var allPhenologicalOrders = allPhenologicalOrdersAsync.Where(s => s.Applications.All(a => a.Field.Season.Current)).ToList();
                var allDateOrdersAsync = await _baseContainer.DateRefApps.GetDateAppRefs().ToListAsync();
                var allDateOrders = allDateOrdersAsync.Where(s => s.Applications.All(a => a.Field.Season.Current)).ToList();

                var continuedOrderAsync = await _baseContainer.ContinuedRefApps.GetContinuedAppRefs().ToListAsync();
                var continuedOrders = continuedOrderAsync.Where(s => s.Applications.All(a => a.Field.Season.Current)).ToList();

                var continuedPhenologicalOrders = continuedOrders.Where(a => allPhenologicalOrders.Any(s => s.Id.Equals(a.IdPreviousRefApplication))).ToList();
                var continuedDateOrders = continuedOrders.Where(a => allDateOrders.Any(s => s.Id.Equals(a.IdPreviousRefApplication))).ToList();
                var continuedBaseOrders = continuedDateOrders.Union(continuedPhenologicalOrders);
                var continuedOnlyOrders = continuedOrders.Where(s => !continuedBaseOrders.Any(a => s.Id.Equals(a.Id))).ToList();


                var phenologicalEventsFounded = allPhenologicalOrders.GroupBy(s => s.PhenologicalEvent.Id).Select(s => s.First().PhenologicalEvent).ToList();



                var indexPhenologicalEvent = 1;
                var indexApplicationPurpose = 1;

                //phenologicalEvents
                foreach (var phenologicalTask in phenologicalEventsFounded)
                {

                    var phenologicalOrder = new ExtPhenologicalEvent
                    {
                        StartDate = phenologicalTask.InitDate,
                        UniqueId = phenologicalTask.Id,
                        TaskName = phenologicalTask.Name
                    };
                    phenologicalOrder.TaskID = $"{phenologicalOrder.Suffix}{indexPhenologicalEvent}";

                    var phenologicalOrders = allPhenologicalOrders.Where(s => s.PhenologicalEvent.Id.Equals(phenologicalTask.Id));

                    var applicationPurposes = phenologicalOrders.GroupBy(s => s.ApplicationPurpose.Id).Select(a => a.First().ApplicationPurpose).ToList();

                    foreach (var appPurpose in applicationPurposes)
                    {
                        ExtApplicationPurpose extAppPurpose;
                        if (phenologicalOrder.SubTasks.Any(s => s.UniqueId.Equals(appPurpose.Id)))
                        {
                            extAppPurpose = phenologicalOrder.SubTasks.First(s => s.UniqueId.Equals(appPurpose.Id));
                        } else {
                            extAppPurpose = new ExtApplicationPurpose
                            {
                                TaskName = appPurpose.Name,
                                UniqueId = appPurpose.Id
                            };
                            extAppPurpose.TaskID = $"{extAppPurpose.Suffix}{indexApplicationPurpose}";
                            phenologicalOrder.SubTasks.Add(extAppPurpose);
                        }
                       

                        var phenologicalOrderEvents = allPhenologicalOrders
                            .Where(s => s.PhenologicalEvent.Id.Equals(phenologicalOrder.UniqueId) && s.ApplicationPurpose.Id.Equals(extAppPurpose.UniqueId)).ToList();

                        extAppPurpose.SubTasks = phenologicalOrderEvents.Select(s => (ExtReferenceApplication)new ExtPhenologicalEventApplication
                        {
                            ApplicationField = s.Applications,
                            ApplicationPurpose = appPurpose,
                            PhenologicalEvent = phenologicalTask,
                            StartDate = phenologicalTask.InitDate,
                            TaskName = s.ApplicationName,
                            TaskID = s.TaskId,
                            Duration = s.Duration,
                            UniqueId = s.Id

                        }).ToList();

                        

                        
                        indexApplicationPurpose++;
                    }

                    extSeason.PhenologicalEvents.Add(phenologicalOrder);

                    indexPhenologicalEvent++;
                }

                // date orders
                if (allDateOrders.Any())
                {
                    extSeason.DateOrder = new DateOrder
                    {
                        TaskName = "Orden por Fecha",
                        UniqueId = "ODF",
                        
                    };

                    var dateSubTasks = allDateOrders.GroupBy(a => a.ApplicationPurpose.Id);

                    foreach (var purpose in dateSubTasks)
                    {
                        var purposeLocal = purpose.Select(s => s.ApplicationPurpose).First();

                        var dateLocalOrders = purpose.Select(o => (ExtReferenceApplication)new ExtDateApplication
                        {
                            TaskID = o.TaskId,
                            ApplicationField = o.Applications,
                            ApplicationPurpose = o.ApplicationPurpose,
                            Duration = o.Duration,
                            StartDate = o.DateInit,
                            TaskName = o.ApplicationName,
                            UniqueId = o.Id

                        }).ToList();
                        var appPurposeSubTask = new ExtApplicationPurpose
                        {
                            TaskName = purposeLocal.Name,
                            UniqueId = purposeLocal.Id
                        };
                        appPurposeSubTask.TaskID = $"{appPurposeSubTask.Suffix}{indexApplicationPurpose}";
                        appPurposeSubTask.SubTasks = dateLocalOrders;
                        
                        extSeason.DateOrder.SubTasks.Add(appPurposeSubTask);
                        indexApplicationPurpose++;
                    }
                }

                //continuedDateOrders
                if (continuedDateOrders.Any())
                {
                    var dateContinuedSubTasks = continuedDateOrders.GroupBy(a => a.ApplicationPurpose.Id);
                    foreach (var purpose in dateContinuedSubTasks)
                    {
                        

                        var dateLocalOrders = purpose.Select(o => (ExtReferenceApplication)new ExtContinuedApplication
                        {
                            TaskID = o.TaskId,
                            ApplicationField = o.Applications,
                            ApplicationPurpose = o.ApplicationPurpose,
                            Duration = o.Duration,
                            Predecessor = allDateOrders.First(f=>f.Id.Equals(o.IdPreviousRefApplication)).TaskId.ToString(),
                            TaskName = o.ApplicationName,
                            UniqueId = o.Id

                        }).ToList();

                        if (extSeason.DateOrder.SubTasks.Any(s => s.UniqueId.Equals(purpose.Key)))
                        {
                            var appPurpose = extSeason.DateOrder.SubTasks.First(s => s.UniqueId.Equals(purpose.Key));
                            
                            appPurpose.SubTasks.AddRange(dateLocalOrders);
                            break;
                        }
                        var purposeLocal = purpose.Select(s => s.ApplicationPurpose).First();
                        var appPurposeSubTask = new ExtApplicationPurpose
                        {
                            TaskName = purposeLocal.Name,
                            UniqueId = purposeLocal.Id
                        };
                        appPurposeSubTask.TaskID = $"{appPurposeSubTask.Suffix}{indexApplicationPurpose}";
                        appPurposeSubTask.SubTasks = dateLocalOrders;
                        extSeason.DateOrder.SubTasks.Add(appPurposeSubTask);
                        indexApplicationPurpose++;
                    }

                }

                //continued PhenologicalOrders
                if (continuedPhenologicalOrders.Any())
                {
                    var phenologicalContinuedSubTasks = continuedPhenologicalOrders.GroupBy(a => a.ApplicationPurpose.Id);
                    foreach (var purpose in phenologicalContinuedSubTasks)
                    {
                        

                        var dateLocalOrders = purpose.Select(o => new ExtContinuedApplication
                        {
                            TaskID = o.TaskId,
                            ApplicationField = o.Applications,
                            ApplicationPurpose = o.ApplicationPurpose,
                            Duration = o.Duration,
                            Predecessor = allPhenologicalOrders.First(f => f.Id.Equals(o.IdPreviousRefApplication)).TaskId.ToString(),
                            TaskName = o.ApplicationName,
                            UniqueId = o.Id

                        }).ToList();

                        var phenologicalPrecessors = allPhenologicalOrders.Where(s => dateLocalOrders.Any(a => a.Predecessor.Equals(s.TaskId.ToString())));

                        var phenologicalGroups = phenologicalPrecessors.GroupBy(s => s.PhenologicalEvent.Id);

                        foreach (var phKey in phenologicalGroups)
                        {
                            var phenological = extSeason.PhenologicalEvents.First(a => a.UniqueId.Equals(phKey.Key));
                            if (phenological.SubTasks.Any(s=>s.UniqueId.Equals(purpose.Key)))
                            {
                                var appPurpose = phenological.SubTasks.First(s => s.UniqueId.Equals(purpose.Key));
                                appPurpose.SubTasks.AddRange(dateLocalOrders.Select(s => (ExtReferenceApplication)s));
                                break;
                            }
                            var purposeLocal = purpose.Select(s => s.ApplicationPurpose).First();
                            var appPurposePhSubTask = new ExtApplicationPurpose
                            {
                                TaskName = purposeLocal.Name,
                                UniqueId = purposeLocal.Id
                            };
                            appPurposePhSubTask.TaskID = $"{appPurposePhSubTask.Suffix}{indexApplicationPurpose}";
                            appPurposePhSubTask.SubTasks = dateLocalOrders.Select(s => (ExtReferenceApplication)s).ToList();
                            phenological.SubTasks.Add(appPurposePhSubTask);
                            indexApplicationPurpose++;

                        }

                        
                    }
                }

                foreach (var continuedOrder in continuedOnlyOrders)
                {
                    var isPhenological = continuedPhenologicalOrders.Any(s => s.Id.Equals(continuedOrder.IdPreviousRefApplication));
                    if (isPhenological)
                    {
                        var prev = continuedPhenologicalOrders.First(s => s.Id.Equals(continuedOrder.IdPreviousRefApplication));
                        var prevPrev = allPhenologicalOrders.First(f => f.Id.Equals(prev.IdPreviousRefApplication));
                        var phenological = extSeason.PhenologicalEvents.First(s => s.UniqueId.Equals(prevPrev.PhenologicalEvent.Id));
                        ExtApplicationPurpose purpose;
                        if (phenological.SubTasks.Any(k => k.UniqueId.Equals(continuedOrder.ApplicationPurpose.Id)))
                        {
                            purpose = phenological.SubTasks.First(k => k.UniqueId.Equals(continuedOrder.ApplicationPurpose.Id));

                        }
                        else
                        {
                            purpose = new ExtApplicationPurpose
                            {
                                UniqueId = continuedOrder.ApplicationPurpose.Id,
                                TaskName = continuedOrder.ApplicationPurpose.Name
                            };
                            purpose.TaskID = $"{purpose.Suffix}{indexApplicationPurpose}";
                            phenological.SubTasks.Add(purpose);
                            indexApplicationPurpose++;


                        }
                        purpose.SubTasks.Add(new ExtContinuedApplication
                        {
                            ApplicationField = continuedOrder.Applications,
                            ApplicationPurpose = continuedOrder.ApplicationPurpose,
                            Duration = continuedOrder.Duration,
                            Predecessor = prev.TaskId.ToString(),
                            TaskID = continuedOrder.TaskId,
                            TaskName = continuedOrder.ApplicationName,
                            UniqueId = continuedOrder.Id
                        });
                    }
                    else if (continuedDateOrders.Any(s => s.Id.Equals(continuedOrder.IdPreviousRefApplication)))
                    {
                        var prev = continuedDateOrders.First(s => s.Id.Equals(continuedOrder.IdPreviousRefApplication));
                        ExtApplicationPurpose purpose;
                        if (extSeason.DateOrder.SubTasks.Any(s => s.UniqueId.Equals(continuedOrder.ApplicationPurpose.Id)))
                        {
                            purpose = extSeason.DateOrder.SubTasks.First(s => s.UniqueId.Equals(continuedOrder.ApplicationPurpose.Id));

                        }
                        else
                        {
                            purpose = new ExtApplicationPurpose
                            {
                                UniqueId = continuedOrder.ApplicationPurpose.Id,
                                TaskName = continuedOrder.ApplicationPurpose.Name
                            };
                            purpose.TaskID = $"{purpose.Suffix}{indexApplicationPurpose}";
                            extSeason.DateOrder.SubTasks.Add(purpose);
                            indexApplicationPurpose++;

                        }
                        purpose.SubTasks.Add(new ExtContinuedApplication
                        {
                            ApplicationField = continuedOrder.Applications,
                            ApplicationPurpose = continuedOrder.ApplicationPurpose,
                            Duration = continuedOrder.Duration,
                            Predecessor = prev.TaskId.ToString(),
                            TaskID = continuedOrder.TaskId,
                            TaskName = continuedOrder.ApplicationName,
                            UniqueId = continuedOrder.Id
                        });
                    }
                    else {
                        var sph = IsPhenological(allPhenologicalOrders, allDateOrders, continuedOrders, continuedOrder);
                        if (sph.GetType()  == typeof(RefApplicaByPhenologicalEvent))
                        {
                            var prev = (RefApplicaByPhenologicalEvent)sph;
                            
                            var phenological = extSeason.PhenologicalEvents.First(s => s.UniqueId.Equals(prev.PhenologicalEvent.Id));
                            ExtApplicationPurpose purpose;
                            if (phenological.SubTasks.Any(k => k.UniqueId.Equals(continuedOrder.ApplicationPurpose.Id)))
                            {
                                purpose = phenological.SubTasks.First(k => k.UniqueId.Equals(continuedOrder.ApplicationPurpose.Id));

                            }
                            else
                            {
                                purpose = new ExtApplicationPurpose
                                {
                                    UniqueId = continuedOrder.ApplicationPurpose.Id,
                                    TaskName = continuedOrder.ApplicationPurpose.Name
                                };
                                purpose.TaskID = $"{purpose.Suffix}{indexApplicationPurpose}";
                                phenological.SubTasks.Add(purpose);
                                indexApplicationPurpose++;

                            }
                            purpose.SubTasks.Add(new ExtContinuedApplication
                            {
                                ApplicationField = continuedOrder.Applications,
                                ApplicationPurpose = continuedOrder.ApplicationPurpose,
                                Duration = continuedOrder.Duration,
                                Predecessor = prev.TaskId.ToString(),
                                TaskID = continuedOrder.TaskId,
                                TaskName = continuedOrder.ApplicationName,
                                UniqueId = continuedOrder.Id
                            });
                        }
                        else {
                            var prev = continuedOnlyOrders.First(s => s.Id.Equals(continuedOrder.IdPreviousRefApplication));
                            ExtApplicationPurpose purpose;
                            if (extSeason.DateOrder.SubTasks.Any(s => s.UniqueId.Equals(continuedOrder.ApplicationPurpose.Id)))
                            {
                                purpose = extSeason.DateOrder.SubTasks.First(s => s.UniqueId.Equals(continuedOrder.ApplicationPurpose.Id));

                            }
                            else
                            {
                                purpose = new ExtApplicationPurpose
                                {
                                    UniqueId = continuedOrder.ApplicationPurpose.Id,
                                    TaskName = continuedOrder.ApplicationPurpose.Name
                                };
                                purpose.TaskID = $"{purpose.Suffix}{indexApplicationPurpose}";
                                extSeason.DateOrder.SubTasks.Add(purpose);
                                indexApplicationPurpose++;

                            }
                            purpose.SubTasks.Add(new ExtContinuedApplication
                            {
                                ApplicationField = continuedOrder.Applications,
                                ApplicationPurpose = continuedOrder.ApplicationPurpose,
                                Duration = continuedOrder.Duration,
                                Predecessor = prev.TaskId.ToString(),
                                TaskID = continuedOrder.TaskId,
                                TaskName = continuedOrder.ApplicationName,
                                UniqueId = continuedOrder.Id
                            });
                        }

                    }
                }



                extSeason.LastPhenologicalTaskID = indexPhenologicalEvent;
                extSeason.LastPurposeTaskID = indexApplicationPurpose;
                return new ExtGetContainer<ExtSeason>
                {
                    Result = extSeason,
                    StatusResult = ExtGetDataResult.Success
                };
            }
            catch (Exception exception)
            {
                return new ExtGetErrorContainer<ExtSeason>
                {
                    StatusResult = ExtGetDataResult.Error,
                    ErrorMessage = exception.Message,
                    InternalException = exception
                };
            }



        }

        private ReferenceApplicationOrder IsPhenological(List<RefApplicaByPhenologicalEvent> phenological, List<RefApplicationDate> dateOrder, List<RefApplicationContinued> continueds, RefApplicationContinued continued) {
            if (phenological.Any(s => s.Id.Equals(continued.IdPreviousRefApplication)))
            {
                return phenological.First(s => s.Id.Equals(continued.IdPreviousRefApplication));
            }
            if (dateOrder.Any(s => s.Id.Equals(continued.IdPreviousRefApplication)))
            {
                return dateOrder.First(s => s.Id.Equals(continued.IdPreviousRefApplication));
            }

            var continuedContinued = continueds.First(s => s.Id.Equals(continued.IdPreviousRefApplication));
            return IsPhenological(phenological, dateOrder, continueds, continuedContinued);



        }

        private async Task<ExtPostContainer<ReferenceApplicationOrder>> GetBaseOrder(string name, string idApplicationPurpose, int duration, ExternalApplication[] applications) {
            try
            {
                var applicationPurpose = await _baseContainer.ApplicationPurposes.GetApplicationPurpose(idApplicationPurpose);

                if (applicationPurpose == null)
                {
                    return new ExtPostContainer<ReferenceApplicationOrder>
                    {
                        MessageResult = ExtMessageResult.ChildRequiredDoesNotExists,
                        Message = $"No existe propósito de aplicación con id {idApplicationPurpose}"
                    };
                }

                var applicationFields = new List<ApplicationInField>();

                foreach (var application in applications)
                {
                    // agroField
                    var agroField = await _baseContainer.Fields.GetAgroField(application.IdField);
                    if (agroField == null || !agroField.Season.Current)
                    {
                        return new ExtPostContainer<ReferenceApplicationOrder>
                        {
                            MessageResult = ExtMessageResult.ChildRequiredDoesNotExists,
                            Message = $"No existe campo con id {application.IdField} o el campo no pertenece a la temporara actual"
                        };
                    }

                    if (!application.IdIngredients.Any() && !application.IdCategories.Any())
                    {
                        return new ExtPostContainer<ReferenceApplicationOrder>
                        {
                            MessageResult = ExtMessageResult.ChildRequiredDoesNotExists,
                            Message = $"No existen categorias ni ingredientes"
                        };
                    }


                    if (application.IdIngredients == null)
                    {
                        application.IdIngredients = new string[] { };
                    }
                    if (application.IdCategories == null)
                    {
                        application.IdCategories = new string[] { };
                    }

                    var applicationField = new ApplicationInField
                    {
                        Field = agroField,
                        WettingByHec = application.WettingByHec
                    };

                    if (application.IdIngredients.Any())
                    {
                        var allIngredients = await _baseContainer.ActiveIngredients.GetActiveIngredients().ToListAsync();

                        // ingredients
                        if (!application.IdIngredients.All(i => allIngredients.Any(s => s.Id.Equals(i))))
                        {
                            return new ExtPostContainer<ReferenceApplicationOrder>
                            {
                                MessageResult = ExtMessageResult.ChildRequiredDoesNotExists,
                                Message = $"Existen ingredientes que no existen en la base de datos"
                            };
                        }
                        var appIngredients = application.IdIngredients.Select(s => allIngredients.First(a => a.Id.Equals(s))).ToList();

                        applicationField.IngredientInstances = appIngredients.Select(i => new ActiveIngredientInstance
                        {
                            ActiveIngredient = i
                        }).ToList();
                    }

                    // categories
                    if (application.IdCategories.Any())
                    {
                        var allCategories = await _baseContainer.ActiveIngredientCategories.GetCategories().ToListAsync();

                        if (application.IdCategories.Any() && !application.IdCategories.All(c => allCategories.Any(s => s.Id.Equals(c))))
                        {
                            return new ExtPostContainer<ReferenceApplicationOrder>
                            {
                                MessageResult = ExtMessageResult.ChildRequiredDoesNotExists,
                                Message = $"Existen categorias que no existen en la base de datos"
                            };
                        }

                        var appCategories = application.IdCategories.Select(c => allCategories.First(s => s.Id.Equals(c))).ToList();

                        applicationField.CategoryInstance = appCategories.Select(c => new ActiveIngredientCategoryInstance
                        {
                            Category = c
                        }).ToList();
                    }

                    applicationFields.Add(applicationField);
                }

                var taskId = await _baseContainer.Counter.GetNextTaskId();

                var order = new ReferenceApplicationOrder
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ApplicationName = name,
                    ApplicationPurpose = applicationPurpose,
                    CreationDate = DateTime.Now,
                    Duration = duration,
                    Applications = applicationFields,
                    TaskId = taskId
                };

                return new ExtPostContainer<ReferenceApplicationOrder>
                {
                    IdRelated = order.Id,
                    Result = order,
                    Message = string.Empty,
                    MessageResult = ExtMessageResult.Ok
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ExtPostContainer<TaskIdentifier>> SavePhenologicalOrder(string name, string idPhenologicalEvent, string idApplicationPurpose, int duration, ExternalApplication[] applications)
        {

            try
            {

                var orderBase = await GetBaseOrder(name, idApplicationPurpose, duration, applications);
                if (orderBase.MessageResult != ExtMessageResult.Ok)
                {
                    return new ExtPostContainer<TaskIdentifier>
                    {
                        MessageResult = orderBase.MessageResult,
                        Message = orderBase.Message
                    };
                }

                var phenologicalEvent = await _baseContainer.PhenologicalEvents.GetPhenologicalEvent(idPhenologicalEvent);

                if (phenologicalEvent == null)
                {
                    return new ExtPostContainer<TaskIdentifier>
                    {
                        MessageResult = ExtMessageResult.ChildRequiredDoesNotExists,
                        Message = $"No existe evento fenológico con id {idPhenologicalEvent}"
                    };
                }



                var order = new RefApplicaByPhenologicalEvent
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ApplicationName = name,
                    ApplicationPurpose = orderBase.Result.ApplicationPurpose,
                    CreationDate = DateTime.Now,
                    Duration = duration,
                    PhenologicalEvent = phenologicalEvent,
                    Applications = orderBase.Result.Applications,
                    TaskId = orderBase.Result.TaskId
                };

                var resultDb = await _baseContainer.PhenologicalRefApps.CreateUpdatePhenologicalAppRef(order);
                return new ExtPostContainer<TaskIdentifier>
                {
                    IdRelated = resultDb,
                    Result = new TaskIdentifier
                    {
                        TaskId = order.TaskId,
                        UniqueId = order.Id
                    },
                    Message = string.Empty,
                    MessageResult = ExtMessageResult.Ok
                };
            }
            catch (Exception e)
            {

                return new ExtPostErrorContainer<TaskIdentifier>
                {
                    IdRelated = string.Empty,
                    Message = e.Message,
                    MessageResult = ExtMessageResult.Error,
                    InternalException = new DbException<RefApplicaByPhenologicalEvent>(new RefApplicaByPhenologicalEvent {
                        ApplicationName = name
                    }, e)
                };
            }
        }

        public async Task<ExtPostContainer<TaskIdentifier>> SaveDateOrder(string name, DateTime initDate, string idApplicationPurpose, int duration, ExternalApplication[] applications)
        {
            try
            {

                var orderBase = await GetBaseOrder(name, idApplicationPurpose, duration, applications);
                if (orderBase.MessageResult != ExtMessageResult.Ok)
                {
                    return new ExtPostContainer<TaskIdentifier>
                    {
                        MessageResult = orderBase.MessageResult,
                        Message = orderBase.Message
                    };
                }


                var order = new RefApplicationDate
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ApplicationName = name,
                    ApplicationPurpose = orderBase.Result.ApplicationPurpose,
                    CreationDate = DateTime.Now,
                    Duration = duration,
                    DateInit = initDate,
                    Applications = orderBase.Result.Applications,
                    TaskId = orderBase.Result.TaskId
                };

                var resultDb = await _baseContainer.DateRefApps.CreateUpdateDateAppRef(order);
                return new ExtPostContainer<TaskIdentifier>
                {
                    IdRelated = resultDb,
                    Result = new TaskIdentifier
                    {
                        TaskId = order.TaskId,
                        UniqueId = order.Id
                    },
                    Message = string.Empty,
                    MessageResult = ExtMessageResult.Ok
                };
            }
            catch (Exception e)
            {

                return new ExtPostErrorContainer<TaskIdentifier>
                {
                    IdRelated = string.Empty,
                    Message = e.Message,
                    MessageResult = ExtMessageResult.Error,
                    InternalException = new DbException<RefApplicationDate>(new RefApplicationDate
                    {
                        ApplicationName = name
                    }, e)
                };
            }
        }

        public async Task<ExtPostContainer<TaskIdentifier>> SaveContinuedOrder(string name, string precessor, string idApplicationPurpose, int duration, ExternalApplication[] applications)
        {
            try
            {

                var orderBase = await GetBaseOrder(name, idApplicationPurpose, duration, applications);
                if (orderBase.MessageResult != ExtMessageResult.Ok)
                {
                    return new ExtPostContainer<TaskIdentifier>
                    {
                        MessageResult = orderBase.MessageResult,
                        Message = orderBase.Message
                    };
                }


                var order = new RefApplicationContinued
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ApplicationName = name,
                    ApplicationPurpose = orderBase.Result.ApplicationPurpose,
                    CreationDate = DateTime.Now,
                    Duration = duration,
                    IdPreviousRefApplication = precessor,
                    Applications = orderBase.Result.Applications,
                    TaskId = orderBase.Result.TaskId
                };

                var resultDb = await _baseContainer.ContinuedRefApps.CreateUpdateContinuedAppRef(order);
                return new ExtPostContainer<TaskIdentifier>
                {
                    IdRelated = resultDb,
                    Result = new TaskIdentifier
                    {
                        TaskId = order.TaskId,
                        UniqueId = order.Id
                    },
                    Message = string.Empty,
                    MessageResult = ExtMessageResult.Ok
                };
            }
            catch (Exception e)
            {

                return new ExtPostErrorContainer<TaskIdentifier>
                {
                    IdRelated = string.Empty,
                    Message = e.Message,
                    MessageResult = ExtMessageResult.Error,
                    InternalException = new DbException<RefApplicationContinued>(new RefApplicationContinued
                    {
                        ApplicationName = name
                    }, e)
                };
            }
        }
    }
}
