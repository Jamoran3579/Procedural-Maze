/*
Imports for what libraries are going to be used within the code
*/
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

//Creation of the Playe controller class
public class PlayerController : MonoBehaviour
{
    //Check to see if the player has lost the gae or not
    public bool gameover = false;

    //Addition of all public objects, cam refers to the main camera, agent is the NavMeshAgent attached to the player
    //character is the character on the player object, player refers to the player prefab which is instantiated
    public Camera cam;
    public NavMeshAgent agent;
    public ThirdPersonCharacter character;
    public GameObject player;

    //This is executed at the start of the game
    void Start()
    {
        //Attach the player GameObject to the player object within the code, attach the NavMeshAgent to the agent variable
        //within the code, and attach the cam object to the main camera
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        cam = Camera.main;

        //This gets the agent to not update it's rotation, so that the animations can work properly
        agent.updateRotation = false; 
    }

    // Update is called once per frame
    void Update()
    {
        //This is a check to see if the game is ended, if so, it enters here to prevent any other game processes happening
        if (gameover)
		{
            character.Move(Vector3.zero, false, false);
		}
        //If the game hasn't ended, go in here and allow the game to run as normal
		else
		{
            //This finds the current player position and camera position
            var current_pos = player.transform.position;
            var cam_pos = cam.transform.position;
            
            //This is a check to see if the mouse has been pressed
            if (Input.GetMouseButtonDown(0))
            {
                //If so, it creates a ray to that point on the map
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                //If we then cast a ray to that point, and it hits something, set that point as a new destination for our agent
                if (Physics.Raycast(ray, out hit))
                {
                    agent.SetDestination(hit.point);
                }
            }

            //If the distance between the agent and the destination is greater than the stopping distance, move the agent at the desired velocity
            //towards the destination
            if (agent.remainingDistance > agent.stoppingDistance)
            {
                character.Move(agent.desiredVelocity, false, false);
            }
            //If the agent is closer than the stopping distance, stop the agent
            else
            {
                character.Move(Vector3.zero, false, false);
            }
            //Here we increase the speed of the agent over time so as to keep up with the increasing camera speed
            var speed = agent.speed;
		    speed = speed + (float)0.0001;
		    agent.speed = speed;

            //This is a position check similar to LevelGenerator.cs, if the player is so far behind the camera that they cn no longer be seen
            //the game is over
            if (current_pos.x < cam_pos.x - 14)
        	{
        	    Application.Quit();
				gameover = true;
        	}
        }
    }
}
