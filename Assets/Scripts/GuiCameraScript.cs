using UnityEngine;
using System.Collections;

public class GuiCameraScript : MonoBehaviour {
	
	public GameObject buttonOneHolder;
	public GameObject buttonTwoHolder;

	private Rect buttonOne = new Rect(1000,0,80,20);
	private Rect buttonTwo = new Rect(1000,0,80,20);
	void OnGUI()
	{

		if(GUI.Button(buttonOne, "End Turn")) {
			//
		}
		/*if(GUI.Button(buttonTwo, "Level 2")) {
			//
		}*/
	}
	// Use this for initialization
	void Start () {
		buttonOne.x = buttonOneHolder.transform.position.x;
		buttonOne.y = buttonOneHolder.transform.position.z;
		//buttonOne.z = buttonOneHolder.transform.position.z;

		
		buttonTwo.x = buttonTwoHolder.transform.position.x;
		buttonTwo.y = buttonTwoHolder.transform.position.z;
		//buttonTwo.z = buttonTwoHolder.transform.position.z;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
