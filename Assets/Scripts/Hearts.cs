using UnityEngine;

public class Hearts : MonoBehaviour
{
    public GameObject[] hearts;
    private int lives = 2;
    private bool canDie = false;

    //Animation et suppression des vies
    public void LoseHeart()
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

    public int GetLives()
    {
        return lives;
    }

    public bool CanDie()
    {
        return canDie;
    }
}