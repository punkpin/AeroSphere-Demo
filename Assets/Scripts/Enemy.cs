using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("基础数值")]
    public float EnemyHP;
    public float EnemySpeed;
    [Header("闪烁效果")]//现在没有用
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
        // 判断是否为UI,同时不能是hp
        if (layerName == "UI" && objectName != "Hp")
        {
            Died(this.gameObject);
        }

    }
    public void Died(GameObject gameobject)//死亡
    {
        StartCoroutine(DiedAnimation(gameobject));        
    }
    IEnumerator DiedAnimation(GameObject gameObject)
    {
        animator.SetBool("Dead", true);
        yield return new WaitForSeconds(1.2f + 0.75f);//动画后停顿在消失
        gameObject.SetActive(false);

    }






    public void Faded()//受伤后闪烁
    {        

    }
    


    

    // Update is called once per frame
    void Update()
    {
        
    }
}
