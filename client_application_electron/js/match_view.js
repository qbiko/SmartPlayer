window.onload = init;

function init(){
  var clubName = document.getElementById("clubName");
  clubName.textContent = sessionStorage.getItem('clubName');
}
