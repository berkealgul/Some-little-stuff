//STEP 1
void genRoom() {  
  //structure to store room model
  room = new int[rW][rH];

  //STEP 1a
  //generate base floor
  for(int i = 0; i < rW; i++)
    for(int j = 0; j < rH; j++)
    {
      
      if(random(1) > 0.5)
        room[i][j] = 1;
      else
        room[i][j] = 0;
    }

  //STEP 1b
  //generate upper floors
  
  // for two floors
  for(int k = 0; k < 2; k++)
    for(int i = 0; i < rW; i++)
      for(int j = 0; j < rH; j++)
      {
        if(room[i][j] < 1)
          continue;
        
        int neighbors = 0;
        // look each neighbor, we also check if index is in valid spot
        // E
        if(i+1 < rW) 
            if(room[i+1][j] > k)
              neighbors++;
        // W
        if(i-1 >= 0) 
            if(room[i-1][j] > k)
              neighbors++;
        
        // N
        if(j+1 < rH) 
            if(room[i][j+1] > k)
              neighbors++;
              
        // S
        if(j-1 >= 0) 
            if(room[i][j-1] > k)
              neighbors++;
         
         if(neighbors > 1)
           room[i][j]++;
      }
   
   
  //STEP 2a
  //generate orbs if no crates and surrounding has at least two neighbours > 1 
  genOrbs();

  //STEP 2b
  genDoor();
}

//STEP 2
void genOrbs() {

  //structure to store the position of orbs once drawn (used in drawOrbs)
  orbs = new PVector[rW][rH];

  //STEP 2a
  //Write code to look for a vacant room[i][j] location surrounded by at least three crates
  //put an orb in that location by storing a -1 in room[i][j]
  //increment the numOrbs variable
  for(int i = 0; i < rW; i++)
      for(int j = 0; j < rH; j++) {
        if(room[i][j] != 0)
          continue;
         
         int neighbors = 0;
        // check surrounding
        for(int ni = -1; ni <= 1; ni++)
          for(int nj = -1; nj <= 1; nj++) {
               if(ni == 0 && nj == 0)
                 continue; // dont check self
               else if(ni+i >= rW  || ni+i < 0 || nj+j >= rH || nj+j < 0) // check to prevent index error
                 continue;
               
               if(room[ni+i][nj+j] > 0)
                 neighbors++;
          }
        
        if(neighbors >= 3)
          room[i][j] = -1;
      }
  
}
//STEP 2
void genDoor() {
  //generate exit
  while (!exitGenerated) { 

    //There is a 25% chance for each of the N,S,E,W walls to be selected as the exit
    //For the selected wall, move along the locations touching that wall and find an empty one
    //The chosen location, that has no crates nor orb, gets assigned the door (use a negative value less than -1)  
    //If there is no vacant space along the chosen wall, roll again for the wall choice (use the while loop)
    int selectedWall = int(random(4));
    
    switch(selectedWall) {
      case 1: // N  
        for(int i = 0; i < rH; i++) {
         if(room[0][i] == 0) {
           room[0][i] = -2;
          exitGenerated = true;
          break;
         }
        }
        break;
     
      case 2: // S
        for(int i = 0; i < rH; i++) {
         if(room[rW-1][i] == 0) {
           room[rW-1][i] = -2;
          exitGenerated = true;
          break;
         }
        }
        break;
      case 3:  // W
        for(int i = 0; i < rW; i++) {
         if(room[i][0] == 0) {
           room[i][0] = -2;
          exitGenerated = true;
          break;
         }
        }
         break;
         
      case 4:
        for(int i = 0; i < rW; i++) {
         if(room[i][rH-1] == 0) {
           room[i][rH-1] = -2;
          exitGenerated = true;
          break;
         }
        }
        break;
    }
  }

//this call to drawFloorplan() outputs a textual representation of the room to console
drawFloorplan();
}


//PROVIDED CODE
void createAssets() {
  c = createShape(BOX, gridSize-5, gridSize-5, gridSize-5);
  c.setTexture(tex[CRATE]);

  orb = createShape(SPHERE, gridSize/4);
  orb.setTexture(tex[ORB]);
  orb.setStroke(0);

  door = createShape(BOX, gridSize, gridSize, 1);
  door.setTexture(tex[DOOR]);
  door.setStroke(0);
}
