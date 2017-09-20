var mark = null;
var blip = null;
API.onServerEventTrigger.connect(function (name, args) {
    if (name == "markerblip") {
        if (blip != null) {
            if (API.doesEntityExist(mark))
                API.deleteEntity(mark);
            if (API.doesEntityExist(blip))
                API.deleteEntity(blip);
        }
        var position = args[0];
        mark = API.createMarker(1, position, new Vector3(), new Vector3(), new Vector3(1, 1, 1), 255, 255, 0, 100);
        blip = API.createBlip(position);
        API.setBlipColor(blip, 1);
    }
    if (name == "removemarkerblip") {
        if (blip != null) {
            if (API.doesEntityExist(mark))
                API.deleteEntity(mark);
            if (API.doesEntityExist(blip))
                API.deleteEntity(blip);
        }
    }
});
//# sourceMappingURL=BlipMarkerManager.js.map 
//# sourceMappingURL=BlipMarkerManager.js.map 
//# sourceMappingURL=BlipMarkerManager.js.map 
//# sourceMappingURL=BlipMarkerManager.js.map 
//# sourceMappingURL=BlipMarkerManager.js.map