using UnityEngine;
using UnityEngine.UI; 

public class Telescreen : MonoBehaviour {

	public bool turnedOn; 
	public Sprite On, Off; 

	public static Telescreen _current;
	public static Telescreen current { 
		get {
			if (_current == null) {
				_current = FindObjectOfType<Telescreen> (); 
			}
			return _current;
		}		
	}

	void Start()
	{
		GetComponent<Image> ().sprite = On; 
	}

	public void Toggle ()
	{
		turnedOn = !turnedOn; 
		if (!turnedOn) {
			GetComponent<Image> ().sprite = Off; 
		} else {
			GetComponent<Image> ().sprite = On; 
		}
	}

	public void Turn(bool state)
	{
		turnedOn = state; 
		if (!turnedOn) {
			GetComponent<Image> ().sprite = Off; 
		} else {
			GetComponent<Image> ().sprite = On; 
		} 
	} 
}
