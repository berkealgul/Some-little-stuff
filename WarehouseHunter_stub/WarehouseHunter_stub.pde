import queasycam.*;

QueasyCam cam;
boolean exitGenerated = false;
float texW[], texH[];
int orbScored = 0;
int numOrbs = 0;
int numDoors = 0;
int roomW = 10000;
int roomH = 3000;
int roomD = 5000;
int gridSize = 1000;
int[] pos = {0, 0, 0};
int[][] room;
PImage[] tex;
PShader shaderPhong;
PShape c;
PShape orb, door;
PVector doorPos;
PVector[][] orbs;

void setup() {
  fullScreen(P3D);
  
  //ses up the FPS camera
  cam = new QueasyCam(this);
  cam.sensitivity = 0.5;
  cam.speed = 5;
  perspective(PI/3, (float)width/height, 0.01, 10000);

  loadTextures(); //loads the textures necessary for the warehouse, orbs and door
  createAssets(); //creates the crate, orb and door shapes
  
  //STEP 1
  genRoom(); //generates the room 
  
  //STEP 4
  //WRITE CODE HERE :Load the shaders and set the coefficients for the Phong Illumination Model
 
}

void draw() {
  //shader(shaderPhong);
  background(0);
  noStroke();
  ambientLight(100, 100, 100);
  setupLights(); 
  translate(0, roomH/2, 0);
  drawRoom(10000, 3000, 5000); 
  drawCrates(); // STEP 1
  //scoreOrbs(); //STEP 6a
  //drawScore(); //STEP 6a
  //checkExit(); //STEP 6b
  
  
}

void keyPressed() {
  if (key=='f')
    drawFloorplan();
    if (key=='r')
    resetGame();
    if (key=='v')
    save("warehouse.jpg");
}

void resetGame() {
  orbScored = 0;
  numOrbs = 0;
  exitGenerated = false;
  genRoom();
  
}
