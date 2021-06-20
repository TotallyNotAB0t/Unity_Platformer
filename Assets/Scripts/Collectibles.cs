using UnityEngine;
using UnityEngine.UI;

public class Collectibles : MonoBehaviour
{
    public Text KiwiCountDisplay;
    private int KiwiCounter;

    public void FruitCollected(Collider2D fruit)
    {
        Animator fruitAnimator = fruit.GetComponent<Animator>();
        fruitAnimator.SetBool("isFruitCollected", true);
        KiwiCounter += 1;
        SetKiwiCountDisplay(KiwiCounter.ToString());
    }

    public void FruitGone(Collider2D fruit)
    {
        Destroy(fruit.gameObject, 0.50f);
    }

    private void SetKiwiCountDisplay(string numberToAdd)
    {
        KiwiCountDisplay.text = numberToAdd;
    }

    public int GetKiwiCounter()
    {
        return KiwiCounter;
    }
}