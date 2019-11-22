int b;

void setup() {
  // put your setup code here, to run once:
Serial.begin(9600);
pinMode(9, OUTPUT);
}

void loop() {
  // put your main code here, to run repeatedly:
int a = digitalRead(9);
if (a!=b){
  if (a==1){
      Serial.println("ok");
    }
  }
//Serial.println(a);
b=a;
delay(20);
}
