using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphabetSpawner : MonoBehaviour
{
    public Tweener tweener;
    Vector3 nextPos;
    int currentChar = 0;
    public string text;
    char[] characters;
    public Transform player;
    float nextPosPlatforms = 200;
    // Start is called before the first frame update
    void Start()
    {
        characters = text.ToCharArray();
        nextPos = new Vector3(0, 8, 256);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.position, nextPos) < 300f) {

            if (characters[currentChar] == ' ')
            {
                nextPos.x += 20;
                currentChar++;
                return;
            }

            GameObject prefab;

            if (characters[currentChar] == '.')
            {
                prefab = Resources.Load("Alphabet Prefabs/dot") as GameObject;
            }
            else
            {
                prefab = Resources.Load("Alphabet Prefabs/" + characters[currentChar].ToString()) as GameObject;
            }
            
            prefab = Instantiate(prefab, new Vector3(nextPos.x, nextPos.y - 256, nextPos.z), Quaternion.identity);
            tweenChildren(prefab.transform);
            currentChar++;
            nextPos.x += 20;
        }

        if (player.position.x > nextPosPlatforms)
        {
            GameObject prefab = Resources.Load("Beginning Platforms") as GameObject;
            Instantiate(prefab, new Vector3(nextPosPlatforms, 0f, 0f), Quaternion.identity);
            nextPosPlatforms += 400;
        }
    }

    void tweenChildren(Transform prefab)
    {
        for (int x = 0; x < prefab.childCount; x++)
        {
            tweener.AddTween(prefab.GetChild(x), prefab.GetChild(x).position, new Vector3(prefab.GetChild(x).position.x, prefab.GetChild(x).position.y + 256, prefab.GetChild(x).position.z), Random.Range(6.8f, 30f));
            //yield return new WaitForSeconds(0.1f);
        }
    }

}
