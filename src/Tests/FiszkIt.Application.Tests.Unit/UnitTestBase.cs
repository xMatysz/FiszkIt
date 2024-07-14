using FiszkIt.Application.Repository;
using NSubstitute;

namespace FiszkIt.Application.Tests.Unit;

public abstract class UnitTestBase
{
    protected IFlashSetDtoRepository FlashSetDtoRepository { get; set; } = Substitute.For<IFlashSetDtoRepository>();
}