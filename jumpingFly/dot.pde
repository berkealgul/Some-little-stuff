class Dot
{
    int x;
    int y;
    float size;
    
    Dot(int x_, int y_, float size_)
    {
       x = x_;
       y = y_;
       size = size_;
    }
  
    void draw()
    {
        stroke(0);
        strokeWeight(3);
        fill(#3594F0);
        ellipse(x,y,size,size);  
    }  
}
