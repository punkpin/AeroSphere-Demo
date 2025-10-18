using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPUI : MonoBehaviour
{
    private PlayerController playerController;
    private Dictionary<GameObject ,bool > EnemyManage=new Dictionary<GameObject,bool>();//�����������,bool���Ƿ�ִ�й��˺�

    void Awake()
    {
        GameObject player = GameObject.FindWithTag("Player");//��player�л�ȡ���
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


        if (layerName == "Enemy")//����ж�
        {
            if (!EnemyManage.ContainsKey(enemy))//����µ�
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
        if (layerName == "Enemy")//����ж�
        {
            EnemyManage[enemy] = false;

        }
    }
}

