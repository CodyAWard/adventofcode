
data = open("day07.txt", "r").read()
data = data.splitlines()

directories = {}
cd = None
currentDir = directories

for line in data:

	if line[0] == "$": # a command
		command = line[2:4]
		if command == "cd":
			directory = line[5:]
			if directory == "/":
				pass
			elif directory == "..":
				currentDir = currentDir["parent"]
			else: 
				currentDir = currentDir[directory]

	elif line[0:3] == "dir": # a directory
		directory = line[4:]
		currentDir[directory] = {"parent":currentDir}
	else: # a file
		size = line.split(" ")[0]
		name = line.split(" ")[1]
		currentDir[name] = int(size)

def calcSizes(arr, dir):
	size = 0
	
	for key in dir:
		if key == "parent":
			continue

		val = dir[key]
		if type(val) == dict: # sub dir
			size += calcSizes(arr, val)
		else: # a file
			size += val

	arr.append(size)
	return size

sizes = []
calcSizes(sizes, directories)

# Part 01
totalBelowThreshold = 0
for size in sizes:
	if size > 100000:
		continue

	totalBelowThreshold += size 

print("Part 01: ", totalBelowThreshold)

# Part 02

used = sizes[len(sizes)-1]
unused = 70000000 - used
needed = 30000000 - unused
dirToDel = used 

for size in sizes:
	if size > dirToDel:
		continue
	if size <= needed:
		continue

	dirToDel = size 

print("Part 02: ", dirToDel)