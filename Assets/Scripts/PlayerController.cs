using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    Rigidbody rb;
    Vector3 startGamePosition;
    Vector3 targetVelocity;
    Quaternion startGameRotation;
    Coroutine movingCoroutine;
    float laneOffset = 3f;
    float laneChangeSpeed = 15;
    float pointStart;
    float pointFinish;
	float jumpPower = 15;
	float jumpGravity = -40;
	float realGravity = -9.8f;
    bool isMoving = false;
	bool isJumping = false;
	bool isDuck = false;
	public int health = 3;
	
	static public PlayerController instance;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        startGamePosition = transform.position;
        startGameRotation = transform.rotation;
		animator.SetTrigger("Run");
		SwipeManager.instance.MoveEvent += MovePlayer;
		SwipeManager.instance.ClickEvent += ClickP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void Awake()
    {
        instance = this;
    }
	
	public void TakeDamage()
	{
		if ( health > 1 )
		{
			
			animator.SetTrigger("Damage");
			PathGen.instance.speed = 1;
			health--;
			HealthCounter.instance.Damage(health);
		}
		else
		{
			HealthCounter.instance.SetUp();
			health = 3;
			animator.SetTrigger("Died");
			transform.position = startGamePosition;
			transform.rotation = startGameRotation;
			PathGen.instance.ResetLvl();
		}
	}
	
	void MovePlayer(bool[] swipe)
	{
		
		if ( swipe[(int)SwipeManager.Direction.Left] && pointFinish > -laneOffset)
        {
            MoveHorizontal(-laneChangeSpeed);
        }
        if ( swipe[(int)SwipeManager.Direction.Right] && pointFinish < laneOffset)
        {
            MoveHorizontal(laneChangeSpeed);
        }
		if ( swipe[(int)SwipeManager.Direction.Up] && !isJumping)
        {
            jump();
        }
	}
	
	void ClickP( bool touch )
	{
		if ( touch && !isJumping )
		{
			jump();
		}
		if ( PathGen.instance.speed == 0 && touch )
		{
			StartGame();
		}
	}
	
    void MoveHorizontal(float speed)
    {
        animator.applyRootMotion = false;

        pointStart = pointFinish;
        pointFinish += Mathf.Sign(speed) * laneOffset;

        if (isMoving) { StopCoroutine(movingCoroutine); isMoving = false; };
        movingCoroutine = StartCoroutine(MoveCoroutine(speed));
    }
	
	void jump()
	{
		isJumping = true;
		rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
		Physics.gravity = new Vector3(0, jumpGravity, 0);
		StartCoroutine(StopJumping());
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
    IEnumerator MoveCoroutine(float vectorX)
    {
        isMoving = true;

        while (Mathf.Abs(pointStart - transform.position.x) < laneOffset)
        {
            yield return new WaitForFixedUpdate();

            rb.velocity = new Vector3(vectorX, rb.velocity.y, 0);
            float x = Mathf.Clamp(transform.position.x, Mathf.Min(pointStart, pointFinish), Mathf.Max(pointStart, pointFinish));
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        }

        rb.velocity = Vector3.zero;
        transform.position = new Vector3(pointFinish, transform.position.y, transform.position.z);

        isMoving = false;
    }

	IEnumerator StopJumping()
    {
        do
        {
            yield return new WaitForSeconds(0.02f);
        }
        while (rb.velocity.y != 0);

        //isJumping = false;
        Physics.gravity = new Vector3(0, realGravity, 0);
    }
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
    public void StartGame()
    {
        animator.SetTrigger("Run");
        PathGen.instance.StartLvl();
    }
	
	public void PauseGame()
    {
        animator.SetTrigger("Idle");
        PathGen.instance.PauseLvl();
    }
	
    public void ResetGame()
    {
		HealthCounter.instance.SetUp();
        animator.SetTrigger("Run");
        transform.position = startGamePosition;
        transform.rotation = startGameRotation;
        PathGen.instance.ResetLvl();
        PathGen.instance.StartLvl();
		health = 3;
    }
	
	void OnCollisionEnter(Collision coll)
	{
		if(coll.gameObject.tag == "Floor")
		{
			isJumping = false;
		}
	}
	
}