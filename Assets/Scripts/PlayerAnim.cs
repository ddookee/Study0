using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;

public class PlayerAnim : MonoBehaviour
{
    private SpriteRenderer sr;
    Animator anim;
    BoxCollider2D boxCollider2D;
    [SerializeField] private bool isGround = false;
    Player player;
    private float verticalVelocity;

    //기본근접 공격
    Collider2D attackColl;
    private bool isAttack = false;

    //회전공격
    Collider2D RotATKColl;
    private bool isRotATK = false;
    int count = 0;

    float timerRotAtk = 0.0f;
    [Header ("회전공격 쿨타임")]
    [SerializeField] float timerLimitRotAtk = 0.1f;

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if(gameObject.tag == GameTag.Enemy.ToString())
    //    {
    //        sr.color = new Color(1, 0.5f, 0.5f, 1);
    //        Invoke("backSprite", 0.3f);
    //    }
    //}
    public void Hit()//플레이어가 대미지를 받으면 색이 변하게 전달
    {
        sr.color = new Color(1, 0.5f, 0.5f, 1);
        Invoke("backSprite", 0.3f);
    }
    private void backSprite()
    {
        sr.color = Color.white;
    }


    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        player = GetComponentInParent<Player>();
        sr = GetComponent<SpriteRenderer>();

        //플레이어의 공격
        //Transform childPlayerAnim = transform.Find("PlayerAnim");

        Transform childAttack = transform.Find("AttackPos");
        attackColl = childAttack.GetComponent<BoxCollider2D>();
        //회전공격
        Transform childRotATK = transform.Find("RotateATKPos");
        RotATKColl = childRotATK.GetComponent<BoxCollider2D>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        doAnimation();

        checkGround();
        checkCooltime();
    }

    private void doAnimation()
    {
        anim.SetBool("IsGround", isGround);
        //anim.SetInteger("Horizontal", (int)player.MoveDir.x);
        anim.SetInteger("Horizontal", (int)player.GetMoveDir().x);

        //근접공격 애니메이션
        anim.SetBool("Attack", isAttack);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            isAttack = true;
        }

        //회전하며 공격하는 애니메이션
        anim.SetBool("RotateATK", isRotATK);
        if (Input.GetKey(KeyCode.E))
        {
            isRotATK = true;

        }

    }

    private void checkGround()
    {
        isGround = false;
        if (verticalVelocity > 0)
        {
            return;
        }
        //레이저를 쏴서 땅에 닿아있는지 체크
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f,
            Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        if (hit.transform != null && hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGround = true;
        }
    }

    /// <summary>
    /// 기본공격 콜라이더 활성화
    /// </summary>
    private void onAttack()
    {
        //기본공격
        attackColl.enabled = true;
    }

    /// <summary>
    /// 회전공격 콜라이더 활성화
    /// </summary>
    private void onRotATK()
    {
        //회전공격
        if (count == 0)
        { 
            RotATKColl.enabled = true;
        }

    }

    /// <summary>
    /// 기본공격 콜라이더 비활성화
    /// </summary>
    private void offAttack()
    {
        //기본공격
        attackColl.enabled = false;

        //Attack 애니메이션 false로 만들어줌
        isAttack = false;

    }

    /// <summary>
    /// 회전 공격 콜라이더 비활성화
    /// </summary>
    private void offRotATK()
    {
        //회전공격
        count++;
        rotCase();

        //회전공격 애니메이션을 false로
    }

    /// <summary>
    /// 회전공격 횟수 추가
    /// </summary>
    private void rotCase()
    {
        Debug.Log(count);
        switch (count)
        {
            case 1:
                {
                    if (Input.GetKey(KeyCode.E))
                    {
                        isRotATK = true;
                    }

                }
                break;
            case 2:
                {
                    if (Input.GetKey(KeyCode.E))
                    {
                        isRotATK = true;
                    }
                }
                break;
            case 3:
                {
                    isRotATK = false;
                    RotATKColl.enabled = false;//회전이 끝나고 콜라이더를 비활성화 해줌
                    timerRotAtk = timerLimitRotAtk;//쿨타임으로 바로 들어가지 않게 해줌
                }
                break;
            default:
                break;
        }
    }

    private void checkCooltime()
    {
        if (timerRotAtk > 0.0f)//타이머가 0보다 크면 점점 줄어듦
        {
            timerRotAtk -= Time.deltaTime;
            if (timerRotAtk < 0.0f)//타이머가 0보다 작으면 같게 만들어줌
            {
                timerRotAtk = 0.0f;
                count = 0;//카운트를 쿨타임이 끝났을때 0으로 만들어줌
            }
        }
    }
}
