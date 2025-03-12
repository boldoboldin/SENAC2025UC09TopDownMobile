using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineDrone : EnemyCtrl
{
    [SerializeField] private float explosionDistance;

    // Update is called once per frame
    public override void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < explosionDistance)
        {
            ActivateAtk();
        }
    }

    void ActivateAtk()
    {
        anim.SetTrigger("Activate");
    }

    void Explode()
    {
        Vector2 instatePos = new Vector2(transform.position.x, transform.position.y + 1f);

        GameObject fx = Instantiate (sExplosionFX, instatePos, transform.rotation);
        Destroy(fx, 3f);
    }
}
