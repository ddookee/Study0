using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcTalk : MonoBehaviour
{

    private Transform trsNpc;

    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        GameManager manager = GameManager.instance;
        GameObject obj = manager.GetNpcGameObject();
        trsNpc = obj.transform;
    }

    // Update is called once per frame
    void Update()
    {
        checkNpcPos();
    }

    private void checkNpcPos()
    {
        transform.position = trsNpc.position + new Vector3(1f, 0.65f, 0);

    }
}
