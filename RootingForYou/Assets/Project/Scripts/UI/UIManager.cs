using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;


public class UIManager : MonoBehaviour
{
    private int score;
    [SerializeField] private float timer;
    private int multiplier;

    public TMP_Text gameOverScoreText;
    public TMP_Text hudScoreText;
    public TMP_Text timerText;

    public Slider slider;

    [Header("Parameters")]
    [SerializeField] private PlayerInput m_playerInput = null;
    [SerializeField] private string m_navigateActionName = null;
    [SerializeField] private string m_startActionName = null;
    public GameObject pauseMenu, optionsMenu, gameOverMenu, twoDimensionalUI, HUD;
    [SerializeField] List<GameObject> pauseMenuButtons, optionsMenuButtons, gameOverMenuButtons;
    public int pauseMenuButtonIndex, optionsMenuButtonIndex, gameOverMenuButtonIndex = 0;
    private int menuIndex = 0;
    private bool pausePressed = false;
    private bool pauseUpdatedThisFrame = false;

    Vector2 NavigationInput;
    bool didReset = false;

    public int getScore() { return score; }
    public float getTimer() { return timer; }
    public int getMultiplier() { return multiplier; }

    private void Start()
    {
        initMuliptlier();
        score = 0;
        timer = 60;
    }

    private void Update()
    {
        NavigationInput = m_playerInput.actions[m_navigateActionName].ReadValue<Vector2>();
        if (m_playerInput.actions[m_startActionName].WasPressedThisFrame() && !pausePressed
            && !pauseUpdatedThisFrame)
        {
            pausePressed = true;
            pauseUpdatedThisFrame = true;
            LoadPauseMenu();
        }
        if(m_playerInput.actions[m_startActionName].WasPressedThisFrame() && pausePressed
            && !pauseUpdatedThisFrame)
        {
            pausePressed = false;
            pauseUpdatedThisFrame = true;
            UnloadPauseMenu();
        }

        NavigationInput = m_playerInput.actions[m_navigateActionName].ReadValue<Vector2>();
        if (NavigationInput.magnitude == 0)
        {
            didReset = true;
            return;
        }
        if (!didReset)
            return;
        didReset = false;
        if(twoDimensionalUI.activeInHierarchy)
            NavigateMenu(NavigationInput.y);

        updateMultiplier();
        updateScoreText();
        updateTimer();
        pauseUpdatedThisFrame = false;
    }

    public void NavigateMenu(float pNavigationMagnitude)
    {
        if (menuIndex == 0)
        {
            EventSystem.current.SetSelectedGameObject(null);
            if (pNavigationMagnitude < 0)
                pauseMenuButtonIndex++;
            else
                pauseMenuButtonIndex--;
            if (pauseMenuButtonIndex < 0)
                pauseMenuButtonIndex = 0;
            if (pauseMenuButtonIndex > pauseMenuButtons.Count - 1)
                pauseMenuButtonIndex = pauseMenuButtons.Count - 1;
            EventSystem.current.SetSelectedGameObject(pauseMenuButtons[pauseMenuButtonIndex]);
        }
        if (menuIndex == 1)
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
            EventSystem.current.SetSelectedGameObject(optionsMenuButtons[optionsMenuButtonIndex]);
        }
        if (menuIndex == 2)
        {
            EventSystem.current.SetSelectedGameObject(null);
            if (pNavigationMagnitude < 0)
                gameOverMenuButtonIndex++;
            else
                gameOverMenuButtonIndex--;
            if (gameOverMenuButtonIndex < 0)
                gameOverMenuButtonIndex = 0;
            if (gameOverMenuButtonIndex > gameOverMenuButtons.Count - 1)
                gameOverMenuButtonIndex = gameOverMenuButtons.Count - 1;
            EventSystem.current.SetSelectedGameObject(gameOverMenuButtons[gameOverMenuButtonIndex]);
        }
    }

    public void LoadOptionsMenu()
    {
        pauseMenuButtonIndex = 0;
        gameOverMenuButtonIndex = 0;
        menuIndex = 1;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsMenuButtons[optionsMenuButtonIndex]);
    }

    public void LoadPauseMenuFromOtherMenu()
    {
        optionsMenuButtonIndex = 0;
        gameOverMenuButtonIndex = 0;
        menuIndex = 0;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseMenuButtons[pauseMenuButtonIndex]);

    }

    public void LoadCreditsMenu()
    {
        pauseMenuButtonIndex = 0;
        optionsMenuButtonIndex = 0;
        menuIndex = 2;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(gameOverMenuButtons[gameOverMenuButtonIndex]);
    }

    private void LoadPauseMenu()
    {
        HUD.SetActive(false);
        twoDimensionalUI.SetActive(true);
        pauseMenu.SetActive(true);
    }

    public void UnloadPauseMenu()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        twoDimensionalUI.SetActive(false);
        HUD.SetActive(true);
    }

    public void setScore(int pScore)
    {
        score = pScore;
        updateScoreText();
    }

    public void setTimer(int pTimer)
    {
        timer = pTimer;
    }

    public void setMultiplier(int pMultiplier)
    {
        multiplier = pMultiplier;
    }

    public void updateScoreText()
    {
        gameOverScoreText.text = score.ToString();
        hudScoreText.text = score.ToString();
    }

    public void initMuliptlier()
    {
        slider.maxValue = 10;
        slider.value = 0;
    }

    public void updateMultiplier()
    {
        slider.value = multiplier;
    }

    public void updateTimer()
    {
        timerText.text = timer.ToString();
    }
}
