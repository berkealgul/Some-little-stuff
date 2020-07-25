class FieldDrawer
{
  void drawField(int[][] field, int rez, int w, int h)
  {
   for(int j = 0; j < h-1; j++)
    for(int i = 0; i < w-1; i++) 
    { 
      int a = field[j][i];
      int b = field[j][i+1];
      int c = field[j+1][i+1];
      int d = field[j+1][i];
      int state = getState(a, b, c, d);
      
      fill(0);
      strokeWeight(2);
      drawContours(i, j, rez, state);
    }
  }

 int getState(int a, int b, int c, int d)
 {
   return a*8 + b*4 + c*2 + d;
 }
  
 void drawContours(int i, int j, int rez, int state)
 {
   //top left vector elements
   float x = i * rez;
   float y = j * rez;
   
   /*
   vertex notation  
   ---a---
   |     |
   d     b
   |     |
   ---c---
   */
   
   PVector a = new PVector(x+rez*0.5, y        );
   PVector b = new PVector(x+rez    , y+rez*0.5);
   PVector c = new PVector(x+rez*0.5, y+rez    );
   PVector d = new PVector(x        , y+rez*0.5);

   switch(state)
   {
      case 1:
        line(c, d);
        break;
      case 2:
        line(c, b);
        break;
      case 3:
        line(d, b);
        break;
      case 4:
        line(a, b);
        break;
      case 5:
        line(a, d);
        line(b, c);
        break;
      case 6:
        line(a, c);
        break;
      case 7:
        line(d, a);
        break;
      case 8:
        line(d, a);
        break;
      case 9:
        line(a, c);
        break;
      case 10:
        line(b, a);
        line(d, c);
        break;
      case 11:
        line(a, b);
        break;
      case 12:
        line(d, b);
        break;
      case 13:
        line(b, c);
        break;
      case 14:
        line(d, c);
        break;    
   }
 }
}

void line(PVector v1, PVector v2)
{    
  line(v1.x, v1.y, v2.x, v2.y);
}
