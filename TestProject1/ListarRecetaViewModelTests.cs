using Moq;
using pincheCocina.MVVM.Models;
using pincheCocina.MVVM.ViewModels;
using pincheCocina.Services;

namespace TestProject1;

[TestClass]
public class ListarRecetaViewModelTests
{
    private Mock<IRecetaService> _serviceMock;
    private ListarRecetaViewModel _vm;

    [TestInitialize]
    public void Setup()
    {
        _serviceMock = new Mock<IRecetaService>();
        _vm = new ListarRecetaViewModel(_serviceMock.Object);
    }

    [TestMethod]
    public void ObtenerTextoLecturaPaso_DebeTraducirUnidades()
    {
        // Arrange
        var paso = new PasoReceta { Accion = "Mezclar", TiempoMinutos = 5 };
        paso.Ingredientes.Add(new Ingrediente { Nombre = "Leche", Cantidad = 1, Unidad = "l" });

        // Act
        string resultado = _vm.ObtenerTextoLecturaPaso(paso);

        // Assert
        Assert.IsTrue(resultado.Contains("1 litros de Leche"), "No tradujo 'l' a 'litros'");
        Assert.IsTrue(resultado.Contains("Mezclar"), "No incluyó la acción");
    }

    [TestMethod]
    public async Task CargarRecetas_DebeLlenarLaColeccion()
    {
        // Arrange
        var listaFalsa = new List<Receta> { new Receta("Test 1"), new Receta("Test 2") };
        _serviceMock.Setup(s => s.GetRecetasAsync()).ReturnsAsync(listaFalsa);

        // Act
        await _vm.CargarRecetasAsync();

        // Assert
        Assert.AreEqual(2, _vm.Recetas.Count);
    }
}
