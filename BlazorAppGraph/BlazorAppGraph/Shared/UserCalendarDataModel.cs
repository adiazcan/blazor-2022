using System.ComponentModel.DataAnnotations;

namespace BlazorAppGraph.Shared
{
    public class UserCalendarDataModel
    {
        [Required]
        public string Email { get; set; } = "adelev@M365x81424674.onmicrosoft.com";

        [Required]
        public DateTime? From { get; set; }

        [Required]
        public DateTime? To { get; set; }
    }
}
