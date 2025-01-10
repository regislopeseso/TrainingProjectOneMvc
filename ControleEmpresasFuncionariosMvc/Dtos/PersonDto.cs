using System.ComponentModel.DataAnnotations;

namespace ControleEmpresasFuncionariosMvc.Dtos
{
    public class PersonDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? BirthDate { get; set; }
    }
}
