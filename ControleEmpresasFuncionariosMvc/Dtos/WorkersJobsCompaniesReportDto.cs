namespace ControleEmpresasFuncionariosMvc.Dtos
{
    public class WorkersJobsCompaniesReportDto
    {
        public string PersonName { get; set; }
        public List<JobCompanyDto> JobsCompanies { get; set; }
    }
}
