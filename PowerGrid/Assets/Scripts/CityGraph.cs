using UnityEngine;
using System.Collections;

public class CityGraph : MonoBehaviour {

	private City[] cities = null;
	private ArrayList edges = null;


	// Use this for initialization
	void Start () {
		InitializeCities ();

	}

	void InitializeCities() {
		//create board
		cities = FindObjectsOfType(typeof(City)) as City[];
		edges = new ArrayList ();
		
		AddEdge ("Boston", "NewYork", 3);
		AddEdge ("Philadelphia", "NewYork", 0);
		AddEdge ("Buffalo", "NewYork", 8);
		AddEdge ("Philadelphia", "Washington", 3);
		AddEdge ("Pittsburgh", "Washington", 6);
		AddEdge ("Norfolk", "Washington", 5);
		AddEdge ("Norfolk", "Raleigh", 3);
		AddEdge ("Pittsburgh", "Raleigh", 7);
		AddEdge ("Buffalo", "Pittsburgh", 7);
		AddEdge ("Buffalo", "Detroit", 7);
		AddEdge ("Pittsburgh", "Detroit", 6);
		
		AddEdge ("Savannah", "Raleigh", 7);
		AddEdge ("Atlanta", "Raleigh", 7);
		AddEdge ("Atlanta", "Knoxville", 5);
		AddEdge ("Cincinnati", "Knoxville", 6);
		AddEdge ("Cincinnati", "Raleigh", 15);
		AddEdge ("Cincinnati", "Pittsburgh", 7);
		AddEdge ("Cincinnati", "Detroit", 4);
		AddEdge ("Cincinnati", "Chicago", 7);
		AddEdge ("Cincinnati", "StLouis", 12);
		
		AddEdge ("Savannah", "Jacksonville", 0);
		AddEdge ("NewOrleans", "Jacksonville", 16);
		AddEdge ("Birmingham", "Jacksonville", 9);
		AddEdge ("Tampa", "Jacksonville", 4);
		AddEdge ("Tampa", "Miami", 4);
		
		AddEdge ("Atlanta", "Birmingham", 3);
		AddEdge ("Atlanta", "StLouis", 12);
		AddEdge ("Atlanta", "Savannah", 7);
		
		AddEdge ("NewOrleans", "Birmingham", 11);
		AddEdge ("NewOrleans", "Memphis", 7);
		AddEdge ("NewOrleans", "Dallas", 12);
		AddEdge ("NewOrleans", "Houston", 8);
		
		AddEdge ("Memphis", "Birmingham", 6);
		AddEdge ("Memphis", "StLouis", 7);
		AddEdge ("Memphis", "KansasCity", 12);
		AddEdge ("Memphis", "OklahomaCity", 14);
		AddEdge ("Memphis", "Dallas", 12);
		AddEdge ("Dallas", "Houston", 5);
		AddEdge ("SantaFe", "Houston", 21);
		AddEdge ("Dallas", "OklahomaCity", 3);
		AddEdge ("KansasCity", "OklahomaCity", 8);
		AddEdge ("KansasCity", "SantaFe", 16);
		AddEdge ("KansasCity", "StLouis", 6);
		AddEdge ("KansasCity", "Chicago", 8);
		AddEdge ("KansasCity", "Omaha", 5);
		AddEdge ("KansasCity", "Denver", 16);
		
		AddEdge ("Chicago", "Detroit", 7);
		AddEdge ("Chicago", "Duluth", 12);
		AddEdge ("Chicago", "Minneapolis", 8);
		AddEdge ("Chicago", "Omaha", 13);
		
		AddEdge ("Duluth", "Detroit", 15);
		AddEdge ("Duluth", "Fargo", 6);
		AddEdge ("Duluth", "Minneapolis", 5);
		AddEdge ("Minneapolis", "Fargo", 6);
		AddEdge ("Billings", "Fargo", 17);
		AddEdge ("Billings", "Minneapolis", 18);
		
		AddEdge ("Cheyenne", "Minneapolis", 18);
		AddEdge ("Cheyenne", "Omaha", 14);
		AddEdge ("Cheyenne", "Denver", 0);
		AddEdge ("Cheyenne", "Billings", 9);
		AddEdge ("Cheyenne", "Boise", 24);
		
		AddEdge ("Boise", "Billings", 12);
		AddEdge ("Boise", "SaltLakeCity", 8);
		AddEdge ("Boise", "SanFrancisco", 23);
		AddEdge ("Boise", "Portland", 13);
		AddEdge ("Seattle", "Billings", 9);
		AddEdge ("Seattle", "Boise", 12);
		AddEdge ("Seattle", "Portland", 3);
		AddEdge ("Portland", "SanFrancisco", 24);
		AddEdge ("SaltLakeCity", "SanFrancisco", 27);
		AddEdge ("LasVegas", "SanFrancisco", 14);
		AddEdge ("LosAngeles", "SanFrancisco", 9);
		AddEdge ("LosAngeles", "LasVegas", 9);
		AddEdge ("LosAngeles", "SanDiego", 3);
		AddEdge ("LasVegas", "SanDiego", 9);
		AddEdge ("Pheonix", "SanDiego", 14);
		AddEdge ("Pheonix", "SantaFe", 18);
		AddEdge ("Pheonix", "LasVegas", 15);
		
		AddEdge ("SantaFe", "LasVegas", 27);
		AddEdge ("SantaFe", "SaltLakeCity", 28);
		AddEdge ("SantaFe", "Denver", 13);
		AddEdge ("SantaFe", "OklahomaCity", 15);
		AddEdge ("SantaFe", "Dallas", 16);
		AddEdge ("SantaFe", "Houston", 21);
	}
	
	void AddEdge(string city1, string city2, int cost) {
		
		int index1 = -1;
		int index2 = -1;
		
		for(int i = 0; i < cities.Length; i++)
		{
			if( cities[i].gameObject.name.CompareTo(city1)==0) {
				index1 = i;
			}
			if( cities[i].gameObject.name.CompareTo(city2)==0) {
				index2 = i;
			}
		}
		
		if (index1 == -1) {
			print ("Could not find a city:" + city1);
			return;
		}
		
		if (index2 == -1) {
			print ("Could not find a city:" + city2);
			return;
		}
		
		Edge e = new Edge (cities[index1], cities[index2], cost);
		edges.Add (e);
		if (cities [index1].edges == null)
			cities [index1].edges = new ArrayList ();
		if (cities [index2].edges == null)
			cities [index2].edges = new ArrayList ();
		cities[index1].edges.Add (e);
		cities[index2].edges.Add (e);
	}

	public int ComputeCityCost(City c, int step, Player player) {
				
		//if player has no cities, just current step value of city
		//if city is not available at step, return failure
		//if player has city, it is the cheapest combined edge + step value of city

		int cityCost = c.Cost (step);

		//occupied or already owned
		if (cityCost == -1)
			return -1;

		if (c.PlayerIsOwner (player))
			return -1;

		//at city cost, if no current cities
		if (player.cities.Count == 0)
			return cityCost;

		//must be connected to a player owned city
		//or a connected occupied city?

		//if player has cities, 
		//we do graph search and find cheapest combinedEdge 
		//from target city to player owned cities
		ArrayList combinedEdges = new ArrayList();
		ArrayList queuedCity = new ArrayList ();
		queuedCity.Add (c);

		while(queuedCity.Count > 0){
			City cty = (City)queuedCity[0];
			queuedCity.RemoveAt(0);
			cty.beenVisited = true;
			foreach (Edge e in cty.edges) {
				City otherCity;
				if(e.start == cty)
					otherCity = e.end;
				else
					otherCity = e.start;

				if(otherCity.beenVisited)
					continue;
				if(otherCity.Occupancy() == 0)
					continue;
				if(otherCity.PlayerIsOwner(player))
				   e.stopSearch = true;
				else
					queuedCity.Add(otherCity);

				combinedEdges.Add (e);
			}
		}

		int minCost = int.MaxValue;
		foreach(Edge e in combinedEdges) {
			if(e.cost < minCost)
				minCost = e.cost;
		}

		print ("city cost function is incomplete");
		return minCost;
	}

	public void ResetSearchFlags() {
		foreach(City c in cities)
			c.beenVisited = false;
		foreach (Edge e in edges)
			e.stopSearch = false;
	}
	// Update is called once per frame
	void Update () {
	
	}
}
