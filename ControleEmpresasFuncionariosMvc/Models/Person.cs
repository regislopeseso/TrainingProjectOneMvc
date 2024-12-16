using ControleEmpresasFuncionariosMvc.Models.Enums;

namespace ControleEmpresasFuncionariosMvc.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public EmploymentStatus Status { get; set; }
    }
}
