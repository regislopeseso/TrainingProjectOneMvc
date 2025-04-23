using ControleEmpresasFuncionariosMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleEmpresasFuncionariosMvc.Data
{
    public class ControleEmpresasFuncionariosMvcContext : DbContext
    {
        public ControleEmpresasFuncionariosMvcContext (DbContextOptions<ControleEmpresasFuncionariosMvcContext> options) 
            : base(options) 
        {
        }

        public DbSet<Company> Company { get; set; } = default;
        public DbSet<Job> Job { get; set; } = default;
        public DbSet<Person> Person { get; set; } = default;
    }
}
