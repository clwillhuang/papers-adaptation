using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Parcel : MonoBehaviour {

	public Text ins; 
	public GameObject contents;
	public GameObject thememo; 
	public bool makeMemo;

	void Start ()
	{
		if (makeMemo) {
			thememo = Instantiate (StateManager.current.memoPrefab); 
			thememo.transform.Find ("Memo Text").GetComponent<Text> ().text = "FROM BROTHERHOOD. URGENT. OPEN IMMEDIATELY. PRESS O TO OPEN. DOWN WITH BIG BROTHER."; 
			StateManager.current.GiveAnimation (thememo); 
			thememo.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
			transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
		}
		StateManager.current.openedParcel = false; 
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.O)) {
			ins.enabled = false; 
			GetComponent<Image> ().enabled = false; 
			contents.SetActive (true); 
			StateManager.current.openedParcel = true; 
			transform.GetComponent<DragTransform> ().canBeDestroyed = true; 
		}
	}
}
