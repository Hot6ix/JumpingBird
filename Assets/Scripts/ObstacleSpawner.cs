using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public float spawnRate = 3;
    public float obstacleSpeed = 2.5f;
    private float lastSpawnTime;
    private bool isSpawning = false;
    private ArrayList rockList;

    public GameObject[] rocks;
    public bool spawnObstacles = true;

    // Start is called before the first frame update
    void Start()
    {
        rockList = new ArrayList();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.instance.isGameOver == false)
        {
            lastSpawnTime += Time.deltaTime;

            if (lastSpawnTime >= spawnRate && spawnObstacles)
            {
                isSpawning = true;
                lastSpawnTime = 0;

                GameObject rock = Instantiate(rocks[Random.Range(0, rocks.Length)]);
                rock.transform.position = new Vector2(15, -4);
                rockList.Add(rock);

                Debug.Log("Spawning obstacles...");
            }

            // move rocks
            if (rockList.Count > 0 && GameController.instance.isGameOver == false)
            {
                for (int i = 0; i < rockList.Count; i++)
                {
                    GameObject rock = (GameObject)rockList[i];
                    rock.transform.Translate(Vector2.left * Time.deltaTime * obstacleSpeed);

                    if (rock.transform.position.x < -15)
                    {
                        rockList.Remove(rock);
                        Destroy(rock);
                    }
                }
            }
        }
    }
}
