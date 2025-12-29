using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPreGame : MonoBehaviour
{
    [SerializeField] private string _keySave = "LAJ";
    [Space]
    [SerializeField, Scene] private int _sceneDesktop = 0;
    [SerializeField, Scene] private int _sceneMobile = 0;
    [Space]
    [SerializeField] private Slider _slider;

    private void Start()
    {
        Message.Log("Start LoadingPreGame");

        Random.InitState((int)(System.DateTime.Now.Ticks - System.DateTime.UnixEpoch.Ticks));

        var settings = SettingsGame.InstanceF;
		settings.SetPlatform();

		var loadScene = new LoadScene(settings.IsDesktop ? _sceneDesktop : _sceneMobile, _slider, this);

		var localization = Localization.InstanceF;
		if (!localization.Initialize())
			Message.Error("Error loading Localization!");

        Banners.InstanceF.Initialize();

        if (!Storage.StoragesCreate())
            Message.Banner(localization.GetText("ErrorStorage"), MessageType.Error, 7000);

        bool isLoad = Storage.Initialize(_keySave);

        if (isLoad)
            Message.Log("Storage initialize");
        else
            Message.Log("Storage not initialize");

        var dataGame = DataGame.InstanceF;
        dataGame.IsFirstStart = !(settings.Initialize(isLoad) | dataGame.Initialize(isLoad));

        Message.Log("End LoadingPreGame");
        loadScene.End();
    }
}
