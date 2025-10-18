using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("������ֵ")]
    public float EnemyHP;
    public float EnemySpeed;
    [Header("��˸Ч��")]//����û����
    private Renderer enemyRenderer;
    private Color CurrentColor;
    private float flashDuration = 0.3f;
    private Animator animator;
    void Awake()
    {
        enemyRenderer = GetComponent<Renderer>();
        animator = GetComponentInChildren<Animator>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("111");
        int layer = collision.gameObject.layer;
        string layerName = LayerMask.LayerToName(layer);
        string objectName = collision.gameObject.name;
        // �ж��Ƿ�ΪUI,ͬʱ������hp
        if (layerName == "UI" && objectName != "Hp")
        {
            Died(this.gameObject);
        }

    }
    public void Died(GameObject gameobject)//����
    {
        StartCoroutine(DiedAnimation(gameobject));        
    }
    IEnumerator DiedAnimation(GameObject gameObject)
    {
        animator.SetBool("Dead", true);
        yield return new WaitForSeconds(1.2f + 0.75f);//������ͣ������ʧ
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
