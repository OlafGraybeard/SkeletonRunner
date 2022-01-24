using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
	
	int itemSpace = 10;
	int itemCount = 7;
	public float laneOffset = 3f;
	
	enum TrackPos { left = -1, center = 0, right = 1 };
	
	public GameObject Enemy;
	
	int mapSize;
	
	public List<GameObject> maps = new List<GameObject>();
	public List<GameObject> activeMaps = new List<GameObject>();
	
	static public ItemManager instance;
	
    // Start is called before the first frame update
	private void Awake()
	{
		instance = this;
		mapSize = itemCount * itemSpace;
        maps.Add(MakeMap1());
        maps.Add(MakeMap1());
        maps.Add(MakeMap1());
        maps.Add(MakeMap1());
		foreach ( GameObject map in maps)
		{
			map.SetActive(false);
		}
		AddActMap();
		AddActMap();
	}
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (PathGen.instance.speed == 0) return;
		foreach ( GameObject map in activeMaps)
		{
			map.transform.position -= new Vector3(0, 0, PathGen.instance.speed * Time.deltaTime);
		}
		if(activeMaps[0].transform.position.z < -mapSize)
		{
			removeFirstActMap();
			AddActMap();
		}
    }
	
	void removeFirstActMap()
	{
		activeMaps[0].SetActive(false);
		maps.Add(activeMaps[0]);
		activeMaps.RemoveAt(0);
	}
	void AddActMap()
	{
		int r = Random.Range(0, maps.Count);
		GameObject go = maps[r];
		go.SetActive(true);
		foreach ( Transform child in go.transform)
		{
			child.gameObject.SetActive(true);
		}
		go.transform.position = activeMaps.Count > 0 ?
								activeMaps[activeMaps.Count -1].transform.position + Vector3.forward * mapSize :
								new Vector3(0,0,10);
		maps.RemoveAt(r);
		activeMaps.Add(go);
	}
	
	public void ResetMaps()
	{
		while ( activeMaps.Count > 0 )
		{
			removeFirstActMap();
		}
		AddActMap();
		AddActMap();
	}
	
	GameObject MakeMap1()
	{
		GameObject result = new GameObject("map1");
		result.transform.SetParent(transform);
		for(int i =0; i < itemCount; i++)
		{
			GameObject obstacle = null;
			
			int rand = Random.Range(-1,1);
			int itm = Random.Range(-1,2);
			if ( itm >= 0)
			{
				obstacle = Enemy;
			}
			Vector3 ObsPos = new Vector3(laneOffset * rand, 0, i * itemSpace);
			if (obstacle != null)
			{
				GameObject go = Instantiate(obstacle, ObsPos, Quaternion.identity);
				go.transform.SetParent(result.transform);
			}
			
		}
		return result;
	}
	
}
