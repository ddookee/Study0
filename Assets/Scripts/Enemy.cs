using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;

public class Enemy : MonoBehaviour
{
    public enum hitType
    {
        WallCheck,
    }

    [SerializeField] private float MaxHp = 3;
    private float CurHp = 0;
    public float damage = 1;

    [SerializeField,Tooltip("Enemy�̵��ӵ�")] float moveSpeed;
    public float MoveSpeed()
    {
        return moveSpeed;
    }
    [SerializeField] LayerMask ground;
    Rigidbody2D rigid;
    [SerializeField,Tooltip("�ڽžտ� ���� �ִ��� ������ üũ�ϴ� ������Ʈ")] Collider2D trigger;

    private SpriteRenderer sr;

    private void Awake()
    {
        CurHp = MaxHp;
        sr = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();

    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void TriggerEnter(hitType _type, Collider2D _collision)
    {
        if(_type == hitType.WallCheck && _collision.gameObject.tag == GameTag.Object.ToString())
        {
            Vector3 scale = transform.lossyScale;
            scale.x *= -1;
            transform.localScale = scale;
            moveSpeed *= -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        moving();
    }

    private void moving()
    {
        rigid.velocity = new Vector2(moveSpeed, rigid.velocity.y);
    }

    private void FixedUpdate()
    {
        if (trigger.IsTouchingLayers(ground) == false)//���� �ƴ϶�� ��
        {
            turn();
        }
    }

    private void turn()
    {
        Vector3 sclae = transform.lossyScale;
        sclae.x *= -1;
        transform.localScale = sclae;

        moveSpeed *= -1;
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
