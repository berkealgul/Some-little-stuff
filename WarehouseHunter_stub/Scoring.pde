//STEP 6a
void scoreOrbs() {
  PVector pos = cam.position;
//Loop through the room looking for orbs
//if their position (use the orbs array) is less than a gridsize away from the camera position (pos) then
// - increment the value of orbScored
// - clear the orb from the room location (set value to 0)
// - clear the orb location from the orbs 2D array (set value to null)
 
  //STEP 6
  // Remove the call to genDoor() in genRoom() in the tab RoomGen
  // call genDoor() if the value of orbScored is equal to the value of numOrbs 
  // and if the door has not been generated yet (exitGenerated is false)

}

//STEP 6
void drawScore() {
  resetShader();
  camera();
  hint(DISABLE_DEPTH_TEST);
  noLights();
  fill(0);
  rect(0, 0, width, 120);
  translate(30, 10);

  for (int i = 0; i < numOrbs; i++) {
    pushMatrix();
    translate (i*50, 50, 0);
    stroke(255, 255, 0);
    if (i < orbScored) fill(255, 255, 0); 
    else fill(0, 0, 0);    
    ellipse(0, 0, 40, 40);
    popMatrix();
  }
  pushMatrix();
  translate(width-180+40, 0);
  //STEP 6b Write code here to rotate the door icon while looking for the exit
  image(tex[DOOR], -40, 0, 80, 102);
  popMatrix();
  fill(255, 255, 0);
  textMode(SHAPE);
  textSize(72);
  text(numDoors, width-300, 80);
  hint(ENABLE_DEPTH_TEST);
}

//PROVIDED CODE
//Code used in STEP 6
void checkExit() {
  if (doorPos!= null && exitGenerated && cam.position.dist(doorPos) < gridSize) {
      numDoors++;
      resetGame();
    }
}