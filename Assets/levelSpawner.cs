using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelSpawner : MonoBehaviour {

	public GameObject player;
	public GameObject menuHolder;

	bool inMenu = true;

	public float levelChunkWidth;	//level Chunks MUST be at least wider than the camera viewport
	public GameObject lastChunk;
	int chunkPicker = 0;
	int lastChunkNum = 0;

	public GameObject[] chunks = new GameObject[8]; //Number of discrete chunks
	GameObject spawnedChunk;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		inMenu = menuHolder.GetComponent<menuController> ().inMenu;

		if (!inMenu) {
			this.transform.position += player.transform.position + Vector3.right * levelChunkWidth;

//**********spawn new level chunks
			if (this.transform.position.x > (lastChunk.transform.position.x + levelChunkWidth)) {
				lastChunkNum = chunkPicker;
				chunkPicker = (int)Random.Range (0, chunks.Length - 1);

				if (chunkPicker == lastChunkNum) {
					chunkPicker = (int)Random.Range (0, chunks.Length - 1);	//half chance to get identical chunks adjacent
				}
					
				spawnedChunk = Instantiate (chunks [chunkPicker], this.transform.position, Quaternion.identity) as GameObject;

			}
		}
	}
}
