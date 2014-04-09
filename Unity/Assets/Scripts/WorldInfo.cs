using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class WorldInfo {
    public static List<string> listOfPlayers;
	public static GameObject clientGO;
    public static string login;
    public static int countNetworkConnection;
    public static int countOfPlayersForThisGameToStart = 2;
    public static string state = "WAITING FOR PLAYER";

    private static WorldInfo instance;

    public WorldInfo() {
        Debug.Log("[WORLD INFO] WorldInfo");
        instance = this;
    }

    public static WorldInfo getInstance() { 
        if (instance == null) 
            return new WorldInfo();
        return instance; 
    }

	public static GameObject getClientGO() {
		return clientGO;
	}
}
