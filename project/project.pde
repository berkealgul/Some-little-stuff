Player player; // main player

enum GameState {GAMEOVER, RUNNING, MENU}; // game states

GameState currentState; // current game state

// store all types of non playable objects
ArrayList<GameObject> objectList = new ArrayList<GameObject>();

int ENEMY_COUNT = 10;
int COLLECTABLE_COUNT = 20;
int vx, vy; // direction vector for player

// other objects shall not spawn near player
// hence we set offset to the player
float SPAWN_OFFSET_TO_PLAYER = 70;

// objects shall not close to edge
// this determines min distance to edge
float EDGE_OFFSET = 20;

// total game time in seconds
int TIME = 60;

PImage background;

CountDown cd;

void setup() 
{
    size(700,700);
    currentState = GameState.MENU; // we need to see menu first
    background = loadImage("images/grass.png");
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
            drawGameScreen();
            moveObjects();
            checkForTerminate();
            break;

        case GAMEOVER:
            drawGameScreen();
            drawGameOver();
            break;

        case MENU:
            drawMenu();
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
    handleCollisions(); // checks if player hits enemy 
    checkGameBeaten(); // checks if all collectables are collected
    checkTime(); // checks if time is over
}

// updates count timer and checks if time is over
void checkTime()
{
    cd.tick();

    if(cd.remainingTime == 0)
    {
        currentState = GameState.GAMEOVER;
    }
}

void moveObjects()
{
    for(GameObject go : objectList)
    {
        // we only need to move enemy class
        if(go instanceof Enemy)
        {  
            go.move(player.x, player.y);
        }
    }

    player.move(vx,vy);
}

void drawGameOver()
{
    fill(0);
    textSize(20);
    textAlign(CENTER,CENTER);

    // there is two ways to end the game. rather losing or winning
    // if player won the game we give different message
    if(isGameBeaten())
    {
        text("Good Job you won! \n Press any key to restart",width/2,height/2);
    }
    else
    {
        text("Game Over and you lost! \n Press any key to restart",width/2,height/2);
    }
    
}

void drawMenu()
{
    background(255);
    fill(0);
    textSize(20);
    textAlign(CENTER,CENTER);
    text("Welcome \n Use arrows to move \nCollect carrots and avoid foxes \n Press any key to play",width/2,height/2);
}

void drawGameScreen()
{
    //draw objects
    for(GameObject go : objectList)
    {
        go.draw();
    }

    player.draw();

    // draw score
    fill(0);
    textSize(25);
    textAlign(CENTER,UP);
    text("Score: "+str(player.score)+" Time: "+str(cd.remainingTime),width/2,20);
}

void handleCollisions()
{
    for(int i = 0; i < objectList.size(); i++)
    {   
        GameObject go = objectList.get(i);

        // if not colliding go to next object
        if(!player.collidesWith(go))
        {
            continue;
        }

        // if we hit enemy, end the game
        if(go instanceof Enemy)
        {
            currentState = GameState.GAMEOVER;
            break;
        }

        // if we hit collectable, increase score and remove hitted object
        if(go instanceof Collectable)
        {
            player.score++;
            //remove object
            objectList.remove(i);
        }
    }
}

// checks if game is beaten
// if game is beaten we change game state
void checkGameBeaten()
{
    if(isGameBeaten())
    {
        currentState = GameState.GAMEOVER;
    }
}

// after player collects all collectables.
// player wins or beats the game
boolean isGameBeaten()
{
    return player.score == COLLECTABLE_COUNT;
}

// clears enemy and collectable lists
// and initialize them for new game
// you can tweak with value in this funtion
// NOTE: DO NOT CALL THIS BEFORE INITIALIZING PLAYER
void resetList()
{
    objectList.clear();

    for(int i = 0; i < ENEMY_COUNT+COLLECTABLE_COUNT; i++)
    {
        while(true)
        {
            float x = random(0, width);
            float y = random(0, height);

            // if coordinates are not too close to player 
            // we add to the list and break the loop
            // otherwise pick another random location
            boolean closeToPlayer = dist(player.x, player.y, x, y) < SPAWN_OFFSET_TO_PLAYER;
            
            //we shall not spawn objects close to edge
            boolean closeToEdge = x < EDGE_OFFSET || x > width-EDGE_OFFSET || y < EDGE_OFFSET || y > height-EDGE_OFFSET;
            
            if(!closeToPlayer && !closeToEdge)
            {
                // we add certain amount of enemy and collectables
                if(i < ENEMY_COUNT)
                {
                    objectList.add(new Enemy(x, y)); // add enemy
                }
                else
                {
                    objectList.add(new Collectable(x, y)); // add collectable
                }

                break;
            }
        }
    }
}

// set default variables
void restartGame()
{
    currentState = GameState.RUNNING;
    player = new Player(width/2, height/2);
    vx = 0;
    vy = 0;
    cd = new CountDown(TIME);
    resetList();
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
                vy = -1;
            }
            else if (keyCode == DOWN) 
            {
                vy = 1;
            }
            else if (keyCode == LEFT) 
            {
                vx = -1;
            }
            else if (keyCode == RIGHT) 
            {
                vx = 1;
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
        vy = 0;
    }
    else if (keyCode == RIGHT || keyCode == LEFT)
    {
        vx = 0;
    } 
}
