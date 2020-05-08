using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ObjectRecogniser : MonoBehaviour
{
    public Text message;
    public GameObject markerPrefab;

    private ARTrackedImageManager arTrackedImageManager;
    void Awake()
    {
        arTrackedImageManager = GetComponent<ARTrackedImageManager>();
        arTrackedImageManager.trackedImagesChanged += OnImageChanged;
    }

    public void OnImageChanged(ARTrackedImagesChangedEventArgs _args)
    {
        message.text = "Image changed\n";
        foreach (var addedImage in _args.removed)
        {
            message.text += "Rem " + addedImage.referenceImage.name + "\n";
        }

        foreach (var updated in _args.updated)
        {
            //message.text += "Upd " + updated.referenceImage.name;
            if (updated.referenceImage.name.Equals("Marker"))
            {
                GameObject g = Instantiate(markerPrefab);
                g.transform.position = updated.transform.position;
            }
            //           allObjects[updated.referenceImage.name].transform.position = updated.transform.position;
            //           allObjects[updated.referenceImage.name].transform.rotation = updated.transform.rotation;
        }
    }

}