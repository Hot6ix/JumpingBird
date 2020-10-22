using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundObjectController : MonoBehaviour
{
    private Camera camera;
    private Renderer renderer;
    private float backgroundStartX;
    private float backgroundEndX;
    private float cameraHalfHeight;
    private float cameraHalfWidth;

    public Sprite cloud_image;
    public int cloudsOnStart;
    private ArrayList cloudList;
    private float cloudSpawnRate;
    private float cloudSpawnTime;
    private int cloudId = 0;

    public Sprite[] mountain_image;
    public int mountainOnStart;
    private ArrayList mountainList;
    private int mountainId = 0;

    private AudioSource audioSource;

    // If speed is 0.05f and move x + 1, then takes about 20s.

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        renderer = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();

        cameraHalfHeight = camera.orthographicSize;
        cameraHalfWidth = cameraHalfHeight * camera.aspect;

        backgroundStartX = -(renderer.bounds.size.x / 2);
        backgroundEndX = renderer.bounds.size.x / 2;

        // Spawn clouds
        if (cloudsOnStart <= 0) cloudsOnStart = 3;
        cloudSpawnRate = Random.Range(5f, 10f);
        cloudSpawnTime = 0f;

        cloudList = new ArrayList();

        float cloudPositionX = -8;
        float cloudPositionY;

        for (int i = 0; i < cloudsOnStart; i++)
        {
            cloudId = i;

            if (i > 0)
            {
                cloudPositionX += Random.Range(5f, 10f);
            }
            cloudPositionY = Random.Range(1.0f, 3.5f);

            Cloud cloud = createCloud(cloudId, cloudPositionX, cloudPositionY);
            cloudList.Add(cloud);
        }

        // Spawn mountains
        if (mountainOnStart <= 0) mountainOnStart = 5;

        mountainList = new ArrayList();

        float mountainPositionX = -9.5f;
        float mountainPositionY = 0;

        for(int i = 0; i < mountainOnStart; i++)
        {
            mountainId = i;

            if (i > 0)
            {
                Mountain lastMountain = (Mountain) mountainList[i - 1];
                float lastMtWidth = lastMountain.GetObject().GetComponent<Renderer>().bounds.size.x;
                mountainPositionX += (lastMtWidth / 2) + Random.Range(-0.3f, 0.3f);
            }

            Mountain mountain = createMountain(mountainId, mountainPositionX, mountainPositionY);
            mountainList.Add(mountain);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(GameController.instance.isGameOver == false)
        {
            cloudSpawnTime += Time.deltaTime;

            // move clouds
            for (int i = 0; i < cloudList.Count; i++)
            {
                Cloud cloud = (Cloud)cloudList[i];
                cloud.GetObject().transform.Translate(Vector2.left * Time.deltaTime * cloud.GetSpeed());
                if (cloud.GetObject().transform.position.x < backgroundStartX)
                {
                    cloudList.Remove(cloud);
                    Destroy(cloud.GetObject());
                }
            }

            // check all clouds
            bool areCloudsSeparated = false;

            for (int i = 0; i < cloudList.Count; i++)
            {
                Cloud cloud = (Cloud)cloudList[i];
                if (cloud.GetObject().transform.position.x >= 0)
                {
                    areCloudsSeparated = true;
                    break;
                }
            }

            // create new cloud if current clouds are in left area
            if (cloudSpawnTime >= cloudSpawnRate || !areCloudsSeparated)
            {
                cloudSpawnTime = 0f;
                cloudSpawnRate = Random.Range(10f, 30f);
                Cloud cloud = createCloud(++cloudId, backgroundEndX, Random.Range(2.0f, 4.0f));
                cloudList.Add(cloud);
            }

            // move mountains
            for (int i = 0; i < mountainList.Count; i++)
            {
                Mountain mountain = (Mountain)mountainList[i];
                mountain.GetObject().transform.Translate(Vector2.left * Time.deltaTime * mountain.GetSpeed());
                if (mountain.GetObject().transform.position.x < backgroundStartX)
                {
                    mountainList.Remove(mountain);
                    Destroy(mountain.GetObject());
                }
            }

            // spawn mountain in condition
            Mountain lastSpawnedMountain = (Mountain)mountainList[mountainList.Count - 1];
            if (lastSpawnedMountain.GetObject().transform.position.x <= backgroundEndX)
            {
                float lastSpawnedMtPositionX = lastSpawnedMountain.GetObject().transform.position.x;
                float lasSpawnedMtWidth = lastSpawnedMountain.GetObject().GetComponent<Renderer>().bounds.size.x;
                float nextPosition = lastSpawnedMtPositionX + (lasSpawnedMtWidth / 2);

                Mountain mountain = createMountain(++mountainId, nextPosition, 0);
                mountainList.Add(mountain);
            }
        }
    }

    Cloud createCloud(int id, float x, float y)
    {
        float cloudSize = Random.Range(5, 11);
        float cloudSpeed = Random.Range(0.1f, 0.3f);

        GameObject cloudObj = new GameObject();
        cloudObj.AddComponent(typeof(SpriteRenderer));
        cloudObj.GetComponent<SpriteRenderer>().sprite = cloud_image;
        cloudObj.GetComponent<SpriteRenderer>().sortingLayerName = "Background_Obj";
        cloudObj.GetComponent<SpriteRenderer>().sortingOrder = (int)cloudSize;
        cloudObj.transform.localScale = new Vector2(cloudSize, cloudSize);
        cloudObj.transform.position = new Vector2(x, y);
        cloudObj.name = "cloud_" + id;

        Debug.Log("Spawning Cloud - name=" + "cloud_" + id + ", size=" + cloudSize + ", speed=" + cloudSpeed);
        return new Cloud(cloudObj, id, cloudSpeed, cloudSize);
    }

    Mountain createMountain(int id, float x, float y)
    {
        float mountainSize = Random.Range(5, 11);
        if (mountainSize == 5) y = -1.5f;
        else if (mountainSize == 5) y = -1.5f;
        else if (mountainSize == 6) y = -1f;
        else if (mountainSize == 7) y = -0.5f;
        else if (mountainSize == 8) y = 0f;
        else if (mountainSize == 9) y = 0.5f;
        else if (mountainSize == 10) y = 1f;

        float mountainSpeed = mountainSize / 300;

        GameObject mountainObj = new GameObject();
        mountainObj.AddComponent(typeof(SpriteRenderer));
        mountainObj.GetComponent<SpriteRenderer>().sprite = mountain_image[Random.Range(0, mountain_image.Length)];
        mountainObj.GetComponent<SpriteRenderer>().sortingLayerName = "Background_Obj";
        mountainObj.GetComponent<SpriteRenderer>().sortingOrder = (int) mountainSize;
        mountainObj.transform.localScale = new Vector2(mountainSize, mountainSize);
        mountainObj.transform.position = new Vector2(x, y);
        mountainObj.name = "mountain_" + id;

        Debug.Log("Spawning Mountain - name=" + "mountain_" + id + ", size=" + mountainSize + ", speed=" + mountainSpeed);
        return new Mountain(mountainObj, id, mountainSpeed, mountainSize);
    }
}

class Cloud
{
    private int id;
    private GameObject cloud;
    private float speed;
    private float size;

    public Cloud(GameObject cloud, int id, float speed, float size)
    {
        this.cloud = cloud;
        this.id = id;
        this.speed = speed;
        this.size = size;
    }

    public GameObject GetObject()
    {
        return cloud;
    }

    public int GetId()
    {
        return id;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public float GetSize()
    {
        return size;
    }

    public void SetSize(float size)
    {
        this.size = size;
    }
}

class Mountain
{
    private int id;
    private GameObject mountain;
    private float speed;
    private float size;

    public Mountain(GameObject mountain, int id, float speed, float size)
    {
        this.mountain = mountain;
        this.id = id;
        this.speed = speed;
        this.size = size;
    }

    public GameObject GetObject()
    {
        return mountain;
    }

    public int GetId()
    {
        return id;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public float GetSize()
    {
        return size;
    }

    public void SetSize(float size)
    {
        this.size = size;
    }
}