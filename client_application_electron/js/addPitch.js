const {remote, ipcRenderer} = require('electron')

function closeAddPitchWindow(){
  localStorage.removeItem('clubId');
  ipcRenderer.send('add-pitch-window');
  ipcRenderer.send('new-pitch-from-add-window');
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

function validateLongLat(input){
  var spanMessage = document.getElementById("latLongMessage");
  if (input.checkValidity() == false) {
    spanMessage.classList.add("form-error");
    spanMessage.textContent = "Podane współrzędne są nieprawidłowe. Przykładowa długość/szerokość geogr.: 6.653572";
    return false;
  }
  else {
    spanMessage.classList.remove("form-error");
    spanMessage.textContent = ""
    return true;
  }
}

function addPitch() {
  var pitchName = document.getElementById("pitchName");
  var luLatValue = document.getElementById("luLatValue");
  var ldLatValue = document.getElementById("ldLatValue");
  var ruLatValue = document.getElementById("ruLatValue");
  var rdLatValue = document.getElementById("rdLatValue");
  var zipCode = document.getElementById("zipCode");
  var city = document.getElementById("city");
  var street = document.getElementById("street");
  var statusMessage = document.getElementById("statusMessage");
  if(validateInput(pitchName) && validateInput(zipCode) && validateInput(city) && validateInput(street) && validateLongLat(luLatValue)
 && validateLongLat(ldLatValue) && validateLongLat(ruLatValue) && validateLongLat(rdLatValue)) {
    var addPitchForm = document.getElementById("addPitchForm");
    var createAnimation = document.getElementById("createAnimation");
    addPitchForm.classList.add("is-hidden");
    createAnimation.classList.remove("is-hidden");
    var xhr = new XMLHttpRequest();
    xhr.open("POST", "http://inzynierkawebapi.azurewebsites.net/api/Pitch", true);
    xhr.setRequestHeader('Content-Type', 'application/json');
    xhr.send(JSON.stringify({
      "nameOfPitch": pitchName.value,
      "street": street.value,
      "city": city.value,
      "zip": zipCode.value,
      "leftUpPoint": {
        "lat": luLatValue.value,
        "lng": luLongValue.value
      },
      "leftDownPoint": {
        "lat": ldLatValue.value,
        "lng": ldLongValue.value
      },
      "rightUpPoint": {
        "lat": ruLatValue.value,
        "lng": ruLongValue.value
      },
      "rightDownPoint": {
        "lat": rdLatValue.value,
        "lng": rdLongValue.value
      }
    }));
    xhr.onreadystatechange = function () { //Call a function when the state changes.
      if (xhr.readyState == 4 && xhr.status == 200) {
          createAnimation.classList.add("is-hidden");
          addPitchForm.classList.remove("is-hidden");
          document.getElementById("statusMessage").classList.remove("is-hidden");
        }
      }
    }
}
window.onload = init;

function init(){
  var pitchName = document.getElementById("pitchName");
  pitchName.onblur = function() {
    validateInput(this);
  }

  var luLatValue = document.getElementById("luLatValue");
  luLatValue.onblur = function() {
    validateLongLat(this);
  }
  var ldLatValue = document.getElementById("ldLatValue");
  ldLatValue.onblur = function() {
    validateLongLat(this);
  }
  var ruLatValue = document.getElementById("ruLatValue");
  ruLatValue.onblur = function() {
    validateLongLat(this);
  }
  var rdLatValue = document.getElementById("rdLatValue");
  rdLatValue.onblur = function() {
    validateLongLat(this);
  }

  var zipCode = document.getElementById("zipCode");
  zipCode.onblur = function() {
    validateInput(this);
  }

  var city = document.getElementById("city");
  city.onblur = function() {
    validateInput(this);
  }

  var street = document.getElementById("street");
  street.onblur = function() {
    validateInput(this);
  }


}
