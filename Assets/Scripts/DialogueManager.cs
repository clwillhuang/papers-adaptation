using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class DialogueManager : MonoBehaviour {

	public GameObject Credits; 

	public bool goAhead = false; 
	public int index;
	public List<string> Day = new List<string> { 
		"Matheson: Good morning.",
		"You: Good morning, Matheson.",
		"Matheson: Did you enjoy the first chapter of Goldstein's manual?",
		"You: What?",
		"Matheson: Well, at the very least, your love letters from the Brotherhood say you did." }; 

	public TextBox text;
	public GameObject textBox; 
	public Animation animMath; 

	public static DialogueManager _current;
	public static DialogueManager current { 
		get {
			if (_current == null) {
				_current = FindObjectOfType<DialogueManager> (); 
			}
			return _current;
		}		
	}

	public void Start()
	{
		textBox.SetActive (false); 
	}

 	IEnumerator StartDay ()
	{
		if (Day.Count > 0) {
			index = 0; 
			textBox.SetActive (true); 
			while (true) {
				goAhead = false;
				text.text = Day [index];
				StartCoroutine (text.SpellText ()); 
				while (true) {
					yield return new WaitForSeconds (0.5f); 
					if (goAhead) {
						index++; 
						break; 
					}
				}
				if (index >= Day.Count) {
					break; 
				}
			}
			if (Clock.current.day == 6) {
				Instantiate (Clock.current.Day6Brotherhood, GameObject.Find ("Active").transform); 
				Clock.current.Day6Brotherhood.GetComponent<Animation> ().Play (); 
				StateManager.current.openedParcel = false; 
				yield return new WaitForSeconds (0.5f); 
			}
			if (Clock.current.MATHESON) {
				animMath.Play ("MathesonLeaving"); 
			}

			textBox.SetActive (false); 

			if (Clock.current.day == 10) {
				yield return new WaitForSeconds (1f); 
				StartCoroutine (EndGame ()); 
			}
		}
		textBox.SetActive (false); 
	}
	
	// Use this for initialization
	public void StartDay1 () {
		Day = new List<string> {
			"Matheson: Well greetings comrade! I am Mr. Matheson and I will be your Minitrue supervisor here at Recdep!",
			"You: Greetings.",
			"Matheson: I suppose you know what to do. You will receive selected items each day to correct their errors as part of your duty to Minitrue. Drag and drop items with your mouse.",
			"Matheson: Move your items to the right hand side for greater clarity. You will be using the red INGSOC standard manual to direct you. Read the FOREWORD and NEWSPEAK sections for explanations.",
			"Matheson: You will be removing violations in the documents according to the CENSORSHIP and DICTIONARY parts of your manual. Accompanying INSTRUCTIONAL MEMOS may give you hints.",
			"Matheson: Upon discovering a violation, click the sign in the lower right hand corner or press D to bring up DISCREPANCY MODE. Select the offending item and pair it with suitable REFERENCE materials in your manual.",
			"Matheson: Once a pair of selections result in a POSITIVE MATCH, press the C key as directed to save those changes to the censorship machine. When you believe all changes have been made, press PRINT REPLACEMENT located on the right side.",
			"Matheson: Open your stamps (located on the right side), and give the original document a BURN stamp to mark it for incineration. Apply a STORAGE stamp for the replacement article. ",
			"Matheson: Dispose of original article, replacement article and memo before clicking NEXT DOCUMENT to advance.", 
			"Matheson: As a supervisor, I will be monitoring all your actions through the telescreen behind me right now. You will receive Party Point deductions and CITATIONS for each protocol you break. Be alert.",
			"Matheson: Is that understood? Understanded?",
			"You: Yes.",
			"Matheson: Very well. Long live Big Brother!",
			"You: Long Live Big Brother!"
		};
		StartCoroutine (StartDay ()); 
	}

	public void StartDay2() { 
		Day = new List<string> {
			"Matheson: Good morning.",
			"You: Good morning, Comrade Matheson.",
			"Matheson: I came to see how you were doing. I hope your work is going well.", 
			"You: Yes, it's going quite well.",
			"Matheson: Good. It's nice to see people who recognize the importance of what goes on here.",
			"Matheson: You feel it, don't you?",
			"You: Feel what, comrade?",
			"Matheson: The gravity of the work that we do. We are the auditors of truth.",
			"Matheson: We erase every lie, every misprint, every tiny little mistake that could become poison in the veins of our society.",
			"Matheson: Nothing escapes us.",
			"Matheson: And when we are finished with purging all that is false, we replace it with truth. We rectify the fabrication and falsehood that seem to be omnipresent and immortal in this world, that penetrate our communities and suffuse into the minds of good Party members.",
			"Matheson: Here at the Ministry of Truth, we concern ourselves with purity, with ensuring that no one is tainted by vile fiction.",
			"You: I'm ashamed to say that I never thought of it that way, Comrade Matheson.",
			"Matheson: You would do well to keep it in mind then. The work we do at the Ministry of Truth does just as much to keep the people safe as the Thought Police themselves.",
			"Matheson: Our job is prevention, preemptively destroying the seed from which corruption sprouts; the job of the Thought Police is to clean up what we miss before it can do damage, to nip the evil in the bud, so to speak.",
			"You: I've never thought of this job as being so righteous.",
			"Matheson: There is nothing more righteous than to put yourself in the service of the Party, comrade; for the Party is rectitude itself. All virtue, all truth issues from the Party - it is because people forget this that thoughtcrime exists.",
			"Matheson: And it is our job keep that from happening.",
			"Matheson: Keep up the good work, comrade. Long Live Big Brother!",
			"You: Yes, Comrade Matheson. Long Live Big Brother!"
		};
		StartCoroutine(StartDay()); 
	}

	public void StartDay3() { 
		Day = new List<string> {
			"Matheson: Good morning.",
			"You: Good morning.",
			"Matheson: I see that you're hard at work. That's good.",
			"Matheson: Do you remember our talk from the other day?",
			"You: Of course, comrade.",
			"Matheson: There is something that I forgot to mention.",
			"Matheson: Comrade, why do you think thoughtcrime exists?",
			"You: Because people forget that 'all truth issues from party'?",
			"Matheson: Yes, that is why people lose themselves and commit thoughtcrime. But I am asking, why does it *exist*?",
			"You: I don't know comrade.",
			"Matheson: It exists because by nature, humans are imperfect.",
			"Matheson: When faced with perfection, with righteousness itself, an imperfect creature will feel threatened and afraid, for it cannot accept that which transcends its own concept of value; it will avert its eyes and desperately claim that there is no perfection, that such a thing could not possibly exist, to save its imperfect self from being destroyed by the recognizance of a perfect entity.",
			"Matheson: Sometimes, the creature may even believe its own pathetic lies.",
			"Matheson: This is cowardice.",
			"Matheson: You and I, we are not cowards: we know the absolute power and rectitude of the Party, we see its dazzling perfection, yet, instead of averting our eyes, we embrace it and devote ourselves to it.",
			"Matheson: Thus, when faced with perfection, such as of the Party, there are only three possibilities: serve it, which requires strength; deny it, which is delusion; or else be destroyed by it.",
			"You: So we who serve the Party are strong?",
			"Matheson: In serving the Party, you are strengthened; the Party invigorates all its followers, for we know we serve the ultimate cause. There can be no greater pursuit.",
			"You: And what of the weak?",
			"Matheson: If they will not admit to their delusion, they cannot be saved. However regrettable, we cannot allow the cowards to keep our society in shackles.",
			"Matheson: We cannot afford to be hindered by them in our endeavour towards the perfection that the Party has shown us.",
			"Matheson: But remember, comrade. Thoughtcrime exists because we humans are imperfect. Our tools and media are imperfect: we are not equipped to understand perfection.",
			"Matheson: This can be overcome, but not by those too weak or fragile to embrace it regardless. But every day we serve the Party's ends, we bring ourselves closer to its ideals. And now that we know perfection, we need not remain our flawed, imperfect selves forever.",
			"Matheson: We can cast aside our old, obsolete devices, and reform them in the image of the Party.",
			"Matheson: We need no longer have an imperfect language, and by consequence, we need no longer have an imperfect mind.",
			"You: So Newspeak will be perfect?",
			"Matheson: Of course!",
			"Matheson: It is shaped by the minds of the Party, governed by the doctrine of the Party, and at its heart are the ideals of the Party itself. It would be impossible for such a thing to be anything but utter perfection.",
			"Matheson: When the day comes that we all use Newspeak and nothing else, we will have achieved ascension.",
			"You: It would be impossible, then, to be weak?",
			"Matheson: Only as possible as it is for Big Brother himself to be weak.",
			"You: And all ideas, all thoughts would be those of the Party?",
			"Matheson: Any others could not exist. It would be impossible for an affront against the absolute truth of the Party to even be created.",
			"Matheson: The lies of the weak would be erased forever.",
			"You: ... ",
			"Matheson: What is the matter, comrade?",
			"You: ...It's nothing, comrade. I was just thinking of how glorious that day will be.",
			"Matheson: My greatest regret is that I likely won't live long enough to witness it. But keep the image of that day in your mind, comrade, and it will carry you through any challenge our yet flawed world can present you.",
			"You: Yes, Comrade Matheson. Thank you."
		};
		StartCoroutine(StartDay()); 
	}

	public void StartDay4() {
		Day = new List<string> (); 
		StartCoroutine(StartDay()); 
	}

	public void StartDay5()
	{
		Day = new List<string> { 
			"Matheson: Hello.",
			"You: Good morning, Matheson ... " 
		};
		StartCoroutine(StartDay()); 
	}

	public void StartDay6()
	{
		Day = new List<string> { 
			"Matheson: Good morning.",
			"You: Good morning, Matheson.",
			"Matheson: Did you enjoy the first chapter of Goldstein's manual?",
			"You: What?",
			"Matheson: Well, at the very least, your love letters from the Brotherhood say you did.",
			"You: It's not what it--",
			"Matheson: Who do you think has been giving you the orders? Don't worry, I'm on your side. I remember being young and restless like you once too. So full of life -- suffocated by the ever shrinking means of expression. We can change this.",
			"You: We ... ?",
			"Matheson: Me. You. The Brotherhood. I've brought you the next chapter of Goldstein's little book. I expect you to keep working at your job today, but from this moment forth as a Brotherhood operative. As you may have already noticed, we have started to enlighten you on the workings of the Party. Good luck."};
		StartCoroutine(StartDay()); 
	}

	public void StartDay7()
	{
		Day = new List<string> (); 
		StartCoroutine (StartDay ()); 
	}

	public void StartDay8()
	{
		Day = new List<string> { 
			"Matheson: Good morning.",
			"You: Good morning, Matheson.",
			"Matheson: Great work so far. I couldn't have done better myself. You'll rise up in the ranks quick, I suspect.",
			"You: Thank you.",
			"Matheson:  I’ve nothing to say today, I just came by to tell you that you will be receiving the fourth chapter of Goldstein’s manual soon. Same as before.",
			"You: The fourth? What happened to the third?",
			"Matheson: Patience. The third is reserved for veterans of The Brotherhood. Keep working hard and you’ll receive it - sooner rather than later, I suspect. For now, just read the fourth and continue on with business as usual. Good luck.",
			"You: Thank you."
		};  
		StartCoroutine (StartDay ()); 
	}

	public void StartDay9()
	{
		Day = new List<string> (); 
		StartCoroutine (StartDay ()); 
	}

	public void StartDay10()
	{
		Day = new List<string> { 
			"Matheson: Your desk has been cleared. On my decision, you are excused from this position for the time being.",
			"You: Thank you, Matheson, the days have been tough. I long for some rest.",
			"You: But as a worker at Minitrue, I’ll get straight to work as soon as I can.",
			"Matheson: There’s no need.",
			"Matheson: You’re going to be arrested in a few minutes when the police arrive.",
			"You: What?",
			"Matheson: I’ve called the Thought Police, and you are going to be arrested for your crimes against The Party and everything they stand for.",
			"You: You gave me access to all kinds of treasonous materials - how are you going to explain yourself?",
			"Matheson: Everything you received was created by The Party - your little Goldstein book is an Inner Party manual with a few words substituted to make it look like it was written by The Brotherhood.",
			"Matheson: I haven’t failed to notice your dissatisfaction with your job and life under The Party in the past few months. Everything you’ve experienced was an elaborate sting operation, designed to detect disloyalty and purge the Minitrue of any ungoodthinkers.",
			"Matheson: I’m sorry it had to be you, you were so interesting.",
			"You: What happened to your empathy? Weren’t you just like me once? Don’t you believe things could be any other way?",
			"Matheson: I didn’t lie about that. I was once unhappy, a nudge away from defection. Things changed when I read Chapter 3.",
			"You: ... ",
			"Matheson: Chapter 3 is about the ethics of Newspeak. You see, Newspeak means little to the Party itself, and most members of the Inner Party don’t care one way or the other.",
			"Matheson: They recognize life as it is as inevitable for mankind. The Proles are satisfied with their bread and circuses.",
			"Matheson: Newspeak was made for people just like you - weak-minded rebels who are crushed by freedom. There is no greater pleasure in life than being an efficient cog in a machine. Your needs will be met, and your time will be used productively - no moping around wondering if there’s a better way of living.",
			"Matheson: Before The Party, life was miserable. People everywhere were burdened by choice - an existence without an essence, driving people crazier and crazier.",
			"Matheson: People were throwing their lives away, and worse yet, humanity was on the brink of destruction - people were free to choose their ideology, and free to fight for their ideals.",
			"Matheson: Where did that leave us? With millions dead, and billions headed to the slaughter house.",
			"Matheson: The new way is better for us all - wouldn’t things be much better for you right now if you hadn’t been able to think those wretched thoughts?",
			"You: But -- ",
			"Matheson: It’s too late for you now. Maybe one day you’ll learn to love Big Brother.",
			"Matheson: Glory to The Party. Long Live Big Brother."
		};
		StartCoroutine (StartDay ()); 
	}

	public IEnumerator EndGame()
	{
		Clock.current.StopAllCoroutines ();
		text.StopAllCoroutines (); 
		StateManager.current.StopAllCoroutines (); 
		CitationChecker.current.StopAllCoroutines (); 
		Clock.current.ShowHelp (false, false); 
		Credits.SetActive (true); 
		Credits.transform.GetComponent<Animation> ().Play ("FadeIn"); 
		Credits.transform.GetChild (0).GetComponent<Animation> ().Play ("CreditLogo"); 
		yield return new WaitForSeconds (0.5f); 
		Credits.transform.GetChild (1).GetComponent<Animation> ().Play ("Credits"); 
	}

}
