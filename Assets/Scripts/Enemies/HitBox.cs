using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    EnemyCtrl enemyCtrl;

    [SerializeField] bool isExplosion;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        enemyCtrl = GetComponentInParent<EnemyCtrl>();

        if (isExplosion)
        {
            if (collision.CompareTag("Enemy"))
            {
                EnemyCtrl otherEnemyCtrl = collision.GetComponent<EnemyCtrl>();
                otherEnemyCtrl.TakeHit(enemyCtrl.damage);
            }
        }

        if (collision.CompareTag("Player"))
        {
            PlayerCtrl playerCtrl = collision.GetComponent<PlayerCtrl>();
            playerCtrl.TakeHit(enemyCtrl.damage);
        }
    }
}
