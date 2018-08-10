using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject m_platform;
    public Vector2 m_spawnValues;
    public int m_platformCount;
    public float m_spawnWait;

    public static bool m_gameIsPaused = true;
    public GameObject m_startButtonUI;

    public GameObject m_player;
    public static bool m_playerIsDead = false;

    public GameObject m_coinPickUp;
    public GameObject m_doubleJumpPowerUp;
    public GameObject m_doubleJumpUI;

    public SpriteRenderer m_backgroundImage;

    public float m_distanceTravelled;
    public Text m_distanceTravelledText;

    public static float m_highscore;
    public Text m_highscoreText;
    
    bool m_firstStart = false;

    private void Start()
    {
        m_firstStart = true;
        m_gameIsPaused = true;

        if (m_gameIsPaused)
            GamePaused();

        if (!m_gameIsPaused)
            GameResume();

        /*FUTURE_DEVELOPEMENT
         * Feel as if there is a more efficient way of starting the game...*/
        
        m_distanceTravelledText.text = "Distance: " + m_distanceTravelled + "m";
    }

    IEnumerator SpawnWaves()
    {
        /*GOAL_FOR_SPAWNING_PLATFORMS
         * One problem I ran into early in development was platforms out of the player jump range
         * I fixed this by checking the difference between the y-positions of the last spawned platform and the new one before it was spawned in game
         * If the absolute difference between the two y-positions was greater than the jump height then generate a new platform y-position 
         * Then check that one again until the acceptable parameters are met */
         
        //randomNumber1 < 0f && spawnPosition.y > (randomNumber1 + 2.2f)
        float randomNumber1 = 0f;

        for (int i = 0; i < m_platformCount; i++) //Each level has a set number of platforms that are spawned
        {

            Vector2 spawnPosition = new Vector2 (m_spawnValues.x, Random.Range(-m_spawnValues.y, m_spawnValues.y));
            Quaternion spawnRotation = Quaternion.identity;

            float randomNumber2 = spawnPosition.y;

            if((Mathf.Abs((randomNumber2 - randomNumber1))) > 2.2f)
            {
                if (randomNumber1 == 0f && spawnPosition.y > 2.2f)
                    spawnPosition.y = spawnPosition.y - 2.2f;
                else if (randomNumber1 == 0f && spawnPosition.y < -2.2f)
                    spawnPosition.y = spawnPosition.y + 2.2f;
                else if (randomNumber1 < 0f && (randomNumber2 > (randomNumber1 + 2.2f)))
                    spawnPosition.y = 0f;
            }
            
            Instantiate(m_platform, spawnPosition, spawnRotation);
            m_platform.transform.localScale = new Vector2(Random.Range(3.5f, 6.0f), m_platform.transform.localScale.y);
            randomNumber1 = randomNumber2;

            yield return new WaitForSeconds (m_spawnWait);
            
            /*FUTURE_DEVELOPMENT
             * A while-loop has the possibility of eliminate much of the code 
             * In order to generate a new m_spawnPosition.y if parameters are not met*/

        }
    }

    private void Update()
    {
        PlayerDeath();
        
        if(!m_playerIsDead && !m_gameIsPaused)
            m_distanceTravelledText.text = "Distance: " + m_distanceTravelled + "m";
        
    }

    private void FixedUpdate()
    {
        TimeScalling();
        DistanceTravelUpdate();

        if (!m_playerIsDead && m_player)
        {
            if (m_player.GetComponent<PlayerMovement>().m_doubleJumpEnabled)
                m_doubleJumpUI.SetActive(true);
            else
                m_doubleJumpUI.SetActive(false);
        }
    }

    public void GameResume()
    {
        m_gameIsPaused = false;
        m_startButtonUI.SetActive(false);
        Time.timeScale = 1.0f;
        StartCoroutine(SpawnWaves());
    }

    public void GamePaused()
    {
        m_highscoreText.text = "Highscore: " + m_highscore + "m";
        m_startButtonUI.SetActive(true);
        m_gameIsPaused = true;
        Time.timeScale = 0.0f;
    }

    void TimeScalling()
    {
        if (!m_firstStart)
        {
            Time.timeScale = 1.0f;
            m_distanceTravelled = 0f;
        }

        if (((Time.time + 1f) % 8f) == 0f) //Every 9 seconds
        {
            Time.timeScale += 0.25f;
            SpawnPickUp(1);
            m_backgroundImage.color = new Vector4(Random.Range(0f,1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
        }
        else if(((Time.time + 1f)% 11f) == 0f)
        {
            SpawnPickUp(2);
        }
        
    }

    void PlayerDeath()
    {
        if (m_player == false)
            m_playerIsDead = true;

        if (m_playerIsDead)
        {
            if (m_distanceTravelled > m_highscore)
                m_highscore = m_distanceTravelled;

            SceneManager.LoadScene("Main");
            m_playerIsDead = false;
        }
    } 

    void SpawnPickUp(int choice)
    {
        if(choice == 1)
            Instantiate(m_coinPickUp, new Vector2 (m_platform.transform.position.x, m_platform.transform.position.y + Random.Range(1f, 2.2f)), m_platform.transform.rotation);
        else if(choice == 2)
            Instantiate(m_doubleJumpPowerUp, new Vector2(m_platform.transform.position.x, m_platform.transform.position.y + Random.Range(1f, 2.2f)), m_platform.transform.rotation);
    }

    void DistanceTravelUpdate()
    {
        if (((Time.time + 1f) % 1f) == 0f)
            m_distanceTravelled += 1f;
    }
}
