using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerOrdering : MonoBehaviour {

	public List < GameObject > ActiveItems; 

	public void ForceUpdateLayers()
	{
		// Normally reordering should only be done when the level is being reinstantiated
		if (ActiveItems.Count != transform.childCount) {
			ActiveItems.Clear (); 
			foreach (Transform child in transform) {
				ActiveItems.Add (child.gameObject); 
			}
		}
	}

	public void MoveItemToTop(GameObject selected)
	{
		selected.transform.SetSiblingIndex (0); 
	}

}
