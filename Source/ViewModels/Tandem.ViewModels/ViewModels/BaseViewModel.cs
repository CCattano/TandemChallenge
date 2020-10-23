using System.Collections.Generic;

namespace Tandem.Web.App.ViewModels.ViewModels
{
    public class BaseViewModel
    {
        public bool Success { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}
