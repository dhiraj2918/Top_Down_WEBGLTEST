using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject gameoverMenu;
    public GameObject playMenu;
    public GameObject VC1;
    public GameObject VC2;
    public PlayerController pC;
    public TextMeshProUGUI progress;
    public GameObject progressCanvas;

    public EnemyPool ePool; 

    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;

    [SerializeField] private AudioMixer audioMixer; 
    public static bool isPaused;

  

    // Start is called before the first frame update
    void Start()
    {
        IsVisibleandCursorUnlock();
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        gameoverMenu.SetActive(false);
        playMenu.SetActive(true);
        VC2.SetActive(true);
        progressCanvas.SetActive(false);
        // Initialize the volume sliders
        InitializeVolumeSliders();
    }

    // Update is called once per frame
    void Update()
    {
        if (ePool.deathCount == 0)
        {
            GameOver();
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        if (pC.health <= 0)
        {
            GameOver();
        }

        // Update progress text
        if (progress != null)
        {
            progress.text = "Kill " + ePool.deathCount + " enemies to complete this level";
        }
    }

    public void ResumeGame()
    {
        VC1.SetActive(true);
        VC2.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playMenu.SetActive(false);
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
        progressCanvas.SetActive(true);
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
        progressCanvas.SetActive(false);
    }

    public void OptionMenu()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void GameOver()
    {
            IsVisibleandCursorUnlock();
            gameoverMenu.SetActive(true);
            Time.timeScale = 1;
    }
        public void Reset()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void ChangeMasterVolume(float volume)
        {
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        }

        public void ChangeSFXVolume(float volume)
        {
            audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        }

        public void ChangeMusicVolume(float volume)
        {
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        }

        private void InitializeVolumeSliders()
        {
            if (masterVolumeSlider != null)
            {
                float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1);
                masterVolumeSlider.value = masterVolume;
                ChangeMasterVolume(masterVolume);
                masterVolumeSlider.onValueChanged.AddListener(ChangeMasterVolume);
            }

            if (sfxVolumeSlider != null)
            {
                float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1);
                sfxVolumeSlider.value = sfxVolume;
                ChangeSFXVolume(sfxVolume);
                sfxVolumeSlider.onValueChanged.AddListener(ChangeSFXVolume);
            }

            if (musicVolumeSlider != null)
            {
                float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1);
                musicVolumeSlider.value = musicVolume;
                ChangeMusicVolume(musicVolume);
                musicVolumeSlider.onValueChanged.AddListener(ChangeMusicVolume);
            }
        }

        public void NotVisibleandCursorLock()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void IsVisibleandCursorUnlock()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
}
