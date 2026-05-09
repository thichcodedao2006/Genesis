using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyData 
{
    [Header("NPC")]
    public static string NPCSecurity = "0";
    public static string NPCStaffA = "1";
    public static string NPCStaffB = "2";
    public static string NPCStaffC = "3";
    public static string NPCLibrarian = "4";

    [Header("Inventory")]
    public static int KeyA = 2;
    public static int KeyB = 3;
    public static int KeyC = 4;
    public static int KeyE = 5;
    public static int MapA = 6;

    [Header("State")]
    public static string HaveEnteredA = "HaveEnteredA";
    public static string HaveEnteredB = "HaveEnteredB";
    public static string HaveEnteredC = "HaveEnteredC";
    public static string HaveEnteredE = "HaveEnteredE";

    [Header("Spawn")]
    public static string RandomSpawn = "RandomSpawn";
    public static string SpawnFromA = "SpawnFromA";
}
