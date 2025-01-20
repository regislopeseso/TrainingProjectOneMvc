using System.ComponentModel.DataAnnotations;

namespace ControleEmpresasFuncionariosMvc.Dtos
{
    public class PersonDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? BirthDate { get; set; }
        public List<JobCompanyDto>? Jobs {  get; set; }
    }
}
