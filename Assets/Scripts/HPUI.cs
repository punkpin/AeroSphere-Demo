using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPUI : MonoBehaviour
{
    [Header("HP图像")]
    public Sprite hasHeart;
    public Sprite noHeart;
    public List<Image> HPImage = new List<Image>();//储存图片组件
    private PlayerController playerController;
    private Dictionary<GameObject ,bool > EnemyManage=new Dictionary<GameObject,bool>();//储存怪物数据,bool是是否执行过伤害

    void Awake()
    {
        GameObject player = GameObject.FindWithTag("Player");//从player中获取组件
        if (player != null)
        {

            playerController = player.GetComponent<PlayerController>();

            if (playerController == null)
            {
                Debug.LogError("PlayerController component not found on the Player object!");
            }

        }

    }
    
    void Update()
    {
        UpdateHPUI();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject enemy = collision.gameObject;
        int layer = collision.gameObject.layer;
        string layerName = LayerMask.LayerToName(layer);
        string objectName = collision.gameObject.name;


        if (layerName == "Enemy")//层次判断
        {
            if (!EnemyManage.ContainsKey(enemy))//添加新的怪物
            {
                EnemyManage.Add(enemy, true);
                Debug.Log(objectName);                                 
                playerController.HPChange();
            }
            else
            {
                if (EnemyManage[enemy] == false)
                {
                    playerController.HPChange();
                }
            }

        }

    }
    void OnCollisionExit2D(Collision2D collision)
    {
        GameObject enemy = collision.gameObject;
        int layer = collision.gameObject.layer;
        string layerName = LayerMask.LayerToName(layer);
        string objectName = collision.gameObject.name;
        if (layerName == "Enemy")//层次判断
        {
            EnemyManage[enemy] = false;

        }
    }
    private void UpdateHPUI()//更新血量ui的图片
    {
        for(int number = 1; number <= 3; number++)
        {
            if (playerController.HP >= number)
            {
                Image image = HPImage[number - 1];
                image.sprite = hasHeart;
                
            }
            else
            {
                Image image = HPImage[number - 1];
                image.sprite = noHeart;
                
            }
            
        }
    }
}

