using System;
using System.Collections;
using System.Collections.Generic;

public abstract class ASaveLoadJsonTo
{
    protected Dictionary<string, string> _saved = null;
    protected bool _dictModified = false;

    public abstract bool IsValid { get; }

    public abstract IEnumerator Initialize_Coroutine(string key, Action<bool> callback);

    public virtual Return<T> Load<T>(string key) where T : class
    {
        if (_saved.TryGetValue(key, out string json))
            return Deserialize<T>(json);

        return Return<T>.Empty;
    }

    public abstract IEnumerator Save_Coroutine(string key, object data, Action<bool> callback);
    protected virtual bool SaveToMemory(string key, object data)
    {
        try
        {
            string json = Serialize(data);
            _saved.TryGetValue(key, out string saveJson);
            if (saveJson == null || saveJson != json)
            {
                _saved[key] = json;
                _dictModified = true;
            }
            return true;
        }
        catch (Exception ex)
        {
            Message.Log(ex.Message);
        }

        return false;
    }

    protected string Serialize(object obj) => Storage.Serialize(obj);
    protected Return<T> Deserialize<T>(string json) where T : class => Storage.Deserialize<T>(json);
}
