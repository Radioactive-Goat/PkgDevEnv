using RG.Events;
using UnityEngine;



public class EventSystemTest : MonoBehaviour
{
    public void RegAlpha()
    {
        EventSystem.Instance.Register<EventAlpha>();
        EventSystem.Instance.GetEvent<EventAlpha>().AddCallback(EACB);
    }

    private void EACB(AlphaArgs args)
    {
        Debug.Log($"{args.oldValue} -> {args.newValue}");
    }

    public void InvokeAlpha()
    {
        EventSystem.Instance.Invoke<EventAlpha, AlphaArgs>(new AlphaArgs {});
    }

    public void RegBeta()
    {
        EventSystem.Instance.Register<EventBeta>();
        EventSystem.Instance.GetEvent<EventBeta>().AddCallback((BetaArgs args) => { Debug.Log(args); });
    }

    public void InvokeBeta() 
    {
        EventSystem.Instance.GetEvent<EventBeta>().Invoke(new BetaArgs());
    }
}
public class AlphaArgs : IEventArgs
{
    public float oldValue = 2.51f;
    public float newValue = 3.14f;
}
public class EventAlpha : Event<AlphaArgs> { }

public class BetaArgs : IEventArgs
{
    public string name = "Joe";
    public int age = 32;
    public float height = 188f;
    public float weight = 90f;
}
public class EventBeta : Event<BetaArgs> { }


