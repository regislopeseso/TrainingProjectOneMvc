using System.Diagnostics;
using ControleEmpresasFuncionariosMvc.Dtos;
using ControleEmpresasFuncionariosMvc.Data;
using ControleEmpresasFuncionariosMvc.Models.ViewModels;
using ControleEmpresasFuncionariosMvc.Services;
using Microsoft.AspNetCore.Mvc;


namespace ControleEmpresasFuncionariosMvc.Controllers
{
    public class HomeController(CompanyService companyService, JobService jobService, PersonService personService) : Controller
    {
        private readonly CompanyService _companyService = companyService;
        private readonly JobService _jobService = jobService;
        private readonly PersonService _personService = personService;

        public async Task<IActionResult> Index()
        {
            var response = new ResponseViewModel<CompaniesJobsPersonsHomeDto>()
            {
                Content = new CompaniesJobsPersonsHomeDto
                {
                    CountCompanies = await _companyService.CountAsync(),
                    CountJobs = await _jobService.CountAsync(),
                    CountPersons = await _personService.CountAsync(),
                }
            };

            return View(response);
        }        
    }     
}
