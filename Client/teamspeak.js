var vcBrowser = null;
var lgBrowser = null;
var plBrowser = null;
var oldDateTime = 0;
var refresh = false;
var local_tsid = "";
var onKeyDown = true;
var onKeyUp = true;
var uri = "http://localhost:25984";

API.onResourceStart.connect(function () {
    lgBrowser = API.createCefBrowser(0, 0, false); API.setCefBrowserPosition(lgBrowser, 0, 0); API.waitUntilCefBrowserInit(lgBrowser);
    vcBrowser = API.createCefBrowser(0, 0, false); API.setCefBrowserPosition(vcBrowser, 0, 0); API.waitUntilCefBrowserInit(vcBrowser);
    plBrowser = API.createCefBrowser(0, 0, false); API.setCefBrowserPosition(plBrowser, 0, 0); API.waitUntilCefBrowserInit(plBrowser);
});

API.onServerEventTrigger.connect(function (name, args) {
    if (name == "Teamspeak_Connect") {
        local_tsid = API.getEntitySyncedData(API.getLocalPlayer(), "Nom_Prenom");
        API.loadPageCefBrowser(lgBrowser, uri + "/login/" + local_tsid + "/");
        refresh = true;
    }
    else if (name == "updateTeamspeak") {
        if (!API.isCefBrowserLoading(plBrowser) && refresh && API.getEntitySyncedData(API.getLocalPlayer(), "LOGGED_IN")) {
            if (playerNames == "0") {
                API.loadPageCefBrowser(plBrowser, uri + "/players/0/");
            } else {
                var playerNames = new Array();
                playerNames.push(args[0]);
                API.loadPageCefBrowser(plBrowser, uri + "/players/" + playerNames.join(";") + "/");
                var playerNames = new Array();
            }

        }
    }
});

API.onKeyDown.connect(function (sender, e) {
    if (!refresh) return;
    if (!onKeyDown) return;
    if (e.KeyCode === Keys.CapsLock) {
        onKeyDown = false;
        onKeyUp = true;
        API.loadPageCefBrowser(vcBrowser, uri + "/vocal/" + local_tsid + ";true/");
        API.triggerServerEvent("Speak", 1);
    } else if (e.KeyCode === Keys.M) {
        onKeyDown = false;
        onKeyUp = true;
        var voice = API.getEntitySyncedData(API.getLocalPlayer(), "VOICE_RANGE");
        if (voice == "Parler") {
            API.sendNotification("~r~Vocal: ~s~Vous crier.");
            API.setEntitySyncedData(API.getLocalPlayer(), "VOICE_RANGE", "Crier");
        }
        else if (voice == "Crier") {
            API.sendNotification("~r~Vocal: ~s~Vous chuchoter.");
            API.setEntitySyncedData(API.getLocalPlayer(), "VOICE_RANGE", "Chuchoter");
        }
        else if (voice == "Chuchoter") {
            API.sendNotification("~r~Vocal: ~s~Vous parler.");
            API.setEntitySyncedData(API.getLocalPlayer(), "VOICE_RANGE", "Parler");
        }
    }
});

API.onKeyUp.connect(function (sender, e) {
    if (!refresh) return;
    if (e.KeyCode === Keys.CapsLock) {
        if (onKeyUp) {
            onKeyUp = false;
            onKeyDown = true;
            API.loadPageCefBrowser(vcBrowser, uri + "/vocal/" + local_tsid + ";false/");
            API.triggerServerEvent("Speak", 0);
        }
    }
});

API.onResourceStop.connect(function () {
    if (lgBrowser != null) API.destroyCefBrowser(lgBrowser);
    if (vcBrowser != null) API.destroyCefBrowser(vcBrowser);
    if (plBrowser != null) API.destroyCefBrowser(plBrowser);
});