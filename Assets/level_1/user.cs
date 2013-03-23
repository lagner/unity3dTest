using UnityEngine;
using System.Collections;
using Common;

public class user : MonoBehaviour
{
	
	public float Range = 100;
	public float Cooldown = 1;
	public int shotRange = 100;
	
	
	private float lastAttack = 0; 
	private string enemyObjName = "Stone";
	
	// Use this for initialization
	void Start ()
	{
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
				// if shot was successfull, reset the cooldown
				if (ResizeTargetStone())
					lastAttack = Cooldown;
			}
			if (Input.GetKeyDown (KeyCode.Mouse1)) {
				// right click -> create new Stone
				CreateNewStone();
				lastAttack = Cooldown;
			}			
		}	
	}
	
	RaycastHit? CheckTarget ()
	{
		// make a hit, if cube exists resize it
		// else return null
		Ray direction = Camera.main.ScreenPointToRay(Input.mousePosition);
		
		RaycastHit hit;
		
		if (Physics.Raycast(direction, out hit, shotRange) &&
			hit.transform.name == enemyObjName)
		{
			Debug.DrawLine(Vector3.zero, hit.point, Color.green);
			return hit;
		}
		else 
		{
			Debug.Log ("miss");
			return null;
		}
	}
	
	bool ResizeTargetStone()
	{
		// check if the target exist
		var target = CheckTarget();		
		if (target == null) return false;
		
		
		Debug.Log ("Resize target");
		return true;
	}
	
	void CreateNewStone()
	{
		var obj = GameObject.CreatePrimitive (PrimitiveType.Cube);
		obj.name = enemyObjName;
		obj.AddComponent<Rigidbody> ();
		obj.transform.position = this.transform.position;				
		obj.transform.rotation = this.transform.rotation;
	}
	
}

