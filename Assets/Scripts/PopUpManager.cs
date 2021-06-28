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
    private Vector3 forward;
    private Vector3 left;
    private Vector3 right;
    private enum biome { Hell, Grass, Ice };
    private biome currentBiome;
    private Del popMethodGroup;

    delegate void Del();


    // Start is called before the first frame update
    void Start()
    {
        posArray = new List<GameObject>();
        pastPlatforms = new List<GameObject>();
        toBeRemoved = new List<GameObject>();
        posX = player.position.x;
        posZ = player.position.z;
        relHeight = player.position.y;
        Del biomeSet = setBiome;
        Del builder = posChangedInstantiate;
        popMethodGroup = biomeSet + builder;

    }

    // Update is called once per frame
    void Update()
    {
        popMethodGroup();
        /*Debug.Log("pos " + posArray.Count);
        Debug.Log("past " + pastPlatforms.Count);
        Debug.Log("removed " + toBeRemoved.Count); */ 
    }

    void setBiome()
    {
        if (player.position.y < 3)
        {
            currentBiome = biome.Hell;
        }
        else if(player.position.y > 2 && player.position.y < 14)
        {
            currentBiome = biome.Grass;
        }
        else if (player.position.y > 14)
        {
            currentBiome = biome.Ice;
        }
    }

    void posChangedInstantiate()
    {
        double relPosX = Math.Round(player.position.x);
        double relPosZ = Math.Round(player.position.z);
        double relPosY = Math.Round(player.position.y);
        if (relPosX > posX + 2 || relPosZ > posZ + 2 || relPosX < posX - 2 || relPosZ < posZ - 2)
        {
            posX = player.position.x;
            posZ = player.position.z;


            foreach (GameObject obj in posArray)
            {
                pastPlatforms.Add(obj);
            }

            posArray.Clear();

            forward = roundVector3(player.forward * 5 + player.position);
            left = roundVector3(player.right * 2 + player.forward * 5 + player.position);
            right = roundVector3(-player.right * 2 + player.forward * 5 + player.position);

            switch (currentBiome)
            {
                case biome.Hell:
                    popBiome(HellArray);
                    break;

                case biome.Grass:
                    popBiome(ForestArray);
                    break;

                case biome.Ice:
                    popBiome(WinterArray);
                    break;
            }


            tweenerManager();

        }
    }

    void tweenerManager()
    {
        foreach (GameObject obj in posArray)
        {
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

    void popBiome(GameObject[] biomeArray)
    {
        posArray.Add(Instantiate(biomeArray[UnityEngine.Random.Range(0, biomeArray.Length)], new Vector3(forward.x, -2f, forward.z), Quaternion.identity));
        posArray.Add(Instantiate(biomeArray[UnityEngine.Random.Range(0, biomeArray.Length)], new Vector3(left.x, -2f, left.z), Quaternion.identity));
        posArray.Add(Instantiate(biomeArray[UnityEngine.Random.Range(0, biomeArray.Length)], new Vector3(right.x, -2f, right.z), Quaternion.identity));
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
