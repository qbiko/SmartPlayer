

#define PULSE_SENSOR_PIN 0

void setup() {
  // put your setup code here, to run once:

}

void loop() {
  // put your main code here, to run repeatedly:
  
}

int dataFromPulseSensor(){
    return analogRead(PULSE_SENSOR_PIN);
}
