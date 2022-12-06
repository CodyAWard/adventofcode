
data = open("day03.txt", "r").read()
data = data.splitlines()

totalPriority = 0

def calculatePriority(char):
	if char.isupper():
		return ord(char) - 38 # normalize to 27 - 52
	return ord(char) - 96 # normalize to 1 - 26


for line in data:
	length = len(line)
	halfLength = int(length/2)
	a = line[0:halfLength]
	b = line[halfLength:length]

	for char in a:
		if char in b:
			priority = calculatePriority(char)
			totalPriority += priority 
			break

print("Part 1: ", totalPriority)


totalPriority = 0

i = 0
while i < len(data):

	a = data[i]
	b = data[i+1]
	c = data[i+2]
	i+=3

	for char in a:
		if char in b and char in c:
			priority = calculatePriority(char)
			totalPriority += priority 
			break

print("Part 2: ", totalPriority)