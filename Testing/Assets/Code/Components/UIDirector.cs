using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDirector : MonoBehaviour
{
    [SerializeField] GameObject m_blurWidget;
    [SerializeField] GameObject m_pauseMenu;

    private int m_numCircles = 0;

    private void Awake()
    {
        Debug.Assert(m_blurWidget != null, "Blur widget not set in UIDirector");
        Debug.Assert(m_pauseMenu != null, "Pause menu not set in UIDirector");
    }

    // Start is called before the first frame update
    void Start()
    {
        m_blurWidget.gameObject.SetActive(false);
        m_pauseMenu.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivatePause()
    {
        m_blurWidget.gameObject.SetActive(true);
        m_pauseMenu.gameObject.SetActive(true);
    }

    public void DeactivatePause()
    {
        m_blurWidget.gameObject.SetActive(false);
        m_pauseMenu.gameObject.SetActive(false);
    }

    public void OnNumCirclesChanged(int i_numCircles)
    {
        m_numCircles = i_numCircles;
    }

    public int GetNumCircles()
    {
        return m_numCircles;
    }
}
