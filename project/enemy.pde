class Enemy extends GameObject
{
    float rand_dirX, rand_dirY; // random directions
    float imgDirX, imgDirY;
    
    // enemy will move with random direction and to the player
    // this constant will determine the rate of following the player
    // this must be in range [0,1] 0.15 means its movement is 
    // %15 of seeking and %85 random 
    float SEEK_PLAYER_RATE = 0.45;

    // random direction wont change in every frame. Instead,
    // change random direction after each specfied amount of frame
    float UPDATE_RAND_DIR_FRAME = 45;

    // we have images for running animation
    ArrayList<PImage> images;

    // if player moves backwards we need to rotate depend on dirScale
    int dirScale = 1;

    Enemy(float _x, float _y)
    {
        super(_x, _y);

        SPEED = 1.5;
        RADIUS = 20;

        // load images for animation
        loadImages();
    }

    // check if we need to update random direction if needed update it
    void updateRandDir()
    {
        if(frameCount % UPDATE_RAND_DIR_FRAME == 0 || frameCount == 0)
        {
            rand_dirX = random(-1, 1);
            rand_dirY = random(-1, 1);
        }
    }

    void loadImages()
    {
        images = new ArrayList<PImage>(); 

        //load images based on known file locations
        images.add(loadImage("images/fox/foxrun1.png"));
        images.add(loadImage("images/fox/foxrun2.png"));
        images.add(loadImage("images/fox/foxrun3.png"));
        images.add(loadImage("images/fox/foxrun4.png"));
        images.add(loadImage("images/fox/foxrun5.png"));
        images.add(loadImage("images/fox/foxrun6.png"));
        images.add(loadImage("images/fox/foxrun7.png"));
        images.add(loadImage("images/fox/foxrun8.png"));

        // resize them for fitting in game
        for(PImage img : images)
        {
            img.resize(int(3*img.width),  int(3*img.height));
        }
    }

    // this will move enemy
    // px and py are player location components
    void move(float px, float py)
    {   
        // check random direction during main moving function
        updateRandDir();

        // random vx and vy
        float rand_vx = rand_dirX * SPEED;
        float rand_vy = rand_dirY * SPEED;
        
        // we calculate velocity to the player
        // in order to do this first we need to normalize vector between
        // player and enemy. Then we shall scale that vector to determine
        // the velocity
        float distToPlayer = dist(x,y,px,py);
        float player_vx = ((px-x) / distToPlayer) * SPEED;
        float player_vy = ((py-y) / distToPlayer) * SPEED;

        // we use weighted sum in order to calculate total velocity
        float vx = SEEK_PLAYER_RATE * player_vx + (1-SEEK_PLAYER_RATE) * rand_vx;
        float vy = SEEK_PLAYER_RATE * player_vy + (1-SEEK_PLAYER_RATE) * rand_vy;

        // if enemy goes out of canvas it will bounce back to the canvas
        if(x < RADIUS || x > width) 
        {
            vx = vx * -1;
            rand_dirX = rand_dirX * -1;
        }
        else if(y < 0 || y > height)
        {
            vy = vy * -1;
            rand_dirY = rand_dirY * -1;
        }   

        // update locations
        x = x + vx;
        y = y + vy;

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

    void draw()
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
