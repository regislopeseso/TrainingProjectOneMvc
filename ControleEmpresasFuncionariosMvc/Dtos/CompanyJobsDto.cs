namespace ControleEmpresasFuncionariosMvc.Dtos
{
    public class CompanyJobsDto
    {
        public string Name { get; set; }
        public int CompanyId { get; set; }
        public List<JobDto> Jobs { get; set; }

    }
}
