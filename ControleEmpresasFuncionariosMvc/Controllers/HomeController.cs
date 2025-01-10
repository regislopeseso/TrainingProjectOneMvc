using System.Diagnostics;
using ControleEmpresasFuncionariosMvc.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;


namespace ControleEmpresasFuncionariosMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }        
    }     
}
