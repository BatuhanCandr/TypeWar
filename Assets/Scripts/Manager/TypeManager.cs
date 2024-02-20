using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TypeManager : NetworkBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    public TextMeshProUGUI _wordDisplay;
    [SerializeField] private TextMeshProUGUI scoreText;

    private string[] words =
    {
       "programming", "mirror",
        "sun", "moon", "star", "planet", "keyboard", "mouse", "guitar", "piano", "ocean", "mountain", "forest", "cloud",
        "rain", "snow", "fire", "water", "earth", "wind", "light", "dark", "happy", "sad", "angry", "love", "hate",
        "peace", "war", "friend", "enemy", "family", "stranger", "history", "future", "science", "art", "music",
        "dance", "dream", "reality", "space", "time", "hope",
        "flower", "breeze", "smile", "laughter", "journey", "whisper", "courage", "wisdom", "serenity", "joy",
        "freedom", "imagination", "creation", "adventure", "inspiration", "harmony", "tranquility", "passion", "purpose",
        "forgiveness", "gratitude", "compassion", "kindness", "innovation", "wonder", "infinity", "reflection", "victory",
        "mystery", "celebration", "magic", "balance", "noble", "eternity", "discovery", "effort", "thrive", "grace",
        "radiant", "effervescent", "illuminate", "whimsical", "ethereal", "ripple", "chime", "savor", "delight", "luminous"
    };

   private int startIndex = 0;
    private int endIndex = 0;
    private int currentIndex = 0;
    private int score = 0;

    public override void OnStartClient()
    {
        inputField.ActivateInputField();
        DisplayParagraph();
    }

    private void Update()
    {
        CheckInput();
        ResetInput();
    }

    void DisplayParagraph()
    {
        string coloredText = GetColoredText();
        SetWordDisplay(coloredText);
        ResetInputField();
    }

    private string GetColoredText()
    {
        string coloredText = "";

        for (int i = startIndex; i <= endIndex && i < words.Length; i++)
        {
            coloredText += GetWordColor(i);
        }

        return coloredText;
    }

    private string GetWordColor(int index)
    {
        string word = words[index];
        string colorTag = (index < currentIndex) ? "<color=green>" : (index == currentIndex) ? "<color=white>" : "";
        return colorTag + word + "</color> ";
    }

    private void SetWordDisplay(string coloredText) 
    {
        _wordDisplay.text = coloredText;
    }

    [Client]
    private void CheckInput()
    {
        if (currentIndex < words.Length)
        {
            string inputText = inputField.text.Trim().ToLower();
            string currentWord = words[currentIndex].ToLower();

            if (inputText == currentWord)
            {
                IncrementScoreAndIndex();
                DisplayParagraphOrPerformExtraActions();
                GameManager.Instance.playerController.Shoot();
            }
            else if (inputText.StartsWith(currentWord))
            {
                IncrementIndex();
                DisplayParagraph();
            }
        }
    }

    private void IncrementScoreAndIndex()
    {
        score++;
        scoreText.text = "Score: " + score.ToString();
        currentIndex++;

        if (currentIndex > endIndex && endIndex < words.Length - 1)
        {
            startIndex += 1;
            endIndex += 1;
        }
    }

    private void DisplayParagraphOrPerformExtraActions()
    {
        if (currentIndex < words.Length)
        {
            DisplayParagraph();
        }
        else
        {
            // Perform extra actions when the paragraph is completed.
            // For example, finish the game or move to the next level.
        }
    }

    private void IncrementIndex()
    {
        currentIndex++;
    }

    private void ResetInputField()
    {
        inputField.text = "";
        inputField.ActivateInputField();
    }

    private void ResetInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            ResetInputField();
        }
    }
}
