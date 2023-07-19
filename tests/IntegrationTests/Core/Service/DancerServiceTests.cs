using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces;
using Application.Core.Models;
using Application.Core.Models.Dancer;
using Application.Core.Services;
using Infrastructure.Data;
using IntegrationTests.Helpers.DataGenerators;
using Moq;
using Xunit;

namespace IntegrationTests.Core.Service;

[Collection("Dancer service collection")]
public class DancerServiceTests
{
    private readonly PostgresDatabaseFixture _fixture;
    private readonly DancerService _dancerService;

    private readonly Mock<IFileStorage> _fileStorage = new Mock<IFileStorage>();

    public DancerServiceTests(PostgresDatabaseFixture fixture)
    {
        _fixture = fixture;
        var repository = new DancerRepository(_fixture._context);
        _dancerService = new DancerService(repository, _fileStorage.Object);
    }

    private void AddDancerToTable(Dancer d)
    {
        _fixture._context.Dancers.Add(d);
        _fixture._context.SaveChanges();
        _fixture._context.ChangeTracker.Clear();
    }

    #region MigrateDancer

    [Fact(DisplayName = "Migrates dancer record")]
    public async Task MigrateDancer_MigratesDancerRecord()
    {
        var dancer = DancerGenerator.CreateDancer("DancerName");
        var model = new MigrateDancerRequestModel
        {
            AuthId = Guid.NewGuid().ToString(),
            LegacyId = dancer.AuthenticationId
        };
        AddDancerToTable(dancer);

        var result = await _dancerService.MigrateDancer(model, CancellationToken.None);
        
        Assert.Equal(ResultCode.Ok, result.ResultCode);
        Assert.Equal(model.AuthId, result.Value.Value.AuthenticationId);
        Assert.Equal(dancer.DdrName, result.Value.Value.DdrName);
    }

    #endregion

    #region UpdateDancerAsync

    [Fact(DisplayName = "Updates dancer record")]
    public async Task UpdateDancerAsync_UpdatesDancerRecord()
    {
        var dancer = DancerGenerator.CreateDancer("DancerName");
        AddDancerToTable(dancer);
        var model = new UpdateDancerRequestModel
        {
            AuthId = dancer.AuthenticationId,
            DdrCode = "456",
            DdrName = "Def",
            PrimaryMachineLocation = "Def456",
            State = "Wvu"
        };

        var result = await _dancerService.UpdateDancerAsync(model, CancellationToken.None);
        
        Assert.Equal(ResultCode.Ok, result.ResultCode);
        Assert.True(
            result.Value.Value.State == model.State &&
            result.Value.Value.DdrCode == model.DdrCode &&
            result.Value.Value.DdrName == model.DdrName &&
            result.Value.Value.PrimaryMachineLocation == model.PrimaryMachineLocation &&
            result.Value.Value.AuthenticationId == model.AuthId &&
            result.Value.Value.Id == dancer.Id
        );
    }

    #endregion

    #region CreateDancerAsync

    [Fact(DisplayName = "Creates dancer record")]
    public async Task CreateDancerAsync_CreatesDancerRecord()
    {
        var model = new CreateDancerRequestModel
        {
            AuthId = Guid.NewGuid().ToString(),
            DdrCode = "123",
            DdrName = "abc",
            PrimaryMachineLocation = "Def456",
            State = "Wvu"
        };

        var result = await _dancerService.CreateDancerAsync(model, CancellationToken.None);
        
        Assert.Equal(ResultCode.Ok, result.ResultCode);
        Assert.True(
            result.Value.Value.State == model.State &&
            result.Value.Value.DdrCode == model.DdrCode &&
            result.Value.Value.DdrName == model.DdrName &&
            result.Value.Value.PrimaryMachineLocation == model.PrimaryMachineLocation &&
            result.Value.Value.AuthenticationId == model.AuthId
        );
    }

    #endregion
}