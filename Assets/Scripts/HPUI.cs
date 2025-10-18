using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPUI : MonoBehaviour
{
    [Header("HPͼ��")]
    public Sprite hasHeart;
    public Sprite noHeart;
    public List<Image> HPImage = new List<Image>();//����ͼƬ���
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


        if (layerName == "Enemy")//����ж�
        {
            if (!EnemyManage.ContainsKey(enemy))//����µĹ���
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
    private void UpdateHPUI()//����Ѫ��ui��ͼƬ
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

