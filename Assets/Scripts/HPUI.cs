using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPUI : MonoBehaviour
{
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
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject enemy = collision.gameObject;
        int layer = collision.gameObject.layer;
        string layerName = LayerMask.LayerToName(layer);
        string objectName = collision.gameObject.name;


        if (layerName == "Enemy")//层次判断
        {
            if (!EnemyManage.ContainsKey(enemy))//添加新的
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
}

