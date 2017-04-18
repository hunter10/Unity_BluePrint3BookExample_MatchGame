using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour {

	public int numberOfTitles = 16;
	RaycastHit hit;
	bool canClick = true;

	GameObject matchOne;
	GameObject matchTwo;

	string tileName1;
	string[] tName1 = new string[]{};

	string tileName2;
	string[] tName2 = new string[]{};

	public GameObject[] tileObjects;
	Vector3[] tileLocations = { new Vector3(0f, 0f, 0f), 	new Vector3(1.5f, 0f, 0f), 	new Vector3(3f, 0, 0), 		new Vector3(4.5f, 0, 0),
								new Vector3(0, 1.5f, 0), 	new Vector3(1.5f, 1.5f, 0), new Vector3(3f, 1.5f, 0), 	new Vector3(4.5f, 1.5f, 0),
								new Vector3(0, 3f, 0), 		new Vector3(1.5f, 3f, 0), 	new Vector3(3f, 3f, 0), 	new Vector3(4.5f, 3f, 0),
								new Vector3(0, 4.5f, 0), 	new Vector3(1.5f, 4.5f, 0), new Vector3(3f, 4.5f, 0), 	new Vector3(4.5f, 4.5f, 0),
							  };

	int scoreInt = 0;
	string scoreTxt = "";

	float startTime;
	int Seconds;
	int roundedSeconds;
	int txtSeconds;
	int txtMinutes;
	int countSeconds;
	bool stopTimer;

	public GUISkin egyptSkin;
	public Texture2D finishedTxture;
	public Texture2D timeUpTxture;
	bool finished = false;
	bool timeUp = false;

	void Awake(){
		startTime = 30f;
		countSeconds = 0;
		roundedSeconds = 0;
		txtSeconds = 0;
		txtMinutes = 0;
		countSeconds = 0;
		stopTimer = false;
	}

	// Use this for initialization
	void Start () {
		Camera.main.transform.position = new Vector3 (2.25f, 2.25f, -8f);
		for (var i = 0; i < numberOfTitles; i++) {
			Instantiate (tileObjects [i], tileLocations [i], Quaternion.identity);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (canClick == false)
			return;

		if (Input.GetButtonDown ("Fire1")) {

			var ray1 = Camera.main.ScreenPointToRay (Input.mousePosition);

			if (Physics.Raycast (ray1, out hit, Mathf.Infinity)) {
				
				if (!matchOne) {
					revealCardOne ();
				} else {
					//revealCardTwo ();
					StartCoroutine(revealCardTwo());
				}
			}
		}
	}

	void OnGUI()
	{
		GUI.skin = egyptSkin;
		scoreTxt = scoreInt.ToString ();
		GUI.Label (new Rect (10, 10, 100, 20), scoreTxt);

		if (stopTimer == false) {
			float guiTime = Time.time - startTime;
			//print ("Time.time:" + Time.time + ", startTime:" + startTime + ", guiTime:" + guiTime);
			Seconds = countSeconds - (int)(guiTime);
		}

		if (Seconds == 0) {
			print ("The time is over");
			stopTimer = true;
			timeUp = true;
		}

		roundedSeconds = Mathf.CeilToInt (Seconds);
		txtSeconds = roundedSeconds % 60;
		txtMinutes = roundedSeconds / 60;

		string text = string.Format ("{0:00}:{1:00}", txtMinutes, txtSeconds);
		GUI.Label (new Rect (10, 30, 100, 30), text);

		if (finished == true) {
			GUI.Label (new Rect (270, 305, 512, 256), finishedTxture);
		} 

		if (timeUp == true) {
			GUI.Label (new Rect (270, 305, 512, 256), timeUpTxture);
		}
	}

	void revealCardOne()
	{
		matchOne = hit.transform.gameObject;
		tileName1 = matchOne.transform.parent.name;

		//print ("the value of matchOne is -" + matchOne);
		//print ("the value of titleName1 = " + tileName1);

		if (matchOne == null) {
			print ("No object found");
		} else {
			tName1 = tileName1.Split('_');
			//print("tileName1[0] = " + tName1[0]);
			//matchOne.transform.Rotate (new Vector3 (0, 180f, 0));
			matchOne.transform.parent.GetComponent<Animation>().Play("tileReveal");
		}
	}

	IEnumerator revealCardTwo()
	{
		matchTwo = hit.transform.gameObject;
		tileName2 = matchTwo.transform.parent.name;

		//print ("the value of matchTwo is -" + matchTwo);
		//print ("the value of titleName2 = " + tileName2);

		if (tileName1 == tileName2) {
			//print ("이름이 같네");
			yield break;
		} else {
			//print ("different name");
		}

		if (matchTwo == null) {
			print ("No object found");
		} else {
			tName2 = tileName2.Split('_');
			//print("tileName2[0] = " + tName2[0]);
			//matchTwo.transform.Rotate (new Vector3 (0, 180f, 0));
			matchTwo.transform.parent.GetComponent<Animation>().Play("tileReveal");
		}

		if (tName1 [0] == tName2 [0]) {
			scoreInt++;
			canClick = false;
			yield return new WaitForSeconds (2);
			Destroy (matchOne);
			Destroy (matchTwo);
			canClick = true;
			numberOfTitles = numberOfTitles - 2;

			if (numberOfTitles == 0) {
				print ("End game");
				stopTimer = true;
				finished = true;
			}
		} else {
			canClick = false;
			yield return new WaitForSeconds (2);
			//matchOne.transform.Rotate (new Vector3 (0, -180, 0));
			//matchTwo.transform.Rotate (new Vector3 (0, -180, 0));
			matchOne.transform.parent.GetComponent<Animation>().Play("tileHide");
			matchTwo.transform.parent.GetComponent<Animation>().Play("tileHide");
			canClick = true;
		}

		matchOne = null;
		matchTwo = null;

		yield return null;
	}
}
