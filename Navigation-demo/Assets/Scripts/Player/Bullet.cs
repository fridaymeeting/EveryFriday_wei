using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float atk;
    public RaycastHit hit;
    private float atkDistance;
    public LayerMask layer;
    public Vector3 targetPos;
    public void Init(float atk, float distance)
    {
        this.atk = atk;
        atkDistance = distance;
        CalculateTargetPoint();
    }


    //通过射线计算击中物体
    private void CalculateTargetPoint()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, atkDistance, layer))
        {
            targetPos = hit.point;
        }
        else
        {
            //targetPos = transform.position + transform.forward * atkDistance;
            targetPos = transform.TransformPoint(0, 0, atkDistance);
        }
    }
    private void Start()
    {
        if (hit.collider != null && hit.collider.tag == "Enemy")
        {
            hit.collider.GetComponentInParent<EnemyStatus>().Damage(atk);
        }
    }
    private void Update()
    {
        Movement();
        if ((transform.position - targetPos).sqrMagnitude < 0.1f)
        { 
            Destroy(gameObject);
        }
    }
    public float moveSpeed = 100;
    private void Movement()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeed);
    }
}
