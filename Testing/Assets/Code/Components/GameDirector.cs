using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    [SerializeField] GameObject m_world;
    [SerializeField] UIDirector m_ui;

    enum PausedState
    {
        Active,
        Paused
    }

    PausedState m_pausedState = PausedState.Active;

    private void Awake()
    {
        Debug.Assert(m_world != null, "World is not assigned in GameDirector");
        Debug.Assert(m_ui != null, "UI Director is not assigned in GameDirector");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UserRequestedPause()
    {
        switch(m_pausedState)
        {
            case PausedState.Active:
                m_world.SetActive(false);
                m_ui.ActivatePause();
                m_pausedState = PausedState.Paused;
                break;

            case PausedState.Paused:
                m_world.SetActive(true);
                m_ui.DeactivatePause();
                m_pausedState = PausedState.Active;
                break;
        }
    }
}
