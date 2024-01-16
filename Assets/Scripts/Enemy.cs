using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float MaxHp = 3;
    private float CurHp = 0;
    public float damage = 1;


    private SpriteRenderer sr;

    private void Awake()
    {
        CurHp = MaxHp;
        sr = GetComponent<SpriteRenderer>();

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
        else//대미지를 받았는데 체력이 0이 아닐때 색을 변경
        {
            sr.color = new Color(1, 0.5f, 0.5f, 1);
            Invoke("delegateCode", 0.3f); //float만큼 시간이 흐른뒤 delegateCode실행
        }
        Debug.Log(CurHp);
    }
    /// <summary>
    /// 스프라이트를 원랙 색으로 바꿔줌
    /// </summary>
    private void delegateCode()
    {
        sr.color = Color.white;
    }
}
