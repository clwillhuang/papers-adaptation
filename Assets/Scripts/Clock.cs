using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Clock : MonoBehaviour {

	public Sprite[] numberGO;
	public int hour; 
	public int minute; 
	public int day = 0; 
	public float secondsBetweenTicks = 2.5f; 
	public bool isPaused = false; 
	public bool onEndScreen = false; 
	public bool MATHESON = false; 
	public Animation animMath; 

	public GameObject Day6Brotherhood; 

	public GameObject help1, help2; 

	public GameObject DayCover;
	public GameObject nextButton; 
	public Text date; 
	private string sameDate = "April 31st, 1984";
	public bool canPressSpace = false; 

	public int errorsCorrected, errorsMissed, errorsTotal, stampingErrors; 

	public Image f1, f2, f3, f4; 

	public static Clock _current;
	public static Clock current { 
		get {
			if (_current == null) {
				_current = FindObjectOfType<Clock> (); 
			}
			return _current;
		}		
	}

	void Start()
	{
		ShowHelp (false, false); 
		StartCoroutine (ClockTime()); 
		StartDay (); 
	}

	void Update()
	{
		if (onEndScreen && canPressSpace) {
			if (Input.GetKeyDown (KeyCode.Space)) {
				onEndScreen = false;
				DayCover.SetActive (false);
				date.enabled = false; 
				StartDay ();
				nextButton.SetActive (true);
				isPaused = false; 
				canPressSpace = false; 
			}
		}
	}

	public IEnumerator ClockTime ()
	{
		while (true) { 
			while (isPaused) {
				yield return new WaitForSeconds (0.25f); 
			}
			minute++;
			if (minute >= 60) {
				hour++;
				minute = 0; 
				if (hour == 24) {
					hour = 0; 
				}
			}

			int hour1 = (int)(hour / 10);
			if (hour1 > 0) {
				f1.enabled = true;
				f1.sprite = numberGO [hour1];
			} else {
				f1.enabled = false; 
			}
			f2.sprite = numberGO [hour % 10]; 
			f3.sprite = numberGO [(int)(minute / 10)]; 
			f4.sprite = numberGO [minute % 10]; 

			yield return new WaitForSeconds (secondsBetweenTicks); 
		
		}
	} 

	public IEnumerator SpellDay ()
	{
		isPaused = true; 
		DayCover.SetActive (true);
		DayCover.GetComponent<Image> ().color = new Color (0, 0, 0, 245); 
		date.enabled = true; 
		hour = 8;
		minute = 0; 
		f1.enabled = false;
		f2.sprite = numberGO [8]; 
		f3.sprite = numberGO [0]; 
		f4.sprite = numberGO [0]; 
		date.text = ""; 
		date.color = Color.red; 
		animMath.gameObject.SetActive (true);
		animMath.Play ("Stationary"); 
		animMath.gameObject.SetActive (false); 
		DialogueManager.current.textBox.SetActive (false);
		for (int i = 0; i < sameDate.Length; i++) {
			date.text = date.text + sameDate [i]; 
			yield return new WaitForSeconds (0.2f); 
		}
		string dayapp = "\nDay ... " + day.ToString() + "?";
		for (int i = 0; i < dayapp.Length; i++) {
			date.text = date.text + dayapp [i]; 
			yield return new WaitForSeconds (0.2f); 
		}
		yield return new WaitForSeconds (1f); 
		DayCover.GetComponent<Animation> ().Play ("Fade"); 
		isPaused = false; 
		date.enabled = false; 
		yield return new WaitForSeconds (2f);
		DayCover.SetActive (false);
		if (day != 10) {
			StateManager.current.nextDocumentRef = (day - 1) * 5; 
			StateManager.current.nextDocument = StateManager.current.scriptableDocuments [(day - 1) * 5]; 
			StateManager.current.ShowTable (); 
		}
		if (day == 1) {
			MATHESON = true; 
			Telescreen.current.Turn (true); 
			animMath.gameObject.SetActive (MATHESON);
			animMath.Play ("MathesonWalking"); 
			yield return new WaitForSeconds (1.5f); 
			DialogueManager.current.StartDay1 (); 
		}
		else if (day == 2) {
			MATHESON = true; 
			Telescreen.current.Turn (true); 
			animMath.gameObject.SetActive (MATHESON);
			animMath.Play ("MathesonWalking"); 
			yield return new WaitForSeconds (1.5f); 
			DialogueManager.current.StartDay2 (); 
		}
		else if (day == 3) {
			MATHESON = true; 
			Telescreen.current.Turn (true); 
			animMath.gameObject.SetActive (MATHESON);
			animMath.Play ("MathesonWalking"); 
			yield return new WaitForSeconds (1.5f); 
			DialogueManager.current.StartDay3 (); 
		}
		else if (day == 4) {
			MATHESON = false; 
			Telescreen.current.Turn (false); 
			animMath.gameObject.SetActive (MATHESON);
			DialogueManager.current.StartDay4 (); 
		}
		else if (day == 5) {
			MATHESON = true; 
			animMath.gameObject.SetActive (MATHESON);
			animMath.Play ("MathesonWalking"); 
			yield return new WaitForSeconds (1.5f); 
			Telescreen.current.Turn (true); 
			DialogueManager.current.StartDay5 (); 
		}
		else if (day == 6) {
			MATHESON = true; 
			animMath.gameObject.SetActive (MATHESON);
			animMath.Play ("MathesonWalking"); 
			yield return new WaitForSeconds (1.5f); 
			Telescreen.current.Turn (false); 
			DialogueManager.current.StartDay6 (); 
		}
		else if (day == 7) {
			MATHESON = false; 
			animMath.gameObject.SetActive (MATHESON);
			Telescreen.current.Turn (true); 
			DialogueManager.current.StartDay7 (); 
		}
		else if (day == 8) {
			MATHESON = true; 
			animMath.gameObject.SetActive (MATHESON); 
			animMath.Play ("MathesonWalking"); 
			yield return new WaitForSeconds (1.5f); 
			Telescreen.current.Turn (false); 
			DialogueManager.current.StartDay8 (); 
		}
		else if (day == 9) {
			MATHESON = false; 
			animMath.gameObject.SetActive (MATHESON);
			Telescreen.current.Turn (true); 
			DialogueManager.current.StartDay9 (); 
		}
		else if (day == 10) {
			StateManager.current.ClearOffTable (true); 
			MATHESON = true; 
			animMath.gameObject.SetActive (MATHESON);
			animMath.Play ("MathesonWalking"); 
			yield return new WaitForSeconds (1.5f); 
			Telescreen.current.Turn (false); 
			DialogueManager.current.StartDay10 (); 
		}
		nextButton.SetActive (true); 
	}

	public void StartDay()
	{
		day++;
		errorsTotal = 0;
		errorsMissed = 0;
		errorsCorrected = 0;
		stampingErrors = 0; 
		StateManager.current.doneDocuments = -1; 
		StopCoroutine ("SpellDay"); 
		StartCoroutine(SpellDay ()); 
		ShowHelp (false, false); 
	}

	public void EndDay()
	{
		ShowHelp (false, false); 
		canPressSpace = false; 
		onEndScreen = true;
		nextButton.SetActive (false); 
		DayCover.SetActive (true);
		DayCover.GetComponent<Image> ().color = new Color (0, 0, 0, 245);
		date.enabled = true;
		date.color = Color.white;
		isPaused = true; 

		string dayend = "END OF DAY " + day.ToString () + "\n" +
		                "CORRECTED ERRORS: " + errorsCorrected.ToString () + "\n" +
		                "ERRORS MISSED: " + errorsMissed.ToString () + "\n" +
		                "STAMPING/OTHER ERRORS: " + stampingErrors.ToString () + "\n\n\n" +
		                "YOUR GRADE: "; 
		
		float grade = errorsCorrected / (stampingErrors + errorsMissed + errorsCorrected);
		if (grade >= 90f) {
			dayend += "DOUBLEPLUSGOOD";
		} else if (grade >= 80f) {
			dayend += "PLUSGOOD"; 
		} else if (grade >= 60f) {
			dayend += "AVERAGE";
		} else if (grade >= 50f) { 
			dayend += "SUFFICIENT";
		} else {
			dayend += "UNSATISFACTORY";
		}
		dayend += "\n PRESS SPACE TO START NEXT DAY \n"; 
		StartCoroutine (SpellEndDayText (dayend)); 
		StateManager.current.ClearOffTable (false); 
	}

	IEnumerator SpellEndDayText(string d)
	{
		date.text = ""; 
		for (int i = 0; i < d.Length; i++) {
			date.text += d [i]; 
			yield return new WaitForSeconds (0.1f);
			if (d[i] == ':') {
				yield return new WaitForSeconds (0.3f);
			}
		}
		canPressSpace = true; 
	}

	public void SkipToDay(int nextDay)
	{
		ShowHelp (false, false); 
		day = nextDay - 1; 
		onEndScreen = false;
		DayCover.SetActive (false);
		date.enabled = false; 
		canPressSpace = false;
		DialogueManager.current.textBox.SetActive (false); 
		StateManager.current.ClearOffTable (false);
		Clock.current.StopCoroutine ("SpellDay"); 
		Clock.current.StopCoroutine ("SpellEndDayText"); 
		StopCoroutine ("SpellDay"); 
		StopCoroutine ("SpellEndDayText"); 
		DialogueManager.current.StopAllCoroutines (); 
		StateManager.current.StopAllCoroutines (); 
		DialogueManager.current.text.StopAllCoroutines (); 
		DialogueManager.current.textBox.SetActive (false); 
		StartDay ();
		nextButton.SetActive (true);
		isPaused = false; 
		canPressSpace = false; 
	} 

	public void ShowHelp(bool show1, bool show2)
	{
		help1.gameObject.SetActive (show1);
		help2.gameObject.SetActive (show2); 
	}

	public void ShowHelp1(bool show1)
	{
		help1.gameObject.SetActive (show1);
		help2.gameObject.SetActive (false); 
	}

	public void ShowHelp2(bool show2)
	{
		help1.gameObject.SetActive (false);
		help2.gameObject.SetActive (show2); 
	}
}
