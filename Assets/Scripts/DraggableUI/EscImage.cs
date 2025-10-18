using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EscImage : MonoBehaviour
{
    public Player inputSystem;//����ϵͳ
    public Image image;//ͼ�����
    private bool switchImage;
    public Sprite Esc;
    public Sprite Right;
    public GameObject Text;//�����ı�����
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
        if (inputSystem.GamePlay.Esc.WasPressedThisFrame())//���esc��
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
