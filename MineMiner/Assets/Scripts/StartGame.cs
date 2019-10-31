using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    //public GameObject tapToHoldpanel;
    //public GameObject homeButton;
    //public GameObject LevelPanel;

    public void BeginGame()
    {
        //tapToHoldpanel.SetActive(false);
        //homeButton.SetActive(true);
        //SceneManager.LoadScene("Level1");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
