using ControleEmpresasFuncionariosMvc.Dtos;
using ControleEmpresasFuncionariosMvc.Models.ViewModels;
using ControleEmpresasFuncionariosMvc.Services;
using Microsoft.AspNetCore.Mvc;


namespace ControleEmpresasFuncionariosMvc.Controllers
{
    public class HomeController(CompanyService companyService, JobsPersonsService jobsPersonsService, PersonService personService) : Controller
    {
        private readonly CompanyService _companyService = companyService;
        private readonly JobsPersonsService _jobsPersonsService = jobsPersonsService;
        private readonly PersonService _personService = personService;

        public async Task<IActionResult> Index(string filter)
        {
            var persons = await _personService.PersonsListForSearch(filter);
            var companies = await _companyService.CompaniesListForSearch(filter);

            var response = new ResponseViewModel<CompaniesJobsPersonsHomeDto>()
            {
                Content = new CompaniesJobsPersonsHomeDto
                {
                    CountCompanies = await _companyService.Count(),
                    CountPersons = await _personService.Count(),
                    CountWorkers = await _jobsPersonsService.Count(),
                    Companies = companies,
                    Persons = persons
                }           
            };

            return View(response);
        }        
    }     
}
