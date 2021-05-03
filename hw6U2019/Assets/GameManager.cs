using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Slider timeBar;
    public Text scoreText;
    public GameObject[] cubeList;

    public float MaxTime = 30f;
    private float CurrentTime = 0f;
    private float generateCubeCD = 3f;
    private float CurrentCD = 0f;
    private int score = 0;
    private bool gameRunning = true;

    private void Update()
    {
        if (gameRunning) 
        {
            CurrentTime += Time.deltaTime;
            CurrentCD += Time.deltaTime;
            if (CurrentTime>MaxTime)
            {
                GameOver();
            }
            SetTimeBar();
            UpdateScoreText();
            GenerateCube();
        }

    }

    void SetTimeBar()
    {
        timeBar.value = 1.0f - CurrentTime/MaxTime;
    }

    void Restart()
    {
        CurrentTime = 0f;
        CurrentCD = 0f;
        SetTimeBar();
        gameRunning = true;
    }

    void GenerateCube()
    {
        if (CurrentCD < generateCubeCD) return;
        CurrentCD = 0f;
        int index = Random.Range(0, cubeList.Length);
        GameObject cube = Instantiate(cubeList[index]);
        var rad = Random.Range(0, 6.18f);
        var r = 5.0f;
        cube.transform.position = new Vector3(
            Mathf.Sin(rad) * r, Random.Range(-0.5f, 0.5f), Mathf.Cos(rad) * r
        );
        BaseCube baseCube = cube.GetComponent<BaseCube>();
        baseCube.gm = this;
    }

    void GameOver()
    {
        gameRunning = false;
    }

    public void GetScore(int pt)
    {
        score += pt;
    }

    public void LoseScore(int pt)
    {
        score -= pt;
    }

    void UpdateScoreText()
    {
        scoreText.text = "SCORE: " + score.ToString();
    }
}
