namespace ControleEmpresasFuncionariosMvc.Dtos
{
    public class UnemployedReportDto
    {
        public string Name { get; set; }
        public int Age { get; set; }

        //Sintaxe alternativa:
        //Obs: PersonService.FindUnemployed() precisa ser alterado também
        //public DateTime BirthDate { get; set; }  
        //public int Age
        //{
        //    get
        //    {
        //        return  DateTime.Now.Year - BirthDate.Year;
        //    }
        //}
    }
}

