class Boundary 
{  
  PVector v1, v2; 
  
  Boundary(float x1, float y1, float x2, float y2) 
  {
    v1 = new PVector(x1, y1);
    v2 = new PVector(x2, y2);
  }
 
  void show()
  {
    stroke(255);
    line(v1.x, v1.y, v2.x, v2.y);
  }
}
