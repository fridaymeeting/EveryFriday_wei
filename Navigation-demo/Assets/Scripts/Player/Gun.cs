using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float atk;
    public float atkDistance = 200;
   // public Text text01;
   // public Text text02;
    public GameObject bulletPrefab;//子弹预制件
    public Transform firePoint; //起点

    private void Start()
    {
      //  text01.text = string.Format("弹夹子弹数: {0:d2}", currentAmmoBullets);
      //  text02.text = string.Format("剩余子弹数: {0:d2}", remainBullets);
    }
    public bool Firing(Vector3 dir)
    {
        if (!Ready()) return false;   //判定能否发射子弹(弹匣子弹数     攻击动画是否播放)
                                      //  Debug.Log("ready");
                                      //如果可以发射子弹
        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(dir)) as GameObject;//创建子弹（预制件，位置，指向）

        bulletGO.GetComponent<Bullet>().Init(atk, atkDistance);        //初始化，传递攻击力、攻击距离

        return true;
    }
    public int ammoCapacity;//弹匣容量
    public int currentAmmoBullets; //当前弹匣内子弹数
    public int remainBullets;//剩余子弹数
    private bool Ready()
    {
        if (currentAmmoBullets <= 0) return false;

        currentAmmoBullets--;
      //  text01.text = string.Format("弹夹子弹数: {0:d2}", currentAmmoBullets);

        return true;

    }
    public void UpdateAmmo()
    {
        //能否更换(剩余子弹数    当前弹匣子弹数)
        if (remainBullets <= 0 || currentAmmoBullets == ammoCapacity) return;

        //向当前弹匣内添加子弹
        currentAmmoBullets = remainBullets >= ammoCapacity ? ammoCapacity : remainBullets;

        //扣除剩余子弹
        remainBullets -= currentAmmoBullets;
     //   text02.text = string.Format("剩余子弹数: {0:d2}", remainBullets);
    //    text01.text = string.Format("弹夹子弹数: {0:d2}", currentAmmoBullets);

    }
}
