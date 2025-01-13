using ControleEmpresasFuncionariosMvc.Data;
using ControleEmpresasFuncionariosMvc.Dtos;
using ControleEmpresasFuncionariosMvc.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;
using System.Text.RegularExpressions;

namespace ControleEmpresasFuncionariosMvc.Services
{
    public class JobsPersonsService
    {
        private readonly ControleEmpresasFuncionariosMvcContext _context;

        public JobsPersonsService(ControleEmpresasFuncionariosMvcContext context)
        {
            this._context = context;
        }
        public async Task<JobPersonIndexDto?> FindAllAsync(int companyId)
        {
            return await _context.Company
                .Where(a => a.Id == companyId)
                .Select(a => new JobPersonIndexDto
                {
                    CompanyId = companyId,
                    CompanyName = a.Name,

                    JobPersons = a.Jobs
                        .Select(b => new JobPersonsDto
                        {
                            Job = new JobDto
                            {
                                Id = b.Id,
                                Name = b.Name,
                                CompanyId = b.Company.Id,
                            },

                            Persons = b.Persons
                                 .Select(c => new PersonDto
                                 {
                                     Id = c.Id,
                                     Name = c.Name,
                                 })
                                 .OrderBy(c => c.Name)
                                 .ToList()
                        })
                        .OrderBy(a => a.Job.Name)
                        .ToList()
                })
                .FirstOrDefaultAsync();       
        }


        #region CREATE
        public async Task<(bool, string)> CreateAsync(JobPersonDto jobPerson)
        {
            var (isValid, message) = this.WorkerIsValid(jobPerson);

            if (isValid == false)
            {
                return (false, message);
            }
            var person = await _context.Person
                .Include(a => a.Jobs)
                .Where(a => a.Id == jobPerson.PersonId)
                .FirstOrDefaultAsync();

            var job = await _context.Job.FindAsync(jobPerson.JobId);

            if (person == null || job == null)
            {
                return (false, "Dados inválidos");
            }

            person.Jobs.Add(job);

            await _context.SaveChangesAsync();

            return (true, string.Empty);
        }



        private (bool, string) WorkerIsValid(JobPersonDto jobPerson)
        {
            if (jobPerson == null)
            {
                return (false, "É necessário informar os dados do funcionário a ser cadastrado.");
            }

            if (jobPerson.PersonId == null && jobPerson.JobId == null)
            {
                return (false, "É necessário informar os dados do funcionário a ser cadastrado.");
            }

            if (jobPerson.PersonId == null)
            {
                return (false, "É necessário informar uma pessoa.");
            }

            if (jobPerson.JobId == null)
            {
                return (false, "É necessário informar um cargo.");
            }


            return (true, string.Empty);
        }



        #endregion

        #region Delete
        public async Task<(JobPersonDeleteDto?, string)> Delete(int personId, int jobId)
        {
            if (jobId <= 0 || personId <= 0)
            {
                return (null, "Id(s) inexistente(s)!");
            }

            var job = await _context.Job
                .AsNoTracking()
                .Where(a => a.Id == jobId)
                .Select(a => new JobDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    CompanyId = a.Company.Id,
                })
                .FirstOrDefaultAsync();

            if (job == null)
            {
                return (null, "Este cargo não existe");
            }

            var person = await _context.Person
                .AsNoTracking()
                .Where(a => a.Id == personId
                    && a.Jobs.Any(a => a.Id == job.Id))
                .Select(a => new PersonDto
                {
                    Id = a.Id,
                    Name = a.Name,
                })
                .FirstOrDefaultAsync();


            if (person == null)
            {
                return (null, "Pessoa inexistente!");
            }

            var result = new JobPersonDeleteDto
            {
                Job = job,
                Person = person,

            };

            return (result, string.Empty);
        }

        public async Task DeleteAsync(int jobId, int personId)
        {
            var jobDb = await _context.Job
                .Include(a => a.Persons)
                .Where(a => a.Id == jobId)
                .FirstOrDefaultAsync();

            if (jobDb != null)
            {
                var person = await _context.Person
                    .Where(a => a.Id == personId)
                    .FirstOrDefaultAsync();

                if (person != null)
                {
                    jobDb.Persons.Remove(person);
                    await _context.SaveChangesAsync();
                }
            }
        }
        #endregion
    }
}
