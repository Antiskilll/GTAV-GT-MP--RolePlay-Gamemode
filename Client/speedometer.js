var res_X = API.getScreenResolution().Width;
var res_Y = API.getScreenResolution().Height;
var inVeh = false;
var seat = null;
var Veh = null;

API.onResourceStart.connect(function () {
    API.onPlayerEnterVehicle.connect(speedometerON);
    API.onPlayerExitVehicle.connect(speedometerOFF);
    API.onUpdate.connect(speedometer);
});

function speedometerON(veh) {
    inVeh = true;
    player = API.getLocalPlayer();
    seat = API.getPlayerVehicleSeat(player);
    Veh = API.getPlayerVehicle(player);
};

function speedometerOFF(veh) {
    inVeh = false;
    seat = null;
    Veh = null;
};

function speedometer() {
    if (inVeh && Veh != null) {
        var vel = API.getEntityVelocity(Veh);
        var health = API.getVehicleHealth(Veh);
        var maxhealth = API.returnNative("GET_ENTITY_MAX_HEALTH", 0, Veh);
        var healthpercent = Math.floor((health / maxhealth) * 100);
        var vehicleFuel = API.getEntitySyncedData(Veh, "VEHICLE_FUEL");
        var vehicleFuelMax = API.getEntitySyncedData(Veh, "VEHICLE_FUEL_MAX");
        var fuelPercent = Math.round(vehicleFuel * 100 / vehicleFuelMax);
        var speed = Math.sqrt(vel.X * vel.X +
            vel.Y * vel.Y +
            vel.Z * vel.Z);
        var displaySpeedkmph = Math.round(speed * 3.6);
        API.drawText("" + displaySpeedkmph, res_X - 100, res_Y - 200, 1, 255, 255, 255, 255, 4, 2, false, true, 0);
        API.drawText("km/h", res_X - 20, res_Y - 180, 0.5, 255, 255, 255, 255, 4, 2, false, true, 0);
        API.drawText("Santé:", res_X - 70, res_Y - 225, 0.5, 255, 255, 255, 255, 4, 2, false, true, 0);
        API.drawText("Carburant:", res_X - 70, res_Y - 260, 0.5, 255, 255, 255, 255, 4, 2, false, true, 0);
        API.drawText(healthpercent + "%", res_X - 20, res_Y - 225, 0.5, 255, 255, 255, 255, 4, 2, false, true, 0);
        API.drawText(fuelPercent + "%", res_X - 20, res_Y - 260, 0.5, 255, 255, 255, 255, 4, 2, false, true, 0);

        if (fuelPercent < 50) {
            API.drawText(fuelPercent + "%", res_X - 20, res_Y - 260, 0.5, 219, 122, 46, 255, 4, 2, false, true, 0);
        }
        else if (fuelPercent < 15) {
            API.drawText(fuelPercent + "%", res_X - 20, res_Y - 260, 0.5, 219, 46, 46, 255, 4, 2, false, true, 0);
        }

        if (healthpercent < 60) {
            API.drawText(healthpercent + "%", res_X - 20, res_Y - 225, 0.5, 219, 122, 46, 255, 4, 2, false, true, 0);
        }
        else if (healthpercent < 30) {
            API.drawText(healthpercent + "%", res_X - 20, res_Y - 225, 0.5, 219, 46, 46, 255, 4, 2, false, true, 0);
        }
    }
};
