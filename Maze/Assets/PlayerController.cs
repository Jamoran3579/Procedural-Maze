using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class PlayerController : MonoBehaviour
{
    public bool gameover = false;

    public Camera cam;
    public NavMeshAgent agent;
    public ThirdPersonCharacter character;
    public GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        cam = Camera.main;
    
        agent.updateRotation = false; 
    }

    // Update is called once per frame
    void Update()
    {
        if (gameover)
		{

		}
		else
		{
            var current_pos = player.transform.position;
            var cam_pos = cam.transform.position;
            
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    agent.SetDestination(hit.point);
                }
            }

            if (agent.remainingDistance > agent.stoppingDistance)
            {
                character.Move(agent.desiredVelocity, false, false);
            }
            else
            {
                character.Move(Vector3.zero, false, false);
            }
            var speed = agent.speed;
		    speed = speed + (float)0.0001;
		    agent.speed = speed;

            if (current_pos.x < cam_pos.x - 14)
        	{
        	    Application.Quit();
				gameover = true;
        	}
        }
    }
}
