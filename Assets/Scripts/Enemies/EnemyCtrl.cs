using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
    [SerializeField] protected GameObject player;
    [SerializeField] protected GameObject sExplosionFX;

    [SerializeField] protected float  distance;

    [SerializeField] protected int hp;
    [SerializeField] protected float spd;
    public int damage;

    protected Animator anim;
        
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    public virtual void Update()
    {  

    }

    public void TakeHit(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        anim.SetTrigger("Die");
    }
}
