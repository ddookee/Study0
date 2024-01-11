using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
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


    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        player = GetComponentInParent<Player>();

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
        RotATKColl.enabled = true;

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
        if (isRotATK == false)
        {
            RotATKColl.enabled = false;

        }
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
                    //Input.GetKey(KeyCode.E);
                    isRotATK = false;
                    count = 0;
                }
                break;
            default:
                break;
        }
    }
}
