#include <ESP8266HTTPClient.h>
#include <ESP8266WiFi.h>
#include <ArduinoJson.h>
#include <Ticker.h>
#include <SoftwareSerial.h> 

//==================Network settings==========================
#define ssid "ssid"
#define password "password"
#define wifiConnectionAttempts 10

//==================Pins definitions=============================
#define ERROR_LED 14
#define CONNECTION_LED 5
#define SEND_LED 4

#define RXPin 12
#define TXPin 13
#define GPSBaud 115200

//==================Constants for REST API========================
#define API_ADDRESS "http://inzynierkawebapi.azurewebsites.net/api/"
#define PULSE_ENDPOINT "Sensors/pulse"
#define GPS_ENDPOINT "Sensors/location"
#define CREDENTIALS_ENDPOINT "controller/getCredentials"

int playerId = 0;
int gameId = 0;
long startTime = 0;

// The Ticker/flipper routine
Ticker flipper;

//open software serial port for GPS connection
SoftwareSerial gpsSerial(RXPin, TXPin); 

// these variables are volatile because they are used during the interrupt service routine!
volatile int Signal;                // holds the incoming raw data
volatile int IBI = 600;             // holds the time between beats, must be seeded!
volatile boolean Pulse = false;

void setup() {
  //set each pin modes
  pinMode(CONNECTION_LED, OUTPUT);
  pinMode(SEND_LED, OUTPUT);
  pinMode(ERROR_LED, OUTPUT);

  //set voltage levels to LEDs
  digitalWrite(CONNECTION_LED, LOW);
  digitalWrite(SEND_LED, LOW);
  digitalWrite(ERROR_LED, HIGH);
  
  Serial.begin(115200);
  if(connectToWiFi()) {
    digitalWrite(ERROR_LED, LOW);
    Serial.println("");
    Serial.println("WiFi connected");  
    Serial.println("IP address: ");
    Serial.println(WiFi.localIP());
    Serial.println(WiFi.macAddress());
    boolean toggle = false;
    int attempt = 0;
    if(WiFi.status() == WL_CONNECTED) {
      while((!getCredentials()) && (attempt < 50)) {
        if(toggle) {
          digitalWrite(CONNECTION_LED, HIGH);
          toggle = false;
        }
        else {
          digitalWrite(CONNECTION_LED, LOW);
          toggle = true;
        }
        attempt++;
        delay(100);
      }
    }
  }
  if(startTime != 0) {
    digitalWrite(CONNECTION_LED, HIGH);
    interruptSetup();
  }
}

boolean connectToWiFi() {
  Serial.print("Connecting to ");
  Serial.println(ssid);
  WiFi.mode(WIFI_STA);
  WiFi.begin(ssid, password);
  int attempt = 0;
  while (WiFi.status() != WL_CONNECTED && attempt++ < wifiConnectionAttempts) {
    delay(500);
    Serial.print(".");
  }
  return WiFi.status() == WL_CONNECTED;
}

void loop() {
  digitalWrite(SEND_LED, LOW);
  if(WiFi.status() == WL_CONNECTED && (startTime != 0)) {
    //sendGPSData(LAT, LNG);
    sendPulseData(Signal);
  }
  else {
    digitalWrite(CONNECTION_LED, LOW);
    digitalWrite(ERROR_LED, HIGH);
  }
  delay(1);
}

boolean getCredentials() {
  Serial.println("Getting credentials...");
  String response;
  boolean responseCode = sendGetRequest(CREDENTIALS_ENDPOINT, "mac=" + WiFi.macAddress(), response);
  if(responseCode) {
    StaticJsonBuffer<200> jsonBuffer;
    JsonObject& credentials = jsonBuffer.parseObject(response);
    playerId = credentials["playerId"];
    gameId = credentials["gameId"];
    startTime = credentials["serverTime"];
    startTime -= millis();
    Serial.printf("Czas startu po synchronizacji: %d\n", startTime);
    Serial.println(response);
  }
  return responseCode;
}

boolean sendGPSData(double latitude, double longitude) {
  StaticJsonBuffer<200> jsonBuffer;
  JsonObject& gpsData = jsonBuffer.createObject();
  gpsData["lat"] = latitude;
  gpsData["lng"] = longitude;
  gpsData["playerId"] = playerId;
  gpsData["gameId"] = gameId;
  String jsonStr;
  gpsData.printTo(jsonStr);
  String response;
  boolean responseCode = sendPostRequest(GPS_ENDPOINT, jsonStr, response);
  digitalWrite(SEND_LED, HIGH);
  //Serial.println(response);
  return responseCode;
}

boolean sendPulseData(int data) {
  StaticJsonBuffer<200> jsonBuffer;
  JsonObject& pulseData = jsonBuffer.createObject();
  pulseData["playerId"] = playerId;
  pulseData["value"] = data;
  pulseData["gameId"] = gameId;
  String jsonStr;
  pulseData.printTo(jsonStr);
  String response;
  boolean responseCode = sendPostRequest(PULSE_ENDPOINT, jsonStr, response);
  digitalWrite(SEND_LED, HIGH);
  //Serial.println(response);
  return responseCode;
}

boolean sendGetRequest(String endpoint, String query, String& response) {
  HTTPClient http;
  http.begin(API_ADDRESS + endpoint + '?' + query);
  http.addHeader("Content-Type", "application/json");
  int httpCode = http.GET();
  Serial.println(httpCode);
  if(httpCode > 0) {
    response = http.getString();
    if(httpCode == HTTP_CODE_OK) {
      return true;
    }
    else {
      return false;
    }
  }
  else {
    response = http.errorToString(httpCode);
    return false;
  }
}

boolean sendPostRequest(String endpoint, String body, String& response) {
  HTTPClient http;
  http.begin(API_ADDRESS + endpoint);
  http.addHeader("Content-Type", "application/json");
  int httpCode = http.POST(body);
  Serial.println(endpoint + ": " + httpCode);
  if(httpCode > 0) {
    response = http.getString();
    if(httpCode == HTTP_CODE_OK) {
      return true;
    }
    else {
      return false;
    }
  }
  else {
    response = http.errorToString(httpCode);
    return false;
  }
}
