using ControleEmpresasFuncionariosMvc.Data;
using ControleEmpresasFuncionariosMvc.Dtos;
using ControleEmpresasFuncionariosMvc.Models;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;


namespace ControleEmpresasFuncionariosMvc.Services
{
    public class JobService
    {
        private readonly ControleEmpresasFuncionariosMvcContext _context;

        public JobService(ControleEmpresasFuncionariosMvcContext context)
        {
            this._context = context;
        }

        #region Search Mechanisms
        public async Task<CompanyJobsDto?> Search(int companyId)
        {

            return await _context.Company
                .Where(x => x.Id == companyId)
                .Select(x => new CompanyJobsDto
                {
                    CompanyId = x.Id,
                    Name = x.Name,
                    Jobs = x.Jobs.Select(job => new JobDto
                    {
                        Id = job.Id,
                        Name = job.Name,
                    }).ToList(),
                }).FirstOrDefaultAsync();
        }

        public async Task<List<JobDto>> FindJobs(int companyId)
        {
            return await _context.Job
                .Where(x => x.Company.Id == companyId)
                .Select(x => new JobDto
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToListAsync();
        }
        public async Task<int> Count()
        {
            return await _context.Job.CountAsync();
        }



        #endregion

        #region Create
        public async Task<(JobDto, string)> Create(JobDto job)
        {
            var (isValid, message) = this.JobIsValid(job);

            if (isValid == false)
            {
                return (job, message);
            }

            var company = await _context.Company.FindAsync(job.CompanyId);
            if (company == null)
            {
                return (job, "A empresa não foi encontrada!%$#");
            }

            _context.Add(new Job()
            {
                Name = job.Name,
                Company = company,
            });
            await _context.SaveChangesAsync();

            return (job, message);
        }
        private (bool, string) JobIsValid(JobDto job)
        {
            if (job == null)
            {
                return (false, "É necessário informar os dados do cargo a ser cadastrado");
            }

            if (string.IsNullOrWhiteSpace(job.Name) == true)
            {
                return (false, "É necessário preencher o campo \"Nome\"");
            }

            return (true, string.Empty);
        }
        #endregion

        #region Delete
        public async Task<(JobDto?, string)> Delete(int? id)
        {
            if (id == null || id <= 0)
            {
                return (null, "Id inexistente!");
            }

            var job = await _context.Job
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Select(x => new JobDto
                {
                    Name = x.Name,
                    CompanyId = x.Company.Id,
                })
                .FirstOrDefaultAsync();

            if (job == null)
            {
                return (null, "Cargo inexistente!");
            }

            return (job, string.Empty);
        }

        public async Task Delete(int id)
        {
            var jobDb = await _context.Job.FirstOrDefaultAsync(c => c.Id == id);

            if (jobDb != null)
            {
                _context.Job.Remove(jobDb);
            }

            await _context.SaveChangesAsync();
        }
        #endregion

        #region Edit
        public async Task<(JobDto?, string)> Edit(int? id)
        {
            if (id == null || id <= 0)
            {
                return (null, "Id inexistente");
            }

            var job = await _context.Job
               .AsNoTracking()
               .Where(c => c.Id == id)
               .Select(x => new JobDto
               {
                   Id = x.Id,
                   Name = x.Name,
                   CompanyId = x.Company.Id,
               })
               .FirstOrDefaultAsync();

            if (job == null)
            {
                return (null, "Cargo inexistente!");
            }

            return (job, string.Empty);
        }


        public async Task<(JobDto, string)> Edit(JobDto job)
        {
            var (isValid, message) = this.EditIsValid(job);

            if (isValid == false)
            {
                return (job, message);
            }

            var jobDb = await _context.Job.FindAsync(job.Id);

            if (jobDb == null)
            {
                return (job, "Empresa não encontrada!!!");
            }


            jobDb.Name = job.Name;


            await _context.SaveChangesAsync();

            return (job, string.Empty);
        }
        private (bool, string) EditIsValid(JobDto job)
        {
            if (job == null)
            {
                return (false, "É necessário atribuir um título ao cargo a ser editado");
            }

            if (string.IsNullOrWhiteSpace(job.Name) == true)
            {
                return (false, "É necessário atribuir um título ao cargo a ser editado");
            }

            return (true, string.Empty);
        }
        #endregion         
    }
}