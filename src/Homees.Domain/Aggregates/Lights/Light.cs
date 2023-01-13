using Homees.Domain.Aggregates.Common;
using Homees.Domain.Aggregates.Lights.Events;
using Homees.Domain.Common.Aggregates;
using Homees.Domain.Common.Events;

namespace Homees.Domain.Aggregates.Lights;

public class Light : Aggregate
{
    private Light()
    {
    }
    
    public Light(Guid id, string name)
    {
        var domainEvent = LightCreated.Create(id, name);

        Apply(domainEvent);
    }

    public DeviceConnectionStatus ConnectionStatus { get; private set; }

    public bool IsOn { get; private set; }

    public string Name { get; private set; }

    public byte DimmerValue { get; private set; }

    public override void When(object domainEvent)
    {
        switch (domainEvent)
        {
            case LightCreated e:
                OnLightCreated(e);
                break;
            case LightConnected e:
                OnLightConnected(e);
                break;
            case LightDisconnected e:
                OnLightDisconnected(e);
                break;
            case LightTurnedOn e:
                OnLightTurnedOn(e);
                break;
            case LightTurnedOff e:
                OnLightTurnedOff(e);
                break;
            case LightDimmerValueUpdated e:
                OnLightDimmerValueUpdated(e);
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
    
    private void OnLightCreated(LightCreated domainEvent)
    {
        Version = 0;
        
        Id = domainEvent.Id;
        Name = domainEvent.Name;
    }
    
    private void OnLightTurnedOn(LightTurnedOn domainEvent)
    {
        Version++;
        
        IsOn = true;
    }
    
    private void OnLightTurnedOff(LightTurnedOff domainEvent)
    {
        Version++;
        
        IsOn = false;
    }
    
    private void OnLightDisconnected(LightDisconnected domainEvent)
    {
        Version++;
        
        ConnectionStatus = DeviceConnectionStatus.Disconnected;
    }

    private void OnLightConnected(LightConnected domainEvent)
    {
        Version++;
        
        ConnectionStatus = DeviceConnectionStatus.Connected;
    }

    private void OnLightDimmerValueUpdated(LightDimmerValueUpdated domainEvent)
    {
        Version++;
        
        DimmerValue = domainEvent.Value;
        IsOn = true;
    }
}