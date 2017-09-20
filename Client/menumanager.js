var pool = null;
var text = null;

API.onServerEventTrigger.connect(function (name, args) {
    if (name == "bettermenuManager") {
        pool = API.getMenuPool();
        var callbackId = args[0];
        var banner = args[1];
        var subtitle = args[2];
        var noExit = args[3];
        var menu = null;
        if (banner == null)
            menu = API.createMenu(subtitle, 0, 0, 6);
        else
            menu = API.createMenu(banner, subtitle, 0, 0, 6);
        if (noExit) {
            menu.ResetKey(menuControl.Back);
        }
        API.setHudVisible(false);
        API.showCursor(false);
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
            API.triggerServerEvent("menu_handler_select_item", callbackId, index, item.Text);
            pool = null;
            API.setHudVisible(true);
        });

        menu.OnMenuClose.connect(function (player) {
            API.setHudVisible(true);
        });

        menu.Visible = true;
        pool.Add(menu);
    }
    else if (name === "menu_handler_close_menu") {
        API.setHudVisible(true);
        pool = null;
    }
    else if (name == "get_user_input") {
        text = API.getUserInput(args[1], args[2]);
        if (args[3] == null) {
            API.triggerServerEvent("menu_handler_user_input", args[0], text);
        }
        else {
            API.triggerServerEvent("menu_handler_user_input", args[0], text, args[3]);
        }
    }
});

API.onUpdate.connect(function () {
    if (pool != null) {
        pool.ProcessMenus();
    }
});
