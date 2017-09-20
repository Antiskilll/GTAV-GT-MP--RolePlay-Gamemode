var pool = null;
var text = null;

API.onServerEventTrigger.connect(function (name, args) {

    if (name == "lspdservice") {
        pool = API.getMenuPool();
        var callback = args[0];
        var banner = args[1];
        var subtitle = args[2];
        var menu = null;
        var skin = null;

        menu = API.createMenu(banner, subtitle, 0, 0, 6);

        var Nomliste = args[3];
        var ListeItem = args[4];
        let menuListe = new Array(Nomliste.Count);
        //Listes
        for (var i = 0; i < Nomliste.Count; i++) {
            var liste = new List(String);
            for (var j = 0; j < ListeItem[i].Count; j++) {
                if (ListeItem[i][j] != null) {
                    liste.Add(ListeItem[i][j]);
                }
                
            }
            menuListe[i] = API.createListItem(Nomliste[i], "test", liste, 0);
            menu.AddItem(menuListe[i]);

        }
        let Selection = new Array(Nomliste.Count);
        Selection[0] = ListeItem[0][0];
        var taille = Nomliste.Count;
        if (taille > 0) {
            Selection[0] = ListeItem[0][0];
            menuListe[0].OnListChanged.connect(function (sender, new_index) {
                switch (new_index) {
                    case new_index:
                        {
                            Selection[0] = ListeItem[0][new_index];
                            break;
                        }
                }
            });
        }
        //Items
        var items = args[5]
        for (var i = 0; i < items.Count; i++) {
            var listItem = API.createMenuItem(items[i], "");
            menu.AddItem(listItem);
        }

        API.drawMenu(menu);
        menu.Visible = true;

        menu.RefreshIndex();
        pool.Add(menu);

        menu.OnItemSelect.connect(function (sender, item, index) {
            API.triggerServerEvent("menu_handler_select_item", callback, index, Selection[0]);
            pool = null;
        });
    }
    if (name == "jailmenu") {
        pool = API.getMenuPool();
        var callback = args[0];
        var banner = args[1];
        var subtitle = args[2];
        var menu = null;
        var skin = null;

        menu = API.createMenu(banner, subtitle, 0, 0, 6);
        
        var Nomliste = args[3];
        var ListeItem = args[4];
        let menuListe = new Array(Nomliste.Count);
        //Listes
        for (var i = 0; i < Nomliste.Count; i++) {
            var liste = new List(String);
            for (var j = 0; j < ListeItem[i].Count; j++) {
                liste.Add(ListeItem[i][j]);
            }
            menuListe[i] = API.createListItem(Nomliste[i], "", liste, 0);
            menu.AddItem(menuListe[i]);
        }
        

        let Selection = new Array(Nomliste.Count);
        Selection[0] == ListeItem[0][0];
        menuListe[0].OnListChanged.connect(function (sender, new_index) {
                switch (new_index) {
                    case new_index:
                        {
                            Selection[0] = ListeItem[0][new_index];
                            break;
                        }
                }
        });
        Selection[1] = ListeItem[1][0];
        menuListe[1].OnListChanged.connect(function (sender, new_index) {
            switch (new_index) {
                case new_index:
                    {
                        Selection[1] = ListeItem[1][new_index];
                        break;
                    }
            }
        });
        Selection[2] = ListeItem[2][0];
        menuListe[2].OnListChanged.connect(function (sender, new_index) {
            switch (new_index) {
                case new_index:
                    {
                        Selection[2] = ListeItem[2][new_index];
                        break;
                    }
            }
        });
        var items = args[5]
        for (var i = 0; i < items.Count; i++) {
            var listItem = API.createMenuItem(""+items[i], "");
            menu.AddItem(listItem);
        }

        API.drawMenu(menu);
        menu.Visible = true;

        menu.RefreshIndex();
        pool.Add(menu);
        menu.OnItemSelect.connect(function (sender, item, index) {
            if (Selection[0] != null && Selection[1] != null && Selection[2] != null) {
                API.triggerServerEvent("menu_handler_select_item", callback, index, Selection[0], Selection[1], Selection[2]);
            }
            pool = null;
        });
    }
    if (name == "amende") {
        pool = API.getMenuPool();
        var callback = args[0];
        var banner = args[1];
        var subtitle = args[2];
        var menu = null;
        var skin = null;

        menu = API.createMenu(banner, subtitle, 0, 0, 6);

        var Nomliste = args[3];
        var ListeItem = args[4];
        let menuListe = new Array(Nomliste.Count);
        //Listes
        for (var i = 0; i < Nomliste.Count; i++) {
            var liste = new List(String);
            for (var j = 0; j < ListeItem[i].Count; j++) {
                liste.Add(ListeItem[i][j]);
            }
            menuListe[i] = API.createListItem(Nomliste[i], "", liste, 0);
            menu.AddItem(menuListe[i]);
        }


        let Selection = new Array(Nomliste.Count);
        Selection[0] == null;
        menuListe[0].OnListChanged.connect(function (sender, new_index) {
            switch (new_index) {
                case new_index:
                    {
                        Selection[0] = ListeItem[0][new_index];
                        break;
                    }
            }
        });
        Selection[1] = ListeItem[1][0];
        menuListe[1].OnListChanged.connect(function (sender, new_index) {
            switch (new_index) {
                case new_index:
                    {
                        Selection[1] = ListeItem[1][new_index];
                        break;
                    }
            }
        });
        var items = args[5]
        for (var i = 0; i < items.Count; i++) {
            var listItem = API.createMenuItem("" + items[i], "");
            menu.AddItem(listItem);
        }

        API.drawMenu(menu);
        menu.Visible = true;

        menu.RefreshIndex();
        pool.Add(menu);
        menu.OnItemSelect.connect(function (sender, item, index) {
            if (Selection[0] != null) {
                API.triggerServerEvent("menu_handler_select_item", callback, index, Selection[0], Selection[1]);
            }
            pool = null;
        });
    }
    if (name == "fouiller") {
        pool = API.getMenuPool();
        var callback = args[0];
        var banner = args[1];
        var subtitle = args[2];
        var menu = null;
        var skin = null;

        menu = API.createMenu(banner, subtitle, 0, 0, 6);

        var Nomliste = args[3];
        var ListeItem = args[4];
        let menuListe = new Array(Nomliste.Count);
        //Listes
        for (var i = 0; i < Nomliste.Count; i++) {
            var liste = new List(String);
            for (var j = 0; j < ListeItem[i].Count; j++) {
                liste.Add(ListeItem[i][j]);
            }
            menuListe[i] = API.createListItem(Nomliste[i], "", liste, 0);
            menu.AddItem(menuListe[i]);
        }


        let Selection = new Array(Nomliste.Count);
        Selection[0] == null;
        menuListe[0].OnListChanged.connect(function (sender, new_index) {
            switch (new_index) {
                case new_index:
                    {
                        Selection[0] = ListeItem[0][new_index];
                        break;
                    }
            }
        });
        var items = args[5]
        for (var i = 0; i < items.Count; i++) {
            var listItem = API.createMenuItem("" + items[i], "");
            menu.AddItem(listItem);
        }

        API.drawMenu(menu);
        menu.Visible = true;

        menu.RefreshIndex();
        pool.Add(menu);
        menu.OnItemSelect.connect(function (sender, item, index) {
            if (Selection[0] != null) {
                API.sendChatMessage("Called it");
                API.triggerServerEvent("menu_handler_select_item", callback, index, Selection[0]);
            }
            pool = null;
        });

    }
    if (name == "amendeenvoi") {
        pool = API.getMenuPool();
        var callbackId = args[0];
        var banner = args[1];
        var subtitle = args[2];
        var noExit = args[3];

        var menu = null;
        if (banner == null)
            menu = API.createMenu(subtitle, 0, 0, 6);
        else menu = API.createMenu(banner, subtitle, 0, 0, 6);

        if (noExit) {
            menu.ResetKey(menuControl.Back);
        }

        var items = args[4];
        if (args.Length > 5) {
            var labels = args[5];
            for (var i = 0; i < items.Count; i++) {
                var listItem = API.createMenuItem(items[i], "");
                if (labels[i] != null) {
                    listItem.SetRightLabel(labels[i]);
                }
                menu.AddItem(listItem);
            }
        }
        else {
            for (var i = 0; i < items.Count; i++) {
                var listItem = API.createMenuItem(items[i], "");
                menu.AddItem(listItem);
            }
        }

        menu.OnItemSelect.connect(function (sender, item, index) {
            API.triggerServerEvent("menu_handler_select_item", callbackId, index, args[6], labels[0]);
            pool = null;
        });
        menu.Visible = true;

        pool.Add(menu);
    }
    else if (name == "LSPD_QUITMENU") {
        pool = null;
    }
    else if (name == "LSPD_ServiceCloth") {
        var player = API.getLocalPlayer();
        if (API.getEntitySyncedData(player,"Sexe") == "Homme") {
            API.setPlayerClothes(player, 11, 55, 0); //Survet
            API.setPlayerClothes(player, 3, 0, 0); // Bras
            API.setPlayerClothes(player, 8, 57, 0); // Chemise
            API.setPlayerClothes(player, 6, 25, 0); // Chaussure
            API.setPlayerClothes(player, 4, 31, 0); // Pantalon
            API.setPlayerClothes(player, 8, 58, 0);
            API.setPlayerAccessory(player, 0, 46, 0);
            if (API.getEntitySyncedData(player, "LSPDrank") == 4) {
                API.setPlayerClothes(player, 9, 15, 0);
            }
            if (API.getEntitySyncedData(player, "LSPDrank") == 5) {
                API.setPlayerClothes(player, 9, 16, 0);
            }
        }
        else if (API.getEntitySyncedData(player, "Sexe") == "Femme") {
            API.setPlayerClothes(player, 11, 48, 0); //Survet
            API.setPlayerClothes(player, 3, 0, 0); // Bras
            API.setPlayerClothes(player, 8, 0, 0); // Chemise
            API.setPlayerClothes(player, 8, 3, 0); // Chemise
            API.setPlayerClothes(player, 6, 25, 0); // Chaussure
            API.setPlayerClothes(player, 4, 30, 0); // Pantalon
            API.setPlayerClothes(player, 8, 58, 0);
            if (API.getEntitySyncedData(player, "LSPDrank") == 4) {
                API.setPlayerClothes(player, 9, 17, 0);
            }
            if (API.getEntitySyncedData(player, "LSPDrank") == 5) {
                API.setPlayerClothes(player, 9, 18, 0);
            }
        }
    }
    else if (name == "LSPD_FOURRIERE") {
        var streamedCar = API.getAllVehicles();
        var player = API.getLocalPlayer();
        for (var i = 0; i < streamedCar.Length; i++) {
            var car = streamedCar[i];
            var streamedcarPos = API.getEntityPosition(car);
            var playerPos = API.getEntityPosition(player);
            var distance = playerPos.DistanceTo(streamedcarPos);

            if (distance < 3) {
                API.triggerServerEvent("LSPD_FOURRIERE_SRV", car);
            }
        }
    }
    else if (name == "LSPD_VEHINFO") {
        var streamedCar = API.getAllVehicles();
        var player = API.getLocalPlayer();
        for (var i = 0; i < streamedCar.Length; i++) {
            var car = streamedCar[i];
            var streamedcarPos = API.getEntityPosition(car);
            var playerPos = API.getEntityPosition(player);
            var distance = playerPos.DistanceTo(streamedcarPos);

            if (distance < 3) {
                API.triggerServerEvent("LSPD_VEHINFO_SRV", car);
            }
        }
    }
    else if (name == "LSPD_CROCHETER") {
        var streamedCar = API.getAllVehicles();
        var player = API.getLocalPlayer();
        for (var i = 0; i < streamedCar.Length; i++) {
            var car = streamedCar[i];
            var streamedcarPos = API.getEntityPosition(car);
            var playerPos = API.getEntityPosition(player);
            var distance = playerPos.DistanceTo(streamedcarPos);

            if (distance < 3) {
                API.triggerServerEvent("LSPD_CROCHETER_SRV", car);
            }
        }
    }
});

API.onUpdate.connect(function () {
    if (pool != null) {
        pool.ProcessMenus();
    }
});