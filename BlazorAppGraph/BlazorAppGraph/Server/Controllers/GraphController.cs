using BlazorAppGraph.Server.Services;
using BlazorAppGraph.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace BlazorAppGraph.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class GraphController : ControllerBase
    {
        private GraphService graphApiClientService;

        public GraphController(GraphService graphApiClientService)
        {
            this.graphApiClientService = graphApiClientService;
        }

        [HttpGet("UserProfile")]
        public async Task<IEnumerable<string>> UserProfile()
        {
            var userData = await graphApiClientService.GetGraphApiUser(User.Identity.Name);
            return new List<string> { $"DisplayName: {userData.DisplayName}",
                $"GivenName: {userData.GivenName}", $"Preferred Language: {userData.PreferredLanguage}" };
        }

        [HttpPost("MailboxSettings")]
        public async Task<IActionResult> MailboxSettings([FromBody] string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest("No email");
            try
            {
                var mailbox = await graphApiClientService.GetUserMailboxSettings(email);

                if (mailbox == null)
                {
                    return NotFound($"mailbox settings for {email} not found");
                }
                var result = new List<MailboxSettingsData> {
                new MailboxSettingsData { Name = "User Email", Data = email },
                new MailboxSettingsData { Name = "AutomaticRepliesSetting", Data = mailbox.AutomaticRepliesSetting.Status.ToString() },
                new MailboxSettingsData { Name = "TimeZone", Data = mailbox.TimeZone },
                new MailboxSettingsData { Name = "Language", Data = mailbox.Language.DisplayName }
            };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("TeamsPresence")]
        public async Task<IActionResult> PresencePost([FromBody] string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest("No email");
            try
            {
                var userPresence = await graphApiClientService.GetPresenceforEmail(email);

                if (userPresence.Count == 0)
                {
                    return NotFound(email);
                }

                var result = new List<PresenceData> {
                new PresenceData { Name = "User Email", Data = email },
                new PresenceData { Name = "Availability", Data = userPresence[0].Availability }
            };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UserCalendar")]
        public async Task<IEnumerable<FilteredEventDto>> UserCalendar(UserCalendarDataModel userCalendarDataModel)
        {
            var userCalendar = await graphApiClientService.GetCalanderForUser(
                userCalendarDataModel.Email,
                userCalendarDataModel.From.Value.ToString("yyyy-MM-ddTHH:mm:ss.sssZ"),
                userCalendarDataModel.To.Value.ToString("yyyy-MM-ddTHH:mm:ss.sssZ"));

            return userCalendar.Select(l => new FilteredEventDto
            {
                IsAllDay = l.IsAllDay.GetValueOrDefault(),
                Sensitivity = l.Sensitivity.ToString(),
                Start = l.Start?.DateTime,
                End = l.End?.DateTime,
                ShowAs = l.ShowAs.Value.ToString(),
                Subject = l.Subject
            });
        }
    }
}
