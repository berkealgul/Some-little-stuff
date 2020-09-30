int order = 5;

int N = int(pow(2, order));
int total = N*N;
PVector curve[];

void setup()
{
  size(512,512);
  curve = new PVector[total];
  
  float len = width / N;
  
  for(int i = 0; i < total; i++)
  {
    PVector v = hilbert(i);
    v.mult(len);
    v.add(len/2,len/2);
    curve[i] = v;
  }
    
}

void draw()
{
  background(0);  
  noFill();
  stroke(255);
  strokeWeight(2);
  
  beginShape();
  for(int i = 0; i < total; i++)
  {
    PVector v = curve[i];
    vertex(v.x, v.y);
  }
  endShape();
  
}

PVector hilbert(int i)
{
  PVector firstOrder[] = {
    new PVector(0,0),
    new PVector(0,1),
    new PVector(1,1),
    new PVector(1,0)
  };
  
  int idx = i & 3;
  PVector v = firstOrder[idx];
  
  for(int j = 1; j < order; j++)
  {
    float len = pow(2,j);
    i = i >>> 2;
    idx = i & 3;
    
    if(idx == 0)
    {
       float temp = v.x;
       v.x = v.y;
       v.y = temp;
    }
    else if(idx == 1)
    {
     v.y+=len; 
    }
    else if(idx == 2)
    {
     v.y+=len;
     v.x+=len;
    }
    else if(idx == 3)
    {
     float temp = len-1-v.x;
     v.x = len-1-v.y;
     v.y = temp;
     v.x+=len; 
    }
  }
  
  return v;
}
