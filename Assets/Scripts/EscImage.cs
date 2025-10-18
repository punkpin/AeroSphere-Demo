using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EscImage : MonoBehaviour
{
    public Player inputSystem;//输入系统
    public Image image;//图像组件
    private bool switchImage;
    public Sprite Esc;
    public Sprite Right;
    public GameObject Text;//隐藏文本内容
    void Awake()
    {
        inputSystem = new Player();
        image = GetComponent<Image>();
    }

    void Start()
    {
        image.sprite = Esc;
        switchImage = false;
        Text.SetActive(false);     
    }
    void OnEnable()
    {
        inputSystem.Enable();
    }
    void OnDisable()
    {
        inputSystem.Disable();
    }
    void Update()
    {
        if (inputSystem.GamePlay.Esc.WasPressedThisFrame())//检测esc键
        {
            Debug.Log("666");
            {
                
            }
            if (switchImage == false)
            {
                image.sprite = Right;
                switchImage = !switchImage;
            }
            else
            {
                image.sprite = Esc;
                switchImage = !switchImage;
            }
        }
    }
}
