using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq ;

public class TextFlow : MonoBehaviour {

	[TextArea(10, 20)]  
	public string message;
	[TextArea(5, 10)]  
	public string correctedMessage;
	[TextArea(2, 10)]
	public List<string> replacedSnippets;
	public string alternateTitle; 


	public string word = "";
	public GameObject prefabText;
	public GameObject prefabRow; 
	public float maxWidth, currentWidth; 
	public float spacing; 
	private float leftspacing, rightspacing;
	public int textSize;

	public bool isTitle;

	/// <summary>
	/// Whether this item is in the manual
	/// </summary>
	public bool isManual; 

	public int activeFlags = 0, comm = 0, violationCounter = -1, correctedViolations = 0; 
	private List<FlagsDef.Violation> violations;
	public List<FlagsDef.Violation> listOfAllViolations;

	public int updatedChildCount = 0; 

	public void DeleteOld ()
	{
		foreach (Transform textChild in transform) {
			GameObject.Destroy (textChild.gameObject); 
		}	
		foreach (Transform child in transform.parent.transform) {
			if (child != transform && child.GetComponent<TextFlow>() == null) {
				GameObject.Destroy (child.gameObject);  
			}
		}
	}

	// Use this for initialization
	public void OnEnable ()
	{
		maxWidth = transform.parent.transform.GetComponent<RectTransform> ().rect.width - 40f;
		currentWidth = 0.0f; 
		spacing = prefabRow.GetComponent<HorizontalLayoutGroup> ().spacing; 
		leftspacing = transform.GetComponent<VerticalLayoutGroup> ().padding.left;
		rightspacing = transform.GetComponent<VerticalLayoutGroup> ().padding.right;
		violations = new List<FlagsDef.Violation> (); 
		comm = 0;
		activeFlags = 0; 
		word = ""; 
		listOfAllViolations = new List<FlagsDef.Violation> (); 
		updatedChildCount = 0; 
		violationCounter = -1;
		if (correctedMessage == "") {
			correctedMessage = message; 
		}

		Debug.Log ("Will create new text with " + message + " " + updatedChildCount + " " + transform.parent.name); 

		for (int i = 0; i < message.Length; i++) {
			comm--; 
//			if (message[i] == '\n') {
//				Debug.Log ("New line detected."); 
//				Instantiate (prefabRow, transform); 
//				currentWidth = leftspacing + rightspacing;
//			}
			if (message [i] != ' ' && message [i] != '\\' && comm < 0) {
				word = word + message [i]; 
			} 
			if (message [i] == ' ' || i == message.Length - 1 || message [i] == '\\') {
				if (updatedChildCount == 0){
					Instantiate (prefabRow, transform); 
					updatedChildCount++; 
				}
				//Debug.Log (transform.GetChild (transform.childCount - 1).name); 
				if (word.Length > 0) {
					GameObject newText = Instantiate (prefabText, transform.GetChild (updatedChildCount - 1).transform); 
					if (textSize != 0) {
						newText.GetComponent<Text> ().fontSize = textSize; 
					}
					newText.GetComponent<Text> ().text = word;
					Flags newScript = newText.GetComponent<Flags> (); 
					LayoutRebuilder.ForceRebuildLayoutImmediate (transform.GetComponent<RectTransform> ()); 
					// Check if overflow text
					if (newText.GetComponent<RectTransform> ().rect.width + currentWidth >= maxWidth) {
						currentWidth = newText.GetComponent<RectTransform> ().rect.width + leftspacing + rightspacing;
						Instantiate (prefabRow, transform);
						updatedChildCount++;
						newText.transform.SetParent (transform.GetChild (updatedChildCount - 1).transform); 
					} else {
						currentWidth += newText.GetComponent<RectTransform> ().rect.width + spacing;
					}

					// Update trigger box.
					newText.GetComponent<BoxCollider2D> ().size = new Vector2 (newText.GetComponent<RectTransform> ().rect.width + spacing, 
																			   newText.GetComponent<RectTransform> ().rect.height + spacing);
					// TODO: Allow the selecting of other pieces of text. (Maybe?) 

					//Debug.Log ("Made word" + word + " " + newText.GetComponent<RectTransform> ().rect.width);
					// Has a violation. 
					if (activeFlags > 0)  { 
						foreach (FlagsDef.Violation v in violations) {
							newScript.violations.Add (v); 
						}
						newScript.refNumber = violationCounter;
					}
					// Check if is reference material
					newScript.canVerify = isManual; 
				}
				word = ""; 
			}	
			// Command detected, adding appropriate violation flag. 
			if (message[i] == '\\') {
				comm = 3; 
				FlagsDef.Violation newCommand = FlagsDef.getViolation (message[i+1].ToString() + message [i+2].ToString());
				Debug.Log (newCommand + " command found!"); 
				if (violations.Contains(newCommand)) {
					violations.Remove (newCommand); 
					activeFlags--; 
				} else {
					violations.Add (newCommand); 
					activeFlags++; 
					violationCounter++;
					listOfAllViolations.Add (newCommand); 
				}
			}
		}
		if (activeFlags < 0) {
			Debug.Log ("Too many flags removed: " + activeFlags); 
		}
		else if (activeFlags > 0) {
			Debug.Log ("Too many flags initialized: " + activeFlags); 
		}
	}

	public void saystuff (string stuff)
	{
		Debug.Log (stuff); 
	}

	/// <summary>
	/// Checks for more things to be selected.
	/// </summary>
	/// <param name="thisIndex">Row's child index.</param>
	/// <param name="childIndex">Child index within the row.</param>
	public void highlightSelection (int thisIndex, int childIndex, List<FlagsDef.Violation> matchingViolations, bool isRef, Vector3 loc)
	{
		bool stopLoop = false; 
		int startChild = childIndex; 
		int referenceNumber = -1;

		// Check if is on left side. 
		DragTransform dt = transform.parent.GetComponent<DragTransform> (); 
		if (!dt.onRightSide) {
			return; 
		}
		if (StateManager.current.first == null || StateManager.current.second == null) {
			for (int rowIndex = thisIndex; rowIndex < transform.childCount; rowIndex++) {
				Transform rowGO = transform.GetChild (rowIndex); 
				if (rowIndex != thisIndex) {
					startChild = 0; 
				}
				for (int index = startChild; index < rowGO.transform.childCount; index++) {
					Transform textGO = rowGO.GetChild (index); 
					if (textGO.childCount > 0) {
						if (rowIndex == thisIndex && index == childIndex) {
							Debug.Log ("Already selected!");
							return; 
						}
						continue; 
					}
					//Debug.Log ("Scanning " + textGO.GetComponent<Text> ().text); 
					if (index != childIndex && matchingViolations.Count == 0) {
						stopLoop = true;
						break; 
					}
					if (textGO.GetComponent<Flags> ().violations.SequenceEqual (matchingViolations)) {
						GameObject highlightGO = Instantiate (StateManager.current.highlightPrefab, textGO); 
						referenceNumber = textGO.GetComponent<Flags> ().refNumber; 
						LayoutFixer (highlightGO, textGO); 

					} else {
						//Debug.Log ("Stopped scan, no equals" + matchingViolations.Count + " " + textGO.GetComponent<Flags> ().violations.Count); 
						stopLoop = true;
						break; 
					}
				}
				if (stopLoop) {
					break; 
				}
			}
			stopLoop = false; 
			startChild = childIndex; 
			for (int rowIndex = thisIndex; rowIndex >= 0; rowIndex--) {
				Transform rowGO = transform.GetChild (rowIndex); 
				if (rowIndex != thisIndex) {
					startChild = rowGO.childCount-1; 
				}
				for (int index = startChild; index >= 0; index--) {
					Transform textGO = rowGO.GetChild (index); 
					if (textGO.childCount > 0) {
						continue; 
					}
					//Debug.Log ("Scanning " + textGO.GetComponent<Text> ().text);
					if (index != childIndex && matchingViolations.Count == 0) {
						stopLoop = true;
						break; 
					} 
					if (textGO.childCount >0) {
						continue;
					}
					if (textGO.GetComponent<Flags> ().violations.SequenceEqual (matchingViolations)) {
						GameObject highlightGO = Instantiate (StateManager.current.highlightPrefab, textGO); 
						referenceNumber = textGO.GetComponent<Flags> ().refNumber; 
						LayoutFixer (highlightGO, textGO); 
					} else {
						//Debug.Log ("Stopped scan, no equals" + matchingViolations.Count + " " + textGO.GetComponent<Flags> ().violations.Count); 
						stopLoop = true;
						break; 
					}
				}
				if (stopLoop) {
					break; 
				}
			}
			StateManager.current.ReOrderLayers (); 
			LayoutRebuilder.ForceRebuildLayoutImmediate (transform.GetComponent<RectTransform> ()); 

		}
		if (StateManager.current.first == null) {
			StateManager.current.first = matchingViolations; 
			StateManager.current.firstLoc = loc; 
			StateManager.current.firstIsRef = isRef; 
			StateManager.current.selectionTF = this;
			StateManager.current.tfRefNumber = referenceNumber; 
		} else if (StateManager.current.second == null) {
			StateManager.current.second = matchingViolations; 
			StateManager.current.secondLoc = loc; 
			StateManager.current.secondisRef = isRef;
			StateManager.current.selectionTF = this;
			StateManager.current.tfRefNumber = referenceNumber; 
			// Check if one is a reference resource
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
		rt.sizeDelta = new Vector2 (tt.sizeDelta.x + spacing, tt.sizeDelta.y+ spacing/2); 
		StateManager.current.activeHighlight.Add (highlightGO);

		float leftPixel = Camera.main.WorldToScreenPoint (textGO.transform.position).x - textGO.transform.GetComponent<RectTransform>().rect.width/2; 
		//Debug .Log("Left PIXEL IS " + leftPixel.ToString ()); 
		if (leftPixel < StateManager.current.left) {
			float offset = leftPixel - StateManager.current.left; 
			rt.anchoredPosition = new Vector2 (-offset / 2, rt.anchoredPosition.y); 
			rt.sizeDelta = new Vector2 (rt.sizeDelta.x + offset - spacing, rt.sizeDelta.y); 
			//Debug.Log("Adjusting left pixel " + leftPixel.ToString() + " " + StateManager.current.left.ToString() + " " + offset.ToString()); 
		}

		float topPixel = Camera.main.WorldToScreenPoint (textGO.transform.position).y + textGO.transform.GetComponent<RectTransform>().rect.height / 2; 
		Debug.Log("Top PIXEL IS " + topPixel.ToString ()); 
		if (topPixel > StateManager.current.top) {
			float offset = StateManager.current.top - topPixel; 
			rt.anchoredPosition = new Vector2 (rt.anchoredPosition.x, offset/2); 
			rt.sizeDelta = new Vector2 (rt.sizeDelta.x, rt.sizeDelta.y + offset - 3f); 
			Debug.Log("Adjusting left pixel " + topPixel.ToString() + " " + StateManager.current.top.ToString() + " " + offset.ToString()); 
		}
	}

	public void EraseDiscrepancy(int refNumber, FlagsDef.Violation vioType)
	{
		if (isTitle) {
			correctedMessage = alternateTitle; 
			correctedViolations++; 
			listOfAllViolations.Remove (vioType); 
			return; 
		}
		if (refNumber == -1) {
			Debug.Log ("Invalid reference number"); 
			return; 
		}
		Debug.Log ("Trying to erase discrep on " + transform.name + " " + refNumber.ToString ()); 
		short copyPhase = -1;
		string phrase = ""; 
		int index = 0; 
		for (int i = 0; i < correctedMessage.Length; i++) {
			if (correctedMessage[i] == '\\') {
				if ((int)correctedMessage[i+3] - 48 == refNumber) {
					Debug.Log ("Found reference!"); 
					copyPhase++; 
					index = i; 
				}
				if (copyPhase >= 1) {
					phrase = phrase + correctedMessage [i] + correctedMessage [i + 1] + correctedMessage [i + 2] + correctedMessage [i + 3];
					break;
				}
			}
			if (copyPhase >= 0) {
				phrase += correctedMessage [i]; 
			}
		}	
		if (phrase == "") {
			StateManager.current.StopCoroutine ("SaySomething"); 
			StartCoroutine (StateManager.current.SaySomething ("Change already made.", 2.0f, Color.red)); 
			return; 
		}
		Debug.Log ("Replacing " + phrase + " with " + replacedSnippets [refNumber]); 
		correctedViolations++; 
		listOfAllViolations.Remove (vioType); 
		if (refNumber >= replacedSnippets.Count) {
			if (replacedSnippets.Count == 0) {
				
			} else { 
				correctedMessage = correctedMessage.Replace (phrase, replacedSnippets [replacedSnippets.Count - 1]); 
			}
		} else {
			correctedMessage = correctedMessage.Replace (phrase, replacedSnippets [refNumber]); 
		}
		correctedMessage = correctedMessage.Replace ("  ", " "); 
		correctedMessage = correctedMessage.Replace (" .", "."); 
		correctedMessage = correctedMessage.Replace (" ,", ","); 
		correctedMessage = correctedMessage.Replace (" !", "!"); 
		return;
	}

}
