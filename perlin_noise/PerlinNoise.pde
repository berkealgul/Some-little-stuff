float fade(float t)
{
   return t * t * t * (t * (t * 6 - 15) + 10); 
}

PVector getGradient()
{
   float x = random(5);
   
   if(x == 1)
     return g1;
      
   if(x==2)
     return g2;
     
   if(x==3)
     return g3;
     
   else
     return g4;
}

PVector g1 = new PVector(1.0 , 1.0);
PVector g2 = new PVector(-1.0 , 1.0);
PVector g3 = new PVector(-1.0 , -1.0);
PVector g4 = new PVector( 1.0 , -1.0);

PVector d1 = new PVector(0, 1);
PVector d2 = new PVector(1, 1);
PVector d3 = new PVector(0, 1);
PVector d4 = new PVector(0, 0);

double PerlinNoise2d(float x, float y)
{
  x = x - (int)x;
  y = y - (int)y;
  
  PVector loc = new PVector(x, y);
  
  PVector dis1 = d1.sub(loc);
  PVector dis2 = d2.sub(loc);
  PVector dis3 = d3.sub(loc);
  PVector dis4 = d4.sub(loc);
  
  float dot1 = dis1.dot(getGradient());
  float dot2 = dis2.dot(getGradient());
  float dot3 = dis3.dot(getGradient());
  float dot4 = dis4.dot(getGradient());
  
  float ab = dot4 + x * (dot1 - dot4);  
  float cd = dot3 + y * (dot2 - dot3);
  
  float val = ab + y * (cd - ab); 
  
  return fade(val);
  
  
}
