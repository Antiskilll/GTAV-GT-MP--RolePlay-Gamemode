var localPlayer = null;
var lastCheck = new Date().getTime();
var lookingAtEntity = null;
var lookingAtPosition = null;
var modelname = null;
var textlabel = null;
var detectbool = false;
var atmdetect = false;
var pumpdetect = false;
var onKeyDown = true;
var onKeyUp = true;

API.onResourceStart.connect(() => {
    API.onServerEventTrigger.connect(ServerEventTrigger);
    API.onUpdate.connect(HandleRayTracing);
    API.onKeyDown.connect(onKeyDownPress);
});

function ServerEventTrigger(name, args) {
    if (name == "StartStatus") {
        localPlayer = API.getLocalPlayer();
    }
}

function HandleRayTracing() {
    if (localPlayer == null) { return; }
    var currentTime = new Date().getTime();
    if (currentTime - lastCheck > 100) {
        if (API.isPlayerInAnyVehicle(localPlayer)) { return; }
        lookingAtEntity = GetlookingAtEntity();
        lookingAtPosition = lookingAtEntity != null ? API.getEntityPosition(lookingAtEntity) : null;
    }
    if (lookingAtEntity != null || lookingAtPosition != null) {
        var modellooking = API.getEntityModel(lookingAtEntity);
        if ((AtmModels.indexOf(modellooking) != -1)) {
            modelname = "le distributeur.";
            lookingAtPosition.Z += 1;
            lookingAtPosition.Y += 1;
            atmdetect = true;
        }
        if ((GasPumpModels.indexOf(modellooking) != -1)) {
            modelname = "la pompe à essence.";
            lookingAtPosition.Z += 1;
            pumpdetect = true;
        }

        if (!detectbool) {
            detectbool = true;
            textlabel = API.createTextLabel("Appuyer sur la touche ~r~E ~s~pour utiliser " + modelname, lookingAtPosition, 35, 0.5, true);

        }
    } else {
        if (detectbool) {
            API.deleteEntity(textlabel);
            textlabel = null;
            detectbool = false;
            pumpdetect = false;
            atmdetect = false;
        }
    }
}

function onKeyDownPress(sender, args) {
    if (localPlayer == null) { return; }
    if (API.getEntitySyncedData(localPlayer, "LOGGED_IN") == true && onKeyDown) {
        onKeyDown = false;
        onKeyUp = true;

        // Menu Admin
        if (args.KeyCode == Keys.F1) {
            API.triggerServerEvent("onKeyDown", 0);
        }
        // Menu Telephone
        else if (args.KeyCode == Keys.O) {
            API.triggerServerEvent("onKeyDown", 1);
        }
        // Menu Joueur
        else if (args.KeyCode == Keys.Y) {
            API.triggerServerEvent("onKeyDown", 2);
        }
        // Lock Unlock Vehicle
        else if (args.KeyCode == Keys.U) {
            API.triggerServerEvent("onKeyDown", 4);
        }
        // Menu Vehicle
        else if (args.KeyCode == Keys.I) {
            API.triggerServerEvent("onKeyDown", 5);
        }
        // Interaction Key
        else if (args.KeyCode == Keys.E) {
            if (pumpdetect) {
                API.triggerServerEvent("USE_PUMP", lookingAtPosition);
            }
            else if (atmdetect) {
                API.triggerServerEvent("ATM");
            } else {
                API.triggerServerEvent("onKeyDown", 6);
            }
        }
        else if (args.KeyCode == Keys.Enter) {
            API.triggerServerEvent("onKeyDown", 7);
        }
        // ANIMATION
        //if (args.LControlKey) {
           
        else if (args.KeyCode == Keys.NumPad1) {
            API.triggerServerEvent("AnimationSystem", "mp_player_intfinger", "mp_player_int_finger", 0);
        }
        else if (args.KeyCode == Keys.NumPad2) {
            API.triggerServerEvent("AnimationSystem", "anim@mp_player_intcelebrationmale@salute", "salute", 0);
        }
        else if (args.KeyCode == Keys.NumPad3) {
            API.triggerServerEvent("AnimationSystem", "anim@mp_player_intcelebrationmale@surrender", "surrender", 0);
        }
        else if (args.KeyCode == Keys.NumPad4) {
            API.triggerServerEvent("AnimationSystem", "anim@mp_player_intcelebrationmale@thumb_on_ears", "thumb_on_ears", 0);
        }
        else if (args.KeyCode == Keys.NumPad5) {
            API.triggerServerEvent("AnimationSystem", "anim@mp_player_intcelebrationmale@air_shagging", "air_shagging", 0);
        }
        else if (args.KeyCode == Keys.NumPad6) {
            API.triggerServerEvent("AnimationSystem", "anim@mp_player_intcelebrationmale@chin_brush", "chin_brush", 0);
        }
        else if (args.KeyCode == Keys.NumPad7) {
            API.triggerServerEvent("AnimationSystem", "anim@mp_player_intcelebrationmale@shush", "shush", 0);
        }
        else if (args.KeyCode == Keys.NumPad8) {
            API.triggerServerEvent("AnimationSystem", "anim@mp_player_intcelebrationmale@slow_clap", "slow_clap", 0);
        }
        else if (args.KeyCode == Keys.NumPad9) {
            API.triggerServerEvent("AnimationSystem", "anim@mp_player_intcelebrationmale@peace", "peace", 0);
        }
        else if (args.KeyCode == Keys.NumPad0) {
            API.triggerServerEvent("AnimationSystem", "missminuteman_1ig_2 handsup_base", "Main", 0);
        }


        //}
    }
}

API.onKeyUp.connect(function (sender, e) {
    if (onKeyUp) {
        onKeyUp = false;
        onKeyDown = true;
    }
});


function getAimPoint() {
    var resolution = API.getScreenResolution();
    return API.screenToWorld(new PointF(resolution.Width / 2, resolution.Height / 2));
}

function Vector3Lerp(start, end, fraction) {
    return new Vector3(
        (start.X + (end.X - start.X) * fraction),
        (start.Y + (end.Y - start.Y) * fraction),
        (start.Z + (end.Z - start.Z) * fraction)
    );
}

var AtmModels = [
    -1126237515,
    -1364697528,
    506770882,
    -870868698
];

var GasPumpModels = [
    1339433404,
    1933174915,
    -2007231801,
    -462817101,
    -469694731,
    1694452750
];

function GetlookingAtEntity() {
    var startPoint = API.getGameplayCamPos();
    var aimPoint = getAimPoint();

    startPoint.Add(aimPoint);

    var endPoint = Vector3Lerp(startPoint, aimPoint, 1.1);
    var rayCast = API.createRaycast(startPoint, endPoint, 16, null);

    if (!rayCast.didHitEntity) return null;

    var hitEntityHandle = rayCast.hitEntity;
    var entityModel = API.getEntityModel(hitEntityHandle);

    if (AtmModels.indexOf(entityModel) == -1 && GasPumpModels.indexOf(entityModel) == -1) return null;

    if (API.getEntityPosition(hitEntityHandle).DistanceTo(API.getEntityPosition(API.getLocalPlayer())) > 4) return null;

    return hitEntityHandle;
}
