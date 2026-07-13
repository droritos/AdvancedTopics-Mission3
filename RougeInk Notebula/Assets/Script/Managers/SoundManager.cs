using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioClip shootingSound;
    [SerializeField] AudioClip enemyDied;
    [SerializeField] AudioClip backgroundMusic;
    [SerializeField] AudioClip gameOver;
    [SerializeField] private AudioSource audioSource;
    public static SoundManager Instance;

    private void Awake()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    public void PlaySound(Sounds sound)
    {
        switch (sound)
        {
            case Sounds.BackgroundMusic:
                audioSource.PlayOneShot(backgroundMusic);
                break;
            case Sounds.ShootingSound:
                audioSource.PlayOneShot(shootingSound);
                break;
            case Sounds.EnemyDiedSound:
                audioSource.PlayOneShot(enemyDied);
                break;
            case Sounds.GameOver:
                audioSource.PlayOneShot(gameOver);
                break;
        }
    }
}
public enum Sounds
{
    BackgroundMusic,
    ShootingSound,
    EnemyDiedSound,
    GameOver
};