
var menu = API.createMenu("Cellule", "Quelle cellule voulez-vous verrouillez:", 0, 0, 6, true);
var C1 = API.createCheckboxItem("Cellule 1", "Vérrouillez la porte de cellule n°1", false);
var C2 = API.createCheckboxItem("Cellule 2", "Vérrouillez la porte de cellule n°2", false);
var C3 = API.createCheckboxItem("Cellule 3", "Vérrouillez la porte de cellule n°3", false);

menu.AddItem(C1);
menu.AddItem(C2);
menu.AddItem(C3);

C1.CheckboxEvent.connect(function (sender) {
    API.triggerServerEvent("CellManager", 1);
});

C2.CheckboxEvent.connect(function (sender) {
    API.triggerServerEvent("CellManager", 2);
});

C3.CheckboxEvent.connect(function (sender) {
    API.triggerServerEvent("CellManager", 3);
});

API.onResourceStart.connect(function () {
    API.onServerEventTrigger.connect(onServerEventTrigger);
});

function onServerEventTrigger(name, args) {
    if (name == "OpenCeilManager") {
        C1.Checked = args[0];
        C2.Checked = args[1];
        C3.Checked = args[2];
        API.showCursor(false);
        menu.Visible = true;
    }
};

API.onUpdate.connect(function () {
    API.drawMenu(menu);
});