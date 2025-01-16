using ControleEmpresasFuncionariosMvc.Dtos;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControleEmpresasFuncionariosMvc.Models
{
    public class Company
    {
        public int Id { get; set; }
   
        [DisplayName("Nome")]
        public string Name { get; set; }
        public string? Cnpj { get; set; }

        [InverseProperty("Company")]
        public List<Job> Jobs { get; set; }
    }
}
