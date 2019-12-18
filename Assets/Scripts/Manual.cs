using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Manual : MonoBehaviour {

	public int pageNumber, dictPageNumber, censorPageNumber, forewordPageNumber, newspeakForeward; 
	public List<GameObject>[] contents;
	public GameObject nextPageButton, lastPageButton; 
	public Text leftTitle, rightTitle; 
	public Text pgnor, pgnol; 

	public void Start()
	{
		contents = new List<GameObject>[20];
		for (int i = 0; i < 20; i++) {
			contents [i] = new List<GameObject> (); 
		}
		RefreshPages (); 
	} 

	public void RefreshPages ()
	{
		Debug.Log (transform.childCount); 
		for (int i = 0; i < transform.childCount; i++) {
			transform.GetChild (i).gameObject.SetActive (true); 
		}
		for (int i = 0; i < transform.childCount; i++) {
			Transform child = transform.GetChild(i); 
			Debug.Log (child == null); 
			if (child.name == "Last Page" || child.name == "Next Page") {
				continue;
			}
			ManualPage mp = child.GetComponent<ManualPage> (); 
			if (mp == null) {
				Debug.Log ("Unable to find script ManualPage on " + child.name); 
				continue;
			} 
			contents [mp.page].Add (child.gameObject);
			child.gameObject.SetActive (mp.page == pageNumber); 
		}
		pgnol.text = (pageNumber * 2 + 1).ToString (); 
		pgnor.text = (pageNumber * 2 + 2).ToString (); 
	} 

	public void NextPage()
	{
		Debug.Log ("Page turned +!"); 
		HideCurrentPages ();
		pageNumber = Mathf.Min(pageNumber+1, 14); 
		ShowCurrentPages ();
	}

	public void LastPage()
	{
		Debug.Log ("Page turned -!"); 
		HideCurrentPages ();
		pageNumber = Mathf.Max(pageNumber-1, 0);
		ShowCurrentPages ();
	} 

	public void ShowCurrentPages ()
	{
		foreach (var item in contents[pageNumber]) {
			if (item.name == "Last Page" || item.name == "Next Page") {
				continue;
			}
			if (item.GetComponent<ManualPage> ().isActiveInGame) {
				item.SetActive (true); 
			}
		}
		if (pageNumber >= dictPageNumber) {
			leftTitle.text = "DICTIONARY";
			rightTitle.text = "DICTIONARY"; 
		} else if (pageNumber >= newspeakForeward) {
			leftTitle.text = "NEWSPEAK";
			rightTitle.text = "NEWSPEAK";
		} else if (pageNumber >= censorPageNumber) {
			leftTitle.text = "CENSORSHIP";
			rightTitle.text = "CENSORSHIP";
		} else { 
			leftTitle.text = "FOREWORD";
			rightTitle.text = "FOREWORD";
		} 

		pgnol.text = (pageNumber * 2 + 1).ToString (); 
		pgnor.text = (pageNumber * 2 + 2).ToString (); 
	}

	public void HideCurrentPages()
	{
		foreach (var item in contents[pageNumber]) {
			if (item.name == "Last Page" || item.name == "Next Page") {
				continue;
			}
			item.SetActive (false); 
		}
	}

	public void TurnToDictionary()
	{
		HideCurrentPages ();
		pageNumber = dictPageNumber; 
		ShowCurrentPages (); 
	}

	public void TurnToCensor()
	{
		HideCurrentPages ();
		pageNumber = censorPageNumber; 
		ShowCurrentPages (); 
	} 

	public void TurnToForeword()
	{
		HideCurrentPages ();
		pageNumber = forewordPageNumber; 
		ShowCurrentPages (); 
	} 

	public void TurnToNewspeak()
	{
		HideCurrentPages ();
		pageNumber = newspeakForeward; 
		ShowCurrentPages (); 
	} 
}
