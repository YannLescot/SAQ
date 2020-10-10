using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RedirectionFinPartie : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }

    // Update is called once per frame
    public void Rejouer()
    {
        SceneManager.LoadScene("Game");
    }

    public void ChangeMode()
    {
        SceneManager.LoadScene("Home");
    }
}
