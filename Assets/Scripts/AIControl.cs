using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Character/AI Control")]
[RequireComponent(typeof(CharacterController))]
public class AIControl : MonoBehaviour {

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
	public List<Vector3> walkOrder = new List<Vector3>();
	public List<int> checkWalk = new List<int>();
	public int speed = 3;
	public int step = 0;
	public int maxSteps = 5;
	public int currentSteps = 0;
	//private bool movingBackward = false;
	private Vector3 startingPos = Vector3.zero;
	public Vector3 targetPos = Vector3.zero;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (walkOrder.Count > 0)
		if (walkOrder[step] != transform.position)
		{
			//targetPos = transform.position - walkOrder[step];
			
			RaycastHit hit;
			Vector3 fwd = transform.transform.position - walkOrder[step];
			targetPos = walkOrder[step];	
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
			}
			else 
			{
				moving = true;
			}
		}
		if (moving)
		{
			float tempSpeed = speed;
			if (falling)
			{
				tempSpeed *= 5;
				//Vector3 newTargetPos = targetPos;
				//newTargetPos.x = transform.position.x;
				//newTargetPos.z = transform.position.z;
				//transform.position = Vector3.MoveTowards(transform.position,newTargetPos,(tempSpeed * Time.deltaTime));
			}
			//else
			//Vector3 fwd = transform.position + transform.transform.position - walkOrder[step];;
			/*Vector3 fwd = (transform.position - walkOrder[step])/4;
			if (Physics.Raycast(transform.position,fwd,4))
			{
				lockControls = false;
				falling = false;
				currentSteps++;
				step++;
			}
			else*/
			if (checkWalk[step] == 0)
			{
				checkWalk[step] = 1;
				Vector3 fwd = (transform.position - walkOrder[step])/4;
				if(fwd.x == -1)
					fwd.x *= -1;
				if (Physics.Raycast(transform.position,fwd,4))
				{
					checkWalk[step] = 2;
					
				}
			}
			if (checkWalk[step] != 2)
				transform.position = Vector3.MoveTowards(transform.position,targetPos,(tempSpeed * Time.deltaTime));
			if (transform.position == targetPos || checkWalk[step] == 2)
			{
				Vector3 dwn = transform.TransformDirection(Vector3.down);
				if (Physics.Raycast(transform.position, dwn, 4))
				//if (Physics.Raycast(transform.position, fwd, out hit, 4.0F))
				{
					if (checkWalk[step] == 2 && step != maxSteps - 1)
						walkOrder[step + 1] = walkOrder[step];
					lockControls = false;
					falling = false;
					currentSteps++;
					step++;
				}
				else
				{
					falling = true;
					walkOrder[step] = new Vector3(walkOrder[step].x,walkOrder[step].y - 4,walkOrder[step].z);
					int i = step;
					while(walkOrder.Count >= i)
					{
						Vector3 temp = walkOrder[i];
						temp.y = walkOrder[step].y;
						walkOrder[i] = temp;
						i++;
					}

					//walkOrder[step].y -= 4.0f;
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
