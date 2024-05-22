#if UNITY_EDITOR

using NaughtyAttributes;
using System.IO;
using UnityEngine;

public partial class YandexSDK
{
    [Space]
    [SerializeField] private string _playerName = "Best of the Best";
    [SerializeField] private bool _isDesktop = true;
    [SerializeField] private bool _isLogOn = true;
    [SerializeField, Dropdown("GetLangValues")] private string _lang = "ru";
    [SerializeField] private bool _isInitialize = true;
    [SerializeField] private bool _isPlayer = true;

    private DropdownList<string> GetLangValues()
    {
        return new DropdownList<string>()
        {
            { "Русский",  "ru" },
            { "English",  "en" }
        };
    }

    public bool IsDesktop => _isDesktop;

    public bool IsInitialize => _isInitialize;
    public bool IsPlayer => IsInitialize && _isPlayer;
    public string PlayerName => _playerName;
    public bool IsLogOn => _isLogOn;
    public string Lang => _lang;

    public WaitResult<bool> InitYsdk() => new(_isInitialize);
    public void LoadingAPI_Ready() { }
    public WaitResult<bool> InitPlayer() => new(_isPlayer);
    public WaitResult<bool> LogOn()
    {
        _isLogOn = true;
        return new(_isLogOn); ;
    }

    public string GetPlayerAvatarURL(AvatarSize size) => string.Empty;
        

    public WaitResult<bool> Save(string key, string data)
    {
        using StreamWriter sw = new(Path.Combine(Application.persistentDataPath, key));
        sw.Write(data);

        return new(true);
    }
    public WaitResult<string> Load(string key)
    {
        string path = Path.Combine(Application.persistentDataPath, key);
        if (File.Exists(path))
        {
            using StreamReader sr = new(path);
            return new(sr.ReadToEnd());
        }
        return new(string.Empty);
    }
}
#endif
