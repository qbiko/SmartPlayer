const { app, BrowserWindow } = require('electron');
const ipc = app.ipcMain

let startScreen;

app.on('ready', () => {
    startScreen = new BrowserWindow({
        width: 400,
        height: 700
    });

    startScreen.on('closed', () => {
        startScreen = null;
    });

    startScreen.loadURL('file://' + __dirname + '/login.html');
});

app.on('window-all-closed', () => {
    if (process.platform !== 'darwin') {
        app.quit();
    }
});
