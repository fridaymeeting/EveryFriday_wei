using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemyspawn : MonoBehaviour
{
    public int startCount = 5;
    //当前产生的敌人数量
    private int spawnedCount;
    /// 可以产生敌人的最大值
    public int maxCount;
    /// 产生敌人的最大延迟时间
    public int maxDelay = 10;
    /// 敌人类型
    public GameObject[] enemyTypes;
    public GameObject player_ob;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < startCount; i++)
        {
            GenerateEnemy();
        }
    }
    public void GenerateEnemy()
    {
        if (spawnedCount < maxCount)
        {
            spawnedCount++;
            // 随机延迟时间 
            float delay = Random.Range(0, maxDelay);
            Invoke("CreateEnemy", delay);
        }
        else
        {
            print("over");
            //生成任务结束……
            //如果所有敌人死亡，再启用下一个生成器
         //   if (IsEnemyAllDeath())
          //  {
            //    print("death");
            //    GetComponentInParent<SpawnSystem>().ActivateNextSpawn();
           // }
        }
    }
    private void CreateEnemy()
    {
    
        int enemyTypeIndex = Random.Range(0, enemyTypes.Length);
        GameObject enemyGO = Instantiate(enemyTypes[enemyTypeIndex], this.transform.position, Quaternion.identity) as GameObject;
        enemyGO.GetComponent<Enemy>().player_object = player_ob;
        enemyGO.transform.SetParent(this.transform);

    }
    // Update is called once per frame
    private bool temp=false;
    private float delaytime = 10;
    void Update()
    {
        if (Time.time>delaytime)
        {
            if (this.transform.childCount == 0 && !temp)
            {
                GetComponent<Renderer>().material.color = Color.red;
               // 
                temp = true;
            }
        }
        
    }
    void OnCollisionEnter(Collision collisionInfo)
    {
        // Debug-draw all contact points and normals
        //  foreach (ContactPoint contact in collisionInfo.contacts)
        //  {
        //  Debug.DrawRay(contact.point, contact.normal * 10, Color.white);
        // }
       // Debug.Log("LLL");
    }
    private bool tempp = false;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag!="Enemy"&&(this.GetComponent<Renderer>().material.color == Color.red) && !tempp)
        {
            //Destroy(other.gameObject);
            Debug.Log("胜利");
            tempp = true;
        }
    }
}
