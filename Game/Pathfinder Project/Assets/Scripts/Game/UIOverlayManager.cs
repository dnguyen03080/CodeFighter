﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class UIOverlayManager : MonoBehaviour
{
    private string _InstructionsHeader = string.Empty;
    private string _Instructions = string.Empty;
    private float _StartTime;
    public Text StageNameTextBox;
    public Text TimerTextBox;
    public Text InstructionHeaderTextBox;
    public Text InstructionsTextBox;

	// Use this for initialization
	void Start ()
    {
        GetInstructionsForCurrentStage();
        _StartTime = Time.time;
	}
	
	// Update is called once per frame
	void Update ()
    {   
        UpdateLeftPanel();
        UpdateRightPanel();
    }

    /// <summary>
    /// Updates components used in the left panel of the UI overlay
    /// </summary>
    private void UpdateLeftPanel()
    {
        UpdateStageName();
        UpdateTimeElapsed();

    }

    /// <summary>
    /// Update the time elapsed in the top left of the UI
    /// </summary>
    private void UpdateTimeElapsed()
    {
        // If active, then increment the time
        float timeElapsedValue = Time.time - _StartTime;

        // Convert time elapsed into minutes and seconds
        var minutes = (int)timeElapsedValue / 60;
        var seconds = (int)timeElapsedValue % 60;

        string timeElapsed = String.Format("Time Elapsed: {0:0}:{1:00}",
                                           minutes, seconds);

        if (TimerTextBox != null)
        {
            TimerTextBox.text = timeElapsed;
        }
    }

    /// <summary>
    /// Update the name of the stage in the top left of the UI
    /// </summary>
    private void UpdateStageName()
    {
        if (StageNameTextBox != null)
        {
            StageNameTextBox.text = GameManager.CurrentStage;
        }
    }

    /// <summary>
    /// Updates components used in the right panel of the UI overlay
    /// </summary>
    private void UpdateRightPanel()
    {
        InstructionHeaderTextBox.text = _InstructionsHeader;
        InstructionsTextBox.text = _Instructions;
    }

    public void GetInstructionsForCurrentStage()
    {
        List<string> instructionsList = new List<string>();
        string instructionsHeader = string.Empty;
        string instructions = string.Empty;

        string stageName = GameManager.CurrentStage;
        string instructionsPath = Application.dataPath + "/Resources/StageInstructions/" + stageName + ".txt";

        if (File.Exists(instructionsPath))
        {
            instructionsList = new List<string>(File.ReadAllLines(instructionsPath));
        }

        if (instructionsList.Count > 0)
        {
            // First line is reserved for the instructions heading
            instructionsHeader = instructionsList[0];
            instructionsList.RemoveAt(0);

            // The rest is used to form the instructions string
            foreach (string line in instructionsList)
            {
                instructions += line;
            }
        }

        _InstructionsHeader = instructionsHeader;
        _Instructions = instructions;
    }
}