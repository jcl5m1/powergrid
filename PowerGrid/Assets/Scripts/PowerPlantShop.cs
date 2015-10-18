using UnityEngine;
using System.Collections;

public class PowerPlantShop : MonoBehaviour {

	[HideInInspector]
	public ArrayList drawDeckPowerPlants = null;
	[HideInInspector]
	public ArrayList inMarketPowerPlants = null;

	[HideInInspector]
	public int marketCount = 8;
	[HideInInspector]
	public int biddableCount = 4;
	public int maxPowerPlants = 3;
	private int randomlyRemovedCards = 4;
	public int currentBid;

	[HideInInspector]
	public PowerPlant selectedPlant = null;
	
	private TextMesh bidText;

	// Use this for initialization
	void Start () {
		bidText = GameObject.Find("BidText").GetComponent<TextMesh>();
		InitializePowerPlants ();
		DealCards ();		
		ShufflePowerPlantCards ();
	}

	public void Reset() {
		inMarketPowerPlants.Clear ();
		drawDeckPowerPlants.Clear ();
		InitializePowerPlants ();
		DealCards ();		
		ShufflePowerPlantCards ();
	}

	
	public void MovePowerPlantCard(int fromPosition, int toPosition) {
		
		if (toPosition == fromPosition)
			return;
		drawDeckPowerPlants.Insert (toPosition, drawDeckPowerPlants [fromPosition]);
		
		if (toPosition < fromPosition) 
			drawDeckPowerPlants.RemoveAt (fromPosition+1);
		else
			drawDeckPowerPlants.RemoveAt (fromPosition);
	}
	
	public int FindIndexOfPowerPlantCard(int cost) {
		for (int i = 0; i < drawDeckPowerPlants.Count; i++) {
			if( ((PowerPlant)drawDeckPowerPlants[i]).baseCost == cost)
				return i;			
		}
		return -1;
	}
	
	void ShufflePowerPlantCards() {
		ArrayList shuffled = new ArrayList();
		
		int randomIndex = 0;
		while (drawDeckPowerPlants.Count > 0)
		{
			randomIndex = Random.Range(0, drawDeckPowerPlants.Count); //Choose a random object in the list
			shuffled.Add(drawDeckPowerPlants[randomIndex]); //add it to the new, random list
			drawDeckPowerPlants.RemoveAt(randomIndex); //remove to avoid duplicates
		}
		
		drawDeckPowerPlants = shuffled;
		
		//put step 3 on bottom
		int index = FindIndexOfPowerPlantCard (100);
		if (index != -1)
			MovePowerPlantCard (index, drawDeckPowerPlants.Count);
		
		//put 13 on top
		index = FindIndexOfPowerPlantCard (13);
		if (index != -1)
			MovePowerPlantCard (index, 0);
		
	}
	
	public void InitializePowerPlants() {
		drawDeckPowerPlants = new ArrayList ();
		inMarketPowerPlants = new ArrayList ();
		
		//create power plant cards
		PowerPlant[] plants = FindObjectsOfType(typeof(PowerPlant)) as PowerPlant[];
		for (int i = 0; i < plants.Length; i++) {
			drawDeckPowerPlants.Add (plants [i]);
			plants[i].purchased = false;
		}
		
		drawDeckPowerPlants.Sort ();
	}
	
	public void DealCards() {
		while (inMarketPowerPlants.Count < marketCount) {
			inMarketPowerPlants.Add(drawDeckPowerPlants[0]);
			drawDeckPowerPlants.RemoveAt(0);
		}	
		
		inMarketPowerPlants.Sort ();

		LayoutPowerPlantCards ();
	}

	public void LayoutPowerPlantCards() {
		//layout cards
		float xPos = -0.4f;
		float xStep = 0.1f;
		float yPos = 0.1f;
		float yStep = 0.2f;

		for (int i = 0; i < inMarketPowerPlants.Count; i++) {
			PowerPlant pp = (PowerPlant)inMarketPowerPlants[i];
			pp.gameObject.transform.parent = transform;
			if(i < biddableCount) {
				pp.gameObject.transform.localPosition = new Vector3(xPos + (i%biddableCount)*xStep, yPos ,-0.01f); 
				pp.gameObject.GetComponent<Renderer>().material.color = Color.green;
			} else {
				pp.gameObject.transform.localPosition = new Vector3(xPos + (i%biddableCount)*xStep, yPos + yStep,-0.01f); 
				pp.gameObject.GetComponent<Renderer>().material.color = Color.blue;
			}
		}
	}

	public void DiscardSelectedPowerPlant() {
		if (selectedPlant == null)
			return;

		if (inMarketPowerPlants.Contains (selectedPlant)) {
			inMarketPowerPlants.Remove(selectedPlant);
			selectedPlant.Hide();
			DealCards();
			LayoutPowerPlantCards();
			return;
		}

		foreach (Player p in GameState.instance.Players) {
			if(p.powerPlants.Contains(selectedPlant)) {
				p.powerPlants.Remove(selectedPlant);
				selectedPlant.Hide();
				break;
			}
		}
		LayoutPlayerMiniViews ();
	}

	public void LayoutPlayerMiniViews() {
		//layout players
		float xPos = -3.25f;
		float xStep = 2.2f;
		float zPos = -1.0f;

		Vector3 localPos = transform.position;
		int index = 0;
		foreach(Player p in GameState.instance.Players) {
			p.PlayerMiniViewObj.transform.position = localPos + new Vector3(xPos + xStep*index, 0.01f, zPos);
			p.PlayerMiniViewObj.GetComponent<PlayerMiniView>().Layout();
			index++;
		}
	}

	public void DeselectAll() {
		selectedPlant = null;
	}

	public void BuyCurrentCity(Player p) {
		if (selectedPlant == null)
			return;
		if (selectedPlant.purchased)//can't purchase already purchased plant
			return;
		if (p.cash < currentBid)
			return;
		if (p.powerPlants.Count >= maxPowerPlants)
			return;

		p.powerPlants.Add(selectedPlant);
		p.cash -= currentBid;
		selectedPlant.purchased = true;
		inMarketPowerPlants.Remove(selectedPlant);
		selectedPlant.transform.position = new Vector3 (100, 100, 100);
		DeselectAll ();
		DealCards();
		LayoutPowerPlantCards();
		LayoutPlayerMiniViews ();

	}

	public void Show(bool show) {

		if (show) {
			transform.localPosition = new Vector3 (0.0f,0.0f, -0.01f);
			DeselectAll ();
			DealCards ();
			LayoutPowerPlantCards ();
			LayoutPlayerMiniViews ();
		} else {
			transform.localPosition = new Vector3 (-2.0f,0.3f, -0.15f);

		}
	}
	
	public void SetSelectedPlant(PowerPlant p) {
		selectedPlant = p;
		currentBid = p.baseCost;
		LayoutPowerPlantCards ();
		LayoutPlayerMiniViews ();
	}

	// Update is called once per frame
	void Update () {

		bidText.text = "Bid: " + currentBid;

//		foreach (PowerPlant pp in drawDeckPowerPlants) {
//			pp.gameObject.GetComponent<Renderer>().material.color = Color.gray;
//		}

		if(selectedPlant != null) {
			Vector3 pos = selectedPlant.gameObject.transform.position;
			pos.y = 0.2f + 0.2f * (1.0f + Mathf.Sin (5 * UnityEngine.Time.realtimeSinceStartup));
			selectedPlant.gameObject.transform.position = pos;

			pos = selectedPlant.MiniCardObj.transform.position;
			pos.y = 0.2f + 0.2f * (1.0f + Mathf.Sin (5 * UnityEngine.Time.realtimeSinceStartup));
			selectedPlant.MiniCardObj.transform.position = pos;
		}

	}

}
