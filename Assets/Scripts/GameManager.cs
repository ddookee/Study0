using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;//�̱���, �� �ϳ��� �����Ҽ��ִ� ��ũ��Ʈ

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    [Header("Npc")]
    [SerializeField] GameObject objNpc;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public GameObject GetNpcGameObject()//Npc���ӿ�����Ʈ�� ��������
    {
        return objNpc;
    }
}
