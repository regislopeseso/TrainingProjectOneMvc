using System.ComponentModel.DataAnnotations;

namespace ControleEmpresasFuncionariosMvc.Dtos
{
    public class CompanyDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Cnpj { get; set; }
    }
}
