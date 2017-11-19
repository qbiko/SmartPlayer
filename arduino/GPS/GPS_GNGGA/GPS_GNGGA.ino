#include "SoftwareSerial.h"

#define PC_BAUDRATE   9600
#define GPS_BAUDRATE  9600
#define GPS_RX        3
#define GPS_TX        2

SoftwareSerial ss(GPS_TX, GPS_RX);

byte command[5];
bool isAnalazing = true;
double lat;
double lon;
double utc; 

void setup() {
    Serial.begin(PC_BAUDRATE);
    ss.begin(GPS_BAUDRATE);
    
    configGPS();
}

void loop(){
  
  findLatAndLon();
  delay(20);
}

void configGPS(){

  //Ustawia częstotliwość GPS na 500ms
   byte packet[] = {
        0xB5, // sync char 1
        0x62, // sync char 2
        0x06, // class
        0x08, // id
        0x06, // length
        0x00, // length
        0xF4, // payload
        0x01, // payload
        0x01, // payload
        0x00, // payload
        0x01, // payload
        0x00, // payload
        0x0B, // CK_A
        0x77, // CK_B
    };
    
  for (byte i = 0; i < sizeof(packet); i++) {
        ss.write(packet[i]);
    }
 
}

void findLatAndLon(){
    lat = 0;
    lon = 0;
    utc = 0;
    encode();
    
    Serial.println(lat,8);
    Serial.println(lon,8);
    Serial.println(utc,2);    
    Serial.println();
}

void encode(){
  while(isAnalazing){
     if(read() != 36 ) continue; //36 = '$'

     for(int i = 0; i<5;i++){
      char b = read();
      while(b == 0){
          b = read();
      }
      command[i] = b;
     }

     if(!isCommandEquals()) continue;
     
     if(read() != 44) continue; // 44 = ','
      //==========================================UTC=============================================
      float multipler = 100000.0;
      
      while (true){
        byte r = read();
        if(r == 46) continue; //46 = '.'
        if(r == 44) break; //44 = ','
        
        utc += (r - 48) * multipler; // Kolejne cyfry czasu UTC
        multipler /= 10.0;
      } 
      //===========================================================================================

     //=======================================================================================
     //Wysokość w formacie ddmm.mm (i tak dalej m)

      byte first = read();
      lat += (first - 48) * 10; //Pierwsza cyfra wysokości

      
      byte sec = read();
      lat += (sec - 48); //Druga cyfra wysokości

      float rest = 0;

      multipler = 10.0;

      while (true){
        byte r = read();
        if(r == 46) continue; //46 = '.'
        if(r == 44) break; //44 = ','
        
        rest += (r - 48) * multipler; //Pierwsza cyfra wysokości (minuty)
        multipler /= 10.0;
      } 
      rest /= 60.0;
      lat += rest;
      
      if(read() == 83) //83 = 'S'
        lat *= -1;
      if(read() != 44) //44 = ','
        continue;
      //===========================================================================================
      //==========================================================================================
      //Szerokość w formacie dddmm.mm (i tak dalej m)
      
      first = read();
      lon += (first - 48) * 100; //Pierwsza cyfra szerokosci

      
      sec = read();
      lon += (sec - 48) * 10; //Druga cyfra szerokosci

      byte third = read();
      lon += (third - 48); //Trzecia cyfra szerokosci

      rest = 0;

      multipler = 10.0;

      while (true){
        byte r = read();
        if(r == 46) continue; //46 = '.'
        if(r == 44) break; //44 = ','
        
        rest += (r - 48) * multipler; //Kolejne cyfry szerokosci (minuty)
        multipler /= 10.0;
      } 
      rest /= 60.0;
      lon += rest;

      if(read() == 87) //87 = 'W'
        lon *= -1;

      if(read() != 44) //44 = ','
        break;
      //===========================================================================================
      if(read() == 86) //86 = 'V' = data invalid
        continue;

      
      break; //Wszystko ok
  }
}

byte read(){
   byte character = 0;
   do{
    if(ss.available()>0)
      character = ss.read();
   } while (character == 0);
  return character;
}

bool isCommandEquals(){
    return command[0] == 71 &&  // 71 = 'G'
            command[1] == 78 && // 78 = 'N'
            command[2] == 71 && // 71 = 'G'
            command[3] == 71 && // 71 = 'G'
            command[4] == 65;   // 65 = 'A'
}
