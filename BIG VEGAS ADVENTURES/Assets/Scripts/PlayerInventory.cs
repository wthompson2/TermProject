using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerInventory 
{
    private static List<GameObject> inventory = new List<GameObject>();
    public static void Add(GameObject gameObject)
    {
        gameObject.SetActive(false);
        inventory.Add(gameObject);
        Debug.Log("Added " + gameObject + " to inventory.");
    }

    public static void Clear()
    {
        inventory.Clear();
    }

    public static bool isEmpty()
    {
        return inventory.Count == 0; 
    }
    
}
