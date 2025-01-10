using System.ComponentModel.DataAnnotations.Schema;

namespace ControleEmpresasFuncionariosMvc.Models
{
    public class Job
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [InverseProperty("Jobs")]
        public Company Company { get; set; }


        [InverseProperty("Jobs")]
        public List<Person> Persons { get; set; } 

    }
}
