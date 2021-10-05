using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[AddComponentMenu("Game/GameManager")]
public class Gamemanager : MonoBehaviour {

    //// Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}
    public static Gamemanager Instance = null;
    //游戏得分
    public int m_score = 0;
    //游戏最高得分
    public static int m_hiscore = 0;
    //弹药数量
    public int m_ammo = 100;

    Player m_player;

    //UI文字
    Text txt_ammo;
    Text txt_hiscore;
    Text txt_life;
    Text txt_score;
    Button button_restart;
    public List<Transform> uiList;

    public static void FindChildAll<T>(Transform obj, List<T> list) where T : Component
    {
        T t = obj.GetComponent<T>();
        if (t != null)
        {
            list.Add(t);
        }
        if (obj.childCount > 0)
        {
            for (int i = 0; i < obj.childCount; i++)
            {
                FindChildAll(obj.GetChild(i), list);
            }
        }
    }

    void Start()
    {
        Instance = this;
        //找到主角对象
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        //获得UI文字
        GameObject uicanvas = GameObject.Find("Canvas");
        //if(uicanvas!=null)
        //{
        //    testui = 1;
        //}

        FindChildAll<Transform>(uicanvas.transform,uiList);

        foreach (Transform t in uiList)
        {
            if(t.name.CompareTo("txt_ammo") ==0)
            {
                //text_ammotest = 1;
                txt_ammo = t.GetComponent<Text>();
            }
            else if(t.name.CompareTo("txt_hiscore")==0)
            {
                txt_hiscore = t.GetComponent<Text>();
                txt_hiscore.text = "Hiscore" + m_hiscore;
            }
            else if(t.name.CompareTo("txt_life") ==0)
            {
                txt_life = t.GetComponent<Text>();
            }
            else if(t.name.CompareTo("txt_score")==0)
            {
                txt_score = t.GetComponent<Text>();
            }
            else if(t.name.CompareTo("Restart Button")==0)
            {
                button_restart = t.GetComponent<Button>();
                button_restart.onClick.AddListener(delegate ()
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                });
                button_restart.gameObject.SetActive(false);
            }
            Debug.Log(t.name);
        }
        SetLife(m_player.m_life);
        SetAmmo(100);
        SetScore(0);

    }

    //更新分数
    public void SetScore(int score)
    {
        m_score += score;
        if(m_score>m_hiscore)
        {
            m_hiscore = m_score;
        }
        txt_score.text = "Scene<color=yellow>" + m_score + "</color>";
        txt_hiscore.text = "High Score" + m_hiscore.ToString();
    }

    public void SetAmmo(int ammo)
    {
        m_ammo -= ammo;
        if(m_ammo<0)
        {
            m_ammo = 100 - ammo;
        }
        txt_ammo.text = m_ammo.ToString() + "/100";
    }

    //更新生命
    public void SetLife(int life)
    {
        txt_life.text = life.ToString();
        if(life<=0)
        {
            button_restart.gameObject.SetActive(true);
        }
    }


}
