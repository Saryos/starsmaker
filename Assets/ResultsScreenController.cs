﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class ResultsScreenController : MonoBehaviour {

    enum State { PLAYING, END_DELAY, RESULTS, LEAVING };
    State state;
    bool gameOver = false;
    float endGameCounter = 5;
    float leaveCounter = 3;
    public Texture2D gameoverTexture;
    public Texture2D survivalTexture;
    public Texture2D massTexture;
    public Texture2D barTexture;
    List<PlanetResult> results;
    // Use this for initialization
    void Start () {
        state = State.PLAYING;
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(state);
        switch(state)
        {
            case State.PLAYING:
                break;

            case State.END_DELAY:
                endGameCounter -= Time.fixedDeltaTime;

                if (endGameCounter <= 0)
                {
                    state = State.RESULTS;
                    Time.timeScale = 0;
                }
                break;

            case State.RESULTS:
                if (Input.GetButtonDown("Submit"))
                    state = State.LEAVING;
                break;

            case State.LEAVING:
                leaveCounter -= Time.fixedDeltaTime;
                Time.timeScale = 0.3f;
                if (leaveCounter < 0)
                {
                    Time.timeScale = 1;
                    SceneManager.LoadScene("title");
                }
                break;
        }
	}

    private void OnGUI()
    {
        switch(state)
        {
            case State.RESULTS:
                GUI.DrawTexture(new Rect(Screen.width / 2 - gameoverTexture.width / 2, 100 - gameoverTexture.height / 2, gameoverTexture.width, gameoverTexture.height), gameoverTexture);

                results.Sort(SortByMass);
                float maxMass = 0;

                for(int i = 0; i < results.Count; i++)
                {
                    if (maxMass == 0)
                        maxMass = results[i].endMass;

                    GUI.color = results[i].color;
                    GUI.DrawTexture(new Rect(Screen.width * 0.2f, 300 + 30 * i, 100f * (results[i].endMass/maxMass), 18),
                        barTexture);
                    GUI.color = Color.white;
                    GUI.Label(new Rect(Screen.width * 0.2f + 2, 300 + 30 * i, 100, 100),
                        Mathf.Round(results[i].endMass).ToString());
                }


                break;
        }
     }

    int SortByMass(PlanetResult a, PlanetResult b)
    {
        if (a.endMass < b.endMass)
            return 1;
        if (a.endMass > b.endMass)
            return -1;
        return 0;
    }

    public void EndGame(List<PlanetResult> data)
    {
        state = State.END_DELAY;
        Time.timeScale = 0.3f;
        results = data;
    }
}
