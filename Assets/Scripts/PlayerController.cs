using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class PlayerController : MonoBehaviour
{
    public float TimeRemaining = 99999999;
    public float CoolDown = 0;
    private double VelocityTotal;
    private Vector3 offset = new Vector3(0.0f, 9f, -10f);

    public bool CanDash = false;
    public bool IsAllBlocksGone = false;
    public bool HasSpeed = false;

    public GameObject Arrow;
    private Vector3 currentDirection;

    public float speed = 10;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI DebugText;
    public TextMeshProUGUI MouseCenterAncher;
    public TextMeshProUGUI DestroyedCountText;
    public GameObject winTextObject;
    public GameObject Camera;
    public Camera camcam;

    private Rigidbody rb;
    private int count;

    public LayerMask layer;
    private Vector3 rayhitpoint;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winTextObject.SetActive(false);
        Arrow.SetActive(false);
    }
    void Update()
    {
        VelocityTotal = Math.Sqrt(Math.Pow(rb.velocity.x, 2) + Math.Pow(rb.velocity.y, 2) + Math.Pow(rb.velocity.z, 2));

        //Arrow direction
        RaycastHit hit;
        if (Physics.Raycast(camcam.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, layer, QueryTriggerInteraction.Collide))
        {
            rayhitpoint = new Vector3(hit.point.x, 0, hit.point.z);
            currentDirection = (new Vector3(transform.position.x, 0, transform.position.z) - rayhitpoint).normalized;
        }
        Arrow.transform.position = transform.position - currentDirection;

        if (CoolDown > 0) { CoolDown -= Time.deltaTime; }

        DebugText.text = VelocityTotal.ToString();
        //DebugText.text = Math.Sqrt(Math.Pow(currentDirection.x, 2) + Math.Pow(currentDirection.z, 2)).ToString();
        //DebugText.text = Input.mousePosition.x.ToString() + " , " + Input.mousePosition.y.ToString();
        //DebugText.text = (MouseCenterAncher.transform.position.x - Input.mousePosition.x).ToString() + " , " + (MouseCenterAncher.transform.position.y - Input.mousePosition.y).ToString();
        //DebugText.text = CoolDown.ToString();

    }

    public void SetCountText()
    {
        //cube text and win text
        countText.text = "Cubes Collected: " + count.ToString() + "/34";
        if(count >= 34 && IsAllBlocksGone == true) {winTextObject.SetActive(true);}
    }

    void FixedUpdate()
    {
        //Add force based on key presses
        if (Input.GetKey("w"))
        {
            rb.AddForce(new Vector3(-1 *(Camera.transform.position.x - transform.position.x),0.0f, -1 * (Camera.transform.position.z - transform.position.z)));
        }
        if( Input.GetKey("s"))
        {
            rb.AddForce(new Vector3(Camera.transform.position.x - transform.position.x, 0.0f,Camera.transform.position.z - transform.position.z));
        }
        if (Input.GetKey("a"))
        {
            rb.AddForce(new Vector3(Camera.transform.position.z - transform.position.z, 0.0f, -1 * (Camera.transform.position.x - transform.position.x)));
        }
        if (Input.GetKey("d"))
        {
            rb.AddForce(new Vector3(-1 * (Camera.transform.position.z - transform.position.z), 0.0f,Camera.transform.position.x - transform.position.x));
        }

        //Dash
        if (Input.GetKey("space") && CoolDown <= 0 && CanDash == true)
        {
            rb.AddForce(new Vector3(-rb.velocity.x * 50, 0.0f, -rb.velocity.z * 50));
            rb.AddForce(-currentDirection * (750 + (10 * (float)VelocityTotal))); 
            CoolDown = 0.7f; 
        }

        //Speed Cap
        if (VelocityTotal >= 15.0f && CoolDown <= 0.4f)
        {
            rb.AddForce(-rb.velocity);
        }

        //Camera Movement
        Camera.transform.position = transform.position + offset;

        if (Input.GetKey("q") == true && Input.GetKey("e") == false)
        {
            Camera.transform.RotateAround(new Vector3(transform.position.x, transform.position.y + 10.5f, transform.position.z), new Vector3(0.0f, 1.0f, 0.0f), 1f);
            offset = Camera.transform.position - transform.position;
        }

        if (Input.GetKey("e") == true && Input.GetKey("q") == false)
        {
            Camera.transform.RotateAround(new Vector3(transform.position.x, transform.position.y + 10.5f, transform.position.z), new Vector3(0.0f, 1.0f, 0.0f), -1f);
            offset = Camera.transform.position - transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }

        if (other.gameObject.CompareTag("Void"))
        {
            transform.position = new Vector3(-36.07f, 0.5f, 0.0f);
        }
    }
}
