var YandexPlugin =
{
    IsInitializeJS: function () { return func.isInitialize(); },
    IsPlayerJS: function () { return func.isPlayer(); },
    IsLogOnJS: function () { return func.isLogOn(); },
    IsDesktopJS: function () { return vars.ysdk.deviceInfo.isDesktop(); },
    IsMobileJS: function () { return vars.ysdk.deviceInfo.isMobile() || vars.ysdk.deviceInfo.isTablet(); },

    GetLangJS: function () {
        var lang;
        if (func.isInitialize())
            lang = vars.ysdk.environment.i18n.lang;
        else
            lang = '';

        return func.toUnityString(lang);
    },
    GetPlayerNameJS: function () {
        var name;
        if (func.isLogOn())
            name = vars.player.getName();
        else
            name = '';

        return func.toUnityString(name);
    },
    GetPlayerAvatarURLJS: function (size) {
        var sSize = UTF8ToString(size);
        var url;
        if (func.isLogOn())
            url = vars.player.getPhoto(sSize);
        else
            url = '';

        return func.toUnityString(url);
    },

    InitYsdkJS: function () {

        if (typeof YaGames === 'undefined' || func.isEmpty(YaGames))
        {
            console.log("-- InitYsdk: not YaGames --");
            window.unityInstance.SendMessage('YandexSDK', 'OnEndInitYsdk', 0);
            return;
        }

        YaGames
            .init()
            .then((_ysdk) => {
                console.log("++ Yandex SDK initialized ++");
                vars.ysdk = _ysdk;
                window.unityInstance.SendMessage('YandexSDK', 'OnEndInitYsdk', 1);
                
            })
            .catch((message) => {
                console.log("-- InitYsdk: " + message + "  --");
                window.unityInstance.SendMessage('YandexSDK', 'OnEndInitYsdk', 0);
            });
    },

    ReadyJS: function () {

         if (!func.isInitialize() || !func.isEmpty(vars.ysdk.features.LoadingAPI)) {
             console.log("-- ReadyJS: not LoadingAPI  --");
            return;
         }

         vars.ysdk.features.LoadingAPI.ready(); 
    },

    InitPlayerJS: function () {

        if (!func.isInitialize()) {
            console.log("-- InitPlayer: not ysdk --");
            window.unityInstance.SendMessage('YandexSDK', 'OnEndInitPlayer', 0);
            return;
        }

        vars.ysdk
            .getPlayer()
            .then(_player => {
                vars.player = _player;
                console.log("++ InitPlayer ++");
                window.unityInstance.SendMessage('YandexSDK', 'OnEndInitPlayer', 1);
            })
            .catch(err => {
                vars.player = null;
                console.log("-- InitPlayer: " + err + "  --");
                window.unityInstance.SendMessage('YandexSDK', 'OnEndInitPlayer', 0);
            });
    },

    LogOnJS: function () {
        if (!func.isPlayer()) {
            console.log("-- Authorization: not player --");
            window.unityInstance.SendMessage('YandexSDK', 'OnEndLogOn', 0);
            return;
        }

        if (vars.player.getMode() === 'lite') {
            // игрок не авторизован.
            vars.ysdk.auth
                .openAuthDialog()
                .then(() => {
                    // игрок успешно авторизован
                    vars.ysdk
                        .getPlayer()
                        .then((_player) => {
                            vars.player = _player;
                            // успешная инициализации объекта player.
                            console.log("++ Authorization | Player ++");
                            window.unityInstance.SendMessage('YandexSDK', 'OnEndLogOn', 1);
                        })
                        .catch((message) => {
                            // ошибка при инициализации объекта player.
                            vars.player = null;
                            console.log("-- Authorization | Player: " + message + "  --");
                            window.unityInstance.SendMessage('YandexSDK', 'OnEndLogOn', 0);
                        });
                })
                .catch((message) => {
                    // игрок не авторизован.
                    console.log("-- Authorization: " + message + "  --");
                    window.unityInstance.SendMessage('YandexSDK', 'OnEndLogOn', 0);
                });
        }
        else {
            // игрок уже авторизован
            console.log("++ уже Authorization ++");
            window.unityInstance.SendMessage('YandexSDK', 'OnEndLogOn', 1);
        }

    },

    SaveJS: function (key, data) {
        if (!func.isPlayer()) {
            console.log("-- SaveYSDK: not player --");
            window.unityInstance.SendMessage('YandexSDK', 'OnEndSave', 0);
            return;
        }

        var sKey = UTF8ToString(key);
        var dt = UTF8ToString(data);
        dt = zip(dt);
        var obj = { [sKey]: dt };
        vars.player
            .setData(obj)
            .then(() => {
                console.log("++ SaveYSDK ++");
                window.unityInstance.SendMessage('YandexSDK', 'OnEndSave', 1);
            })
            .catch((message) => {
                console.log("-- SaveYSDK: " + message + "  --");
                window.unityInstance.SendMessage('YandexSDK', 'OnEndSave', 0);
            });
    },
    LoadJS: function (key) {
        if (!func.isPlayer()) {
            console.log("-- LoadYSDK: not player --");
            window.unityInstance.SendMessage('YandexSDK', 'OnEndLoad', '');
            return;
        }

        var sKey = UTF8ToString(key);
        var keys = [sKey];
        vars.player
            .getData(keys)
            .then((data) => {
                if (func.isEmpty(data)) {
                    console.log("++ LoadYSDK: NULL ++");
                    window.unityInstance.SendMessage('YandexSDK', 'OnEndLoad', '');
                }
                else {
                    console.log("++ LoadYSDK ++");
                    var dt = data[sKey];
                    dt = unzip(dt);
                    window.unityInstance.SendMessage('YandexSDK', 'OnEndLoad', dt);
                }

            })
            .catch((message) => {
                console.log("-- LoadYSDK: " + message + "  --");
                window.unityInstance.SendMessage('YandexSDK', 'OnEndLoad', '');
            });

    },
 

    ShowBannerAdvJS: function () {
        if (!func.isInitialize()) {
            console.log("-- ShowBannerAdv: not ysdk --");
            return;
        }

        vars.ysdk.adv.showBannerAdv();
    },

    HideBannerAdvJS: function () {
        if (!func.isInitialize()) {
            console.log("-- HideBannerAdv: not ysdk --");
            return;
        }

        vars.ysdk.adv.hideBannerAdv();
    },

    $func: {
        toUnityString: function (returnStr) {
            if (func.isEmpty(returnStr))
                return null;

            var bufferSize = lengthBytesUTF8(returnStr) + 1;
            var buffer = _malloc(bufferSize);
            stringToUTF8(returnStr, buffer, bufferSize);
            return buffer;
        },
        isInitialize: function () { return !func.isEmpty(vars.ysdk); },
        isPlayer: function () { return func.isInitialize() && !func.isEmpty(vars.player); },
        isLogOn: function () { return func.isPlayer() && !(vars.player.getMode() === 'lite'); },

        isEmpty: function (obj) {
            if (!obj)
                return true;

            return Object.keys(obj).length === 0;
        },
    },
    $vars:
    {
        ysdk: null,
        player: null,
    },
}
    
autoAddDeps(YandexPlugin, '$func');
autoAddDeps(YandexPlugin, '$vars');
mergeInto(LibraryManager.library, YandexPlugin);

