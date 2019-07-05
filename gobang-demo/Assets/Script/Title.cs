using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Title : MonoBehaviour {

    public Button StartButton;
    public Button ExitButton;
    public Button Start01Button;
    // board窗口
    public GameObject board;

	// Use this for initialization
	void Start () {
        StartButton.onClick.AddListener(() =>
        {
            // gameObject.SetActive(false);
            this.gameObject.SetActive(false);
            board.SetActive(true);
            Time.timeScale = 1;
        });

        ExitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
        Start01Button.onClick.AddListener(() =>
        {
            // gameObject.SetActive(false);
            this.gameObject.SetActive(false);
            board.SetActive(true);
        });

    }
	
}
