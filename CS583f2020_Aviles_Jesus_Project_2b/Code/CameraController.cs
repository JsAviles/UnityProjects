using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject Player1;
    public Transform PlayerTransform;
    public Transform CameraTransform;
    public GameObject mainCamera;

    void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        Player1 = GameObject.FindGameObjectWithTag("Player1");
        PlayerTransform = GameObject.FindGameObjectWithTag("Player1").GetComponent<Transform>();
        CameraTransform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CameraTransform.position = new Vector3(PlayerTransform.position.x, PlayerTransform.position.y, CameraTransform.position.z);
    }
}
