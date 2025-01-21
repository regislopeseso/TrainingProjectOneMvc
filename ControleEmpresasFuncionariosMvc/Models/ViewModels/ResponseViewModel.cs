namespace ControleEmpresasFuncionariosMvc.Models.ViewModels
{
    public class ResponseViewModel<T>
    {
        public T? Content { get; set; }

        public string? Message { get; set; }
        public int? Page {  get; set; }
        public int? PagesQty { get; set; }
    }
}
