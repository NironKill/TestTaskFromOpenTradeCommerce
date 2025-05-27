using JiraManager.Application.DTOs;
using JiraManager.Application.Repositories.Interfaces;
using JiraManager.Application.Services.Interfaces;
using JiraManager.Infrastructure.Responses.JIra;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;

namespace JiraManager.Infrastructure.Services.JIra
{
    public class JiraApiService : IJiraApiService
    {
        private readonly IJiraService _jira;
        private readonly ITicketRepository _ticket;

        private readonly IHttpClientFactory _clientFactory;

        public JiraApiService(IJiraService jira, ITicketRepository ticket, IHttpClientFactory clientFactory)
        {
            _jira = jira;
            _ticket = ticket;

            _clientFactory = clientFactory;
        }

        public async Task<bool> CreateIssue(JiraUserResponse jiraUserResponse, TicketDTO ticketDTO, CancellationToken cancellationToken)
        {
            using HttpClient client = _clientFactory.CreateClient();
            
            client.DefaultRequestHeaders.Authorization = _jira.GetAuthenticationToken();

            var issue = new
            {
                fields = new
                {
                    project = new { key = "MFLP" },
                    summary = ticketDTO.Summary,
                    reporter = new { id = jiraUserResponse.AccountId ?? "" },
                    priority = new { name = ticketDTO.Priority },
                    issuetype = new { name = "Bug" },
                    description = new
                    {
                        version = 1,
                        type = "doc",
                        content = new[] {
                             new {
                                 type = "paragraph",
                                 content = new []{
                                     new {
                                         type = "text",
                                         text =  ticketDTO.Summary
                                     }
                                 }
                             }
                        }
                    },
                }
            };

            HttpResponseMessage response = await client.PostAsJsonAsync($"{_jira.GetUrl()}/rest/api/3/issue", issue);

            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();

                throw new HttpRequestException($"Error: {errorMessage}");
            }

            string responseData = await response.Content.ReadAsStringAsync();
            IssueResponse issueDto = JsonConvert.DeserializeObject<IssueResponse>(responseData);

            if (issueDto == null)
                return false;

            ticketDTO.TicketUrl = $"{_jira.GetUrl()}/browse/{issueDto.Key}";
            ticketDTO.AccountId = jiraUserResponse.AccountId;
            ticketDTO.Key = issueDto.Key;
            ticketDTO.TicketJiraId = issueDto.Id;

            await _ticket.Create(ticketDTO, cancellationToken);

            return true;    
        }

        public async Task<JiraUserResponse> CreateUser(UserDTO dto)
        {
            var payload = new
            {
                displayName = dto.Name,
                products = new[] { "jira-software" }
            };

            using HttpClient client = _clientFactory.CreateClient();
         
            client.DefaultRequestHeaders.Authorization = _jira.GetAuthenticationToken();

            StringContent content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync($"{_jira.GetUrl()}/rest/api/3/user", content);

            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();

                throw new HttpRequestException($"Error: {errorMessage}");
            }

            string responseData = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<JiraUserResponse>(responseData);          
        }

        public async Task<JiraUserResponse> GetUserByEmail(string email)
        {
            string destination = $"{_jira.GetUrl()}/rest/api/3/user/search?query={email}";

            using HttpClient client = _clientFactory.CreateClient();
            
            client.DefaultRequestHeaders.Authorization = _jira.GetAuthenticationToken();

            HttpResponseMessage response = await client.GetAsync(destination);

            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();

                throw new HttpRequestException($"Error: {errorMessage}");
            }

            string responseAsJson = await response.Content.ReadAsStringAsync();
            List<JiraUserResponse> users = JsonConvert.DeserializeObject<List<JiraUserResponse>>(responseAsJson);

            return users?.FirstOrDefault(); 
        }

        public async Task<List<TicketDTO>> GetAllTicketByAccountId(string accountId)
        {
            string destination = $"{_jira.GetUrl()}/rest/api/3/search?jql=assignee={accountId} OR reporter={accountId}";

            using HttpClient client = _clientFactory.CreateClient();
            
            client.DefaultRequestHeaders.Authorization = _jira.GetAuthenticationToken();

            HttpResponseMessage response = await client.GetAsync(destination);

            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();

                throw new HttpRequestException($"Error: {errorMessage}");
            }
        
            string responseAsJson = await response.Content.ReadAsStringAsync();
            IssuelistResponse issues = JsonConvert.DeserializeObject<IssuelistResponse>(responseAsJson);

            List<TicketDTO> tickets = new List<TicketDTO>();

            foreach (var issue in issues.Issues)
            {
                TicketDTO ticket = new TicketDTO()
                {
                    TicketUrl = $"{_jira.GetUrl()}/browse/{issue.Key}",
                    Status = issue.Fields.Status.Name,
                    Priority = issue.Fields.Priority.Name,
                    Summary = issue.Fields.Summary
                };
                tickets.Add(ticket);
            }
            return tickets;                
        }
    }
}
