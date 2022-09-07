using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces;
using Application.Core.Interfaces.Repositories;
using Infrastructure.Cache;
using Infrastructure.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using Moq;
using Xunit;

namespace UnitTests.Infrastructure.Identity;

public class OAuth2IdentityTests
{
    private readonly Mock<ICache> _cache;
    private readonly Mock<ISessionRepository> _sessionRepository;
    private readonly Mock<IDancerRepository> _dancerRepository;
    private readonly Mock<ILogger> _logger;
    private readonly Mock<HttpMessageHandler> _messageHandler;
    
    private readonly HttpClient _client;
    private readonly OAuth2IdentityConfig _config;

    private readonly OAuth2Identity _oauth2;

    private readonly JwtSecurityTokenHandler _tokenHandler = new();

    public OAuth2IdentityTests()
    {
        _cache = new Mock<ICache>();
        _sessionRepository = new Mock<ISessionRepository>();
        _dancerRepository = new Mock<IDancerRepository>();
        _logger = new Mock<ILogger>();
        _messageHandler = new Mock<HttpMessageHandler>();

        _client = new HttpClient(_messageHandler.Object);
        _config = new OAuth2IdentityConfig()
        {

        };

        _oauth2 = new OAuth2Identity(
            _cache.Object,
            _config,
            _client,
            _sessionRepository.Object,
            _dancerRepository.Object,
            _logger.Object
        );
    }

    #region IsAdmin

    [Fact(DisplayName = "When jwt contains cognito:groups claim including value administrators, returns true")]
    public void JwtContainingGroupsAndAdministrators()
    {
        var jwtWithAdministrators =
            "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJteS1pc3MiLCJpYXQiOjE1Nzc4MzY4MDAsImV4cCI6NDEwMjQ0NDgwMCwiYXVkIjoid3d3Lm15LWF1ZC5jb20iLCJzdWIiOiJzdWIiLCJjb2duaXRvOmdyb3VwcyI6WyJhZG1pbmlzdHJhdG9ycyIsImdyb3VwMiJdfQ.4LD_dq4ePCMBdMylRdneDx7Kp6xkWLwmF3yBtB17Zmw";
        _cache.Setup(c =>
            c.Fetch<string>(It.IsAny<string>())
        ).Returns(jwtWithAdministrators);
        
        var result = _oauth2.IsAdmin(jwtWithAdministrators);

        Assert.True(result);
    }
    
    [Fact(DisplayName = "When jwt contains cognito:groups claim but does not contain administrators, returns false")]
    public void JwtContainingGroupsButNoAdministrators()
    {
        var jwtWithoutAdministrators =
            "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJteS1pc3MiLCJpYXQiOjE1Nzc4MzY4MDAsImV4cCI6NDEwMjQ0NDgwMCwiYXVkIjoid3d3Lm15LWF1ZC5jb20iLCJzdWIiOiJzdWIiLCJjb2duaXRvOmdyb3VwcyI6WyJncm91cDEiLCJncm91cDIiXX0.rD9rWpxZ-CYddcmMDgHKKTRK6IPHgkP5r_heplBKeAs";
        _cache.Setup(c =>
            c.Fetch<string>(It.IsAny<string>())
        ).Returns(jwtWithoutAdministrators);
        
        var result = _oauth2.IsAdmin(jwtWithoutAdministrators);

        Assert.False(result);
    }
    
    [Fact(DisplayName = "When jwt does not contain cognito:groups claim, returns false")]
    public void JwtWithoutGroupsClaim()
    {
        var jwtWithoutGroupsClaim =
            "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJteS1pc3MiLCJpYXQiOjE1Nzc4MzY4MDAsImV4cCI6NDEwMjQ0NDgwMCwiYXVkIjoid3d3Lm15LWF1ZC5jb20iLCJzdWIiOiJzdWIifQ.dSm_NuJx9eH6wosTPU5CNK5EV7o0LiWLZ2tYpaC9IHA";
        _cache.Setup(c =>
            c.Fetch<string>(It.IsAny<string>())
        ).Returns(jwtWithoutGroupsClaim);
        
        var result = _oauth2.IsAdmin(jwtWithoutGroupsClaim);

        Assert.False(result);
    }
    
    #endregion

    #region IsSessionActive

    [Fact(DisplayName = "When cache has token, and expiration is later than current time, return true")]
    public void IsSessionActive_TokenExists_ExpirationLater_True()
    {
        var token = _tokenHandler.WriteToken(new JwtSecurityToken(expires: DateTime.Now.AddDays(1)));

        _cache.Setup(c =>
            c.Fetch<string>(It.IsAny<string>())
        ).Returns(token);

        var result = _oauth2.IsSessionActive("session-cookie");
        
        Assert.True(result);
        
        _cache.Verify(c =>
                c.Fetch<string>(It.Is<string>(value => value.Equals("session-cookie"))),
            Times.Once
        );
    }

    [Fact(DisplayName = "When cache has token, and expiration has passed, return false")]
    public void IsSessionActive_TokenExists_ExpirationPassed_False()
    {
        var token = _tokenHandler.WriteToken(new JwtSecurityToken(expires: DateTime.Now.AddDays(-1)));

        _cache.Setup(c =>
            c.Fetch<string>(It.IsAny<string>())
        ).Returns(token);

        var result = _oauth2.IsSessionActive("session-cookie");
        
        Assert.False(result);
        
        _cache.Verify(c =>
                c.Fetch<string>(It.Is<string>(value => value.Equals("session-cookie"))),
            Times.Once
        );
    }

    [Fact(DisplayName = "When cache has no token, return false")]
    public void IsSessionActive_NoTokenInCache_ReturnFalse()
    {
        _cache.Setup(c =>
            c.Fetch<string>(It.IsAny<string>())
        ).Returns(null as string);

        var result = _oauth2.IsSessionActive("session-cookie");
        
        Assert.False(result);
    }

    [Fact(DisplayName = "When cache has token, token does not have exp claim, return false")]
    public void IsSessionActive_TokenExists_NoExpClaim_False()
    {
        var token = _tokenHandler.WriteToken(new JwtSecurityToken());

        _cache.Setup(c =>
            c.Fetch<string>(It.IsAny<string>())
        ).Returns(token);

        var result = _oauth2.IsSessionActive("session-cookie");
        
        Assert.False(result);
        
        _cache.Verify(c =>
                c.Fetch<string>(It.Is<string>(value => value.Equals("session-cookie"))),
            Times.Once
        );
    }

    #endregion

    #region CreateSession

    

    #endregion

    #region RefreshSession

    

    #endregion
}
