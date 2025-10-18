using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("������ֵ")]
    public float EnemyHP;
    public float EnemySpeed;
    [Header("��˸Ч��")]
    private Renderer enemyRenderer;
    private Color CurrentColor;
    private float flashDuration = 0.3f;
    void Awake()
    {
        enemyRenderer = GetComponent<Renderer>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        int layer = collision.gameObject.layer;
        string layerName = LayerMask.LayerToName(layer);
        string objectName = collision.gameObject.name;
        // �ж��Ƿ�ΪUI,ͬʱ������hp
        if (layerName == "UI" && objectName != "Hp")
        {
            Died(collision.gameObject);
        }

    }
    public void Died(GameObject gameobject)
    {
        gameObject.SetActive(false);
    }
    public void Faded()//���˺���˸
    {        

    }
    


    

    // Update is called once per frame
    void Update()
    {
        
    }
}
