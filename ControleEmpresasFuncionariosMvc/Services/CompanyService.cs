using ControleEmpresasFuncionariosMvc.Data;
using ControleEmpresasFuncionariosMvc.Dtos;
using ControleEmpresasFuncionariosMvc.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace ControleEmpresasFuncionariosMvc.Services
{
    public class CompanyService
    {
        private readonly ControleEmpresasFuncionariosMvcContext _context;

        public CompanyService(ControleEmpresasFuncionariosMvcContext context)
        {
            this._context = context;
        }
        public async Task<List<CompanyDto>> FindAllAsync()
        {
            return await _context.Company
                .Select(a => new CompanyDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Cnpj = a.Cnpj,
                })
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        #region Create
        public async Task<(CompanyDto, string)> CreateAsync(CompanyDto company)
        {
            var (isValid, message) = await this.CompanyIsValidAsync(company);

            if (isValid == false)
            {
                return (company, message);
            }

            _context.Add(new Company()
            {
                Name = company.Name,
                Cnpj = company.Cnpj,
            });


            await _context.SaveChangesAsync();

            return (company, message);
        }
        private async Task<(bool, string)> CompanyIsValidAsync(CompanyDto company)
        {
            if (company == null)
            {
                return (false, "É necessário informar os dados da Empresa a ser cadastrada!");
            }

            if (string.IsNullOrWhiteSpace(company.Name) == true)
            {
                return (false, "É necessário preencher o campo \"Nome\"");
            }

            if (string.IsNullOrWhiteSpace(company.Cnpj) == true)
            {
                return (false, "É necessário informar o Cnpj da empresa!");
            }

            string pattern = @"(^\d{2}).(\d{3}).(\d{3})/(\d{4})-(\d{2}$)";

            if (Regex.IsMatch(company.Cnpj, pattern) == false)
            {
                return (false, "Formato incorreto! O formato correto é: 00.000.000/0000-00");
            }

            if (await _context.Company.AnyAsync(a => a.Name == company.Name) == true)
            {
                return (false, "Já existe uma empresa com este nome!");
            }

            return (true, string.Empty);
        }
        #endregion

        #region Delete
        public async Task<(CompanyDto?, string)> Delete(int? id)
        {
            if (id == null || id <= 0)
            {
                return (null, "Id inexistente!");
            }

            var company = await _context.Company
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Select(x => new CompanyDto
                {
                    Name = x.Name,
                    Cnpj = x.Cnpj,
                })
                .FirstOrDefaultAsync();

            if (company == null)
            {
                return (null, "Empresa inexistente!");
            }

            return (company, string.Empty);
        }

        public async Task DeleteAsync(int id)
        {
            var companyDb = await _context.Company.FirstOrDefaultAsync(c => c.Id == id);

            if (companyDb != null)
            {
                _context.Company.Remove(companyDb);
            }

            await _context.SaveChangesAsync();
        }
        #endregion

        #region Details
        public async Task<(CompanyDetailsDto?, string)> Details(int? id)
        {

            if (id == null || id <= 0)
            {
                return (null, "Id inexistente");
            }

            var company = await _context.Company
                .Where(c => c.Id == id)
                .Select(x => new CompanyDetailsDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Cnpj = x.Cnpj,
                    Jobs = x.Jobs.Select(a => new JobDto
                    {
                        Id = a.Id,
                        Name = a.Name,
                    }).ToList(),
                    JobsQty = x.Jobs.Count(),
                })
                .FirstOrDefaultAsync();

            if (company == null)
            {
                return (null, "Empresa inexistente!");
            }

            return (company, string.Empty);
        }
        #endregion

        #region Edit
        public async Task<(CompanyDto?, string)> Edit(int? id) // Segue a lógica do método Delete(Service)
        {
            if (id == null || id <= 0)
            {
                return (null, "Id inexistente");
            }

            var company = await _context.Company
               .AsNoTracking()
               .Where(c => c.Id == id)
               .Select(x => new CompanyDto
               {
                   Id = x.Id,
                   Name = x.Name,
                   Cnpj = x.Cnpj,
               })
               .FirstOrDefaultAsync();

            if (company == null)
            {
                return (null, "Empresa inexistente!");
            }

            return (company, string.Empty);
        }


        public async Task<(CompanyDto, string)> EditAsync(CompanyDto company)
        {
            var (isValid, message) = this.EditIsValidAsync(company);

            if (isValid == false)
            {
                return (company, message);
            }

            var companyDb = await _context.Company.FindAsync(company.Id);

            if (companyDb == null)
            {
                return (company, "Empresa não encontrada!!!");
            }


            companyDb.Name = company.Name;
            companyDb.Cnpj = company.Cnpj;        
         
            await _context.SaveChangesAsync();

            return (company, string.Empty);
        }
        private (bool, string) EditIsValidAsync(CompanyDto company)
        {
            if (company == null)
            {
                return (false, "É necessário enviar os dados da Empresa a ser editada");
            }

            if (string.IsNullOrWhiteSpace(company.Name) == true)
            {
                return (false, "É necessário preencher o Nome da empresa");
            }

            if (string.IsNullOrWhiteSpace(company.Cnpj) == true)
            {
                return (false, "É necessário informar o Cnpj da empresa");
            }

            string pattern = @"(^\d{2}).(\d{3}).(\d{3})/(\d{4})-(\d{2}$)";

            if (Regex.IsMatch(company.Cnpj, pattern) == false)
            {
                return (false, "Formato incorreto! O formato correto é: 00.000.000/0000-00");
            }            

            return (true, string.Empty);
        }
        #endregion
       
       
    }
}

