using System.Collections.Generic;
using System.Linq;
using CalculatorApp.Controllers;
using CalculatorApp.Models;
using CalculatorApp.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CalculatorTests;

public class CalculationControllerTests
{
    [Fact]
    public void Index_ReturnsAsViewResult()
    {
        // Arrange
        var mock = new Mock<ICalculationService>();
        var calculationController = new CalculationController(mock.Object);
        
        // Act
        var result = calculationController.Index();
        
        // Assert
        Assert.IsType<ViewResult>(result);
    }
    
    // TEST
    [Fact]
    public void Privacy_ReturnsAsViewResult()
    {
        // Arrange
        var mock = new Mock<ICalculationService>();
        var calculationController = new CalculationController(mock.Object);
        
        // Act
        var result = calculationController.Privacy();
        
        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Error_ReturnsAsViewResult()
    {
        // Arrange
        var mock = new Mock<ICalculationService>();
        var calculationController = new CalculationController(mock.Object);

        // Act
        calculationController.ControllerContext.HttpContext = new DefaultHttpContext();
        var result = calculationController.Error();
        
        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void GetExpression_ReturnsAsJsonWithCorrectData()
    {
        // Arrange
        const string expression = "1+1";
        var mock = new Mock<ICalculationService>();
        mock.Setup(calcService => calcService.Calculate(expression)).Returns(GetList());
        var calculationController = new CalculationController(mock.Object);
        
        // Act
        var result = calculationController.GetExpression(new DataModel(expression));
        
        // Assert
        var jsonResult = Assert.IsType<JsonResult>(result);
        var valueResult = Assert.IsType<List<string>>(jsonResult.Value);
        Assert.True(valueResult.SequenceEqual(GetList()));
    }

    [Fact]
    public void GetExpression_ReturnsAsJsonWithError()
    {
        // Arrange
        const string expression = "1+1";
        var mock = new Mock<ICalculationService>();
        mock.Setup(calcService => calcService.Calculate(expression)).Returns(GetList());
        var calculationController = new CalculationController(mock.Object);
        calculationController.ModelState.AddModelError("Value", "Required");
        
        // Act
        var result = calculationController.GetExpression(new DataModel(expression));
        
        // Assert
        var jsonResult = Assert.IsType<JsonResult>(result);
        Assert.Equal("ModelError", jsonResult.Value);
    }

    private static List<string> GetList()
    {
        return new List<string>()
        {
            "1+1=2",
            "Result: 2" 
        };
    }
}
