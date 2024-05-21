using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ScreenMessage : MonoBehaviour
{
    [SerializeField] private TMP_Text _textCaption;
    [Space]
    [SerializeField] private Message _gameLevel;
    [SerializeField] private Message _levelComplete;
    [SerializeField] private Message _hint;

    public void Initialize()
    {
        SoundSingleton sound = SoundSingleton.Instance;

        _gameLevel.Initialize(_textCaption, sound.PlayNewLevel);
        _levelComplete.Initialize(_textCaption, sound.PlayLevelComplete);
        _hint.Initialize(_textCaption, sound.PlayHint);

        Clear();
    }

    public void GameLevel(int level)
    {
        gameObject.SetActive(true);
        StartCoroutine(GameLevel_Coroutine());

        #region Local function
        //=================================
        IEnumerator GameLevel_Coroutine()
        {
            yield return _gameLevel.SendFormat_Wait(level.ToString());
            yield return _gameLevel.Fide();
            Clear();
        }
        #endregion
    }

    public void LevelComplete()
    {
        gameObject.SetActive(true);
        StartCoroutine(GameOver_Coroutine());

        #region Local GameOver_Coroutine()
        //=================================
        IEnumerator GameOver_Coroutine()
        {
            yield return _levelComplete.Send_Wait();
            yield return _levelComplete.Fide();
            Clear();
        }
        #endregion
    }

    public void Hint()
    {
        gameObject.SetActive(true);
        StartCoroutine(Hint_Coroutine());

        #region Local Hint_Coroutine()
        //=================================
        IEnumerator Hint_Coroutine()
        {
            yield return _hint.Send_Wait();
            yield return _hint.Fide();
            Clear();
        }
        #endregion
    }

    public void ResetMessage()
    {
        StopAllCoroutines();
        Clear();
    }
    
    private void Clear()
    {
        _textCaption.text = string.Empty;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _gameLevel.OnDestroy();
        _levelComplete.OnDestroy();
    }

    #region Nested classes
    //********************************************
    [Serializable]
    private class Message
    {
        [SerializeField] private Text _caption;
        [Space]
        [SerializeField] private float _timeMessage = 5f;
        [Space]
        [SerializeField] private float _appearDuration = 0.5f;
        [SerializeField] private float _fadeDuration = 0.5f;

        private Action _playSound;
        
        public void Initialize(TMP_Text textCaption, Action playSound)
        {
            _caption.Initialize(textCaption);
            _playSound = playSound;
        }

        public WaitForSeconds Send_Wait()
        {
            _playSound?.Invoke();
            _caption.Send(_appearDuration);

            return new(Mathf.Max(_timeMessage, _appearDuration));
        }

        public WaitForSeconds SendFormat_Wait(string value)
        {
            _playSound?.Invoke();
            _caption.SendFormat(value, _appearDuration);

            return new(Mathf.Max(_timeMessage, _appearDuration));
        }

        public WaitForSeconds Fide()
        {
            _caption.Fade(_fadeDuration);

            return new(_fadeDuration);
        }

        public void OnDestroy()
        {
            _caption.OnDestroy();
        }

        #region Nested Classe
        //*******************************
        [Serializable]
        private class Text
        {
            [SerializeField] private string _key = string.Empty;
            [SerializeField] private Color _color = Color.white;

            private TMP_Text _text;
            private Localization _localization;
            private string _value = null;
            private bool _isActive = false;

            public void Initialize(TMP_Text text)
            {
                _text = text;
                _localization = Localization.Instance;
                _localization.EventSwitchLanguage += ReLocalize;
            }

            public void Send(float appearDuration)
            {
                _value = null;
                _text.text = (_isActive = !string.IsNullOrEmpty(_key)) ? _localization.GetText(_key) : string.Empty;
                if (_isActive)
                    _text.Appear(_color, appearDuration);
            }

            public void SendFormat(string value, float appearDuration)
            {
                _value = value;
                _text.text = (_isActive = !string.IsNullOrEmpty(_key)) ? _localization.GetTextFormat(_key, value) : string.Empty;
                if (_isActive)
                    _text.Appear(_color, appearDuration);
            }

            public void Fade(float fadeDuration)
            {
                if (!_isActive)
                    return;

                _isActive = false;
                _text.Fade(_color, fadeDuration);
            }

            public void OnDestroy()
            {
                if(Localization.Instance != null)
                    _localization.EventSwitchLanguage -= ReLocalize;
            }

            private void ReLocalize()
            {
                if (!_isActive)
                    return;

                if (_value != null)
                    _text.text = _localization.GetTextFormat(_key, _value);
                else
                    _text.text = _localization.GetText(_key);
            }
        }
        #endregion

    }
    #endregion
}
