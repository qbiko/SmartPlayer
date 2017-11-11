#include "SoftwareSerial.h"

#define PC_BAUDRATE   19200
#define GPS_BAUDRATE  19200
#define GPS_RX        3
#define GPS_TX        2

SoftwareSerial ss(GPS_TX, GPS_RX);

byte command[5];
bool isAnalazing = true;
double lat;
double lon;
double utc; 

void setup() {
}

void loop(){
  
  findLatAndLon();
  delay(20);
}


void findLatAndLon(){
    Serial.begin(PC_BAUDRATE);
    ss.begin(GPS_BAUDRATE);
    
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

     //=======================================================================================
     //Wysokość w formacie ddmm.mm (i tak dalej m)

      byte first = read();
      lat += (first - 48) * 10; //Pierwsza cyfra wysokości

      
      byte sec = read();
      lat += (sec - 48); //Druga cyfra wysokości

      float rest = 0;

      float multipler = 10.0;

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
      //==========================================================================================
      multipler = 100000.0;
      
      while (true){
        byte r = read();
        if(r == 46) continue; //46 = '.'
        if(r == 44) break; //44 = ','
        
        utc += (r - 48) * multipler; // Kolejne cyfry czasu UTC
        multipler /= 10.0;
      } 

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
            command[3] == 76 && // 76 = 'L'
            command[4] == 76;   // 76 = 'L'
}
