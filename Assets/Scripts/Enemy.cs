using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    Transform m_transform;
    //动画组件
    Animator m_ani;

    Player m_player;
    //寻路组件
    NavMeshAgent m_agent;
    float m_moveSpeed = 2.5f;
    float m_rotSpeed = 5.0f;
    float m_timer = 2;
    public int m_life = 1;

    protected EnemySpawn m_spawn;
    //出生点
    //protected EnemySpawn m_spawn;

    // Use this for initialization
    void Start ()
    {
        m_transform = this.transform;
        //初始化动画播放器
        m_ani = this.GetComponent<Animator>();
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        m_agent = GetComponent<NavMeshAgent>();
        m_agent.speed = m_moveSpeed;
        m_agent.SetDestination(m_player.transform.position);
		
	}

    void RotateTo()
    {
        //获取player方向
        Vector3 targetdir = m_player.m_transform.position - m_transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetdir, m_rotSpeed * Time.deltaTime, 0.0f);
        m_transform.rotation = Quaternion.LookRotation(newDir);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(m_player.m_life<=0)
        {
            return;
        }
        m_timer -= Time.deltaTime;
        AnimatorStateInfo stateInfo = m_ani.GetCurrentAnimatorStateInfo(0);

        //如果处于待机且不是过渡状态
        if(stateInfo.fullPathHash==Animator.StringToHash("Base Layer.idle")&&!m_ani.IsInTransition(0))
        {
            m_ani.SetBool("idle", false);
            if(m_timer>0)
            {
                return;
            }
            if(Vector3.Distance(m_transform.position,m_player.m_transform.position)<1.5f)
            {
                //停止寻路
                m_agent.ResetPath();
                m_ani.SetBool("attack", true);

            }
            else
            {
                //重置定时器
                m_timer = 1;
                //设置寻路
                m_agent.SetDestination(m_player.m_transform.position);
                //进入跑步
                m_ani.SetBool("run", true);

            }
        }

        //如果处于跑步且不是过渡状态
        if(stateInfo.fullPathHash==Animator.StringToHash("Base Layer.run")&&!m_ani.IsInTransition(0))
        {
            m_ani.SetBool("run", false);
            if(m_timer<0)
            {
                m_agent.SetDestination(m_player.m_transform.position);
                m_timer = 1;
            }

            //如果距离小于1.5，进入攻击模式
            if(Vector3.Distance(m_transform.position,m_player.m_transform.position)<=1.5f)
            {
                m_agent.ResetPath();
                //进入攻击状态
                m_ani.SetBool("attack", true);
            }
        }
        //如果处于攻击且不是过渡状态
        if(stateInfo.fullPathHash==Animator.StringToHash("Base Layer.attack")&&!m_ani.IsInTransition(0))
        {
            //面向主角
            RotateTo();
            m_ani.SetBool("attack", false);
            if(stateInfo.normalizedTime>=1.0f)
            {
                m_ani.SetBool("idle", true);
                //重置计时器
                m_timer = 2;
                m_player.OnDamage(1);
            }
        }
        //如果处于死亡且不是过渡状态
        if(stateInfo.fullPathHash==Animator.StringToHash("Basw Layer.death")&&!m_ani.IsInTransition(0))
        {
            m_ani.SetBool("death", false);
            if(stateInfo.normalizedTime>=1.0f)
            {
                //加分
                Gamemanager.Instance.SetScore(100);
                m_spawn.m_enemyCount--;
                Destroy(this.gameObject);
                
            }
        }

        //if(stateInfo.fullPathHash==Animator.StringToHash("Base Layer.attack")&&!m_ani.IsInTransition(0))
        //{
        //    RotateTo();
        //    m_ani.SetBool("attack")

        //}
		
	}

    public void OnDamage(int damage)
    {
        m_life -= damage;
        if(m_life<=0)
        {
            m_ani.SetBool("death", true);
            m_agent.ResetPath();
        }
    }

    public void Init(EnemySpawn spawn)
    {
        m_spawn = spawn;
        m_spawn.m_enemyCount++;
    }


}
