namespace ControleEmpresasFuncionariosMvc.Dtos
{
    public class CompanyDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Cnpj { get; set; }
        public List<JobDto> Jobs { get; set; } = [];
        public int JobsQty { get; set; }
    }
}
