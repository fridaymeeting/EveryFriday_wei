using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playgun : MonoBehaviour
{
    private Gun gun;

    private void Start()
    {
        gun = GetComponent<Gun>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {//朝向枪口正前方发射子弹
            gun.Firing(gun.firePoint.forward);
            //  Debug.LogError(gun.firePoint.forward);
        }
        if (Input.GetMouseButtonDown(2))
        {//朝向枪口正前方发射子弹
            gun.UpdateAmmo();
        }
    }
}
