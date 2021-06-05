using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public float movementSpeed = 5;
    public float jumpForce;
    private Rigidbody2D body;
    private bool facingRight = true;
    private bool isJumping = false;
    public Animator animator;
    private Vector3 checkpoint = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        Restart();
    }

    //Permet de deplacer le personnage et de le faire courir
    private void Move()
    {
        var movement = Input.GetAxis("Horizontal");
        transform.position = transform.position + new Vector3(movement, 0, 0) * Time.deltaTime * movementSpeed;
        animator.SetInteger("movementSpeed", (int)Input.GetAxisRaw("Horizontal"));

    }

    //Fait sauter le personnage et permet a l'animator de jouer le sprite correspondant
    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && Mathf.Abs(body.velocity.y) < 0.0001f)
        {
            body.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }

        if (Mathf.Abs(body.velocity.y) > 0.0001f)
        {
            isJumping = true;
        }

        animator.SetBool("isJumping", isJumping);
        isJumping = false;
    }

    //Check le cote ou se situe le personnage et le fait tourner si besoin
    private void Turn()
    {
        if ((Input.GetAxis("Horizontal") < 0) && facingRight)
        {
            transform.Rotate(0, -180, 0);
            facingRight = false;

        }
        else if ((Input.GetAxis("Horizontal") > 0) && !facingRight)
        {
            transform.Rotate(0, -180, 0);
            facingRight = true;
        }
    }

    //Teleporte le joueur a "nouvellePosition"
    private void changePosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    //Reset la position du personnage a 0
    private void Restart()
    {
        animator.Play("blueRestart");
        changePosition(checkpoint);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log(col.collider);
        //Si on rentre en contact avec une scie
        if (col.collider.name == "Saw")
        {
            Restart();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision);
        checkpoint = collision.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        Turn();

        Jump();
    }
}
