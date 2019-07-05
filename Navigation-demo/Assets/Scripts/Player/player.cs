using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class player : MonoBehaviour
{
    // Start is called before the first frame update
    private NavMeshAgent navMashAgent;
    public Transform target;
    public GameObject door_object;
    private HingeJoint door_joint;
   // private Rigidbody door_;
    static JointMotor motor;
    private Ray ray;
    void Start()
    {
        motor = new JointMotor();

        navMashAgent = this.gameObject.GetComponent<NavMeshAgent>();
        door_joint = door_object.GetComponent<HingeJoint>();
        //var motoro = door_joint.motor;
       // motoro.targetVelocity = 30;
      //  door_joint.motor = motoro;
        // Debug.Log(motor.targetVelocity);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        { 
             ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                //Transform parent = hit.transfor
                transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
                navMashAgent.SetDestination(hit.point);
               // navMashAgent.walkableMask
            }

        }
    }
    private float ratateSpeed = 2f;
    private void FixedUpdate()
    {
        float Ho = Input.GetAxis("Horizontal");
        if (Mathf.Abs(Ho) > 0)
        {
            Ho *= ratateSpeed;
            this.transform.Rotate(0, Ho, 0, Space.World);
        }
    }
    [System.Obsolete]
    void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width - 190, 60, 100, 20), "启动吊桥"))
        {
            if (Bridgestate())
            {
                navMashAgent.walkableMask = 215;
            }
            else
                navMashAgent.walkableMask = 223;
        }
    }
    private bool doorflag=true;
    private bool Bridgestate()
    {

        doorflag = !doorflag;
        if (!doorflag) 
        {
            motor.targetVelocity = 90;
            motor.force = 8;
            door_joint.motor = motor;
            return true;
        }
        else
        {
            motor.targetVelocity =-90;
            motor.force = 8;
            door_joint.motor = motor;
            return false;
        }
       
    }

}
