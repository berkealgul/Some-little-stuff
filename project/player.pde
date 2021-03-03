class Player extends GameObject
{
    int score;
    
    // if player moves backwards we need to rotate depend on dirScale
    int dirScale = 1;

    Player(float _x, float _y)
    {
        super(_x, _y);

        // initialize score
        score = 0;

        // set up constants
        RADIUS = 20;
        SPEED = 2.5;

        //get particular sprite from spritesheet
        img = loadImage("images/bunny.png");
        img = img.get(0,0,img.width/15, img.height);
        img.resize(int(2.5*img.width),  int(2.5*img.height));
    }

    /*
    moves player class. dirX and dirY are direction components
    and should be in range [-1,1]
    */
    void move(float dirX, float dirY)
    {   
        // scale vector by speed
        float vx = dirX * SPEED;
        float vy = dirY * SPEED;

        // update location
        x = x + vx;
        y = y + vy;

        // if player goes out of canvas we reverse movement
        if(x < RADIUS || x > width || y < RADIUS || y > height)
        {
            x = x - vx;
            y = y - vy;
        }

        // if player moves backwards we shall rotate image
        if(vx < 0)
        {
            dirScale = -1;
        }
        else
        {
            dirScale = 1;
        }
    }

    void draw()
    {
        push();
        translate(x, y); 
        scale(dirScale, 1); //mirror image if player moves back
        imageMode(CENTER);
        image(img, 0, 0);
        pop();
    }
}
