using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweener : MonoBehaviour
{
   // private Tween activeTween;
    private List<Tween> activeTweens;
    private List<Tween> toBeRemoved;

    // Start is called before the first frame update
    void Start()
    {
        activeTweens = new List<Tween>();
        toBeRemoved = new List<Tween>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Tween activeTween in activeTweens)
        {
            if (activeTween.Target != null)
            {
                if (Vector3.Distance(activeTween.Target.position, activeTween.EndPos) > 0.1f)
                {
                    float timeFraction = (Time.time - activeTween.StartTime) / activeTween.Duration;
                    float newTime = Mathf.Pow(timeFraction, 2);
                    activeTween.Target.position = Vector3.Lerp(activeTween.Target.position, activeTween.EndPos, timeFraction);

                }
                else
                {
                    activeTween.Target.position = activeTween.EndPos;
                    toBeRemoved.Add(activeTween);
                    //Cursor.lockState = CursorLockMode.Confined;
                }
            }
        }

        foreach (Tween tween in toBeRemoved)
        {
            activeTweens.Remove(tween);
        }
    }

    public void AddTween(Transform targetObject, Vector3 startPos, Vector3 endPos, float duration)
    {
       
            activeTweens.Add(new Tween(targetObject, startPos, endPos, Time.time, duration));
        
    }

   

}
