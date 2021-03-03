// base class of all objects in game
class GameObject
{
    float x, y; // center x, y locations
    float SPEED;
    float RADIUS; // radius of collusion

    int scaleX, scaleY;
    
    PImage img;
    
    GameObject(float _x, float _y)
    {
        x = _x;
        y = _y;
        
        SPEED = 0;
        RADIUS = 20;
    }
    
    // move the object
    // default params are direction components
    void move(float dirX, float dirY)
    {
        x = x + (dirX * SPEED);
        y = y + (dirY * SPEED);
    }

    void draw()
    {
        imageMode(CENTER);
        image(img, x, y);
    }

    // standart collision checking
    // objects are assumed to have circular hitbox
    // returns true if we collide, false otherwise
    boolean collidesWith(GameObject go)
    {
        float d = dist(x, y, go.x, go.y); // distance between centers

        if(d < RADIUS + go.RADIUS)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
