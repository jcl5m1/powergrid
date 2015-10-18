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
		}
		
		drawDeckPowerPlants.Sort ();
	}
	
	public void DealCards() {
		while (inMarketPowerPlants.Count < marketCount) {
			inMarketPowerPlants.Add(drawDeckPowerPlants[0]);
			drawDeckPowerPlants.RemoveAt(0);
		}	
		
		inMarketPowerPlants.Sort ();

		LayoutCards ();
	}

	public void LayoutCards() {
		//layout cards
		float xPos = 0.05f;
		float xStep = 0.15f;
		float yPos = -0.2f;
		float yStep = 0.15f;

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

	public void DeselectAll() {
		selectedPlant = null;
	}

	public void BuyCurrentCity(Player p) {
		if (selectedPlant == null)
			return;
		if (p.cash < currentBid)
			return;

		p.powerPlants.Add(selectedPlant);
		p.cash -= currentBid;
		inMarketPowerPlants.Remove(selectedPlant);
		selectedPlant.transform.position = new Vector3 (100, 100, 100);
		selectedPlant = null;
		DealCards();
		LayoutCards();
	}

	// Update is called once per frame
	void Update () {

		bidText.text = "Bid: " + currentBid;

		foreach (PowerPlant pp in drawDeckPowerPlants) {
			pp.gameObject.GetComponent<Renderer>().material.color = Color.gray;
		}

		LayoutCards ();

		if(selectedPlant != null) {
			Vector3 pos = selectedPlant.gameObject.transform.position;
			pos.y = 1.0f + 0.25f * (1.0f + Mathf.Sin (5 * UnityEngine.Time.realtimeSinceStartup));
			selectedPlant.gameObject.transform.position = pos;
		}
	}

}
