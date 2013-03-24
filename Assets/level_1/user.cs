using UnityEngine;
using System.Collections;

public class user : MonoBehaviour
{
	public float Cooldown = 1;
	public int shotRange = 100;
	
	// box or stone wich will be created
	public GameObject boxObject;	
	
	
	
	
	private float lastAttack = 0; 	
	// check lock every Update time
	private bool mouseLock = true;
	
	
	
	// Use this for initialization
	void Start ()
	{
		// sight instead of cursor
		Screen.lockCursor = true;
		Screen.showCursor = false;
	}
	
	
	// Update is called once per frame
	void Update ()
	{		
		if (lastAttack >= 0) {
			// check the action cooldown
			lastAttack -= Time.deltaTime;
			
		} else {		
			
			if (Input.GetKeyDown (KeyCode.Mouse0)) {				
				var rayHit = CheckTarget();
				if (rayHit != null)
				{
					// decrease volume
					Stone stone = rayHit.Value.transform.gameObject.GetComponent<Stone>();
					if (stone != null) stone.DecreaseVolume(rayHit.Value.point);
					// and reset cooldown
					lastAttack = Cooldown;
				}
				else
					Debug.Log ("Miss");
			}
			if (Input.GetKeyDown (KeyCode.Mouse1)) {
				var rayHit = CheckTarget();
				if(rayHit != null)
				{
					// the box already exist, increase volume 
					Stone stone = rayHit.Value.transform.gameObject.GetComponent<Stone>();
					if (stone != null) stone.IncreaseVolume(rayHit.Value.point);
				}
				else
					CreateNewStone();
				
				lastAttack = Cooldown;
			}			
		}
		
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Screen.showCursor = true;
			Screen.lockCursor = false;
			mouseLock = false;
		}
		
		if (Screen.lockCursor != mouseLock)
			Screen.lockCursor = mouseLock;
	}
	
	// make a hit, if cube exists return it
	// else return null
	RaycastHit? CheckTarget ()
	{
		Ray direction = Camera.main.ScreenPointToRay(Input.mousePosition);
		
		RaycastHit hit;
		
		Debug.Log(boxObject);
		
		if (Physics.Raycast(direction, out hit, shotRange) &&
			hit.transform.name == boxObject.name)
		{
			//Debug.Log (hit.transform.gameObject);
			return hit;
		}
		
		return null;
	}
	
		
	// create new instance of boxObject
	void CreateNewStone()
	{
		//Debug.Log ("Create new stone");
		var position = transform.position + transform.rotation * Vector3.forward;
		var stone = Instantiate(boxObject, position, transform.rotation);
		stone.name = boxObject.name;
		//Debug.Log (stone);
	}
	
}

