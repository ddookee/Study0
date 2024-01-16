using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum hitType
    {
        HitCheck,
        EnemyCheck,
        RotATKCheck,
    }

    //Animator anim;

    Rigidbody2D rigid;
    Vector3 moveDir;
    public Vector3 MoveDir
    {
        get => moveDir; //{return moveDir}
        set => moveDir = value; //{moveDir = value;}
    }
    public Vector3 GetMoveDir()
    {
        return moveDir;
    }

    [SerializeField] float moveSpeed = 5f;

    BoxCollider2D boxCollider2D;
    [Header("플레이어")]
    [SerializeField] bool isGround = false;
    private float verticalVelocity;
    [SerializeField,Tooltip("땅에 떨어진 후 복구하는 힘")] float zeroVelocity = 2f;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float jumpForce = 5;
    private bool isJump = false;
    //플레이어의 체력
    [SerializeField] private float MaxHp = 3;
    private float CurHp;

    private SpriteRenderer sr;

    private Camera mainCam;

    //에임 회전할때 우측에 있는지 확인하기 위함
    private bool isPlayerLookAtRightDirection;
    Transform trsHand;
    [Header("투척공격")]
    [SerializeField] GameObject objThrowKnife;
    Transform trsThrowKnife;
    [SerializeField] Transform trsObjDynamic;
    [SerializeField] float throwlimit = 0.3f;
    float throwTimer = 0.0f;

    [Header("기본근접공격")]
    [SerializeField] private float attackDamage;
    //Collider2D attackColl;
    //private bool isAttack = false;

    [Header("회전 공격")]
    [SerializeField] private float RotATKDamage = 0;
    //Collider2D RotATKColl;
    //private bool isRotATK = false;
    int count = 0;

    PlayerAnim playerAnim;




    private void OnDrawGizmos()
    {
        if (boxCollider2D != null)
        {
            Gizmos.color = Color.red;
            Vector3 pos = boxCollider2D.bounds.center - new Vector3(0, 0.1f, 0);
            Gizmos.DrawWireCube(pos, boxCollider2D.bounds.size);
        }
    }

    private void Awake()
    {
        //플레이어의 체력
        CurHp = MaxHp;

        rigid = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        trsHand = transform.GetChild(0);
        sr = GetComponent<SpriteRenderer>();
        playerAnim = GetComponentInChildren<PlayerAnim>();
        //anim = GetComponent<Animator>();

        //플레이어의 공격
        Transform childPlayerAnim = transform.Find("PlayerAnim");

        trsThrowKnife = childPlayerAnim.Find("ThrowPos");

        //Transform childAttack = childPlayerAnim.Find("AttackPos");
        //attackColl = childAttack.GetComponent<BoxCollider2D>();
        ////회전공격
        //Transform childRotATK = childPlayerAnim.Find("RotateATKPos");
        //RotATKColl = childRotATK.GetComponent<BoxCollider2D>();

    }


    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

        //doAnimation();

        moving();
        checkGround();

        checkJumping();

        checkGravity();

        checkMouse();

    }


    public void TriggerEnter(hitType _type, Collider2D _collision)
    {
        //적에게 닿으면 대미지를 받음
        if (_type == hitType.EnemyCheck && _collision.gameObject.tag == GameTag.Enemy.ToString())
        {
            Enemy enemySc = _collision.gameObject.GetComponent<Enemy>();
            CurHp -= enemySc.damage;
            if (CurHp <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                playerAnim.Hit();//PlayerAnim에게 대미지를 받았으니 색을 바꾸라 명령
            }
        }

        //적에게 근접공격으로 인한 대미지를 입힘
        else if (_type == hitType.HitCheck && _collision.gameObject.tag == GameTag.Enemy.ToString())
        {
            Enemy enemySc = _collision.GetComponent<Enemy>();
            enemySc.Hit(attackDamage);
        }

        //회전 공격의 의한 대미지를 입힘
        else if (_type == hitType.RotATKCheck && _collision.gameObject.tag == GameTag.Enemy.ToString())
        {
            Enemy enemySc = _collision.GetComponent<Enemy>();
            enemySc.Hit(RotATKDamage);
        }
    }

    

    //private void doAnimation()
    //{
    //    anim.SetBool("IsGround", isGround);
    //    anim.SetInteger("Horizontal", (int)moveDir.x);

    //    int curHorizontal = anim.GetInteger("Horizontal");


    //    //근접공격 애니메이션
    //    anim.SetBool("Attack", isAttack);

    //    if (Input.GetKeyDown(KeyCode.Mouse0))
    //    {
    //        isAttack = true;
    //    }



    //    //회전하며 공격하는 애니메이션
    //    anim.SetBool("RotateATK", isRotATK);
    //    if (Input.GetKey(KeyCode.E))
    //    {
    //        isRotATK = true;

    //    }
    //}

    /// <summary>
    /// 캐릭터의 움직임
    /// </summary>
    private void moving()
    {
        moveDir.x = Input.GetAxisRaw("Horizontal") * moveSpeed;
        moveDir.y = rigid.velocity.y;
        rigid.velocity = moveDir;
    }

    /// <summary>
    /// 땅에 닿아있는지 체크
    /// </summary>
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

    private void checkJumping()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround == true)
        {
            isJump = true;
        }

    }
    /// <summary>
    /// 중력체크
    /// </summary>
    private void checkGravity()
    {
        if (isGround == false)
        {
            verticalVelocity -= gravity * Time.deltaTime;
            if (verticalVelocity < -10.0f)
            {
                verticalVelocity = -10;
            }
        }

        else if (isGround == true)
        {
            if (verticalVelocity < 0)//받는 힘이 음수일때 천천히 0으로 만들어줌
            { 
                verticalVelocity += Time.deltaTime * zeroVelocity;
            }
            else//음수가 아니라면 0
            { 
                verticalVelocity = 0; 
            }
        }

        //점프
        if (isJump == true)
        {
            isJump = false;
            verticalVelocity = jumpForce;
        }
        rigid.velocity = new Vector2(rigid.velocity.x, verticalVelocity);
    }

    /// <summary>
    /// 마우스위치에 따른 캐릭터 방향전환
    /// </summary>
    private void checkMouse()
    {
        //마우스의 위치 가져오기
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -mainCam.transform.position.z;
        Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(mousePos);


        //마우스의 좌우 확인
        //우측을 바라보는
        Vector3 distanceMouseToPlayer = mouseWorldPos - transform.position;
        if (distanceMouseToPlayer.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            isPlayerLookAtRightDirection = true;
        }

        //좌측을 바라보는
        else if (distanceMouseToPlayer.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            isPlayerLookAtRightDirection = false;
        }

        Vector3 direction = Vector3.right;
        if (isPlayerLookAtRightDirection == false)
        {
            direction = Vector3.left;
        }

        float angle = Quaternion.FromToRotation(direction, distanceMouseToPlayer).eulerAngles.z;
        if (isPlayerLookAtRightDirection == true)
        {
            angle = -angle;
        }
        trsHand.localEulerAngles = new Vector3(trsHand.localEulerAngles.x,
            trsHand.localEulerAngles.y, angle);


        if (throwTimer != 0.0f)
        {
            throwTimer -= Time.deltaTime;
            if (throwTimer < 0.0f)
            {
                throwTimer = 0.0f;
            }
        }

        //나이프 던짐
        if (/*GamePause == false &&*/ Input.GetKeyDown(KeyCode.Q) && throwTimer == 0.0f)
        {
            throwKnife();
            throwTimer = throwlimit;
        }
    }

    /// <summary>
    /// 칼을 던지는 스킬
    /// </summary>
    private void throwKnife()
    {
        Vector3 rot = trsThrowKnife.localRotation.eulerAngles;
        rot.z += -90;
        if (isPlayerLookAtRightDirection == false)
        {
            rot.z = +90;
        }
        GameObject obj = Instantiate(objThrowKnife, trsThrowKnife.position, Quaternion.Euler(rot), trsObjDynamic);
        ThrowKnife sc = obj.GetComponent<ThrowKnife>();
        Vector2 throwForce = new Vector2(10.0f, 0f);// * Time.deltaTime *speed;
        if (isPlayerLookAtRightDirection == false)
        {
            throwForce = new Vector2(-10.0f, 0f);// * Time.deltaTime * speed;
            //throwForce.x = -10.0f;

        }

        sc.SetForce(trsThrowKnife.rotation * throwForce, isPlayerLookAtRightDirection);
    }



    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == GameTag.Enemy.ToString())
    //    {
    //        Enemy enemySc = collision.GetComponent<Enemy>();
    //        enemySc.Hit(attackDamage);
    //    }
    //}

    /// <summary>
    /// 기본공격 콜라이더 활성화
    /// </summary>
    //private void onAttack()
    //{
    //    //기본공격
    //    attackColl.enabled = true;
    //}
    /// <summary>
    /// 회전공격 콜라이더 활성화
    /// </summary>
    //private void onRotATK()
    //{
    //    //회전공격
    //    RotATKColl.enabled = true;

    //}

    /// <summary>
    /// 기본공격 콜라이더 비활성화
    /// </summary>
    //private void offAttack()
    //{
    //    //기본공격
    //    attackColl.enabled = false;

    //    //Attack 애니메이션 false로 만들어줌
    //    isAttack = false;

    //}
    /// <summary>
    /// 회전 공격 콜라이더 비활성화
    /// </summary>
    //private void offRotATK()
    //{
    //    //회전공격
    //    RotATKColl.enabled = false;

    //    //회전공격 애니메이션을 false로
    //    count++;
    //    rotCase();
    //}
    //private void rotCase()
    //{
    //    Debug.Log(count);
    //    switch (count)
    //    {
    //        case 1:
    //            {
    //                if (Input.GetKey(KeyCode.E))
    //                {
    //                    isRotATK = true;
    //                }
                    
    //            }
    //            break;
    //        case 2:
    //            {
    //                if (Input.GetKey(KeyCode.E))
    //                {
    //                    isRotATK = true;
    //                }
    //            }
    //            break;
    //        case 3:
    //            {
    //                //Input.GetKey(KeyCode.E);
    //                isRotATK = false;
    //                count = 0;
    //            }
    //            break;
    //        default:
    //            break;
    //    }
    //}
}
