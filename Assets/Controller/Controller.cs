using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Animations;

public class Controller : MonoBehaviour
{
    public float movementSpeed = 5;
    public float jumpForce;
    private Rigidbody2D body;
    private Rigidbody2D enemyBody;
    private bool facingRight = true;
    public Animator animator;
    private Vector3 checkpoint = new Vector3(0, 0, 0);
    private bool canMove = true;
    bool enemyTurned = false;
    private Vector3 PosEnemy;
    public GameObject[] hearts;
    private int lives = 2;
    public Animator heartReset;
    private bool canDie = false;
    private GameObject Platform;
    private Vector3 OriginPlatform;
    bool PlatformGoingUp = true;
    public Animator FruitAnimator;
    public Animation KiwiCollected;
    public TextAsset KiwiCount;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        enemyBody = GameObject.Find("FrogEnemy").GetComponent<Rigidbody2D>();
        PosEnemy = enemyBody.transform.position;
        Platform = GameObject.Find("Platform");
        OriginPlatform = Platform.transform.position;
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
            CharacterTurn();
            Jump();
        }
    }

    //Ennemi bouge et tourne
    private void EnemyMovement()
    {
        Vector3 xPos = enemyBody.transform.position;

        if (!enemyTurned)
        {
            enemyBody.transform.position = xPos + movementSpeed * Time.deltaTime * new Vector3(0.5f, 0, 0);
        }
        else
        {
            enemyBody.transform.position = xPos + movementSpeed * Time.deltaTime * new Vector3(-0.5f, 0, 0);
        }

        if (xPos.x > PosEnemy.x + 2 && !enemyTurned)
        {
            Turn(enemyBody);
            enemyTurned = true;
        }
        else if (xPos.x < PosEnemy.x - 2 && enemyTurned)
        {
            Turn(enemyBody);
            enemyTurned = false;
        }

    }

    //Permet de faire bouger une plateforme sur l'axe des Y
    private void PlatformMove()
    {
        if (Platform.transform.position.y < OriginPlatform.y + 1 && PlatformGoingUp)
        {
            Platform.transform.position = Platform.transform.position + Time.deltaTime * new Vector3(0, 1, 0);
        }
        else if (Platform.transform.position.y > OriginPlatform.y - 2)
        {
            PlatformGoingUp = false;
            Platform.transform.position = Platform.transform.position + Time.deltaTime * new Vector3(0, -1, 0);
        }
        else
        {
            PlatformGoingUp = true;
        }
        
    }

    //Fait sauter le personnage et permet a l'animator de jouer le sprite correspondant
    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && Mathf.Abs(body.velocity.y) < 0.0001f)
        {
            body.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            animator.SetBool("isJumping", true);
        }
        else if (Mathf.Abs(body.velocity.y) < 0.0001f)
        {
            animator.SetBool("isJumping", false);
        }
    }

    public void ReloadScene()
    {
        if (lives == 0)
        {
            SceneManager.LoadScene(1);
        }
    }

    //Animation et suppression des vies
    private void LoseHeart()
    {
        Animator heartAnimator = hearts[lives].GetComponent<Animator>();
        heartAnimator.Play("heart_brocken");

        if (lives > 0)
        {
            lives--;
        }
        else
        {
            canDie = true;
        }
    }

    //Joue l'animation de mort et enclenche la fonction Restart (event de l'animator)
    private void Die()
    {
        LoseHeart();
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
    private void CharacterTurn()
    {
        if ((Input.GetAxis("Horizontal") < 0) && facingRight)
        {
            Turn(body);
            facingRight = false;

        }
        else if ((Input.GetAxis("Horizontal") > 0) && !facingRight)
        {
            Turn(body);
            facingRight = true;
        }
    }

    //Permet de faire tourner le sprite d'un rigidbody
    private void Turn(Rigidbody2D bod)
    {
        bod.transform.Rotate(0, -180, 0);
    }

    //Teleporte un rigidbody a "nouvellePosition"
    private void ChangePosition(Rigidbody2D bod, Vector3 newPosition)
    {
        bod.transform.position = newPosition;
    }

    //Reset la position du personnage a 0
    private void Restart()
    {
        Debug.Log(lives);
        if (canDie)
        {
            ReloadScene();
        }
        else
        {
            animator.Play("blueRestart");
            ChangePosition(body, checkpoint);
            MoveAgain();
        }
    }

    //Gère les collisions
    void OnCollisionEnter2D(Collision2D col)
    {
        //Si on rentre en contact avec une scie ou un ennemi le personnage meurt
        if (col.collider.name == "Saw" | col.collider.name == "FrogEnemy")
        {
            Die();
        }
    }

    void FruitCollected()
    {
        Debug.Log("hello");
        FruitAnimator.SetBool("isFruitCollected", true);
    }

    public void FruitGone()
    {
        Destroy(FruitAnimator.gameObject, 0.50f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name != "Kiwi")
        {
            Debug.Log(collision);
            checkpoint = collision.transform.position;
        } else
        {
            FruitCollected();
            FruitGone();
        }

    }

    // Update is called once per frame
    void Update()
    {
        EnemyMovement();
        PlayerMovement();
        PlatformMove();
    }
}