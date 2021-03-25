import copy
import random
from block import Block

class Game:
    # width: width of the screen
    # you must set in outside class
    # default value is 200 (pixels)
    def __init__(self, width=200):
        # number of rows and cols of grid
        self.ROWS = 20
        self.COLS = 10

        # cell size depend on width
        # width is screen width
        # cells assumed to be square
        self.CELL_SIZE = width / self.COLS

        # top-left coordinates of displayed text on window
        self.TEXT_X = width * 0.80
        self.TEXT_Y = 10

        # rate of offset for block in each cell
        # the value must be between 0-1
        # lower value means the block will be closer
        # to cell so value 1 should
        # we draw blocks smaller than cell for better looking
        # see function self.display for further details
        self.INNER_OFFSET_RATE = .05 

        # number of required adject vertical block
        # for removing them
        self.STACK_SIZE = 4

        # offset for block from cell
        # (we assume offsets are equal since shape will be square)
        # dont change directly
        self.OFF = self.INNER_OFFSET_RATE * self.CELL_SIZE

        # block size for display. dont change directly
        self.BLOCK_SIZE = self.CELL_SIZE-2*self.OFF

        # representation of empty cell on grid
        self.EMPTY = 0

        # grid is storing our blocks in matrix like format
        # for access block in row,col -> grid[row][col]
        self.grid = self.createGrid()

        # row and col of block controlled by player
        # active block means it is moveable and controllable by player
        self.ab_i = 0 # active_block_i (row) 
        self.ab_j = 0 # active_block_j (col)
        self.spawnActiveBlock()

        # speed of the game
        self.speed = 0
        self.D_SPEED = 0.25 # speed increment after block spawn

        # players score
        # this will increment by 1 after deleting blocks
        self.score = 0
    
    # the main function of game 
    # this function combines display and update
    # it should be called outside class
    # return boolean that indicates game status
    # (gameOver -> true, false)
    # inputs:
    #   horizontalDir: dir value from outside of class
    #   it is input from player. 1 means right -1 means left
    #   zero means down default value is zero
    def step(self, horizontalDir=0):
        gameOver = self.update(horizontalDir)
        self.display()
        return gameOver

    # update game state. Handle all logical operations  
    # return boolean for indicating if we need to spawn new block
    # retuned boolean: gameOver -> if this is true game is over
    # inputs:
    #   horizontalDir: dir value from outside of class
    #   it is input from player. 1 means right -1 means left
    #   zero means down
    #   this value comes from self.step function
    def update(self, horizontalDir):
        needNewBlock = False

        # if we got horizontal movement
        # we need to make horizontal movement
        # otherwise block will fall down
        # the idea here is that we want to avoid 
        # blocks to make diagonal movements
        if horizontalDir is not 0:
            self.horizontalMove(horizontalDir)
        else:
            needNewBlock = self.verticalMove()
        
        if needNewBlock:
            # check for stacks to be deleted
            blockDeleted = self.checkForStack()

            # if we need new block lets spawn it
            gameOver = self.spawnActiveBlock()

            # if we successfully spawned block
            # we need to increment speed
            if not gameOver:
                self.speed += self.D_SPEED

            # if we deleted blocks
            # we need to increment score and make speed 0
            if blockDeleted:
                self.score += 1
                self.speed = 0
        else:
            gameOver = False
        
        return gameOver
    
    # display game state to the screen
    # this is main displaying function of the game
    # this will be called from step funtion
    def display(self):
        self.displayBoard()
        self.displayScore()

    # display score of player
    def displayScore(self):
        textAlign(CENTER, CENTER)
        t = "Score: " + str(self.score)
        textSize(15) 
        fill(0)
        text(t, self.TEXT_X, self.TEXT_Y)

    # display grid by iterating every grid on cell
    def displayBoard(self):
        background(210)
        for i in range(self.ROWS):
            for j in range(self.COLS):
                if not self.isCellEmpty(i, j):
                    self.grid[i][j].display(i, j)
                else:
                    self.displayEmptyCell(i, j)

    # spawn active cell on first row and random empty column
    # return boolean indicates wheter it spawned block or not
    # if it couldn't spawn block, it means game is over and must terminate
    def spawnActiveBlock(self):
        cantSpawn = True
        col_list = [i for i in range(0, self.COLS)]

        # set active cell coordinates
        i = 0
        while len(col_list) is not 0 and cantSpawn:
            j = random.choice(col_list)

            if self.isCellEmpty(i,j):
                # spawn block and set active block coordinates
                # and set cantSpawn False which indicates our game continues
                cantSpawn = False
                self.ab_i = i
                self.ab_j = j
                self.grid[i][j] = Block(self.CELL_SIZE, self.OFF, self.BLOCK_SIZE)
            else:
                # remove col number if that cell is filled
                col_list.remove(j)

        return cantSpawn

    # this function creates grid with size (self.ROWS, self.COLS)
    # grid is main board of all blocks
    def createGrid(self):
        grid = []
        for i in range(self.ROWS):
            row = []
            for j in range(self.COLS):
                # set grid i,j to empty 
                row.append(self.EMPTY)
            grid.append(row)
        return grid
    
    def isCellEmpty(self, i, j):
        return self.grid[i][j] == self.EMPTY

    # Display empty cell at location row i and col j
    def displayEmptyCell(self, i, j):
        #calulate x and y and add offset
        x = (j*self.CELL_SIZE) + self.OFF 
        y = (i*self.CELL_SIZE) + self.OFF
        noStroke()
        fill(180)
        rect(x,y,self.BLOCK_SIZE,self.BLOCK_SIZE)

    # descend block and vertical movement
    def verticalMove(self):
        needNewBlock = False

        # assing old active vals
        ab_i_ = self.ab_i 
        self.ab_i += 1

        # bellow surface
        if self.ab_i == self.ROWS:
            # restore vals and indicate for new block
            self.ab_i = ab_i_
            needNewBlock = True
        # vertical check if true descend block
        elif self.isCellEmpty(self.ab_i, self.ab_j):
            # assing values for better reading
            i = self.ab_i
            j = self.ab_j
            _i = ab_i_

            # put block on new place and make old one empty
            self.grid[i][j], self.grid[_i][j] = self.grid[_i][j], self.grid[i][j]
        else:
            # restore vals and indicate for new block
            self.ab_i = ab_i_
            needNewBlock = True

        return needNewBlock

    # move block left or right depend on user input
    # dir is direction of horizontal movement
    # can be 1 or -1
    # 1 -> right  -1 -> left
    def horizontalMove(self, dir):
        # assing old active vals
        ab_j_ = self.ab_j
        self.ab_j += dir

        # check if new pos exceeds horizontal boundaries
        if self.ab_j == self.COLS or self.ab_j == -1:
            # restore vals and indicate for new block
            self.ab_j = ab_j_
        # vertical check if true descend block
        elif self.isCellEmpty(self.ab_i, self.ab_j):
            # assing values for better reading
            i = self.ab_i
            j = self.ab_j
            _j = ab_j_

            # put block on new place and make old one empty
            self.grid[i][j], self.grid[i][_j] = self.grid[i][_j], self.grid[i][j]
        else:
            # restore vals and indicate for new block
            self.ab_j = ab_j_

    # checks for vertical adject color and delete them 
    # if conditions are met delete those blocks and increment score
    # assumes active block at the top
    # return boolean if we deleted blocks
    def checkForStack(self):
        # this is false by default
        blocksDeleted = False

        i = self.ab_i
        j = self.ab_j # j are same for all blocks
        refColor = self.grid[i][j].getColor() # take ref color as color of active block

        i_coordinates = [] # store i coordinates to be deleted

        for _ in range(self.STACK_SIZE):
            # if we are out of board, we must abort
            if i == self.ROWS:
                break

            color = self.grid[i][j].getColor()
            # color and ref color matches add coordinate to delete
            if color == refColor:
                i_coordinates.append(i)
            else:
                break

            # increment i for next block
            i+=1

        # we got stack_size in coordinates
        # we need to delete all of them
        # we delete all of them
        if len(i_coordinates) == self.STACK_SIZE:
            blocksDeleted = True
            for i in i_coordinates:
                self.grid[i][j] = self.EMPTY

        return blocksDeleted
