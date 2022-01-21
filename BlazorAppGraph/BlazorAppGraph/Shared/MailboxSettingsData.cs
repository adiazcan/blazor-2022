using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAppGraph.Shared
{
    public class MailboxSettingsData
    {
        public string Name { get; set; }
        public string Data { get; set; }
    }

    public class MailboxSettingsModel
    {
        [Required]
        public string EmailMailboxSettings { get; set; }
    }
}
