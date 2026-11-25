using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public GameObject dataPacketPrefab;
    public Transform playerPivot;

    public float spawnDistance = 60f;
    public float spawnInterval = 0.5f;
    public float tubeRadius = 4.5f;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnObject();
            timer = 0f;
        }
    }

    void SpawnObject()
    {
        float randomAngle = Random.Range(0f, 360f);
        float spawnZ = playerPivot.position.z + spawnDistance;

        Vector3 basePos = new Vector3(0, -tubeRadius, 0);
        Vector3 rotatedPos = Quaternion.Euler(0, 0, randomAngle) * basePos;
        Vector3 finalPos = new Vector3(rotatedPos.x, rotatedPos.y, spawnZ);
        GameObject prefabToSpawn = Random.value > 0.5f ? obstaclePrefab : dataPacketPrefab;
        GameObject newObj = Instantiate(prefabToSpawn, finalPos, Quaternion.Euler(0, 0, randomAngle));
        Destroy(newObj, 15f);
    }
}