using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    public float topSpeed;
    public float acceleration;
    public float railGridingAcceleration;
    public float jumpHeight;
    public bool grounded;
    public float playerHeight;
    public LayerMask ground;
    public float maxJump;
    public float minJump;
    private bool trackMouse;
    private Vector3 lastPosition;
    private float mouseDistance;
    public bool railGrinding;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        
    }
    private void Update()
    {
   
        if (transform.position.y < -10)
        {
            transform.position = new Vector3(0.0f,10.0f,0.0f);
        }
       
        grounded = Physics2D.Raycast(transform.position, Vector2.down, playerHeight, ground);
        if (Input.GetMouseButton(1))
        {
            gameObject.layer = 10;
        }
        else
        {
            gameObject.layer = 0;

        }

        if (Input.GetButtonDown("Fire1"))
        {
            trackMouse = true;
            lastPosition = Input.mousePosition;
        }

        if (Input.GetButtonUp("Fire1"))
        {
            trackMouse = false;
            float jumpMultiplier = (mouseDistance * 0.01f);
            float fullJump = jumpHeight * jumpMultiplier;
            fullJump = Mathf.Clamp(fullJump, minJump, maxJump);
            rb.AddForce(Vector2.up * fullJump, ForceMode2D.Impulse);
            Debug.Log(fullJump);
            Debug.Log("Mouse moved " + mouseDistance + " while button was down.");
            mouseDistance = 0.0f;
        }

        if (trackMouse)
        {
            Vector3 newPosition = Input.mousePosition;
            mouseDistance += Mathf.Abs(newPosition.y - lastPosition.y);
            //mouseDistance += (newPosition - lastPosition).magnitude;
            lastPosition = newPosition;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (rb.velocity.magnitude < topSpeed)
        {
            if (railGrinding)
            {

                rb.AddForce(Vector2.right * railGridingAcceleration * Time.fixedDeltaTime, ForceMode2D.Force);
            }
            else
            {
                rb.AddForce(Vector2.right * acceleration * Time.fixedDeltaTime, ForceMode2D.Force);

            }
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Rail")
        {
            railGrinding = true;
        }
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Rail")
        {
            railGrinding = false;
        }

    }

}
