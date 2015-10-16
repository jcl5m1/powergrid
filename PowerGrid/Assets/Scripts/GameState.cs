using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {

	public enum State {
		ComputeTurn,
		BuyPlants,
		BuyMaterials,
		BuildCities,
		Bureaucracy,
	};
	
	private State currentState = State.ComputeTurn;
	private ArrayList players = null;
	private ArrayList drawDeckPowerPlants = null;
	private ArrayList inMarketPowerPlants = null;
		
	private int marketCount = 8;
	private int biddableCount = 4;
	private int gameRound = 1;

	private int maxPowerPlants = 3;
	private int gameEndCityCount = 17;
	private int step2CityCount = 7;
	private int randomlyRemovedCards = 4;
	private int numberOfRegions = 3;
	public int[] cityCountPayoutTable;
	private int gameStep = 1;

	private int playerTurn = 0;

	public GameObject powerPlantPrefab;
	public GameObject cityPrefab;

	private PowerPlantMaterialStore materialStore;

	// Use this for initialization
	void Start () {
		print ("Initialize game");

		//setup players
		players = new ArrayList();

		//create objects from GUI setup
		Player[] p = FindObjectsOfType(typeof(Player)) as Player[];
		for (int i = 0; i < p.Length; i++) {
			players .Add (p [i]);
		}
		players.Sort ();
		players.Reverse ();

		//create material store
		materialStore = new PowerPlantMaterialStore ();

		InitializePayoutTable ();
		InitializePowerPlants ();
		DealCards ();

		ShufflePowerPlantCards ();
	}

	void Reset() {
		foreach (Player p in players)
			p.Reset ();

		inMarketPowerPlants.Clear ();
		drawDeckPowerPlants.Clear ();
		InitializePowerPlants ();
		DealCards ();

		ShufflePowerPlantCards ();
	}

	void InitializePayoutTable() {
		cityCountPayoutTable = new int[21];
		cityCountPayoutTable [0] = 10;
		cityCountPayoutTable [1] = 22;
		cityCountPayoutTable [2] = 33;
		cityCountPayoutTable [3] = 44;
		cityCountPayoutTable [4] = 54;
		cityCountPayoutTable [5] = 64;
		cityCountPayoutTable [6] = 73;
		cityCountPayoutTable [7] = 82;
		cityCountPayoutTable [8] = 90;
		cityCountPayoutTable [9] = 98;
		cityCountPayoutTable [10] = 105;
		cityCountPayoutTable [11] = 112;
		cityCountPayoutTable [12] = 118;
		cityCountPayoutTable [13] = 124;
		cityCountPayoutTable [14] = 129;
		cityCountPayoutTable [15] = 134;
		cityCountPayoutTable [16] = 138;
		cityCountPayoutTable [17] = 142;
		cityCountPayoutTable [18] = 145;
		cityCountPayoutTable [19] = 148;
		cityCountPayoutTable [20] = 150;
	}



	void MovePowerPlantCard(int fromPosition, int toPosition) {

		if (toPosition == fromPosition)
			return;
		drawDeckPowerPlants.Insert (toPosition, drawDeckPowerPlants [fromPosition]);

		if (toPosition < fromPosition) 
			drawDeckPowerPlants.RemoveAt (fromPosition+1);
		else
			drawDeckPowerPlants.RemoveAt (fromPosition);
	}

	int FindIndexOfPowerPlantCard(int cost) {
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
	
	void InitializePowerPlants() {
		drawDeckPowerPlants = new ArrayList ();
		inMarketPowerPlants = new ArrayList ();

		//create power plant cards
		PowerPlant[] plants = FindObjectsOfType(typeof(PowerPlant)) as PowerPlant[];
		for (int i = 0; i < plants.Length; i++) {
			drawDeckPowerPlants.Add (plants [i]);
		}

		drawDeckPowerPlants.Sort ();
	}

	void DealCards() {
		while (inMarketPowerPlants.Count < marketCount) {
			inMarketPowerPlants.Add(drawDeckPowerPlants[0]);
			drawDeckPowerPlants.RemoveAt(0);
		}	

		inMarketPowerPlants.Sort ();
	}

	void DoBureaucracy() {
		foreach (Player p in players) {
			int powerCount = 0;

			foreach(PowerPlant pp in p.powerPlants)
				powerCount += pp.RunPlant();
			if(powerCount > 20)
				powerCount = 20;
			p.cash += cityCountPayoutTable[powerCount];
			p.cash += Random.Range(0,20);
		}
	}

	// Update is called once per frame
	void Update () {	
		bool advanceState = false;
		bool advanceTurn = false;


		if (Input.GetKeyDown (KeyCode.Space))
			advanceTurn = true;

		if (Input.GetKeyDown (KeyCode.R)) {
			Reset();
		}


		switch (currentState) {
		case  State.ComputeTurn:
//			print (currentState);
			players.Sort();
			players.Reverse ();

			currentState = State.BuyPlants;
			playerTurn = 0;
			break;
		case  State.BuyPlants:
			//show plants
			//click on plant or pass
			//adjust price
			//click on player that buys
			//redraw
			if (advanceTurn) {
				playerTurn++;
				if(playerTurn == players.Count) {
					advanceState = true;
					playerTurn = players.Count-1;
				}
			}

			if(advanceState) {
				DealCards();
				currentState = State.BuyMaterials;
//				print (currentState);
			}
		break;
		case  State.BuyMaterials:
			//show each player's power plants
			//show market
			//allow buying into each plant by clicking
			if (advanceTurn) {
				playerTurn--;
				if(playerTurn == -1) {
					advanceState = true;
					playerTurn = players.Count-1;
				}
			}

			if(advanceState) {
				currentState = State.BuildCities;
//				print (currentState);
			}
		break;
		case  State.BuildCities:
			//hovering over city to shows cost (compute cost by searching over graph)

			//click to buy
			if (advanceTurn) {
				playerTurn--;
				if(playerTurn == -1) {
					advanceState = true;
					playerTurn = 0;
				}
			}
			if(advanceState) {
				currentState = State.Bureaucracy;
//				print (currentState);
			}
		break;
		case  State.Bureaucracy:
//			print (currentState);
			DoBureaucracy();
			materialStore.Restock(players.Count, gameStep);
			currentState = State.ComputeTurn;
			gameRound++;
			break;
		}


		int index = 0;
		foreach (Player p in players) {
			if(index == playerTurn) {
				p.gameObject.GetComponent<Renderer>().material.color = Color.red;
			} else {
				p.gameObject.GetComponent<Renderer>().material.color = Color.white;

			}
			index++;
		}

		foreach (PowerPlant pp in drawDeckPowerPlants) {
			pp.gameObject.GetComponent<Renderer>().material.color = Color.gray;
		}
		index = 0;
		foreach (PowerPlant pp in inMarketPowerPlants) {
			if(index < biddableCount)
				pp.gameObject.GetComponent<Renderer>().material.color = Color.green;
			else
				pp.gameObject.GetComponent<Renderer>().material.color = Color.blue;
			index++;
		}

	}

	void OnGUI() {

		int yPos = 10;
		GUI.Label (new Rect (10, yPos, 500, 20), "Round: " + gameRound + 
            " Step: " + gameStep + 
			" State: " + currentState + 
			" PlayerTurn: " + ((Player)players [playerTurn]).gameObject.name);
		yPos += 20;
		GUI.Label(new Rect(10,yPos,500,20),players[0].ToString());
		yPos += 20;
		GUI.Label(new Rect(10,yPos,500,20),players[1].ToString());
		yPos += 20;
		GUI.Label(new Rect(10,yPos,500,20),players[2].ToString());
		yPos += 20;
		GUI.Label(new Rect(10,yPos,500,20),players[3].ToString());
		yPos += 20;

		GUI.Label(new Rect(10,yPos,500,20),"Material Store: " +
		          " Coal: [" + materialStore.QueryInventory(PowerPlantMaterialStore.Type.Coal) + "," + materialStore.QueryCost(PowerPlantMaterialStore.Type.Coal)+"]" +
		          " Oil: [" + materialStore.QueryInventory(PowerPlantMaterialStore.Type.Oil) + "," + materialStore.QueryCost(PowerPlantMaterialStore.Type.Oil)+"]" +
		          " Garbage: [" + materialStore.QueryInventory(PowerPlantMaterialStore.Type.Garbage) + "," + materialStore.QueryCost(PowerPlantMaterialStore.Type.Garbage)+"]" +
		          " Uranium: [" + materialStore.QueryInventory(PowerPlantMaterialStore.Type.Uranium) + "," + materialStore.QueryCost(PowerPlantMaterialStore.Type.Uranium)+"]");
		yPos += 20;

		int count = 0;
		foreach (PowerPlant pp in inMarketPowerPlants) {
			if(count < biddableCount)
				GUI.Label (new Rect (10, yPos, 500, 20), "Biddable: " + pp.ToString ());
			else
				GUI.Label (new Rect (10, yPos, 500, 20), "On Deck: " + pp.ToString ());
			count++;
			yPos += 20;
		}

	}
}
