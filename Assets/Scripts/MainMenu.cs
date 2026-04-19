using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public UIFader logoFader; // assign in Inspector

    public void PlayGame()
    {
        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        // Trigger fade
        logoFader.FadeOut();

        // Wait for fade to finish
        yield return new WaitForSeconds(logoFader.fadeDuration);

        // Now load scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Le jeu se ferme !");
        Application.Quit();
    }
}