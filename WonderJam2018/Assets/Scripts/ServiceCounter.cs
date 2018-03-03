﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CounterSide
{
    Left,
    Right
};

public class ServiceCounter : MonoBehaviour {

    public CounterSide side;
    
    Queue<GameObject> clientsEnFile;

    [HideInInspector] public GameObject potion;  //TODO: pt changer le type

	// Use this for initialization
	void Start () {
        clientsEnFile = new Queue<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddClientToQueue(GameObject newClient)
    {
        if (clientsEnFile.Count < 2)
            clientsEnFile.Enqueue(newClient);
    }

    public void PopClientFromQueue()
    {
        clientsEnFile.Dequeue();
    }

    public bool IsQueueFull()
    {
        if (clientsEnFile.Count == 2)
            return true;
        else
            return false;
    }

    public int GetQueueCount()
    {
        return clientsEnFile.Count;
    }
    
    public bool DoesQueueContain(GameObject client)
    {
        if (clientsEnFile.Contains(client))
            return true;
        else
            return false;
    }
}
