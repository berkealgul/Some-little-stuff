class Ray 
{
  PVector loc;
  PVector dir;
  
  Ray(float x,float y) 
  {
    loc = new PVector(x, y);
    dir = new PVector(0,0);
    //temporary variable must change!!
  }
  
  void show() 
  {
    stroke(255);
    push();
    translate(loc.x, loc.y);
    line(0, 0, dir.x * 10, dir.y * 10);
    pop();
  }
  
  
  void lookAt(float x, float y)
  {
    dir.x = x - loc.x;
    dir.y = y - loc.y;
    dir.normalize();
  }
  
  PVector findIntersection(Boundary b)
  {
    //prepare variables for special formula 
    final float x1 = b.v1.x;
    final float y1 = b.v1.y;
    final float x2 = b.v2.x;
    final float y2 = b.v2.y;
    
    final float x3 = loc.x;
    final float y3 = loc.y;
    final float x4 = x3 + dir.x;
    final float y4 = y3 + dir.y;
   
    //special formula calculation
    float t, u, den;
    
    den = ((x1 - x2) * (y3 - y4)) - ((y1 - y2) * (x3 - x4));
    
    if(den == 0)
      return null;
    // if den is 0, lines are parallel and check is done;
      
    t = (((x1 - x3) * (y3 - y4)) - ((y1 - y3) * (x3 - x4))) / den;
    u = -(((x1 - x2) * (y1 - y3)) - ((y1 - y2) * (x1 - x3))) / den;
    
    if(!((t < 1 && t > 0) && u > 0))
       return null;
       
     //if there is intersection lets denote common x point as Px, common y point as Py;
     float Px, Py;
     
     Px = x1 + t * (x2 - x1);
     Py = y1 + t * (y2 - y1);
     
     return new PVector(Px,Py);
  }
}
