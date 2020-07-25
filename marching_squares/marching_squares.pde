int field[][];
int rez = 20;
int map_w, map_h;
float noise_off = 0.01;

FieldDrawer drawer = new FieldDrawer();

void setup()
{
  size(640, 480);
  map_w = 1 + width/rez;
  map_h = 1 + height/rez;
  setup_field();
}

void draw()
{
  background(100);
  drawer.drawField(field, rez, map_w, map_h);
  noise_off+= 0.005;
  setup_field();
  //render_field(); 
}

void render_field()
{
  for(int j = 0; j < map_h; j++)
    for(int i = 0; i < map_w; i++) 
    {
      stroke(field[j][i]*255);
      strokeWeight(5);
      point(i*rez, j*rez);
    }
}

void setup_field()
{ 
  field = new int[map_h][map_w];
  
  for(int j = 0; j < map_h; j++)
    for(int i = 0; i < map_w; i++) 
    {
      field[j][i] = round(noise(i+noise_off, j+noise_off));          
    }
}
