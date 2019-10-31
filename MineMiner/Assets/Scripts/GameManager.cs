using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int Dist;
    public int DeathDist;

    gameLoader2 loader;
    public Player2 player;

    [SerializeField] Image progressBar;
    [SerializeField] GameObject gameOver;
    [SerializeField] GameObject NextLevel;

    float tileCount;

    private void Start() {
        loader = GetComponent<gameLoader2>();
    }

    private void Update() {
        tileCount = (loader.chunksToLoad.Length - 1.5f) * (player.currentChunk.chunkSize.y - 1);
        progressBar.fillAmount = (float)Dist / (float)tileCount;

        if (DeathDist > Dist && Dist > 10) {
            //gameover

            GetComponent<SwipeDetection2>().StopAllCoroutines();
            GetComponent<TimeLimit>().StopAllCoroutines();
            gameOver.SetActive(true);
        }
        if (Dist >= tileCount) {
            GetComponent<SwipeDetection2>().StopAllCoroutines();
            GetComponent<TimeLimit>().StopAllCoroutines();
            NextLevel.SetActive(true);
        }
    }
}
