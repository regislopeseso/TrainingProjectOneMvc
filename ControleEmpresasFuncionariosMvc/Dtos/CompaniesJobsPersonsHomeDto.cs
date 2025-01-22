namespace ControleEmpresasFuncionariosMvc.Dtos
{
    public class CompaniesJobsPersonsHomeDto
    {
        public int CountCompanies { get; set; }
        public int CountPersons { get; set; }
        public int CountWorkers { get; set; }

        public List<CompanyDto> Companies { get; set; }
        public List<PersonDto> Persons { get; set; }

    }
}
