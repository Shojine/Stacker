using System.Collections.Generic;
using UnityEngine;

public class CubePlacement : MonoBehaviour
{
    [SerializeField] GameObject cubePrefab;
    [SerializeField] List<Material> materials = new List<Material>();
    [SerializeField] GameObject camera;
    [SerializeField] public GameObject bounds;
    [SerializeField] float speed;

    private GameObject lastPlacedCube = null;
    private GameObject previousCube = null;
    private float stackTolerance = 0.5f;
    public float cubeHeight = 1f; // Set this to your cube's actual height
    public int fallOffCount = 0;

    void Start()
    {
        // Set a random initial color for the spawner
        gameObject.GetComponent<Renderer>().material = materials[Random.Range(0, materials.Count)];
    }

    void Update()
    {
        // Move spawner left/right
        gameObject.transform.position = new Vector3(
            Mathf.Sin(Time.time) * speed,
            gameObject.transform.position.y,
            gameObject.transform.position.z
        );

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 1. Store the current spawner material
            Material currentMaterial = gameObject.GetComponent<Renderer>().material;

            // 2. Prepare spawn position
            Vector3 spawnPos = gameObject.transform.position;
            if (previousCube != null)
            {
                Cube prevCubeScript = previousCube.GetComponent<Cube>();
                if (prevCubeScript != null)
                    prevCubeScript.isLastPlaced = false;

                Vector3 prevPos = previousCube.transform.position;
                spawnPos.y = prevPos.y + cubeHeight;
            }

            // 3. Instantiate the cube
            lastPlacedCube = Instantiate(cubePrefab, spawnPos, Quaternion.identity);
            Cube cubeScript = lastPlacedCube.GetComponent<Cube>();
            if (cubeScript != null)
            {
                cubeScript.spawner = this;
                cubeScript.isLastPlaced = true;
            }

            // 4. Assign the stored material to the new cube
            lastPlacedCube.GetComponent<Renderer>().material = currentMaterial;

            // 5. Change the spawner's color for the next round
            gameObject.GetComponent<Renderer>().material = materials[Random.Range(0, materials.Count)];

            // 6. Update previousCube reference
            previousCube = lastPlacedCube;

            // 7. Move spawner up for next placement
            gameObject.transform.position = new Vector3(
                gameObject.transform.position.x,
                spawnPos.y,
                gameObject.transform.position.z
            );

            // 8. Optionally move camera up if needed (example: after 5 points)
            if (cubeScript != null)
            {
                camera.gameObject.transform.position = new Vector3(
                    camera.gameObject.transform.position.x,
                    spawnPos.y + 5, // Adjust the offset as needed
                    camera.gameObject.transform.position.z
                );
            }
        }
    }

    private bool IsCubeStackedCorrectly(GameObject current, GameObject previous)
    {
        if (previous == null) return true;

        Vector3 prevPos = previous.transform.position;
        Vector3 currPos = current.transform.position;

        bool isAbove = Mathf.Abs(currPos.x - prevPos.x) <= stackTolerance &&
                       Mathf.Abs(currPos.z - prevPos.z) <= stackTolerance &&
                       Mathf.Abs(currPos.y - (prevPos.y + cubeHeight)) <= 0.1f;

        return isAbove;
    }

    public void OnCubeFellOff(Cube cube)
    {
        fallOffCount++;
        Debug.Log("Cube fell off! Total fallen: " + fallOffCount);
        // Add additional logic here if needed (e.g., end game, reduce score, etc.)
    }
}
