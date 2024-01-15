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

    //�⺻���� ����
    Collider2D attackColl;
    private bool isAttack = false;

    //ȸ������
    Collider2D RotATKColl;
    private bool isRotATK = false;
    int count = 0;

    float timerRotAtk = 0.0f;
    [Header ("ȸ������ ��Ÿ��")]
    [SerializeField] float timerLimitRotAtk = 0.1f;

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if(gameObject.tag == GameTag.Enemy.ToString())
    //    {
    //        sr.color = new Color(1, 0.5f, 0.5f, 1);
    //        Invoke("backSprite", 0.3f);
    //    }
    //}
    public void Hit()//�÷��̾ ������� ������ ���� ���ϰ� ����
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

        //�÷��̾��� ����
        //Transform childPlayerAnim = transform.Find("PlayerAnim");

        Transform childAttack = transform.Find("AttackPos");
        attackColl = childAttack.GetComponent<BoxCollider2D>();
        //ȸ������
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

        //�������� �ִϸ��̼�
        anim.SetBool("Attack", isAttack);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            isAttack = true;
        }

        //ȸ���ϸ� �����ϴ� �ִϸ��̼�
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
        //�������� ���� ���� ����ִ��� üũ
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f,
            Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        if (hit.transform != null && hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGround = true;
        }
    }

    /// <summary>
    /// �⺻���� �ݶ��̴� Ȱ��ȭ
    /// </summary>
    private void onAttack()
    {
        //�⺻����
        attackColl.enabled = true;
    }

    /// <summary>
    /// ȸ������ �ݶ��̴� Ȱ��ȭ
    /// </summary>
    private void onRotATK()
    {
        //ȸ������
        if (count == 0)
        { 
            RotATKColl.enabled = true;
        }

    }

    /// <summary>
    /// �⺻���� �ݶ��̴� ��Ȱ��ȭ
    /// </summary>
    private void offAttack()
    {
        //�⺻����
        attackColl.enabled = false;

        //Attack �ִϸ��̼� false�� �������
        isAttack = false;

    }

    /// <summary>
    /// ȸ�� ���� �ݶ��̴� ��Ȱ��ȭ
    /// </summary>
    private void offRotATK()
    {
        //ȸ������
        count++;
        rotCase();

        //ȸ������ �ִϸ��̼��� false��
    }

    /// <summary>
    /// ȸ������ Ƚ�� �߰�
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
                    RotATKColl.enabled = false;//ȸ���� ������ �ݶ��̴��� ��Ȱ��ȭ ����
                    timerRotAtk = timerLimitRotAtk;//��Ÿ������ �ٷ� ���� �ʰ� ����
                }
                break;
            default:
                break;
        }
    }

    private void checkCooltime()
    {
        if (timerRotAtk > 0.0f)//Ÿ�̸Ӱ� 0���� ũ�� ���� �پ��
        {
            timerRotAtk -= Time.deltaTime;
            if (timerRotAtk < 0.0f)//Ÿ�̸Ӱ� 0���� ������ ���� �������
            {
                timerRotAtk = 0.0f;
                count = 0;//ī��Ʈ�� ��Ÿ���� �������� 0���� �������
            }
        }
    }
}
