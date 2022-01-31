using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ChestVisual : MonoBehaviour
{
    Animator m_animator;
    [SerializeField] GameObject m_openFX;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    public void OnAction()
    {
        m_animator.SetTrigger("OnOpened");
    }

    public void OnOpenStarted()
    {
        Instantiate(m_openFX, transform);
    }
}
