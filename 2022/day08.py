
data = open("day08.txt", "r").read()
data = data.splitlines()

grid = []
for line in data:
	row = []
	for char in line:
		row.append(int(char))
	grid.append(row)

def isVisible(x, y, grid):
	rowLen = len(grid[0])-1
	colLen = len(grid)-1

	if x == 0 or x == rowLen:
		return True
	if y == 0 or y == colLen:
		return True 

	value = grid[y][x]

	hiddenCount = 0;

	# left
	xx = x
	while xx > 0:
		xx-=1
		if grid[y][xx] >= value:
			hiddenCount+=1
			break
	# right
	xx = x
	while xx < rowLen:
		xx+=1
		if grid[y][xx] >= value:
			hiddenCount+=1
			break
	# up
	yy = y
	while yy > 0:
		yy-=1
		if grid[yy][x] >= value:
			hiddenCount+=1
			break
	# down
	yy = y
	while yy < colLen:
		yy+=1
		if grid[yy][x] >= value:
			hiddenCount+=1
			break

	return hiddenCount < 4 # hidden in all 4 directions?

rowLen = len(grid[0])
colLen = len(grid)

totVisible = 0
for y in range(colLen):
	for x in range(rowLen):
		if isVisible(x, y, grid):
			totVisible += 1

print("Part 01: ", totVisible)


def scenicScore(x, y, grid):
	rowLen = len(grid[0])-1
	colLen = len(grid)-1

	
	value = grid[y][x]

	scoreLeft = 0;
	scoreRight = 0;
	scoreUp = 0;
	scoreDown = 0;

	# left
	xx = x
	while xx > 0:
		xx-=1
		scoreLeft += 1
		if grid[y][xx] >= value:
			break
	# right
	xx = x
	while xx < rowLen:
		xx+=1
		scoreRight += 1
		if grid[y][xx] >= value:
			break
	# up
	yy = y
	while yy > 0:
		yy-=1
		scoreUp += 1
		if grid[yy][x] >= value:
			break
	# down
	yy = y
	while yy < colLen:
		yy+=1
		scoreDown += 1
		if grid[yy][x] >= value:
			break

	return scoreLeft * scoreRight * scoreUp * scoreDown

highestScore = 0
for y in range(colLen):
	for x in range(rowLen):
		score = scenicScore(x, y, grid)
		if highestScore < score:
			highestScore = score

print("Part 02: ", highestScore)
