using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator anim;

    Rigidbody2D rigid;
    Vector3 moveDir;
    [SerializeField] float moveSpeed = 5f;

    BoxCollider2D boxCollider2D;
    [Header("플레이어")]
    [SerializeField] bool isGround = false;
    private float verticalVelocity;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float jumpForce = 5;
    private bool isJump = false;

    private Camera mainCam;

    //에임 회전할때 우측에 있는지 확인하기 위함
    private bool isPlayerLookAtRightDirection;
    Transform trsHand;
    [Header("투척공격")]
    [SerializeField] GameObject objThrowKnife;
    Transform trsThrowKnife;
    [SerializeField] Transform trsObjDynamic;

    Collider2D attackColl;
    private bool isAttack = false;



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
        rigid = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        trsHand = transform.GetChild(0);
        anim = GetComponent<Animator>();
        trsThrowKnife = transform.Find("ThrowPos");
        Transform childAttack = transform.Find("AttackPos");
        attackColl = childAttack.GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        doAnimation();

        moving();
        checkGround();

        checkJumping();

        checkGravity();

        checkMouse();

    }

    private void doAnimation()
    {
        anim.SetBool("IsGround", isGround);
        anim.SetInteger("Horizontal", (int)moveDir.x);

        int curHorizontal = anim.GetInteger("Horizontal");


        //근접공격 애니메이션
        anim.SetBool("Attack", isAttack);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            isAttack = true;
        }
    }

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
        if(verticalVelocity > 0)
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
        if(Input.GetKeyDown(KeyCode.Space) && isGround == true)
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
            if(verticalVelocity < -10.0f)
            {
                verticalVelocity = -10;
            }
        }

        else if (isGround == true)
        {
            verticalVelocity = 0;
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
        if(distanceMouseToPlayer.x > 0)
        {
            transform.localScale = new Vector3(-1,1,1);
            isPlayerLookAtRightDirection = true;
        }

        //좌측을 바라보는
        else if(distanceMouseToPlayer.x < 0)
        {
            transform.localScale = new Vector3(1,1,1);
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

        //나이프 던짐
        if (/*GamePause == false &&*/ Input.GetKeyDown(KeyCode.Q))
        {
            throwKnife();
            //throwTimer = throwlimit;
        }
    }

    /// <summary>
    /// 칼을 던지는 스킬
    /// </summary>
    private void throwKnife()
    {
        Vector3 rot = trsThrowKnife.localRotation.eulerAngles;
        rot.z += -90;
        if(isPlayerLookAtRightDirection == false)
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

    private void attack()
    {

    }
    
    private void onAttack()
    {
        attackColl.enabled = true;
    }

    private void offAttack()
    {
        attackColl.enabled = false;
    }
}
