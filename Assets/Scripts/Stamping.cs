using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamping : MonoBehaviour {

	public Animation anim;
	public GameObject stampInstance;
	float stampDelay = 0.2f; 
	public float threshold;

	public bool isBurn; 
	bool success = false; 
	bool onOriginal = true; 
	Vector2 v1, s1; 

	public void Stamp ()
	{
		success = false; 

		if (StateManager.current.document == null && StateManager.current.printedDocument == null) {
			return; 
		}
		if (StateManager.current.document == null && StateManager.current.printedDocument != null) {
			checkPrinted (); 
		}
		if (StateManager.current.printedDocument == null && StateManager.current.document != null) {
			checkOriginal (); 
		}
		else if (StateManager.current.document.transform.GetSiblingIndex () > StateManager.current.printedDocument.transform.GetSiblingIndex ()) {
			checkOriginal ();
			if (!success) {
				checkPrinted (); 
			}
		} else {
			checkPrinted ();
			if (!success) {
				checkOriginal (); 
			}
		}

		if (success) {
			anim.Play (); 
			StartCoroutine (stampTiming (onOriginal)); 
		} else {
			StateManager.current.StopCoroutine ("SaySomething"); 
			StartCoroutine (StateManager.current.SaySomething ("Align correctly to apply stamps.", 2.0f, Color.red)); 
		}
		return;
	}

	public void checkOriginal ()
	{
		onOriginal = true; 
		v1 = new Vector2 (StateManager.current.document.GetComponent<RectTransform> ().localPosition.x, StateManager.current.document.GetComponent<RectTransform> ().localPosition.y); 
		s1 = new Vector2 (StateManager.current.document.GetComponent<RectTransform> ().rect.width, StateManager.current.document.GetComponent<RectTransform> ().rect.height);
		if (!isBurn) {
			if (v1.x - s1.x / 2f <= 0f + threshold && v1.x + s1.x / 2f >= 124.6f - threshold && v1.y + s1.y / 2f >= 50f - threshold && v1.y - s1.y / 2f <= 0f + threshold) {
				Debug.Log ("Success"); 
				success = true;
			} else {
				Debug.Log ("No contact."); 
			}
		} else {
			if (v1.x - s1.x / 2f <= 180f + threshold && v1.x + s1.x / 2f >= 300f - threshold && v1.y + s1.y / 2f >= 50 - threshold && v1.y - s1.y / 2f <= 0f - threshold) {
				Debug.Log ("Success"); 
				success = true;
			} else {
				Debug.Log ("No contact."); 
			}
		}
	}

	public void checkPrinted()
	{
		onOriginal = false; 
		if (StateManager.current.printedDocument != null) {
			v1 = new Vector2 (StateManager.current.printedDocument.GetComponent<RectTransform> ().localPosition.x, StateManager.current.printedDocument.GetComponent<RectTransform> ().localPosition.y); 
			s1 = new Vector2 (StateManager.current.printedDocument.GetComponent<RectTransform> ().rect.width, StateManager.current.printedDocument.GetComponent<RectTransform> ().rect.height);
			if (!isBurn) {
				if (v1.x - s1.x / 2f <= 0f + threshold && v1.x + s1.x / 2f >= 124.6f - threshold && v1.y + s1.y / 2f >= 50f - threshold && v1.y - s1.y / 2f <= 0f + threshold) {
					Debug.Log ("Success"); 
					success = true;
				} else {
					Debug.Log ("No contact."); 
				}
			} else {
				if (v1.x - s1.x / 2f <= 180f + threshold && v1.x + s1.x / 2f >= 300f - threshold && v1.y + s1.y / 2f >= 50 - threshold && v1.y - s1.y / 2f <= 0f + threshold) {
					Debug.Log ("Success"); 
					success = true;
				} else {
					Debug.Log ("No contact."); 
				}
			}
		}
	}

	IEnumerator stampTiming(bool onOriginal)
	{
		yield return new WaitForSeconds (stampDelay); 
		if (onOriginal) {
				GameObject newStamp = Instantiate (stampInstance, StateManager.current.document.transform, true);
				if (isBurn) {
					StateManager.current.document.GetComponent<DragTransform> ().burntStamps++; 
				} else {
					StateManager.current.document.GetComponent<DragTransform> ().storageStamps++; 
				}
				newStamp.GetComponent<Image> ().enabled = true; 
			}
			else {
				GameObject newStamp = Instantiate (stampInstance, StateManager.current.printedDocument.transform, true); 
				if (isBurn) {
					StateManager.current.printedDocument.GetComponent<DragTransform> ().burntStamps++; 
				} else {
					StateManager.current.printedDocument.GetComponent<DragTransform> ().storageStamps++; 
				}
				newStamp.GetComponent<Image> ().enabled = true; 
			}
	}


}
