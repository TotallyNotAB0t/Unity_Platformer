using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public float movementSpeed = 5;
    public float jumpForce;
    private Rigidbody2D body;
    private Rigidbody2D enemyBody;
    private bool facingRight = true;
    private bool isJumping = false;
    public Animator animator;
    private Vector3 checkpoint = new Vector3(0, 0, 0);
    private bool canMove = true;
    bool enemyTurned = false;
    //private int Lives = 3;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        enemyBody = GameObject.Find("FrogEnemy").GetComponent<Rigidbody2D>();
        Restart();
    }

    //Permet de deplacer le personnage et de le faire courir
    private void Move()
    {
        var movement = Input.GetAxis("Horizontal");
        body.transform.position = body.transform.position + movementSpeed * Time.deltaTime * new Vector3(movement, 0, 0);
        animator.SetInteger("movementSpeed", (int)Input.GetAxisRaw("Horizontal"));

    }

    private void PlayerMovement()
    {
        if (canMove)
        {
            Move();
            Turn(body);
            Jump();
        }
    }

    //Ennemi bouge et tourne
    private void EnemyMovement()
    {
        if (!enemyTurned)
        {
            enemyBody.transform.position = enemyBody.transform.position + movementSpeed * Time.deltaTime * new Vector3(0.5f, 0, 0);
        }
        else
        {
            enemyBody.transform.position = enemyBody.transform.position + movementSpeed * Time.deltaTime * new Vector3(-0.5f, 0, 0);
        }
        

        if (enemyBody.transform.position.x > 10 && !enemyTurned)
        {
            enemyBody.transform.Rotate(0, -180, 0);
            enemyTurned = true;
        }
        else if (enemyBody.transform.position.x < 7 && enemyTurned)
        {
            enemyBody.transform.Rotate(0, -180, 0);
            enemyTurned = false;
        }

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

    //Joue l'animation de mort et enclenche la fonction Restart (event de l'animator)
    private void Die()
    {
        animator.Play("blueDie");
        NotMove();
    }

    //body.constraints ne marche pas sur l'axe X donc j'utilise un boolean
    private void NotMove()
    {
        canMove = false;
    }

    private void MoveAgain()
    {
        canMove = true;
    }

    //Check le cote ou se situe le personnage et le fait tourner si besoin
    private void Turn(Rigidbody2D bod)
    {
        if ((Input.GetAxis("Horizontal") < 0) && facingRight)
        {
            bod.transform.Rotate(0, -180, 0);
            facingRight = false;

        }
        else if ((Input.GetAxis("Horizontal") > 0) && !facingRight)
        {
            bod.transform.Rotate(0, -180, 0);
            facingRight = true;
        }
    }

    //Teleporte le joueur a "nouvellePosition"
    private void ChangePosition(Rigidbody2D bod, Vector3 newPosition)
    {
        bod.transform.position = newPosition;
    }

    //Reset la position du personnage a 0
    private void Restart()
    {
        animator.Play("blueRestart");
        ChangePosition(body, checkpoint);
        MoveAgain();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log(col.collider);
        //Si on rentre en contact avec une scie ou un ennemi
        if (col.collider.name == "Saw" | col.collider.name == "FrogEnemy")
        {
            Die();
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
        EnemyMovement();
        PlayerMovement();
    }
}