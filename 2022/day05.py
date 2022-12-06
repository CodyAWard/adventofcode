
data = open("day05.txt", "r").read()
data = data.splitlines()

# determine where instructions start
instructionIndex = 1
for line in data:
	instructionIndex += 1
	if(line.startswith(" 1 ")):
		break

# determine number of stacks total
stackCount = int(len(data[0]) / 4 + 0.5)

# returns the starting state of all the stacks
def readStartState():
	stacks = []
	for i in range(stackCount):
		stacks.append([]);

	# read inital state into each stack
	for line in data:
		if(line.startswith(" 1 ")):
			break

		offset = 0
		for i in range(stackCount):
			if line[offset] == "[":
				stacks[i].append(line[offset + 1])

			offset += 4

	for stack in stacks:
		stack = stack.reverse()

	return stacks

# Part 01

stacks = readStartState()
i = instructionIndex
while i < len(data):
	
	split 	= data[i].split(" ")
	amount 	= int(split[1])
	start 	= int(split[3]) - 1
	end 	= int(split[5]) - 1

	for x in range(amount):
		val = stacks[start].pop()
		stacks[end].append(val)

	i+=1

answer = ""
for stack in stacks:
	answer += stack[len(stack)-1]

print("Part 1: ", answer)


# Part 02

stacks = readStartState()
i = instructionIndex
while i < len(data):
	
	split 	= data[i].split(" ")
	amount 	= int(split[1])
	start 	= int(split[3]) - 1
	end 	= int(split[5]) - 1

	liftedGroup = []
	for x in range(amount):
		val = stacks[start].pop()
		liftedGroup.append(val)

	liftedGroup.reverse()
	for val in liftedGroup:
		stacks[end].append(val)

	i+=1

answer = ""
for stack in stacks:
	answer += stack[len(stack)-1]

print("Part 2: ", answer)

