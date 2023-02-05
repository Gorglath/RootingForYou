using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class WinLoseScreen : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private PlayerInput m_playerInput = null;
    [SerializeField] private string m_navigateActionName = null;
    [SerializeField] private string m_lockActionName = null;

    private void Update()
    {
        if(m_playerInput.actions[m_navigateActionName].ReadValue<Vector2>().magnitude!= 0)
        {
            Debug.Log("Navigating");
        }
    }

    public void ReplayLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
