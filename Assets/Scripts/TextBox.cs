using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBox : MonoBehaviour {

	public string text = "Wow this text is slowly appearing across the screen as if someone is actually saying it. That's cool. But how do they do that?"; 
	public Text textbox;
	float textDelay = 0.015f;
	public bool done = false; 
	public GameObject continueButton;

	public void Start()
	{
		textbox = GetComponentInChildren<Text> (); 

		StartCoroutine (SpellText ());
	}

	public void ClickContinue()
	{
		if (done) {
			DialogueManager.current.goAhead = true;
			done = false; 
			continueButton.SetActive (false); 
		}
	}

	public void Update()
	{
//		if (Input.GetMouseButtonDown(0)) {
//			if (done) {
//				DialogueManager.current.goAhead = true;
//				done = false; 
//				continueButton.SetActive (false); 
//			}
//		}
	}

	// Update is called once per frame
	public IEnumerator SpellText ()
	{ 
		done = false;
		continueButton.SetActive (false); 
		textbox.text = ""; 
		while (true) { 
			if (textbox.text.Length < text.Length) {
				textbox.text += text [textbox.text.Length]; 
				yield return new WaitForSeconds (textDelay); 
			} else {
				break; 
			}
		}
		continueButton.SetActive (true); 
		done = true; 
		
	}

}
