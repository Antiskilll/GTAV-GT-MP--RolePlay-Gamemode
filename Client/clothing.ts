var menuPool = null;
var draw = false;


API.onResourceStart.connect(function () {

});

API.onServerEventTrigger.connect(function (name, args) {

    if (name == "clothing") {
        var callback = args[0];
        var banner = args[1];
        var subtitle = args[2];
        var menu = null;
        if (banner == null)
            menu = API.createMenu(subtitle, 0, 0, 6);
        else menu = API.createMenu(banner, subtitle, 0, 0, 6);

        var Nomliste = args[3];
        var ListeItem = args[4];
        let menuListe = new Array(Nomliste.Count);
        menu.ResetKey(menuControl.Back);
        //Listes
        for (var i = 0; i < Nomliste.Count; i++) {
            var liste = new List(String);
            for (var j = 0; j < ListeItem[i].Count; j++) {
                liste.Add(ListeItem[i][j]);
            }
            menuListe[i] = API.createListItem(Nomliste[i], "", liste, 0);
            menu.AddItem(menuListe[i]);
        }
        //Items
        let accept = API.createMenuItem("Accepter", "");
        menu.AddItem(accept);
        let refuser = API.createMenuItem("Refuser", "");
        menu.AddItem(refuser);
        //*********WASSAP MOTHERFUCKER**************
        let Selection = [-1, -1, -1,-1,-1];
        menuListe[0].OnListChanged.connect(function (sender, new_index) {
            switch (new_index) {
                case new_index:
                    {
                        API.setPlayerClothes(API.getLocalPlayer(), 4, new_index, 0);
                        Selection[0] = ListeItem[0][new_index];
                        break;
                    }
            }
        });
        menuListe[1].OnListChanged.connect(function (sender, new_index) {
            switch (new_index) {
                case new_index:
                    {
                        API.setPlayerClothes(API.getLocalPlayer(), 8, new_index, 0);
                        Selection[1] = ListeItem[1][new_index];
                        break;
                    }
            }
        });
        menuListe[2].OnListChanged.connect(function (sender, new_index) {
            switch (new_index) {
                case new_index:
                    {
                        API.setPlayerClothes(API.getLocalPlayer(), 11, new_index, 0);
                        Selection[2] = ListeItem[2][new_index];
                        break;
                    }
            }
        });
        menuListe[3].OnListChanged.connect(function (sender, new_index) {
            switch (new_index) {
                case new_index:
                    {
                        API.setPlayerClothes(API.getLocalPlayer(), 6, new_index, 0);
                        Selection[3] = ListeItem[3][new_index];
                        break;
                    }
            }
        });
        menuListe[4].OnListChanged.connect(function (sender, new_index) {
            switch (new_index) {
                case new_index:
                    {
                        API.setPlayerClothes(API.getLocalPlayer(), 3, new_index, 0);
                        Selection[4] = ListeItem[4][new_index];
                        break;
                    }
            }
        });
        //*********EWWWWWWW SHIEEEEEEETTT**************
        refuser.Activated.connect(function (menu, item) {
            menu.Visible = false;
            menu.Clear();
            API.setActiveCamera(null);
            //API.triggerServerEvent("resetcloth")


        });
        accept.Activated.connect(function (menu, item) {
            API.triggerServerEvent("changecloth", Selection[0], Selection[1], Selection[2], Selection[3], Selection[4])
            menu.Visible = false;
            menu.Clear();
            API.setActiveCamera(null);

        });


        API.onUpdate.connect(function () {
            API.drawMenu(menu);
        });

        menu.Visible = true;

    }
});


API.onUpdate.connect(function () {

    if (menuPool != null) menuPool.ProcessMenus();
});