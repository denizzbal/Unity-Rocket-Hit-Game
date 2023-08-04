using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSettings : MonoBehaviour
{
    [SerializeField] List<GameObject> BallList = new();
    [SerializeField] Transform ObjeSpawner;

    [SerializeField] List<GameObject> RocketList = new();
    [SerializeField] Transform RocketSpawner;

    [SerializeField] Image BonusImageAnimPanel;
    [SerializeField] Animator BonusImageAnimator;

    [SerializeField] GameObject GameOverPanel;
    [SerializeField] GameObject GameWinPanel;

    private AudioSource _explosionSound;
    public AudioSource ExplosionSound { get => _explosionSound;}

    [SerializeField] AudioSource WinSound;
    [SerializeField] AudioSource BonusSound;

    [SerializeField] Button RocketHitButton;

    float _firstTime;
    float _rocketSpeed;

    [Header("Health Image")]
    [SerializeField] Image _healthImage;
    public Image HealthImage { get=> _healthImage; set=> _healthImage = value; }

    [SerializeField] Sprite[] _healthSpriteArray = new Sprite[4];
    public Sprite[] HealthSpriteArray { get=> _healthSpriteArray; set=> _healthSpriteArray = value; }


    [SerializeField] TMP_Text _bonusText;
    public TMP_Text BonusText { get => _bonusText; set => _bonusText = value; }

    [SerializeField] TMP_Text _winGameText;
    public TMP_Text WinGameText { get => _winGameText; set => _winGameText = value; }



    // If you cant hit the ball 3 times in a row
    private int _gameOverNumber;
    public int GameOverNumber { get => _gameOverNumber; set => _gameOverNumber = value; }

    //If you hit the ball 10 times in a row.
    private int _bonusNumber;
    public int BonusNumber
    {
        get 
        {
            return _bonusNumber;
        }
        set 
        {
            _bonusNumber = value;
            BonusText.text = "Bonus : " + _bonusNumber;
            if (_bonusNumber >= 10)
            {
                BonusSound.Play();
                ScoreAdd();
                _bonusNumber = 0;
                BonusText.text = "Bonus : " + _bonusNumber;
                Debug.Log("Bonus verildi.");
            }
        } 
    }

    //If you hit the ball 100 times in a row.
    private int _winNumber;
    public int WinNumber
    {
        get 
        { 
            return _winNumber; 
        }
        set 
        { 
            _winNumber = value;
            WinGameText.text = "WIN : " + _winNumber;
            if(_winNumber >= 100)
            {
                GameWin();
            }
        }
    }

    private int _score;
    public int Score
    {
        get 
        {
            return _score;
        }
        set 
        {
            _score = value;
            ScoreTextGet.text = "Score : " + _score;
            if(_score >= 10000)
            {
                GameWin();
            }
        }
    }

    [SerializeField] TMP_Text ScoreText;
    public TMP_Text ScoreTextGet { get => ScoreText; set => ScoreText = value; }

    [SerializeField] Rocket _rocketScript;


    //Explosion Pool
    [SerializeField] GameObject ExplosionAnim;
    [HideInInspector]
    public List<GameObject> ExplosionList = new();

    private void Awake()
    {
        _explosionSound = GetComponent<AudioSource>();
    }

    void Start()
    {
        FirstSettings();
        FirstSave();

        for (int i = 0; i < 4; i++) // Explosion Pool
        {
            ExplosionList.Add(Instantiate(ExplosionAnim));
            ExplosionList[i].SetActive(false);
        }   

        InvokeRepeating(nameof(RandomBall), 1f, Random.Range(1f,2f));
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            RocketLaunch();
        }


    }

    void FirstSettings()
    {
        GameOverPanel.SetActive(false);
        GameWinPanel.SetActive(false);
        BonusImageAnimPanel.gameObject.SetActive(false);
        _rocketSpeed = 1700f;
        GameOverNumber = 0;

        BonusNumber = 0;
        BonusText.text = "Bonus : " + BonusNumber;

        WinNumber = 0;
        WinGameText.text = "WIN : " + WinNumber;

        HealthImage.GetComponent<Image>().sprite = HealthSpriteArray[0]; //default 3 health

        foreach (var item2 in BallList)
        {
            item2.SetActive(false); //Baþlangýçta false yapýyoruz listimizin içindeki elemanlarý
        }

        foreach (var item3 in RocketList)
        {
            item3.SetActive(false);
        }
    }

    void FirstSave()
    {
        if (PlayerPrefs.HasKey("Score"))
        {
            Score = PlayerPrefs.GetInt("Score");
            //ScoreTextGet.text = "Score : " + _score.ToString();
        }
        else
        {
            Score = 0;
            PlayerPrefs.SetInt("Score", Score);
            //ScoreTextGet.text = "Score : " + PlayerPrefs.GetInt("score").ToString();
        }
    }

    void RandomBall()
    {
        int _random = Random.Range(0, BallList.Count);
        GameObject Balls2 = BallList[_random];
        Balls2.transform.position = ObjeSpawner.transform.position;
        Balls2.SetActive(true);

    }
    public void RocketLaunch()
    {
        if (Time.time > _firstTime)
        {
            foreach (var item in RocketList)
            {
                if (!item.activeSelf)
                {
                    Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
                    item.transform.SetPositionAndRotation(RocketSpawner.transform.position, RocketSpawner.transform.rotation);
                    item.SetActive(true);
                    rb.AddForce(Vector3.right * _rocketSpeed, ForceMode2D.Force);
                    break;
                }
            }
            _firstTime = Time.time + 0.5f; // En fazla 0.5 saniye aralýðýnda bu fonksiyon çalýþabilir.
        }
    }

    public void GameOver()
    {
        CancelInvoke();
        GameOverPanel.SetActive(true);
    }

    public void GameWin()
    {       
        CancelInvoke();
        GameWinPanel.SetActive(true);
        WinSound.Play();
        Score = 0;
        PlayerPrefs.SetInt("Score", Score);
    }

    void ScoreAdd()
    {
        Score += 100;
        PlayerPrefs.SetInt("Score", Score);
        StartCoroutine(BonusAnimFunc());
    }

    IEnumerator BonusAnimFunc() //Bonus animation
    {
        BonusImageAnimPanel.gameObject.SetActive(true);
        BonusImageAnimator.SetTrigger("IsBonusImage");
        yield return new WaitForSeconds(1.2f);
        BonusImageAnimPanel.gameObject.SetActive(false);
    }

    public void HealthControl()
    {
        switch (GameOverNumber)
        {
            case 0:
                HealthImage.GetComponent<Image>().sprite = HealthSpriteArray[0];
                break;
            case 1:
                HealthImage.GetComponent<Image>().sprite = HealthSpriteArray[1];
                break;
            case 2:
                HealthImage.GetComponent<Image>().sprite = HealthSpriteArray[2];
                break;
            case 3:
                HealthImage.GetComponent<Image>().sprite = HealthSpriteArray[3];
                GameOver();
                break;
            default:
                break;
        }
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void HomeButton()
    {
        SceneManager.LoadScene("Main");
    }

}
