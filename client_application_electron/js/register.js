function isValidPassword(password, repeatedPassword) {
  var passwordMessage = document.getElementById("passwordMessage");
  if(password != repeatedPassword) {
    passwordMessage.classList.add("form-error");
    passwordMessage.textContent = "Podane hasła nie są takie same."
    return false;
  }
  if(password.length > 5 && repeatedPassword.length > 5) {
    if(hasNumbers(password) && hasNumbers(repeatedPassword)) {
      passwordMessage.classList.remove("form-error");
      passwordMessage.textContent = ""
      return true;
    }
  }
  passwordMessage.classList.add("form-error");
  passwordMessage.textContent = "Hasło musi zawierać cyfry, litery oraz składać się z co najmniej 6 znaków."
  return false;
}

function hasNumbers(word)
{
  return /\d/.test(word);
}

function isNotEmpty(someInput) {
  var clubNameMessage = document.getElementById("clubNameMessage");
	if(someInput == "") {
    clubNameMessage.classList.add("form-error");
    clubNameMessage.textContent = "Nazwa klubu nie może być pusta."
		return false;
	}
  else {
    clubNameMessage.classList.remove("form-error");
    clubNameMessage.textContent = ""
		return true;
	}
}

function register() {
  var clubName = document.getElementById("clubName");
  var password = document.getElementById("password");
  var repeatedPassword = document.getElementById("repeatedPassword");
  if(isNotEmpty(clubName.value) && isValidPassword(password.value, repeatedPassword.value)) {
    var registerForm = document.getElementById("registerForm");
    var createAnimation = document.getElementById("createAnimation");
    registerForm.classList.add("is-hidden");
    createAnimation.classList.remove("is-hidden");
    var xhr = new XMLHttpRequest();
    xhr.open("POST", REST_API + "/Main/register", true);
    xhr.setRequestHeader('Content-Type', 'application/json');
    xhr.send(JSON.stringify({
      "clubName": clubName.value,
      "password": password.value
    }));
    xhr.onreadystatechange = function () { //Call a function when the state changes.
      if (xhr.readyState == 4 && xhr.status == 200) {
          createAnimation.classList.add("is-hidden");
          registerForm.classList.remove("is-hidden");
          document.getElementById("statusMessage").classList.remove("is-hidden");
      }
    }
  }
}

window.onload = init;

function init(){
  var clubName = document.getElementById("clubName");
  clubName.onblur = function() {
    isNotEmpty(this.value);
  }
  var password = document.getElementById("password");
  password.onblur =  function() {
    isValidPassword(this.value, repeatedPassword.value)
  }
  var repeatedPassword = document.getElementById("repeatedPassword");
  repeatedPassword.onblur =  function() {
    isValidPassword(this.value, password.value)
  }
}
