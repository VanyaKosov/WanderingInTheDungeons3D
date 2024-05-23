using Assets.Scripts;
using Assets.Scripts.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void TinyMazeButton()
    {
        MazeParameters.mazeSize = new Vector2Int(7, 7);
        SceneManager.LoadScene("Maze");
    }

    public void MediumMazeButton()
    {
        MazeParameters.mazeSize = new Vector2Int(15, 15);
        SceneManager.LoadScene("Maze");
    }

    public void LargeMazeButton()
    {
        MazeParameters.mazeSize = new Vector2Int(27, 27);
        SceneManager.LoadScene("Maze");
    }

    public void QuitGameButton()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
