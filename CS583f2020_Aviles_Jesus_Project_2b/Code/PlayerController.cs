using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Awake Variables
    public GameObject PlayerObj;
    public Animator PlayerAnimator;
    public Collider2D PlayerCollider;
    public Rigidbody2D PlayerMovement;
    public AudioSource footstep;
    public AudioSource jump;
    public AudioSource monsterHit;
    public AudioSource takeHit;
    public AudioSource igMusic;
    public bool Paused;
    public Button MenuButton;
    public Button QuitButton;
    public Button ResumeButton;
    public GameObject MenuUI;
    public int curLevel;

    //Finite State Machine
    public enum State {idle, moving, jumping, falling, takeDamage }
    public State charState = State.idle;

    //movement variables
    public LayerMask Ground;
    public float speed;
    public float jumpHeight;
    public float takeDmg;
    public float xDirection;

    //character variable
    public int HP;
    public GameObject Heart1;
    public GameObject Heart2;
    public GameObject Heart3;
    public GameObject Heart4;
    public GameObject Heart5;
    public bool hasJumped;
    public int lives;
    public Vector3 checkPoint;
    public int checkpointFlag;
    //collectable variables
    public int coins;
    public int enemies;
    public GameObject CoinsUI;
    public GameObject monsUI;
    public bool justSpawned;


    public static PlayerController Instance { get; private set; }
    public void Awake()
    {
        if (Instance == null)
        {
            //Debug.Log("New Level");
            DontDestroyOnLoad(this);
            Instance = this;
            PlayerObj = GameObject.FindGameObjectWithTag("Player1");
            PlayerMovement = GameObject.FindGameObjectWithTag("Player1").GetComponent<Rigidbody2D>();
            PlayerAnimator = GameObject.FindGameObjectWithTag("Player1").GetComponent<Animator>();
            PlayerCollider = GameObject.FindGameObjectWithTag("Player1").GetComponent<Collider2D>();
            CoinsUI = GameObject.FindGameObjectWithTag("CoinCtr");
            monsUI = GameObject.FindGameObjectWithTag("MonsterCtr");
            coins = 0;
            enemies = 0;
            CoinsUI.GetComponent<Text>().text = coins.ToString();
            monsUI.GetComponent<Text>().text = enemies.ToString();
            curLevel = 1;
            speed = 3f;
            jumpHeight = 8f;
            takeDmg = 5f;
            HP = 5;
            curLevel = 1;
            if(curLevel == 1)
            {
                checkPoint = new Vector3(-7, -4.183f, 0);
            }
            checkpointFlag = 0;
            Heart1 = GameObject.FindGameObjectWithTag("Heart1");
            Heart2 = GameObject.FindGameObjectWithTag("Heart2");
            Heart3 = GameObject.FindGameObjectWithTag("Heart3");
            Heart4 = GameObject.FindGameObjectWithTag("Heart4");
            Heart5 = GameObject.FindGameObjectWithTag("Heart5");
            lives = 1;
            hasJumped = false;
            Paused = false;
            justSpawned = false;
            monsterHit = GameObject.FindGameObjectWithTag("MonsterKill").GetComponent<AudioSource>();
            takeHit = GameObject.FindGameObjectWithTag("TakeHit").GetComponent<AudioSource>();
            igMusic = GameObject.FindGameObjectWithTag("InGameMusic").GetComponent<AudioSource>();
            MenuUI = GameObject.FindGameObjectWithTag("PauseMenu");
            MenuButton = GameObject.FindGameObjectWithTag("BackButton").GetComponent<Button>();
            MenuButton.onClick.AddListener(() => mainMenu());
            QuitButton = GameObject.FindGameObjectWithTag("QuitButton").GetComponent<Button>();
            QuitButton.onClick.AddListener(() => Game_Quit());
            ResumeButton = GameObject.FindGameObjectWithTag("Resume").GetComponent<Button>();
            ResumeButton.onClick.AddListener(() => ResumeGame());
            MenuUI.SetActive(false);

        }
        else
        {
            //stop all other versions of this game object
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        //set beginning checkpoint
        if (curLevel == 2 && checkpointFlag == -1)
        {

        }
        stateDetect();
        if (charState != State.takeDamage)
        {
            charMovement();
        }
        //stateDetect();
        //update Animator to current state
        PlayerAnimator.SetInteger("charState", (int)charState);
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(Paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

    }
    private void OnLevelWasLoaded(int level)
    {

        if (level == 3)
        {
            PlayerObj.transform.position = new Vector3(-7, -4.183f, 0);
        }
        if (level == 4)
        {
            PlayerObj.transform.position = new Vector3(-8, -3f, 0);

        }
    }
    //collect a coin when you collide with it
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Collectable")
        {
            CoinsUI.GetComponent<AudioSource>().Play();
            Destroy(collision.gameObject);
            coins++;
            CoinsUI.GetComponent<Text>().text = coins.ToString();
        }
    }

    //Destory enemy when jumping on it
    private void OnCollisionEnter2D(Collision2D enemy)
    {
        if(enemy.gameObject.tag == "Enemy")
        {
            if(charState == State.falling)
            {
                monsterHit.Play();
                enemy.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
                enemy.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                enemy.gameObject.GetComponent<Animator>().SetBool("Die", true);
                PlayerMovement.velocity = new Vector2(PlayerMovement.velocity.x, jumpHeight/2);
                charState = State.jumping;
                enemies++;
                monsUI.GetComponent<Text>().text = enemies.ToString();
            }
            else
            {
                takeHit.Play();
                charState = State.takeDamage;
                //take damage depending on player position to enemy
                if(enemy.gameObject.transform.position.x > PlayerObj.transform.position.x && enemy.gameObject.GetComponent<Rigidbody2D>().bodyType != RigidbodyType2D.Static)
                {
                    PlayerMovement.velocity = new Vector2(-takeDmg, PlayerMovement.velocity.y);
                }
                else if(enemy.gameObject.GetComponent<Rigidbody2D>().bodyType != RigidbodyType2D.Static)
                {
                    PlayerMovement.velocity = new Vector2(takeDmg, PlayerMovement.velocity.y);
                }
                damageHealth(HP);
                HP--;
            }
        }
        if(enemy.gameObject.tag == "Death")
        {
            takeHit.Play();
            damageHealth(HP);
            HP--;
            if(HP>0)
            {
                PlayerObj.transform.position = checkPoint;
            }
        }
        if(enemy.gameObject.tag == "NextLevel")
        {
            Scene_Manager.LoadScene(4);
        }
        if (enemy.gameObject.tag == "CheckPoint")
        {
            checkPointReached();
        }
        if(enemy.gameObject.tag == "Complete")
        {
            igMusic.Stop();
            Scene_Manager.LoadScene(6);
        }
    }
    private void charMovement()
    {
        xDirection = Input.GetAxisRaw("Horizontal");

        //move left
        if (xDirection < 0)
        {
            transform.localScale = new Vector2(-1, 1);
            PlayerMovement.velocity = new Vector2(-speed, PlayerMovement.velocity.y);
        }
        //move right
        else if (xDirection > 0)
        {
                transform.localScale = new Vector2(1, 1);
            PlayerMovement.velocity = new Vector2(speed, PlayerMovement.velocity.y);
        }
        if(hasJumped == false && charState == State.falling && Input.GetButtonDown("Jump"))
        {
            hasJumped = true;
            jump.Play();
            PlayerMovement.velocity = new Vector2(PlayerMovement.velocity.x, jumpHeight);
            charState = State.jumping;
        }
        //jump
        if (Input.GetButtonDown("Jump") && PlayerCollider.IsTouchingLayers(Ground))
        {
            hasJumped = true;
            jump.Play();
            PlayerMovement.velocity = new Vector2(PlayerMovement.velocity.x, jumpHeight);
            charState = State.jumping;
        }
    }
    //Detect current state based on velocity
    private void stateDetect()
    {
        // if character is currently moving in positive Y direction
        if (charState == State.jumping)
        {
            if(PlayerMovement.velocity.y < .1f)
            {
                charState = State.falling;
            }
        }
        // if character is falling (negative Y direction)
        else if(charState == State.falling)
        {
            if(PlayerCollider.IsTouchingLayers(Ground))
            {
                charState = State.idle;
            }
        }
        //if player collided with an enemy
        else if (charState == State.takeDamage)
        {
            if (Mathf.Abs(PlayerMovement.velocity.x) < .01f)
            {
                charState = State.idle;
            }
        }
        else if(!PlayerCollider.IsTouchingLayers(Ground) && PlayerMovement.velocity.y < .1f)
        {
            charState = State.falling;
        }
        //if player is moving
        else if(Mathf.Abs(PlayerMovement.velocity.x) > 2f)
        {
            charState = State.moving;
            hasJumped = false;
        }
        else
        {
            PlayerMovement.velocity = new Vector2(0, PlayerMovement.velocity.y);
            charState = State.idle;
            hasJumped = false;
        }
    }
    public void damageHealth(int hp)
    {
        if (HP == 5)
        {
            Heart5.SetActive(false);
        }
        else if (HP == 4)
        {
            Heart4.SetActive(false);
        }
        else if (HP==3)
        {
            Heart3.SetActive(false);
        }
        else if(HP == 2)
        {
            Heart2.SetActive(false);
        }
        //game over
        else if (HP == 1)
        {
            Heart1.SetActive(false);
            checkpointFlag = 0;
            checkPoint = new Vector3(-7, -4.183f, 0);
            Scene_Manager.LoadScene(5);
            //or load game over canvas then load screen
        }

    }

    public void ResumeGame()
    {
        Scene_Manager.click.Play();
        MenuUI.SetActive(false);
        igMusic.mute = false;
        Time.timeScale = 1f;
        Paused = false;
 
    }

    public void PauseGame()
    {
        MenuUI.SetActive(true);
        igMusic.mute = true;
        Time.timeScale = 0f;
        Paused = true;
    }

    public void mainMenu()
    {
        MenuUI.SetActive(false);
        Time.timeScale = 1f;
        Scene_Manager.LoadScene(0);
    }

    public void footStep()
    {
        footstep.Play();
    }

    public void checkPointReached()
    {
        if (curLevel == 1)
        {
            //update checkpoint to middle map
            if (checkpointFlag == 0 && checkPoint.x == -7 && PlayerObj.transform.position.x > 25)
            {
                checkPoint = new Vector3(28, 3.6f, 0);
                checkpointFlag++;
            }
            //update checkpoint to end
            else if (checkpointFlag == 1 && checkPoint.x == 28 && PlayerObj.transform.position.x > 60)
            {
                checkPoint = new Vector3(64, -2.5f, 0);
            }
        }
        if(curLevel == 2)
        {
            if (checkpointFlag == 0 && checkPoint.x == -8 && PlayerObj.transform.position.x > 25)
            {
                checkPoint = new Vector3(37, 3f, 0);
                checkpointFlag++;
            }
            //update checkpoint to end
            else if (checkpointFlag == 1 && checkPoint.x == 37 && PlayerObj.transform.position.x > 65)
            {
                checkPoint = new Vector3(75, 0f, 0);
                checkpointFlag++;
            }
            else if (checkpointFlag == 2 && checkPoint.x == 75 && PlayerObj.transform.position.x > 90)
            {
                checkPoint = new Vector3(115, 0f, 0);
                checkpointFlag++;
            }
            else if (checkpointFlag == 3 && checkPoint.x == 115 && PlayerObj.transform.position.x > 125)
            {
                checkPoint = new Vector3(130, 0f, 0);
                checkpointFlag++;
            }
        }
    }
    public void Game_Quit()
    {
        Debug.Log("Quitting game.");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
