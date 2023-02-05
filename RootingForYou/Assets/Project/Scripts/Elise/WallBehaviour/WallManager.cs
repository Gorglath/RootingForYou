using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

public class WallManager : MonoBehaviour
{

    public GameObject startingPosition;
    private Wall[] walls;
    private Wall currentWall;
    private Wall currentShadowWall;
    public float speed  = 5.0f;


    void Start()
    {
        walls = GetWalls();
        DeactivateWalls();
        currentWall = GetRandomWall();
        ResetWall();
    }

    // Update is called once per frame
    void Update()
    {
        //currentWall.UpdatePosition();
    }

    private void DeactivateWalls()
    {
        foreach(Wall wall in walls)
        {
            wall.gameObject.SetActive(false);
        }
    }
    public void ChangeWall()
    {
        // Deactivate currentwall
        currentWall.gameObject.SetActive(false);
        // Get random Wall
        currentWall = GetRandomWall();
        // Reset new Wall
        ResetWall();
    }
    public Wall GetCurrentWall()
    {
        return currentWall;
    }

    private Wall[] GetWalls()
    {
        return FindObjectsOfType<Wall>();
    }
    public Wall GetRandomWall()
    {
        return walls[UnityEngine.Random.Range(0, walls.Length)];
    }

    public void ResetWall()
    {
        currentWall.transform.position = startingPosition.transform.position;
        currentWall.gameObject.SetActive(true);
        currentWall.SetSpeed(speed);
        MakeShadowWall();
    }

    private void MakeShadowWall()
    {
        if(currentShadowWall)
            Destroy(currentShadowWall.gameObject);
        currentShadowWall = Instantiate(currentWall);
        Destroy(currentShadowWall.gameObject.GetComponentInChildren<Collider>());
        Destroy(currentShadowWall.gameObject.GetComponentInChildren<Rigidbody>());
        BaseCollider baseCollider = FindObjectOfType<BaseCollider>();
        currentShadowWall.transform.position = new Vector3(baseCollider.transform.position.x, baseCollider.transform.position.y, baseCollider.transform.position.z);
        Material mat = AssetDatabase.LoadAssetAtPath("Assets/Project/Materials/ShadowWall.mat", typeof(Material)) as Material;
        currentShadowWall.GetComponent<MeshRenderer>().material = mat;
    }
}
