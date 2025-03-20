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

           anim.SetFloat("moveInput", currentSpd);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        if(!isAiming)
        {
            if (inputX > 0)
            {
                transform.localScale = new Vector2(-1, 1);
                isFliped = true;
            }
            else
            {
                transform.localScale = new Vector2(1, 1);
                isFliped = false;
            }

            Vector2 aimDirection = isFliped ? weapon.transform.right : -weapon.transform.right;
            RaycastHit2D hit = Physics2D.Raycast(weapon.transform.position, aimDirection, 9f, ~ignoredLayers);

            if (hit.collider != null)
            {
                float distance = hit.distance;
                //Ajustar mira
            }
        }
        else
        {
            isAiming = false;
        }
    }

    void Shoot()
    {
        Vector2 direction = isFliped ?  weapon.transform.right : -weapon.transform.right;
        RaycastHit2D hit = Physics2D.Raycast (weapon.transform.position, direction, Mathf.Infinity, ~ignoredLayers);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                EnemyCtrl enemyCtrl = hit.collider.GetComponent<EnemyCtrl>();
                enemyCtrl.TakeHit(damage);
            }

            Vector2 instatiatePos = new Vector2(hit.transform.position.x, 
            hit.transform.position.y + 1f);

            GameObject _sparkFX = Instantiate(sparkFX, instatiatePos, 
            Quaternion.identity);
            Destroy (_sparkFX);
        }

        //weaponAnim.SetTrigger("shoot");
    }

    void AdjustAimWidth (float distance)
    {
        float currentWidth = Mathf.Clamp(Mathf.Abs(distance), 3f, 9f);
        aim.size = new Vector2(currentWidth, 0.0625f);
    }

    void WeaponMove()
    {
        float inputX = rightJoystick.Horizontal;
        float inputY = rightJoystick.Vertical;

        Vector2 direction = isFliped ? new Vector2(inputX, inputY) : new Vector2(-inputX, -inputY);
    
    
        if (direction.magnitude > 0)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            weapon.transform.rotation = Quaternion.Slerp(weapon.transform.rotation, Quaternion.Euler(0, 0, angle), maxSpd * 2 * Time.deltaTime);
        }

        if (inputX != 0 && inputY != 0)
        {
            isAiming = true;

            if(currentShotTimer <= shotTimer)
            {
                currentShotTimer += 1 * Time.deltaTime;
            }
            else
            {
                // atirar aqui
                currentShotTimer = 9;
            }

            if (inputX > 0)
            {
                transform.localScale = new Vector2(-1, 1);
                isFliped = true;
            }
            else
            {
                transform.localScale = new Vector2(1, 1);
                isFliped = false;
            }
        }
    }

PrimitiveType
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

