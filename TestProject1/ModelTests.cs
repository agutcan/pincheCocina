using Microsoft.VisualStudio.TestTools.UnitTesting;
using pincheCocina.MVVM.Models;


namespace UnitTest
{
    [TestClass]
    public class ModelTests
    {
        [TestMethod]
        public void PasoReceta_TieneTiempo_DebeSerVerdaderoSiEsMayorACero()
        {
            // Arrange
            var paso = new PasoReceta { TiempoMinutos = 10 };
            // Act & Assert
            Assert.IsTrue(paso.TieneTiempo);
        }

        [TestMethod]
        public void PasoReceta_AddIngrediente_DebeIncrementarLista()
        {
            // Arrange
            var paso = new PasoReceta();
            var ing = new Ingrediente("Sal");
            // Act
            paso.AddIngrediente(ing);
            // Assert
            Assert.AreEqual(1, paso.Ingredientes.Count);
            Assert.AreEqual("Sal", paso.Ingredientes[0].Nombre);
        }

        [TestMethod]
        public void Receta_Constructor_DebeInicializarNombre()
        {
            // Arrange & Act
            var receta = new Receta("Pasta Carbonara");
            // Assert
            Assert.AreEqual("Pasta Carbonara", receta.Nombre);
        }

    }
}