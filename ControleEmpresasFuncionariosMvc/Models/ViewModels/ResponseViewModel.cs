using Newtonsoft.Json.Linq;

namespace ControleEmpresasFuncionariosMvc.Models.ViewModels
{
    public class ResponseViewModel<T>
    {
        public T? Content { get; set; } 
        public int? PageIn { private get; set; }
        public string? Message { get; set; }
        //public int? Page {  get; set; }
        public int? PagesQty { get; set; }

        public int? Page
        {
            get
            {
                if (PageIn < 0) return 0;
                if (PageIn > PagesQty) return PagesQty-1;
                return PageIn;
            }           
        }
    }
}
