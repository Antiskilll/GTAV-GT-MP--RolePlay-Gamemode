var res_X = API.getScreenResolution().Width;
var res_Y = API.getScreenResolution().Height;
var lastCheck = new Date().getTime();
var statusStarted = false;
var isComa = false;
var player = null;
var nextHungerCheck = 0;
var nextThristCheck = 0;
var hunger = 100;
var drink = 100;

API.onResourceStart.connect(function () {
    API.onServerEventTrigger.connect(ServerEventTrigger);
    API.onUpdate.connect(onUpdateHunger);
    API.onUpdate.connect(onUpdateThrist);
    API.onUpdate.connect(onUpdateHUD);

});

function onUpdateThrist() {
    if (nextThristCheck <= (+new Date()) && statusStarted && !(API.getEntitySyncedData(player, "IsOnComa"))) {
        drink = API.getEntitySyncedData(player, "PLAYER_THIRSTY");
        API.setEntitySyncedData(player, "PLAYER_THIRSTY", (drink - 1));
        if (drink <= 0 && drink != null) {
            API.sendNotification("Vous êtes mort de soif.");
            API.setPlayerHealth(-1);
        }
        nextThristCheck = (+new Date()) + 44000;
    } 
};

function onUpdateHunger() {
    if (nextHungerCheck <= (+new Date()) && statusStarted && !(API.getEntitySyncedData(player, "IsOnComa"))) {
        hunger = API.getEntitySyncedData(player, "PLAYER_HUNGRY");
        API.setEntitySyncedData(player, "PLAYER_HUNGRY", (hunger - 1));
        if (hunger <= 0 && hunger != null) {
            API.sendNotification("Vous êtes mort de faim.");
            API.setPlayerHealth(-1);
        }
        nextHungerCheck = (+new Date()) + 72000;
    }
};

function ServerEventTrigger(name, args) {
    if (name == "StartStatus") {
        player = API.getLocalPlayer();
        drink = API.getEntitySyncedData(player, "PLAYER_THIRSTY");
        hunger = API.getEntitySyncedData(player, "PLAYER_HUNGRY");
        statusStarted = true;
    }
    if (name == "UpdateSurvival") {
        drink = API.getEntitySyncedData(player, "PLAYER_THIRSTY");
        hunger = API.getEntitySyncedData(player, "PLAYER_HUNGRY");
    }
};


function onUpdateHUD() {
    if (!API.getHudVisible()) return;
    player = API.getLocalPlayer();
    var health = API.getPlayerHealth(player);
    var maxhealth = API.returnNative("GET_ENTITY_MAX_HEALTH", 0, player);
    var talk = API.getEntitySyncedData(player, "VOICE_RANGE");
    var healthpercent = Math.floor((health / maxhealth) * 200);
    var cash = API.getEntitySyncedData(player, "Money");
    //API.drawText("VIE:", res_X - 55, res_Y - 580, 0.5, 255, 255, 255, 255, 4, 2, false, true, 0);
    //API.drawText("FAIM:", res_X - 55, res_Y - 550, 0.5, 255, 255, 255, 255, 4, 2, false, true, 0);
    //API.drawText("SOIF:", res_X - 55, res_Y - 520, 0.5, 255, 255, 255, 255, 4, 2, false, true, 0);

    API.dxDrawTexture("/Client/hud/coeur.png", new Point(res_X - 120, res_Y - 960), new Size(40, 40), 1);
    API.dxDrawTexture("/Client/hud/faim.png", new Point(res_X - 115, res_Y - 910), new Size(30, 30), 1);
    API.dxDrawTexture("/Client/hud/soda.png", new Point(res_X - 118, res_Y - 865), new Size(35, 35), 1);


    API.drawText("$ " + cash, res_X - 19, 50, 0.6, 114, 204, 114, 255, 7, 2, false, true, 0);


    if (talk == "Chuchoter") {
        API.dxDrawTexture("/Client/hud/mic1.png", new Point(res_X - 90, res_Y - 800), new Size(40, 60), 1);
    }
    else if (talk == "Parler") {
        API.dxDrawTexture("/Client/hud/mic2.png", new Point(res_X - 90, res_Y - 800), new Size(40, 60), 1);
    }
    else if (talk == "Crier") {
        API.dxDrawTexture("/Client/hud/mic3.png", new Point(res_X - 90, res_Y - 800), new Size(40, 60), 1);
    }

    //FAIM
    if (hunger <= 100) {
        API.drawText(hunger + "%", res_X - 5, res_Y - 908, 0.45, 255, 255, 255, 255, 7, 2, false, true, 0);
    }
    else if (hunger <= 60) {
        API.drawText(hunger + "%", res_X - 5, res_Y - 908, 0.45, 219, 122, 46, 255, 7, 2, false, true, 0);
    }
    else if (hunger <= 30) {
        API.drawText(hunger + "%", res_X - 5, res_Y - 908, 0.45, 219, 46, 46, 255, 7, 2, false, true, 0);
    }

    //Soif
    if (drink <= 100) {
        API.drawText(drink + "%", res_X - 5, res_Y - 860, 0.45, 255, 255, 255, 255, 7, 2, false, true, 0);
    }
    else if (drink <= 60) {
        API.drawText(drink + "%", res_X - 5, res_Y - 840, 0.45, 219, 122, 46, 255, 7, 2, false, true, 0);
    }
    else if (drink <= 30) {
        API.drawText(drink + "%", res_X - 5, res_Y - 840, 0.45, 219, 46, 46, 255, 7, 2, false, true, 0);
    }

    //VIE
    if (healthpercent <= 100) {
        API.drawText(healthpercent + "%", res_X - 5, res_Y - 955, 0.45, 255, 255, 255, 255, 7, 2, false, true, 0);
    }
    else if (healthpercent <= 60) {
        API.drawText(healthpercent + "%", res_X - 5, res_Y - 945, 0.45, 219, 122, 46, 255, 7, 2, false, true, 0);
    }
    else if (healthpercent <= 30) {
        API.drawText(healthpercent + "%", res_X - 5, res_Y - 945, 0.45, 219, 46, 46, 255, 7, 2, false, true, 0);
    }


};
