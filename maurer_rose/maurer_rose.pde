float n = 6;
float d = 71;
float scale = 250;

void setup()
{
  size(600,600);
  
}


void draw()
{
  background(0);
  translate(width/2,height/2);
  stroke(255);
  strokeWeight(1);
  
  noFill();
  beginShape();
  for(int i = 0; i < 361; i++)
  {
    float k = radians(i*d);
    
    float r = sin(n*k);
    int x = int(scale*r*cos(k));
    int y = int(scale*r*sin(k));
    vertex(x, y);
  }
  endShape();
  
  //n += 0.001;
  //d += 0.001;
}
