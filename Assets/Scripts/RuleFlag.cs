using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class RuleFlag : MonoBehaviour {

	public GameObject highlightPrefab;
	public FlagsDef.Violation violation; 
	
	// Use this for initialization
	void Start () {
	}
	
	public void OnMouseDownCommand ()
	{
		if (!StateManager.current.inCorrectionMode) {
			return;
		}
		if (StateManager.current.first != null && StateManager.current.second != null) {
			return; 
		}
		if (transform.childCount > 0) {
			Debug.Log ("Rule flag has already selected!");
			return; 
		}
		GameObject highlight = Instantiate (highlightPrefab, transform); 
		StateManager.current.activeHighlight.Add (highlight); 
		LayoutFixer (highlight, transform); 
		LayoutRebuilder.ForceRebuildLayoutImmediate (transform.GetComponent<RectTransform> ()); 
		if (StateManager.current.first == null) {
			StateManager.current.first = new List<FlagsDef.Violation>() { violation }; 
			StateManager.current.firstLoc = transform.position; 
			StateManager.current.firstIsRef = true; 
		} else if (StateManager.current.second == null) {
			StateManager.current.second = new List<FlagsDef.Violation>() { violation }; 
			StateManager.current.secondLoc = transform.position; 
			StateManager.current.secondisRef = true;
			StateManager.current.CheckDiscrepancy (); 
		} else {
			return;
		}
	} 

	public void LayoutFixer(GameObject highlightGO, Transform textGO)
	{
		RectTransform rt = highlightGO.GetComponent<RectTransform> (); 
		RectTransform tt = textGO.GetComponent<RectTransform> (); 
		rt.anchoredPosition = new Vector2 (0f, 2f); 
		rt.sizeDelta = new Vector2 (tt.sizeDelta.x, tt.sizeDelta.y); 
		StateManager.current.activeHighlight.Add (highlightGO);

		float leftPixel = Camera.main.WorldToScreenPoint (textGO.transform.position).x - textGO.transform.GetComponent<RectTransform>().rect.width/2; 
		//Debug .Log("Left PIXEL IS " + leftPixel.ToString ()); 
		if (leftPixel < StateManager.current.left) {
			float offset = leftPixel - StateManager.current.left; 
			rt.anchoredPosition = new Vector2 (-offset / 2, rt.anchoredPosition.y); 
			rt.sizeDelta = new Vector2 (rt.sizeDelta.x + offset, rt.sizeDelta.y); 
			//Debug.Log("Adjusting left pixel " + leftPixel.ToString() + " " + StateManager.current.left.ToString() + " " + offset.ToString()); 
		}
		Debug.Log ("Unaltered is" + Camera.main.WorldToScreenPoint (textGO.transform.position).y); 
		float topPixel = Camera.main.WorldToScreenPoint (textGO.transform.position).y + textGO.transform.GetComponent<RectTransform>().rect.height / 2; 
		Debug.Log("Top PIXEL IS " + topPixel.ToString ()); 
		if (topPixel > StateManager.current.top) {
			float offset = StateManager.current.top - topPixel; 
			rt.anchoredPosition = new Vector2 (rt.anchoredPosition.x, offset/2); 
			rt.sizeDelta = new Vector2 (rt.sizeDelta.x, rt.sizeDelta.y + offset); 
			Debug.Log("Adjusting left pixel " + topPixel.ToString() + " " + StateManager.current.top.ToString() + " " + offset.ToString()); 
		}
	}

}
