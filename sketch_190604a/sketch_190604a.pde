void setup() {
  size(800, 600);
}

Boundary b = new Boundary(100,100,200,200);
Ray r = new Ray(50,100);
void draw() {
  background(0);
  
  //set origin to center of the canvas
  translate(width/2, height/2);
  
  r.lookAt(mouseX, mouseY);
  b.show();
  r.show();
}
