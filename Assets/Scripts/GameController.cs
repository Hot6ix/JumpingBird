using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public bool isGameOver = false;
    public GameObject gameOverText;
    public Button interactButton;
    public Text scoreText;
    public GameObject background;

    private int score = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != null)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "SCORE: " + score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(isGameOver == true && Input.GetButtonDown("Jump"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        gameOverText.SetActive(true);
        interactButton.enabled = false;
        background.GetComponent<AudioSource>().Pause();
        Debug.Log("Game over");
    }

    public void Scored()
    {
        if (isGameOver) return;

        score++;
        scoreText.text = "SCORE: " + score.ToString();
    }
}
