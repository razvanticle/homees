using FluentAssertions;
using Homees.Domain.Aggregates.Common;
using Homees.Domain.Aggregates.Lights;
using NUnit.Framework;

namespace Homees.Domain.UnitTests.Aggregates.Lights;


public class LightTests
{
    [Test]
    public void WhenCreated_ApplyName()
    {
        // arrange
        const string name = "Office Light";
        var id = Guid.NewGuid();
        
        // act
        var sut = CreateSut(id, name);
        
        // assert
        sut.Id.Should().Be(id);
        sut.Name.Should().Be(name);
        sut.IsOn.Should().Be(false);
        sut.DimmerValue.Should().Be(0);
    }

    [Test]
    public void TurnOn_WhenStatusNotConnected_ThrowException()
    {
        // arrange
        var sut = CreateSut();

        // act
        var act = () =>  sut.TurnOn();
        
        // assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Test]
    public void TurnOn_WhenStatusConnected_TurnOnLight()
    {
        // arrange
        var sut = CreateSut();
        sut.Connect();

        // act
        sut.TurnOn();
        
        // assert
        sut.IsOn.Should().Be(true);
    }
    
    
    [Test]
    public void TurnOff_WhenStatusNotConnected_ThrowException()
    {
        // arrange
        var sut = CreateSut();

        // act
        var act = () =>  sut.TurnOff();
        
        // assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Test]
    public void TurnOff_WhenStatusConnected_TurnOffLight()
    {
        // arrange
        var sut = CreateSut();
        sut.Connect();
        sut.TurnOn();

        // act
        sut.TurnOff();
        
        // assert
        sut.IsOn.Should().Be(false);
    }

    [Test]
    public void Connect_WhenCalled_ShouldConnectLight()
    {
        // arrange
        var sut = CreateSut();

        // act
        sut.Connect();
        
        // assert
        sut.ConnectionStatus.Should().Be(DeviceConnectionStatus.Connected);
    }
    
    [Test]
    public void Disconnect_WhenCalled_ShouldDisconnectLight()
    {
        // arrange
        var sut = CreateSut();
        sut.Connect();

        // act
        sut.Disconnect();
        
        // assert
        sut.ConnectionStatus.Should().Be(DeviceConnectionStatus.Disconnected);
    }
    
    [Test]
    public void SetDimmerValue_WhenStatusNotConnected_ThrowException()
    {
        // arrange
        var sut = CreateSut();

        // act
        var act = () =>  sut.SetDimmerValue(24);
        
        // assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Test]
    public void SetDimmerValue_WhenStatusConnected_UpdateValue()
    {
        // arrange
        var sut = CreateSut();
        sut.Connect();
        const byte expected = 23;
        
        // act
        sut.SetDimmerValue(expected);
        
        // assert
        sut.DimmerValue.Should().Be(expected);
    }
    
    [TestCase(0)]
    [TestCase(101)]
    public void SetDimmerValue_ValueOutOfRange_ThrowException(byte value)
    {
        // arrange
        var sut = CreateSut();
        sut.Connect();
        
        // act
        var act = () =>  sut.SetDimmerValue(value);
        
        // assert
        act.Should().Throw<InvalidOperationException>();
    }

    private Light CreateSut(Guid? id = null, string name = "Office Light")
    {
        return new Light(id ?? Guid.NewGuid(), name);
    }
}