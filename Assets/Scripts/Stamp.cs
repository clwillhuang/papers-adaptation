using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamp : MonoBehaviour {

	public Animation anim; 
	bool extended; 

	public void Start()
	{
		extended = false; 
	}

	// Update is called once per frame
	public void Click()
	{
		Debug.Log ("Stamp clicked!"); 
		extended = !extended;
		if (extended) {
			Show (); 
		} else {
			Hide (); 
		}
	}

	public void Show()
	{
		anim.Play ("StampShow"); 
	}

	public void Hide ()
	{
		anim.Play ("StampHide"); 
	}
}
