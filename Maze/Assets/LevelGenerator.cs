/*
Imports for what libraries are going to be used within the code
*/
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class LevelGenerator : MonoBehaviour {

	public NavMeshSurface surface;

	public int width = 10;
	public int height = 10;
	public int total_ground = 0;
	public float difficulty = .95f;
	private bool playerSpawned = false;
	public bool gameover = false;

	public GameObject wall;
	public GameObject player;
	public GameObject ground;
	public Camera cam;
	public Text score;
	public Text result;

	// Use this for initialization
	void Start () {
		GenerateLevel();
		player = GameObject.FindGameObjectWithTag("Player");
		cam = Camera.main;

		surface.BuildNavMesh();
		total_ground++;
	}

	void Update () {
		if (gameover)
		{

		}
		else
		{
			var current_pos = player.transform.position;
			if (current_pos.x >= ((total_ground-1)*22)-11)
			{
				Vector3 pos = new Vector3((total_ground*22f), 0f, 0f);
				Instantiate(ground, pos, Quaternion.identity, transform);
				GenerateLevel();
				surface.BuildNavMesh();
				total_ground++;
				if (difficulty > 0.75f)
				{
					difficulty = difficulty - 0.01f;
				}
			}
			var cam_pos = cam.transform.position;
			cam_pos.x = cam_pos.x + (float)(0.02 + 0.001 * total_ground);
			cam.transform.position = cam_pos;
			score.text = "Score: " + (total_ground - 2).ToString();

			if (current_pos.x < cam_pos.x - 14)
        	{
				result.text = "Game Over\nYou scored: " + (total_ground - 2).ToString();
        	    Application.Quit();
				gameover = true;
        	}
		}
	}
	
	// Create a grid based level
	void GenerateLevel()
	{
		// Loop over the grid
		for (int x = total_ground*22; x <= width + (total_ground*22); x+=2)
		{
			for (int y = 0; y <= height; y+=2)
			{
				// Should we place a wall?
				if (Random.value > difficulty)
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
