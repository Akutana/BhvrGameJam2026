using System.Collections.Generic;
using UnityEngine;

public class ChunkContentManager : MonoBehaviour
{
    // To hide objects, ChunkContentManager.Instance.HideObstacles();
    // To show objects, ChunkContentManager.Instance.ShowObstacles();
    
    public static ChunkContentManager Instance;

    private List<GameObject> obstacles = new List<GameObject>();

    public bool obstaclesVisible = true;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Find all objects tagged as "Obstacle"
        GameObject[] found = GameObject.FindGameObjectsWithTag("Obstacle");

        foreach (GameObject obj in found)
        {
            obstacles.Add(obj);
        }
    }

    void Update()
    {
        // TEST: press H to toggle visibility
        if (Input.GetKeyDown(KeyCode.H))
        {
            ToggleObstacles();
        }
    }

    public void HideObstacles()
    {
        obstaclesVisible = false;

        foreach (GameObject obj in obstacles)
        {
            obj.SetActive(false);
        }
    }

    public void ShowObstacles()
    {
        obstaclesVisible = true;

        foreach (GameObject obj in obstacles)
        {
            obj.SetActive(true);
        }
    }

    public void ToggleObstacles()
    {
        if (obstaclesVisible)
            HideObstacles();
        else
            ShowObstacles();
    }
}