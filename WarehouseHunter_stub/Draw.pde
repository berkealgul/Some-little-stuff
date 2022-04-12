int rW = floor(roomW/gridSize);
int rH = floor(roomD/gridSize);


//STEP 3a
void drawOrb() {
  pushMatrix();
  shape(orb);
  popMatrix();
}

//STEP 3b
void drawDoor() { 
  pushMatrix();
  shape(door);
  popMatrix();
}

//PROVIDED CODE

void drawCrate() {  
  shape(c);
}

void drawCrates() {
  pushMatrix();
  translate(-roomW/2+gridSize/2+2,roomH/4-gridSize/2+2,-roomD/2+gridSize/2+2);
  
    for (int i =0; i<rW; i++) 
    for (int j =0; j<rH; j++)
    {
      pushMatrix();
      translate(i*gridSize,0,j*gridSize);
      if (room[i][j] > 0)
      for (int c = 0; c< room[i][j]; c++) {         
        drawCrate();
        translate(0,-gridSize,0);
      }
     else if (room[i][j] == -1) {
      orbs[i][j] = new PVector(modelX(0,0,0),modelY(0,0,0),modelZ(0,0,0)); //store the orbs' position
      drawOrb();
      }
      else if (room[i][j] < -1) {
      doorPos = new PVector(modelX(0,0,0),modelY(0,0,0),modelZ(0,0,0)); //store the door's position
      drawDoor();            
      }
      popMatrix();
    }
  popMatrix();
}


void drawRoom(float w, float h, float d) {
  int texNum = ROOF;
  beginShape();
  texture(tex[texNum]);
  vertex(-w/2, -3*h/4, -d/2, 0, 0);
  vertex(-w/2, -3*h/4, d/2, d/texH[texNum], 0);
  vertex(w/2, -3*h/4, d/2, d/texH[texNum], w/texW[texNum]);
  vertex(w/2, -3*h/4, -d/2, 0, w/texW[texNum]);
  endShape(CLOSE);

  texNum = FLOOR;
  beginShape();
  texture(tex[texNum]);
  vertex(-w/2, h/4, -d/2, 0, 0);
  vertex(w/2, h/4, -d/2, 0, w/texW[texNum]);
  vertex(w/2, h/4, d/2, d/texH[texNum], w/texW[texNum]);
  vertex(-w/2, h/4, d/2, d/texH[texNum], 0);
  endShape(CLOSE);

  texNum = WALL;
  beginShape();
  texture(tex[texNum]);
  vertex(w/2, -3*h/4, -d/2, 0, 0);
  vertex(w/2, h/4, -d/2, 0, h/texH[texNum]);
  vertex(w/2, h/4, d/2, d/texW[texNum], h/texH[texNum]);
  vertex(w/2, -3*h/4, d/2, d/texW[texNum], 0);
  endShape(CLOSE);

  beginShape();
  texture(tex[texNum]);
  vertex(-w/2, -3*h/4, -d/2, 0, 0);
  vertex(-w/2, h/4, -d/2, 0, h/texH[texNum]);
  vertex(-w/2, h/4, d/2, d/texW[texNum], h/texH[texNum]);
  vertex(-w/2, -3*h/4, d/2, d/texW[texNum], 0);
  endShape(CLOSE);

  beginShape();
  texture(tex[texNum]);
  vertex(-w/2, -3*h/4, -d/2, 0, 0);
  vertex(-w/2, h/4, -d/2, 0, h/texW[texNum]);
  vertex(w/2, h/4, -d/2, w/texW[texNum], h/texH[texNum]);
  vertex(w/2, -3*h/4, -d/2, w/texW[texNum], 0);
  endShape(CLOSE);

  beginShape();
  texture(tex[texNum]);
  vertex(-w/2, -3*h/4, d/2, 0, 0);
  vertex(-w/2, h/4, d/2, 0, h/texW[texNum]);
  vertex(w/2, h/4, d/2, w/texW[texNum], h/texH[texNum]);
  vertex(w/2, -3*h/4, d/2, w/texW[texNum], 0);
  endShape(CLOSE);
}

void drawFloorplan() {
  println("FLOORPLAN");
  int rW = floor(roomW/gridSize);
  int rH = floor(roomD/gridSize);
  //show floorplan
  for (int i =0; i<rW; i++) {
    for (int j =0; j<rH; j++) 
      print(room[i][j]>=0?room[i][j]:"*");
    println();
  }
}