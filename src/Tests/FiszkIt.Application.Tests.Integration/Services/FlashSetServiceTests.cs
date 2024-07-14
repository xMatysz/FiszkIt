using FiszkIt.Application.Repository.Dtos;
using FiszkIt.Application.Services;
using FiszkIt.Domain;
using FiszkIt.Shared.Tests.Builders;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace FiszkIt.Application.Tests.Integration.Services;

public class FlashSetServiceTests : IntegrationTestsBase
{
    private readonly IFlashSetService _sut;

    public FlashSetServiceTests(WebAppFactory factory)
        : base(factory)
    {
        _sut = Services.GetRequiredService<IFlashSetService>();
    }

    [Fact]
    public async Task GetAll_Return_AllUserFlashSetDtos()
    {
        FlashSet[] otherFlashSets =
        [
            new FlashSetBuilder().Build(),
            new FlashSetBuilder().Build(),
            new FlashSetBuilder().Build(),
        ];

        await AssumeEntityInDbAsync(otherFlashSets);

        var userId = Guid.NewGuid();
        FlashSet[] userFlashSets =
        [
            new FlashSetBuilder().WithCreatorId(userId).Build(),
            new FlashSetBuilder().WithCreatorId(userId).Build(),
            new FlashSetBuilder().WithCreatorId(userId).Build(),
        ];

        await AssumeEntityInDbAsync(userFlashSets);
        var expectedFlashSetDtos = userFlashSets.Select(x => new FlashSetDto(x)).ToArray();

        var results = await _sut.GetAllAsync(userId, CancellationToken.None);

        results.Should().HaveCount(expectedFlashSetDtos.Length);
        results.Should().BeEquivalentTo(expectedFlashSetDtos);
    }
}