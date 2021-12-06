using UnityEngine;
using UnityEngine.AI;

public class LevelGenerator : MonoBehaviour {

	public NavMeshSurface surface;

	public int width = 10;
	public int height = 10;
	public int total_ground = 0;
	public float difficulty = .99f;
	private bool playerSpawned = false;

	public GameObject wall;
	public GameObject player;
	public GameObject ground;
	public Camera cam;

	// Use this for initialization
	void Start () {
		GenerateLevel();
		player = GameObject.FindGameObjectWithTag("Player");
		cam = Camera.main;

		surface.BuildNavMesh();
		total_ground++;
	}

	void Update () {
		var current_pos = player.transform.position;
		if (current_pos.x >= ((total_ground-1)*22)-11)
		{
			Vector3 pos = new Vector3((total_ground*22f), 0f, 0f);
			Instantiate(ground, pos, Quaternion.identity, transform);
			GenerateLevel();
			surface.BuildNavMesh();
			total_ground++;
		}
		var cam_pos = cam.transform.position;
		cam_pos.x = cam_pos.x + (float)(0.01 + 0.001 * total_ground);
		cam.transform.position = cam_pos;

	}
	
	// Create a grid based level
	void GenerateLevel()
	{
		difficulty = difficulty - total_ground/100;
		// Loop over the grid
		for (int x = total_ground*22; x <= width + (total_ground*22); x+=2)
		{
			for (int y = 0; y <= height; y+=2)
			{
				// Should we place a wall?
				if (Random.value > .99f - total_ground/100)
				{
					// Spawn a wall
					Vector3 pos = new Vector3(x - width / 2f, 1.6f, y - height / 2f);
					Instantiate(wall, pos, Quaternion.identity, transform);
				} else if (!playerSpawned) // Should we spawn a player?
				{
					// Spawn the player
					Vector3 pos = new Vector3(x - width / 2f, 1.25f, y - height / 2f);
					Instantiate(player, pos, Quaternion.identity);
					playerSpawned = true;
				}
			}
		}
	}

}
