const {remote, ipcRenderer} = require('electron');
window.onload = init;
ekg = false;
i = 0;


function init(){
  var clubName = document.getElementById("clubName");
  clubName.textContent = sessionStorage.getItem('clubName');

  ipcRenderer.on('new-player-to-match-window', function () {
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

  setInterval(
    function() {
      var playersIds = [];
      var k = 0;
      var fieldContainer = document.getElementById("fieldContainer");
      for(var j=0; j<fieldContainer.childNodes.length; j++) {
        if(!fieldContainer.children[j].classList.contains("is-hidden")){
          playersIds[k] = fieldContainer.children[j].getAttribute('id').replace("badge-on-field-", "");
          k++;
        }
      }
      var players = document.getElementById("players");
      var xhr = new XMLHttpRequest();
      var urlForGet = "http://inzynierkawebapi.azurewebsites.net/api/Sensors/locationsBatch" + "?gameId=" + sessionStorage.getItem('gameId') + "&startDateString="
      + JSON.stringify(lastDate).replace(/:/g, "%3A").replace(/\+/g, "%2B").replace(/\"/g, "") + "&playerIds=" + playersIds.toString();
      xhr.open("GET", urlForGet, true);
      xhr.setRequestHeader('Accept', 'application/json');
      xhr.send();
      xhr.onreadystatechange = function () { //Call a function when the state changes.
        if (xhr.readyState == 4 && xhr.status == 200) {
          obj = JSON.parse(xhr.responseText);
          for(var i=0;i<obj.length;i++){
            for(var j=0;j<obj[i].listOfPositions.length;j++){
              var timeout = 0;
              if(j!=0) timeout = new Date(obj[i].listOfPositions[j].timeOfOccur).getTime() - new Date(obj[i].listOfPositions[j-1].timeOfOccur).getTime();
              setTimeout(setPosition(obj[i].listOfPositions[j].x, obj[i].listOfPositions[j].y, obj[i].playerId),timeout);
            }
            var dateFromDB = new Date(obj[i].listOfPositions[obj[i].listOfPositions.length-1].timeOfOccur);
            if(dateFromDB.getTime()>lastDate.getTime()){
              lastDate=dateFromDB;
            }
          }
          //document.getElementById("badge-on-field-3").setAttribute("style", "position: absolute; top: "+ obj[0].listOfPositions[1].y + "px; left: " + obj[0].listOfPositions[1].x+"px;");

        }

      }

    }, 10000);

    function setPosition(x,y,playerId){
      document.getElementById("badge-on-field-" + playerId).setAttribute("style", "position: absolute; top: "+ y + "px; left: " + x +"px;");
    }



  /*var chart = new SmoothieChart(),
    canvas = document.getElementById('ekgGraph'),
    series = new TimeSeries(100);*/




  /*graph = document.getElementById("ekgGraph");

  ctx = graph.getContext('2d'),
  w = graph.width,
  h = graph.height,
  px = 0, opx = 0, speed = 3,
  py = h * 0.8, opy = py,
  scanBarWidth = 3;

  ctx.strokeStyle = '#FF0000';
  ctx.lineWidth = 1;

  graph.onmousemove = function(e) {
    var r = demo.getBoundingClientRect();
    py = e.clientY - r.top;
}

  function loop() {
    if(ekg){
      var players = document.getElementById("players");
      var xhr = new XMLHttpRequest();
      var urlForGet = "http://inzynierkawebapi.azurewebsites.net/api/Sensors/pulseBatch" + "?playerId=" + players.children[players.selectedIndex].value + "&gameId=" + sessionStorage.getItem('gameId');
      //var urlForGet = "http://inzynierkawebapi.azurewebsites.net/api/Sensors/pulseBatch" + "?playerId="
       //+ 14 + "&gameId=" + 93;
      xhr.open("GET", urlForGet, false);
      xhr.setRequestHeader('Accept', 'application/json');
      xhr.send();
      xhr.onreadystatechange = function () { //Call a function when the state changes.
        if (xhr.readyState == 4 && xhr.status == 200) {
          obj = JSON.parse(xhr.responseText);
          //py = obj[i++].value/10;
          py = obj[obj.length-1].value/10;
          px += speed;

          ctx.clearRect(px,0, scanBarWidth, h);
          ctx.beginPath();
          ctx.moveTo(opx, opy);
          ctx.lineTo(px, py);
          ctx.stroke();

          opx = px;
          opy = py;

          if (opx > w) {
              px = opx = -speed;
          }
        }
      }

      px += speed;

      ctx.clearRect(px,0, scanBarWidth, h);
      ctx.beginPath();
      ctx.moveTo(opx, opy);
      ctx.lineTo(px, py);
      ctx.stroke();

      opx = px;
      opy = py;

      if (opx > w) {
          px = opx = -speed;
      }
    }

    requestAnimationFrame(loop);
  }

  loop();*/
}

function refreshPlayerDetails(playerId) {
  var xhr = new XMLHttpRequest();
  var urlForGet = "http://inzynierkawebapi.azurewebsites.net/api/Player" + "?playerId=" + playerId
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
  var urlForGet = "http://inzynierkawebapi.azurewebsites.net/api/Player/gameplayers" + "?gameId=" + sessionStorage.getItem('gameId')
  xhr.open("GET", urlForGet, true);
  xhr.setRequestHeader('Accept', 'application/json');
  xhr.send();
  xhr.onreadystatechange = function () { //Call a function when the state changes.
    if (xhr.readyState == 4 && xhr.status == 200) {
      obj = JSON.parse(xhr.responseText);
      var players = document.getElementById("players");
      document.getElementById("playerDetails").classList.add("is-hidden");
      for(var i=0; i<players.options.length; i++){
        players.options[i].selected = false;
        players.remove(i);
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

        for(var i=0; i<players.options.length; i++){
          players.options[i].selected = false;
          players.remove(i);
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
          document.getElementById("playersOnField").appendChild(input); // put it into the DOM

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

/*function funkcja(input){
    if(input.checked){
      document.getElementById(input.getAttribute('id').replace("playerCheckboxID-", "badge-on-field-")).classList.remove("is-hidden");
      document.getElementById(input.getAttribute('id').replace("playerCheckboxID-", "badge-")).setAttribute('class', 'badge primary');
    }
    else {
      document.getElementById(input.getAttribute('id').replace("playerCheckboxID-", "badge-on-field-")).classList.add("is-hidden");
      document.getElementById(input.getAttribute('id').replace("playerCheckboxID-", "badge-")).setAttribute('class', 'badge secondary');
    }
}*/

function refreshFieldsList() {
  var xhr = new XMLHttpRequest();
  var urlForGet = "http://inzynierkawebapi.azurewebsites.net/api/Pitch";
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
  localStorage.setItem('clubId', sessionStorage.getItem('clubId'));
  ipcRenderer.send('add-pitch-window');
}

function createGame() {
    var mainContainer = document.getElementById("mainContainer");
    var createAnimation = document.getElementById("createAnimation");
    mainContainer.classList.add("is-hidden");
    createAnimation.classList.remove("is-hidden");
    var xhr = new XMLHttpRequest();
    xhr.open("POST", "http://inzynierkawebapi.azurewebsites.net/api/Game/create", true);
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
  var urlForDelete = "http://inzynierkawebapi.azurewebsites.net/api/Player/removeFromGame?playerId=" + playerId + "&gameId=" + sessionStorage.getItem('gameId');
  xhr.open("DELETE", urlForDelete, true);
  xhr.send();
  xhr.onreadystatechange = function () {
  	if (xhr.readyState == 4 && xhr.status == 200) {
      var players = document.getElementById("players");
      for(var i=0; i<players.options.length; i++){
        players.options[i].selected = false;
        players.remove(i);
      }
      refreshPlayerList();
      ekg = false;
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

Date.prototype.toJSON = function () {
  var timezoneOffsetInHours = -(this.getTimezoneOffset() / 60); //UTC minus local time
  var sign = timezoneOffsetInHours >= 0 ? '+' : '-';
  var leadingZero = (timezoneOffsetInHours < 10) ? '0' : '';

  //It's a bit unfortunate that we need to construct a new Date instance
  //(we don't want _this_ Date instance to be modified)
  var correctedDate = new Date(this.getFullYear(), this.getMonth(),
      this.getDate(), this.getHours(), this.getMinutes(), this.getSeconds(),
      this.getMilliseconds());
  correctedDate.setHours(this.getHours() + timezoneOffsetInHours);
  var iso = correctedDate.toISOString().replace('Z', '');

  return iso + sign + leadingZero + Math.abs(timezoneOffsetInHours).toString() + ':00';
}
