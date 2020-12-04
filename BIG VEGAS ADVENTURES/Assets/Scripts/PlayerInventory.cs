using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerInventory 
{
    public static List<GameObject> inventory = new List<GameObject>();
    private static bool hitting;
    public static void Add(GameObject gameObject)
    {
        inventory.Add(gameObject);
        gameObject.SetActive(false);
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
    public static bool getHitting()
    {
        return hitting; 
    }
    public static void setHitting(bool hit)
    {
        hitting = hit;
    }

    public static List<GameObject> getInventory()
    {
        return inventory; 
    }
}
