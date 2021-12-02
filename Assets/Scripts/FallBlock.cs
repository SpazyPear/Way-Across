using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallBlock : MonoBehaviour
{
    public Tweener tweener;
    private bool stationary = false;
    private bool falling = false;
    private Vector3 prevPos;
    private float timer = 5f;
    // Start is called before the first frame update
    void Start()
    {
        tweener = GameObject.Find("TweenManager").GetComponent<Tweener>();
        prevPos = Vector3.negativeInfinity; 
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position == prevPos && stationary == false)
        {
            stationary = true;
            Debug.Log("equal");
        }
        else
        {
            prevPos = transform.position;
            Debug.Log("set");
        }

        if (stationary == true)
        {
            timer -= Time.deltaTime;
            Debug.Log("timer");
        }

        if (timer < 0 && falling == false)
        {
            tweener.AddTween(transform, transform.position, new Vector3(transform.position.x, transform.position.y - 10, transform.position.z), 6.0f);
            falling = true;
            Debug.Log("tween");
        }
    }

}
