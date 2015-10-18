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

	// Update is called once per frame
	void Update () {
		if (player != null) {
			nameText.text = player.gameObject.name;
			cityText.text = "C:" + player.PowerPotential () + "/" + player.cities.Count;
			cashText.text = "$" + player.cash.ToString ();
			for(int i = plantMiniViews.Count; i < player.powerPlants.Count; i++) {
				GameObject obj = ((PowerPlant)player.powerPlants[i]).MiniCardObj;
				obj.transform.parent = transform;
				obj.transform.localPosition = new Vector3(0,0.1f - 0.2f*i,-0.01f);
				plantMiniViews.Add (obj);
			}
		}
	}
}
