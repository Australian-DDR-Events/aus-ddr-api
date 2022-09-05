using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces.ExternalServices;
using Application.Core.Interfaces.Repositories;
using Application.Core.Interfaces.Services;
using Application.Core.Models.Connections;

namespace Application.Core.Services.Connections;

public class DiscordConnection : IConnectionService<DiscordConnectionRequestModel>
{
    private readonly IDancerRepository _dancerRepository;
    private readonly IConnectionRepository _connectionRepository;
    private readonly IDiscordApiService _discordApiService;

    public DiscordConnection(IDancerRepository dancerRepository, IConnectionRepository connectionRepository, IDiscordApiService discordApiService)
    {
        _dancerRepository = dancerRepository;
        _connectionRepository = connectionRepository;
        _discordApiService = discordApiService;
    }
    
    public async Task<bool> CreateConnection(string authId, DiscordConnectionRequestModel connectionData, CancellationToken cancellationToken)
    {
        var dancer = _dancerRepository.GetDancerByAuthId(authId);
        if (dancer == null) return false;
        var existingConnection =
            _connectionRepository.GetConnection(dancer.Id, Connection.ConnectionType.DISCORD);
        if (existingConnection.Any()) return false;
        var connectionToken = await _discordApiService.AuthCodeExchange(connectionData.Code);
        if (!connectionToken.Any()) return false;
        var userData = await _discordApiService.GetAuthorizationInfo(connectionToken, cancellationToken);
        if (userData?.User == null) return false;
        var connection = new Connection
        {
            Id = Guid.NewGuid(),
            DancerId = dancer.Id,
            ConnectionData = JsonSerializer.Serialize(userData.User),
            Type = Connection.ConnectionType.DISCORD
        };
        await _connectionRepository.CreateConnection(connection, cancellationToken);
        return true;
    }
}