using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Transform pos;
    private Tweener tweener;
    public Camera cam;
    private bool isGrounded;
    public Rigidbody rb;
    public GameObject prefab;
    public float jumpForce;
    public float sensitivity;
    public float moveSens;
    private float moveHorizontal;
    private float moveVertical;
    private float moveX;
    private float moveY;
    private float timer = 0.4f;
    private bool isTimerRunning = false;
    private Collider other;
    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        //camMove.handleCamMove(target);
        //Cursor.lockState = CursorLockMode.Locked;
        Application.targetFrameRate = 144;
    }

    // Update is called once per frame
    void Update()
    {
 
        collectInput();
        movement();
        jump();

    }

    private void collectInput()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
        Transform[] ts = target.GetComponentsInChildren<Transform>();
        foreach (Transform com in ts)
        {
        }
        if (ts.Length > 1)
        {

            moveX = Input.GetAxis("Mouse X");
            moveY = Input.GetAxis("Mouse Y");
        }
    }

    private void movement()
    {
        target.transform.Rotate(0, moveX * sensitivity, 0);
        cam.transform.Rotate(-moveY * sensitivity, 0, 0);
        target.transform.Translate(Vector3.right * moveHorizontal * moveSens * Time.deltaTime, Space.Self);
        target.transform.Translate(Vector3.forward * moveVertical * moveSens * Time.deltaTime, Space.Self);
    }

    private void jump()
    {
        if (Input.GetKeyDown(KeyCode.Space)) //used to be isgoru
        {
            rb.AddForce(new Vector3(0, 2.0f, 0) * jumpForce, ForceMode.Impulse);
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "box")
        {
            if (timer == 0)
            {
                timer = 0.4f;
                this.other = other;
            }
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "terrain")
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isGrounded = false;
    }






}

