using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{

    public float currentHP=2f;
    public float maxHP=2f;
    public void Damage(float amount)
    {

        if (currentHP <= 0) return;    //如果敌人已经死亡 则退出(防止虐尸)

        currentHP -= amount;

        if (currentHP <= 0)
            Death();
    }
    public float deathDelay = 0f;
    public void Death()
    {
       // GetComponent<IdleChanger>().DamageDownAni();
        //销毁当前游戏物体
        Destroy(gameObject, deathDelay);

        //修改状态
       // GetComponent<EenemyAI>().currentFindState = EenemyAI.State.Death;
    }
}
