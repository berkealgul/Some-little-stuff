import pygame
import random
from time import sleep


class node:
    scale = 20
    bomb_probality = 0.1

    def __init__(self, ix, iy, has_bomb):
        self.loc = (ix, iy)
        self.rect = (ix * node.scale + offsetX, iy * node.scale + offsetY, node.scale, node.scale)
        self.rect_center = (self.rect[0] + 3, self.rect[1] + 3)

        self.neighbors = []
        self.bombCount_on_around = 0
        self.number_sprite = None

        self.has_bomb = has_bomb
        self.is_opened = False
        self.is_flagged = False

        if self.loc[0] % 2 == self.loc[1] % 2:
            self.original_color = Dark_Green
        else:
            self.original_color = Green

    def show(self):
        if self.am_i_highlighted():
            color = Highlight_Color
        else:
            color = self.original_color

        pygame.draw.rect(screen, color, self.rect)

        # display number on node
        if self.is_opened and self.number_sprite is not None:
            screen.blit(self.number_sprite, self.rect_center)

        if self.is_flagged:
            screen.blit(flag_sprite, (self.rect[0], self.rect[1]))

    def open(self):
        if self.is_opened:
            return

        if self.is_flagged:
            self.is_flagged = False

        self.is_opened = True
        self.original_color = OpenedNode_Color
        self.look_around()

        if self.bombCount_on_around == 0:
            for neighbor_node in self.neighbors:
                if not neighbor_node.is_opened:
                    neighbor_node.open()

    def look_around(self):
        for j in range(-1, 2):
            for i in range(-1, 2):
                if j == 0 and i == 0:
                    continue

                y = self.loc[1] + j
                x = self.loc[0] + i

                if y < 0 or x < 0 or x > gameBoardW - 1 or y > gameBoardH - 1:
                    continue

                neighbor = gameBoard[y][x]
                if neighbor.has_bomb:
                    self.bombCount_on_around += 1
                else:
                    self.neighbors.append(neighbor)

                # sprite is rebundant when node dosen't have any bomb on around
                if self.bombCount_on_around != 0:
                    self.number_sprite = self.create_number_sprite()

    def am_i_highlighted(self):
        if self.loc == highlighted_loc and not self.is_opened:
            return True
        else:
            return False

    def create_number_sprite(self):
        color_select = {
            1: (0, 0, 238),      # dark blue
            2: (69, 139, 0),     # dark green
            3: (255, 30, 30),    # red
            4: (72, 61, 139),    # purple - black
            5: (138, 43, 226),   # purple
            6: (255, 127, 0),    # orange
            7: (0, 0, 0),        # black
            8: (0, 191, 255)     # turquoise
        }

        return font.render(str(self.bombCount_on_around), True, color_select[self.bombCount_on_around], None)

    def erectRemove_flag(self):
        if self.is_opened:
            return

        if not self.is_flagged:
            self.is_flagged = True
        else:
            self.is_flagged = False


def create_node(i, j):
    num = random.random()

    has_bomb = False
    if num <= node.bomb_probality:
        has_bomb = True

    return node(i, j, has_bomb)


def create_board(width, height):
    board = []

    for j in range(height):
        row = []

        for i in range(width):
            created_node = create_node(i, j)
            row.append(created_node)

        board.append(row)

    return board


def get_highlighted():
    pos = pygame.mouse.get_pos()

    hx = pos[0] - offsetX
    hy = pos[1] - offsetY

    # check if mouse in out of game borders
    if hx < 0 or hy < 0 or hy > screenScale[1] - 2 * offsetY or hx > screenScale[0] - 2 * offsetX:
        return None

    hx /= node.scale
    hy /= node.scale

    return int(hx), int(hy)


def control_mouse_input():
    # if we have no node to click, we should pass this function
    if highlighted_loc is None:
        return

    # button_states = (mouse1, mouse3, mouse2) 3 booleans
    button_states = pygame.mouse.get_pressed()
    left_clicked = button_states[0]
    right_clicked = button_states[2]

    try:
        clicked_node = gameBoard[highlighted_loc[1]][highlighted_loc[0]]
    except IndexError:
        return

    if left_clicked and not clicked_node.is_flagged:
        if clicked_node.has_bomb:
            global gameLost
            gameLost = True
        else:
            clicked_node.open()
    elif right_clicked:
        clicked_node.erectRemove_flag()


def render_screen():
    screen.fill(Background_Color)

    for row in gameBoard:
        for node in row:
            node.show()


def check_for_win():
    for row in gameBoard:
        for node in row:
            if node.is_opened:
                continue
            elif node.has_bomb and node.is_flagged:
                continue
            else:
                return False

    return True


def game_loop():
    global highlighted_loc
    global gameLost
    global gameWon

    gameLost = False
    gameWon = False

    while not gameLost and not gameWon:
        highlighted_loc = get_highlighted()
        for event in pygame.event.get():
            if event.type == pygame.MOUSEBUTTONDOWN:
                control_mouse_input()

        render_screen()
        pygame.display.update()

        gameWon = check_for_win()
        clock.tick(FPS)


def after_game():
    if gameWon:
        pygame.display.set_caption("KAZANDIN")
        screen.fill((0, 255, 255))
        sleep(1)
    elif gameLost:
        pygame.display.set_caption("KAYBETTİN")
        render_screen()
        for row in gameBoard:
            for node in row:
                if node.has_bomb:
                    screen.blit(mine_sprite, (node.rect[0], node.rect[1]))
    pygame.display.update()
    sleep(1)


# initialize pygame components
pygame.init()
# Global variables
gameBoardW = 25
gameBoardH = 25
offsetX = 20
offsetY = 20
screenScale = (gameBoardW * node.scale + 2 * offsetX, gameBoardH * node.scale + 2 * offsetY)

# initialize sprites
flag_sprite = pygame.image.load("flagSprite.png")
mine_sprite = pygame.image.load("mineSprite.png")

Dark_Green = (69, 139, 0)
Green = (127, 255, 0)
Highlight_Color = (205, 200, 177)
Background_Color = (139, 26, 26)
OpenedNode_Color = (139, 131, 120)

highlighted_loc = None
font = pygame.font.Font(None, node.scale + 5)

screen = pygame.display.set_mode(screenScale)
pygame.display.set_caption("Mayın Tarlası Berke Algül")
clock = pygame.time.Clock()
FPS = 60

gameBoard = create_board(gameBoardW, gameBoardH)
gameLost = False
gameWon = False

# start game
game_loop()
after_game()

sleep(1)
