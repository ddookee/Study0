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
        else//������� �޾Ҵµ� ü���� 0�� �ƴҶ� ���� ����
        {
            sr.color = new Color(1, 0.5f, 0.5f, 1);
            Invoke("delegateCode", 0.3f); //float��ŭ �ð��� �帥�� delegateCode����
        }
        Debug.Log(CurHp);
    }
    /// <summary>
    /// ��������Ʈ�� ���� ������ �ٲ���
    /// </summary>
    private void delegateCode()
    {
        sr.color = Color.white;
    }
}
