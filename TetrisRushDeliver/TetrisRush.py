from game import Game

# you can give width of screen
# default value is 200
game = Game()

#variable that holds game state. 
# can be updated by Game class and restartGame function
gameOver = False

# horizontal movement value determined by player
# see game.step function for further details
horizontalDir = 0

# this determines if key is pressed
# we need to prevent key holding behaviour of players
keyHold = False

def setup():
    size(200, 400)
    
def draw():
    global gameOver, horizontalDir

    if gameOver:
        displayGameOver()
    else:
        # slow down the game by not displaying every frame
        if frameCount % (max(1, int(8 - game.speed))) == 0 or frameCount==1:
            # display current game state
            # and update game state 
            # returns game state
            gameOver = game.step(horizontalDir)

            # block only move once in horizontal directions
            # without falling down
            if horizontalDir is not 0:
                horizontalDir = 0

# Display game over screen
def displayGameOver():
    background(0)
    fill(255, 51, 52) # red color taken from assingment sheet
    textSize(15) 
    textAlign(CENTER, CENTER)

    x = (width / 2)
    text("GAME OVER", x, height * 0.3)
    text("Score: "+str(game.score), x, height * 0.4)
    text("press mouse to restart", x, height * 0.6)

# restart game variables to default
def restartGame():
    global game, gameOver, horizontalDir, keyHold
    gameOver = False
    game = Game(width=width)
    horizontalDir = 0
    keyHold = False

def mouseClicked():
    if gameOver:
        restartGame()

def keyPressed():
    global horizontalDir, keyHold

    # if key is pressed without released
    # we need to return
    if keyHold:
        return
    else:
        keyHold = True # key is pressed and holding now

    if key == CODED:
        if keyCode == LEFT:
            horizontalDir = -1
        elif keyCode == RIGHT:
            horizontalDir = 1

def keyReleased():
    global horizontalDir, keyHold
    horizontalDir = 0
    keyHold = False # we can press again
