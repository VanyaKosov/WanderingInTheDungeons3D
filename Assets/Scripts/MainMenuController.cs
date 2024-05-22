using Assets.Scripts;
using Assets.Scripts.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void TinyMazeButtonClicked()
    {
        MazeParameters.mazeSize = new Vector2Int(7, 7);
        SceneManager.LoadScene("Maze");
    }

    public void MediumMazeButtonClicked()
    {
        MazeParameters.mazeSize = new Vector2Int(15, 15);
        SceneManager.LoadScene("Maze");
    }

    public void LargeMazeButtonClicked()
    {
        MazeParameters.mazeSize = new Vector2Int(27, 27);
        SceneManager.LoadScene("Maze");
    }
}
