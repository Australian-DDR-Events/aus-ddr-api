using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Interfaces.ExternalServices;
using Application.Core.Interfaces.ExternalServices.Discord;
using Infrastructure.Services.DiscordApi;
using JetBrains.Annotations;

namespace Infrastructure.Services;

public class DiscordApiService : IDiscordApiService
{
    private readonly DiscordApiConfiguration _configuration;
    private readonly HttpClient _httpClient;
    
    public DiscordApiService(DiscordApiConfiguration configuration, HttpClient httpClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(_configuration.DiscordApiEndpoint);
    }
    
    public async Task<string> AuthCodeExchange(string code)
    {
        var content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
        {
            new("client_id", _configuration.ClientId),
            new("client_secret", _configuration.ClientSecret),
            new("grant_type", "authorization_code"),
            new("code", code),
            new("redirect_uri", _configuration.RedirectUri)
        });
        var response = await _httpClient.PostAsync("/api/v10/oauth2/token", content);
        if (response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadFromJsonAsync<AuthCodeExchangeResponse>();
            return body?.AccessToken;
        }

        return string.Empty;
    }

    public async Task<AuthorizationMeResponse> GetAuthorizationInfo(string token, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/v10/oauth2/@me");
        request.Headers.Add("Authorization", $"Bearer {token}");
        var response = await _httpClient.SendAsync(request, cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<AuthorizationMeResponse>(cancellationToken: cancellationToken);
        }

        return null;
    }
}