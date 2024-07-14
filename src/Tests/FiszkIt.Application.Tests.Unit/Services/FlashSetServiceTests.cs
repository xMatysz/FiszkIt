using FiszkIt.Application.Repository.Dtos;
using FiszkIt.Application.Services;
using FiszkIt.Domain;
using FiszkIt.Shared.Tests.Builders;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace FiszkIt.Application.Tests.Unit.Services;

public class FlashSetServiceTests : UnitTestBase
{
    private readonly IFlashSetService _sut;

    public FlashSetServiceTests()
    {
        _sut = new FlashSetService(FlashSetDtoRepository);
    }

    [Fact]
    public async Task GetById_Return_ErrorWhenFlashSetNotExist()
    {
        var result = await _sut.GetByIdAsync(Guid.NewGuid(), Guid.NewGuid(), CancellationToken.None);

        result.IsError.Should().BeTrue();
        result.Errors.Single().Should().Be(FlashSetErrors.NotFound);
    }

    [Fact]
    public async Task GetById_Return_FlashSetDto()
    {
        var userId = Guid.NewGuid();
        var dto = new FlashSetDto(new FlashSetBuilder().WithCreatorId(userId).Build());
        FlashSetDtoRepository
            .GetByIdForUserAsync(Arg.Is(dto.Id), Arg.Is(userId), Arg.Any<CancellationToken>())
            .Returns(dto);

        var result = await _sut.GetByIdAsync(dto.Id, userId, CancellationToken.None);

        result.IsError.Should().BeFalse();
        result.Value.Should().Be(dto);
    }
}