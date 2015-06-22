using UnityEngine;
using System.Collections;

public class PlayerPanelScript : MonoBehaviour {

	public int number = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.Euler(90,0,0);
		//renderer.material.SetTextureOffset("_MainTex", new Vector2(offset, -0.01 +( number * 0.1f) ));
	}
	void changeNumber(int newNumber)
	{
		number = newNumber;
		renderer.material.SetTextureOffset("_MainTex", new Vector2(-0.01f +( number * 0.1f) ,0f ));
	}
}
