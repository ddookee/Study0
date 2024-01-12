using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float MaxHp = 3;
    private float CurHp = 0;
    public float damage = 1;

    [SerializeField] private Sprite sprHit;
    private Sprite sprDefault;

    private SpriteRenderer sr;

    private void Awake()
    {
        CurHp = MaxHp;
        sr = GetComponent<SpriteRenderer>();

        sprDefault = sr.sprite;
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
        else
        {
            sr.sprite = sprHit;//하얗게 변함
            //몇초 후에 어떤 기능을 실행해줘, 매개변수가 없어야 함
            Invoke("setSpriteDefault", 0.1f);
        }
    }

    private void setSpriteDefault()
    {
        sr.sprite = sprDefault;
    }
}
