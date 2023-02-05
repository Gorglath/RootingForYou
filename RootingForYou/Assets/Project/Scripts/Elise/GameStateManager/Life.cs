using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{

    public int numLives = 3;

    public void RemoveLife()
    {
        numLives--;
        if(numLives == 0)
        {
            OnNoLives();
        }
    }

    public void AddLife()
    {
        numLives++;
    }

    public int GetNumLives()
    {
        return numLives;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnNoLives()
    {

    }
}
