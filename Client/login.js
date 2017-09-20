var loginPage = null;
var passwordTries = 0;

class CEF {
    constructor(resourcePath) {
        this.path = resourcePath
        this.open = false
    }

    show() {
        if (this.open === false) {
            this.open = true

            var resolution = API.getScreenResolution()

            this.browser = API.createCefBrowser(resolution.Width / 4, resolution.Height / 3 + 165, true)
            API.waitUntilCefBrowserInit(this.browser)
            API.setCefBrowserPosition(this.browser, resolution.Width / 4 + 230, resolution.Height / 2 - 280)
            API.loadPageCefBrowser(this.browser, this.path)
            API.showCursor(true)
        }
    }

    destroy() {
        this.open = false
        API.destroyCefBrowser(this.browser)
        API.showCursor(false)
    }

    eval(string) {
        this.browser.eval(string)
    }
}


API.onServerEventTrigger.connect(function(eventName, args) {
    switch (eventName) {
        case "SHOW_LOGIN_PAGE":
            toggleHUD(false);
            loginPage.show();
            break;

        case "CLOSE_LOGIN_PAGE":
            endLoginProcedure();
            break;

        case "RETRY_LOGIN_PAGE":
            passwordTries = passwordTries + 1;

            if (passwordTries > 2) {
                loginPage.destroy();
                loginPage = null;
                toggleHUD(true);
                API.disconnect("[LOGIN] 3 echecs lors de la saisie du mot de passe.");
                return;
            }

            var remainingTries = 3 - passwordTries;
            var errorMessage = "Erreur dans le mot de passe, " + remainingTries + " essais restants.";
            loginPage.browser.call("showMessage", errorMessage);
            break;
    }
});

// Load datas
API.onResourceStart.connect(function() {
    if (loginPage == null) loginPage = new CEF('Client/Login/LoginPage.html');
});

// Reset variables
API.onResourceStop.connect(function() {
    toggleHUD(true);
    loginPage = null;
    passwordTries = null;
});



/**********************
       FUNCTIONS
***********************/

// Disconnect using login page button
function sendDisconnect() {
    loginPage = null;
    passwordTries = null;
    toggleHUD(true);
    API.disconnect("[LOGIN] Vous vous êtes déconnecté.");
}

// Trigger password check (Event in Player.cs -> Function in Database.cs)
function login(password) {
    API.triggerServerEvent("PLAYER_CHECK_PASSWORD", password);
}

// Reset variables & close browser
function endLoginProcedure() {
    loginPage.browser.call("showMessage", "Connexion réussie, veuillez patienter...");
    API.sleep(3000);
    loginPage.destroy();
    loginPage = null;
    toggleHUD(true);
}

// Show (true) of hide (false) HUD
function toggleHUD(mode) {
    if (!mode) {
        let newCamera = API.createCamera(new Vector3(-27.31, 606.78, 307.39), new Vector3(-10, 0, 152.69));
        API.setActiveCamera(newCamera);
        API.setHudVisible(false);
        API.setCanOpenChat(false);
        API.setChatVisible(false);
        API.displaySubtitle(" ", 1);
    } else {
        API.setActiveCamera(null);
        API.setHudVisible(true);
        API.setCanOpenChat(true);
        API.setChatVisible(true);
        API.showCursor(false);
        API.displaySubtitle(" ", 1);
    }
}