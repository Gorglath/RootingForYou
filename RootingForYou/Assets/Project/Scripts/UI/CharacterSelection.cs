using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CharacterSelection : MonoBehaviour
{
    public Image firstCharacterImage;
    public Image firstCharacterAccessory;
    int firstCharacterSpriteIndex;
    int firstCharacterAccessoryIndex;
    public TMP_Text firstCharacterName;

    public Image secondCharacterImage;
    public Image secondCharacterAccessory;
    int secondCharacterSpriteIndex;
    int secondCharacterAccessoryIndex;
    public TMP_Text secondCharacterName;

    public List<Sprite> CharacterSprites;
    public List<Sprite> CosmeticSprites;
    public List<Sprite> CharacterNames;

    public List<GameObject> CharacterSelectionScreenButtons;
    private int CharacterSelectionButtonIndex;

    Vector2 NavigationInput;
    bool didReset = false;

    [Header("Parameters")]
    [SerializeField] private PlayerInput m_playerInput = null;
    [SerializeField] private string m_navigateActionName = null;

    public List<GameObject> PlayerOneHeadButtons;
    public List<GameObject> PlayerOneCharacterButtons;
    private int PlayerOneHeadButtonIndex;
    private int PlayerOneCharacterButtonIndex;

    public List<GameObject> PlayerTwoHeadButtons;
    public List<GameObject> PlayerTwoCharacterButtons;
    private int PlayerTwoHeadButtonIndex;
    private int PlayerTwoCharacterButtonIndex;

    /*public List<GameObject> PlayerOneVerticalButtons;
    public GameObject PlayerOneHeadPreviousButton;
    public GameObject PlayerOneCharacterPreviousButton;
    private int PlayerOneButtonIndex;

    public List<GameObject> PlayerTwoVerticalButtons;
    public GameObject PlayerTwoHeadPreviousButton;
    public GameObject PlayerTwoCharacterPreviousButton;
    private int PlayerTwoButtonIndex;*/


    private void Start()
    {
        //InitCharacterSprites();
    }
    private void Update()
    {
        NavigationInput = m_playerInput.actions[m_navigateActionName].ReadValue<Vector2>();
        if (NavigationInput.y == 0)
        {
            didReset = true;
            return;
        }
        if (!didReset)
            return;
        didReset = false;
        NavigateMenuVertically(NavigationInput.y);
        //NavigateMenuHorizontally(NavigationInput.x);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void NavigateMenuHorizontally(float pNavigationMagnitude)
    {

        EventSystem.current.SetSelectedGameObject(null);
        if (CharacterSelectionButtonIndex == 0)
        {
            if (pNavigationMagnitude < 0)
                PlayerOneHeadButtonIndex++;
            else
                PlayerOneHeadButtonIndex--;

            if (PlayerOneHeadButtonIndex < 0)
                PlayerOneHeadButtonIndex = 0;
            if (PlayerOneHeadButtonIndex > PlayerOneHeadButtons.Count - 1)
                PlayerOneHeadButtonIndex = PlayerOneHeadButtons.Count - 1;

            EventSystem.current.SetSelectedGameObject(
            PlayerOneHeadButtons[PlayerOneHeadButtonIndex]);
            return;
        }
        
        if(CharacterSelectionButtonIndex == 1)
        {
            if (pNavigationMagnitude < 0)
                PlayerOneCharacterButtonIndex++;
            else
                PlayerOneCharacterButtonIndex--;

            if (PlayerOneCharacterButtonIndex < 0)
                PlayerOneCharacterButtonIndex = 0;
            if (PlayerOneCharacterButtonIndex > PlayerOneCharacterButtons.Count - 1)
                PlayerOneCharacterButtonIndex = PlayerOneCharacterButtons.Count - 1;

            EventSystem.current.SetSelectedGameObject(
            PlayerOneCharacterButtons[PlayerOneCharacterButtonIndex]);
            return;
        }
        
        if(CharacterSelectionButtonIndex == 2)
        {
            if (pNavigationMagnitude < 0 && CharacterSelectionButtonIndex == 2)
                PlayerTwoHeadButtonIndex++;
            else
                PlayerTwoHeadButtonIndex--;

            if (PlayerTwoHeadButtonIndex < 0)
                PlayerTwoHeadButtonIndex = 0;
            if (PlayerTwoHeadButtonIndex > PlayerTwoHeadButtons.Count - 1)
                PlayerTwoHeadButtonIndex = PlayerTwoHeadButtons.Count - 1;

            EventSystem.current.SetSelectedGameObject(
                PlayerTwoHeadButtons[PlayerTwoHeadButtonIndex]);
            return;
        }
        
        if(CharacterSelectionButtonIndex == 3)
        {
            if (pNavigationMagnitude < 0 && CharacterSelectionButtonIndex == 3)
                PlayerTwoCharacterButtonIndex++;
            else
                PlayerTwoCharacterButtonIndex--;

            if (PlayerTwoCharacterButtonIndex < 0)
                PlayerTwoCharacterButtonIndex = 0;
            if (PlayerTwoCharacterButtonIndex > PlayerTwoCharacterButtons.Count - 1)
                PlayerTwoCharacterButtonIndex = PlayerTwoCharacterButtons.Count - 1;

            EventSystem.current.SetSelectedGameObject(
                PlayerTwoCharacterButtons[PlayerTwoCharacterButtonIndex]);
            return;
        }
    }

    public void NavigateMenuVertically(float pNavigationMagnitude)
    {
        EventSystem.current.SetSelectedGameObject(null);
        if (pNavigationMagnitude <= 0)
            CharacterSelectionButtonIndex++;
        else
            CharacterSelectionButtonIndex--;
        
        if (CharacterSelectionButtonIndex == 0)
            PlayerOneHeadButtonIndex = 0;

        if (CharacterSelectionButtonIndex == 1)
            PlayerOneCharacterButtonIndex = 0;

        if (CharacterSelectionButtonIndex == 2)
            PlayerTwoHeadButtonIndex = 0;

        if (CharacterSelectionButtonIndex == 3)
            PlayerTwoCharacterButtonIndex = 0;

        if (CharacterSelectionButtonIndex < 0)
            CharacterSelectionButtonIndex = 0;
        if (CharacterSelectionButtonIndex > CharacterSelectionScreenButtons.Count - 1)
            CharacterSelectionButtonIndex = CharacterSelectionScreenButtons.Count - 1;
        EventSystem.current.SetSelectedGameObject(
            CharacterSelectionScreenButtons[CharacterSelectionButtonIndex]);
        Debug.Log("Current Button Index: " + CharacterSelectionButtonIndex);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void InitCharacterSprites()
    {
        firstCharacterImage.sprite = CharacterSprites[0];
        firstCharacterAccessory.sprite = CosmeticSprites[0];
        firstCharacterSpriteIndex = 0;
        firstCharacterAccessoryIndex = 0;
        secondCharacterImage.sprite = CharacterSprites[1];
        secondCharacterImage.sprite = CosmeticSprites[1];
        secondCharacterSpriteIndex = 1;
        secondCharacterAccessoryIndex = 1;
    }

    public void PlayerOneNextCharacter()
    {
        firstCharacterSpriteIndex++;
        firstCharacterImage.sprite = CharacterSprites[firstCharacterSpriteIndex];
    }
    public void PlayerOnePreviousCharacter()
    {
        firstCharacterSpriteIndex--;
        firstCharacterImage.sprite = CharacterSprites[firstCharacterSpriteIndex];
    }

    public void PlayerOneNextCosmetic()
    {
        firstCharacterAccessoryIndex++;
        firstCharacterAccessory.sprite = CosmeticSprites[firstCharacterAccessoryIndex];
    }

    public void PlayerOnePreviousCosmetic()
    {
        firstCharacterAccessoryIndex--;
        firstCharacterAccessory.sprite = CosmeticSprites[firstCharacterAccessoryIndex];
    }


    public void PlayerTwoNextCharacter()
    {
        secondCharacterSpriteIndex++;
        secondCharacterImage.sprite = CharacterSprites[secondCharacterSpriteIndex];
    }
    public void PlayerTwoPreviousCharacter()
    {
        secondCharacterSpriteIndex--;
        secondCharacterImage.sprite = CharacterSprites[secondCharacterSpriteIndex];
    }
    public void PlayerTwoNextCosmetic()
    {
        secondCharacterAccessoryIndex++;
        secondCharacterAccessory.sprite = CosmeticSprites[secondCharacterAccessoryIndex];
    }

    public void PlayerTwoPreviousCosmetic()
    {
        secondCharacterAccessoryIndex--;
        secondCharacterAccessory.sprite = CosmeticSprites[secondCharacterAccessoryIndex];
    }

}
