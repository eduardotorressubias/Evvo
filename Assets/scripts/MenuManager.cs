using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update

    private LevelChanger levelChanger;

    void Start()
    {
        levelChanger = FindObjectOfType<LevelChanger>();
    }

    // Update is called once per frame
    public void GameScene()
    {
        StartCoroutine(WaitPlay());

    }

    public void ExitGame()
    {
        StartCoroutine(WaitExit());

    }

    public void GameOver()
    {
       
        StartCoroutine(WaitLose());
        //SceneManager.LoadScene("LoseScene");
    }

    /*public void MainMenu()
    {
        StartCoroutine(WaitMenu());
    }
    */

    public void WinScene()
    {
        StartCoroutine(WaitWin());
    }

    private IEnumerator WaitPlay()
    {
        //audioPlayer.PlaySound(0, 1, 1);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("GameScene");
    }
    private IEnumerator WaitExit()
    {
        //audioPlayer.PlaySound(1, 1, 1);
        yield return new WaitForSeconds(0.8f);
        Application.Quit();
    }
    /*private IEnumerator WaitMenu()
    {
        //audioPlayer.PlaySound(2, 1, 1);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Menu");
    }*/
    private IEnumerator WaitWin()
    {

        //audioPlayer.PlaySound(3, 1, 1);
        yield return new WaitForSeconds(1f);
        levelChanger.OnFadeComplete();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("WinScene");
    }
    private IEnumerator WaitLose()
    {
        //audioPlayer.PlaySound(3, 1, 1);

        yield return new WaitForSeconds(1f);
        levelChanger.OnFadeComplete();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("LoseScene");

    }
}
