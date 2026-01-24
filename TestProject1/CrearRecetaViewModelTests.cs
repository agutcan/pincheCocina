using Moq;
using pincheCocina.MVVM.Models;
using pincheCocina.MVVM.ViewModels;
using pincheCocina.Services;

namespace TestProject1;

[TestClass]
public class CrearRecetaViewModelTests
{
    private Mock<IRecetaService> _serviceMock;
    private CrearRecetaViewModel _vm;

    [TestInitialize]
    public void Setup()
    {
        _serviceMock = new Mock<IRecetaService>();
        _vm = new CrearRecetaViewModel(_serviceMock.Object);
    }

    [TestMethod]
    public void ProcesarTextoDictado_DebeExtraerAccionYTiempo()
    {
        // Arrange
        string dictado = "Cocinar por 15 minutos";
        // Act
        _vm.ProcesarTextoDictado(dictado);
        // Assert
        var paso = _vm.PasosVisuales[0];
        Assert.AreEqual("Cocinar por 15 minutos", paso.Accion);
        Assert.AreEqual(15, paso.TiempoMinutos);
    }

    [TestMethod]
    public void ProcesarTextoDictado_DebeConvertirHorasAMinutos()
    {
        // Arrange
        string dictado = "Hornear por 1 hora";
        // Act
        _vm.ProcesarTextoDictado(dictado);
        // Assert
        Assert.AreEqual(60, _vm.PasosVisuales[0].TiempoMinutos);
    }

    [TestMethod]
    public void GuardarReceta_DebeLlamarAlServicio_CuandoNombreEsValido()
    {
        // Arrange
        _vm.NombreReceta = "Nueva Receta";
        _vm.RecetaAEditar = new Receta();
        // Act
        var resultado = _vm.GuardarReceta().Result;
        // Assert
        _serviceMock.Verify(s => s.AddRecetaAsync(It.IsAny<Receta>()), Times.Once);
        Assert.IsTrue(resultado);
    }
}
