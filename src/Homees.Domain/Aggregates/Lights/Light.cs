using Homees.Domain.Aggregates.Common;
using Homees.Domain.Aggregates.Lights.Events;
using Homees.Domain.Common.Aggregates;
using Homees.Domain.Common.Events;

namespace Homees.Domain.Aggregates.Lights;

public class Light : Aggregate
{
    public Light(Guid id, string name)
    {
        var domainEvent = LightCreated.Create(id, name);

        Apply(domainEvent);
    }

    public DeviceConnectionStatus ConnectionStatus { get; private set; }

    public bool IsOn { get; private set; }

    public string Name { get; private set; }

    public byte DimmerValue { get; private set; }

    public override void When(DomainEventBase domainEvent)
    {
        switch (domainEvent)
        {
            case LightCreated e:
                Handle(e);
                break;
            case LightConnected e:
                Handle(e);
                break;
            case LightDisconnected e:
                Handle(e);
                break;
            case LightTurnedOn e:
                Handle(e);
                break;
            case LightTurnedOff e:
                Handle(e);
                break;
            case LightDimmerValueUpdated e:
                Handle(e);
                break;
        }
    }

    public void Connect()
    {
        var domainEvent = LightConnected.Create(Id);

        Apply(domainEvent);
    }
    
    public void Disconnect()
    {
        var domainEvent = LightDisconnected.Create(Id);

        Apply(domainEvent);
    }
    
    public void TurnOn()
    {
        if (ConnectionStatus == DeviceConnectionStatus.Disconnected)
        {
            throw new InvalidOperationException($"Light {Id} is not connected.");
        }
        
        var domainEvent = LightTurnedOn.Create(Id);
        Apply(domainEvent);
    }
    
    public void TurnOff()
    {
        if (ConnectionStatus == DeviceConnectionStatus.Disconnected)
        {
            throw new InvalidOperationException($"Light {Id} is not connected.");
        }
        
        var domainEvent = LightTurnedOff.Create(Id);
        Apply(domainEvent);
    }
    
    
    public void SetDimmerValue(byte value)
    {
        if (ConnectionStatus == DeviceConnectionStatus.Disconnected)
        {
            throw new InvalidOperationException($"Light {Id} is not connected.");
        }

        if (value is < 1 or > 100)
        {
            throw new InvalidOperationException(
                $"The new dimmer value for light {Id} is {value}, but it mut be between 1 and 100.");
        }

        var domainEvent = LightDimmerValueUpdated.Create(Id, value);
        Apply(domainEvent);
    }
    
    private void Handle(LightCreated domainEvent)
    {
        Id = domainEvent.Id;
        Name = domainEvent.Name;
    }
    
    private void Handle(LightTurnedOn domainEvent)
    {
        IsOn = true;
    }
    
    private void Handle(LightTurnedOff domainEvent)
    {
        IsOn = false;
    }
    
    private void Handle(LightDisconnected domainEvent)
    {
        ConnectionStatus = DeviceConnectionStatus.Disconnected;
    }

    private void Handle(LightConnected domainEvent)
    {
        ConnectionStatus = DeviceConnectionStatus.Connected;
    }

    private void Handle(LightDimmerValueUpdated domainEvent)
    {
        DimmerValue = domainEvent.Value;
    }
}