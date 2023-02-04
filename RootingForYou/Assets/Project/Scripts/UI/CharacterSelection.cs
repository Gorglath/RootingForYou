using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private void Start()
    {
        InitCharacterSprites();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
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
