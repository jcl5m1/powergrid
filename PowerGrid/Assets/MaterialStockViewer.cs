using UnityEngine;
using System.Collections;

public class MaterialStockViewer : MonoBehaviour {


	public TextMesh coalText;
	public TextMesh oilText;
	public TextMesh garbageText;
	public TextMesh uraniumText;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		coalText.text = "Coal:\n" + GameState.instance.MaterialShop.QueryInventory (MaterialShop.MaterialType.Coal)
			+ " @ $" + GameState.instance.MaterialShop.QueryCost (MaterialShop.MaterialType.Coal);
		oilText.text = "Oil:\n" + GameState.instance.MaterialShop.QueryInventory (MaterialShop.MaterialType.Oil)
			+ " @ $" + GameState.instance.MaterialShop.QueryCost (MaterialShop.MaterialType.Oil);
		garbageText.text = "Garbage:\n" + GameState.instance.MaterialShop.QueryInventory (MaterialShop.MaterialType.Garbage)
			+ " @ $" + GameState.instance.MaterialShop.QueryCost (MaterialShop.MaterialType.Garbage);
		uraniumText.text = "Uranium:\n" + GameState.instance.MaterialShop.QueryInventory (MaterialShop.MaterialType.Uranium)
			+ " @ $" + GameState.instance.MaterialShop.QueryCost (MaterialShop.MaterialType.Uranium);
	}
}
