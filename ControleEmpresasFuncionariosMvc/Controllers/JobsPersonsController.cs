using ControleEmpresasFuncionariosMvc.Data;
using ControleEmpresasFuncionariosMvc.Dtos;
using ControleEmpresasFuncionariosMvc.Models;
using ControleEmpresasFuncionariosMvc.Models.ViewModels;
using ControleEmpresasFuncionariosMvc.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;

namespace ControleEmpresasFuncionariosMvc.Controllers
{
    public class JobsPersonsController(
        JobsPersonsService jobPersonService,
        PersonService personService,
        JobService jobService) : Controller
    {
        private readonly JobsPersonsService _jobPersonService = jobPersonService;
        private readonly PersonService _personService = personService;
        private readonly JobService _jobService = jobService;

        public async Task<IActionResult> Index(int companyId)
        {
            var result = await _jobPersonService.FindAllAsync(companyId);

            var response = new ResponseViewModel<JobPersonIndexDto>()
            {
                Content = result
            };

            return View(response);
        }

        #region CREATE
        //Get
        public async Task<IActionResult> Create(int companyId)
        {
            var persons = await _personService.FindPersonsAsync();
            var jobs = await _jobService.FindJobsAsync(companyId);

            var response = new ResponseViewModel<JobsPersonsDto>()
            {
                Content = new JobsPersonsDto()
                {
                    CompanyId = companyId,
                    Jobs = jobs,
                    Persons = persons,
                }
            };

            return View(response);
        }

        //Post
        [HttpPost]
        public async Task<IActionResult> Create(JobPersonDto jobPerson)
        {
            var (isValid, message) = await _jobPersonService.CreateAsync(jobPerson);

            if (isValid == false)
            {
                var persons = await _personService.FindPersonsAsync();
                var jobs = await _jobService.FindJobsAsync(jobPerson.CompanyId);

                var response = new ResponseViewModel<JobsPersonsDto>()
                {
                    Content = new JobsPersonsDto()
                    {
                        CompanyId = jobPerson.CompanyId,
                        Jobs = jobs,
                        Persons = persons,
                    },
                    Message = message
                };

                return View(response);
            }

            return RedirectToAction("Index", "JobsPersons", new { companyId = jobPerson.CompanyId });
        }
        #endregion

        #region DELETE
        [HttpGet]
        public async Task<IActionResult> Delete(int personId, int jobId)
        {
            var (result, message) = await _jobPersonService.Delete(personId, jobId);

            var response = new ResponseViewModel<JobPersonDeleteDto>()
            {

                Content = result,
                Message = message
            };

            if (string.IsNullOrWhiteSpace(response.Message) == false)
            {
                return View(response);
            }

            return View(response);
        }

        [HttpPost]
        //Post: Companies/Delete
        public async Task<IActionResult> DeleteAsync(int jobId, int personId, int companyId)
        {
            await _jobPersonService.DeleteAsync(jobId, personId);

            return RedirectToAction(nameof(Index), new { companyId = companyId });
        }
        #endregion
    }
}
