using UnityEngine;
using System.Collections;

public class PlayerMiniView : MonoBehaviour {

	public TextMesh nameText;
	public TextMesh cityText;
	public TextMesh cashText;

	public Player player;

	private ArrayList plantMiniViews = new ArrayList();

	// Use this for initialization
	void Start () {
	
	}

	public void Setup(Player p) {
		player = p;
	}

	public void Layout() {
		if (player != null) {
			nameText.text = player.gameObject.name;
			cityText.text = "C:" + player.PowerPotential () + "/" + player.cities.Count;
			cashText.text = "$" + player.cash.ToString ();
			for(int i = plantMiniViews.Count; i < player.powerPlants.Count; i++) {
				GameObject obj = ((PowerPlant)player.powerPlants[i]).MiniCardObj;
				plantMiniViews.Add (obj);
			}
			int index = 0;
			foreach(PowerPlant pp in player.powerPlants) {
				GameObject obj = pp.MiniCardObj;
				obj.transform.parent = transform;
				obj.transform.localPosition = new Vector3(0,0.1f - 0.2f*index,-0.01f);
				index++;
			}
		}
	}

	// Update is called once per frame
	void Update () {
	}

	public void OnMouseOver() {
		if (GameState.instance.CurrentState == GameState.State.BuyPlants) {
			if (Input.GetMouseButtonDown (0)) {
				GameState.instance.PowerplantShop.BuyCurrentCity(player);
			}
		}
	}
}
