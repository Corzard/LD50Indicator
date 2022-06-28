using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject particles;
    [SerializeField] private Text scoreUI;
    [SerializeField] PlayerController plContr;
    [SerializeField] Light envLight;
    [SerializeField] Color agressiveLightColor;
    [SerializeField] GameObject endGameUI;
    private bool gameIsStarted = false;
    private float scoreValue = 0;
    private float timeToStart = 9f;

    private void Start()
    {
        plContr.PlayerDead += EndGame;
        enemy.GetComponent<EnemyLogic>().StartHitting += () => StartCoroutine(ChangeColorRoutine(agressiveLightColor));
        enemy.GetComponent<EnemyLogic>().SpeedUp += () => StartCoroutine(ChangeColorRoutine(Color.black));

    }
    private void Awake()
    {
        Time.timeScale = 0;
    }

    private void Update()
    {
        if (timeToStart > 0)
        {
            timeToStart -= Time.deltaTime;
            return;
        }
        if (timeToStart <= 0 && gameIsStarted)
        {
            particles.SetActive(false);
            enemy.SetActive(true);
        }
        if (Time.timeSinceLevelLoad >= 10 && scoreUI != null)
        {
            scoreValue += Time.deltaTime;
            scoreUI.text = $"{Mathf.Round(scoreValue)}sec.";
        }
        if (!gameIsStarted)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    private void EndGame()
    {
        scoreUI = null;
        //enemy.SetActive(false);
        gameIsStarted = false;
        particles.transform.position = enemy.transform.position;
        particles.SetActive(true);
        endGameUI.SetActive(true);
    }

    // private void ChangeColor()
    // {
    //     StartCoroutine(ChangeColorRoutine());
    // }
    private IEnumerator ChangeColorRoutine(Color endColor)
    {
        while (envLight.color != endColor)
        {
            envLight.color = Color.Lerp(envLight.color, endColor, Time.deltaTime);
            yield return new WaitForSeconds(0.0001f);
        }
    }

    public void StartGame(GameObject sender)
    {
        sender.SetActive(false);
        Time.timeScale = 1;
        gameIsStarted = true;
        particles.SetActive(true);
    }
}
