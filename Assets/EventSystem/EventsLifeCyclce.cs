using System.Collections;
using UnityEngine;

public class EventsLifeCyclce : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine("lifeCycle");
    }

    private IEnumerator lifeCycle()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            EventsLifeCyclceBackend.instance.publishBoofers();
        }
    }

}