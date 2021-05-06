using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Slider timeBar;
    public Text scoreText;
    public GameObject[] cubeList;
    public GameObject timeOverUI;
    public GameObject startUI;

    public float MaxTime = 30f;
    private float CurrentTime = 0f;
    public float generateCubeCD = 1.0f;
    private float CurrentCD = 0f;
    private int score = 0;
    private bool gameRunning = false;

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

    public void GameStart()
    {
        score = 0;
        CurrentTime = 0f;
        CurrentCD = 0f;
        SetTimeBar();
        UpdateScoreText();
        gameRunning = true;
        startUI.SetActive(false);
        timeOverUI.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
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
        if (index==0)
        {
            BaseCube baseCube = cube.GetComponent<BaseCube>();
            baseCube.gm = this;
        }
        else if (index == 1)
        {
            Cube_bad tmp_cube = cube.GetComponent<Cube_bad>();
            tmp_cube.gm = this;
        }
        else if (index == 2)
        {
            Cube_scaling tmp_cube = cube.GetComponent<Cube_scaling>();
            tmp_cube.gm = this;
        }
        else if (index == 3)
        {
            Cube_shaking tmp_cube = cube.GetComponent<Cube_shaking>();
            tmp_cube.gm = this;
        }
       
    }

    void GameOver()
    {
        gameRunning = false;
        timeOverUI.SetActive(true);
        startUI.SetActive(true);
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
