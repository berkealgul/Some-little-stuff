class GameObject 
{
  float dyaw = 0.05;
  float x, y, yaw;
  float speed;
  float rad;
  float v, r; //v is forward vector y is rotation vector
  
  PImage img;
  
  GameObject(float x_, float y_)
  {
    x = x_;
    y = y_;
    yaw = 0;
    v = 0;
    r = 0;
  }

  void move()
  {
    yaw += r * dyaw;
    
    float dx = speed * v * cos(yaw);
    float dy = speed * v * sin(yaw);
    
    x += dx;
    y += dy;
    
    // if we pasa boundaries undo movement
    if(x < 0 || y < 0 || x > width || y > height)
    {
      x -= dx;
      y -= dy;
      hitBoundary();
    }
    
  }
  
  // called when boundary has hitted
  // so that response to boundary can be different fr each class
  void hitBoundary()
  {
  
  }
  
  void show()
  {
    pushMatrix();
    stroke(#E80707);
    translate(x, y);
    strokeWeight(5);
    rotate(yaw);
    imageMode(CENTER);
    image(img, 0, 0);
    popMatrix();
  }
  
  boolean collides(GameObject obj)
  {
    if(dist(x, y, obj.x, obj.y) < rad + obj.rad)
    {
      return true;
    }
    else
    {
      return false;
    }
  }
  
}

class Coin extends GameObject
{

  Coin(float x_, float y_)
  {
    super(x_, y_);
    rad = 10;
    img = loadImage("images/coin.png");
    img.resize(int(img.width/45), int(img.height/45));
  }
  
}

class Enemy extends GameObject
{
  float T = 100;
  float t;
  
  Enemy(float x_, float y_)
  {
    super(x_, y_);
    speed = 2;
    rad = 10;
    v = 1;
    changeMovement();
    img = loadImage("images/enemy.png");
    img.resize(int(img.width/3), int(img.height/3));
  }
  
  void move()
  {
    //old x y
    float x_ = x;
    float y_ = y; 
    
    super.move();
    
    t += dist(x_, y_, x, y);
     
    if(t > T)
    {
      t = 0;
      changeMovement();
    }
    
  }
  
  void hitBoundary()
  {
    changeMovement();
  }
  
  void changeMovement()
  {
    yaw = random(0, 6.28);
    t = 0;
  }

}

class Laser extends GameObject
{
  boolean hitted;
  
  Laser(float x_, float y_)
  {
    super(x_, y_);
    speed = 5;
    rad = 20;
    hitted = false;
    img = loadImage("images/laser.png");
    img.resize(int(img.width/10), int(img.height/10));
  }
  
  void move()
  {
    float x_ = x;
    float y_ = y;
    
    super.move();
    
    if(x-x_==0 && y-y_==0)
      hitted = true;
     
  }
  
  void hitBoundary()
  {
    hitted = true;
  }
}


class Player extends GameObject
{
  float score;
  boolean dead;
  
  Player(float x_, float y_)
  {
    super(x_, y_);
    speed = 2.75;
    dyaw = 0.075;
    score = 0;
    rad = 20;
    dead = false;
    img = loadImage("images/player.png");
    img.resize(int(img.width/3), int(img.height/3));
  }
  
  Laser shoot()
  {
    Laser l = new Laser(x, y);
    l.v = 1;
    l.r = 0;
    l.yaw = yaw;    
    return l;
  }
}

Player p;

enum GameState {GAMEOVER, RUNNING, MENU}; // game states

GameState currentState = GameState.MENU;

// store all types of non playable objects
ArrayList<GameObject> npcList;
ArrayList<Laser> laserList;
ArrayList<GameObject> coinList;

final int ENEMY_COUNT = 10;
final int COIN_COUNT = 5;

PImage background;

void setup()
{
  size(1000, 600);
  background = loadImage("images/space.jpg");
  background.resize(width, height);
}

void draw() 
{
    // draw background
    imageMode(CORNER);
    image(background,0,0);

    switch(currentState)
    {
        case RUNNING:
            update();
            showGameScreen();
            break;

        case GAMEOVER:
            showGameScreen();
            showGameOver();
            break;

        case MENU:
            drawMenu();
            break;

    }
}

void drawMenu()
{
    background(255);
    fill(0);
    textSize(20);
    textAlign(CENTER,CENTER);
    text("Welcome Captain\n Use arrows to move Shift to shoot\nCollect coins and destroy all enemy ships while avoid crashing them \n Press any key to play",width/2,height/2);
}

void showGameScreen()
{
  p.show();
  
  for(int i = 0, n = npcList.size(); i< n; i++)
  {
    GameObject npc = npcList.get(i);
    npc.show();
  }
  
  for(int i = 0, n = laserList.size(); i< n; i++)
  {
    Laser laser = laserList.get(i);
    laser.show();
  }
  
  for(int i = 0, n = coinList.size(); i< n; i++)
  {
    GameObject coin = coinList.get(i);
    coin.show();
  }
  
    // show score
    fill(255);
    textSize(25);
    textAlign(CENTER,UP);
    text("Score: "+str(p.score),width/2,20);
}

void showGameOver()
{
    fill(255);
    textSize(30);
    textAlign(CENTER,CENTER);

    // there is two ways to end the game. rather losing or winning
    // if player won the game we give different message
    if(p.dead)
    {
        text("Game Over and you lost! \n Press any key to restart",width/2,height/2);
        
    }
    else
    {
        text("Good Job you won! \n Press any key to restart",width/2,height/2);
    }
    
}

void handleCollusions()
{ //<>//
  //player with coıns
  for(int i = 0; i < coinList.size(); i++)
  {
    GameObject coin = coinList.get(i);
    
    if(p.collides(coin))
    {
      p.score++;
      coinList.remove(i);
    }
  }
  
  //player with npc (enemies)
  for(int i = 0; i < npcList.size(); i++)
  {
    GameObject npc = npcList.get(i);
    
    if(p.collides(npc))
    {
      p.dead = true;
      currentState = GameState.GAMEOVER;
    }
  }
  
  
  //npc with lasers
  for(int i = 0; i <  npcList.size(); i++)
  {
    GameObject npc = npcList.get(i);
    
    for(int j = 0; j< laserList.size(); j++)
    {
      Laser laser = laserList.get(j);
      if(npc.collides(laser))
      {
        coinList.add(new Coin(npc.x, npc.y));
        npcList.remove(i);
        laser.hitted = true;
      }
    }
  }
  
  for(int i = 0; i < laserList.size(); i++)
  {
    Laser laser = laserList.get(i);
    if(laser.hitted)
      laserList.remove(i);
  }
}

// update logical operations such as movement
void update()
{
  p.move();
  
  for(int i = 0, n = npcList.size(); i< n; i++)
  {
    GameObject npc = npcList.get(i);
    npc.move();
  }
  
  
  for(int i = 0, n = laserList.size(); i< n; i++)
  {
    Laser laser = laserList.get(i);
    laser.move();
  }
  
  handleCollusions();
  
  //check if all ships atre destroyed
  // if it is we won
  if(npcList.size() == 0)
  {
    currentState = GameState.GAMEOVER;
  }
}

void keyPressed() 
{
    // differect actions based on current game state
    switch(currentState)
    {
        // take arrow inputs and set player directions
        case RUNNING:
            if (key != CODED) 
            {
                break;
            }

            if (keyCode == UP) 
            {
                p.v = 1;
            }
            else if (keyCode == DOWN) 
            {
                p.v = -1;
            }
            else if (keyCode == LEFT) 
            {
                p.r = -1;
            }
            else if (keyCode == RIGHT) 
            {
                p.r = 1;
            }
            else if(keyCode == SHIFT)
            {
              laserList.add(p.shoot());
            }

            break;

        // restart game and prepeare for run again
        case GAMEOVER:
            currentState = GameState.RUNNING;
            restartGame();
            break;

        // start game
        case MENU:
            currentState = GameState.RUNNING;
            restartGame();
            break;
    }

}

void keyReleased() 
{
    // since we are dealing with arrows. we wont accept non coded key
    if (key != CODED) 
    {
        return;
    }   

    // if we relase keys we make respective direction component zero
    if (keyCode == UP || keyCode == DOWN) 
    {
        p.v = 0;
    }
    else if (keyCode == RIGHT || keyCode == LEFT)
    {
        p.r = 0;
    }
}

Enemy spawnEnemy()
{
  float x, y;
  
  while(true)
  {
    x = random(50, width-50);
    y = random(50, height-50);
    
    if(dist(x,y,p.x,p.y) > 50)
    {
      break;
    }
    
  }
  Enemy e = new Enemy(x, y);
  return e;
}

Coin spawnCoin()
{
  float x, y;
  
  while(true)
  {
    x = random(50, width-50);
    y = random(50, height-50);
    
    if(dist(x,y,p.x,p.y) > 50)
    {
      break;
    }
    
  }
  Coin c = new Coin(x, y);
  return c;
}

void restartGame()
{
  p = new Player(width/2,height/2);
  
  npcList = new ArrayList<GameObject>();
  coinList = new ArrayList<GameObject>();
  laserList = new ArrayList<Laser>();
  
  for(int i = 0; i < ENEMY_COUNT; i++)
  {
    npcList.add(spawnEnemy());
  }
  
  for(int i = 0; i < COIN_COUNT; i++)
  {
    coinList.add(spawnCoin());
  }
  
}