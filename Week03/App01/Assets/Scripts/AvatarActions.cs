using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
public class AvatarActions : MonoBehaviourPun
{
    [Tooltip("The prefab for objects created when the drop event is triggered.")]
    public GameObject blockTemplate;
    [Tooltip("Turn speed in degrees per second")]
    public float turnSpeed = 100.0f;

    [Tooltip("Movement speed in meters per second (assumes 1 unit = 1 meter)")]
    public float moveSpeed = 10.0f;

    public Vector3 SpawnArea = new Vector3(10, .25f, 10);

    private float turn = 0.0f;

    private float move = 0.0f;
    
    Rigidbody rb;
    
    void Start()
    {
        setButtonCallbacks();

        // to stop the player avatar falling on spawn
        rb = this.GetComponent<Rigidbody>();
        if (!rb.isKinematic) rb.isKinematic = true;
        if (rb.useGravity) rb.useGravity = false;
        StartCoroutine(Spawn());
    }

    private void Awake()
    {
        // set the players spawn post
        transform.SetPositionAndRotation(new Vector3(Random.Range(-SpawnArea.x, SpawnArea.x),
                                            transform.position.y, 
                                            Random.Range(-SpawnArea.z, SpawnArea.z)), transform.rotation);
    }

    IEnumerator Spawn()
    {
        // switch player rigidbody on to become interactable
        yield return new WaitForSeconds(1);
        rb.useGravity = true;
        rb.isKinematic = false;
    }
    public void Update()
    {
        if (photonView.IsMine == true || PhotonNetwork.IsConnected == false)
        {
            transform.rotation *= Quaternion.AngleAxis(turn * turnSpeed * Time.deltaTime, Vector3.up);
            transform.position += move * moveSpeed * transform.forward * Time.deltaTime;
            photonView.transform.position = transform.position;
            photonView.transform.rotation = transform.rotation;
        }
        else
            {
                transform.Find("head_H/Main Camera").gameObject.SetActive(false);
            }
    }
    private void dropObject()
    {
        if (photonView.IsMine == true || PhotonNetwork.IsConnected == false)
        {
            // Create the new object up and to the front, so it is away from the avatar.
            GameObject o = PhotonNetwork.Instantiate(blockTemplate.name, photonView.transform.position + photonView.transform.forward +
                        photonView.transform.up, Quaternion.identity);
        }
    }
    public void Left()
    {

        turn = -1.0f;
    }
    public void Right()
    {
        turn = 1.0f;
    }
    public void Forward()
    {
        move = 1.0f;
    }
    public void Drop()
    {
        dropObject();
    }
    public void Stop()
    {
        turn = 0.0f;
        move = 0.0f;
    }

    private void setButtonCallbacks()
    {
        // Assumes each button has already been set with two triggers; a pointer down followed 	by a pointer up.
        GameObject.Find("Canvas/ButtonTurnL").GetComponent<EventTrigger>().triggers[0].callback.AddListener((data) => { Left(); });

        GameObject.Find("Canvas/ButtonTurnL").GetComponent<EventTrigger>().triggers[1].callback.AddListener((data) => { Stop(); });

        GameObject.Find("Canvas/ButtonTurnR").GetComponent<EventTrigger>().triggers[0].callback.AddListener((data) => { Right(); });

        GameObject.Find("Canvas/ButtonTurnR").GetComponent<EventTrigger>().triggers[1].callback.AddListener((data) => { Stop(); });

        GameObject.Find("Canvas/ButtonUp").GetComponent<EventTrigger>().triggers[0].callback.AddListener((data) => { Forward(); });

        GameObject.Find("Canvas/ButtonUp").GetComponent<EventTrigger>().triggers[1].callback.AddListener((data) => { Stop(); });

        GameObject.Find("Canvas/ButtonDrop").GetComponent<EventTrigger>().triggers[0].callback.AddListener((data) => { Drop(); });

        GameObject.Find("Canvas/ButtonDrop").GetComponent<EventTrigger>().triggers[1].callback.AddListener((data) => { Stop(); });
    }
}