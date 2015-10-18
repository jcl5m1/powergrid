using UnityEngine;
using System.Collections;

public class ChangeBid : MonoBehaviour {


	public int delta;

	private PowerPlantShop shop;

	// Use this for initialization
	void Start () {
		shop = GameObject.Find ("PowerPlantShop").GetComponent<PowerPlantShop> ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void OnMouseOver() {
		if (Input.GetMouseButtonDown (0)) {
			shop.currentBid += delta;
		}
	}
}
