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

    [SerializeField,Tooltip("Enemy이동속도")] float moveSpeed;
    public float MoveSpeed()
    {
        return moveSpeed;
    }
    [SerializeField] LayerMask ground;
    Rigidbody2D rigid;
    [SerializeField,Tooltip("자신앞에 땅이 있는지 없는지 체크하는 오브젝트")] Collider2D trigger;

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
        if (trigger.IsTouchingLayers(ground) == false)//땅이 아니라면 턴
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
