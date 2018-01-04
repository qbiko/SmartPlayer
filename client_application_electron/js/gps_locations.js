function gpsLocations() {
  var lastDateGPS = new Date();

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
      if(playersIds.length!=0){
        var xhr = new XMLHttpRequest();
        var urlForGet = REST_API + "/Sensors/locationsBatch" + "?gameId=" + sessionStorage.getItem('gameId') + "&startDateString="
        + adjustDateForGet(JSON.stringify(lastDateGPS)) + "&playerIds=" + playersIds.toString();
        xhr.open("GET", urlForGet, true);
        xhr.setRequestHeader('Accept', 'application/json');
        xhr.send();
        xhr.onreadystatechange = function () {
          if (xhr.readyState == 4 && xhr.status == 200) {
            obj = JSON.parse(xhr.responseText);
            for(var i=0;i<obj.length;i++){
              for(var j=0;j<obj[i].listOfPositions.length;j++){
                var timeout = 0;
                if(j!=0) timeout = new Date(obj[i].listOfPositions[j].timeOfOccur).getTime() - new Date(obj[i].listOfPositions[j-1].timeOfOccur).getTime();
                setTimeout(setPosition(obj[i].listOfPositions[j].x, obj[i].listOfPositions[j].y, obj[i].playerId),timeout);
              }
              var dateFromDB = new Date(obj[i].listOfPositions[obj[i].listOfPositions.length-1].timeOfOccur);
              if(dateFromDB.getTime()>lastDateGPS.getTime()){
                lastDateGPS=dateFromDB;
              }
            }
          }
        }
      }
    }, DELAY);
}

function setPosition(x,y,playerId){
  document.getElementById("badge-on-field-" + playerId).setAttribute("style", "position: absolute; top: "+ y + "px; left: " + x +"px;");
}
