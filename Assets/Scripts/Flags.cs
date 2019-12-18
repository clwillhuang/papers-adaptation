using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

/// <summary>
/// Attach to every piece of information that can be selected.
/// </summary>
public class Flags : MonoBehaviour {

	/// <summary>
	/// True if this is a reference word. (i.e. Is entry in dictionary/manual/reference material) 
	/// </summary>
	public bool canVerify = false; 

	public List<FlagsDef.Violation> violations;

	public int refNumber; 

	// Use this for initialization
	void Awake () {
		violations = new List<FlagsDef.Violation> (); 
	}

	public void OnMouseDownCommand ()
	{ 
		if (StateManager.current.inCorrectionMode) {
			Debug.Log ("Word " + GetComponent<Text> ().text + " is being pressed.");
//			Comment out if you want all words to be highlightable.
//			if (violations.Count == 0) {
//				return; 
//			}
			TextFlow parentScript = transform.parent.parent.GetComponent<TextFlow> ();
			if (parentScript != null) {
				transform.parent.parent.GetComponent<TextFlow> ().highlightSelection (transform.parent.GetSiblingIndex (), transform.GetSiblingIndex (), violations, canVerify, transform.position); 
			} else {
				Debug.Log ("No attached Textflow script on" + transform.name); 
			}

		} else {
			if (transform.parent.parent.parent.GetComponent<DragTransform> () != null) {
				transform.parent.parent.parent.GetComponent<DragTransform> ().OnMouseDown (); 
				}
		}
	}

	public void OnMouseUpCommand()
	{
		if (transform.parent.parent.parent.GetComponent<DragTransform> () != null) {
			transform.parent.parent.parent.GetComponent<DragTransform> ().OnMouseUp (); 
		}
	}

}
