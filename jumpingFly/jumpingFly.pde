/////// constants ////////

//lenght and sizes
final float L_LONG_HAND = 270;
final float L_SHORT_HAND = 200;
final float S_DOT = 40;
final float MIN_S_FLY = 40;
final float MAX_S_FLY = 50;
final float MAX_LONG_HAND_SPEED = 0.1;
final float MAX_SHORT_HAND_SPEED = 0.05;

final int FLY_JUMP_DUR = 50;
final float DOT_SPACING = 280;
final float MAX_Z = 30;
final float ANGLE_TRESH = 0.1; // minimum angle between fly and hand for collusion

/*
* S = Stop
* U = Up
* L = Left
* R = Right
* D = Down
*/
enum FlyMotionState{S, U, L, R, D};
FlyMotionState motionState;

////// non-constant global variables//////

Hand longHand, shortHand;
Dot dots[];
Fly fly;

//these angular speed are radians
//note that they are not constants
//since speeds will increase as the game proceeds
float long_hand_speed = 0.02;
float short_hand_speed = 0.01;

int score; //count of successful jumps
boolean isGameOver;

////// main functions ///////

void setup()
{
  size(600,600);
  frameRate(60);
  initFly();
  initHands(0);
  initDots();
  score = 0;
  isGameOver = false;
  motionState = FlyMotionState.S;
}
  
void draw()
{
  translate(width/2, height/2); //translate grid to center of the canvas
  background(#34F700);
  
  drawDots();
  drawScore();
  
  //if flys height exceeds hands it should be looking above
  if(fly.z > MAX_Z/4)
  {
    drawHands();
    fly.draw();
  }
  else
  {
    fly.draw();
    drawHands();
  }
  
  if(!isGameOver)
  {
    update();
  }
  else
  {
     drawEndMsg(); 
  }
  
}

//update game logic
void update()
{
  moveHands();
  
  if(motionState != FlyMotionState.S)
  {
    moveFly();
  }
  
  increaseDifficulty();
  isGameOver = isFlyColliding();
  
}

//////// sub functions used for drawing //////////

void drawHands()
{ 
  //extra circle in the center for better look
  noStroke();
  fill(#F70008);
  ellipse(0,0,25,25);
  
  longHand.draw();
  shortHand.draw(); 
}

void drawDots()
{
   for(Dot d : dots) //<>//
   {
      d.draw();
   }
}

void drawEndMsg()
{
  textSize(height/10);
  fill(0);
  String message = "GAME OVER!";
  text(message, (-textWidth(message))/2, (textAscent() 
                  -textDescent())/2); 
}

void drawScore()
{
  textSize(height/20);
  fill(0);
  String message = "Number of jumps: " + score; text(message, (-textWidth(message))/2, textAscent()-height/2+10); 
  
}

//////// sub functions for updating game logic ////////

void moveHands()
{
  longHand.move(long_hand_speed);
  shortHand.move(short_hand_speed);
}

void moveFly()
{
  float vel = DOT_SPACING / FLY_JUMP_DUR;
  fly.jumpOffset += vel;
  
  switch(motionState)
  {
   case L:
     fly.x -= vel;
     break;
   case R:
     fly.x += vel;
     break;
   case U:
     fly.y -= vel;
     break;
   case D:
     fly.y += vel;
     break;
  }
  
  if(fly.timeInAir != FLY_JUMP_DUR)
  {
      fly.z = 2*MAX_Z*(1-fly.jumpOffset/DOT_SPACING);
      fly.timeInAir++;
  }
  else
  {
    fly.timeInAir = 0;
    fly.jumpOffset = 0;
    motionState = FlyMotionState.S;
    score++;
    
    alingFlyWithDot();
  }
  
}

boolean isFlyColliding()
{
   return fly.isColliding(shortHand, ANGLE_TRESH) ||
           fly.isColliding(longHand, ANGLE_TRESH);
  
}

//when fly has laned we must make sure flys pos
//must be equal to laned dot so we will set flys
//pos to closest dot which it laned on
void alingFlyWithDot()
{
   float dis = dist(fly.x, fly.y, dots[0].x, dots[0].y);
   Dot closest = dots[0];
   
   for(Dot d : dots)
   {
     float dis_ = dist(fly.x, fly.y, d.x, d.y);
     if(dis_ < dis)
     {
      dis = dis_;
      closest = d;
     }
   }
   
   fly.x = closest.x;
   fly.y = closest.y;
}

void keyPressed()
{
  if(motionState != FlyMotionState.S)
  {
     return;
  }
  
  float d = DOT_SPACING/2; 
  
  //while we looking for key we also need to
  //be sure if the flys motion is valid
  //for instance if are in upper dots
  //we cant move up
  if((key=='W' || key=='w') && fly.y != -d)
  {
     motionState = FlyMotionState.U; 
  }
  if((key=='S' || key=='s') && fly.y != d)  
  {
     motionState = FlyMotionState.D; 
  }
  if((key=='A' || key=='a') && fly.x != -d)
  {
     motionState = FlyMotionState.L; 
  }
  if((key=='D' || key=='d') && fly.x != d)
  {
     motionState = FlyMotionState.R; 
  }
}

void increaseDifficulty()
{
  //both hands will hit max speed simultaneously 
  if(long_hand_speed > MAX_LONG_HAND_SPEED)
  {
     return; 
  }
  
  if(score < 10)
  {
     return; 
  }
  
  if(score > 40)
  {
     long_hand_speed += 0.0002; 
     short_hand_speed += 0.0002; 
  }
  else
  {
    long_hand_speed += 0.0001; 
     short_hand_speed += 0.0001; 
  }
}


//////// sub functions used in setup() /////////

void initDots()
{
  dots = new Dot[4];
  
  int d = int(DOT_SPACING/2); // placeholder derived from constant
  /*
  * indices of dots start from top right 
  * end in top left
  * also dots are placed to array in clockwise order
  */
  dots[0] = new Dot(d,d,S_DOT);
  dots[1] = new Dot(d,-d,S_DOT);
  dots[2] = new Dot(-d,-d,S_DOT);
  dots[3] = new Dot(-d,d,S_DOT);
}

// a_i : starting angle in radians
void initHands(float a_i)
{
  longHand = new Hand(L_LONG_HAND, a_i);
  shortHand = new Hand(L_SHORT_HAND, a_i);
}

void initFly()
{
  float d = DOT_SPACING/2;
  
  //pick random stating position
  float x = d*(2*int(random(2))-1);
  float y = d*(2*int(random(2))-1);
  
  fly = new Fly(int(x), int(y), MIN_S_FLY, MAX_S_FLY, MAX_Z); //<>//
  
}
