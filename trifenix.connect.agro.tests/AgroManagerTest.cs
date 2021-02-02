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
    public class AgroManagerTest
    {


        public class SaveInputProductMethod
        {
            /// <summary>
            /// Haremos todo el proceso de ingreso de un producto, incluyendo todos las entidades relacionadas.
            /// </summary>
            /// <returns></returns>
            [Fact]
            public async Task InsertProductSuccessProduct()
            {
                //assign
                var agroManager = MockHelper.AgroManager;


                // entidades dependientes de producto

                // categoria de ingrediente

                var categoryIngredientResult = await agroManager.IngredientCategory.SaveInput(new IngredientCategoryInput
                {
                    Name = "Fertilizantes"
                });

                var categoryIngredient = await agroManager.IngredientCategory.Get(categoryIngredientResult.IdRelated);


                Assert.True(categoryIngredient.Result.Name.Equals("Fertilizantes"));



                // marca.
                var brandInput = await agroManager.Brand.SaveInput(new BrandInput
                {
                    Name = "Anasac"
                });


                var brand = await agroManager.Brand.Get(brandInput.IdRelated);


                Assert.True(brand.Result.Name.Equals("Anasac"));

                var idIngredientCategory = categoryIngredient.Result.Id;


                var ingredientInput = await agroManager.Ingredient.SaveInput(new IngredientInput
                {
                    idCategory = idIngredientCategory,
                    Name = "OXIDO CUPROSO"
                });

                var ingredient = await agroManager.Ingredient.Get(ingredientInput.IdRelated);


                Assert.True(ingredient.Result.Name.Equals("OXIDO CUPROSO"));



                // entidades dependientes de dosis input

                // especie
                var specieInput = await agroManager.Specie.SaveInput(new SpecieInput
                {
                    Abbreviation = "CRZ",
                    Name = "Cereza"
                });

                var specie = await agroManager.Specie.Get(specieInput.IdRelated);

                Assert.True(specie.Result.Name.Equals("Cereza"));


                // variedad
                var varietyInput = await agroManager.Variety.SaveInput(new VarietyInput
                {
                    Abbreviation = "STN",
                    Name = "Santina",
                    IdSpecie = specie.Result.Id
                });

                var variety = await agroManager.Variety.Get(varietyInput.IdRelated);

                Assert.True(variety.Result.Name.Equals("Santina"));

                // objetivo de la aplicación.
                var targetInput = await agroManager.ApplicationTarget.SaveInput(new ApplicationTargetInput
                {
                    Abbreviation = "CB",
                    Name = "Cancer Bacterial"

                });


                var target = await agroManager.ApplicationTarget.Get(targetInput.IdRelated);


                Assert.True(target.Result.Name.Equals("Cancer Bacterial"));



                // Entidades certificadoras o mercados 
                var marketInput = await agroManager.CertifiedEntity.SaveInput(new CertifiedEntityInput
                {
                    Abbreviation = "CHN",
                    Name = "China"


                });

                var market = await agroManager.CertifiedEntity.Get(marketInput.IdRelated);

                Assert.True(market.Result.Name.Equals("China"));


                var marketInput2 = await agroManager.CertifiedEntity.SaveInput(new CertifiedEntityInput
                {
                    Abbreviation = "USA",
                    Name = "Estados Unidos"


                });

                var market2 = await agroManager.CertifiedEntity.Get(marketInput2.IdRelated);

                Assert.True(market2.Result.Name.Equals("Estados Unidos"));




                // tiempo de espera para la cosecha según mercado.
                var waitingHarvestInput1 = new WaitingHarvestInput
                {
                    IdCertifiedEntity = market.Result.Id,
                    Ppm = 10,
                    WaitingDays = 20
                };

                var waitingHarvestInput2 = new WaitingHarvestInput
                {
                    IdCertifiedEntity = market2.Result.Id,
                    Ppm = 10,
                    WaitingDays = 20
                };


                // dosis asignada al producto
                // cuando se inserta una dosis directamente en  su repositorio, el id de producto es obligatorio.
                // cuando es insertado a través de la inserción de productos, es este el que se encarga de asignare su propia id en la dosis de producto.
                // lo mismo pasa con active y default


                var dosesInput = new DosesInput
                {
                    DosesQuantityMax = 5,
                    DosesQuantityMin = 3,
                    HoursToReEntryToBarrack = 22,
                    IdsApplicationTarget = new string[] { target.Result.Id },
                    IdVarieties = new string[] { variety.Result.Id },
                    ApplicationDaysInterval = 10,
                    NumberOfSequentialApplication = 3,
                    WaitingDaysLabel = 5,
                    WettingRecommendedByHectares = 2000,
                    WaitingToHarvest = new WaitingHarvestInput[] {
                        waitingHarvestInput1, waitingHarvestInput2
                     }
                };


                var productInput = new ProductInput
                {
                    Doses = new DosesInput[] { dosesInput },
                    IdActiveIngredient = ingredient.Result.Id,
                    IdBrand = brand.Result.Id,
                    Name = "NORDOX SUPER 75 WG",
                    MeasureType = MeasureType.KL,
                    SagCode = "1223H5"
                };



                var productInputTest = await agroManager.Product.SaveInput(productInput);


                var product = await agroManager.Product.Get(productInputTest.IdRelated);

                Assert.Equal(productInput.Name, product.Result.Name);

                var compareModel = Mdm.Validation.CompareModel(
                    productInput,
                    product.Result,
                    new Dictionary<Type, Func<object, IEnumerable<object>>> {
                        {   typeof(DosesInput[]),
                            (i)=> {
                                var doses = AgroData.Doses;
                                var dosesInput = (DosesInput[])i;

                                var idProductDetected = dosesInput.Select(s=>s.IdProduct);


                                var dosesForInput = doses.Where(v=>idProductDetected.Any(a=>v.IdProduct.Equals(a))).ToList();

                                return dosesForInput;
                            }
                        },
                        {   typeof(WaitingHarvestInput[]),
                            (i)=> {
                                var doses = AgroData.Doses;

                                var wharvest = (WaitingHarvestInput[])i;

                                var dosesProduct =  doses.Where(s=>s.IdProduct.Equals(product.Result.Id));

                                var idCertifiedEntities = wharvest.Select(s=>s.IdCertifiedEntity);



                                var whvest = dosesProduct.Where(dp=>dp.WaitingToHarvest.Select(s=>s.IdCertifiedEntity).Distinct().Any(ce=>wharvest.Any(wh=>wh.IdCertifiedEntity.Equals(ce)))).SelectMany(s=>s.WaitingToHarvest).ToList();

                                return wharvest;

                            }
                        }

                    }

                );

                Assert.True(compareModel);

                //sectores y parcelas

                var sectorInput = await agroManager.Sector.SaveInput(new SectorInput
                {
                    Name = "Cordillera"
                });

                var sector = await agroManager.Sector.Get(sectorInput.IdRelated);

                Assert.True(sector.Result.Name.Equals("Cordillera"));

                
                var plotLandInput = await agroManager.PlotLand.SaveInput(new PlotLandInput
                {
                    IdSector = sector.Result.Id,
                    Name = "Delirio"
                });

                var plotLand = await agroManager.PlotLand.Get(plotLandInput.IdRelated);

                Assert.True(plotLand.Result.Name.Equals("Delirio"));


                //Razon social

                var businessNameInput = await agroManager.BusinessName.SaveInput(new BusinessNameInput
                {
                    Name = "TrifenixA",
                    Email = "trifenix@trifenix.io",
                    Rut = "76955261-2",
                    WebPage = "connect.trifenix.io",
                    Giro = "agro-fenix",
                    Phone = "99999999"
                });

                var businnesName = await agroManager.BusinessName.Get(businessNameInput.IdRelated);

                Assert.True(businnesName.Result.Name.Equals("TrifenixA"));

                //Centro de costo

                var costCenterInput = await agroManager.CostCenter.SaveInput(new CostCenterInput
                {
                    Name = "Centro de costo",
                    IdBusinessName = businnesName.Result.Id
                });

                var costCenter = await agroManager.CostCenter.Get(costCenterInput.IdRelated);

                Assert.True(costCenter.Result.Name.Equals("Centro de costo"));

                //Trabajo

                var jobInput = await agroManager.Job.SaveInput(new JobInput 
                {
                    Name = "Agronomo"
                });

                var job = await agroManager.Job.Get(jobInput.IdRelated);

                Assert.True(job.Result.Name.Equals("Agronomo"));

                //Nebulizadora
                var nebulizerInput = await agroManager.Nebulizer.SaveInput(new NebulizerInput
                {
                    Brand = "AM162",
                    Code = "927002"
                });

                var nebulizer = await agroManager.Nebulizer.Get(nebulizerInput.IdRelated);

                Assert.True(nebulizer.Result.Brand.Equals("AM162"));

                //Eventos Fenologicos
                var phenologicalEventInput = await agroManager.PhenologicalEvent.SaveInput(new PhenologicalEventInput
                {
                    Name = "Lluvia",
                    StartDate = new DateTime(2021, 3, 1),
                    EndDate = new DateTime(2022, 3, 1)
                });

                var phenologicalEvent = await agroManager.PhenologicalEvent.Get(phenologicalEventInput.IdRelated);

                Assert.True(phenologicalEvent.Result.Name.Equals("Lluvia"));

                //Rol
                var roleInput = await agroManager.Role.SaveInput(new RoleInput
                {
                    Name = "Recogedor",
                });

                var role = await agroManager.Role.Get(roleInput.IdRelated);

                Assert.True(role.Result.Name.Equals("Recogedor"));

                //Raiz
                var rootstockInput = await agroManager.Rootstock.SaveInput(new RootstockInput
                {
                    Name = "Royal",
                    Abbreviation = "Ryl"
                });

                var rootstock = await agroManager.Rootstock.Get(rootstockInput.IdRelated);

                Assert.True(rootstock.Result.Name.Equals("Royal"));

                //temporada
                var seasonInput = await agroManager.Season.SaveInput(new SeasonInput
                {
                    Current = false,
                    StartDate = new DateTime(2020, 3, 1),
                    EndDate = new DateTime(2022, 3, 1),
                    IdCostCenter = costCenter.Result.Id
                });

                var season = await agroManager.Season.Get(seasonInput.IdRelated);

                Assert.True(season.Result.Current.Equals(false));

                /*  //Pre orden
                  var preOrdenInput = await agroManager.PreOrder.SaveInput(new PreOrderInput
                  {
                      Name = "Pre orden 1",
                      IdIngredient = ingredient.Result.Id,
                      OrderFolderId = ,
                      PreOrderType = PreOrderType.DEFAULT,
                      BarracksId = 
                  });

                  var preOrden = await agroManager.PreOrden.Get(preOrdenInput.IdRelated);

                  Assert.True(preOrden.Result.Name.Equals("Pre orden 1"));*/

                //Tractor
                var tractorInput = await agroManager.Tractor.SaveInput(new TractorInput
                {
                    Brand = "Kubota",
                    Code = "B2320"
                });

                var tractor = await agroManager.Tractor.Get(tractorInput.IdRelated);

                Assert.True(tractor.Result.Brand.Equals("Kubota"));
                /*
                //Older Folder
                var orderFolderInput = await agroManager.OrderFolder.SaveInput(new OrderFolderInput
                {
                    IdPhenologicalEvent = phenologicalEvent.Result.Id,
                    IdApplicationTarget = target.Result.Id,
                    IdSpecie = specie.Result.Id,
                    IdIngredient = ingredient.Result.Id,
                    IdIngredientCategory = categoryIngredient.Result.Id
                
                });

                var orderFolder = await agroManager.OrderFolder.Get(orderFolderInput.IdRelated);

                Assert.True(orderFolder.Result.Id.Equals();*/
                
                //barack
                var barrackInput = await agroManager.Barrack.SaveInput(new BarrackInput
                {
                    Hectares = 1.1,
                    IdPlotLand = plotLand.Result.Id,
                    IdPollinator = variety.Result.Id,
                    NumberOfPlants = 444,
                    IdVariety = variety.Result.Id,
                    Name = "Barrack 3",
                    SeasonId = season.Result.Id,
                    IdRootstock = rootstock.Result.Id,
                    PlantingYear = 1980,

                });

                var barrack = await agroManager.Barrack.Get(barrackInput.IdRelated);

                Assert.True(barrack.Result.Name.Equals("Barrack 3"));
            }

        }


    }
}
