import random
import time
import pygame as py

py.init()  

# colors
background   = [89, 87, 85]
line         = [0, 0, 0]
open_cell    = [255, 255, 255]
close_cell   = [237, 125, 12]
hovered_cell = [250, 217, 2]
text_box     = [255, 255, 255]
true_cell    = [11, 252, 3]
wrong_cell   = [252, 3, 3]

# global variables
done = False
offSetW = 250
offSetH = 1
scl = 80
rows = 7
cols = 12
font = py.font.Font('freesansbold.ttf', 15)
res_font = py.font.Font('freesansbold.ttf', 17)
screen = py.display.set_mode((scl*cols+offSetW, scl*rows+offSetH))
mouse_pos = (0, 0)
cell_grid = []
opened_cells = []
pair_count = 42
wait = False
correction = False
wrong = False
clock = py.time.Clock()
FPS = 30

class Text:
	def __init__(self, data, id):
		self.data = data
		self.id = id		

class Cell:
	def __init__(self, text):
		self.is_opened = False
		self.is_hovered = False
		self.text = text

	def pickColor(self):
		if self.is_opened:
			return open_cell

		if self.is_hovered:
			self.is_hovered = False
			return hovered_cell
		
		return close_cell

	def render(self, i, j):
		color = self.pickColor()
		rect = py.Rect(i*scl, j*scl, scl, scl)
		py.draw.rect(screen, color, rect)

		if self.is_opened:
			py.draw.circle(screen, line, rect.center, 10)

		"""
		id = str(self.getId())
		text = font.render(id, True, [0,0,0], [255,255,255])
		text_rect = text.get_rect()
		text_rect.center = rect.center
		screen.blit(text, text_rect)
		"""

	def getId(self):
		return self.text.id

	def getText(self):
		return self.text.data

def drawText(text, rect, color):
	rect = py.Rect(rect)
	y = rect.top
	lineSpacing = -2

	# get the height of the font
	fontHeight = font.size("Tg")[1]

	while text:
		i = 1

		# determine if the row of text will be outside our area
		if y + fontHeight > rect.bottom:
			break

		# determine maximum width of line
		while font.size(text[:i])[0] < rect.width and i < len(text):
			i += 1

		# if we've wrapped the text, then adjust the wrap to the last word      
		if i < len(text): 
			i = text.rfind(" ", 0, i) + 1

		# render the line and blit it to the surface
		image = font.render(text[:i], 1, line, color)
		image.set_colorkey(color)

		screen.blit(image, (rect.left, y))
		y += fontHeight + lineSpacing

		# remove the text we just blitted
		text = text[i:]

	return text

def createTextList():
	tl = []

	# pair 1
	tl.append(Text("Edad del 60% de las personas con IMSEST", 0))
	tl.append(Text(">65 años", 0))
	# pair 2
	tl.append(Text("Factores de riesgo clave", 1))
	tl.append(Text("Mayor edad, enfermedad cardiovascular, tabaquismo, dislipidemia, diabetes y obesidad", 1))
	# pair 3
	tl.append(Text("Las adipocinas se relacionan con procesos de", 2))
	tl.append(Text("Inflamación y aterosclerosis", 2))
	# pair 4
	tl.append(Text("Reduce suministro de oxígeno en sangre, aumenta la trombogénesis y produce espasmos de la coronaria", 3))
	tl.append(Text("Tabaquismo", 3))
	# pair 5
	tl.append(Text("Se caracteriza por la ausencia de daño miocárdico", 4))
	tl.append(Text("Angina inestable", 4))
	# pair 6
	tl.append(Text("Síndrome por oclusión casi completa de coronaria", 5))
	tl.append(Text("SCASEST", 5))
	# pair 7
	tl.append(Text("Se caracteriza por la ausencia de daño miocárdico", 6))
	tl.append(Text("Angina inestable", 6))
	# pair 8
	tl.append(Text("Hallazgos en ECG de SCASEST", 7))
	tl.append(Text("Normal, depresión ST, elevación transitoria ST, inversión onda T", 7))
	# pair 9
	tl.append(Text("Manifestación más temprana de SCACEST en ECG", 8))
	tl.append(Text("Onda T alta acuminada", 8))
	# pair 10
	tl.append(Text("Tiempo para obtener ECG tras contacto médico", 9))
	tl.append(Text("<10 minutos", 9))
	# pair 11
	tl.append(Text("Hallazgos necesarios para Dx de SCACEST", 10))
	tl.append(Text("ST > 1 mm en 2 derivaciones contiguas", 10))
	# pair 12
	tl.append(Text("Hallazgos en ECG de SCACEST", 11))
	tl.append(Text("Depresión del segmento ST y/u ondas T simétricamente invertidas", 11))
	# pair 13
	tl.append(Text("Hallazgos en ECG de angina inestable", 12))
	tl.append(Text("Depresión ST, inversión T o normal", 12))
	# pair 14
	tl.append(Text("Tiempos de medición de troponinas ", 13))
	tl.append(Text("Al ingreso, a 3 y a 6 horas", 13))
	# pair 15
	tl.append(Text("Estudio indicado para descartar ICC y causas no cardiacas de dolor torácico", 14))
	tl.append(Text("Radiografía de torax", 14))
	# pair 16
	tl.append(Text("Escalas para estratificar el riesgo", 15))
	tl.append(Text("TIMI y GRACE", 15))
	# pair 17
	tl.append(Text("Escala para estratificar el riesgo según evidencia de insuficiencia ventricular izquierda", 16))
	tl.append(Text("Clasificación de Killip", 16))
	# pair 18
	tl.append(Text("Clase I de Killip", 17))
	tl.append(Text("Infarto no complicado", 17))
	# pair 19
	tl.append(Text("Clase II de Killip", 18))
	tl.append(Text("IC moderada", 18))
	# pair 20
	tl.append(Text("Clase III de Killip", 19))
	tl.append(Text("IC grave con edema agudo de pulmón", 19))
	# pair 21
	tl.append(Text("Clase IV de Killip", 20))
	tl.append(Text("Shock cardiogénico", 20))
	# pair 22
	tl.append(Text("Mejor marcador de pronóstico a corto tiempo", 21))
	tl.append(Text("Troponinas", 21))
	# pair 23
	tl.append(Text("Criterios de diagnóstico según la OMS:", 22))
	tl.append(Text("Dolor torácido, marcadores de necrosis y ondas Q patológicas", 22))
	# pair 24
	tl.append(Text("Meta de HbA1c en el seguimiento", 23))
	tl.append(Text("< 7%", 23))
	# pair 25
	tl.append(Text("Elevación ST en V1-V6, DI y aVL sugiere:", 24))
	tl.append(Text("Infarto anterior extenso", 24))
	# pair 26
	tl.append(Text("Elevación ST en DII, DIII y avF sugiere:", 25))
	tl.append(Text("Infarto inferior", 25))
	# pair 27
	tl.append(Text("Contraindicación de nitratos", 26))
	tl.append(Text("PAS <90 mmHg o infarto ventricular derecho", 26))
	# pair 28
	tl.append(Text("Indicación para implementar O2", 27))
	tl.append(Text("SatO2 < 90% o insuficiencia respiratoria", 27))
	# pair 29
	tl.append(Text("Dosis de aspirina", 28))
	tl.append(Text("Carga de 150-300 mg y mantenimiento con 75-100 mg/día", 28))
	# pair 30
	tl.append(Text("Fármaco para control de síntomas en pacientes con contraindicación de betabloqueadores", 29))
	tl.append(Text("Calcio-antagonistas no dihidropiridínicos", 29))
	# pair 31
	tl.append(Text("En pacientes con SASEST se recomienda administrar", 30))
	tl.append(Text("Enoxaparina o heparina no fraccionada", 30))
	# pair 32
	tl.append(Text("Tratamiento no recomendado en SCASEST", 31))
	tl.append(Text("Tratamiento fibrinolítico", 31))
	# pair 33
	tl.append(Text("Ventajas de ICP primaria sobre fibrinólisis", 32))
	tl.append(Text("Menor mortalidad, reinfarto y hemorragia intracraneal", 32))
	# pair 34
	tl.append(Text("Se presenta insuficiencia cardiaca cuando el infarto afecta:", 33))
	tl.append(Text("25% del ventrículo izquierdo", 33))
	# pair 35
	tl.append(Text("Signos de disfunción ventricular izquierda", 34))
	tl.append(Text("Disnea, taquicardia sinusal, tercer ruido y estertores basales", 34))
	# pair 36
	tl.append(Text("Prueba requerida para indicar ejercicio en el manejo posterior ", 35))
	tl.append(Text("Prueba de esfuerzo", 35))
	# pair 37
	tl.append(Text("Objetivo de LDL-C tras infarto", 36))
	tl.append(Text("< 100 mg/dL", 36))
	# pair 38
	tl.append(Text("Meta de presión arterlal", 37))
	tl.append(Text("<130/85 y <130/80 en diabéticos o con ERC", 37))
	# pair 39
	tl.append(Text("Periodo de vigilancia del perfil de lípidos", 38))
	tl.append(Text("Cada 3 meses durante un año", 38))
	# pair 40
	tl.append(Text("Métodos de reperfusión coronaria", 39))
	tl.append(Text("Angioplastia o stent, o terapia fibrinolítica", 39))
	# pair 41
	tl.append(Text("Complicación más frecuente en infartos de cara inferior", 40))
	tl.append(Text("Bradicardia sinusal", 40))
	# pair 42
	tl.append(Text("Es la segunda causa de dolor precordial post IAM", 41))
	tl.append(Text("Pericarditis", 41))

	return tl

def createRestartBox():
	w = screen.get_width() - offSetW
	w = screen.get_width()
	rect = py.Rect(0,0, 250,50)
	rect.center = (w+offSetW/2, 300)
	return rect

def applyHovering():
	hi, hj = getHoverPosition()
	try:
		cell_grid[hj][hi].is_hovered = True
		return cell_grid[hj][hi]
	except:
		return

def getHoverPosition():
	x = mouse_pos[0]
	y = mouse_pos[1]
	i = int(x/scl)
	j = int(y/scl)
	return i, j

def setCellGrid():
	text_list = createTextList()

	for j in range(rows):
		cell_row = []
		for i in range(cols):
			idx = random.randint(0, len(text_list)-1)
			text = text_list[idx]
			cell_row.append(Cell(text))
			text_list.pop(idx)
		cell_grid.append(cell_row)

def handleEvents():
	global mouse_pos
	mouse_pos = py.mouse.get_pos()

	for event in py.event.get():
		if event.type == py.QUIT:
			global done
			done = True

		if event.type == py.MOUSEBUTTONUP:
			hi, hj = getHoverPosition()
			try:
				cell = cell_grid[hj][hi]
				if not cell.is_opened:
					cell.is_opened = True
					opened_cells.append(cell)
			except:
				continue

def render():
	# background
	global correction, wrong

	if correction:
		color = true_cell
		correction = False
	elif wrong:
		color = wrong_cell
		wrong = False
	else:
		color = background
		
	screen.fill(color)

	w = screen.get_width() - offSetW
	h = screen.get_height() - offSetH
	hovered_cell = applyHovering()

	# cells
	for i in range(cols):
		for j in range(rows):
			p = cell_grid[j][i]
			p.render(i, j)

	# horizontal lines
	for i in range(rows+1):
		py.draw.line(screen, line, (0, i*scl), (w, i*scl), 2)   

	# vertical lines
	for i in range(cols+1):
		py.draw.line(screen, line, (i*scl, 0), (i*scl, h), 2)

	# text box and its text
	strokeWeight = 5
	rect = py.Rect(0, 0, 200, 250)
	rect.center = (w+offSetW/2, 200)
	rectStroke = py.Rect(0, 0, rect.width+strokeWeight, rect.height+strokeWeight)
	rectStroke.center = rect.center
	py.draw.rect(screen, line, rectStroke)
	py.draw.rect(screen, close_cell, rect)

	# howered cell 
	if hovered_cell is not None:
		if hovered_cell.is_opened:
			drawText(hovered_cell.getText(), rect, close_cell)

	py.display.flip()

def update():
	if len(opened_cells) == 2:
		id1 = opened_cells[0].getId()
		id2 = opened_cells[1].getId()
		correction = False

		if id1 == id2:
			# correction
			global pair_count, correction
			pair_count -= 1
			correction = True
		else:
			global wrong
			wrong = True
			opened_cells[0].is_opened = False
			opened_cells[1].is_opened = False

		global wait
		wait = True
		opened_cells.clear()

	if pair_count == 0:
		global done
		done = True

def main(): 
	py.display.set_caption('Memory Game') 
	setCellGrid()

	while not done:
		handleEvents()
		update()
		render()

		global wait
		if wait:
			time.sleep(0.5)
			wait = False

		clock.tick(FPS)
		
if __name__ == "__main__":
	main()