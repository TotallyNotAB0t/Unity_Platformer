using UnityEngine;

public class Controller : MonoBehaviour
{
    public float movementSpeed = 5;
    public float jumpForce;
    private Rigidbody2D body;
    private bool facingRight = true;
    public Animator animator;
    private Vector3 checkpoint = new Vector3(0, 0, 0);
    private bool canMove = true;
    private Platform platformClass;
    private Collectibles collectiblesClass;
    public Hearts heartsClass;
    private Scenes scenesClass;
    private Enemy enemyClass;

    // Start is called before the first frame update
    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        enemyClass = GameObject.Find("FrogEnemy").GetComponent<Enemy>();
        platformClass = GameObject.Find("Platform").GetComponent<Platform>();
        heartsClass = gameObject.GetComponent<Hearts>();
        scenesClass = GameObject.Find("Scene").GetComponent<Scenes>();
        collectiblesClass = GameObject.Find("Collectibles").GetComponent<Collectibles>();
        Restart();
    }

    //Permet de deplacer le personnage et de le faire courir
    private void Move()
    {
        float movement = Input.GetAxis("Horizontal");
        body.transform.position = body.transform.position + (movementSpeed * Time.deltaTime * new Vector3(movement, 0, 0));
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

    //Joue l'animation de mort et enclenche la fonction Restart (event de l'animator)
    private void Die()
    {
        heartsClass.LoseHeart();
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
        if (heartsClass.CanDie())
        {
            scenesClass.ReloadScene();
        }
        else
        {
            animator.Play("blueRestart");
            ChangePosition(body, checkpoint);
            MoveAgain();
        }
    }

    //Gère les collisions
    private void OnCollisionEnter2D(Collision2D col)
    {
        //Si on rentre en contact avec une scie ou un ennemi le personnage meurt
        if (col.collider.name == "Saw" | col.collider.name == "FrogEnemy")
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name != "Kiwi")
        {
            checkpoint = collision.transform.position;
        }
        else
        {
            collectiblesClass.FruitCollected(collision);
            collectiblesClass.FruitGone(collision);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        enemyClass.EnemyMovement();
        PlayerMovement();
        platformClass.PlatformMove();
    }
}