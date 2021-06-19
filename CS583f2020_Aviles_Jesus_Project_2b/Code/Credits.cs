using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    public float highPoint;
    public float lowPoint;
    public Rigidbody2D CreditsRB;
    public bool Down;
    public float riseSpeed;
    public float dropSpeed;
    public GameObject Sebastian;//c2
    public GameObject BG;//c3
    public GameObject Logo;
    public GameObject text;//c4
    public GameObject Button;
    public Button MainMenu;
    // Start is called before the first frame update
    public void Start()
    {
        CreditsRB = GetComponent<Rigidbody2D>();
        Down = false;
        Logo = GameObject.FindGameObjectWithTag("C1");
        Sebastian = GameObject.FindGameObjectWithTag("C2");
        BG = GameObject.FindGameObjectWithTag("C3");
        text = GameObject.FindGameObjectWithTag("C4");
        Button = GameObject.FindGameObjectWithTag("BackButton");
        MainMenu = GameObject.FindGameObjectWithTag("BackButton").GetComponent<Button>();
        Logo.SetActive(false);
        Button.SetActive(false);
    }

    // Update is called once per frame
    public void Update()
    {

        patrol();

    }

    private void OnCollisionEnter2D(Collision2D enemy)
    {
        if(enemy.gameObject.tag == "Logo")
        {
            Sebastian.SetActive(false);
            BG.SetActive(false);
            text.SetActive(false);
            Logo.SetActive(true);
            Button.SetActive(true);
            MainMenu = GameObject.FindGameObjectWithTag("BackButton").GetComponent<Button>();
            MainMenu.onClick.AddListener(() => Scene_Manager.LoadScene(0));
        }
    }
    public void patrol()
    {
        if (Down)
        {
            if (transform.position.y > lowPoint)
            {
                CreditsRB.velocity = new Vector2(transform.position.x, -dropSpeed);
            }
            else
            {
                Down = false;
            }
        }
        else
        {
            if (transform.position.y < highPoint)
            {
                CreditsRB.velocity = new Vector2(transform.position.x, riseSpeed);
            }
            else
            {
                Down = true;
            }
        }
    }
}
