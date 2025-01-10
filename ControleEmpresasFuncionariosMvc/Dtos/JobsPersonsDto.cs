namespace ControleEmpresasFuncionariosMvc.Dtos
{
    public class JobsPersonsDto
    {
        public int CompanyId { get; set; }
        public List<PersonDto> Persons { get; set; }
        public List<JobDto> Jobs { get; set; }
    }
}
