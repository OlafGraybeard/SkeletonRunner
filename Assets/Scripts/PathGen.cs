using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGen : MonoBehaviour
{
    public static PathGen instance;
	private void Awake() { instance = this; }
	
	public int partCount = 7;
	public List<GameObject> PrefabList = new List<GameObject>();
	public float speed = 0;
	public float maxSpeed = 1;
	
	private List<GameObject> parts = new List<GameObject>();
	private int selector;
	
    // Start is called before the first frame update
    void Start()
    {
		ResetLvl();
        StartLvl();
    }

    // Update is called once per frame
    void Update()
    {
        if (speed < 1) { return; }
		if (speed < maxSpeed) { speed += 3 * Time.deltaTime; }
		
		foreach ( GameObject part in parts )
		{
			part.transform.position -= new Vector3(0, 0, speed * Time.deltaTime);
		}
		
		if (parts[0].transform.position.z < -15)
        {
            Destroy(parts[0]);
            parts.RemoveAt(0);
            CreatePart();
        }
		
    }
	
	public void StartLvl()
	{
		speed = 1;
		SwipeManager.instance.enabled = true;
	}
	public void PauseLvl()
	{
		speed = 0;
		SwipeManager.instance.enabled = false;
	}
	
	public void ResetLvl()
	{
		speed = 0;
		while (parts.Count > 0)
        {
            Destroy(parts[0]);
            parts.RemoveAt(0);
        }
		for( int i = 0; i < partCount; i++ )
		{
			CreatePart();
		}
		SwipeManager.instance.enabled = false;
		ItemManager.instance.ResetMaps();
	}
	
	private void CreatePart()
    {
		
        Vector3 pos = Vector3.zero;

        if (parts.Count > 0)
        {
            pos = parts[parts.Count - 1].transform.position + new Vector3(0, 0, 10);
        }
		selector = (selector + 1) % PrefabList.Count;
        GameObject go = Instantiate(PrefabList[selector], pos, Quaternion.identity);
        go.transform.SetParent(transform);
        parts.Add(go);
    }
	
}
