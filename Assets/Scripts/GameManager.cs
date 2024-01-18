using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;//싱글턴, 단 하나만 존재할수있는 스크립트

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
    public GameObject GetNpcGameObject()//Npc게임오브젝트를 내보내줌
    {
        return objNpc;
    }
}
