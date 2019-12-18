using System.Collections;
using UnityEngine;
using UnityEngine.UI; 

public class MatchText : MonoBehaviour {

	Text text; 
	
	void OnEnable()
	{
		StartCoroutine (textFlash ()); 
		text = GetComponent<Text> (); 
	}

	IEnumerator textFlash()
	{
		yield return new WaitForSeconds (0.3f); 
		text.enabled = false;
		yield return new WaitForSeconds (0.3f);
		text.enabled = true;
		yield return new WaitForSeconds (0.3f);
		text.enabled = false;
		yield return new WaitForSeconds (0.3f);
		text.enabled = true;
	} 

}
