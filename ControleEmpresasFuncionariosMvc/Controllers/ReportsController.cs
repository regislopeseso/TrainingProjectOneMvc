using ControleEmpresasFuncionariosMvc.Dtos;
using ControleEmpresasFuncionariosMvc.Models;
using ControleEmpresasFuncionariosMvc.Models.ViewModels;
using ControleEmpresasFuncionariosMvc.Services;
using Microsoft.AspNetCore.Mvc;
using NuGet.DependencyResolver;

namespace ControleEmpresasFuncionariosMvc.Controllers
{
    public class ReportsController
        (
            JobsPersonsService jobsPersonsService,
            PersonService personService,

            CompanyService companyService,
            JobService jobService
        ) : Controller
    {
        private readonly JobsPersonsService _jobsPersonsService = jobsPersonsService;
        private readonly PersonService _personService = personService;

        private readonly CompanyService _companyService = companyService;
        private readonly JobService _jobService = jobService;
        #region GET: Reports
        public async Task<IActionResult> IndexAsync()
        {
            var companiesQty= await _companyService.Count();
            var unemploeyQty = await _personService.CountUnemployed();
            var workersQty = await _jobsPersonsService.CountAsync();

            var response = new ResponseViewModel<ReportsIndexDto>()
            {          
                Content = new ReportsIndexDto()
                {
                    CompaniesCount = companiesQty,
                    UnemployedCount = unemploeyQty,
                    WorkersCount = workersQty,
                },
            };

            return View(response);
        }

        public async Task<IActionResult> ReportCompaniesWorkersJobsAsync()
        {
            var jobs = await _jobsPersonsService.FindCompaniesAsync();

            var response = new ResponseViewModel<List<CompaniesWorkersJobsReportDto>>()
            {
                Content = jobs              
            };

            return View(response);
        }

        public async Task<IActionResult> ReportUnemployedAsync()
        {
            var persons = await _personService.FindUnemployed();

            var response = new ResponseViewModel<List<UnemployedReportDto>>()
            {
                Content = persons
            };

            return View(response);
        }

        public async Task<IActionResult> ReportWorkersJobsCompaniesAsync()
        {
            var workers = await _jobsPersonsService.ShowAllAsync();

            var response = new ResponseViewModel<List<WorkersJobsCompaniesReportDto>>()
            {
                Content = workers
            };

            return View(response);
        }

        #endregion
    }
}
