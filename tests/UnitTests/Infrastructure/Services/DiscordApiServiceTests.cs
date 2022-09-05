using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Services;
using Infrastructure.Services.DiscordApi;
using Moq;
using Moq.Protected;
using Xunit;

namespace UnitTests.Infrastructure.Services;

public class DiscordApiServiceTests
{
    private readonly DiscordApiConfiguration _config = new()
    {
        DiscordApiEndpoint = "https://discord.com",
        ClientId = "client_id",
        ClientSecret = "client_secret",
        RedirectUri = "http://localhost:1234"
    };
    
    private readonly Mock<HttpMessageHandler> _messageHandler;
    private readonly HttpClient _httpClient;
    private readonly DiscordApiService _discordApiService;
    
    public DiscordApiServiceTests()
    {
        _messageHandler = new Mock<HttpMessageHandler>();

        _httpClient = new HttpClient(_messageHandler.Object);
        _discordApiService = new DiscordApiService(_config, _httpClient);
    }

    #region AuthCodeExchange

    [Fact(DisplayName = "Form is populated with correct body")]
    public async Task AuthCodeExchange_BodyHasAllFormParams()
    {
        var apiResponseBody = new AuthCodeExchangeResponse
        {
            AccessToken = "access-token"
        };
        var apiResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = JsonContent.Create(apiResponseBody)
        };
        
        _messageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync", 
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(apiResponse);

        var response = await _discordApiService.AuthCodeExchange("code");
        
        Assert.Equal(apiResponseBody.AccessToken, response);
        
        //todo: verify calling body
    }

    [Fact(DisplayName = "Returns empty string when status code is not successful")]
    public async Task AuthCodeExchange_ResponseIsError_EmptyResult()
    {
        var apiResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.BadRequest
        };
        
        _messageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync", 
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(apiResponse);
        
        var response = await _discordApiService.AuthCodeExchange("code");
        
        Assert.Empty(response);
    }

    #endregion

    #region GetAuthorizationInfo

    [Fact(DisplayName = "When success response, with user context, return user context")]
    public async Task GetAuthorizationInfo_SuccessWithUserContext()
    {
        var apiResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = JsonContent.Create(new
            {
                User = new
                {
                    Id = "user-id",
                    Username = "user-name",
                    Discriminator = "1234"
                }
            })
        };
        
        _messageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync", 
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(apiResponse);

        var response = await _discordApiService.GetAuthorizationInfo("token", CancellationToken.None);
        
        Assert.NotNull(response);
        Assert.NotNull(response.User);
        Assert.Equal("user-id", response.User.Id);
        Assert.Equal("user-name", response.User.Username);
        Assert.Equal("1234", response.User.Discriminator);
    }

    [Fact(DisplayName = "When success response, without user context, return null context")]
    public async Task GetAuthorizationInfo_SuccessWithoutUserContext()
    {
        var apiResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = JsonContent.Create(new
            {
                OtherData = "abc"
            })
        };
        
        _messageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync", 
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(apiResponse);

        var response = await _discordApiService.GetAuthorizationInfo("token", CancellationToken.None);
        
        Assert.NotNull(response);
        Assert.Null(response.User);
    }

    [Fact(DisplayName = "When error response, return null")]
    public async Task GetAuthorizationInfo_ErrorResponse()
    {
        var apiResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.BadRequest,
            Content = JsonContent.Create(new
            {
                OtherData = "abc"
            })
        };
        
        _messageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync", 
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(apiResponse);

        var response = await _discordApiService.GetAuthorizationInfo("token", CancellationToken.None);
        
        Assert.Null(response);
    }

    #endregion
}