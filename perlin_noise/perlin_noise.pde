float scl =  0.15;
int tileSize = 20;

void setup()
{
 size(1080, 720);
}

void draw()
{
  for(int i = 0, w= width/tileSize; i < w; i++)
  {
    for(int j = 0, h= height/tileSize; j < h; j++)
    {
      fill(getColor(noise(i*scl, j*scl)));
      rect(i*tileSize, j*tileSize, tileSize, tileSize);
    } 
  }
}
 

color getColor(double x)
{ 
  if(x > 0.7) //snow peak
    return #d4d6d9;
  
  if(x > 0.6) //mountain
    return #5d5f61; 
   
  if(x > 0.5) //grass
    return #52c40a; 
    
  if(x > 0.3) //sand
    return #F5E902;
    
  else //water
    return #24CFFC;
}
