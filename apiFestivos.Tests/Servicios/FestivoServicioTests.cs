using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using apiFestivos.Aplicacion.Servicios;
using apiFestivos.Dominio.Entidades;
using apiFestivos.Core.Interfaces.Repositorios;
using apiFestivos.Dominio.DTOs;

namespace apiFestivos.Tests.Servicios
{
    [TestClass]
    public class FestivoServicioTests
    {
        private FestivoServicio _servicio;
        private Mock<IFestivoRepositorio> _repositorioMock;

        [TestInitialize]
        public void Setup()
        {
            _repositorioMock = new Mock<IFestivoRepositorio>();
            _servicio = new FestivoServicio(_repositorioMock.Object);
        }

        [TestMethod]
        public async Task EsFestivo_FechaFestiva_ReturnsTrue()
        {
            var fechafestiva = new DateTime(2024, 1, 1); //Ejemplo de fecha festiva


            _repositorioMock.Setup(r => r.ObtenerTodos())
                .ReturnsAsync(new List<Festivo> { 
                    new Festivo { IdTipo = 1, Mes = 1, Dia = 1, Nombre = "Año Nuevo" } //Festivo esperado
                });

            var resultado = await _servicio.EsFestivo(fechafestiva);

            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public async Task EsFestivo_FechaNoFestiva_ReturnsFalse()
        {
            var fechaNofestiva = new DateTime(2024, 2, 15); //Ejemplo de fecha no festiva


            _repositorioMock.Setup(r => r.ObtenerTodos())
                .ReturnsAsync(new List<Festivo> 
                { 
                    new Festivo {IdTipo = 1, Mes = 1, Dia = 1, Nombre = "Año Nuevo" } 
                });

            var resultado = await _servicio.EsFestivo(fechaNofestiva);

            Assert.IsFalse(resultado);
        }

        [TestMethod]
        public void ObtenerFestivos_FestivosTipo1_ReturnsEsperada()
        {
            var festivo = new Festivo { IdTipo = 1, Mes = 1, Dia = 1, Nombre = "Año Nuevo" };
            int año = 2024;

            var resultado = _servicio.ObtenerFestivo(año, festivo);

            Assert.AreEqual(new DateTime(año, festivo.Mes, festivo.Dia), resultado.Fecha);
        }

        [TestMethod]
        public void ObtenerFestivo_FestivoTipo2_ReturnsLunesSiguiente()
        {
            
            var festivo = new Festivo { IdTipo = 2, Mes = 8, Dia = 7, Nombre = "Batalla de Boyacá" };
            int año = 2024;

            
            var resultado = _servicio.ObtenerFestivo(año, festivo);

           
            Assert.AreEqual(DayOfWeek.Monday, resultado.Fecha.DayOfWeek);
        }

        [TestMethod]
        public void ObtenerFestivo_FestivoTipo4_ReturnsFechaEsperada()
        {
            
            var festivo = new Festivo { IdTipo = 4, DiasPascua = 40, Nombre = "Ascensión del Señor" };
            int año = 2024;

            
            var resultado = _servicio.ObtenerFestivo(año, festivo);

            
            Assert.AreEqual(DayOfWeek.Monday, resultado.Fecha.DayOfWeek);
        }

    }
}
