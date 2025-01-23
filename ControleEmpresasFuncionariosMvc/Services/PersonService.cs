using ControleEmpresasFuncionariosMvc.Data;
using ControleEmpresasFuncionariosMvc.Dtos;
using ControleEmpresasFuncionariosMvc.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace ControleEmpresasFuncionariosMvc.Services
{
    public class PersonService
    {
        private readonly ControleEmpresasFuncionariosMvcContext _context;

        public PersonService(ControleEmpresasFuncionariosMvcContext context)
        {
            this._context = context;
        }

        #region SEARCH MECHANISMS
        //public async Task<List<PersonDto>> FindAll()
        //{
        //    return await _context.Person
        //        .OrderBy(c => c.Name)
        //        .Select(a => new PersonDto
        //        {
        //            Id = a.Id,
        //            Name = a.Name,
        //            BirthDate = a.BirthDate,
        //        })
        //        .ToListAsync();
        //}
        public async Task<List<PersonDto>> FindAll(string? filter)
        {
            var personQueryable = _context.Person.AsNoTracking().AsQueryable();

            if (string.IsNullOrWhiteSpace(filter) == false)
            {
                personQueryable = personQueryable.Where(a => a.Name.Contains(filter));
            }

            return await personQueryable
                .Select(a => new PersonDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    BirthDate = a.BirthDate,
                })
                .OrderBy(c => c.Name)
                .ToListAsync();
        }
        public async Task<List<PersonDto>> PersonsListForSearch(string? filter)
        {

            if (string.IsNullOrWhiteSpace(filter) == true)
            {
                return [];
            }

            return await _context.Person.AsNoTracking()
                .Where(a => a.Name.Contains(filter))
                .Select(a => new PersonDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    BirthDate = a.BirthDate,
                })
                .OrderBy(c => c.Name)
                .ToListAsync();
        }
        public async Task<List<PersonDto>> FindPersons()
        {
            return await _context.Person
                .OrderBy(c => c.Name)
                .Select(a => new PersonDto
                {
                    Id = a.Id,
                    Name = a.Name,
                })
                .ToListAsync();
        }
        public async Task<List<UnemployedReportDto>> FindUnemployed()
        {
            return await _context.Person
                .Where(a => a.Jobs.Any() == false)
                .Select(a => new UnemployedReportDto
                {
                    Name = a.Name,
                    Age = DateTime.Now.Subtract(a.BirthDate).Days / 365,

                    //BirthDate = a.BirthDate
                })
                .OrderBy(a => a.Name)
                .ToListAsync();
        }
        public async Task<int> Count()
        {
            return await _context.Person.CountAsync();
        }
        public async Task<int> CountUnemployed()
        {
            return await _context.Person
               .Where(a => a.Jobs.Any() == false)
               .CountAsync();
        }
        #endregion

        #region Create
        public async Task<(PersonDto, string)> Create(PersonDto person)
        {
            var (isValid, message) = this.PersonIsValid(person);

            if (isValid == false)
            {
                return (person, message);
            }

            person.Name = String.Join(" ", person.Name
                                     .Split(" ")
                                     .Select(word => new String(word.Select((c, index) => index == 0 ? char.ToUpper(c) : char.ToLower(c))
                                     .ToArray()))
                                     );

            _context.Add(new Person()
            {
                Name = person.Name,
                BirthDate = (DateTime)person.BirthDate,
            });


            await _context.SaveChangesAsync();

            return (person, message);
        }
        private (bool, string) PersonIsValid(PersonDto person)
        {
            if (person == null)
            {
                return (false, "É necessário informar os dados da pessoa a ser cadastrada");
            }

            if (string.IsNullOrWhiteSpace(person.Name) == true)
            {
                return (false, "É necessário preencher o campo \"Nome\"");
            }

            if (person.Name.Length < 3)
            {
                return (false, "É necessário que o nome tenha no mínimo três caracteres");
            }

            if (person.BirthDate == null)
            {
                return (false, "É necessário preencher o campo \"Data de Nascimento\"");
            }

            if (DateTime.Now.Year - person.BirthDate.Value.Year < 18)
            {
                return (false, "Não é pssível cadastrar um menor de idade");
            }

            if (DateTime.Now.Year - person.BirthDate.Value.Year > 100)
            {
                return (false, "Não é possível cadastrar uma pessoa que tenha mais de 100 anos");
            }

            return (true, string.Empty);
        }
        #endregion

        #region Delete
        public async Task<(PersonDto?, string)> Delete(int? id)
        {
            if (id == null || id <= 0)
            {
                return (null, "Id inexistente!");
            }

            var person = await _context.Person
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Select(x => new PersonDto
                {
                    Name = x.Name,
                    BirthDate = x.BirthDate,
                })
                .FirstOrDefaultAsync();

            if (person == null)
            {
                return (null, "Pessoa inexistente!");
            }

            return (person, string.Empty);
        }

        public async Task Delete(int id)
        {
            var personDB = await _context.Person.FirstOrDefaultAsync(c => c.Id == id);

            if (personDB != null)
            {
                _context.Person.Remove(personDB);
            }

            await _context.SaveChangesAsync();
        }
        #endregion

        #region Details
        public async Task<(PersonDetailsDto?, string)> Details(int? id)
        {

            if (id == null || id <= 0)
            {
                return (null, "Id inexistente");
            }

            var person = await _context.Person
                .Where(a => a.Id == id)
                .Select(a => new PersonDetailsDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    BirthDate = a.BirthDate,
                    Jobs = a.Jobs.Select(b => new JobCompanyDto
                    {
                        JobName = b.Name,
                        CompanyName = b.Company.Name,
                        Cnpj = b.Company.Cnpj,
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (person == null)
            {
                return (null, "Pessoa inexistente!");
            }

            return (person, string.Empty);
        }
        #endregion

        #region Edit
        public async Task<(PersonDto?, string)> Edit(int? id)
        {
            if (id == null || id <= 0)
            {
                return (null, "Id inexistente");
            }

            var person = await _context.Person
               .AsNoTracking()
               .Where(c => c.Id == id)
               .Select(x => new PersonDto
               {
                   Id = x.Id,
                   Name = x.Name,
                   BirthDate = x.BirthDate,
               })
               .FirstOrDefaultAsync();

            if (person == null)
            {
                return (null, "Pessoa inexistente!");
            }

            return (person, string.Empty);
        }

        public async Task<(PersonDto, string)> Edit(PersonDto person)
        {
            var (isValid, message) = this.EditIsValid(person);

            if (isValid == false)
            {
                return (person, message);
            }

            var personDb = await _context.Person.FindAsync(person.Id);

            if (personDb == null)
            {
                return (person, "Pessoa não encontrada!!!");
            }

            person.Name = String.Join(" ", person.Name
                                     .Split(" ")
                                     .Select(word => new String(word.Select((c, index) => index == 0 ? char.ToUpper(c) : char.ToLower(c))
                                     .ToArray()))
                                     );


            personDb.Name = person.Name;
            personDb.BirthDate = (DateTime)person.BirthDate;

            await _context.SaveChangesAsync();

            return (person, string.Empty);
        }
        private (bool, string) EditIsValid(PersonDto person)
        {
            if (person == null)
            {
                return (false, "É necessário informar os dados da pessoa a ser cadastrada");
            }

            if (string.IsNullOrWhiteSpace(person.Name) == true)
            {
                return (false, "É necessário preencher o campo \"Nome\"");
            }

            if (person.BirthDate == null)
            {
                return (false, "É necessário preencher o campo \"Data de Nascimento\"");
            }

            return (true, string.Empty);
        }
        #endregion    
    }
}
