using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cinemachine;

public class Scene_Manager : MonoBehaviour
{
    public Button Play;
    public Button Quit;
    public Button About;
    public Button FAQ;
    public Button Back;
    public Button Mute;
    public AudioSource menuMusic;
    public static AudioSource click;
    public bool musicPlaying;
    public static GameObject PlayerInst;
    public GameObject sOff;
    public GameObject Cine;

    public static Scene_Manager Instance { get; private set; }
    public void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(this);
            Instance = this;

            //Initial Link of Main Menu Buttons
            Play = GameObject.FindGameObjectWithTag("PlayButton").GetComponent<Button>();
            Play.onClick.AddListener(() => LoadScene(3));
            About = GameObject.FindGameObjectWithTag("AboutButton").GetComponent<Button>();
            About.onClick.AddListener(() => LoadScene(1));
            FAQ = GameObject.FindGameObjectWithTag("FAQButton").GetComponent<Button>();
            FAQ.onClick.AddListener(() => LoadScene(2));
            Quit = GameObject.FindGameObjectWithTag("QuitButton").GetComponent<Button>();
            Quit.onClick.AddListener(() => Game_Quit());
            Mute = GameObject.FindGameObjectWithTag("MuteButton").GetComponent<Button>();
            Mute.onClick.AddListener(() => MuteMusic());
            menuMusic = GetComponent<AudioSource>();
            menuMusic.Play();
            click = GameObject.FindGameObjectWithTag("Click").GetComponent<AudioSource>();
            musicPlaying = true;
            sOff = GameObject.FindGameObjectWithTag("SoundOff");
            sOff.SetActive(false);
        }
        else
        {
            //stop all other versions of this game object
            Destroy(gameObject);
        }
    }
    private void OnLevelWasLoaded(int level)
    {
        //MainMenu
        if (level == 0)
        {
            //only replay the music loop if coming from Play Game scenes
            if (musicPlaying == false)
            {
                menuMusic.Play();
            }
            Play = GameObject.FindGameObjectWithTag("PlayButton").GetComponent<Button>();
            Play.onClick.AddListener(() => LoadScene(3));
            About = GameObject.FindGameObjectWithTag("AboutButton").GetComponent<Button>();
            About.onClick.AddListener(() => LoadScene(1));
            FAQ = GameObject.FindGameObjectWithTag("FAQButton").GetComponent<Button>();
            FAQ.onClick.AddListener(() => LoadScene(2));
            Quit = GameObject.FindGameObjectWithTag("QuitButton").GetComponent<Button>();
            Quit.onClick.AddListener(() => Game_Quit());
            Mute = GameObject.FindGameObjectWithTag("MuteButton").GetComponent<Button>();
            Mute.onClick.AddListener(() => MuteMusic());
            Destroy(PlayerInst);
        }
        //About
        if (level == 1)
        {
            musicPlaying = true;
            Back = GameObject.FindGameObjectWithTag("BackButton").GetComponent<Button>();
            Back.onClick.AddListener(() => LoadScene(0));
        }
        //FAQ
        if (level == 2)
        {
            musicPlaying = true;
            Back = GameObject.FindGameObjectWithTag("BackButton").GetComponent<Button>();
            Back.onClick.AddListener(() => LoadScene(0));
        }
        //PlayGame/Level 1
        if (level == 3)
        {
            menuMusic.Stop();
            musicPlaying = false;
            PlayerInst = GameObject.FindGameObjectWithTag("Player1");
        }
        //Level 2
        if(level == 4)
        {
            PlayerInst.GetComponent<PlayerController>().checkPoint = new Vector3(-8.0f, -3.3f, 0);
            PlayerInst.transform.position = new Vector3(-8.0f,-3.3f,0);
            PlayerInst.GetComponent<PlayerController>().checkpointFlag = 0;
            PlayerInst.GetComponent<PlayerController>().curLevel = 2;
            GameObject.FindGameObjectWithTag("Cine").GetComponent<CinemachineVirtualCamera>().Follow = PlayerInst.transform;
  
        }
        //GameOver
        if(level == 5)
        {
            Back = GameObject.FindGameObjectWithTag("BackButton").GetComponent<Button>();
            Back.onClick.AddListener(() => LoadScene(0));
            Play = GameObject.FindGameObjectWithTag("PlayButton").GetComponent<Button>();
            Play.onClick.AddListener(() => LoadScene(3));
            //If player is dead, destory the current singleton player so they can restart
            Destroy(PlayerInst);
        }
        //player wins
        if(level == 6)
        {
            GameObject.FindGameObjectWithTag("WinningScreen").GetComponent<AudioSource>().Play();
            menuMusic.Play();
            GameObject.FindGameObjectWithTag("PlayerUI").SetActive(false);
            Destroy(PlayerInst.GetComponent<SpriteRenderer>());
        }
    }
    //turn music on/off in menu from button press
    public void MuteMusic()
    {
        if (musicPlaying)
        {
            menuMusic.volume = 0;
            //menuMusic.Stop();
            musicPlaying = false;
            sOff.SetActive(true);
        }
        else if(musicPlaying == false)
        {
            menuMusic.volume = .15f;
            //menuMusic.Play();
            musicPlaying = true;
            sOff.SetActive(false);
        }
    }
    //function to load scenes by scene index
    public static void LoadScene(int sceneIndex)
    {
        click.Play();
        //Debug.Log("Loading scene: " + sceneIndex);
        SceneManager.LoadScene(sceneIndex);
    }
    //Quit application and debug for Unity Editor awareness
    public static void Game_Quit()
    {
        Debug.Log("Quitting game.");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
