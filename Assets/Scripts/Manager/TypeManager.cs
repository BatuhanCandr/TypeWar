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


    [SerializeField] private TextMeshProUGUI _wordDisplay;
    [SerializeField] private TextMeshProUGUI scoreText;


    private string[] words = { "hello", "world", "unity", "game", "speed", "test" };
    private int currentIndex = 0;
    private int score = 0;

    void Start()
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

        for (int i = 0; i < words.Length; i++)
        {
            coloredText += GetWordColor(i);
        }

        return coloredText;
    }

    private string GetWordColor(int index)
    {
        string word = words[index];
        string colorTag = (index < currentIndex) ? "<color=green>" : (index == currentIndex) ? "<color=blue>" : "";
        return colorTag + word + "</color> ";
    }

    private void SetWordDisplay(string coloredText)
    {
        _wordDisplay.text = coloredText;
    }

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
                GameManager.Instance.castleController.SpawnBullets();
            }
            else if (inputText.Length >= currentWord.Length && inputText.StartsWith(currentWord))
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
    }

    private void DisplayParagraphOrPerformExtraActions()
    {
        if (currentIndex < words.Length)
        {
            DisplayParagraph();
        }
        else
        {
            // Paragraf sona erdiğinde ekstra işlemler yapabilirsiniz.
            // Örneğin, oyunu bitirme veya bir sonraki seviyeye geçme gibi.
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