using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float MaxHp = 3;
    private float CurHp = 0;


    private void Awake()
    {
        CurHp = MaxHp;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit(float _damage)
    {
        CurHp -= _damage;

        if (CurHp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
