using System;

public static class GameEventsHandler
{
    public static event Action ExampleEvent;
    public static void ExampleEventHandler()
    {
        ExampleEvent?.Invoke();
    }
}
