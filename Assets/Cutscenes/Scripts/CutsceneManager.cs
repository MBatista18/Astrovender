using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] Image cutsceneDisplay;
    [SerializeField] TextMeshProUGUI cutsceneText;

    [SerializeField] CutscenePortion[] scenes;

    private void Start()
    {
        StartCoroutine(stringDisplay());
    }

    [SerializeField] bool openingCutscene;

    IEnumerator stringDisplay()
    {
        int currentIndex = 0;
        char[] textString;
        string finishedText = "";

        while (currentIndex < scenes.Length)
        {
            finishedText = "";
            textString = scenes[currentIndex].text.ToCharArray();

            if (cutsceneDisplay != null && scenes[currentIndex].image != null)
            {
                cutsceneDisplay.sprite = scenes[currentIndex].image;
            }

            for (int i = 0; i < textString.Length; i++)
            {
                finishedText = finishedText + textString[i];

                if (cutsceneText != null)
                {
                    cutsceneText.text = finishedText;
                }

                yield return new WaitForSeconds(0.07f);
            }

            yield return new WaitForSeconds(2f);

            currentIndex++;
        }

        if (openingCutscene)
        {
            FindAnyObjectByType<BeginNextDay>().ContinueToNextDay();
        }
        else
        {
            FindAnyObjectByType<CallSceneAt>().LoadScene();
        }
    } 
}

[System.Serializable]
public struct CutscenePortion
{
    public Sprite image;
    public string text;
}
