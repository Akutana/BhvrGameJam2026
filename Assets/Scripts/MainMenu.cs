using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Fonction appelée quand on clique sur Play
    public void PlayGame()
    {
        // Charge la scène suivante dans l'ordre du jeu
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Fonction appelée quand on clique sur Quit
    public void QuitGame()
    {
        Debug.Log("Le jeu se ferme !"); // Juste pour vérifier dans l'éditeur
        Application.Quit();
    }
}