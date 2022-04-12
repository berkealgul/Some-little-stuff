//PROVIDED CODE

final static int ROOF = 0;
final static int FLOOR = 1;
final static int WALL = 2;
final static int CRATE = 3;
final static int ORB = 4;
final static int DOOR = 5;

void loadTextures() {
  //setup texture array data structure
  tex = new PImage[6];
  texW = new float[6];
  texH = new float[6];

  //load textures
  tex[ORB] = loadImage("gold.jpg"); 
  tex[ROOF] = loadImage("roof.jpg"); 
  tex[WALL] = loadImage("walltile.jpg"); 
  tex[DOOR] = loadImage("door.png");
  tex[CRATE] = loadImage("metal.jpg");  
  tex[FLOOR] = loadImage("floortile.jpg"); 

  //initialise dimensions
  for (int t = 0; t<tex.length; t++) {
    texW[t] = tex[t].width;
    texH[t] = tex[t].height;
  }

  textureMode(NORMAL); //texture u,v coordinates are in the range 0..1
  textureWrap(REPEAT); //repeat texture rather than scale
}