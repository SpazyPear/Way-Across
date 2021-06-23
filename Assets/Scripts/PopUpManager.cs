using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    public Transform player;
    public GameObject prefab;
    private List<GameObject> posArray;
    private Vector2 currentPos;
    private float posX;
    private float posZ;
    public Tweener tweener;
    private List<GameObject> pastPlatforms;
    private bool jumped;
    private float relHeight;
    public GameObject[] ForestArray;
    public GameObject[] WinterArray;
    public GameObject[] HellArray;

    // Start is called before the first frame update
    void Start()
    {
        posArray = new List<GameObject>();
        pastPlatforms = new List<GameObject>();
        posX = player.position.x;
        posZ = player.position.z;
        relHeight = player.position.y;

    }

    // Update is called once per frame
    void Update()
    {

        pop();

    }



    void pop()
    {
        Debug.Log(player.forward + player.position);
        Vector2 currentPos = new Vector2((float)Math.Floor(posX), (float)Math.Floor(posZ));
        double relPosX = Math.Round(player.position.x);
        double relPosZ = Math.Round(player.position.z);
        if (relPosX > posX + 2 || relPosZ > posZ + 2 || relPosX < posX - 2 || relPosZ < posZ - 2 && player.GetComponent<Movement>().isGrounded)
        {
            // if (Physics.CheckSphere(player.forward + player.position, 1f) == true)
            //{



            posX = player.position.x;
            posZ = player.position.z;


            foreach (GameObject obj in posArray)
            {
                pastPlatforms.Add(obj);
            }

            posArray.Clear();

            Vector3 forward = roundVector3(player.forward * 5 + player.position);
            Vector3 left = roundVector3(player.right * 2 + player.forward * 5 + player.position);
            Vector3 right = roundVector3(-player.right * 2 + player.forward * 5 + player.position);
            if (player.position.y < 3)
            {

                posArray.Add(Instantiate(HellArray[UnityEngine.Random.Range(0, ForestArray.Length - 1)], new Vector3(forward.x, -2f, forward.z), Quaternion.identity));
                posArray.Add(Instantiate(HellArray[UnityEngine.Random.Range(0, ForestArray.Length - 1)], new Vector3(left.x, -2f, left.z), Quaternion.identity));
                posArray.Add(Instantiate(HellArray[UnityEngine.Random.Range(0, ForestArray.Length - 1)], new Vector3(right.x, -2f, right.z), Quaternion.identity));
                
            }
            else if (player.position.y > 2)
            {
                posArray.Add(Instantiate(ForestArray[UnityEngine.Random.Range(0, ForestArray.Length-1)], new Vector3(forward.x, -2f, forward.z), Quaternion.identity));
                posArray.Add(Instantiate(ForestArray[UnityEngine.Random.Range(0, ForestArray.Length-1)], new Vector3(left.x, -2f, left.z), Quaternion.identity));
                posArray.Add(Instantiate(ForestArray[UnityEngine.Random.Range(0, ForestArray.Length-1)], new Vector3(right.x, -2f, right.z), Quaternion.identity));
                GameObject Tree = Instantiate(ForestArray[2], new Vector3(left.x, -2f, left.z), Quaternion.identity);
                tweener.AddTween(Tree.transform, Tree.transform.position, new Vector3(Tree.transform.position.x, nearestMultiple(Convert.ToInt32(player.position.y - 4) + 1), Tree.transform.position.z), 4f);
            }
           /* posArray.Add(Instantiate(prefab, new Vector3(forward.x, -2f, forward.z), Quaternion.identity));
            posArray.Add(Instantiate(prefab, new Vector3(left.x, -2f, left.z), Quaternion.identity));
            posArray.Add(Instantiate(prefab, new Vector3(right.x, -2f, right.z), Quaternion.identity)); */

            foreach (GameObject obj in pastPlatforms)
            {
                if (Vector3.Distance(player.transform.position, obj.transform.position) > 15)
                {
                    tweener.AddTween(obj.transform, obj.transform.position, new Vector3(obj.transform.position.x, -20f, obj.transform.position.z), 2f);
                }
            }

            foreach (GameObject obj in posArray)
            {
                Debug.Log("pos");
                tweener.AddTween(obj.transform, obj.transform.position, new Vector3(obj.transform.position.x, nearestMultiple(Convert.ToInt32(player.position.y - 4)), obj.transform.position.z), 1.5f);
            }
            Debug.Log("popped");
        }
    }
    Vector3 roundVector3(Vector3 pos)
    {
        return new Vector3(nearestMultiple(Convert.ToInt32(Mathf.Round(pos.x))), Mathf.Round(pos.y), nearestMultiple(Convert.ToInt32(Mathf.Round(pos.z))));
    }

    int nearestMultiple(int num) //isnt working with negatives
    {
        num -= 1;
        if (num < 0)
        {
            int remainderNeg = num % 4;
            return num + 4 + remainderNeg;
        }

        int remainder = num % 4;
      

        return num + 4 - remainder;
    }
}
