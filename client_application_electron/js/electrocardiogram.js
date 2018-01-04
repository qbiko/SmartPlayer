function electrocardiogram(){
  var ekgGraph = document.getElementById('ekgGraph');

  function setYRange(range) {
    var min = 150;
    var max = 1000;
    return {min: min, max: max};
  }

  var smoothie = new SmoothieChart({
    interpolation:'linear',
    yRangeFunction: setYRange,
    grid: {
      strokeStyle: 'rgb(255,0,0)',
      fillStyle: 'rgb(255,255,255)',
      lineWidth: 1,
      millisPerLine: 250,
      verticalSections: 12
    },
    labels: {
      fillStyle: 'rgb(0, 0, 0)'
    }
  });

  smoothie.streamTo(ekgGraph);

  var ekgLine = new TimeSeries();
  var lastDateEkg = new Date();

  setInterval(
    function() {
      if(checkIfSomePlayerIsSelected()){
        var xhr = new XMLHttpRequest();
        var urlForGet = REST_API + "/Sensors/pulseBatchWithStartDate" + "?playerId=" + players.children[players.selectedIndex].value + "&gameId=" + sessionStorage.getItem('gameId')
        + "&startDateString=" + adjustDateForGet(JSON.stringify(lastDateEkg));
        xhr.open("GET", urlForGet, true);
        xhr.setRequestHeader('Accept', 'application/json');
        xhr.send();
        xhr.onreadystatechange = function () {
          if (xhr.readyState == 4 && xhr.status == 200) {
            obj = JSON.parse(xhr.responseText);
            for(var i = 0; i < obj.length; i++){
              var dateFromDB = new Date(obj[i].timeOfOccur);
              var delayForGraph = (DELAY/1000)+10;
              dateFromDB.setSeconds(dateFromDB.getSeconds() + delayForGraph);
              var dateForGraph = dateFromDB.getTime();

              ekgLine.append(dateForGraph, obj[i].value);
              smoothie.addTimeSeries(ekgLine,{lineWidth:2,strokeStyle:'#000000'});
              smoothie.streamTo(ekgGraph);
            }
            lastDateEkg = new Date(obj[obj.length-1].timeOfOccur);
          }
        }
      }
    }, DELAY);
  }
