using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    [SerializeField] DynamicJoystick leftJoystick, rightJoystick;

    private Rigidbody2D rb;
    private Animator anim, weaponAnim;

    [SerializeField] private GameObject weapon, shotPos, smokeFX,  sparkFX;
    [SerializeField] float shotTimer;
    private float currentShotTimer = 0f;
    [SerializeField] SpriteRenderer aim;
    
    [SerializeField] protected int hp, damage;
    [SerializeField] float maxSpd;
    
    
    private bool canMove = true, isFliped = false, isAiming = false;
    [SerializeField] LayerMask ignoredLayers;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        weaponAnim = weapon.GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float inputX = leftJoystick.Horizontal;
        float inputY = leftJoystick.Vertical;

        Vector2 direction = new Vector2(inputX, inputY);

        float currentSpd = Mathf.Lerp(0, maxSpd, direction.magnitude);

        if (canMove)
        {
           if(isAiming)
           {
                rb.velocity = direction * currentSpd / 2;
           }
           else
           {
                rb.velocity = direction * currentSpd;
           }
        }
    }

    public void TakeShock(int damage)
    {
        TakeHit(damage);
    }

    public void TakeHit(int damage)
    {
        Debug.Log("Deu dano");

        anim.SetTrigger("electrocute");
        canMove = false;

        hp -= damage;

        if (hp <= 0)
        {
            Die();
        }
    }

    public void ReturnMove()
    {
        canMove = true;
    }

    private void Die()
    {
        Debug.Log("F");
        //anim.SetTrigger("die");
    }
}

