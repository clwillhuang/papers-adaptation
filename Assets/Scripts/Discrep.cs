using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Discrep : MonoBehaviour {

	public Sprite On, Off;
	public Text discrepText; 
	public Image background;

	void Awake()
	{
		discrepText.enabled = false; 
		background.enabled = false; 
	}

	public void Update ()
	{
		if (Input.GetKeyDown(KeyCode.D)) {
			OnMouseDown (); 
		}
		if (Input.GetKeyDown(KeyCode.C) && StateManager.current.inCorrectionMode && StateManager.current.selectionTF != null && StateManager.current.vioType != null) {
			StateManager.current.selectionTF.EraseDiscrepancy (StateManager.current.tfRefNumber, StateManager.current.vioType); 
			StateManager.current.commitText.gameObject.SetActive (false); 
		}
		if (StateManager.current.inCorrectionMode) {
			discrepText.enabled = true; 
		}
	}	

	void OnMouseDown()
	{
		StateManager.current.toggleCorrectionMode (); 
		if (StateManager.current.inCorrectionMode) {
			turnDiscrepOn (); 
		} else {
			turnDiscrepOff (); 
		}
	}

	public void turnDiscrepOn()
	{
		Debug.Log ("Hello"); 
		StateManager.current.canCommit = false; 
		GetComponent<SpriteRenderer> ().sprite = On;
		discrepText.enabled = true; 
		background.enabled = true; 
		for (int i = StateManager.current.activeHighlight.Count-1; i >= 0; i--) {
			GameObject.Destroy (StateManager.current.activeHighlight [i].gameObject);
		}
		StateManager.current.first = null; 
		StateManager.current.second = null; 
		StateManager.current.firstIsRef = false;
		StateManager.current.secondisRef = false; 
		StateManager.current.activeHighlight.Clear (); 
		StateManager.current.ReOrderLayers (); 
		StateManager.current.selectionTF = null;
		StateManager.current.tfRefNumber = -1;
		LayoutRebuilder.ForceRebuildLayoutImmediate (transform.GetComponent<RectTransform> ());
		StateManager.current.ReOrderLayers (); 
		LayoutRebuilder.ForceRebuildLayoutImmediate (transform.GetComponent<RectTransform> ());
	}

	public void turnDiscrepOff()
	{
		StateManager.current.canCommit = false; 
		GetComponent<SpriteRenderer> ().sprite = Off;
		discrepText.enabled = false; 
		background.enabled = false; 
		StateManager.current.commitText.gameObject.SetActive (false); 
		for (int i = StateManager.current.activeHighlight.Count-1; i >= 0; i--) {
			GameObject.Destroy (StateManager.current.activeHighlight [i].gameObject);
		}
		StateManager.current.first = null; 
		StateManager.current.second = null; 
		StateManager.current.firstIsRef = false;
		StateManager.current.secondisRef = false; 
		StateManager.current.activeHighlight.Clear (); 
		StateManager.current.ReOrderLayers (); 
		StateManager.current.selectionTF = null;
		StateManager.current.tfRefNumber = -1;
		LayoutRebuilder.ForceRebuildLayoutImmediate (transform.GetComponent<RectTransform> ());
	}
}
