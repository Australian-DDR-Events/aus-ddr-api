using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces.ExternalServices;
using Application.Core.Interfaces.ExternalServices.Discord;
using Application.Core.Interfaces.Repositories;
using Application.Core.Models.Connections;
using Application.Core.Services.Connections;
using Moq;
using Xunit;

namespace UnitTests.Core.Services.Connections;

public class DiscordConnectionTests
{
    private readonly Mock<IDancerRepository> _dancerRepository;
    private readonly Mock<IConnectionRepository> _connectionRepository;
    private readonly Mock<IDiscordApiService> _discordApiService;
    private readonly DiscordConnection _discordConnection;

    private readonly string _dancerAuthId = Guid.NewGuid().ToString();
    private readonly Guid _dancerId = Guid.NewGuid();

    private readonly AuthorizationMeResponse.UserPortion _discordUserInfo = new()
    {
        Id = Guid.NewGuid().ToString(),
        Username = "user-name",
        Discriminator = "1234"
    };

    public DiscordConnectionTests()
    {
        _dancerRepository = new Mock<IDancerRepository>();
        _connectionRepository = new Mock<IConnectionRepository>();
        _discordApiService = new Mock<IDiscordApiService>();
        _discordConnection = new DiscordConnection(_dancerRepository.Object, _connectionRepository.Object,
            _discordApiService.Object);
    }

    #region CreateConnection

    private void SetupCreateConnectionDefaultState()
    {
        _dancerRepository.Setup(r =>
            r.GetDancerByAuthId(It.Is<string>(id => id.Equals(_dancerAuthId)))
        ).Returns(
            new Dancer
            {
                Id = _dancerId,
                AuthenticationId = _dancerAuthId
            }
        );

        _connectionRepository.Setup(r =>
            r.GetConnection(It.IsAny<Guid>(), It.IsAny<Connection.ConnectionType>())
        ).Returns(new List<Connection>());

        _discordApiService.Setup(r =>
            r.AuthCodeExchange(It.IsAny<string>())
        ).ReturnsAsync("token");

        _discordApiService.Setup(r =>
            r.GetAuthorizationInfo(It.IsAny<string>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(
            new AuthorizationMeResponse()
            {
                User = _discordUserInfo
            }
        );
    }

    [Fact(DisplayName =
        "When all responses are successful, and user does not have prior connection, create new connection")]
    public async Task CreateConnection_ResponsesSuccessful_NoPriorConnection_CreatesConnection()
    {
        SetupCreateConnectionDefaultState();

        var requestModel = new DiscordConnectionRequestModel()
        {
            Code = "auth-code"
        };

        var result = await _discordConnection.CreateConnection(_dancerAuthId, requestModel, CancellationToken.None);
        
        Assert.True(result);

        var connectionData = JsonSerializer.Serialize(_discordUserInfo);
        _connectionRepository.Verify(
            r => r.CreateConnection(
                It.Is<Connection>(c => c.ConnectionData.Equals(connectionData)),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact(DisplayName =
        "When dancer does not exist, return false")]
    public async Task CreateConnection_DancerNotFound()
    {
        SetupCreateConnectionDefaultState();
        
        _dancerRepository.Setup(r =>
            r.GetDancerByAuthId(It.Is<string>(id => id.Equals(_dancerAuthId)))
        ).Returns(null as Dancer);

        var requestModel = new DiscordConnectionRequestModel()
        {
            Code = "auth-code"
        };

        var result = await _discordConnection.CreateConnection(_dancerAuthId, requestModel, CancellationToken.None);
        
        Assert.False(result);
    }

    [Fact(DisplayName =
        "When connection for Discord already exists, return false")]
    public async Task CreateConnection_ConnectionAlreadyExists()
    {
        SetupCreateConnectionDefaultState();

        _connectionRepository.Setup(r =>
            r.GetConnection(It.IsAny<Guid>(), It.IsAny<Connection.ConnectionType>())
        ).Returns(new List<Connection>
        {
            new()
            {
                Id = Guid.NewGuid()
            }
        });

        var requestModel = new DiscordConnectionRequestModel()
        {
            Code = "auth-code"
        };

        var result = await _discordConnection.CreateConnection(_dancerAuthId, requestModel, CancellationToken.None);
        
        Assert.False(result);
    }

    [Fact(DisplayName =
        "When auth code exchange fails, return false")]
    public async Task CreateConnection_AuthCodeExchangeFails()
    {
        SetupCreateConnectionDefaultState();

        _discordApiService.Setup(r =>
            r.AuthCodeExchange(It.IsAny<string>())
        ).ReturnsAsync(string.Empty);

        var requestModel = new DiscordConnectionRequestModel()
        {
            Code = "auth-code"
        };

        var result = await _discordConnection.CreateConnection(_dancerAuthId, requestModel, CancellationToken.None);
        
        Assert.False(result);
    }

    [Fact(DisplayName =
        "When service fails to get authorization info, return false")]
    public async Task CreateConnection_FailToGetAuthorizationInfo()
    {
        SetupCreateConnectionDefaultState();

        _discordApiService.Setup(r =>
            r.GetAuthorizationInfo(It.IsAny<string>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(null as AuthorizationMeResponse);

        var requestModel = new DiscordConnectionRequestModel()
        {
            Code = "auth-code"
        };

        var result = await _discordConnection.CreateConnection(_dancerAuthId, requestModel, CancellationToken.None);
        
        Assert.False(result);
    }

    [Fact(DisplayName =
        "When service gets authorization info, and no user data, return false")]
    public async Task CreateConnection_NoUserData()
    {
        SetupCreateConnectionDefaultState();

        _discordApiService.Setup(r =>
            r.GetAuthorizationInfo(It.IsAny<string>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(
            new AuthorizationMeResponse()
        );

        var requestModel = new DiscordConnectionRequestModel()
        {
            Code = "auth-code"
        };

        var result = await _discordConnection.CreateConnection(_dancerAuthId, requestModel, CancellationToken.None);
        
        Assert.False(result);
    }
    
    #endregion

    
    
}