using RG.Events;
using UnityEngine;



public class EventSystemTest : MonoBehaviour
{
    public void RegAlpha()
    {
        EventSystem.Instance.Register<EventAlpha>();
        EventSystem.Instance.Subscribe<EventAlpha>(EACB);
    }

    private void EACB(IEvent args)
    {
        var ev = args as EventAlpha;
        Debug.Log($"{ev.oldValue} -> {ev.newValue}");
    }

    public void InvokeAlpha()
    {
        EventSystem.Instance.Invoke<EventAlpha>(new EventAlpha { });
    }

    public void RegBeta()
    {
        EventSystem.Instance.Register<EventBeta>();
        EventSystem.Instance.Subscribe<EventBeta>((IEvent beta) => { Debug.Log(beta as EventBeta); });
    }

    public void InvokeBeta()
    {
        EventSystem.Instance.Invoke<EventBeta>(new EventBeta());
    }
}

public class EventAlpha : IEvent
{
    public float oldValue = 2.51f;
    public float newValue = 3.14f;
}

public class EventBeta : IEvent
{
    public string name = "Joe";
    public int age = 32;
    public float height = 188f;
    public float weight = 90f;
}


