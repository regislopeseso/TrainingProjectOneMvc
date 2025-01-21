namespace ControleEmpresasFuncionariosMvc.Dtos
{
    public class WorkersJobsCompaniesReportDto
    {
        public string PersonName { get; set; }
        public List<JobCompany_Dto> JobsCompanies { get; set; }

        public class JobCompany_Dto
        {
            public int JobId { get; set; }
            public string JobName { get; set; }
            public int CompanyId { get; set; }
            public string CompanyName { get; set; }
            public string? Cnpj { get; set; }
        }
    }
}
