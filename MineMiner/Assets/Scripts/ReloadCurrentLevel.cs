using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadCurrentLevel : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReloadCurrentScene()
    {
        Debug.Log("Scene Reloaded");
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
