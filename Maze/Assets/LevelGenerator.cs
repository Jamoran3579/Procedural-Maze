/*
Imports for what libraries are going to be used within the code
*/
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

//Creation of the Level generator class
public class LevelGenerator : MonoBehaviour {

	//Creating a new NavMeshSurface
	public NavMeshSurface surface;

	//All of the public variables, width and height control the size of the walls, total ground is the total number of ground
	//Prefabs instansiated so far, difficulty controls the difficulty of the game between 0.99 and 0.75. The lower the number, 
	//the more difficult the game, player spawned determines if the player has been spawned yet, gameover tracks whether the 
	//player has lost the game yet or not
	public int width = 10;
	public int height = 10;
	public int total_ground = 0;
	public float difficulty = .95f;
	private bool playerSpawned = false;
	public bool gameover = false;

	//Creating all of the public objects. Wall refers to the wall prefab, player refers to the player prefab and ground refers to
	//the ground prefab. Camera refers to the main camera and this will be attached in the start method. Score and result are
	//Text boxes attached to the canvas the player can see
	public GameObject wall;
	public GameObject player;
	public GameObject ground;
	public Camera cam;
	public Text score;
	public Text result;

	// Use this for initialization
	void Start () {
		//This function starts the generation of walls
		GenerateLevel();

		//This attaches the player object which has been spawned to te player GameObject and the main camera GameObject
		//to the Camera object
		player = GameObject.FindGameObjectWithTag("Player");
		cam = Camera.main;

		//This builds the NavMesh that the agent is able to walk on, and then adds one to the total ground spawned as one 
		//already exists at the beginning of the game
		surface.BuildNavMesh();
		total_ground++;
	}

	void Update () {
		//This checks to see if the game has ended, if so, it goes into an area of the code that does nothing to stop the movement
		//of the camera and to stop any other processes occuring
		if (gameover)
		{

		}
		//If the game hasn't ended, it will enter here to allow the application to run normally
		else
		{
			//This grabs the position of the player GameObject
			var current_pos = player.transform.position;

			//This is a check to see if the current position of the player requires the spawning of a new platform.
			if (current_pos.x >= ((total_ground-1)*22)-11)
			{
				//If so, we get the position to spawn the new ground based on the total number of ground spawned so far. We then instantiate this new
				//piece of ground, and then build the walls on this piece of ground. Then we build a new NavMesh that the agent can walk on, and increase
				//the gound generated count by one
				Vector3 pos = new Vector3((total_ground*22f), 0f, 0f);
				Instantiate(ground, pos, Quaternion.identity, transform);
				GenerateLevel();
				surface.BuildNavMesh();
				total_ground++;

				//Next we check the difficulty and if greater than 0.75, we lower it (Lower number means higher difficulty)
				if (difficulty > 0.75f)
				{
					difficulty = difficulty - 0.01f;
				}
			}
			//We want the camera to move, so next we get the position of the camera. The game runs along the x axis so we update the cameras
			//x position based on how much ground has been generated, aka. the difficulty. The more ground, the more we move the camera
			var cam_pos = cam.transform.position;
			cam_pos.x = cam_pos.x + (float)(0.02 + 0.001 * total_ground);
			cam.transform.position = cam_pos;

			//We then update the players score to allign with the total ground generated - 2, as the game starts with 2 ground already existing
			score.text = "Score: " + (total_ground - 2).ToString();

			//Here we check to see how far the player is behind the camera, once the player is more than 14 on the x axis behind, they can no longer
			//be seen. This is considered game over
			if (current_pos.x < cam_pos.x - 14)
        	{
				//We update the result text to say game over, the applicartion is then quit, and the gameover variable is set to true, stopping all
				//processes within the game
				result.text = "Game Over\nYou scored: " + (total_ground - 2).ToString();
        	    Application.Quit();
				gameover = true;
        	}
		}
	}
	
	//This is the function where I build the level aka walls
	void GenerateLevel()
	{
		// Loop over the grid that needs walls to be generated, this is done using the total ground variable to find how far away from the origin we are
		for (int x = total_ground*22; x <= width + (total_ground*22); x+=2)
		{
			//This is part of looping over the grid, but as height doesn't change, this does not depend on the number of grounds instantiated so far
			for (int y = 0; y <= height; y+=2)
			{
				// This checks to see if a wall should be placed based on the difficulty. As the random value has a higher chance of being above the 
				//difficulty if it is smaller, the smaller the difficulty number, the harder the level
				if (Random.value > difficulty)
				{
					// Instanciate a wall at the current width and height
					Vector3 pos = new Vector3(x - width / 2f, 1.6f, y - height / 2f);
					Instantiate(wall, pos, Quaternion.identity, transform);
				} else if (!playerSpawned) // Check to see if a player is spawned, if not, spawn one
				{
					// Instantiate player and set bool check to true
					Vector3 pos = new Vector3(x - width / 2f, 1.25f, y - height / 2f);
					Instantiate(player, pos, Quaternion.identity);
					playerSpawned = true;
				}
			}
		}
	}

}
