using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum HatsEnum
{
    NONE,
    ANVIL,
    BUTTERFLY,
    CLAYSTATUE,
    HAT,
    SCARF,
    SHADES,
    STRAWHAT
}
[Serializable]
public struct PlayerCharacterData
{
    public Mesh PlayerMesh;
    public Material[] PlayerMaterials;
    public HatsEnum Hat;
}
public class PlayerMeshManager : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer PlayerOneRenderer = null;
    [SerializeField] private SkinnedMeshRenderer PlayerTwoRenderer = null;
    [SerializeField] private BodyController PlayerOneController = null;
    [SerializeField] private BodyController PlayerTwoController = null;
    [SerializeField] private PlayerCharacterData[] playerCharacterDatas;
    private void Start()
    {
        SetPlayerMesh(playerCharacterDatas[UnityEngine.Random.Range(0, playerCharacterDatas.Length)],1);
        SetPlayerMesh(playerCharacterDatas[UnityEngine.Random.Range(0, playerCharacterDatas.Length)],0);
    }
    public void SetPlayerMesh(PlayerCharacterData data,int number)
    {
        if (number == 1)
        {
            PlayerOneRenderer.sharedMesh = data.PlayerMesh;
            PlayerOneRenderer.materials = data.PlayerMaterials;
            PlayerOneController.SetHat(data.Hat);
        }
        else
        {
            PlayerTwoRenderer.sharedMesh = data.PlayerMesh;
            PlayerTwoRenderer.materials = data.PlayerMaterials;
            PlayerTwoController.SetHat(data.Hat);
        }
    }
}
