using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform m_transform;
    CharacterController m_ch;//��ɫ���������
    float m_moveSpeed=3.0f;//��ɫ�ƶ��ٶ�
    float m_gravity = 2.0f;//����
    public int m_life = 5;
    Transform m_camTransform;

    Vector3 m_camRot;
    float m_camHeight = 1.4f;

    Transform m_mullzepoint;
    public LayerMask m_layer;
    public Transform m_fx;
    public AudioClip m_audio;
    float m_shootTimer = 0;

    //public List al;
    //struct A
    //{
    //    public List<int> a1;
    //}



    // Start is called before the first frame update
    void Start()
    {
        m_transform = this.transform;
        m_camTransform = Camera.main.transform;

        m_mullzepoint = m_camTransform.Find("M16/weapon/muzzlepoint").transform;

        m_ch = this.GetComponent<CharacterController>();
        //��ȡ�����
        
        //�����������ʼλ��
        m_camTransform.position = m_transform.TransformPoint(0, m_camHeight, 0);
        m_camTransform.rotation = m_transform.rotation;
        m_camRot = m_camTransform.eulerAngles;
        //Gamemanager.Instance.SetLife(m_life);
        //Screen.lockCursor = true;
        //Cursor.lockState = true;
    }

    // Update is called once per frame
    void Update()
    {
        m_shootTimer -= Time.deltaTime;
        if(Input.GetMouseButton(0)&&m_shootTimer<=0)
        {
            m_shootTimer = 0.1f;
            this.GetComponent<AudioSource>().PlayOneShot(m_audio);
            //���ٵ�ҩ������UI
            Gamemanager.Instance.SetAmmo(1);
            RaycastHit info;
            bool hit = Physics.Raycast(m_mullzepoint.position, m_camTransform.TransformDirection(Vector3.forward), out info, m_layer);
            if(hit)
            {
                if(info.transform.tag.CompareTo("enemy")==0)
                {
                    Enemy enemy = info.transform.GetComponent<Enemy>();
                    enemy.OnDamage(1);
                }
            }
            Instantiate(m_fx, info.point, info.transform.rotation);
        }
        //�������ֵΪ0��ʲôҲ����
        if(m_life<=0)
        {
            return;
        }
        Control();
        
    }

    void Control()//���ƴ���
    {
        float rh = Input.GetAxis("Mouse X");
        float rv = Input.GetAxis("Mouse Y");
        m_camRot.x -= rv;
        m_camRot.y += rh;
        m_camTransform.eulerAngles = m_camRot;

        Vector3 camrot = m_camTransform.eulerAngles;
        camrot.x = 0;
        camrot.z = 0;
        m_camTransform.eulerAngles = camrot;


        Vector3 motion = Vector3.zero;
        motion.x = Input.GetAxis("Horizontal") * m_moveSpeed * Time.deltaTime;
        motion.z = Input.GetAxis("Vertical") * m_moveSpeed * Time.deltaTime;
        motion.y -= m_gravity * Time.deltaTime;//����
        m_ch.Move(m_transform.TransformDirection(motion));
        m_camTransform.position = m_transform.TransformPoint(0, m_camHeight, 0);



    }
    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(this.transform.position, "Spawn.tif");
    }
    public void OnDamage(int damage)
    {
        m_life -= damage;
        Debug.Log(m_life);
        Gamemanager.Instance.SetLife(m_life);
        if(m_life<0)
        {
            Screen.lockCursor = false;
        }
    }
}
