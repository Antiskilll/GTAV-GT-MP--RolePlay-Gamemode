var rpmList = [];
var nextCheck = 0;
var isinvehicle = false;
var seat = 0;
var statusStarted = false;
var vehicleplayer = null;
var checkEngineOn = true;
var currentFuel = 0;

API.onResourceStart.connect(function () {
    API.verifyIntegrityOfCache();
    API.sendNotification("Bienvenue sur le serveur GTAV ~r~AdAstra Gaming~s~.");
    API.onServerEventTrigger.connect(onServerEventTrigger);
    API.onPlayerEnterVehicle.connect(playerEnterVehicle);
    API.onPlayerExitVehicle.connect(playerExitVehicle);
    API.onUpdate.connect(onUpdateVehicle);
});

function playerEnterVehicle(veh) {
    isinvehicle = true;
    var player = API.getLocalPlayer();
    vehicleplayer = veh;
    seat = API.getPlayerVehicleSeat(player);
    var currentFuel = API.getEntitySyncedData(veh, "VEHICLE_FUEL");
    if (currentFuel <= 0) {
        API.setVehicleEngineStatus(veh, false);
    }
}

function playerExitVehicle(veh) {
    isinvehicle = false;
    vehicleplayer = null;
    API.setVehicleEngineStatus(veh, false);
}

function getVehiclePetrolTankHealth(veh) {
    var returnNative = API.callNative("GET_VEHICLE_PETROL_TANK_HEALTH", 7, veh);
    return returnNative;
}



function onUpdateVehicle() {
    if (isinvehicle) {
        if (seat === -1 && vehicleplayer != null) {

            if (!API.getVehicleEngineStatus(vehicleplayer)) {
                var currentFuel = API.getEntitySyncedData(vehicleplayer, "VEHICLE_FUEL");
                if (currentFuel >= 0.5) {
                    API.setVehicleEngineStatus(vehicleplayer, true);
                }
            }

            // Fuel System
            if (nextCheck <= (+ new Date())) {
                //Calcul system by DestinyRP 
                var sum = 0;
                for (var i = 0; i < rpmList.length; i++) {
                    sum += rpmList[i];
                }
                var average = sum / rpmList.length;
                if (isNaN(average)) {
                    average = 0;
                } else {
                    currentFuel = API.getEntitySyncedData(vehicleplayer, "VEHICLE_FUEL");
                    var newfuel = (currentFuel - (average / 20));
                    var newfuel = Math.round(newfuel * 1e3) / 1e3;
                    API.setEntitySyncedData(vehicleplayer, "VEHICLE_FUEL", newfuel);
                }

                if (currentFuel <= 2 && currentFuel > 0.5) {
                    if (checkEngineOn) {
                        API.setVehicleEngineStatus(vehicleplayer, false);
                        checkEngineOn = false;
                    } else {
                        API.setVehicleEngineStatus(vehicleplayer, true);
                        checkEngineOn = true;
                    }
                }

                if (newfuel <= 0.5) {
                    API.setVehicleEngineStatus(vehicleplayer, false);
                }

                rpmList = [];
                nextCheck = (+ new Date()) + 5000;
            } else {
                rpmList[rpmList.length] = API.getVehicleRPM(vehicleplayer);
            }
        }
    }
}
function onServerEventTrigger(name, args) {
    if (name == "StartStatus") {
        player = API.getLocalPlayer();
        statusStarted = true;
        if (API.getEntitySyncedData(player, "adminRank") < 1) {
            API.setChatVisible(false);
        }
    }
    else if (name === "smokeweedeveryday") {
        var pos = API.getGameplayCamPos();
        var rot = API.getGameplayCamRot();
        var drunkcam = API.createCamera(pos, rot);
        var player = API.getLocalPlayer();
        //API.playPlayerScenario(player, "world_human_drug_dealer");
        API.setActiveCamera(drunkcam);
        API.setCameraShake(drunkcam, "DRUNK_SHAKE", 2);
        API.sleep(Number(args[0]));
        API.stopCameraShake(drunkcam);
        API.setActiveCamera(null);
    }
    else if (name === "merrychristmas") {
        API.setSnowEnabled(true, true, true);
    }
};

API.onResourceStop.connect(function () {
    API.sendNotification("Vous êtes déconnecté du serveur GTAV ~r~AdAstra Gaming~s~.");
});
