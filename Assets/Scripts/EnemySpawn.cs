using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Game/EnemySpawn")]
public class EnemySpawn : MonoBehaviour
{

    //// Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}
    //敌人的prefab
    public Transform m_enemy;
    public int m_enemyCount = 0;
    public int m_maxEnemy=3;
    //生成敌人的时间间隔
    public float m_timer = 0;

    protected Transform m_transform;


    void Start()
    {
        m_transform = transform;
    }
    void Update()
    {
        if(m_enemyCount>=m_maxEnemy)
        {
            return;
        }
        m_timer -= Time.deltaTime;
        m_timer = Random.value * 15.0f;
        if(m_timer<5)
        {
            m_timer = 5;
        }
        //生成敌人
        Transform obj = (Transform)Instantiate(m_enemy, m_transform.position, Quaternion.identity);

        Enemy enemy = obj.GetComponent<Enemy>();

        enemy.Init(this);
        
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "item.png", true)
;    }


}
