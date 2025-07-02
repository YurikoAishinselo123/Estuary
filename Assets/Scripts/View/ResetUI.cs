using UnityEngine;
using UnityEngine.UI;


public class ResetUI : MonoBehaviour
{
    [SerializeField] private Button resetButton;

    void Start()
    {
        resetButton.onClick.AddListener(ResetGame);
    }


    private void ResetGame()
    {
        SaveSystem.Instance.ResetAll();
    }

}
