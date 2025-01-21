using Microsoft.AspNetCore.Mvc;

namespace ControleEmpresasFuncionariosMvc.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult NotFound()
        {
            return View("NotFound");
        }
    }
}
