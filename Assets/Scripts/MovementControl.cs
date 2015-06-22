using UnityEngine;
using System.Collections;

[AddComponentMenu("Character/Movement Control")]
[RequireComponent(typeof(CharacterController))]
public class MovementControl : MonoBehaviour {

	private bool rotateNeg = false;
	private bool rotatePos = false;
	private bool lockControls = false;
	private bool moveForward = false;
	private bool moveBackward = false;
	private bool moveLeft = false;
	private bool moveRight = false;
	private bool moving = false;
	private bool falling = false;
	public bool climbTree = false;
	private int treeStep = 0;
	private bool zAxisClimb = true;
	private bool negAxisClimb = false;
	public int speed = 3;
	public int maxSteps = 2;
	public int currentSteps = 0;
	//private bool movingBackward = false;
	private Vector3 startingPos = Vector3.zero;
	public Vector3 targetPos = Vector3.zero;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!lockControls)
		{
		///Rotation///
		if (Input.GetKeyDown(KeyCode.Q))
			rotateNeg = true;
		if (rotateNeg && Input.GetKeyUp(KeyCode.Q))
		{
			rotateNeg = false;
			transform.Rotate(0, -90, 0);
		}
		if (Input.GetKeyDown(KeyCode.E))
			rotatePos = true;
		if (rotatePos && Input.GetKeyUp(KeyCode.E))
		{
			rotatePos = false;
			transform.Rotate(0, 90, 0);
		}
		///End Rotation///
		/// Movement///
		if (Input.GetKeyDown(KeyCode.W))
			moveForward = true;
		if (moveForward && Input.GetKeyUp(KeyCode.W))
		{
			moveForward = false;
			moving = true;
			lockControls = true;
			bool fixLock = false;
			RaycastHit hit;
			//transform.
			int currentRotation = (int)transform.rotation.eulerAngles.y;
			switch(currentRotation)
			{	
			case 0:
				{
					targetPos = transform.position;
					targetPos.z += 4;
					Vector3 fwd = transform.TransformDirection(Vector3.forward);
					//if (Physics.Raycast(transform.position, fwd, 4))
					if (Physics.Raycast(transform.position, fwd, out hit, 4.0F))
					{
						hit.transform.SendMessage ("isTree",this);
						if (climbTree)
						{
							targetPos.z -= 2;
							zAxisClimb = true;
							moving = false;
						}
						else
						{
							fixLock = true;
						}
					}
				}
				break;
			case 90:
				{
					targetPos = transform.position;
					targetPos.x += 4;
					Vector3 fwd = transform.TransformDirection(Vector3.forward);
					if (Physics.Raycast(transform.position, fwd, out hit, 4.0F))
					{
						hit.transform.SendMessage ("isTree",this);
						if (climbTree)
						{
							targetPos.x -= 2;
							zAxisClimb = false;
							moving = false;
						}
						else
						{
							fixLock = true;
						}
					}
				}
				break;
			case 180:
				{
					targetPos = transform.position;
					targetPos.z -= 4;
					Vector3 fwd = transform.TransformDirection(Vector3.forward);
					if (Physics.Raycast(transform.position, fwd, out hit, 4.0F))
					{
						hit.transform.SendMessage ("isTree",this);
						if (climbTree)
						{
							targetPos.z += 2;
							zAxisClimb = true;
							moving = false;
							negAxisClimb = true;
						}
						else
						{
							fixLock = true;
						}
					}
				break;
				}
			case 270:
				{
					targetPos = transform.position;
					targetPos.x -= 4;
					Vector3 fwd = transform.TransformDirection(Vector3.forward);
					if (Physics.Raycast(transform.position, fwd, out hit, 4.0F))
					{
						hit.transform.SendMessage ("isTree",this);
						if (climbTree)
						{
							targetPos.x += 2;
							zAxisClimb = false;
							moving = false;
							negAxisClimb = true;
						}
						else
						{
							fixLock = true;
						}
					}
				break;
				}
			}
			if (fixLock)
			{
				
				lockControls = false;
				moving = false;
				targetPos = transform.position;
			}
		}
		if (Input.GetKeyDown(KeyCode.S))
			moveBackward = true;
		if (moveBackward && Input.GetKeyUp(KeyCode.S))
		{
			moveBackward = false;
			moving = true;
			lockControls = true;
			bool fixLock = false;
			RaycastHit hit;
			//transform.
			int currentRotation = (int)transform.rotation.eulerAngles.y;
			switch(currentRotation)
			{	
			case 0:
			{
				targetPos = transform.position;
				targetPos.z -= 4;
				Vector3 fwd = transform.TransformDirection(Vector3.back);
				//if (Physics.Raycast(transform.position, fwd, 4))
				if (Physics.Raycast(transform.position, fwd, out hit, 4.0F))
				{
					hit.transform.SendMessage ("isTree",this);
					if (climbTree)
					{
						targetPos.z += 2;
						zAxisClimb = true;
						negAxisClimb = true;
						moving = false;
					}
					else
					{
						fixLock = true;
					}
				}
			}
				break;
			case 90:
			{
				targetPos = transform.position;
				targetPos.x -= 4;
					Vector3 fwd = transform.TransformDirection(Vector3.back);
				if (Physics.Raycast(transform.position, fwd, out hit, 4.0F))
				{
					hit.transform.SendMessage ("isTree",this);
					if (climbTree)
					{
						targetPos.x += 2;
						zAxisClimb = false;
						moving = false;
						negAxisClimb = true;
					}
					else
					{
						fixLock = true;
					}
				}
			}
				break;
			case 180:
			{
				targetPos = transform.position;
				targetPos.z += 4;
					Vector3 fwd = transform.TransformDirection(Vector3.back);
				if (Physics.Raycast(transform.position, fwd, out hit, 4.0F))
				{
					hit.transform.SendMessage ("isTree",this);
					if (climbTree)
					{
						targetPos.z -= 2;
						zAxisClimb = true;
						moving = false;
						//negAxisClimb = true;
					}
					else
					{
						fixLock = true;
					}
				}
				break;
			}
			case 270:
			{
				targetPos = transform.position;
				targetPos.x += 4;
					Vector3 fwd = transform.TransformDirection(Vector3.back);
				if (Physics.Raycast(transform.position, fwd, out hit, 4.0F))
				{
					hit.transform.SendMessage ("isTree",this);
					if (climbTree)
					{
						targetPos.x -= 2;
						zAxisClimb = false;
						moving = false;
						//negAxisClimb = true;
					}
					else
					{
						fixLock = true;
					}
				}
				break;
			}
			}
			if (fixLock)
			{
				
				lockControls = false;
				moving = false;
				targetPos = transform.position;
			}
		}
		}
		else if (moving)
		{
			float tempSpeed = speed;
			if (falling)
				tempSpeed *= 5;
			transform.position = Vector3.MoveTowards(transform.position,targetPos,(tempSpeed * Time.deltaTime));
			if (transform.position == targetPos)
			{
				Vector3 fwd = transform.TransformDirection(Vector3.down);
				if (Physics.Raycast(transform.position, fwd, 4))
				//if (Physics.Raycast(transform.position, fwd, out hit, 4.0F))
				{
					lockControls = false;
					falling = false;
					currentSteps++;
				}
				else
				{
					falling = true;
					targetPos.y -= 4;
				}
			}
		}

		else if (climbTree)
		{
			transform.position = Vector3.MoveTowards(transform.position,targetPos,(speed * Time.deltaTime));
			if (transform.position == targetPos && treeStep == 0)
			{
				treeStep += 1;
				targetPos.y += 4;
			}
			else if (transform.position == targetPos && treeStep == 1)
			{
				treeStep += 1;
				if (negAxisClimb)
				{
					if (zAxisClimb)
						targetPos.z -= 2;
					else
						targetPos.x -= 2;
				}
				else
				{
					if (zAxisClimb)
						targetPos.z += 2;
					else
						targetPos.x += 2;
				}
			}
			else if (transform.position == targetPos && treeStep == 2)
			{
				lockControls = false;
				currentSteps++;
				treeStep = 0;
				climbTree = false;
				zAxisClimb = false;
				negAxisClimb = false;
			}
		}
		/// End Movement///
	}
}
