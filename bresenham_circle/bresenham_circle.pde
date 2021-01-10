void setup()
{
  size(500,500);

}

void draw()
{
  background(#4ACDD1);
  translate(width/2, height/2);
  c(0,0,100,255);
  c(0,0,80,250);
  c(0,0,60,155);
  c(0,0,40,100);
  c(0,0,20,50);
  
  stroke(#D14A4A);
  noFill();
  ellipse(0,0,200,200);
}

void c(int x, int y, int r, int val)
{

  int x1 = 0;
  int y1 = r;
   
  int d = 3 - 2*r;
  
  noStroke();
  fill(val);
   
  put(x1, y1);
   
  while(x1 <= y1)
  {
    x1++;
    
    if(d < 0)
    {
      d = d + 4*x1 + 6;
    }
    else
    {
      d = d + 4*(x1-y1) + 10;
      y1--;
    }
    
    put(x1, y1);
  }
  
   
}

void put(int x, int y)
{
   ellipse(x, y, 10,10); //1
   ellipse(y, x, 10,10); //2
   ellipse(y, -x, 10,10); //3
   ellipse(x, -y, 10,10); //4
   ellipse(-x, -y, 10,10); //5
   ellipse(-y, -x, 10,10); //6
   ellipse(-y, x, 10,10); //7
   ellipse(-x, y, 10,10); //8
   
}
