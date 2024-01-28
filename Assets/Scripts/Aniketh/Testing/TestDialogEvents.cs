using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RG.Testing
{
    public class TestDialogEvents : MonoBehaviour
    {
        public void EventForClosingFromResponse()
        {
            Debug.Log("Closing From response");
        }

        public void MovingToResponse()
        {
            Debug.Log("Moving to response");
        }

        public void SomethingEvent1()
        {
            Debug.Log("Something 1");
        }

        public void SomethingEvent2()
        {
            Debug.Log("Something 2");
        }
    }
}
