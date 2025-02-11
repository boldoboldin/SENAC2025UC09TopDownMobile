using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    [SerializeField] DynamicJoystick dynamicJoystick;

    [SerializeField] float maxSpd;

    private Rigidbody2D rb;
    private Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = dynamicJoystick.Horizontal;
        float inputY = dynamicJoystick.Vertical;

        Vector2 direction = new Vector2 (inputX, inputY);

        float currentSpd = Mathf.Lerp (0, maxSpd, direction.magnitude);

        rb.velocity = direction * currentSpd;

        anim.SetFloat("moveInput", currentSpd); 

        if(inputX > 0)
        {
            transform.localScale= new (-1, 1);
        }

        if(inputX < 0)
        {
            transform.localScale= new (1, 1);
        }
    }
}
