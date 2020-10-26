

class Hand
{
 float l; //lenght of hand
 float a; //initial angle of hand (radians)
 
 Hand(float l_, float a_)
 {  
    l = l_;
    a = a_;
 }
 
 // function assumes one leg of hand is in (0,0) point
 // and you can change drawing params (color stroke ect.) in here
 void draw()
 {
   int x = int(l * cos(a));
   int y = int(l * sin(a));
   
   strokeWeight(10);
   stroke(#F70008);
   noFill();
   
   line(0,0,x,y);
 }
 
 //update angle by RADIANS
 void move(float da)
 {
   a = (a + da) % TWO_PI;
 }
  
}
