using System;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public bool isLastPlaced = false;
    public int score = 1;
    public CubePlacement spawner; // Reference to the spawner

    private GameObject parent;
    [SerializeField] GameObject gameManager;
    [SerializeField] AudioSource place;

    void Start()
    {
        parent = GameObject.Find("Spawner");
        gameManager = GameObject.Find("GameManager");
       //gameObject.GetComponent<Renderer>().material = parent.GetComponent<Renderer>().material;
        spawner = parent.GetComponent<CubePlacement>(); // Get the CubePlacement component from the parent
        // spawner is set by CubePlacement when instantiated
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bounds"))
        {
            Debug.Log("Cube entered bounds trigger: " + other.gameObject.name);
            if (isLastPlaced && spawner != null)
            {
                spawner.OnCubeFellOff(this);
            }
           
            if (isLastPlaced && gameManager != null)
            {
                gameManager.GetComponent<GameManager>().GameState("End");
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cube"))
        {
            if (isLastPlaced && gameManager != null && spawner != null)
            {
                gameManager.GetComponent<GameManager>().AddScore(1);

                // Increment stacked count
               
                if(!place.isPlaying)
                {
                    place.Play(); // Play the placement sound
                }

                // Move bounds only every 10 blocks
                if (score % 1 == 0)
                {
                    Debug.Log("Bounds moved");
                    Vector3 cubePos = transform.position;
                    Vector3 boundsPos = spawner.bounds.transform.position;
                    spawner.bounds.transform.position = boundsPos + new Vector3(0, spawner.cubeHeight * 0.5f, 0);

                }
                //isLastPlaced = false;
            }
        }
    }
}