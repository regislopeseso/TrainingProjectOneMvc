using ControleEmpresasFuncionariosMvc.Dtos;
using ControleEmpresasFuncionariosMvc.Models.ViewModels;
using ControleEmpresasFuncionariosMvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControleEmpresasFuncionariosMvc.Controllers
{
    public class JobsController(JobService jobService) : Controller
    {
        private readonly JobService _jobService = jobService;
   

        #region GET: Jobs
        public async Task<IActionResult> Index(int companyId)
        {
            var companyJobs = await _jobService.Search(companyId);
            var response = new ResponseViewModel<CompanyJobsDto>
            {
                Content = companyJobs,
            };
            return View(response);
        }
        #endregion

        #region CREATE
        [HttpGet]
        public IActionResult Create(int companyId)
        {
            return View(new ResponseViewModel<JobDto>() 
            {
                Content = new JobDto { CompanyId = companyId },  
            });
        }

        //Post
        [HttpPost]
        public async Task<IActionResult> Create(JobDto job)
        {
            var (result, message) = await _jobService.Create(job);

            var response = new ResponseViewModel<JobDto>()
            {
                Content = result,
                Message = message,
            };

            if (string.IsNullOrWhiteSpace(response.Message) == false)
            {
                return View(response);
            }

            return RedirectToAction("Index", "Jobs", new { companyId = job.CompanyId });
        }
        #endregion

        #region DELETE
        public async Task<IActionResult> Delete(int? id)
        {
            var (job, message) = await _jobService.Delete(id);

            if (job == null)
            {
                return NotFound(message);
            }

            var response = new ResponseViewModel<JobDto>()
            {
                Content = job,
                Message = message,
            };

            return View(response);
        }

        [HttpPost]
        //Post: Companies/Delete
        public async Task<IActionResult> Delete(JobDto job)
        {
            await _jobService.Delete(job.Id);

            return RedirectToAction("Index", "Jobs", new { companyId = job.CompanyId });
        }
        #endregion

        #region EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            var (job, message) = await _jobService.Edit(id);

            if (job == null)
            {
                return NotFound(message);
            }

            var response = new ResponseViewModel<JobDto>()
            {
                Content = job,
                Message = message,
            };

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(JobDto job)
        {
            var (result, message) = await _jobService.Edit(job);

            var response = new ResponseViewModel<JobDto>()
            {
                Content = result,
                Message = message,
            };

            if (string.IsNullOrWhiteSpace(response.Message) == false)
            {
                return View(response);
            }

            return RedirectToAction("Index","Jobs",new {companyId = job.CompanyId});
        }
        #endregion

        #region SEARCH
        public async Task<IActionResult> Search(int companyId)
        {
            var jobs = await _jobService.Search(companyId);

            return View(jobs);
        }   
        #endregion

    }
}
