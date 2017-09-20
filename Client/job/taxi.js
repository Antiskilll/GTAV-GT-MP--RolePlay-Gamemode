var isTaxiFare = false;
var isCustomer = false;
var currentToPay = 0;
var currentFare = 0;
API.onServerEventTrigger.connect(function (eventName, args) {
    if (eventName == "update_taxi_fare") {
        isTaxiFare = args[0];
        currentFare = args[1];
        currentToPay = args[2];
    }
});
var res_X = API.getScreenResolution().Width;
var res_Y = API.getScreenResolution().Height;
var taxiFare = "Tarif:";
var taxiFareInfo = "Prix chaque kilometre:";
var dollar = "$";
var taxiCustomerInfo = "Paiement";
var taxiCustomerAsk = "Vous allez payer:";
API.onUpdate.connect(function (sender, args) {
    if (isTaxiFare) {
        API.drawText(taxiFare, res_X - 10, res_Y - 500, 0.6, 115, 186, 131, 255, 4, 2, false, true, 0);
        API.drawText(taxiFareInfo, res_X - 10, res_Y - 460, 0.4, 255, 255, 255, 255, 4, 2, false, true, 0);
        API.drawText(dollar + ("" + currentFare), res_X - 10, res_Y - 420, 0.7, 255, 255, 255, 255, 4, 2, false, true, 0);
        API.drawText(taxiCustomerInfo, res_X - 10, res_Y - 380, 0.4, 255, 255, 255, 255, 4, 2, false, true, 0);
        API.drawText(dollar + ("" + currentToPay), res_X - 10, res_Y - 340, 0.7, 255, 255, 255, 255, 4, 2, false, true, 0);
    }
});
//# sourceMappingURL=taxi.js.map 
//# sourceMappingURL=taxi.js.map 
//# sourceMappingURL=taxi.js.map 
//# sourceMappingURL=taxi.js.map 
//# sourceMappingURL=taxi.js.map