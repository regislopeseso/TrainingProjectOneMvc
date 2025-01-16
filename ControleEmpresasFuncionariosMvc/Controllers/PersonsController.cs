using ControleEmpresasFuncionariosMvc.Data;
using ControleEmpresasFuncionariosMvc.Dtos;
using ControleEmpresasFuncionariosMvc.Models.ViewModels;
using ControleEmpresasFuncionariosMvc.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControleEmpresasFuncionariosMvc.Controllers
{
    public class PersonsController(PersonService personService) : Controller
    {
        private readonly PersonService _personService = personService;

        #region GET: Persons
        public async Task<IActionResult> Index()
        {
            var result = await _personService.FindAllAsync();

            var response = new ResponseViewModel<List<PersonDto>>()
            {
                Content = result
            };

            return View(response);
        }
        #endregion

        #region CREATE
        public IActionResult Create()
        {
            return View(new ResponseViewModel<PersonDto>());
        }

        //Post
        [HttpPost]
        public async Task<IActionResult> Create(PersonDto person)
        {
            var (result, message) = await _personService.CreateAsync(person);

            var response = new ResponseViewModel<PersonDto>()
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
            var (person, message) = await _personService.Delete(id);

            if (person == null)
            {
                return NotFound(message);
            }

            var response = new ResponseViewModel<PersonDto>()
            {
                Content = person,
                Message = message,
            };

            return View(response);
        }

        [HttpPost]
        //Post: Companies/Delete
        public async Task<IActionResult> Delete(int id)
        {
            await _personService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Details
        public async Task<IActionResult> Details(int? id)
        {
            var (person, message) = await _personService.Details(id);

            if (person == null)
            {
                return NotFound(message);
            }

            var response = new ResponseViewModel<PersonDto>()
            {
                Content = person,
                Message = message,
            };

            return View(response);
        }
        #endregion

        #region EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            var (person, message) = await _personService.Edit(id);

            if (person == null)
            {
                return NotFound(message);
            }

            var response = new ResponseViewModel<PersonDto>()
            {
                Content = person,
                Message = message,
            };

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PersonDto person)
        {
            var (result, message) = await _personService.EditAsync(person);

            var response = new ResponseViewModel<PersonDto>()
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
