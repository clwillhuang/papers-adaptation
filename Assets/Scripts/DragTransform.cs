using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
 
public class DragTransform : MonoBehaviour
{
    private bool dragging = false;
    private float distance;
	public Vector3 original = Vector3.zero; 
	public Vector3 originalLoc = Vector3.zero; 
	public float top, left, right; 

	public static Transform leftParent, rightParent, leftTopParent; 

	public Sprite leftRender, rightRender;
	public Color leftColor;
	public Color rightColor; 
	public bool onRightSide; 

	public int storageStamps, burntStamps;

	public bool canBeDestroyed = false; 

	public void Start ()
	{
		top = 521f; 
		left = 1024f - 704f; 
		right = 1024f; 
		storageStamps = 0;
		burntStamps = 0; 

		if (Camera.main.WorldToScreenPoint (transform.position).x >= 320f) {
			onRightSide = true;
			transform.GetComponent<RectTransform> ().localScale = new Vector3 (1f, 1f, 1f); 
		} else {
			onRightSide = false; 
			transform.GetComponent<RectTransform> ().localScale = new Vector3 (0.5f, 0.5f, 0.5f); 
		}

		if (leftParent == null) {
			leftParent = GameObject.Find ("BottomLeft").transform; 
		}
		if (rightParent == null) {
			rightParent = GameObject.Find ("Active").transform; 
		}
		if (leftTopParent == null) {
			leftTopParent = GameObject.Find ("Output").transform; 
		}
	}

    public void OnMouseDown()
    {
		if (StateManager.current.inCorrectionMode) {
			return; 
		}
        dragging = true;
		original = Input.mousePosition;
		originalLoc = transform.GetComponent<RectTransform> ().localPosition; 

		transform.SetAsLastSibling (); 
		StateManager.current.ReOrderLayers (); 

    }
 
	public void OnMouseUp ()
	{
		if (StateManager.current.inCorrectionMode) {
			return; 
		}
		if (Input.mousePosition.x < left && Input.mousePosition.x >= 0f && Input.mousePosition.y <= 521f && Input.mousePosition.y >= 235f) {
			if (canBeDestroyed) {
				Debug.Log ("Destroying item."); 
				StateManager.current.DestroyDocument (this.gameObject); 
			} else {
				Debug.Log ("Item not destroyable."); 
				transform.SetParent (leftParent); 
				transform.localPosition = Vector3.zero;
			}
		}
        dragging = false;
		//Debug.Log ("Stopping dragging at position " + transform.position + " mouse " + Input.mousePosition.x + " " + Input.mousePosition.y);
		//Debug.Log (Camera.main.WorldToScreenPoint (Input.mousePosition)); 
		StateManager.current.ReOrderLayers (); 
    }

    // Raycast Checker
	public void Update ()
	{
//		if (Input.GetMouseButtonDown (0)) {
//			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
//			RaycastHit hit;
//			if (Physics.Raycast (ray, out hit)) {
//				Debug.Log ("Name = " + hit.collider.name);
//				Debug.Log ("Tag = " + hit.collider.tag);
//				Debug.Log ("Hit Point = " + hit.point);
//				Debug.Log ("Object position = " + hit.collider.gameObject.transform.position);
//				Debug.Log ("--------------");
//			} else { 
//				Debug.Log ("None");
//			}  
//		}
		if (dragging) {
			if (Input.mousePosition.x < left && Input.mousePosition.x >= 0f && Input.mousePosition.y <= 521f && Input.mousePosition.y >= 0f) {
				// GO INTO SMALL DOC MODE
				if (onRightSide) {
					// Debug.Log ("Going to left side!"); 
					if (Input.mousePosition.y >= 235f) {
						transform.SetParent (leftTopParent); 
					} else {
						transform.SetParent (leftParent); 
					}
					transform.GetComponent<RectTransform> ().localScale = new Vector3 (0.5f, 0.5f, 0.5f); 
					original = Input.mousePosition;
					originalLoc = transform.GetComponent<RectTransform> ().localPosition; 
					if (leftRender != null) {
						transform.GetComponent<Image> ().sprite = leftRender; 
						transform.GetComponent<Image> ().color = leftColor; 
					}
					onRightSide = false; 
//					foreach (Transform child in transform) {
//						child.gameObject.SetActive (false); 
//					}
				}
				if (!onRightSide && transform.parent.transform != leftTopParent && Input.mousePosition.y >= 235f) {
					transform.SetParent (leftTopParent); 
					original = Input.mousePosition;
					originalLoc = transform.GetComponent<RectTransform> ().localPosition; 
				}
			}
			else if (!(Input.mousePosition.x >= left && Input.mousePosition.x <= right && Input.mousePosition.y <= top && Input.mousePosition.y >= 0f)) {
				OnMouseUp (); 
				// Debug.Log ("Stopped!"); 
				return;
			} else {
				if (!onRightSide && Input.mousePosition.x >= left) {
					// Debug.Log ("Going to right side!"); 
					transform.SetParent (rightParent); 
					transform.GetComponent<RectTransform> ().localScale = new Vector3 (1f, 1f, 1f); 
					original = Input.mousePosition;
					originalLoc = transform.GetComponent<RectTransform> ().localPosition; 
					if (rightRender != null) {
						transform.GetComponent<Image> ().sprite = rightRender;
						transform.GetComponent<Image> ().color = rightColor;  
					}
					onRightSide = true;
//					foreach (Transform child in transform) {
//						child.gameObject.SetActive (true); 
//					}
					if (transform.GetComponent<Manual>() != null) {
						transform.GetComponent<Manual> ().RefreshPages (); 
					}
				} 
			}

			transform.GetComponent<RectTransform> ().localPosition = 
				new Vector3 (originalLoc.x + (Input.mousePosition.x - original.x),
							 originalLoc.y + (Input.mousePosition.y - original.y), transform.GetComponent<RectTransform>().localPosition.z); 
         }
    }
}
