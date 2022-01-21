using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAppGraph.Shared
{
    public class PresenceData
    {
        public string Name { get; set; }
        public string Data { get; set; }
    }

    public class EmailPresenceModel
    {
        [Required]
        public string EmailPresence { get; set; } = "adelev@M365x81424674.onmicrosoft.com";
    }
}
