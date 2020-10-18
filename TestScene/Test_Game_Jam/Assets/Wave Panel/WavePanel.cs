using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WavePanel : MonoBehaviour
{
    public static WavePanel instance;

    [SerializeField] Image boneImage;
    [SerializeField] Text waveStartText;
    [SerializeField] Image endScreenPanel;
    [SerializeField] Image endScreenFog;
    [SerializeField] Text waveSurvivedText;
    [SerializeField] Image[] endScreenImages;
    [SerializeField] Text[] endScreenTexts;
    [SerializeField] Button[] endScreenButtons;
    IEnumerator WaveIndicator;

    [SerializeField]
    [Tooltip("How long in seconds it take for the indicator to fade in or out")]
    float waveIndicatorFadeTime = 0.5f;

    [SerializeField]
    [Tooltip("How long in seconds the wave indicator will stay on the screen before it fades out")]
    float waveIndicatorStayTime = 1;

    [SerializeField]
    [Tooltip("How long it takes for the ends screen to fade in")]
    float endScreenFadeInTime = 2;

    [SerializeField]
    [Tooltip("After the panel Fades in this is how long it take for the ofther end screen UI Elements to fade in")]
    float endScreenOtherFadeInTime = 0.5f;

    int wave = 0;

    private void Start()
    {
        if (instance != null) Destroy(gameObject);
        instance = this;
        EnemyManager.instance.SubscribeWaveStart(WaveStart);
    }

    private void Update()
    {
        //if (Input.GetButtonDown("Test")) GameOver();
    }

    public void WaveStart()
    {
        wave = EnemyManager.instance.GetWave();
        if (WaveIndicator != null) StopCoroutine(WaveIndicator);
        WaveIndicator = WaveFade();
        StartCoroutine(WaveIndicator);
    }

    public void GameOver()
    {
        StartCoroutine(EndScreen());
    }

    IEnumerator WaveFade()
    {
        waveStartText.text = "Wave: " + wave.ToString();
        
        float elapsedTime = 0.0f;
        while (elapsedTime < waveIndicatorFadeTime)
        {
            SetIndicatorAlpha(elapsedTime / waveIndicatorFadeTime);
            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }
        SetIndicatorAlpha(1);
        yield return new WaitForSeconds(waveIndicatorStayTime);
        elapsedTime = 0.0f;
        while (elapsedTime < waveIndicatorFadeTime)
        {
            SetIndicatorAlpha(1 - (elapsedTime / waveIndicatorFadeTime));
            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }
        SetIndicatorAlpha(0);
        WaveIndicator = null;
    }

    private void SetIndicatorAlpha(float a)
    {
        Color color;
        color = boneImage.color;
        color.a = a;
        boneImage.color = color;

        color = waveStartText.color;
        color.a = a;
        waveStartText.color = color;
    }

    IEnumerator EndScreen()
    {
        Color fogColor = endScreenFog.color;
        Color panelColor = endScreenPanel.color;
        float elapsedTime = 0.0f;
        float fogTime = endScreenFadeInTime / 2;
        float panelStart = endScreenFadeInTime / 4;
        float a;
        while (elapsedTime < endScreenFadeInTime)
        {
            if (elapsedTime <= fogTime)
            {
                a = elapsedTime / fogTime;
                fogColor.a = a;
                endScreenFog.color = fogColor;
            }
            if (elapsedTime >= panelStart)
            {
                a = (elapsedTime - panelStart) / (endScreenFadeInTime - panelStart);
                panelColor.a = a;
                endScreenPanel.color = panelColor;
            }

            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }

        foreach (Button b in endScreenButtons)
        {
            b.interactable = true;
        }

        waveSurvivedText.text = "You Survived " + (wave - 1).ToString() + " Waves";
        elapsedTime = 0.0f;
        while (elapsedTime < endScreenOtherFadeInTime)
        {
            SetAplhaOtherUI(elapsedTime / endScreenOtherFadeInTime);
            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }
        SetAplhaOtherUI(1);

    }

    private void SetAplhaOtherUI(float a)
    {
        Color color;
        foreach (Image image in endScreenImages)
        {
            color = image.color;
            color.a = a;
            image.color = color;
        }
        foreach (Text text in endScreenTexts)
        {
            color = text.color;
            color.a = a;
            text.color = color;
        }
    }

    public void TryAgain()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
