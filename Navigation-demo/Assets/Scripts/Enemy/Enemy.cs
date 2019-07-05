using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using UnityEngine.Mobile.AdaptivePerformance;
public class Enemy : MonoBehaviour
{

   // private IAdaptivePerformance ap = null;

    public GameObject player_object;
    private NavMeshAgent navMashAgent;
    // Start is called before the first frame update
    void Start()
    {
      //  ap = Holder.instance;
       // if (!ap.active)
         //   return;
       // Debug.Log(ap.averageFrameTime);

        navMashAgent = this.gameObject.GetComponent<NavMeshAgent>();
      //  ap.infosendSet.mainVersion.MainVersionNumber=100;
       // ap.infosendSet.mainVersion.Enable = true;
    }
    // Update is called once per frame
    void Update()
    {
        navMashAgent.SetDestination(player_object.transform.position);
    }
}
