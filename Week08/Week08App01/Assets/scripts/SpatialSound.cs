using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialSound : MonoBehaviour
{
    public List<GameObject> soundSources;
    public GameObject actor;
    public float decayFactor = 1.0f;
    public float dropOff = 0.1f;

    private float[] lastDistance;
    private void Start()
    {
        lastDistance = new float[soundSources.Count];
    }
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < soundSources.Count; i++)
        {
            float distance = (soundSources[i].transform.position - actor.transform.position).magnitude;
            
            soundSources[i].GetComponent<AudioSource>().volume = 1.0f / Mathf.Pow(distance * dropOff, decayFactor);

            float s = (distance - lastDistance[i]) / Time.deltaTime;
            float c = 330.0f;
            soundSources[i].GetComponent<AudioSource>().pitch = (c + s) / (c - s);
            lastDistance[i] = distance;
        }
    }
}
