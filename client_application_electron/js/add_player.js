const {remote, ipcRenderer} = require('electron')

function closeAddPlayerWindow(){
  localStorage.removeItem('clubId');
  localStorage.removeItem('gameId');
  ipcRenderer.send('add-player-window');
}

function changeForm() {
  var newPlayer = document.getElementById("newPlayer");
  var playerFromList = document.getElementById("playerFromList");
  var addPlayerForm = document.getElementById("addPlayerForm");
  var chooseFromListForm = document.getElementById("chooseFromListForm");
  if(newPlayer.classList.contains("is-active")){
    addPlayerForm.classList.add("is-hidden");
    chooseFromListForm.classList.remove("is-hidden");
    newPlayer.classList.remove("is-active");
    playerFromList.classList.add("is-active");
  }
  else {
    chooseFromListForm.classList.add("is-hidden");
    addPlayerForm.classList.remove("is-hidden");
    playerFromList.classList.remove("is-active");
    newPlayer.classList.add("is-active");
  }
}

function isNotEmpty(someInput) {
  var spanMessage = document.getElementById(someInput.id + "Message");
	if(someInput.value == "") {
    spanMessage.classList.add("form-error");
    spanMessage.textContent = "Pole \"" + someInput.getAttribute("name") + "\" nie może być puste."
		return false;
	}
  else {
    spanMessage.classList.remove("form-error");
    spanMessage.textContent = ""
		return true;
	}
}

function validateInput(input){
  var spanMessage = document.getElementById(input.id + "Message");
  if (input.checkValidity() == false) {
    spanMessage.classList.add("form-error");
    spanMessage.textContent = "Pole \"" + input.getAttribute("name") + "\" jest niepoprawne."
    return false;
  }
  else {
    spanMessage.classList.remove("form-error");
    spanMessage.textContent = ""
    return true;
  }
}

function addPlayer() {
  var firstname = document.getElementById("firstname");
  var lastname = document.getElementById("lastname");
  var dateOfBirth = document.getElementById("dateOfBirth");
  var convertedDate = new Date(dateOfBirth.value);
  var height = document.getElementById("height");
  var weight = document.getElementById("weight");
  var newNumber = document.getElementById("newNumber");
  var newPositions = document.getElementById("newPositions");
  var statusMessage = document.getElementById("statusMessage");
  if(isNotEmpty(firstname) && isNotEmpty(lastname) && validateInput(dateOfBirth) && validateInput(height) && validateInput(weight)) {
    var addPlayerForm = document.getElementById("addPlayerForm");
    var createAnimation = document.getElementById("createAnimation");
    addPlayerForm.classList.add("is-hidden");
    createAnimation.classList.remove("is-hidden");
    var xhr = new XMLHttpRequest();
    xhr.open("POST", "http://inzynierkawebapi.azurewebsites.net/api/Player/create", true);
    xhr.setRequestHeader('Content-Type', 'application/json');
    xhr.send(JSON.stringify({
      "firstName": firstname.value,
      "lastName": lastname.value,
      "dateOfBirth": convertedDate,
      "heightOfUser": height.value,
      "weightOfUser": weight.value,
      "clubId": localStorage.getItem('clubId')
    }));
    xhr.onreadystatechange = function () { //Call a function when the state changes.
      if (xhr.readyState == 4 && xhr.status == 200) {
          createAnimation.classList.add("is-hidden");
          addPlayerForm.classList.remove("is-hidden");
          document.getElementById("statusMessage").classList.remove("is-hidden");
          refreshPlayerList();
          }
      }
    }
}

function chooseFromList() {
  var number = document.getElementById("number");
  var positions = document.getElementById("positions");
  var statusMessage = document.getElementById("statusMessage");
  var modules = document.getElementById("modules");
  if(validateInput(number)) {
    var chooseFromListForm = document.getElementById("chooseFromListForm");
    var createAnimation = document.getElementById("createAnimation");
    chooseFromListForm.classList.add("is-hidden");
    createAnimation.classList.remove("is-hidden");
    var xhr = new XMLHttpRequest();
    xhr.open("POST", "http://inzynierkawebapi.azurewebsites.net/api/Player/addToGame", true);
    xhr.setRequestHeader('Content-Type', 'application/json');
    var players = document.getElementById("players");
    xhr.send(JSON.stringify({
      "position": positions.options[positions.selectedIndex].text,
      "number": number.value,
      "active": true,
      "gameId": localStorage.getItem('gameId'),
      "playerId": players.options[players.selectedIndex].value,
      "moduleId":modules.options[modules.selectedIndex].value
    }));
    xhr.onreadystatechange = function () {
      if (xhr.readyState == 4 && xhr.status == 200) {
          createAnimation.classList.add("is-hidden");
          chooseFromListForm.classList.remove("is-hidden");
          ipcRenderer.send('new-player-from-add-window');
          closeAddPlayerWindow();
          }
      }
    }
}

function refreshPlayerList() {
  var xhr = new XMLHttpRequest();
  var urlForGet = "http://inzynierkawebapi.azurewebsites.net/api/Player/clubplayers" + "?clubId=" + localStorage.getItem('clubId')
  xhr.open("GET", urlForGet, true);
  xhr.setRequestHeader('Accept', 'application/json');
  xhr.send();
  xhr.onreadystatechange = function () { //Call a function when the state changes.
    if (xhr.readyState == 4 && xhr.status == 200) {
      obj = JSON.parse(xhr.responseText);
      if(obj.length>0) {
        var players = document.getElementById("players");
        for(var i=0; i<players.options.length; i++){
          players.remove(i);
        }
        for(var i=0; i<obj.length; i++){
          var option = document.createElement("option");
          option.text = obj[i].firstName + " " + obj[i].lastName;
          option.value = obj[i].id;
          players.add(option);
        }
      }
    }
  }
}

function refreshModules() {
  var xhr = new XMLHttpRequest();
  var urlForGet = "http://inzynierkawebapi.azurewebsites.net/api/controller/modules" + "?clubId=" + localStorage.getItem('clubId')
  xhr.open("GET", urlForGet, true);
  xhr.setRequestHeader('Accept', 'application/json');
  xhr.send();
  xhr.onreadystatechange = function () { //Call a function when the state changes.
    if (xhr.readyState == 4 && xhr.status == 200) {
      obj = JSON.parse(xhr.responseText);
      if(obj.length>0) {
        var modules = document.getElementById("modules");
        for(var i=0; i<modules.options.length; i++){
          modules.remove(i);
        }
        for(var i=0; i<obj.length; i++){
          var option = document.createElement("option");
          option.text = obj[i].macAddress;
          option.value = obj[i].id;
          modules.add(option);
        }
      }
    }
  }
}
window.onload = init;

function init(){
  var firstname = document.getElementById("firstname");
  firstname.onblur = function() {
    isNotEmpty(this);
  }
  var lastname = document.getElementById("lastname");
  lastname.onblur = function() {
    isNotEmpty(this);
  }
  var dateOfBirth = document.getElementById("dateOfBirth");
  dateOfBirth.onblur =  function() {
    validateInput(this)
  }
  var height = document.getElementById("height");
  height.onblur =  function() {
    validateInput(this)
  }
  var weight = document.getElementById("weight");
  weight.onblur =  function() {
    validateInput(this)
  }
  var newNumber = document.getElementById("newNumber");
  weight.onblur =  function() {
    validateInput(this)
  }

  refreshPlayerList();

  var number = document.getElementById("number");
  number.onblur =  function() {
    validateInput(this)
  }

  refreshModules();

}
