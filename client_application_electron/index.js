const {app, BrowserWindow, ipcMain} = require('electron')

let startScreen;
let addPlayerScreen;
let addModuleScreen;
let addPitchScreen;
let editPlayerScreen;

app.on('ready', () => {
    startScreen = new BrowserWindow({
        width: 400,
        height: 700
    });

    startScreen.on('closed', () => {
        addPlayerScreen.close();
        addModuleScreen.close();
        addPitchScreen.close();
        editPlayerScreen.close();
        startScreen = null;
    });

    startScreen.loadURL('file://' + __dirname + '/login.html');

    addPlayerScreen = new BrowserWindow({
        width: 450,
        height: 800,
        show: false,
        frame: true //zmienic na false
    });

    addModuleScreen = new BrowserWindow({
        width: 450,
        height: 450,
        show: false,
        frame: true //zmienic na false
    });

    addPitchScreen = new BrowserWindow({
        width: 750,
        height: 900,
        show: false,
        frame: true //zmienic na false
    });

    editPlayerScreen = new BrowserWindow({
        width: 450,
        height: 800,
        show: false,
        frame: true //zmienic na false
    });

    ipcMain.on('add-player-window', function () {
      if (addPlayerScreen.isVisible()){
        addPlayerScreen.hide()
      }
      else{
        addPlayerScreen.loadURL('file://' + __dirname + '/add_player.html');
        addPlayerScreen.show()
      }
    });

    ipcMain.on('add-module-window', function () {
      if (addModuleScreen.isVisible()){
        addModuleScreen.hide()
      }
      else{
        addModuleScreen.loadURL('file://' + __dirname + '/addModule.html');
        addModuleScreen.show()
      }
    });

    ipcMain.on('add-pitch-window', function () {
      if (addPitchScreen.isVisible()){
        addPitchScreen.hide()
      }
      else{
        addPitchScreen.loadURL('file://' + __dirname + '/addPitch.html');
        addPitchScreen.show()
      }
    });

    ipcMain.on('edit-player-window', function () {
      if (editPlayerScreen.isVisible()){
        editPlayerScreen.hide()
      }
      else{
        editPlayerScreen.loadURL('file://' + __dirname + '/edit_player.html');
        editPlayerScreen.show()
      }
    });

    ipcMain.on('new-player-from-add-window', function () {
      startScreen.webContents.send('new-player-to-match-window');
    });

    ipcMain.on('player-from-edited-window', function () {
      startScreen.webContents.send('player-to-edited-window');
    });

    ipcMain.on('new-pitch-from-add-window', function () {
      startScreen.webContents.send('new-pitch-to-match-window');
    });
});

app.on('window-all-closed', () => {
    if (process.platform !== 'darwin') {
        app.quit();
    }
});
