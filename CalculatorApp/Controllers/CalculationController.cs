using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CalculatorApp.Models;
using CalculatorApp.Services.Interfaces;

namespace CalculatorApp.Controllers;

public class CalculationController : Controller
{
    private readonly ICalculationService _calculationService;

    public CalculationController(ICalculationService calculationService)
    {
        _calculationService = calculationService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    [HttpPost]
    public JsonResult GetExpression([FromBody]DataModel expression)
    {
        return ModelState.IsValid
            ? Json(_calculationService.Calculate(expression.Value))
            : Json("ModelError");
    }
}