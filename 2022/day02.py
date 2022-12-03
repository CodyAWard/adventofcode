
data = open("day02.txt", "r").read()
data = data.splitlines()

rock = "rock"
paper = "paper"
scissors = "scissors"

theirRock = "A"
theirPaper = "B"
theirScissors = "C"

ourRock = "X"
ourPaper = "Y"
ourScissors = "Z"


totalScore = 0

for line in data:

	theirMove = line[0]
	ourMove   = line[2]

	if theirMove == theirRock:
		theirMove = rock
	elif theirMove == theirPaper:
		theirMove = paper
	elif theirScissors == theirScissors:
		theirMove = scissors
    
	score = 0

	if ourMove == ourRock:
		ourMove = rock
		score += 1
	elif ourMove == ourPaper:
		ourMove = paper
		score += 2
	elif ourMove == ourScissors:
		ourMove = scissors
		score += 3

	draw = ourMove == theirMove
	win = not draw \
		  and \
		  (  (ourMove == rock and theirMove == scissors)\
		  or (ourMove == scissors and theirMove == paper)\
		  or (ourMove == paper and theirMove == rock)\
		  )

	if draw:
		score += 3
	elif win:
		score += 6

	totalScore += score

print("Part 1: ", totalScore)

needLoss = "X"
needDraw = "Y"
needWin  = "Z"

totalScore = 0

for line in data:

	theirMove = line[0]
	ourCondition = line[2]

	score = 0
	
	if ourCondition == needDraw:
		score += 3
		if theirMove == theirRock:
			score += 1
		elif theirMove == theirPaper:
			score += 2
		elif theirMove == theirScissors:
			score += 3
	elif ourCondition == needWin:
		score += 6
		if theirMove == theirRock:
			score += 2
		elif theirMove == theirPaper:
			score += 3
		elif theirMove == theirScissors:
			score += 1
	elif ourCondition == needLoss:
		if theirMove == theirRock:
			score += 3
		elif theirMove == theirPaper:
			score += 1
		elif theirMove == theirScissors:
			score += 2

	totalScore += score

print("Part 2: ", totalScore)