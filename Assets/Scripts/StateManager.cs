using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using System.Linq; 

public class StateManager : MonoBehaviour {

	public bool inCorrectionMode = false; 

	public GameObject highlightPrefab; 
	public List<GameObject> activeHighlight = new List<GameObject>(); 

	public List<FlagsDef.Violation> first;
	public Vector3 firstLoc; 
	public List<FlagsDef.Violation> second; 
	public Vector3 secondLoc; 
	public bool firstIsRef, secondisRef;
	public TextFlow selectionTF; 
	public int tfRefNumber;
	public bool canCommit = false; 

	public int doneDocuments = -1;
	public bool openedParcel = false; 
	public GameObject Day4Parcel; 

	public float points = 0f;

	/// <summary>
	/// List of all scriptable documents to be given to player.
	/// </summary>
	public ScriptableDocument[] scriptableDocuments; 
	/// <summary>
	/// Next document to be given to player; 
	/// </summary>
	public ScriptableDocument nextDocument; 
	/// <summary>
	/// The current active document. 
	/// </summary>
	public GameObject document;
	/// <summary>
	/// The printed copy of the active document. 
	/// </summary>
	public GameObject printedDocument; 
	/// <summary>
	/// The document prefab.
	/// </summary>
	public GameObject documentPrefab; 
	public int nextDocumentRef;
	public GameObject currentMemo; 

	public Discrep discrepScript; 
	public Text lineText; 
	public Text commitText;

	public LineRenderer line; 
	public Color correctDiscrep, incorrectDiscrep; 
	public FlagsDef.Violation vioType; 

	public AnimationClip slidingAnim; 

	public GameObject citationPrefab; 

	public GameObject memoPrefab; 

	public float top = 521f; 
	public float left = 1024f - 704f; 

	/// <summary>
	/// Z coordinate of line renderer positions.
	/// </summary>
	private float lineDepth = -8.0f; 

	public Text scoreDisplay; 

	public static StateManager _current;
	public static StateManager current { 
		get {
			if (_current == null) {
				_current = FindObjectOfType<StateManager> (); 
			}
			return _current;
		}		
	}

	// Use this for initialization
	void Start () {
		inCorrectionMode = false; 
		first = null;
		second = null;
		firstIsRef = false;
		secondisRef = false; 
		firstLoc = Vector3.zero;
		secondLoc = Vector3.zero;
		lineText.gameObject.SetActive (false); 

		scriptableDocuments = Resources.LoadAll ("", typeof(ScriptableDocument)).Cast<ScriptableDocument>().ToArray(); 
		Debug.Log ("Loaded " + scriptableDocuments.Length.ToString () + " documents from Resources.");
		if (scriptableDocuments.Length > 0) {
			nextDocument = scriptableDocuments [0]; 
			nextDocumentRef = 0;
		} else {
			nextDocument = null; 
		} 
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Q)) {
			toggleCorrectionMode (); 
		}
		if (Input.GetKeyDown(KeyCode.E)) {
			StateManager.current.ReOrderLayers (); 
			LayoutRebuilder.ForceRebuildLayoutImmediate (transform.GetComponent<RectTransform> ());
		}
		scoreDisplay.text = "SCORE: " + points.ToString (); 
	}

	public void toggleCorrectionMode()
	{
		Debug.Log ("Correction mode toggled."); 
		inCorrectionMode = !inCorrectionMode;
		foreach (Transform activeChild in transform) {
			BoxCollider2D bc = activeChild.GetComponent<BoxCollider2D> (); 
			if (bc == null) {
				continue;
			}
			// bc.enabled = !inCorrectionMode; 
		}
		if (inCorrectionMode) {
			line.SetPositions (new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero }); 
		} else {
			lineText.gameObject.SetActive (false); 
		}
		line.enabled = inCorrectionMode; 
		return; 
	}

	public void SayDebug(string something)
	{
		Debug.Log (something); 
	}

	/// <summary>
	/// A nightmare. Reorder the active documents.
	/// </summary>
	public void ReOrderLayers()
	{
		foreach (Transform child in transform) {
			RectTransform c = child.GetComponent<RectTransform> ();
			if (c == null) {
				continue;
			} 
			child.GetComponent<RectTransform> ().localPosition = new Vector3 (c.localPosition.x, c.localPosition.y, -(2.0f + (int)child.GetSiblingIndex ()));
			//Debug.Log ("Sibling index is " + child.GetSiblingIndex().ToString() + " " + child.name + " " + -(2.0f + (int)child.GetSiblingIndex ()) + " " + child.GetComponent<RectTransform>().localPosition); 
		}
	}

	/// <summary>
	/// Draw the discrep line
	/// </summary>
	/// <param name="pos1">Position of first selection.</param>
	/// <param name="pos2">Position of second selection..</param>
	/// <param name="isDiscrep">If set to <c>true</c>, there is a confirmed discrep.</param>
	public void DrawDiscrepLine (Vector3 pos1, Vector3 pos2, bool isDiscrep)
	{
		line.enabled = true; 
		Vector3[] pos = new Vector3[] {
			new Vector3 (pos1.x, pos1.y, lineDepth),
			new Vector3 (pos1.x, pos2.y, lineDepth),
			new Vector3 (pos2.x, pos2.y, lineDepth),
		};
		Debug.Log (pos); 
		line.SetPositions (pos); 

		if (isDiscrep) {
			line.startColor = correctDiscrep;
			line.endColor = correctDiscrep; 
			lineText.color = correctDiscrep; 
			lineText.text = "POSITIVE MATCH";
			commitText.text = "Press C to commit change.";
			commitText.color = Color.green; 
			commitText.gameObject.SetActive (true); 
		} else {
			line.startColor = incorrectDiscrep;
			line.endColor = incorrectDiscrep; 
			lineText.color = incorrectDiscrep;
			lineText.text = "NEGATIVE MATCH";
		}
		lineText.gameObject.SetActive (true); 
	} 

	/// <summary>
	/// Call when two items are selected and must be checked for discrep. 
	/// </summary>
	public bool CheckDiscrepancy()
	{
		vioType = FlagsDef.Violation.d1;
		if (firstIsRef != secondisRef) {
			bool isMatch = false; 
			// Check if has violation flag
			// Is a matching discrepancy if any reference violation is contianed in non-reference resource 
			if (firstIsRef) {
				foreach (FlagsDef.Violation item in first) {
					if (second.Contains(item)) {
						isMatch = true; 
						vioType = item; 
						break; 
					}
				}
			} else {
				foreach (FlagsDef.Violation item in second) {
					if (first.Contains(item)) {
						isMatch = true; 
						vioType = item; 
						break; 
					}
				}
			}
			DrawDiscrepLine (firstLoc, secondLoc, isMatch); 
			if (isMatch) {
				Debug.Log ("Discrepancy detected!!!!!!!"); 
				canCommit = true; 
				//selectionTF.EraseDiscrepancy (tfRefNumber, vioType); 
				return true;
			} else {
				Debug.Log ("False discrepancy detected.");
				return false;
			}
		} else {
			Debug.Log ("Neither selection are references."); 
			DrawDiscrepLine (firstLoc, secondLoc, false);
			return false;
		}
	}

	/// <summary>
	/// Call to create next document. 
	/// </summary>
	public void CreateDocument()
	{
		if (document != null || printedDocument != null || currentMemo != null) {
			StateManager.current.StopCoroutine ("SaySomething"); 
			StartCoroutine (SaySomething ("Dispose the document, replacement and memo before continuing.", 5f, Color.red)); 
			return; 
		}
		doneDocuments++; 

		if ((doneDocuments >= 5 && Clock.current.day != 4) || (doneDocuments >= 6 && Clock.current.day == 4)) {
			if (!openedParcel && Clock.current.day == 4) {
				StateManager.current.StopCoroutine ("SaySomething"); 
				StartCoroutine (SaySomething ("The Brotherhood cannot wait. Open it.", 5f, Color.red)); 
				return; 
			}
			Clock.current.EndDay ();
			return; 
		}

		if (Clock.current.day == 4 && doneDocuments == 1) {
			Debug.Log ("Making Parcel"); 
			openedParcel = false;
			GameObject newItem = Instantiate (Day4Parcel); 
			newItem.GetComponent<DragTransform> ().canBeDestroyed = false; 
			GiveAnimation (newItem);
			return; 
		}

		if (nextDocument == null || nextDocumentRef >= scriptableDocuments.Length) {
			Debug.Log ("No more docs"); 
			return; 
		}

		document = Instantiate (documentPrefab, GameObject.Find ("BottomLeft").transform); 
		Animation anim = document.AddComponent<Animation> () as Animation; 

		TextFlow title = document.transform.GetChild (0).GetComponent<TextFlow> (); 
		TextFlow body = document.transform.GetChild (1).GetComponent<TextFlow> (); 

		document.GetComponent<DragTransform> ().canBeDestroyed = true; 

		title.correctedMessage = nextDocument.title;
		title.message = nextDocument.title;
		title.alternateTitle = nextDocument.alternateTitle; 
		body.message = nextDocument.message; 
		body.correctedMessage = nextDocument.message;
		body.replacedSnippets = nextDocument.replacedSnippets;

		currentMemo = Instantiate (memoPrefab);
		if (nextDocument.memo != "") {
			currentMemo.transform.Find ("Memo Text").GetComponent<Text> ().text = nextDocument.memo; 
		}
		GiveAnimation (currentMemo); 

		StartCoroutine (cloningDoc(title)); 
		StartCoroutine (cloningDoc(body)); 

		anim.playAutomatically = false;
		anim.clip = slidingAnim; 
		anim.AddClip (slidingAnim, "PaperEntry"); 
		anim.Play ("PaperEntry"); 

		if (nextDocumentRef + 1 < (int)scriptableDocuments.Length) {
			nextDocumentRef++;
			nextDocument = scriptableDocuments[nextDocumentRef]; 
		} else {
			nextDocument = null; 
		}
	}


	public void GiveAnimation (GameObject item)
	{
		item.transform.SetParent (GameObject.Find ("BottomLeft").transform); 
		Animation anim = item.AddComponent<Animation> () as Animation; 
		if (anim == null) {
			anim = item.transform.GetComponent<Animation> () as Animation; 
		}
		item.GetComponent<RectTransform> ().localScale = new Vector3 (0.5f, 0.5f, 0.5f); 
		anim.playAutomatically = false;
		anim.clip = slidingAnim; 
		anim.AddClip (slidingAnim, "PaperEntry"); 
		anim.Play ("PaperEntry");
		item.GetComponent<RectTransform> ().localScale = new Vector3 (0.5f, 0.5f, 0.5f); 
	} 

	public void GiveCitation(string text)
	{
		GameObject newCitation = Instantiate (citationPrefab); 
		newCitation.transform.Find ("Citation Text").GetComponent<Text> ().text = text; 
		newCitation.GetComponent<DragTransform> ().canBeDestroyed = true; 
		GiveAnimation(newCitation); 
	}

	IEnumerator cloningDoc(TextFlow clone)
	{
		clone.DeleteOld ();
		yield return null;
		clone.OnEnable (); 
	}

	/// <summary>
	/// Print the corrected copy. 
	/// </summary>
	public void Print ()
	{
		Debug.Log ("Printing new item."); 
		if (printedDocument != null) {
			StateManager.current.StopCoroutine ("SaySomething"); 
			Debug.Log ("Can't print a new document if printed one already exists."); 
			StartCoroutine (SaySomething ("Can't print a replacement if replacement already exists on table.", 3f, Color.red));  
			return;
		}
		else if (document == null) {
			Debug.Log ("First document doesn't exist."); 
			return; 
		}
		printedDocument = Instantiate (document, GameObject.Find ("BottomLeft").transform); 
		printedDocument.GetComponent<DragTransform> ().canBeDestroyed = true; 
		TextFlow[] tf = printedDocument.GetComponentsInChildren<TextFlow> ();
		foreach (TextFlow item in tf) {
			item.message = item.correctedMessage;
			StartCoroutine (cloningDoc(item));
		}
		Animation anim = printedDocument.transform.GetComponent<Animation> (); 
		anim.playAutomatically = false;
		anim.clip = slidingAnim; 
		anim.AddClip (slidingAnim, "PaperEntry"); 
		anim.Play ("PaperEntry"); 
	}

	public void DestroyDocument (GameObject doc)
	{
		if (doc == printedDocument && printedDocument != null) {
//			TextFlow printedTitleTF = printedDocument.transform.GetChild (0).GetComponent<TextFlow> ();
//			TextFlow printedTF = printedDocument.transform.GetChild (1).GetComponent<TextFlow> ();
//
//			int totalCorrected = printedTitleTF.correctedViolations + printedTF.correctedViolations; 
//			int remainingViolations = printedTitleTF.violationCounter + 1 + printedTF.violationCounter + 1;
//
//			int bStamps = printedDocument.GetComponent<DragTransform> ().burntStamps;
//			int sStamps = printedDocument.GetComponent<DragTransform> ().storageStamps; 
//
//			Debug.Log ((printedTitleTF.violationCounter + 1).ToString () + " " + (printedTF.violationCounter + 1).ToString ()); 
//
//			points += totalCorrected * 100f;
//			points -= (remainingViolations) * 50f; 
//
//			Debug.Log ("+" + (totalCorrected * 100f).ToString () + " pts from corrected " +
//			"-" + (remainingViolations * 50f).ToString () + " from missed Score: " + totalCorrected.ToString () + " with rem: " + remainingViolations.ToString ()); 
//
//			if (remainingViolations > 0 || bStamps + sStamps != 0) {
//				string citationText = "MISSED VIOLATIONS: -" + (remainingViolations * 50).ToString() + " total pts:\n"; 
//				List<FlagsDef.Violation> combinedTypes = new List<FlagsDef.Violation> (printedTF.listOfAllViolations);
//				combinedTypes.AddRange (printedTitleTF.listOfAllViolations); 
//				foreach (FlagsDef.Violation v in combinedTypes) {
//					citationText = citationText + "     " + FlagsDef.violationArrayInString [((int)v)] + "\n"; 
//				}
//				points -= 50f;
//				if (bStamps + sStamps == 0) {
//					citationText = citationText + "MISSING STORAGE STAMP. -50pts";
//				}
//				else if (bStamps >= 1 && sStamps == 0) {
//					citationText = citationText + "ITEM DESTROYED. ADD STORAGE STAMP FOR REPLACEMENT DOCS. -25pts";
//					points += 25f;
//				}
//				else if (bStamps >= 1) {
//					citationText = citationText + "INCORRECT TYPE OR NUMBER OF STAMP(S) GIVEN. -50pts";
//				}
//				else if (sStamps > 1) {
//					citationText = citationText + "TOO MANY STORAGE STAMPS. -50pts";
//				} else {
//					points += 50f;
//				}
//				GiveCitation (citationText); 
//			}
//			GameObject.Destroy (printedDocument.gameObject); 
//			printedDocument = null;
			CitationChecker.current.setPrintedDocument(doc); 
			CitationChecker.current.Check (); 
		} else if (doc == document && document != null) {

//			int bStamps = document.GetComponent<DragTransform> ().burntStamps;
//			int sStamps = document.GetComponent<DragTransform> ().storageStamps; 
//
//			string citationText = "";
//			points -= 50f; 
//			if (bStamps + sStamps == 0) {
//				citationText = citationText + "MISSING BURN STAMP. -50pts";
//			} else if (sStamps >= 1) {
//				citationText = citationText + "INCORRECT STAMP(S) GIVEN. -50pts";
//			} else if (bStamps > 1) {
//				citationText = citationText + "TOO MANY BURN STAMPS. -50pts";
//			} else
//				points += 50f;
//			if (citationText != "") {
//				GiveCitation (citationText); 
//			}
//
//			GameObject.Destroy (document.gameObject); 
//			document = null; 
			CitationChecker.current.setDocument(doc); 
			CitationChecker.current.Check (); 
		} else if (doc != null && doc.GetComponent<DragTransform>().canBeDestroyed) {
			GameObject.Destroy (doc.gameObject); 
		}
	}

	public IEnumerator SaySomething(string message, float delay, Color color)
	{
		commitText.text = message;
		commitText.color = color; 
		commitText.enabled = true; 
		StateManager.current.commitText.gameObject.SetActive (true); 
		yield return new WaitForSeconds (delay); 
		commitText.text = "";
		StateManager.current.commitText.gameObject.SetActive (false); 
	}

	public void ClearOffTable (bool deleteManual)
	{
		foreach (Transform child in transform) {
			if (child.GetComponent<DragTransform> () == null) {
				continue; 
			} else if (child.name != "Manual" && child.name != "Unpersons") {
				GameObject.Destroy (child.gameObject);
			} else {
				if (deleteManual) {
					child.GetComponent<Animation> ().Play ("ClearOff"); 
				} else {
					child.localPosition = new Vector3 (0f, 0f, 0f); 
				}
			}
		}
		foreach (Transform child in GameObject.Find("BottomLeft").transform) {
			if (child.GetComponent<DragTransform> () == null) {
				continue; 
			} else if (child.name != "Manual" && child.name != "Unpersons") {
				GameObject.Destroy (child.gameObject);
			} else {
				if (deleteManual) {
					child.GetComponent<Animation> ().Play ("ClearOff"); 
				}else {
					child.localPosition = new Vector3 (0f, 0f, 0f); 
				}
			}
		}
	}

	public void ShowTable()
	{
		for (int i = 0; i < transform.childCount; i++) {
			Transform c = transform.GetChild (i); 
			if ((c.name == "Unpersons" || c.name == "Manual")) {
				c.gameObject.SetActive (true); 
				c.localPosition = new Vector3 (0f, 0f, 0f); 
			}
		}
		for (int i = 0; i < GameObject.Find("BottomLeft").transform.childCount; i++) {
			Transform c = GameObject.Find("BottomLeft").transform.GetChild (i); 
			if (c.name == "Unpersons" || c.name == "Manual") {
				c.gameObject.SetActive (true); 
				c.localPosition = new Vector3 (0f, 0f, 0f); 
			}
		}
	}
}
