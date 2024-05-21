using UnityEngine;
using UnityEngine.Windows;

public class GameUI : MonoBehaviour
{
    [SerializeField] private InputController _input;
    [Space]
    [SerializeField] private GameObject _settings;
    [SerializeField] private GameObject _help;

    private void Start()
    {
        _input.EventToMenu += Open;
        _input.EventContinue += Close;
        _input.EventRestart += Close;

        DataGame dataGame = DataGame.Instance;
        bool isFirstStart = dataGame.IsFirstStart;

        gameObject.SetActive(isFirstStart);
        _input.IsMenu = isFirstStart;

        if (!isFirstStart)
            return;

        gameObject.SetActive(true);
        _settings.SetActive(false);
        _help.SetActive(true);
    }

    public void Open()
    {
        gameObject.SetActive(true);

        _settings.SetActive(true);
        _help.SetActive(false);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
