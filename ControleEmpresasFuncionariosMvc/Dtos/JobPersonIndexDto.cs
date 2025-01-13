namespace ControleEmpresasFuncionariosMvc.Dtos
{
    public class JobPersonIndexDto
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public List<JobPersonsDto> JobPersons { get; set; }
      
    }
}
