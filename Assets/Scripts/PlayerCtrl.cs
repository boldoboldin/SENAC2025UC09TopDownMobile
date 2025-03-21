using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    [SerializeField] DynamicJoystick leftJoystick, rightJoystick;

    private Rigidbody2D rb;
    private Animator anim, weaponAnim;

    [SerializeField] private GameObject weapon, shotPos, shotFX, sparkFX, smokeFX;

    [SerializeField] private int hp;
    [SerializeField] float maxSpd, shotTimer;
    [SerializeField] SpriteRenderer aim;
    public float currentShotTimer = 0f;
    public int damage;

    [SerializeField] private bool isFliped = false, isAiming = false, canMove = true;

    public LayerMask ignoreLayer;

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
        WeaponMove();
        Move();
    }

    private void Move()
    {
        float inputX = leftJoystick.Horizontal;
        float inputY = leftJoystick.Vertical;

        Vector2 direction = new Vector2(inputX, inputY);

        float currentSpd = Mathf.Lerp(0, maxSpd, direction.magnitude);

        if (canMove == true)
        {
            if (isAiming)
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

        if (!isAiming)
        {
            if (inputX > 0)
            {
                transform.localScale = new(-1, 1);
                isFliped = true;
            }
            
            if (inputX < 0)
            {
                transform.localScale = new(1, 1);
                isFliped = false;
            }
        }   
    }

    private void WeaponMove()
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

            if (currentShotTimer <= shotTimer)
            {
                currentShotTimer += 1 * Time.deltaTime;
            }
            else
            {
                Shoot();
                currentShotTimer = 0f;
            }

            if (inputX > 0)
            {
                transform.localScale = new(-1, 1);
                isFliped = true;
            }
            else
            {
                transform.localScale = new(1, 1);
                isFliped = false;
            }

            Vector2 aimrDirection = isFliped ? weapon.transform.right : -weapon.transform.right;
            RaycastHit2D hit = Physics2D.Raycast(weapon.transform.position, aimrDirection, 9f, ~ignoreLayer);

            if (hit.collider != null)
            {
                float distance = hit.distance; 
                AdjustAimWidth(distance);
            }
            else
            {
                AdjustAimWidth(9f);
            }
        }
        else
        {
            isAiming = false;
        }
    }

    private void AdjustAimWidth(float distance)
    {
        float currentWidth = Mathf.Clamp(Mathf.Abs(distance), 0.5f, 9f);
        aim.size = new Vector2(currentWidth, 0.0625f);
    }

    private void Shoot()
    {
        Vector2 direction = isFliped ? weapon.transform.right : -weapon.transform.right;
        RaycastHit2D hit = Physics2D.Raycast(weapon.transform.position, direction, 12f, ~ignoreLayer);

        if (hit.collider != null)
        {
            Debug.Log("Hit: " + hit.collider.name);

            if (hit.collider.CompareTag("Enemy"))
            {
                EnemyCtrl enemyCtrl = hit.collider.GetComponent<EnemyCtrl>();

                enemyCtrl.TakeHit(damage); 
            }

            Vector2 instantiatePos = new Vector2(hit.transform.position.x, hit.transform.position.y + 1f);

            GameObject _sparkFX = Instantiate(sparkFX, instantiatePos, Quaternion.identity);
            Destroy(_sparkFX, 1f);
        }

        //GameObject _shotFX = Instantiate(shotFX, shotPos.transform.position, Quaternion.identity);

        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //_shotFX.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        //Destroy(_shotFX, 1f);

        weaponAnim.SetTrigger("shoot");
    }

    public void TakeShock(int damage)
    {
        TakeHit(damage);
    }

    public void TakeHit(int damage)
    {
        anim.SetTrigger("electrocute");

        hp -= damage;

        if (hp <= 0)
        {
            Die();
        }
    }

    public void MoveCtrl(int canMove)
    {
        if(canMove == 0)
        {
            this.canMove = false;
        }
        else
        {
            this.canMove = true;
        }
    }

    private void Die()
    {
        //anim.SetTrigger("die");
    }
}

