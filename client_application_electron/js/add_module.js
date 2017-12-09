const {remote, ipcRenderer} = require('electron')

function closeAddModuleWindow(){
  localStorage.removeItem('clubId');
  ipcRenderer.send('add-module-window');
}

function validateInput(input){
  var spanMessage = document.getElementById(input.id + "Message");
  if (input.checkValidity() == false) {
    spanMessage.classList.add("form-error");
    spanMessage.textContent = "Pole \"" + input.getAttribute("name") + "\" jest niepoprawne. Przykładowy adres MAC: 00:0A:E6:3E:FD:E1"
    return false;
  }
  else {
    spanMessage.classList.remove("form-error");
    spanMessage.textContent = ""
    return true;
  }
}

function addModule() {
  var macAddress = document.getElementById("macAddress");
  var statusMessage = document.getElementById("statusMessage");
  if(validateInput(macAddress)) {
    var addModuleForm = document.getElementById("addModuleForm");
    var createAnimation = document.getElementById("createAnimation");
    addModuleForm.classList.add("is-hidden");
    createAnimation.classList.remove("is-hidden");
    var xhr = new XMLHttpRequest();
    xhr.open("POST", "http://inzynierkawebapi.azurewebsites.net/api/controller/add", true);
    xhr.setRequestHeader('Content-Type', 'application/json');
    xhr.send(JSON.stringify({
      "macAddress": macAddress.value,
      "clubId": localStorage.getItem('clubId')
    }));
    xhr.onreadystatechange = function () {
      if (xhr.readyState == 4 && xhr.status == 200) {
          createAnimation.classList.add("is-hidden");
          addModuleForm.classList.remove("is-hidden");
          document.getElementById("statusMessage").classList.remove("is-hidden");
        }
      }
    }
}
window.onload = init;

function init(){
  var macAddress = document.getElementById("macAddress");
  macAddress.onblur = function() {
    validateInput(this);
  }

}
