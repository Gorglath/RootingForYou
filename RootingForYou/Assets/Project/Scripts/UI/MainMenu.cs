using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu, optionsMenu, creditsMenu;
    public GameObject mainMenuFirstButton, optionsMenuFirstButton, creditsMenuFirstButton;
    public List<GameObject> menuOptions;
    public int menuIndex = 0;
    public List<GameObject> mainMenuButtons, optionsMenuButtons, creditsMenuButtons;
    public int mainMenuButtonIndex, optionsMenuButtonIndex, creditsMenuButtonIndex = 0;

    Vector2 NavigationInput;
    bool didReset = false;

    [Header("Parameters")]
    [SerializeField] private PlayerInput m_playerInput = null;
    [SerializeField] private string m_navigateActionName = null;
    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(mainMenuButtons[0]);
        menuIndex = 0;
    }

    private void Update()
    {
        NavigationInput = m_playerInput.actions[m_navigateActionName].ReadValue<Vector2>();
        if (NavigationInput.magnitude == 0)
        {
            didReset = true;
            return;
        }
        if (!didReset)
            return;
        didReset = false;
        NavigateMenu(NavigationInput.y);

    }

    public void NavigateMenu(float pNavigationMagnitude)
    {
        if(menuIndex == 0)
        {
            EventSystem.current.SetSelectedGameObject(null);
            if (pNavigationMagnitude < 0)
                mainMenuButtonIndex++;
            else
                mainMenuButtonIndex--;
            if (mainMenuButtonIndex < 0)
                mainMenuButtonIndex = 0;
            if (mainMenuButtonIndex > mainMenuButtons.Count - 1)
                mainMenuButtonIndex = mainMenuButtons.Count - 1;
            EventSystem.current.SetSelectedGameObject(mainMenuButtons[mainMenuButtonIndex]);
        }
        if(menuIndex == 1)
        {
            EventSystem.current.SetSelectedGameObject(null);
            if (pNavigationMagnitude < 0)
                optionsMenuButtonIndex++;
            else
                optionsMenuButtonIndex--;
            if (optionsMenuButtonIndex < 0)
                optionsMenuButtonIndex = 0;
            if (optionsMenuButtonIndex > optionsMenuButtons.Count - 1)
                optionsMenuButtonIndex = optionsMenuButtons.Count - 1;
            Debug.Log("Options menu button index: " + optionsMenuButtonIndex);
            EventSystem.current.SetSelectedGameObject(optionsMenuButtons[optionsMenuButtonIndex]);
        }
        if(menuIndex == 2)
        {
            EventSystem.current.SetSelectedGameObject(null);
            if (pNavigationMagnitude < 0)
                creditsMenuButtonIndex++;
            else
                creditsMenuButtonIndex--;
            if (creditsMenuButtonIndex < 0)
                creditsMenuButtonIndex = 0;
            if (creditsMenuButtonIndex > creditsMenuButtons.Count - 1)
                creditsMenuButtonIndex = creditsMenuButtons.Count - 1;
            EventSystem.current.SetSelectedGameObject(creditsMenuButtons[creditsMenuButtonIndex]);
        }
    }

    public void LoadOptionsMenu()
    {
        mainMenuButtonIndex = 0;
        creditsMenuButtonIndex = 0;
        menuIndex = 1;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsMenuButtons[optionsMenuButtonIndex]);
    }

    public void LoadMainMenu()
    {
        optionsMenuButtonIndex = 0;
        creditsMenuButtonIndex = 0;
        menuIndex = 0;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(mainMenuButtons[mainMenuButtonIndex]);

    }

    public void LoadCreditsMenu()
    {
        mainMenuButtonIndex = 0;
        optionsMenuButtonIndex = 0;
        menuIndex = 2;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(creditsMenuButtons[creditsMenuButtonIndex]);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
