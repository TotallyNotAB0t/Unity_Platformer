using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D enemyBody;
    private bool enemyTurned = false;
    private Vector3 PosEnemy;
    private float movementSpeedEnemy = 5;

    // Start is called before the first frame update
    private void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        PosEnemy = enemyBody.transform.position;
    }

    //Ennemi bouge et tourne
    public void EnemyMovement()
    {
        Vector3 xPos = enemyBody.transform.position;

        enemyBody.transform.position = !enemyTurned
            ? xPos + (movementSpeedEnemy * Time.deltaTime * new Vector3(0.5f, 0, 0))
            : xPos + (movementSpeedEnemy * Time.deltaTime * new Vector3(-0.5f, 0, 0));

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

    //Permet de faire tourner le sprite d'un rigidbody
    private void Turn(Rigidbody2D bod)
    {
        bod.transform.Rotate(0, -180, 0);
    }
}
