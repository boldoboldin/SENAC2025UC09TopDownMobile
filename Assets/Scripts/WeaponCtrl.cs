using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCtrl : MonoBehaviour
{
    [SerializeField] DynamicJoystick rightJoystick;
    [SerializeField] private GameObject weapon, shotPos, shotFX, sparkFX, smokeFX;
    [SerializeField] private int damage;
    [SerializeField] private float shotTimer;
    private Animator weaponAnim;
    private bool isAiming = false;
    private float currentShotTimer = 0f;
    private bool isFliped = false;

    [SerializeField] private LayerMask ignoreLayer;
    [SerializeField] private SpriteRenderer aim;

    // Start is called before the first frame update
    void Start()
    {
        weaponAnim = weapon.GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        WeaponMove();
    }

    private void WeaponMove()
    {
        float inputX = rightJoystick.Horizontal;
        float inputY = rightJoystick.Vertical;

        Vector2 direction = isFliped ? new Vector2(inputX, inputY) : new Vector2(-inputX, -inputY);

        if (direction.magnitude > 0)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            weapon.transform.rotation = Quaternion.Slerp(weapon.transform.rotation, Quaternion.Euler(0, 0, angle), 5f * Time.deltaTime);
        }

        if (inputX != 0 && inputY != 0)
        {
            isAiming = true;

            if (currentShotTimer <= shotTimer)
            {
                currentShotTimer += Time.deltaTime;
            }
            else
            {
                Shoot();
                currentShotTimer = 0f;
            }

            if (inputX > 0)
            {
                isFliped = true;
            }
            else if (inputX < 0)
            {
                isFliped = false;
            }

            Vector2 aimDirection = isFliped ? weapon.transform.right : -weapon.transform.right;

            RaycastHit2D hit = Physics2D.Raycast(weapon.transform.position, aimDirection, 9f, ~ignoreLayer);
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
        float currentWidth = Mathf.Abs(distance);
        aim.size = new Vector2(currentWidth, 0.0625f);
    }

    private void Shoot()
    {
        Vector2 direction = isFliped ? weapon.transform.right : -weapon.transform.right;

        GameObject _shotFX = Instantiate(shotFX, shotPos.transform.position, Quaternion.identity);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _shotFX.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        Destroy(_shotFX, 1f);

        weaponAnim.SetTrigger("shoot");

        RaycastHit2D hit = Physics2D.Raycast(weapon.transform.position, direction, Mathf.Infinity, ~ignoreLayer);

        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            EnemyCtrl enemyCtrl = hit.collider.GetComponent<EnemyCtrl>();
            enemyCtrl.TakeHit(damage);

            Vector2 instantiatePos = hit.transform.position + Vector3.up;
            GameObject _sparkFX = Instantiate(sparkFX, instantiatePos, Quaternion.identity);
            Destroy(_sparkFX, 1f);
        }
    }
}
