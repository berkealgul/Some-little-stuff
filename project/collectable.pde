class Collectable extends GameObject
{
    
    Collectable(float _x, float _y)
    {
        super(_x, _y);
        
        RADIUS = 15;

        // load image for collectable and resize it
        img = loadImage("images/carrot.png");
        img.resize(int(2*RADIUS),  int(2*RADIUS));
    }
}
