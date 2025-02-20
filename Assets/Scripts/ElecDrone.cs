using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElecDrone : EnemyCtrl
{
    [SerializeField] float distance, chaseDistance, atkDistance;
    

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(player.transform.position, transform.position);
        
        if (distance < atkDistance )
        {
            ChargeAtk();
        }
        else if (distance < chaseDistance)
        {
            Chase();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        Debug.Log("Patrol");
    }

    void Chase()
    {
        Debug.Log("Chase");
    }

    void ChargeAtk()
    {
        Debug.Log("ChargeAtk");
    }
}
