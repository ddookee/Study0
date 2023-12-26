using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowKnife : MonoBehaviour
{
    [SerializeField] private float speed;

    Rigidbody2D rigid;
    Vector2 force;
    bool isRight;
    Transform trsKnife;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rigid.AddForce(force, ForceMode2D.Force);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * Time.deltaTime * speed;
    }

    public void SetForce(Vector2 _force, bool _isRight)
    {
        force = _force;
        isRight = _isRight;
    }
}
