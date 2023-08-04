using UnityEngine;

public class BallSpeed : MonoBehaviour
{
    float _minDownSpeed;
    float _maxDownSpeed;
    float _randomSpeed;

    private GameSettings _gameSettings;

    private void Awake()
    {
        _minDownSpeed = 4f;
        _maxDownSpeed = 7f;
    }

    void Start()
    {
        _randomSpeed = Random.Range(_minDownSpeed, _maxDownSpeed);
        _gameSettings = GameObject.FindWithTag("GameSettings").GetComponent<GameSettings>();

    }

    void Update()
    {
        transform.Translate(0, -_randomSpeed * Time.deltaTime, 0);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("ObjeDisable"))
        {
            gameObject.SetActive(false);        
        }

        if (collision.collider.CompareTag("ObjeHitOver"))
        {
            _gameSettings.GameOverNumber++;
            _gameSettings.HealthControl();
        }
    }
}
