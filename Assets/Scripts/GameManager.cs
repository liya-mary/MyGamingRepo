using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public TextMeshProUGUI scoreText;
    int score=0;
    public GameObject gameOverPanel;

    public void Awake(){
        if(instance==null){
            instance=this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void IncrementScore(){
        score=score+10;
        scoreText.text=score.ToString();
    }
    public void Restart(){
        SceneManager.LoadScene("Game");
    }
    public void Menu(){
        SceneManager.LoadScene("Menu");
    }
    public void GameOver(){
        gameOverPanel.SetActive(true);
    }
}
