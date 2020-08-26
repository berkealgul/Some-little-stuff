float total = 0.0;
float hit   = 0.0;
float r;

void setup()
{
  size(500,500);
  background(0);
  fill(255);
  ellipse(width/2, height/2, width, height);
  r = width/2;
}

void draw()
{
  int x = (int)random(0, width);
  int y = (int)random(0, height);
  float d = dist(x, y, r, r);
  
  total++;
  
  if(d < r)
    hit++;
   
  float pi = (hit / total) * 4;
  println(total);
  println(pi);
   
  stroke(255,0,0);
  strokeWeight(3);
  point(x,y); 
}
