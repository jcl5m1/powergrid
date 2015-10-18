using UnityEngine;
using System.Collections;

public class PlantCardMiniView : MonoBehaviour {
	
	public TextMesh nameText;
	public TextMesh materialText;
	public TextMesh powerText;
	public PowerPlant plant;

	// Use this for initialization
	void Start () {
	
	}

	public void Setup(PowerPlant p) {
		plant = p;
	}
	// Update is called once per frame
	void Update () {
		if (plant != null) {
			nameText.text = plant.gameObject.name;
			materialText.text = "M:" + plant.materialStock.ToString() + "/" + plant.materialCost.ToString();
			powerText.text = "P:" + plant.power.ToString();
		}
	}

}
