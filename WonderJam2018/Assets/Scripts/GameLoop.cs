﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLoop : MonoBehaviour {

    public enum States
    {
        Rangement,
        Journee,
        Closed,
        EndDay
    }
    [SerializeField] AudioClip DuringDay;
    [SerializeField] AudioClip AfterDay;
    AudioManager audioMixer;

    public static States currentState;
    public GameObject rideau;


    private Vector3 rideauTarget;
    private Vector3 rideauPosition;
    private float rideauTemps;
    public GameObject clientSpawner1, clientSpawner2;
    public ServiceCounter[] serviceCounters;

    [SerializeField] Sprite winImageP1;
    [SerializeField] Sprite winImageP2;
    [SerializeField] RuntimeAnimatorController animatorWin1;
    [SerializeField] RuntimeAnimatorController animatorWin2;

    public void Awake()
    {
        currentState = States.Rangement;
        rideauTarget = Vector3.zero;
        rideauPosition = rideau.transform.position;
        rideauTemps = 0f;
    }

    public void Start()
    {
        audioMixer = GameObject.Find("AudioMixer").GetComponent<AudioManager>();
        audioMixer.PlayMusic(DuringDay);
    }


    public void StartVentes()
    {
        
        // Monter le Rideau
        rideauTarget = rideau.transform.position + Vector3.up * 5f;
        rideau.GetComponent<BoxCollider2D>().enabled = false;

        // Activer l'horloge et le swap
        GameObject.Find("Canvas").GetComponent<UIManager>();
        GameObject.Find("Timer").GetComponent<Animator>().SetTrigger("Toggle");

        clientSpawner1.SetActive(true);
        clientSpawner2.SetActive(true);

        currentState = States.Journee;
    }

    public void FermerPortes()
    {
        audioMixer.PlayMusic(AfterDay);
        //Faire le panel de fermeture
        GameObject.Find("ClosedSign").GetComponent<Animator>().SetTrigger("CloseSign");

        clientSpawner1.SetActive(false);
        clientSpawner2.SetActive(false);

        currentState = States.Closed;
    }

    public void Update()
    {
        if (rideauTarget != Vector3.zero)
        {
            rideau.transform.position = Vector3.Lerp(rideauPosition, rideauTarget, rideauTemps / 3f);
            rideauTemps += Time.deltaTime;
            if (rideauTemps > 3f)
                rideauTarget = Vector3.zero;
        }

        if (currentState == States.Closed)
        {
            for(int i = 0; i < serviceCounters.Length; i++)
            {
                Debug.Log(i + "     " + serviceCounters[i].GetQueueCount());
                if(serviceCounters[i].GetQueueCount() > 0)
                {
                    return;
                }
            }
            currentState = States.EndDay;

            GameObject winScreen = GameObject.Find("WinScreen");
            Text p1EndScore = winScreen.transform.GetChild(0).GetChild(0).GetComponent<Text>();
            Text p2EndScore = winScreen.transform.GetChild(1).GetChild(0).GetComponent<Text>();

            p1EndScore.text = Score.scoreP1.ToString();
            p2EndScore.text = Score.scoreP2.ToString();

            if (Score.scoreP1 > Score.scoreP2)
            {
                winScreen.GetComponent<Image>().sprite = winImageP1;
                winScreen.GetComponent<Animator>().runtimeAnimatorController = animatorWin1;
            }
            else
            {
                winScreen.GetComponent<Image>().sprite = winImageP2;
                winScreen.GetComponent<Animator>().runtimeAnimatorController = animatorWin2;
            }
        }
    }
	
}
