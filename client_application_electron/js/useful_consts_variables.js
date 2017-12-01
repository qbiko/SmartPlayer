const REST_API = 'http://inzynierkawebapi.azurewebsites.net/api';
const DELAY = 10000;

Date.prototype.toJSON = function () {
  var timezoneOffsetInHours = -(this.getTimezoneOffset() / 60); //UTC minus local time
  var sign = timezoneOffsetInHours >= 0 ? '+' : '-';
  var leadingZero = (timezoneOffsetInHours < 10) ? '0' : '';

  var correctedDate = new Date(this.getFullYear(), this.getMonth(),
      this.getDate(), this.getHours(), this.getMinutes(), this.getSeconds(),
      this.getMilliseconds());
  correctedDate.setHours(this.getHours() + timezoneOffsetInHours);
  var iso = correctedDate.toISOString().replace('Z', '');

  return (iso + sign + leadingZero + Math.abs(timezoneOffsetInHours).toString() + ':00')
}

function adjustDateForGet(date){
  return date.replace(/:/g, "%3A").replace(/\+/g, "%2B").replace(/\"/g, "");
}
