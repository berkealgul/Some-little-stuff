import random

class Block:
    # color dictionary
    # colors are determined by assingment sheet
    colors = {
        "Red": (255, 51, 52),
        "Blue": (12, 150, 228),
        "Green": (30, 183, 66),
        "Yellow": (246, 187, 0),
        "Purple": (76, 0, 153),
        "White": (255, 255, 255),
        "Black": (0, 0, 0)
    }
    
    def __init__(self, cell_size, off, size):
        # set the color of the block
        self.color = self.pickColor()

        # passed rendering parameters from Game class
        self.cell_size = cell_size
        self.off = off
        self.size = size
    
    # Display block to row i and col j
    def display(self, i, j):
        # extract rgb from color
        r = self.color[0]
        g = self.color[1]
        b = self.color[2]

        #calulate x and y and add offset
        x = (j*self.cell_size) + self.off 
        y = (i*self.cell_size) + self.off

        fill(r,g,b)
        rect(x,y,self.size,self.size)
    
    # pick random color for block
    def pickColor(self):
        return random.choice(list(Block.colors.values()))

    # return color
    def getColor(self):
        return self.color
