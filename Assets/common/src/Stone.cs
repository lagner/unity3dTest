using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Stone : MonoBehaviour
{
	//public Mesh mesh;
	public string meshName = "StoneMesh";
	public float size = 0.5f;
		
	public void DecreaseVolume (Vector3 point)
	{	
		Debug.Log("Desrease obj");
		Resize (point, false);
			
		if (topFace < 1 ||
			rightFace - leftFace < 1 ||
			frontFace - backFace < 1)
		{
			Debug.Log ("Distroy the object");
			Destroy (this.gameObject);
		}
	}
		
	public void IncreaseVolume (Vector3 point)
	{
		Debug.Log ("Increase obj");
		Resize (point);
	}
				
						
	// Use this for initialization
	void Start ()
	{				
		mf = this.GetComponent<MeshFilter>();
		mc = this.GetComponent<MeshCollider>();
		if (mf == null || mc == null)
		{
			Debug.LogError("Can't get a component");
			return;
		}
		
		
		var mesh = new Mesh ();			
		mesh.name = "Stone Mesh";
		mesh.vertices = cubeVertexs;
		mesh.triangles = cubeFaces;			
		mesh.uv = cubeUV;
			
		renderer.material.color = Color.white;
			
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		mesh.Optimize();
		
		mf.mesh = mesh;
		mc.sharedMesh = null;
		mc.sharedMesh = mesh;
	}
		
	void Resize (Vector3 point, bool increase = true)
	{
		int i = increase ? 1 : -1;
			
		Mesh newMesh;
		
		var direction = GetDirectional(point);
		Debug.Log(direction);
		
		switch (direction) {
		case Faces.Left:
			newMesh = MoveWall (leftSide, -Vector3.right * i);
			leftFace += (-i);
			Debug.Log(leftFace);
			break;
		case Faces.Right:
			newMesh = MoveWall (rightSide, Vector3.right * i);
			rightFace += i;
			Debug.Log (rightFace);
			break;
		case Faces.Front:
			newMesh = MoveWall (frontSide, Vector3.forward * i);
			frontFace += i;
			Debug.Log (frontFace);
			break;
		case Faces.Back:
			newMesh = MoveWall (backSide, -Vector3.forward * i);
			backFace += (-i);
			Debug.Log (backFace);
			break;
		case Faces.Top:
			newMesh = MoveWall (topSide, Vector3.up);
			topFace += i;
			break;
		default:
			Debug.LogWarning ("Something wrong");
			return;
		}
			
		newMesh.RecalculateBounds();
		newMesh.RecalculateNormals();
		newMesh.Optimize();		
		
		mf.mesh = newMesh;		
		// change box collider size
		mc.sharedMesh = null;
		mc.sharedMesh = newMesh;
	}
		
				
	// return direction to grow
	Faces GetDirectional (Vector3 point)
	{
		var p = this.transform.InverseTransformPoint (point);
		var delta = 0.001;
		
		Debug.Log("Get point: ");
		Debug.Log(p);
		
		if (Mathf.Abs(p.x - leftFace) < delta) 
			return Faces.Left;
		if (Mathf.Abs(p.x - rightFace) < delta) 
			return Faces.Right;
		if (Mathf.Abs(p.z - frontFace) < delta) 
			return Faces.Front;
		if (Mathf.Abs(p.z - backFace) < delta) 
			return Faces.Back;
		
		return Faces.Top;
	}
		
	Mesh MoveWall (int[] verts, Vector3 d)
	{
		var vertices = mf.mesh.vertices;
			
		foreach (int i in verts)
			vertices [i] += d;
			
		Vector2[] uvs = new Vector2[8];
		for (int i=0; i<uvs.Length; i++) {
			uvs [i] = new Vector2 (vertices [i].x, vertices [i].z);
		}
			
		return new Mesh () 
			{
				vertices = vertices,
				triangles = cubeFaces,
				uv = uvs
			};
	}
		
	private MeshFilter mf;
	private MeshCollider mc;
	
		
	
	int frontFace = 1;
	int backFace = 0;
	int leftFace = 0;
	int rightFace = 1;
	int topFace = 1;
		
	// the indexes of vertices 		
	int[] leftSide = { 0, 3, 4, 5};
	int[] rightSide = { 1, 2, 6, 7};
	int[] frontSide = { 2, 3, 4, 7};
	int[] backSide = { 0, 1, 5, 6};
	int[] topSide = { 4, 5, 6, 7};
		
	#region 1x1 cube params 
	private Vector3[] cubeVertexs = new Vector3[]
		{
			new Vector3 (0, 0, 0),
			new Vector3 (1, 0, 0),
			new Vector3 (1, 0, 1),
			new Vector3 (0, 0, 1),
			new Vector3 (0, 1, 1),
			new Vector3 (0, 1, 0),
			new Vector3 (1, 1, 0),
			new Vector3 (1, 1, 1)
		};
	private int[] cubeFaces = new int[] 
		{
			0, 1, 2, // bottom
			0, 2, 3,
			5, 4, 7, // top
			5, 7, 6,
			0, 6, 1, // back
			0, 5, 6,
			0, 3, 4, // left
			0, 4, 5,
			1, 7, 2, // right
			1, 6, 7,
			4, 2, 7, // front
			4, 3, 2			
		};
	private Vector2[] cubeUV = new Vector2[] 
		{
			new Vector2 (0, 0),
			new Vector2 (1, 0),
			new Vector2 (1, 1),
			new Vector2 (0, 1),
			new Vector2 (1, 0),
			new Vector2 (0, 0),
			new Vector2 (1, 0),
			new Vector2 (1, 1)
		};
	#endregion
			
	enum Faces
	{
		Left,
		Right,
		Front,
		Back,
		Top,
		Bottom
	};
}

