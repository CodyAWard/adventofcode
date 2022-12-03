
data = open("day01.txt", "r").read()
data = data.splitlines()

highestCals01 = 0
highestCals02 = 0
highestCals03 = 0

currentCals = 0

for line in data:
	
	if line == '':
		
		if currentCals > highestCals01:
			highestCals03 = highestCals02
			highestCals02 = highestCals01
			highestCals01 = currentCals
		elif currentCals > highestCals02:
			highestCals03 = highestCals02
			highestCals02 = currentCals
		elif currentCals > highestCals03:
			highestCals03 = currentCals

		currentCals = 0
	
	else:

		currentCals += int(line)

print("Part 01: ", highestCals01)
print("Part 02: ", (highestCals01 + highestCals02 + highestCals03))