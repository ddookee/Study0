using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayer : MonoBehaviour
{
    [SerializeField] GameObject objPlayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (objPlayer == null) return;

        Vector3 pos = objPlayer.transform.position;
        pos.z = -10;
        transform.position = pos;
    }
}
