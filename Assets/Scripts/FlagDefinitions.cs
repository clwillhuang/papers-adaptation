using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FlagsDef {

	/// All entries in the newspeak dictionary. Code, d1, d2, d3
	/// All entries in the censorship manual. Code, c1, c2, c3 
	/// All entries in the special sheet denoting vaporized persons. Code, v1, v2, v3
	/// All entries from the daily random newspaper events. Code, n1, n2, n3
	/// All entries from Brotherhod sources. Code, b1, b2, b3
	/// Other entries given special codes.

	/// <summary>
	/// All entries in the newspeak dictionary. Code, d1, d2, d3
	/// </summary>
	[System.Serializable]
	public enum Violation { d1, d2, d3, d4, d5, d6, d7, d8, d9, da, db, dc, c1, c2, c3, c4 } ;

	public static List<string> violationArray = new List<string>() {"d1", "d2", "d3", "d4", "d5", "d6", "d7", "d8", "d9", "da", "db", "dc", "c1", "c2", "c3", "c4"}; 

	public static List<string > violationArrayInString = new List<string> () {
		"DICTIONARY RULE 1",
		"DICTIONARY RULE 2",
		"DICTIONARY RULE 3",
		"DICTIONARY RULE 4",
		"DICTIONARY RULE 5",
		"DICTIONARY RULE 6",
		"DICTIONARY RULE 7",
		"DICTIONARY RULE 8",
		"DICTIONARY RULE 9",
		"DICTIONARY RULE 10",
		"DICTIONARY RULE 11",
		"DICTIONARY RULE 12",
		"CENSOR RULE 1 USE OF OLDSPEAK",
		"CENSOR RULE 2 MALQUOTATION",
		"CENSOR RULE 3 REF TO UNPERSONS",
		"CENSOR RULE 4 MALSTATISTICS"
	}; 

	public static Violation getViolation(string code)
	{
		code = code.Replace(code[0], char.ToLower(code[0])); 
		code = code.Replace (code [1], char.ToLower (code [1])); 
		Debug.Log ("Index: " + violationArray.IndexOf (code) + " " + code); 
		return (Violation)(violationArray.IndexOf (code));
	}
}
