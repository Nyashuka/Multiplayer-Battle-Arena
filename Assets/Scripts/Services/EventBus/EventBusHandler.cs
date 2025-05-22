using Services.EventBus.EventBusArguments;

namespace Services.EventBus
{
    public delegate void EventBusHandler(IEventBusArgs e);
}