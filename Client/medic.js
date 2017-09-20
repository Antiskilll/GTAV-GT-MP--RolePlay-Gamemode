var isComa = false;

API.onResourceStart.connect(function () {
    API.onUpdate.connect(medicalThread);
    API.onServerEventTrigger.connect(onServerEventTrigger);
    API.onKeyDown.connect(onKeyDownPress);
});

function medicalThread() {
    
    if (API.getEntitySyncedData(API.getLocalPlayer(), "IsOnComa") == true) {
        API.sendNotification("Appuyer sur la touche ~r~X ~s~ pour contacter les secours, sinon ~r~D ~s~pour en finir...");
        API.playScreenEffect("DeathFailOut", 1, false);
        sc = API.requestScaleform('MP_BIG_MESSAGE_FREEMODE')
        sc.CallFunction("SHOW_SHARD_WASTED_MP_MESSAGE", "Vous êtes dans le Coma!")
        API.renderScaleform(sc, 0, 0, 1280, 720)
    }
};

function onServerEventTrigger(name, args) {
    if (name == "MakeInComa") {
        onComaPlayer();
    }

    if (name == "MakeOutComa") {
        API.setActiveCamera(null);
    }
};

function onComaPlayer() {

    var newCamera = API.createCamera(API.getEntityPosition(API.getLocalPlayer()), API.getEntityRotation(API.getLocalPlayer()));
    API.setActiveCamera(newCamera);
    API.setCameraShake(newCamera, "DEATH_FAIL_IN_EFFECT_SHAKE", 1);  
}

function onKeyDownPress(sender, args) {
    if (API.getEntitySyncedData(API.getLocalPlayer(), "IsOnComa") == true) {
        if (args.KeyCode == Keys.D) {
            API.triggerServerEvent("suicideEvent");
        }
        else if (args.KeyCode == Keys.X) {
            API.triggerServerEvent("callmedic");
        }
    }
}