using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Document/Sheet", order = 1)]
public class ScriptableDocument : ScriptableObject {

	[TextArea(15, 20)]  
	public string message;
	[TextArea(2, 10)]
	public List<string> replacedSnippets; 
	public string title; 
	public string alternateTitle; 
	public string memo; 
	public bool frombrotherhood; 
}
