using UnityEngine;
using System.Collections;

namespace Common
{
	public class Stone : MonoBehaviour
	{
		public GameObject Cube;

		
		// Use this for initialization
		void Start ()
		{
			Cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			Cube.AddComponent<Rigidbody>();
			Debug.Log ("cube emerges");
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
		
	}
}
