int btnEen = 7;
int btnTwee = 8;
int btnDrie = 10;
int btnVier = 11;
int buttonStateEen;
int buttonStateTwee;
int buttonStateDrie;
int buttonStateVier;
int buttonStateSwitch;
int lastButtonState;
int btnSwitch = 12;
int VRx = A0;
int VRy = A1;
int xVal;
int yVal;

void setup()
{
  pinMode(btnEen, INPUT);
  pinMode(btnTwee, INPUT);
  pinMode(btnDrie, INPUT);
  pinMode(btnVier, INPUT);
  Serial.begin(9600);
  Serial.println("start");
  pinMode(VRx, INPUT);
  pinMode(VRy, INPUT);
  pinMode(btnSwitch, INPUT_PULLUP);
}

void loop()
{
  buttonStateEen = digitalRead(btnEen);
  buttonStateTwee = digitalRead(btnTwee);
  buttonStateDrie = digitalRead(btnDrie);
  buttonStateVier = digitalRead(btnVier);
  buttonStateSwitch = digitalRead(btnSwitch);
  xVal = analogRead(A0);
  yVal = analogRead(A1);
  Serial.println(yVal);
  Serial.println("Y is");

  Serial.println(xVal);
  Serial.println("X is");
  delay(100);

  if (buttonStateEen == HIGH)
  {
    Serial.println("Een");
  }
  else
  {

  }
  buttonStateEen = lastButtonState;
  delay(20);

  if (buttonStateTwee == HIGH)
  {
    Serial.println("Twee");
  }
  else
  {

  }
  buttonStateTwee = lastButtonState;

  if (buttonStateDrie == HIGH)
  {
    Serial.println("Drie");
  }
  else
  {

  }
  buttonStateDrie = lastButtonState;

  if (buttonStateVier == HIGH)
  {
    Serial.println("Vier");
  }
  else
  {

  }
  buttonStateVier = lastButtonState;

  if (buttonStateSwitch == LOW)
  {
    Serial.println("joystickButton");
  }
  else
  {

  }
  buttonStateSwitch = lastButtonState;
}
