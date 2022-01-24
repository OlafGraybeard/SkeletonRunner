using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCounter : MonoBehaviour
{
	public GameObject uiPrefab;
	private List<GameObject> health = new List<GameObject>();
	
	static public HealthCounter instance;
	
    // Start is called before the first frame update
    void Start()
    {
		SetUp();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void Awake()
    {
        instance = this;
    }
	
	public void SetUp()
	{
		if ( health.Count == 0)
		{
			for ( int i = 0; i < 3 ; i++ )
			{
				Vector2 pos = new Vector2(30 + ( i * 65 ),30);
				GameObject go = Instantiate (uiPrefab, pos, transform.rotation);
				go.transform.SetParent(transform);
				health.Add(go);
			}
		}
		else
		{
			foreach (GameObject go in health)
			{
				go.SetActive(true);
			}
		}
	}
	public void Damage(int hp)
	{
		health[hp].SetActive(false);
	}
	
}
