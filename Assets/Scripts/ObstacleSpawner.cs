using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Ссылки")]
    public Transform player;
    public GameObject obstaclePrefab;
    public GameObject dataPrefab;
    public GameObject laserPrefab;

    [Header("Настройки трубы")]
    public float radius = 5f;

    [Header("Настройки сложности")]
    public float spawnDistanceAhead = 200f;
    public float initialSpaceBetweenPatterns = 40f;
    public float minSpaceBetweenPatterns = 15f;
    public float difficultyRampUp = 0.5f;

    private float currentSpace;
    private float nextSpawnZ = 50f;

    void Start()
    {
        currentSpace = initialSpaceBetweenPatterns;
    }

    void Update()
    {
        while (player.position.z + spawnDistanceAhead > nextSpawnZ)
        {
            SpawnRandomPattern();

            if (currentSpace > minSpaceBetweenPatterns)
            {
                currentSpace -= difficultyRampUp;
            }

            nextSpawnZ += currentSpace;
        }
    }

    void SpawnRandomPattern()
    {
        int patternType = Random.Range(0, 4);

        if (patternType == 0) SpawnDataLine();
        else if (patternType == 1 || patternType == 3) SpawnRandomObstacles();
        else if (patternType == 2) SpawnLaserGate();
    }

    void SpawnDataLine()
    {
        float angle = Random.Range(0, 360f);
        int count = Random.Range(3, 6);
        float lineRotSpeed = 0f;
        if (Random.value < 0.3f)
        {
            lineRotSpeed = Random.Range(30f, 60f);
            if (Random.value > 0.5f) lineRotSpeed = -lineRotSpeed;
        }

        for (int i = 0; i < count; i++)
        {
            float zPos = nextSpawnZ + (i * 6f);
            SpawnObject(dataPrefab, angle, zPos, lineRotSpeed);
        }
        nextSpawnZ += count * 6f;
    }

    void SpawnRandomObstacles()
    {
        int count = Random.Range(3, 8);
        for (int i = 0; i < count; i++)
        {
            float angle = Random.Range(0, 360f);
            float rotSpeed = 0f;
            if (Random.value < 0.4f)
            {
                rotSpeed = Random.Range(50f, 120f);
                if (Random.value > 0.5f) rotSpeed = -rotSpeed;
            }

            SpawnObject(obstaclePrefab, angle, nextSpawnZ + Random.Range(-5f, 5f), rotSpeed);
        }
    }

    void SpawnLaserGate()
    {
        int safeQuadrant = Random.Range(0, 4);
        float gateRotSpeed = 0f;
        if (Random.value < 0.5f)
        {
            gateRotSpeed = Random.Range(40f, 90f);
            if (Random.value > 0.5f) gateRotSpeed = -gateRotSpeed;
        }

        for (int i = 0; i < 4; i++)
        {
            float angle = i * 90f;
            if (i == safeQuadrant)
            {
                SpawnObject(dataPrefab, angle, nextSpawnZ, gateRotSpeed);
            }
            else
            {
                SpawnObject(laserPrefab, angle, nextSpawnZ, gateRotSpeed);
            }
        }
    }

    void SpawnObject(GameObject prefab, float angle, float zPosition, float rotSpeed = 0f)
    {
        float rad = angle * Mathf.Deg2Rad;

        float x = Mathf.Cos(rad) * radius;
        float y = Mathf.Sin(rad) * radius;

        Vector3 spawnPos = new Vector3(x, y, zPosition);
        GameObject newObj = Instantiate(prefab, spawnPos, Quaternion.identity);

        Vector3 centerPos = new Vector3(0, 0, zPosition);
        newObj.transform.up = (spawnPos - centerPos).normalized;
        if (rotSpeed != 0f)
        {
            Rotator rotator = newObj.AddComponent<Rotator>();
            rotator.rotationSpeed = rotSpeed;
        }

        Destroy(newObj, 15f);
    }
}
