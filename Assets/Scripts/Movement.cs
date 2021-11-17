using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.XR;

public class Movement : MonoBehaviour
{
    public Transform pos;
    private Tweener tweener;
    public Camera cam;
    public bool isGrounded;
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
    public bool canUseJetPack = false;
    public PopUpManager popUpManager;
    public StatManger statManager;
    private InputDevice RightController;
    private InputDevice LeftController;
    

    // Start is called before the first frame update
    void Start()
    {
        //camMove.handleCamMove(target);
        Cursor.lockState = CursorLockMode.Locked;
        Application.targetFrameRate = 144;
        List<InputDevice> inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller, inputDevices);
        RightController = inputDevices[0];
        inputDevices.Clear();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller, inputDevices);
        LeftController = inputDevices[0];
    }

    // Update is called once per frame
    void Update()
    {
 
        collectInput();
        movement();
        jump();
        jetpackUse();

    }

    private void collectInput()
    {
       // moveHorizontal = Input.GetAxis("Horizontal");
        //moveVertical = Input.GetAxis("Vertical");
        RightController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 axisValue);
        moveHorizontal = axisValue.x;
        moveVertical = axisValue.y;
        Transform[] ts = target.GetComponentsInChildren<Transform>();

        if (ts.Length > 1)
        {

           /* moveX = Input.GetAxis("Mouse X");
            moveY = Input.GetAxis("Mouse Y");*/
        }
        LeftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 lookAxisValue);
        moveX = lookAxisValue.x;
        moveY = lookAxisValue.y;
    }

    void jetpackUse()
    {
        if (Input.GetKeyDown(KeyCode.E) && canUseJetPack)
        {
            rb.AddForce(Vector3.up * 45, ForceMode.Impulse);
            canUseJetPack = false;
            Physics.gravity += new Vector3(0, 1.2f, 0);
            OnNextBiome();
        }
    }

    public void OnNextBiome()
    {
        nextBiomeEvent?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler nextBiomeEvent;

    private void movement()
    {
        target.transform.Rotate(0, moveX * sensitivity, 0);
        cam.transform.Rotate(-moveY * sensitivity, 0, 0);
        target.transform.Translate(Vector3.right * moveHorizontal * moveSens * Time.deltaTime, Space.Self);
        target.transform.Translate(Vector3.forward * moveVertical * moveSens * Time.deltaTime, Space.Self);
    }

    private void jump()
    {
        RightController.TryGetFeatureValue(CommonUsages.primaryButton, out bool buttonPressed);
        Debug.Log(isGrounded);
        if ((Input.GetKeyDown(KeyCode.Space) || buttonPressed) && isGrounded)
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
        if (collision.gameObject.tag == "ground")
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "ground")
        { 
            isGrounded = false;
        }
    }






}

