#include <Arduino_JSON.h>
#include <SPI.h>
#include <SD.h>

#define SW 2
#define joy_x A6
#define joy_y A7

void setup() {
  // Initialize serial communication
  Serial.begin(9600);

  // Open the data file in write mode

  // Initialize pins
  pinMode(SW, INPUT_PULLUP);
  pinMode(joy_x, INPUT);
  pinMode(joy_y, INPUT);
}

void loop() {
  // Read joystick values
  float joy_rx = analogRead(joy_x);
  float joy_ry = analogRead(joy_y);

  // Map joystick values to desired range
  joy_rx = map(joy_rx, 0, 1023, -500, 500);
  joy_ry = map(joy_ry, 0, 1023, -500, 500);

  // Print
  Serial.print(joy_rx);
  Serial.print(",");
  Serial.print(joy_ry);
  Serial.print(",");
  Serial.println(!digitalRead(SW));


  // Delay for stability
  delay(100);
}
