const {remote, ipcRenderer} = require('electron')

function closeEditPlayerWindow(){
  localStorage.removeItem('playerId');
  ipcRenderer.send('edit-player-window');
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

function editPlayer() {
  var height = document.getElementById("height");
  var weight = document.getElementById("weight");
  var statusMessage = document.getElementById("statusMessage");
  if(validateInput(height) && validateInput(weight)) {
    var editPlayerForm = document.getElementById("editPlayerForm");
    var createAnimation = document.getElementById("createAnimation");
    editPlayerForm.classList.add("is-hidden");
    createAnimation.classList.remove("is-hidden");
    var xhr = new XMLHttpRequest();
    xhr.open("PUT", REST_API + "/Player", true);
    xhr.setRequestHeader('Content-Type', 'application/json');
    xhr.send(JSON.stringify({
      "heightOfUser": height.value,
      "weightOfUser": weight.value,
      "playerId": localStorage.getItem('playerId')
    }));
    xhr.onreadystatechange = function () { //Call a function when the state changes.
      if (xhr.readyState == 4 && xhr.status == 200) {
          createAnimation.classList.add("is-hidden");
          editPlayerForm.classList.remove("is-hidden");
          document.getElementById("statusMessage").classList.remove("is-hidden");
          ipcRenderer.send('player-from-edited-window');
          closeEditPlayerWindow();
        }
      }
    }
}


window.onload = init;

function init(){
  var height = document.getElementById("height");
  height.onblur =  function() {
    validateInput(this)
  }
  var weight = document.getElementById("weight");
  weight.onblur =  function() {
    validateInput(this)
  }

  var firstname = document.getElementById("firstname");
  var lastname = document.getElementById("lastname");
  var dateOfBirth = document.getElementById("dateOfBirth");

  var xhr = new XMLHttpRequest();
  var urlForGet = REST_API + "/Player" + "?playerId=" + localStorage.getItem('playerId');
  xhr.open("GET", urlForGet, true);
  xhr.setRequestHeader('Accept', 'application/json');
  xhr.send();
  xhr.onreadystatechange = function () { //Call a function when the state changes.
    if (xhr.readyState == 4 && xhr.status == 200) {
      obj = JSON.parse(xhr.responseText);
      firstname.placeholder = obj.firstName;
      lastname.placeholder = obj.lastName;
      dateOfBirth.placeholder = obj.dateOfBirth.substring(0,10);
      height.textContent = obj.heightOfUser;
      weight.textContent = obj.weightOfUser;
    }
  }
}
