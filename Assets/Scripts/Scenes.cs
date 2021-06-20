using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour
{
    private Hearts heartsClass;

    private void Start()
    {
        heartsClass = GameObject.Find("Player").GetComponent<Hearts>();
    }

    public void ReloadScene()
    {
        if (heartsClass.GetLives() == 0)
        {
            SceneManager.LoadScene(1);
        }
    }
}