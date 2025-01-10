using ControleEmpresasFuncionariosMvc.Data;
using ControleEmpresasFuncionariosMvc.Dtos;
using ControleEmpresasFuncionariosMvc.Models;
using Microsoft.EntityFrameworkCore;
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
        public async Task<List<JobPersonIndexDto>> FindAllAsync(int companyId)
        {
            return await _context.Job
                .Where(a => a.Company.Id == companyId)
                .Select(a => new JobPersonIndexDto
                {
                    Job = new JobDto
                    {
                        Id = a.Id,
                        Name = a.Name,
                        CompanyId = a.Company.Id,
                    },

                    Persons = a.Persons.Select(p => new PersonDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                    }).OrderBy(c => c.Name).ToList(),

                })
                .OrderBy(c => c.Job.Name)
                .ToListAsync();
        }


        #region CREATE
        public async Task<(bool, string)> CreateAsync(JobPersonDto jobPersonDto)
        {
            var person = await _context.Person
                .Include(a => a.Jobs)
                .Where(a => a.Id == jobPersonDto.PersonId)
                .FirstOrDefaultAsync();

            var job = await _context.Job.FindAsync(jobPersonDto.JobId);

            if (person == null || job == null)
            {
                return (false, "Dados inválidos");
            }

            person.Jobs.Add(job);

            await _context.SaveChangesAsync();

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
