using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cameramove : MonoBehaviour
{
    private Vector3 camera_vector;
    public GameObject playerObject;
    private void Start()
    {
        camera_vector =this.transform.position;
        
    }
    // Update is called once per frame
    void Update()
    {
        camera_vector.z = playerObject.transform.position.z;
        this.transform.position= camera_vector;
        
        
    }
}
