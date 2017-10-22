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

function login() {
  var clubName = document.getElementById("clubName");
  var password = document.getElementById("password");
  var statusMessage = document.getElementById("statusMessage");
  if(isNotEmpty(clubName) && isNotEmpty(password)) {
    var loginForm = document.getElementById("loginForm");
    var createAnimation = document.getElementById("createAnimation");
    loginForm.classList.add("is-hidden");
    createAnimation.classList.remove("is-hidden");
    var xhr = new XMLHttpRequest();
    var urlForGet = "http://inzynierkawebapi.azurewebsites.net/api/Main/login" + "?clubName=" + clubName.value + "&password=" + password.value
    xhr.open("GET", urlForGet, true);
    xhr.setRequestHeader('Accept', 'application/json');
    xhr.send();
    xhr.onreadystatechange = function () { //Call a function when the state changes.
      if (xhr.readyState == 4 && xhr.status == 200) {
        obj = JSON.parse(xhr.responseText);
        if(obj.success) {
          sessionStorage.setItem('clubId', obj.id);
          sessionStorage.setItem('clubName', clubName.value);
          window.resizeTo(1400, 1000);
          window.location.href = 'file://' + __dirname + '/match_view.html';
        }
        else {
          statusMessage.textContent = "Nieprawidłowa nazwa klubu lub hasło."
        }
        statusMessage.classList.remove("is-hidden");
        createAnimation.classList.add("is-hidden");
        loginForm.classList.remove("is-hidden");
      }
    }
  }
}

window.onload = init;

function init(){
  var clubName = document.getElementById("clubName");
  clubName.onblur = function() {
    isNotEmpty(this);
  }
  var password = document.getElementById("password");
  password.onblur =  function() {
    isNotEmpty(this)
  }
}
