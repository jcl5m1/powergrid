using UnityEngine;
using System.Collections;

public class DiscardPowerPlant : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnMouseOver() {
		if (GameState.instance.CurrentState == GameState.State.BuyPlants) {
			if (Input.GetMouseButtonDown (0)) {
				GameState.instance.PowerplantShop.DiscardSelectedPowerPlant();
			}
		}
	}
}
