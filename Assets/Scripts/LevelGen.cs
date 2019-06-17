using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGen : MonoBehaviour
{

    public Camera playerCamera;
    public GameObject floorPrefab;
    public GameObject railPrefab;
    public GameObject obsticalPrefab;
    public Queue<GameObject> liveFloors;
    public Queue<GameObject> liveRails;
    public Queue<GameObject> liveObsticals;
    public Queue<GameObject> deadFloors;
    public Queue<GameObject> deadRails;
    public Queue<GameObject> deadObsticals;

    public int numberOfPools =40;

    public float lastPosition =0;
    public float trackBuffer;
    public float placementBuffer;

    public float chanceOfRail;
    public float chanceOfObstical;

    public float minRailHeight;
    public float maxRailHeight;
    public float minRailLength;
    public float maxRailLength;

    // Start is called before the first frame update
    void Start()
    {
        liveRails = new Queue<GameObject>();
        deadRails = new Queue<GameObject>();
        for (int i = 0; i < numberOfPools; i++)
        {
            GameObject rail =Instantiate(railPrefab, transform);
            deadRails.Enqueue(rail);
            rail.SetActive(false);
        }

        liveFloors = new Queue<GameObject>();
        deadFloors = new Queue<GameObject>();
        for (int i = 0; i < numberOfPools; i++)
        {
            GameObject floor =Instantiate(floorPrefab, transform);
            deadFloors.Enqueue(floor);
            floor.SetActive(false);
        }

        liveObsticals = new Queue<GameObject>();
        deadObsticals = new Queue<GameObject>();
        for (int i = 0; i < numberOfPools; i++)
        {
            GameObject obstical = Instantiate(obsticalPrefab, transform);
            deadObsticals.Enqueue(obstical);
            obstical.SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCamera.transform.position.x > (lastPosition + trackBuffer))
        {
            lastPosition = playerCamera.transform.position.x;

            CreateFloor(new Vector3(lastPosition + placementBuffer, 0.0f));
            if (Random.Range(0, 100) <= chanceOfObstical)
            {
                CreateObstical(new Vector3(lastPosition + placementBuffer, 1.0f));

            }
            if (Random.Range(0, 100) <= chanceOfRail)
            {
                GameObject rail= CreateRail(new Vector3(lastPosition + placementBuffer, Random.Range(minRailHeight, maxRailHeight)));
                rail.transform.localScale = new Vector2( Random.Range(minRailLength, maxRailLength),1);
            }

        }

    }

    private GameObject CreateFloor(Vector3 position)
    {
        //Get floor from dead list
        GameObject floor = deadFloors.Dequeue();
        liveFloors.Enqueue(floor);
        floor.SetActive(true);

        //check if we can start getting rid of old floors
        if (liveFloors.Count > 10)
        {
            GameObject deadFloor = liveFloors.Dequeue();
            deadFloor.SetActive(false);
            deadFloors.Enqueue(deadFloor);
        }

        //set pos of new floor
        floor.transform.position = position;
        return floor;
    }

    private GameObject CreateRail(Vector3 position)
    {
        //Get rail from dead list
        GameObject rail = deadRails.Dequeue();
        liveRails.Enqueue(rail);
        rail.SetActive(true);

        //check if we can start getting rid of old rail
        if (liveRails.Count > 10)
        {
            GameObject deadRail = liveRails.Dequeue();
            deadRail.SetActive(false);
            deadFloors.Enqueue(deadRail);
        }

        //set pos of new rail
        rail.transform.position = position;
        return rail;
    }

    private GameObject CreateObstical(Vector3 position)
    {
        //Get rail from dead list
        GameObject obstical = deadObsticals.Dequeue();
        liveObsticals.Enqueue(obstical);
        obstical.SetActive(true);

        //check if we can start getting rid of old rail
        if (liveObsticals.Count > 10)
        {
            GameObject deadObstical = liveObsticals.Dequeue();
            deadObstical.SetActive(false);
            deadFloors.Enqueue(deadObstical);
        }

        //set pos of new rail
        obstical.transform.position = position;
        return obstical;
    }
}
