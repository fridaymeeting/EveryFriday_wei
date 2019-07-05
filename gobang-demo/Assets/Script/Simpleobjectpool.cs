using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simpleobjectpool : MonoBehaviour
{
    private Stack<GameObject> inactiveInstances = new Stack<GameObject>();

    // Start is called before the first frame update
    public GameObject GetObject()
    {
        GameObject spawnedGameObject;
        spawnedGameObject = inactiveInstances.Pop();
        spawnedGameObject.SetActive(true);
        spawnedGameObject.transform.SetParent(null);
        return spawnedGameObject;
    }
    public void ReturnObject(GameObject toReturn)
    {

            toReturn.transform.SetParent(transform);
            toReturn.SetActive(false);
            inactiveInstances.Push(toReturn);


    } 

}
