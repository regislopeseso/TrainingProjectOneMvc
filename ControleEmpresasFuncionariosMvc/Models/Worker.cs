namespace ControleEmpresasFuncionariosMvc.Models
{
    public class Worker
    {
        public int Id { get; set; }
        public Person Person { get; set; }
        public Company Company { get; set; }
        public Job Job { get; set; }

    }
}
