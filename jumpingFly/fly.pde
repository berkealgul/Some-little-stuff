class Fly
{
   float max_z;
   float max_size;
   float min_size;
   int x;
   int y;
   float z;
   float timeInAir;
   float jumpOffset;
  
   Fly(int x_, int y_, float min_size_, float max_size_, float max_z_)
   {
      x = x_;
      y = y_;
      z = 0;
      min_size = min_size_;
      max_size = max_size_;
      max_z = max_z_;
      timeInAir = 0;
      jumpOffset = 0;
   }
   
   void draw()
   {
       float size = map(z, 0, max_z, min_size, max_size);
       strokeWeight(3);
       stroke(0);
       
       fill(#F5FA12);
       ellipse(x,y,size,size);
   }
  
   boolean isColliding(Hand hand, float angle_tresh)
   {
      if(z > max_z/4)
      {
         return false; 
      }
      
      float a_fly = atan2(y,x);
      
      if(a_fly < 0)
      {
         a_fly += TWO_PI; 
      }
      
      float da = abs(a_fly - hand.a);
      
      if (da > angle_tresh)
      {
         return false; 
      }
      else
      {
         return true; 
      }
   }
  
}
