using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms.GameCenter;

public class MapGenerator : MonoBehaviour
{
    int itemSpace = 15;
    int itemCountInMap = 5;
    public float laneOffset = 2.5f;
    int coinsCountInItem = 10;
    float coinsHeight = 0.5f;
    int mapSize;
    enum TrackPos {Left = -1, Center = 0, Right = 1};
    enum CoinsStyle {Line, Jump}

    public GameObject TopWallPrefab;
    public GameObject FullWallPrefab;
    public GameObject BottomWallPrefab;
    public GameObject CoinPrefab;

    public List<GameObject> maps = new List<GameObject>();
    public List<GameObject> activeMaps = new List<GameObject>();

    static public MapGenerator instance;

    struct MapItem
    {
        public void SetValues(GameObject obstacle, TrackPos trackPos, CoinsStyle coinsStyle)
        {
            this.obstacle = obstacle; this.trackPos = trackPos; this.coinsStyle = coinsStyle;
        }
        public GameObject obstacle;
        public TrackPos trackPos;
        public CoinsStyle coinsStyle;
    }

    private void Awake()
    {
        instance = this;
        mapSize = itemCountInMap * itemSpace;
        maps.Add(MakeMap1());
        maps.Add(MakeMap1());
        maps.Add(MakeMap1());
        foreach (GameObject map in maps)
        {
            map.SetActive(false);
        }
    }
    void Start()
    {
        
    }

    void Update()
    {
        if (RoadGenerator.instance.speed == 0) return;
        {

            foreach(GameObject map in activeMaps)
            {
                map.transform.position -= new Vector3(0, 0, RoadGenerator.instance.speed * Time.deltaTime);
            }
        }
        if (activeMaps[0].transform.position.z < -mapSize)
        {
            RemoveFirstActiveMap();
            AddActiveMap();
        }
        
    }

    void RemoveFirstActiveMap()
    {
        activeMaps[0].SetActive(false);
        maps.Add(activeMaps[0]);
        activeMaps.RemoveAt(0);
    }

    public void ResetMaps()
    {
        while (activeMaps.Count > 0)
        {
            RemoveFirstActiveMap();
        }
        AddActiveMap();
        AddActiveMap();
    }

    void AddActiveMap()
    {
        int r = UnityEngine.Random.Range(0, maps.Count);
        GameObject go = maps[r];
        go.SetActive(true);
        foreach (Transform child in go.transform) 
        {
            child.gameObject.SetActive(true);
        }
        go.transform.position = activeMaps.Count > 0 ?
                                activeMaps[activeMaps.Count-1].transform.position + Vector3.forward * mapSize :
                                new Vector3(0, 0, 10);
        maps.RemoveAt(r);
        activeMaps.Add(go);
    }

    GameObject MakeMap1()
    {
        GameObject result = new GameObject("Map1");
        result.transform.SetParent(transform);
        for (int i = 0; i < itemCountInMap; i++)
        {
            GameObject obstacle = null;
            TrackPos trackPos = TrackPos.Center;
            CoinsStyle coinStyle = CoinsStyle.Line;

            Vector3 obstaclePos = new Vector3((int)trackPos * laneOffset, 1, i * itemSpace);
            if (i == 2)
            {
                trackPos = TrackPos.Left; obstacle = BottomWallPrefab; coinStyle = CoinsStyle.Jump;
                obstaclePos = new Vector3((int)trackPos * laneOffset, 1, i * itemSpace);
            }
            else if (i == 3)
            {
                trackPos = TrackPos.Right; obstacle = FullWallPrefab;
                obstaclePos = new Vector3((int)trackPos * laneOffset, 1.5f, i * itemSpace);
            }
            else if (i == 4)
            {
                trackPos = TrackPos.Center; obstacle = TopWallPrefab; coinStyle = CoinsStyle.Line;
                obstaclePos = new Vector3((int)trackPos * laneOffset, 1.7f, i * itemSpace);
            }

            CreateCoins(coinStyle, obstaclePos, result);

            if (obstacle != null)
            {
                GameObject go = Instantiate(obstacle, obstaclePos, Quaternion.identity);
                go.transform.SetParent(result.transform);
            }
        }
        return result;
    }

    void CreateCoins (CoinsStyle style, Vector3 pos, GameObject parentObject)
    {
        Vector3 coinsPos = Vector3.zero;
        if (style == CoinsStyle.Line)
        {
            for (int i = -coinsCountInItem/2; i < coinsCountInItem / 2; i++)
            {
                coinsPos.y = coinsHeight + 1;
                coinsPos.z = i * ((float)itemSpace / coinsCountInItem);
                GameObject go = Instantiate(CoinPrefab, coinsPos, Quaternion.identity);
                go.transform.SetParent(parentObject.transform);
            }
        }
        else if (style == CoinsStyle.Jump)
        {
            for (int i = -coinsCountInItem/2; i < coinsCountInItem / 2; i++)
            {
                coinsPos.y = Mathf.Max(-1/2f * Mathf.Pow(i, 2) + 3, coinsHeight);
                coinsPos.z = i * ((float)itemSpace / coinsCountInItem);
                GameObject go = Instantiate(CoinPrefab, coinsPos + pos, Quaternion.identity);
                go.transform.SetParent(parentObject.transform);
            }
        }
    }
}
