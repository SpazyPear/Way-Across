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
    private float posY;
    public Tweener tweener;
    private List<GameObject> pastPlatforms;
    private List<GameObject> toBeRemoved;
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
        toBeRemoved = new List<GameObject>();
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
        Vector2 currentPos = new Vector2((float)Math.Floor(posX), (float)Math.Floor(posZ));
        double relPosX = Math.Round(player.position.x);
        double relPosZ = Math.Round(player.position.z);
        double relPosY = Math.Round(player.position.y);
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

                posArray.Add(Instantiate(HellArray[UnityEngine.Random.Range(0, HellArray.Length - 1)], new Vector3(forward.x, -2f, forward.z), Quaternion.identity));
                posArray.Add(Instantiate(HellArray[UnityEngine.Random.Range(0, HellArray.Length - 1)], new Vector3(left.x, -2f, left.z), Quaternion.identity));
                posArray.Add(Instantiate(HellArray[UnityEngine.Random.Range(0, HellArray.Length - 1)], new Vector3(right.x, -2f, right.z), Quaternion.identity));

            }
            else if (player.position.y > 2 && player.position.y < 14)
            {
                posArray.Add(Instantiate(ForestArray[UnityEngine.Random.Range(0, ForestArray.Length)], new Vector3(forward.x, -2f, forward.z), Quaternion.identity));
                posArray.Add(Instantiate(ForestArray[UnityEngine.Random.Range(0, ForestArray.Length)], new Vector3(left.x, -2f, left.z), Quaternion.identity));
                posArray.Add(Instantiate(ForestArray[UnityEngine.Random.Range(0, ForestArray.Length)], new Vector3(right.x, -2f, right.z), Quaternion.identity));

                /*if (posY == relPosY)
                {

                    switch (UnityEngine.Random.Range(0, 3))
                    {

                        case 0:
                            break;

                        case 1:

                            GameObject Tree = Instantiate(ForestArray[2], new Vector3(left.x, -2f, left.z), Quaternion.identity);
                            tweener.AddTween(Tree.transform, Tree.transform.position, new Vector3(Tree.transform.position.x, nearestMultiple(Convert.ToInt32(player.position.y - 4) + 1), Tree.transform.position.z), 4f);
                            break;

                        case 2:

                            GameObject TreeR = Instantiate(ForestArray[2], new Vector3(right.x, -2f, right.z), Quaternion.identity);
                            tweener.AddTween(TreeR.transform, TreeR.transform.position, new Vector3(TreeR.transform.position.x, nearestMultiple(Convert.ToInt32(player.position.y - 4) + 1), TreeR.transform.position.z), 4f);
                            break;

                    }
                }
                
            } */
            }

            else if (player.position.y > 14)
            {
                posArray.Add(Instantiate(WinterArray[UnityEngine.Random.Range(0, WinterArray.Length)], new Vector3(forward.x, -2f, forward.z), Quaternion.identity));
                posArray.Add(Instantiate(WinterArray[UnityEngine.Random.Range(0, WinterArray.Length)], new Vector3(left.x, -2f, left.z), Quaternion.identity));
                posArray.Add(Instantiate(WinterArray[UnityEngine.Random.Range(0, WinterArray.Length)], new Vector3(right.x, -2f, right.z), Quaternion.identity));
            }

            foreach (GameObject obj in posArray)
            {
                Debug.Log("pos");
                tweener.AddTween(obj.transform, obj.transform.position, new Vector3(obj.transform.position.x, nearestMultiple(Convert.ToInt32(player.position.y - 4)), obj.transform.position.z), 1.5f);
            }

            foreach (GameObject obj in pastPlatforms)
            {
                if (Vector3.Distance(player.transform.position, obj.transform.position) > 15)
                {
                    
                    GameObject hellObj = Instantiate(HellArray[UnityEngine.Random.Range(0, HellArray.Length - 1)], new Vector3(obj.transform.position.x, obj.transform.position.y, obj.transform.position.z), Quaternion.identity);
                    tweener.AddTween(hellObj.transform, hellObj.transform.position, new Vector3(hellObj.transform.position.x, -20f, hellObj.transform.position.z), 3f);
                    toBeRemoved.Add(obj);
                }
            }

            foreach (GameObject obj in toBeRemoved)
            {
                pastPlatforms.Remove(obj);
                Destroy(obj);

                
            }

            toBeRemoved.Clear();

            posY = player.position.y;
        }
    }
    Vector3 roundVector3(Vector3 pos)
    {
        return new Vector3(nearestMultiple(Convert.ToInt32(Mathf.Round(pos.x))), Mathf.Round(pos.y), nearestMultiple(Convert.ToInt32(Mathf.Round(pos.z))));
    }

    int nearestMultiple(int num)
    {

        num = num % 2 == 0 ? num : num + 1;
       
        if (num < 0)
        {
            int remainderNeg = num % 4;
            return num + remainderNeg;
        }


        int remainder = num % 4;
      

        return num - remainder;
    }
}
