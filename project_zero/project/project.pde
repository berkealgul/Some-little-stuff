// base class of all objects in game
class Object
{
    float x, y;
    float SPEED;
    float RAD; // radius for calculating collusion

    int scaleX, scaleY;
    
    PImage img;
    
    Object(float x_, float y_)
    {
        x = x_;
        y = y_;
        
        SPEED = 0;
        RAD = 20;
    }
    
    // move the object
    // default params are direction components
    void move(float dirX, float dirY)
    {
        x = x + (dirX * SPEED);
        y = y + (dirY * SPEED);
    }

    void show()
    {
        imageMode(CENTER);
        image(img, x, y);
    }

    // standart collision checking
    // objects are assumed to have circular hitbox
    // returns true if we collide, false otherwise
    boolean collides(Object go)
    {
        float d = dist(x, y, go.x, go.y); // distance between centers

        if(d < RAD + go.RAD)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}


class Rabbit extends Object
{
    int score;
    ArrayList<PImage> images;
    
    // if Rabbit moves backwards we need to rotate depend on dirScale
    int dirScale = 1;
    
    boolean stands;

    Rabbit(float _x, float _y)
    {
        super(_x, _y);

        // initialize score
        score = 0;

        // set up constants
        RAD = 20;
        SPEED = 2.5;
        
        loadImages();
    }
    
    //load images 
    void loadImages()
    {
         //get spritesheet
        img = loadImage("arts/bunny.png");
        
        // initialize sprites
        images = new ArrayList<PImage>(); 

        images.add(loadImage("arts/bunny.png").get(0,0,img.width/15, img.height));
        images.add(loadImage("arts/bunny.png").get(img.width/15, 0,img.width/15, img.height));
        images.add(loadImage("arts/bunny.png").get(2*img.width/15, 0,img.width/15, img.height));
        images.add(loadImage("arts/bunny.png").get(3*img.width/15, 0,img.width/15, img.height));
        images.add(loadImage("arts/bunny.png").get(4*img.width/15, 0,img.width/15, img.height));
        images.add(loadImage("arts/bunny.png").get(5*img.width/15, 0,img.width/15, img.height));
        images.add(loadImage("arts/bunny.png").get(6*img.width/15, 0,img.width/15, img.height));
        images.add(loadImage("arts/bunny.png").get(7*img.width/15, 0,img.width/15, img.height));
        images.add(loadImage("arts/bunny.png").get(8*img.width/15, 0,img.width/15, img.height));
        images.add(loadImage("arts/bunny.png").get(9*img.width/15, 0,img.width/15, img.height));
        images.add(loadImage("arts/bunny.png").get(10*img.width/15, 0,img.width/15, img.height));
        images.add(loadImage("arts/bunny.png").get(11*img.width/15, 0,img.width/15, img.height));
        images.add(loadImage("arts/bunny.png").get(12*img.width/15, 0,img.width/15, img.height));
        images.add(loadImage("arts/bunny.png").get(13*img.width/15, 0,img.width/15, img.height));
        images.add(loadImage("arts/bunny.png").get(14*img.width/15, 0,img.width/15, img.height));
        
        // resize sprites
        for(PImage i : images)
        {
         i.resize(int(2.5*i.width),  int(2.5*i.height)); 
        }
    }
    
    
    //this function stands for moving the Rabbit instance. dirX and dirY are direction components
    // öust be in range [-1,1]
    void move(float dirX, float dirY)
    {   
        if(dirX == 0 && dirY == 0)
        {
           stands = true; // stands still
           return;
        }
        else
          stands = false;
          
        // scale vector by speed
        float px = dirX * SPEED;
        float py = dirY * SPEED;
  
        // update location
        x = x + px;
        y = y + py;

        // if Rabbit goes out of canvas we reverse movement
        if(x < RAD || x > width || y < RAD || y > height)
        {
            x = x - px;
            y = y - py;
        }

        // if Rabbit moves backwards we shall rotate image
        if(px < 0)
        {
            dirScale = -1;
        }
        else
        {
            dirScale = 1;
        }
    }

    void show()
    {
        PImage img;
        push();
        // for animation we loop over images before displaing them
        if(!stands)
        {
          img = images.get(frameCount % images.size());
        }
        else
        {
          img = images.get(0);   
        }
        
        imageMode(CENTER);
        translate(x,y);
        scale(dirScale, 1);
        image(img, 0, 0);
        pop();
    }
}


class Fox extends Object
{
    float imgDirX, imgDirY;
    
    float vx, vy; // speed components
    
    // random direction wont change in every frame. Instead,
    // change direction after each specfied amount of frame
    float UPDATE_DIR_FRAME = 45;

    // we have images for running animation
    ArrayList<PImage> images;

    // if  moves backwards we need to rotate depend on dirScale
    int dirScale = 1;

    Fox(float _x, float _y)
    {
        super(_x, _y);

        SPEED = 1.5;
        RAD = 20;

        // load images for animation
        loadImages();
    }

    // check if we need to update random direction if needed update it
    void updateDir()
    {
        if(frameCount % UPDATE_DIR_FRAME == 0 || frameCount == 0)
        {
            float yaw = random(0, 6.38);
            vx = SPEED * cos(yaw);
            vy = SPEED * sin(yaw);
        }
    }

    void loadImages()
    {
        images = new ArrayList<PImage>(); 

        //load images based on known file locations
        images.add(loadImage("arts/fox/foxrun1.png"));
        images.add(loadImage("arts/fox/foxrun2.png"));
        images.add(loadImage("arts/fox/foxrun3.png"));
        images.add(loadImage("arts/fox/foxrun4.png"));
        images.add(loadImage("arts/fox/foxrun5.png"));
        images.add(loadImage("arts/fox/foxrun6.png"));
        images.add(loadImage("arts/fox/foxrun7.png"));
        images.add(loadImage("arts/fox/foxrun8.png"));

        // resize them for fitting in game
        for(PImage img : images)
        {
            img.resize(int(3*img.width),  int(3*img.height));
        }
    }

    // this will move Fox
    // px and py are Rabbit location components
    void move(float rx, float ry)
    {   
        // check random direction during main moving function
        updateDir();

        // update locations
        x = x + vx;
        y = y + vy;
         
         // if Fox goes out of canvas we will produce next direction
        if(x < RAD || x > width || y < 0 || y > height) 
        {
            x -= vx;
            y -= vy;
            updateDir();
        } 
        
        // update dirScale for mirroring
        if(vx < 0)
        {
            dirScale = -1;
        }
        else
        {
            dirScale = 1;
        }
    }

    void show()
    {
        push();
        // for animation we loop over images before displaing them
        PImage img = images.get(frameCount % images.size());
        imageMode(CENTER);
        translate(x,y);
        scale(dirScale, 1);
        image(img, 0, 0);
        pop();
    }
}


class Carrot extends Object
{
    
    Carrot(float _x, float _y)
    {
        super(_x, _y);
        
        RAD = 15;

        // load image for Carrot and resize it
        img = loadImage("arts/carrot.png");
        img.resize(int(3*RAD),  int(3*RAD));
    }
}


class CountDown
{
    int durationSeconds;
    int remainingTime;

    public CountDown(int duration)
    {
        // add millis/1000 offset for recording starting time
        durationSeconds = duration+(millis()/1000);
        remainingTime = durationSeconds;
    }

    // updates remaining time
    void tick() 
    { 
        //millis() processing command, returns time in 1000ths sec since program started
        remainingTime = max(0, durationSeconds - millis()/1000);
    }
}


// MAIN CLASS ---------------------------------------- //

Rabbit Rabbit; // main Rabbit

enum State {GAMEOVER, RUNNING, MENU}; // game states

State currentState; // current game state

// store all types of non playable objects
ArrayList<Object> objectList = new ArrayList<Object>();

int FOX_COUNT = 10;
int CARROT_COUNT = 20;
int px, py; // direction vector for Rabbit

// other objects shall not spawn near Rabbit
float SPAWN_OFFSET_TO_RABBIT = 70;

// objects shall not close to edge
float EDGE_OFFSET = 20;

// total game time in seconds
int GAME_TIME = 60;

PImage background;

CountDown cd;

void setup() 
{
    size(700,700);
    currentState = State.MENU; // we need to see menu first
    background = loadImage("arts/grass.png");
    background.resize(width, height);
}

void draw() 
{
    // show background
    imageMode(CORNER);
    image(background,0,0);

    switch(currentState)
    {
        case RUNNING:
            showGameScreen();
            moveObjects();
            checkForTerminate();
            break;

        case GAMEOVER:
            showGameScreen();
            showGameOver();
            break;

        case MENU:
            showMenu();
            break;

    }
}

// this function run necessary operations
// to determine if game should be terminated and restart over
// there is two ways to terminate.
// 1- game over -> losing
// 2- game beaten -> winning
void checkForTerminate()
{
    handleCollisions(); // checks if Rabbit hits Fox 
    checkGameBeaten(); // checks if all Carrots are collected
    checkTime(); // checks if time is over
}

// updates count timer and checks if time is over
void checkTime()
{
    cd.tick();

    if(cd.remainingTime == 0)
    {
        currentState = State.GAMEOVER;
    }
}

void moveObjects()
{
    for(Object go : objectList)
    {
        // we only need to move Fox class
        if(go instanceof Fox)
        {  
            go.move(Rabbit.x, Rabbit.y);
        }
    }

    Rabbit.move(px,py);
}

void showGameOver()
{
    fill(0);
    textSize(30);
    textAlign(CENTER,CENTER);

    // there is two ways to end the game. rather losing or winning
    // if Rabbit won the game we give different message
    if(isGameWon())
    {
        text("Good Job you collected them all! \n Press any key to restart",width/2,height/2);
    }
    else
    {
        text("Game Over and you failed! \n Press any key to restart",width/2,height/2);
    }
    
}

void showMenu()
{
    background(#0DFACC);
    fill(0);
    textSize(20);
    textAlign(CENTER,CENTER);
    text("Welcome \n Use arrows to move \nCollect carrots and avoid foxes \n Press any key to play",width/2,height/2);
}

void showGameScreen()
{
    //show objects
    for(Object go : objectList)
    {
        go.show();
    }

    Rabbit.show();

    // show score
    fill(0);
    textSize(25);
    textAlign(CENTER,UP);
    text("Score: "+str(Rabbit.score)+" Time: "+str(cd.remainingTime),width/2,20);
}

void handleCollisions()
{
    for(int i = 0; i < objectList.size(); i++)
    {   
        Object go = objectList.get(i);

        // if not colliding go to next object
        if(!Rabbit.collides(go))
        {
            continue;
        }

        // if we hit Fox, end the game
        if(go instanceof Fox)
        {
            currentState = State.GAMEOVER;
            break;
        }

        // if we hit Carrot, increase score and remove hitted object
        if(go instanceof Carrot)
        {
            Rabbit.score++;
            //remove object
            objectList.remove(i);
        }
    }
}

// checks if game is beaten
// if game is beaten we change game state
void checkGameBeaten()
{
    if(isGameWon())
    {
        currentState = State.GAMEOVER;
    }
}

// after Rabbit collects all Carrots.
// Rabbit wins or beats the game
boolean isGameWon()
{
    return Rabbit.score == CARROT_COUNT;
}

// clears Fox and Carrot lists
// and initialize them for new game
// you can tweak with value in this funtion
// NOTE: DO NOT CALL THIS BEFORE INITIALIZING Rabbit
void resetList()
{
    objectList.clear();

    for(int i = 0; i < FOX_COUNT+CARROT_COUNT; i++)
    {
        while(true)
        {
            float x = random(0, width);
            float y = random(0, height);

            // if coordinates are not too close to Rabbit 
            // we add to the list and break the loop
            // otherwise pick another random location
            boolean closeToRabbit = dist(Rabbit.x, Rabbit.y, x, y) < SPAWN_OFFSET_TO_RABBIT;
            
            //we shall not spawn objects close to edge
            boolean closeToEdge = x < EDGE_OFFSET || x > width-EDGE_OFFSET || y < EDGE_OFFSET || y > height-EDGE_OFFSET;
            
            if(!closeToRabbit && !closeToEdge)
            {
                // we add certain amount of Fox and Carrots
                if(i < FOX_COUNT)
                {
                    objectList.add(new Fox(x, y)); // add Fox
                }
                else
                {
                    objectList.add(new Carrot(x, y)); // add Carrot
                }

                break;
            }
        }
    }
}

// set default variables
void restartGame()
{
    currentState = State.RUNNING;
    Rabbit = new Rabbit(width/2, height/2);
    px = 0;
    py = 0;
    cd = new CountDown(GAME_TIME);
    resetList();
}

void keyPressed() 
{
    // differect actions based on current game state
    switch(currentState)
    {
        // take arrow inputs and set Rabbit directions
        case RUNNING:
            if (key != CODED) 
            {
                break;
            }

            if (keyCode == UP) 
            {
                py = -1;
            }
            else if (keyCode == DOWN) 
            {
                py = 1;
            }
            else if (keyCode == LEFT) 
            {
                px = -1;
            }
            else if (keyCode == RIGHT) 
            {
                px = 1;
            } 

            break;

        // restart game and prepeare for run again
        case GAMEOVER:
            currentState = State.RUNNING;
            restartGame();
            break;

        // start game
        case MENU:
            currentState = State.RUNNING;
            restartGame();
            break;
    }

}

void keyReleased() 
{
    if (key != CODED) 
    {
        return;
    }   

    if (keyCode == UP || keyCode == DOWN) 
    {
        py = 0;
    }
    else if (keyCode == RIGHT || keyCode == LEFT)
    {
        px = 0;
    } 
}
