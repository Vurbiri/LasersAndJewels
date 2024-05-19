using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CheatController : ASingleton<CheatController>, IPointerDownHandler
{
    [SerializeField] private int _clickCount = 5;
#if UNITY_EDITOR
    [SerializeField] private KeyCode _keyCode = KeyCode.C;
#endif

    private bool _isCheat;

    public bool IsCheat => _isCheat;

    public event Action<bool> EventChangeCheat;

    public void OnPointerDown(PointerEventData eventData)
    {

        if (eventData.clickCount == _clickCount)
        {
            _isCheat = !_isCheat;
            SoundSingleton.Instance.PlayCheat();
            Message.Banner("Cheat: " + (_isCheat ? "ON" : "OFF"), MessageType.FatalError, 2f);
            EventChangeCheat?.Invoke(_isCheat);
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(_keyCode))
        {
            _isCheat = !_isCheat;
            SoundSingleton.Instance.PlayCheat();
            Message.Banner("Cheat: " + (_isCheat ? "ON" : "OFF"), MessageType.FatalError, 2f);
            EventChangeCheat?.Invoke(_isCheat);
        }
    }
#endif
}
