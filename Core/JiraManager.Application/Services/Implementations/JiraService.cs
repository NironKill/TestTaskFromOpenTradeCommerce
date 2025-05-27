﻿using JiraManager.Application.Common.Configurations;
using JiraManager.Application.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text;

namespace JiraManager.Application.Services.Implementations
{
    public class JiraService : IJiraService
    {
        private readonly IApiService _api;

        public JiraService(IApiService api) => _api = api;

        public AuthenticationHeaderValue GetAuthenticationToken() => new AuthenticationHeaderValue(
            "Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_api.GetApiConfiguration(JiraOption.Username)}:{_api.GetApiConfiguration(JiraOption.ApiToken)}")));

        public string GetUrl() => _api.GetApiConfiguration(JiraOption.Url);
    }
}
