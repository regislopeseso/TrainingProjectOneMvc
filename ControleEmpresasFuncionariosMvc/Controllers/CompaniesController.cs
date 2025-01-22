using ControleEmpresasFuncionariosMvc.Dtos;
using ControleEmpresasFuncionariosMvc.Models.ViewModels;
using ControleEmpresasFuncionariosMvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControleEmpresasFuncionariosMvc.Controllers
{
    public class CompaniesController(CompanyService companyService) : Controller
    {
        private readonly CompanyService _companyService = companyService;           

        #region GET: Companies
        public async Task<IActionResult> Index(int page = 0)
        {
            var (companies, pagesQty) = await _companyService.FindAll(page);

            var response = new ResponseViewModel<List<CompanyDto>>()
            {
                Content = companies,
                PageIn = page,
                PagesQty = pagesQty
            };

            return View(response);
        }
        #endregion

        #region CREATE
        public IActionResult Create()
        {
            return View(new ResponseViewModel<CompanyDto>());
        }

        //Post
        [HttpPost]
        public async Task<IActionResult> Create(CompanyDto company)
        {
            var (result, message) = await _companyService.Create(company);

            var response = new ResponseViewModel<CompanyDto>()
            {
                Content = result,
                Message = message,
            };

            if (string.IsNullOrWhiteSpace(response.Message) == false)
            {
                return View(response);
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region DELETE
        public async Task<IActionResult> Delete(int? id)
        {
            var (company, message) = await _companyService.Delete(id);

            if (company == null)
            {
                return NotFound(message);
            }

            var response = new ResponseViewModel<CompanyDto>()
            {
                Content = company,
                Message = message,
            };

            return View(response);
        }

        [HttpPost]
        //Post: Companies/Delete
        public async Task<IActionResult> Delete(int id)
        {
            await _companyService.Delete(id);

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            var (company, message) = await _companyService.Details(id);

            if (company == null)
            {
                return NotFound(message);
            }

            var response = new ResponseViewModel<CompanyDetailsDto>()
            {
                Content = company,
                Message = message,
            };

            return View(response);
        }
        #endregion

        #region EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            var (company, message) = await _companyService.Edit(id);

            if (company == null)
            {
                return NotFound(message);
            }

            var response = new ResponseViewModel<CompanyDto>()
            {
                Content = company,
                Message = message,
            };

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CompanyDto company)
        {
            var (result, message) = await _companyService.Edit(company);

            var response = new ResponseViewModel<CompanyDto>()
            {
                Content = result,
                Message = message,
            };

            if (string.IsNullOrWhiteSpace(response.Message) == false)
            {
                return View(response);
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}


