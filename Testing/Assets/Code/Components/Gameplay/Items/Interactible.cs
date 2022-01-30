using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactible : MonoBehaviour
{
    [System.Serializable] class OnInteractedEvent: UnityEvent {}
    [SerializeField] OnInteractedEvent sig_onInteracted;

    private void Awake()
    {
        if (sig_onInteracted == null)
        {
            sig_onInteracted = new OnInteractedEvent();
        }
    }

    public void OnInteracted()
    {
        sig_onInteracted.Invoke();
    }
}
