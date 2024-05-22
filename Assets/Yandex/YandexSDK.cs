using System;

//[DefaultExecutionOrder(-1)]
public partial class YandexSDK : ASingleton<YandexSDK>
{

#if !UNITY_EDITOR
    public bool IsInitialize => IsInitializeJS();
    public bool IsPlayer => IsPlayerJS();
    public bool IsLogOn => IsLogOnJS();
    public bool IsDesktop => IsDesktopJS();
    public bool IsMobile => IsMobileJS();

    public string PlayerName => GetPlayerNameJS();
    public string GetPlayerAvatarURL(AvatarSize size) => GetPlayerAvatarURLJS(size.ToString().ToLower());
    public string Lang => GetLangJS();

    public WaitResult<bool> InitYsdk() => WaitResult(ref _waitEndInitYsdk, InitYsdkJS);
    public void LoadingAPI_Ready() => ReadyJS();
    public WaitResult<bool> InitPlayer() => WaitResult(ref _waitEndInitPlayer, InitPlayerJS);

    public WaitResult<bool> LogOn() => WaitResult(ref _waitEndLogOn, LogOnJS);

    public WaitResult<bool> Save(string key, string data) => WaitResult(ref _waitEndSave, SaveJS, key, data);
    public WaitResult<string> Load(string key) => WaitResult(ref _waitEndLoad, LoadJS, key);

#endif

    private WaitResult<T> WaitResult<T>(ref WaitResult<T> completion, Action action)
    {
        completion = completion.Delete();
        action();
        return completion;
    }
    private WaitResult<T> WaitResult<T, U>(ref WaitResult<T> completion, Action<U> action, U value)
    {
        completion = completion.Delete();
        action(value);
        return completion;
    }
    private WaitResult<T> WaitResult<T, U, V>(ref WaitResult<T> completion, Action<U, V> action, U value1, V value2)
    {
        completion = completion.Delete();
        action(value1, value2);
        return completion;
    }
}

