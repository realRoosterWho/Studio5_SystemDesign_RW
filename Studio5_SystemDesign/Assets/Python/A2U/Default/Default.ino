#include <Wire.h>
#include <FastLED.h>
#include <Arduino_JSON.h>
#include <SPI.h>
#include <SD.h>

#define NUM_LEDS 9
#define DATA_PIN 7
CRGB leds[NUM_LEDS];
CRGB white(255, 255, 255);
CRGB blue(0, 0, 255);

#define ACCEL_CONFIG 1
#define GYRO_CONFIG 1
#define G 9.8

#define SW 2
#define joy_x A6
#define joy_y A7
int Menu = 3, Down = 4, Up = 5, Grip = 6, Light = 7;
int Trig = A3, MPUSCL = A5, MPUSDA = A4;
int myTrig = 0, myMenu = 0, myDown = 0, myUp = 0, myGrip = 0;
float val_seven[7]; // Array to hold sensor data

void setup() {
    delay(1000);
    Serial.begin(9600);
    Wire.begin(); // Initialize I2C communication for MPU6050
    MPU_START(); // Initialize MPU6050
    GYRO_CONFIG_SET(0); // Configure gyro sensitivity
    ACCEL_CONFIG_SET(0); // Configure accelerometer sensitivity

    // Initialize LEDs
    pinMode(SW, INPUT_PULLUP);
    pinMode(joy_x, INPUT);
    pinMode(joy_y, INPUT);
    FastLED.setBrightness(100);
    FastLED.addLeds<WS2811, DATA_PIN, RGB>(leds, NUM_LEDS);
    allLight(white);
}

void loop() {
    Get_Value(); // Get MPU6050 sensor data

    // Joystick and button data
    int TrigNum = analogRead(Trig);
    myTrig = (TrigNum < 10) ? 1 : 0;
    myMenu = digitalRead(Menu) ? 1 : 0;
    myDown = digitalRead(Down) ? 1 : 0;
    myUp = digitalRead(Up) ? 1 : 0;
    myGrip = digitalRead(Grip) ? 1 : 0;
    float joy_rx = analogRead(joy_x);
    float joy_ry = analogRead(joy_y);
    joy_rx = map(joy_rx, 0, 1023, -500, 500);
    joy_ry = map(joy_ry, 0, 1023, -500, 500);

    // Print all data in one line separated by commas
    Serial.print(joy_rx); Serial.print(",");
    Serial.print(joy_ry); Serial.print(",");
    Serial.print(!digitalRead(SW)); Serial.print(",");
    Serial.print(myTrig); Serial.print(",");
    Serial.print(myMenu); Serial.print(",");
    Serial.print(myDown); Serial.print(",");
    Serial.print(myUp); Serial.print(",");
    Serial.print(myGrip); Serial.print(",");
    // MPU6050 data
    Serial.print(val_seven[0]); Serial.print(","); // acc_x
    Serial.print(val_seven[1]); Serial.print(","); // acc_y
    Serial.print(val_seven[2]); Serial.print(","); // acc_z
    Serial.print(val_seven[3]); Serial.print(","); // temp
    Serial.print(val_seven[4]); Serial.print(","); // gyr_x
    Serial.print(val_seven[5]); Serial.print(","); // gyr_y
    Serial.println(val_seven[6]); // gyr_z

    allLight(white);
    delay(100);
    allLight(blue);
}

void allLight(CRGB color) {
    for (int i = 0; i < NUM_LEDS; i++) {
        leds[i] = color;
    }
    FastLED.show();
}

// Define other functions (MPU_START, GYRO_CONFIG_SET, ACCEL_CONFIG_SET, Get_Value) here
void MPU_START(void)
{
   Wire.beginTransmission(0x68); //开启MPU6050的传输
   Wire.write(0x6B); //指定寄存器地址
   Wire.write(0); //写入一个字节的数
   Wire.endTransmission(true); //结束传输，true表示释放总线
}
//配置角速度倍率
void GYRO_CONFIG_SET(int f)
{   
   Wire.beginTransmission(0x68); //开启MPU-6050的传
   Wire.write(0x1B); //角速度倍率寄存器的地址
   Wire.requestFrom(0x68, 1, true); //先读出原配置
   unsigned char acc_conf = Wire.read();
   acc_conf = ((acc_conf & 0xE7) | (f << 3));
   Wire.write(acc_conf);
   Wire.endTransmission(true); //结束传输，true表示释放总线
}
//配置加速度倍率
void ACCEL_CONFIG_SET(int f)
{
   Wire.beginTransmission(0x68); //开启MPU-6050的传输
   Wire.write(0x1C); //加速度倍率寄存器的地址
   Wire.requestFrom(0x68, 1, true); //先读出原配置
   unsigned char acc_conf = Wire.read();
   acc_conf = ((acc_conf & 0xE7) | (f << 3));
   Wire.write(acc_conf);
   Wire.endTransmission(true); //结束传输，true表示释放总线
}
//获取MPU数据
void Get_Value(void)
{
   //获取各个轴分量数据
   Wire.beginTransmission(0x68); //开启MPU-6050的传输
   Wire.write(0x3B);
   Wire.requestFrom(0x68, 7 * 2, true);
   Wire.endTransmission(true);
   for (long i = 0; i < 7; ++i) 
   {
      val_seven[i] = Wire.read() << 8 | Wire.read();
   }
   //数据换算
   val_seven[0] = (float)(val_seven[0] / 32768 * (2 ^ ACCEL_CONFIG) *
G);//acc_x
   val_seven[1] = (float)(val_seven[1] / 32768 * (2 ^ ACCEL_CONFIG) * G);//acc_y
   val_seven[2] = (float)(val_seven[2] / 32768 * (2 ^ ACCEL_CONFIG) *
G);//acc_z
   val_seven[3] = (float)(val_seven[3] / 340 + 36.53);//acc_z
   val_seven[4] = (float)(val_seven[4] / 32768 * (2 ^ GYRO_CONFIG) *
250);//gyr_x
   val_seven[5] = (float)(val_seven[5] / 32768 * (2 ^ GYRO_CONFIG) *
250);//gyr_y
   val_seven[6] = (float)(val_seven[6] / 32768 * (2 ^ GYRO_CONFIG) *
250);//gyr_z
}