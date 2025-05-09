﻿using ControleEmpresasFuncionariosMvc.Data;
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
        public async Task<(List<CompanyDto>, int companiesQty)> FindAll(int page)
        {
            const int pageSize = 5;

            var companiesQty = await _context.Company.CountAsync();
            var maxPages = (int)Math.Ceiling(companiesQty / (decimal)pageSize);
            var lastPages = 0;
            


            if (page > maxPages)
            {
                lastPages = (pageSize * maxPages) - pageSize;
            }
            else if (page < 0)
            {
                lastPages = 0;
            }
            else
            {
                lastPages = pageSize * page;
            }

            var companies = await _context.Company
                .Select(a => new CompanyDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Cnpj = a.Cnpj,
                })
                .OrderBy(c => c.Name)
                .Skip(lastPages)
                .Take(pageSize)
                .ToListAsync();

            return (companies, maxPages);
        }
        public async Task<List<CompanyDto>> CompaniesListForSearch(string filter)
        {

            if (string.IsNullOrWhiteSpace(filter) == true)
            {
                return [];
            }

            return await _context.Company.AsNoTracking()
                .Where(a => a.Name.Contains(filter))
                .Select(a => new CompanyDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Cnpj = a.Cnpj,
                })
                .OrderBy(c => c.Name)
                .ToListAsync();
        }
        public async Task<int> Count()
        {
            return await _context.Company.CountAsync();
        }

        #region Create
        public async Task<(CompanyDto, string)> Create(CompanyDto company)
        {
            var (isValid, message) = await this.CompanyIsValid(company);

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
        private async Task<(bool, string)> CompanyIsValid(CompanyDto company)
        {
            if (company == null)
            {
                return (false, "É necessário informar os dados da Empresa a ser cadastrada!");
            }

            if (string.IsNullOrWhiteSpace(company.Name) == true)
            {
                return (false, "É necessário preencher o campo \"Nome\"");
            }

            if (company.Name.Length < 3)
            {
                return (false, "É necessário que o nome da empresa tenha pelo menos 3 caracteres");
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

            if (await _context.Company.AnyAsync(a => a.Cnpj == company.Cnpj) == true)
            {
                return (false, "Já existe uma empresa com este CNPJ!");
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

        public async Task Delete(int id)
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
                .Where(a => a.Id == id)
                .Select(a => new CompanyDetailsDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Cnpj = a.Cnpj,
                    JobsQty = a.Jobs.Count,
                    WorkersQty = a.Jobs.SelectMany(b => b.Persons).Count(),
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
        public async Task<(CompanyDto?, string)> Edit(int? id)
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


        public async Task<(CompanyDto, string)> Edit(CompanyDto company)
        {
            var (isValid, message) = this.EditIsValid(company);

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
        private (bool, string) EditIsValid(CompanyDto company)
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

