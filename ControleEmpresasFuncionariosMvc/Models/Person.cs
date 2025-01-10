
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControleEmpresasFuncionariosMvc.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }

        [InverseProperty("Persons")]
        public List<Job> Jobs { get; set; }
    }
}
