using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class musicControl : MonoBehaviour {

	public AudioSource BGM;

	public AudioClip AnotherPart;
	public AudioClip Bad;
	public AudioClip BeatIt;
	public AudioClip BillieJean;
	public AudioClip SmoothCriminal;

	int i = -1;
	bool playNow = false;

	public Text currentlyPlaying;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		musicSelection ();
		currentSongUI ();

		if (i > 4) {
			i = 0;
		}
	}

	void musicSelection ()	{
		if (Input.GetKeyDown (KeyCode.M) || !BGM.isPlaying) {
			playNow = true;
			i++;
		}

		if (i == 0)	{
			BGM.clip = AnotherPart;
		}
		if (i == 1)	{
			BGM.clip = Bad;
		}
		if (i == 2)	{
			BGM.clip = BeatIt;
		}
		if (i == 3)	{
			BGM.clip = BillieJean;
		}
		if (i == 4)	{
			BGM.clip = SmoothCriminal;
		}

		if (playNow) {
			BGM.Play ();
			playNow = false;
		}
	}

	void currentSongUI	()	{
		if (BGM.clip == AnotherPart) {
			currentlyPlaying.text = " Another Part of Me\n(1987)";
		}
		if (BGM.clip == Bad) {
			currentlyPlaying.text = " Bad (1987)";
		}
		if (BGM.clip == BeatIt) {
			currentlyPlaying.text = " Beat It (1982)";
		}
		if (BGM.clip == BillieJean) {
			currentlyPlaying.text = " Billie Jean (1982)";
		}
		if (BGM.clip == SmoothCriminal) {
			currentlyPlaying.text = " Smooth Criminal\n(1988)";
		}
	}

}
