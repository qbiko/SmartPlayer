const {remote, ipcRenderer} = require('electron');
window.onload = init;

function init(){
  var clubName = document.getElementById("clubName");
  var players = document.getElementById("players");
  clubName.textContent = sessionStorage.getItem('clubName');

  electrocardiogram();

  gpsLocations();

  ipcRenderer.on('new-player-to-match-window', function () {
    refreshPlayerList();
  });

  ipcRenderer.on('player-to-edited-window', function () {
    refreshPlayerList();
  });

  ipcRenderer.on('new-pitch-to-match-window', function () {
    refreshFieldsList();
  });

  refreshFieldsList();

  document.getElementById("players").onchange = function() {
    ekg = true;
    var index = this.selectedIndex;
    var playerDetails = document.getElementById("playerDetails");
    if(playerDetails.classList.contains("is-hidden")) playerDetails.classList.remove("is-hidden");
    document.getElementById("playerName").textContent = this.children[index].innerHTML.trim();
    document.getElementById("position").textContent = this.children[index].name.trim().charAt(0).toUpperCase() + this.children[index].name.trim().slice(1);
    refreshPlayerDetails(this.children[index].value);
    if(document.getElementById("playerDetails").classList.contains("is-hidden")) document.getElementById("playerDetails").classList.remove("is-hidden");
  }
}

function checkIfSomePlayerIsSelected() {
  for(var i=0; i<players.options.length; i++){
    if(players.options[i].selected==true) return true;
  }
  return false;
}

function refreshPlayerDetails(playerId) {
  var xhr = new XMLHttpRequest();
  var urlForGet = REST_API + "/Player" + "?playerId=" + playerId
  xhr.open("GET", urlForGet, true);
  xhr.setRequestHeader('Accept', 'application/json');
  xhr.send();
  xhr.onreadystatechange = function () { //Call a function when the state changes.
    if (xhr.readyState == 4 && xhr.status == 200) {
      obj = JSON.parse(xhr.responseText);
      document.getElementById("weight").textContent = obj.weightOfUser + " kg";
      document.getElementById("height").textContent = obj.heightOfUser + " cm";
      var dateOfBirth = new Date(obj.dateOfBirth);

      var ageDifMs = Date.now() - dateOfBirth.getTime();
      var ageDate = new Date(ageDifMs); // miliseconds from epoch
      document.getElementById("age").textContent = Math.abs(ageDate.getUTCFullYear() - 1970) + " lat";
    }
  }
}

function refreshPlayerList() {
  var xhr = new XMLHttpRequest();
  var urlForGet = REST_API + "/Player/gameplayers" + "?gameId=" + sessionStorage.getItem('gameId')
  xhr.open("GET", urlForGet, true);
  xhr.setRequestHeader('Accept', 'application/json');
  xhr.send();
  xhr.onreadystatechange = function () {
    if (xhr.readyState == 4 && xhr.status == 200) {
      obj = JSON.parse(xhr.responseText);
      var players = document.getElementById("players");
      document.getElementById("playerDetails").classList.add("is-hidden");
      while(players.options.length > 0){
        players.remove(0);
      }
      var playersOnField = document.getElementById("playersOnField");
      while (playersOnField.firstChild) {
          playersOnField.removeChild(playersOnField.firstChild);
      }
      var fieldContainer = document.getElementById("fieldContainer");
      while (fieldContainer.firstChild) {
          fieldContainer.removeChild(fieldContainer.firstChild);
      }
      if(obj.length>0) {
          document.getElementById("removePlayer").classList.remove("disabled");
          document.getElementById("editPlayer").classList.remove("disabled");
          if(document.getElementById("finishGame").classList.contains("disabled")){
            document.getElementById("startGame").classList.remove("disabled");
          }
        }

        for(var i=0; i<obj.length; i++){
          var option = document.createElement("option");
          option.text = obj[i].number + ". " + obj[i].firstname + " " + obj[i].lastname;
          option.value = obj[i].id;
          option.name = obj[i].position;
          players.add(option);



          var span = document.createElement("span");
          span.setAttribute('class', 'badge secondary');
          var elementID = "badge-" + obj[i].id;
          span.setAttribute('id', elementID);
          span.innerHTML = obj[i].number;


          var spanOnField = document.createElement("span");
          spanOnField.setAttribute('class', 'badge');
          var elementID = "badge-on-field-" + obj[i].id;
          spanOnField.setAttribute('id', elementID);
          spanOnField.setAttribute("style", "position: absolute; top: 0px; left: 0px;");
          spanOnField.innerHTML = obj[i].number;
          spanOnField.classList.add('is-hidden');

          document.getElementById("fieldContainer").appendChild(spanOnField);

          var input = document.createElement("input");
          input.type = "checkbox";
          var playerCheckboxID = "playerCheckboxID-" + obj[i].id;
          input.setAttribute('id', playerCheckboxID);
          document.getElementById("playersOnField").appendChild(input);

          addListener(input);

          var label = document.createElement("label");
          label.setAttribute('for', playerCheckboxID);

          label.appendChild(span);
          document.getElementById("playersOnField").appendChild(label);
        }
    }
  }
}

function addListener(input){
  input.addEventListener('click',function(e){
    if(input.checked){
      document.getElementById(input.getAttribute('id').replace("playerCheckboxID-", "badge-on-field-")).classList.remove("is-hidden");
      document.getElementById(input.getAttribute('id').replace("playerCheckboxID-", "badge-")).setAttribute('class', 'badge primary');
    }
    else {
      document.getElementById(input.getAttribute('id').replace("playerCheckboxID-", "badge-on-field-")).classList.add("is-hidden");
      document.getElementById(input.getAttribute('id').replace("playerCheckboxID-", "badge-")).setAttribute('class', 'badge secondary');
    }
  })
}

function refreshFieldsList() {
  var xhr = new XMLHttpRequest();
  var urlForGet = REST_API + "/Pitch";
  xhr.open("GET", urlForGet, true);
  xhr.setRequestHeader('Accept', 'application/json');
  xhr.send();
  xhr.onreadystatechange = function () { //Call a function when the state changes.
    if (xhr.readyState == 4 && xhr.status == 200) {
      obj = JSON.parse(xhr.responseText);
      if(obj.length>0) {
        var fields = document.getElementById("fields");
        for(var i=0; i<fields.options.length; i++){
          fields.remove(i);
        }
        for(var i=0; i<obj.length; i++){
          var option = document.createElement("option");
          option.text = obj[i].nameOfPitch;
          option.value = obj[i].id;
          fields.add(option);
        }
      }
    }
  }
}

function openAddPlayerWindow(){
  localStorage.setItem('clubId', sessionStorage.getItem('clubId'));
  localStorage.setItem('clubName', sessionStorage.getItem('clubName'));
  localStorage.setItem('gameId', sessionStorage.getItem('gameId'));
  ipcRenderer.send('add-player-window');
}

function openAddModuleWindow(){
  localStorage.setItem('clubId', sessionStorage.getItem('clubId'));
  ipcRenderer.send('add-module-window');
}

function openAddPitchWindow(){
  localStorage.setItem('clubId', sessionStorage.getItem('playerId'));
  ipcRenderer.send('add-pitch-window');
}

function openEditPlayerWindow(){
  var players = document.getElementById("players");
  var playerId = players.options[players.selectedIndex].value;
  localStorage.setItem('playerId', playerId);
  ipcRenderer.send('edit-player-window');
}

function createGame() {
    var mainContainer = document.getElementById("mainContainer");
    var createAnimation = document.getElementById("createAnimation");
    mainContainer.classList.add("is-hidden");
    createAnimation.classList.remove("is-hidden");
    var xhr = new XMLHttpRequest();
    xhr.open("POST", REST_API + "/Game/create", true);
    xhr.setRequestHeader('Content-Type', 'application/json');
    xhr.send(JSON.stringify({
      "nameOfGame": sessionStorage.getItem('clubName')+new Date(),
      "timeOfStart": new Date(),
      "clubId": sessionStorage.getItem('clubId'),
      "pitchId": sessionStorage.getItem('pitchId')
    }));
    xhr.onreadystatechange = function () { //Call a function when the state changes.
      if (xhr.readyState == 4 && xhr.status == 200) {
          obj = JSON.parse(xhr.responseText);
          createAnimation.classList.add("is-hidden");
          mainContainer.classList.remove("is-hidden");
          document.getElementById("newGame").classList.add("disabled");
          document.getElementById("addPlayer").classList.remove("disabled");
          sessionStorage.setItem('gameId', obj.id);
        }
      }
}

function removePlayer() {
  var players = document.getElementById("players");
  var playerId = players.options[players.selectedIndex].value;
  var xhr = new XMLHttpRequest();
  var urlForDelete = REST_API + "/Player/removeFromGame?playerId=" + playerId + "&gameId=" + sessionStorage.getItem('gameId');
  xhr.open("DELETE", urlForDelete, true);
  xhr.send();
  xhr.onreadystatechange = function () {
  	if (xhr.readyState == 4 && xhr.status == 200) {
      var players = document.getElementById("players");
      refreshPlayerList();
  	}
  }
}

function choosePitch() {
  var fields = document.getElementById("fields");
  var pitchId = fields.options[fields.selectedIndex].value;
  sessionStorage.setItem('pitchId', pitchId);
  fields.disabled=true;
  document.getElementById("chooseFieldButton").classList.add("disabled");
  document.getElementById("addPitchButton").classList.add("disabled");
  document.getElementById("newGame").classList.remove("disabled");
}
