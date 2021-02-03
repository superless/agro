﻿using System;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.external.operations.tests.data;
using trifenix.connect.agro.external.helper;
using trifenix.connect.agro.tests.mock;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using Xunit;

namespace trifenix.connect.agro.tests
{
    public class ValidatorTest
    {
        public ValidatorTest()
        {

        }

        #region Validate Products
        /// <summary>
        /// Se ingresa un producto nuevo sin marca, debería analizar su atributo y alertar de que es obligatorio.        
        /// </summary>
        [Fact]
        public async Task ValidatingAutomatedProductWithoutBrand()
        {
            //assign
            var mainValidator = new MainValidator<Product, ProductInput>(MockHelper.GetExistElement());
            //action
            var result = await mainValidator.Valida(AgroInputData.ProductNewWithoutBrand);
            //assert
            Assert.True(result.Messages.Count() == 1 && result.Messages.First().Equals("ProductInput.IdBrand es obligatorio"));
        }



        /// <summary>
        /// Al ingresar un producto con un id, significa que debe existir en la base de datos.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ValidationAutomatedProductToEditWithIdInvalid()
        {
            //assign
            var mainValidator = new MainValidator<Product, ProductInput>(MockHelper.GetExistElement());

            //action
            var result = await mainValidator.Valida(AgroInputData.ProductWithInvalidIdToEdit);


            //assert
            Assert.True(result.Messages.Count() == 1 && result.Messages.First().Contains("no existe en la base de datos"));


        }


        /// <summary>
        /// Validacion Correcta de un producto con dosis y con todos los campos correctos
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ValidationFullProductInputValid()
        {
            //assign
            var mainValidator = new MainValidator<Product, ProductInput>(MockHelper.GetExistElement());

            //action
            var result = await mainValidator.Valida(AgroInputData.Product1);


            //assert
            Assert.True(result.Valid);


        }

        /// <summary>
        /// Valida que si un campo WettingRecommendedByHectares es cero, alertará con un mensaje.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ValidationProductWithADosesWithWettingZero()
        {
            //assign
            var mainValidator = new MainValidator<Product, ProductInput>(MockHelper.GetExistElement());

            //action
            var result = await mainValidator.Valida(AgroInputData.ProductWithDosesWithoutWett);



            //assert
            Assert.True(result.Messages.Count() == 1 && result.Messages.First().Contains("DosesInput.WettingRecommendedByHectares es obligatorio"));


        }


        /// <summary>
        /// Se asigna un id de entidad certificadora que no existe,
        /// deberá envíar error, por la certificadora no encontrada
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ValidationProductWithInvalidCertifiedEntityInsideWettingHarvest()
        {

            var mainValidator = new MainValidator<Product, ProductInput>(MockHelper.GetExistElement());

            var prd = AgroInputData.Product1;



            var nguid = Guid.NewGuid().ToString("N");

            prd.Doses[0].WaitingToHarvest[0].IdCertifiedEntity = nguid;


            //action
            var result = await mainValidator.Valida(prd);

            //No existe CertifiedEntity con id


            //assert
            Assert.True(result.Messages.Count() == 1 && result.Messages.First().Contains("No existe CertifiedEntity con id"));
        }

        /// <summary>
        /// Verifica que el código Sag de un producto sea único. 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ValidaSagCodeisUnique()
        {

            var mainValidator = new MainValidator<Product, ProductInput>(MockHelper.GetExistElement());

            var prd = AgroInputData.Product1;

            prd.SagCode = "43322"; // usa código SAG de product2

            //action
            var result = await mainValidator.Valida(prd);

            //No existe CertifiedEntity con id


            //assert
            Assert.True(result.Messages.Count() == 1 && result.Messages.First().Contains("La propiedad del input ProductInput.SagCode con valor 43322 existe previamente en la base de datos"));
        }

        /// <summary>
        /// Verifica que el código Sag de un producto sea único, para un nuevo producto.
        /// quitamos el id de producto para asignarlo como nuevo
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ValidaSagCodeisUniqueInNewProduct()
        {

            var mainValidator = new MainValidator<Product, ProductInput>(MockHelper.GetExistElement());

            var prd = AgroInputData.Product1;
            prd.Id = string.Empty;

            prd.SagCode = "43322"; // usa código SAG de product2
            prd.Name = "New Product Random";
            //action
            var result = await mainValidator.Valida(prd);

            //No existe CertifiedEntity con id


            //assert
            Assert.True(result.Messages.Count() == 1 && result.Messages.First().Contains("La propiedad del input ProductInput.SagCode con valor 43322 existe previamente en la base de datos"));
        }


        /// <summary>
        /// Ingresa un nuevo producto, con un nombre ya existente en la base de datos.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ValidaNombreRepetidoInvalidoSiElNombreYaExiste()
        {

            var mainValidator = new MainValidator<Product, ProductInput>(MockHelper.GetExistElement());

            var prd = AgroInputData.Product1;
            prd.Id = string.Empty;

            prd.SagCode = "433221"; // usa código SAG de product2
            prd.Name = "Producto 1"; // ya existe en la base mock.
            //action
            var result = await mainValidator.Valida(prd);

            //No existe CertifiedEntity con id


            //assert
            Assert.True(result.Messages.Count() == 1 && result.Messages.First().Contains("La propiedad del input ProductInput.Name con valor Producto 1 existe previamente en la base de datos"));
        }
        #endregion

        #region Validate Barracks


        /// <summary>
        /// Valida un barrak que tiene los datos correctamente
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ValidaOkBarrack() {
            // assign
            var mainValidator = new MainValidator<Barrack, BarrackInput>(MockHelper.GetExistElement());
            // action
            var result = await mainValidator.Valida(AgroInputData.Barrack1);

            Assert.True(result.Valid);
            // assert
        }

        #endregion

        #region Validete Specie
        [Fact]
        public async Task ValidadaEspecie()
        {
            //assign
            var mainValidator = new MainValidator<Specie, SpecieInput>(MockHelper.GetExistElement());
            //action
            var result = await mainValidator.Valida(AgroInputData.Specie1);
            //assert
            Assert.True(result.Valid);
        }
        #endregion

        #region Validete Specie
        [Fact]
        public async Task ValidadaVariety()
        {
            //assign
            var mainValidator = new MainValidator<Variety, VarietyInput>(MockHelper.GetExistElement());
            //action
            var result = await mainValidator.Valida(AgroInputData.Variety1);
            //assert
            Assert.True(result.Valid);
        }
        #endregion

        #region Validete Plotland
        [Fact]
        public async Task ValidadaPlotland()
        {
            //assign
            var mainValidator = new MainValidator<PlotLand, PlotLandInput>(MockHelper.GetExistElement());
            //action
            var result = await mainValidator.Valida(AgroInputData.Plotland1);
            //assert
            Assert.True(result.Valid);
        }
        #endregion

        #region Validete Rootstock
        [Fact]
        public async Task ValidadaRootstock()
        {
            //assign
            var mainValidator = new MainValidator<Rootstock, RootstockInput>(MockHelper.GetExistElement());
            //action
            var result = await mainValidator.Valida(AgroInputData.Rootstock1);
            //assert
            Assert.True(result.Valid);
        }

        #endregion
        #region Validete Sector
        [Fact]
        public async Task ValidadaSector()
        {
            //assign
            var mainValidator = new MainValidator<Sector, SectorInput>(MockHelper.GetExistElement());
            //action
            var result = await mainValidator.Valida(AgroInputData.Sector1);
            //assert
            Assert.True(result.Valid);
        }
        #endregion

        #region Validete Season
        [Fact]
        public async Task ValidadaSeason()
        {
            //assign
            var mainValidator = new MainValidator<Season, SeasonInput>(MockHelper.GetExistElement());
            //action
            var result = await mainValidator.Valida(AgroInputData.Season1);
            //assert
            Assert.True(result.Valid);
        }
        #endregion

        #region Validete Season
        [Fact]
        public async Task ValidadaSeasons()
        {
            //assign
            var mainValidator = new MainValidator<Season, SeasonInput>(MockHelper.GetExistElement());
            //action
            var result = await mainValidator.Valida(AgroInputData.Season1);
            //assert
            Assert.True(result.Valid);
        }
        #endregion

    }
}
