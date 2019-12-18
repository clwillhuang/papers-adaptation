using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitationChecker : MonoBehaviour {

	public static CitationChecker _current;
	public static CitationChecker current { 
		get {
			if (_current == null) {
				_current = FindObjectOfType<CitationChecker> (); 
			}
			return _current;
		}		
	}

	public GameObject printedDocument, document;
	public TextFlow printedTitleTF, printedTF; 
	public int bStampsprinted, sStampsprinted, bStamps, sStamps; 
	string citationText; 

	public void setPrintedDocument (GameObject doc)
	{
		printedDocument = doc; 
		printedTitleTF = printedDocument.transform.GetChild (0).GetComponent<TextFlow> ();
		printedTF = printedDocument.transform.GetChild (1).GetComponent<TextFlow> ();
		bStampsprinted = printedDocument.GetComponent<DragTransform> ().burntStamps;
		sStampsprinted = printedDocument.GetComponent<DragTransform> ().storageStamps;
		doc.SetActive (false);
	}

	public void setDocument (GameObject doc)
	{ 
		document = doc; 
		bStamps = document.GetComponent<DragTransform> ().burntStamps;
		sStamps = document.GetComponent<DragTransform> ().storageStamps; 
		doc.SetActive (false);
	}

	public void Check() {

		citationText = ""; 

		if (document != null && StateManager.current.document != null && StateManager.current.printedDocument == null) {
			Debug.Log ("No printed document detected."); 
			StateManager.current.points = StateManager.current.points - 100f;
			citationText = "REPL - DID NOT FIND REPLACEMENT PAPER. -100pts\n"; 
			int missed = StateManager.current.document.transform.GetChild (0).GetComponent<TextFlow> ().violationCounter + StateManager.current.document.transform.GetChild (0).GetComponent<TextFlow> ().violationCounter; 
			if (missed != 0) {
				citationText += "MISSED ALL " + (-missed).ToString () + " VIOLATIONS: " + (missed * 50f).ToString () + " total pts.\n";
				StateManager.current.points = StateManager.current.points - (-missed * 50f); 
				Clock.current.errorsMissed -= missed; 
			}
			Clock.current.stampingErrors++; 
			if (bStamps + sStamps == 0) {
				citationText = citationText + "ORIG - MISSING BURN STAMP. -50pts\n";
				Clock.current.stampingErrors++; 
				StateManager.current.points -= 50f; 
			} else if (sStamps >= 1) {
				citationText = citationText + "ORIG - INCORRECT STAMP(S) GIVEN. -50pts\n";
				Clock.current.stampingErrors++; 
				StateManager.current.points -= 50f; 
			} else if (bStamps > 1) {
				citationText = citationText + "ORIG - TOO MANY BURN STAMPS. -50pts\n";
				Clock.current.stampingErrors++; 
				StateManager.current.points -= 50f; 
			} 
			if (citationText != "") {
				StateManager.current.GiveCitation (citationText); 
			}
			GameObject.Destroy (document.gameObject); 
			document = null;
			printedDocument = null; 
			StateManager.current.document = null;
			return; 
		}

		if ((printedDocument == null && StateManager.current.printedDocument != null) || (document == null && StateManager.current.document != null)) {
			Debug.Log ("Hand in all documents please.");
			return; 
		}

		int totalCorrected = printedTitleTF.correctedViolations + printedTF.correctedViolations; 
		int remainingViolations = printedTitleTF.violationCounter + 1 + printedTF.violationCounter + 1;
		Debug.Log ((printedTitleTF.violationCounter + 1).ToString () + " " + (printedTF.violationCounter + 1).ToString ()); 

		StateManager.current.points += totalCorrected * 100f;
		StateManager.current.points -= (remainingViolations) * 50f; 

		Clock.current.errorsMissed += remainingViolations; 
		Clock.current.errorsCorrected += totalCorrected;

		Debug.Log ("+" + (totalCorrected * 100f).ToString () + " pts from corrected " +
		"-" + (remainingViolations * 50f).ToString () + " from missed Score: " + totalCorrected.ToString () + " with rem: " + remainingViolations.ToString ()); 

		if (remainingViolations > 0) {
			citationText = "MISSED VIOLATIONS: -" + (remainingViolations * 50).ToString () + " total pts:\n"; 
			List<FlagsDef.Violation> combinedTypes = new List<FlagsDef.Violation> (printedTF.listOfAllViolations);
			combinedTypes.AddRange (printedTitleTF.listOfAllViolations); 
			foreach (FlagsDef.Violation v in combinedTypes) {
				citationText = citationText + "     " + FlagsDef.violationArrayInString [((int)v)] + "\n"; 
			} 
		}
		StateManager.current.points -= 50f;
		if (bStampsprinted + sStampsprinted == 0) {
			citationText = citationText + "REPL - MISSING STORAGE STAMP. -50pts\n";
			Clock.current.stampingErrors++; 
		} else if (bStampsprinted >= 1 && sStampsprinted == 0) {
			citationText = citationText + "REPL - ITEM DESTROYED. ADD STORAGE STAMP FOR REPLACEMENT DOCS. -25pts\n";
			StateManager.current.points += 25f;
			Clock.current.stampingErrors++; 
		} else if (bStampsprinted >= 1) {
			citationText = citationText + "REPL - INCORRECT TYPE OR NUMBER OF STAMP(S) GIVEN. -50pts\n";
			Clock.current.stampingErrors++; 
		} else if (sStampsprinted > 1) {
			citationText = citationText + "REPL - TOO MANY STORAGE STAMPS. -50pts\n";
			Clock.current.stampingErrors++; 
		} else {
			StateManager.current.points += 50f;
		}

		StateManager.current.points -= 50f; 
		if (bStamps + sStamps == 0) {
			citationText = citationText + "ORIG - MISSING BURN STAMP. -50pts\n";
			Clock.current.stampingErrors++; 
		} else if (sStamps >= 1) {
			citationText = citationText + "ORIG - INCORRECT STAMP(S) GIVEN. -50pts\n";
			Clock.current.stampingErrors++; 
		} else if (bStamps > 1) {
			citationText = citationText + "ORIG - TOO MANY BURN STAMPS. -50pts\n";
			Clock.current.stampingErrors++; 
		} else
			StateManager.current.points += 50f;
		if (citationText != "") {
			StateManager.current.GiveCitation (citationText); 
		}
		GameObject.Destroy (printedDocument.gameObject); 
		GameObject.Destroy (document.gameObject); 
		document = null; 
		printedDocument = null; 
		StateManager.current.document = null;
		StateManager.current.printedDocument = null;
	}
}
