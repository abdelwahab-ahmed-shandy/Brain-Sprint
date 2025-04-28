using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class ConfirmationVM
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string Icon { get; set; }
        public string ButtonText { get; set; }
        public string ButtonUrl { get; set; }
    }
}
