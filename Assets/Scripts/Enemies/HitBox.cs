using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    EnemyCtrl enemyCtrl;

    void Start()
    {
        enemyCtrl = GetComponentInParent<EnemyCtrl>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerCtrl playerCtrl = collision.GetComponent<PlayerCtrl>();
            playerCtrl.TakeHit(enemyCtrl.damage);
        }
    }
}
