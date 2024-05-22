using System;

public partial class YandexSDK
{
    private WaitResult<bool> _waitEndInitYsdk = new();
    private WaitResult<bool> _waitEndInitPlayer = new();
    private WaitResult<bool> _waitEndLogOn = new();
    private WaitResult<bool> _waitEndSave = new();
    private WaitResult<string> _waitEndLoad = new();

    public void OnEndInitYsdk(int result) => _waitEndInitYsdk.SetResult(Convert.ToBoolean(result));
    public void OnEndInitPlayer(int result) => _waitEndInitPlayer.SetResult(Convert.ToBoolean(result));
    public void OnEndLogOn(int result) => _waitEndLogOn.SetResult(Convert.ToBoolean(result));
    public void OnEndSave(int result) => _waitEndSave.SetResult(Convert.ToBoolean(result));
    public void OnEndLoad(string value) => _waitEndLoad.SetResult(value);
}
