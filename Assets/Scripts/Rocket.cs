using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rocket : MonoBehaviour
{
    private GameSettings _gameSettings;

    private void Awake()
    {
        _gameSettings = GameObject.FindWithTag("GameSettings").GetComponent<GameSettings>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("RocketDisable"))
        {
            gameObject.SetActive(false);
            _gameSettings.HealthControl();
            _gameSettings.BonusNumber = 0;
            _gameSettings.WinNumber = 0;

        }

        if (collision.collider.CompareTag("Balls"))
        {
            _gameSettings.ExplosionSound.Play();
            _gameSettings.GameOverNumber = 0;
            _gameSettings.HealthControl();
            _gameSettings.BonusNumber++;
            _gameSettings.WinNumber++;
            _gameSettings.Score += int.Parse(collision.collider.name);
            PlayerPrefs.SetInt("Score",_gameSettings.Score);
            //_gameSettings.ScoreTextGet.text ="Score : " +  PlayerPrefs.GetInt("Score").ToString();

            foreach(var item in _gameSettings.ExplosionList)
            {
                if (!item.activeInHierarchy)
                {
                    item.transform.position = gameObject.transform.position;
                    item.SetActive(true);
                    break;
                   
                }
            }
            collision.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    
}
