using UnityEngine;
using System.Collections;

public class AudioTestScript : MonoBehaviour {

	//public AudioSource audioInto;
	public AudioSource audioGuitarChrous;
	public AudioSource audioGuitarSolo;

	//public AudioSource audioImpact;


	public bool isSFXRunning;
	// Use this for initialization
	void Start () {
		//float startMusicDelay = audioInto.clip.length;
		//audio.PlayOneShot(audioImpact, 0.7F);
		//StartCoroutine(startMainMusic(startMusicDelay));
		startMainMusic ();
		//isSFXRunning = false;
	}
	
	// Update is called once per frame


	 void Update () {

		/*
		float randPrecent = Random.Range(0.0F, 1.0F);
		if(randPrecent > 0.2f){
			if(isSFXRunning == false){
				float randTime = Random.Range(5.0f,12.0f);
				StartCoroutine(activeSFX(randTime));
			}
		} */

		startMainMusic ();
	}


	/*

	IEnumerator activeSFX(float waitTime) 
	{
		isSFXRunning = true;

		yield return new WaitForSeconds(waitTime);
			audioImpact.volume = 1.0f;
		float randTime = Random.Range(1.0f,2.0f);
		yield return new WaitForSeconds(randTime);
			audioImpact.volume = 0.0f;
			isSFXRunning = false;
	} */



	//IEnumerator startMainMusic(float waitTime) {

		void startMainMusic() {
		//yield return new WaitForSeconds(waitTime);
		//print("startMainMusic " + Time.time);
		//audioGuitarChrous.volume = 1.0f;
		audioGuitarSolo.volume = 1.0f;
		audioGuitarChrous.volume= 1.0f;
	}
}
