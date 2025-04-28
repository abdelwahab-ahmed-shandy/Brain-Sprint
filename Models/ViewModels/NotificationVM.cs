using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Enums;

namespace Models.ViewModels
{
    public class NotificationVM
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public NotificationType Type { get; set; }
        public string Icon { get; set; }
        public bool AutoDismiss { get; set; }
        public int Delay { get; set; }
    }
}
